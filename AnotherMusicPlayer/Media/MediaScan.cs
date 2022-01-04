using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Windows.Input;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary> list of newly scanned files(absolute path) </summary>
        List<string> MediatequeScanedFiles = new List<string>();

        /// <summary> Asynchronus call for MediatequeScanning Library in a new thread </summary>
        private async void MediatequeInvokeScan(bool DoClean = false, bool boot = false)
        {
            Debug.WriteLine("MediatequeInvokeScan(DoClean="+((DoClean)?"true":"false")+ ", boot=" + ((boot) ? "true" : "false") + ")");
            //boot = false;
            DirectoryInfo di = new DirectoryInfo(Settings.LibFolder); 
            Dictionary<string, Dictionary<string, object>> DatabaseFiles = bdd.DatabaseQuery("SELECT * FROM files ORDER BY Path ASC", "Path");

            if (MediatequeWatcher == null) { MediatequeCreateWatcher(); }
            else if (MediatequeWatcher.Path != Settings.LibFolder) { MediatequeCreateWatcher(); }

            if (boot == true & DatabaseFiles.Count > 1)
            {
                MediatequeScanedFiles.Clear();
                MediatequeLoadFiles(Settings.LibFolder, MediatequeRefFolder);
                MediatequeLoadSubDirectories(Settings.LibFolder, MediatequeRefFolder);

                _ = Dispatcher.InvokeAsync(new Action(() => {
                    MediatequeScanning = false;
                    string[] a = Directory.GetFiles(Settings.LibFolder, "*.*");
                    string[] b = Directory.GetDirectories(Settings.LibFolder, "*");
                    List<string> openItems = new List<string>();

                    foreach (string arrItem in a)
                    {
                        openItems.Add(arrItem.Replace(Settings.LibFolder + SeparatorChar, ""));
                    }
                    Folder fold = new Folder() { Name = "Home", Path = Settings.LibFolder, Parent = null };
                    MediatequeLoadFiles(Settings.LibFolder, fold);
                    MediatequeLoadSubDirectories(Settings.LibFolder, fold);
                    _ = Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MediatequeBuildNavigationPath(fold.Folders[0]);

                        MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
                        args.RoutedEvent = TextBlock.MouseDownEvent;
                        args.Source = LibNavigationPathContener.Children[1];
                        LibNavigationPathContener.Children[1].RaiseEvent(args);

                        MouseButtonEventArgs args2 = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
                        args2.RoutedEvent = TextBlock.MouseDownEvent;
                        args2.Source = LibNavigationPathContener.Children[0];
                        LibNavigationPathContener.Children[0].RaiseEvent(args2);

                        TabControler.SelectedIndex = 0;

                        Dictionary<string, Dictionary<string, object>> DatabaseFiles = bdd.DatabaseQuery("SELECT * FROM files WHERE Size = 0 ORDER BY Path ASC", "Path");

                        if (DatabaseFiles.Count > 0)
                        {
                            Thread objThread = new Thread(new ParameterizedThreadStart(MediatequeScanTags));
                            objThread.IsBackground = true;
                            objThread.Priority = ThreadPriority.Normal;
                            objThread.Start(DatabaseFiles.Keys.ToList().ToArray());
                        }
                        else
                        {
                            MediatequeEnableFilters();
                        }
                    }));

                    //MediatequeBuildNavigationContent(MediatequeRefFolder);
                    dialog1.IsOpen = false;
                }));
            }
            else
            {
                try
                {
                    setLoadingState(true, "Library scan");
                    Thread objThread = new Thread(new ParameterizedThreadStart(MediatequeScan));
                    objThread.IsBackground = true;
                    objThread.Priority = ThreadPriority.Normal;
                    objThread.Start(DoClean);
                }
                catch { }
            }
        }

        private void MediatequeEnableFilters() {
            bdd.DatabaseQuerys(new string[] { "UPDATE files SET Genres = NULL WHERE TRIM(Genres) = ''" }, true);
            Dictionary<string, Dictionary<string, object>> genres = bdd.DatabaseQuery("SELECT Genres FROM files GROUP BY Genres ORDER BY TRIM(Genres) ASC", "Genres");
            List<ComboBoxItem> li = new List<ComboBoxItem>();
            //li.Add(new ComboBoxItem() { Content = "", Tag = "" });
            Debug.WriteLine(JsonConvert.SerializeObject(genres.Keys));
            foreach (string key in genres.Keys)
            {
                li.Add(new ComboBoxItem()
                {
                    Content = (key == "0")?"------":key,
                    Tag = (key == "0") ? null : key,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
            }
            _ = Dispatcher.BeginInvoke(new Action(() =>
            {
                if (LibraryFiltersGenreList.ItemsSource != null) { ((List<ComboBoxItem>)LibraryFiltersGenreList.ItemsSource).Clear(); }
                LibraryFiltersGenreList.ItemsSource = li;
                LibraryFiltersGrid.IsEnabled = true;
            }));
        }

        /// <summary> Launch a scan of the files for tag retrieval </summary>
        private async void MediatequeScanTags(object param = null)
        {
            Debug.WriteLine("--> MediatequeScanTags <--");
            string[] files = (string[])param;
            setMetadataScanningState(true, files.Length);
            _ = bdd.DatabaseFilesInfo(files, this);
            MediatequeEnableFilters();
            setMetadataScanningState(false);
            Debug.WriteLine("--> MediatequeScanTags END <--");
        }

        /// <summary> Launch a scan of th Library </summary>
        private async void MediatequeScan(object param = null)
        {
            Debug.WriteLine("--> MediatequeScan <--");
            bool DoClean = false;
            try { DoClean = (bool)param; } catch { }
            if (Settings.LibFolder == null) { return; }
            if (!System.IO.Directory.Exists(Settings.LibFolder)) { return; }

            MediatequeScanedFiles.Clear();

            MediatequeLoadFiles(Settings.LibFolder, MediatequeRefFolder);
            MediatequeLoadSubDirectories(Settings.LibFolder, MediatequeRefFolder);

            MediatequeScanedFiles = MediatequeScanedFiles.Distinct().ToList();
            //Debug.WriteLine("--> MediatequeScan SUB 0 <--");
            //Debug.WriteLine(MediatequeScanedFiles.Count);

            List<string> MediatequeCacheQuerys = new List<string>();
            List<string> NewFiles = new List<string>();

            MediatequeScanning = true;

            //Debug.WriteLine("--> MediatequeScan SUB 1 <--");
            _ = Dispatcher.BeginInvoke(new Action(() => {
                LibraryFiltersGrid.IsEnabled = false;
                LibNavigationContent.Children.Clear();
                MediatequeBuildNavigationScan();

                if (DoClean)
                {
                    MediatequeCurrentFolder = null; 
                    MediatequeWatcher = null;
                    MediatequeTotalScanedSize = 0;
                    MediatequeTotalScanedDuration = 0;
                    MediatequeTotalScanedFiles = 0;
                    //DatabaseQuery("DELETE FROM files");
                    MediatequeScanedFiles = new List<string>();
                    LibraryFiltersMode.SelectedIndex = 0;
                    LibraryFiltersGenreList.SelectedIndex = 0;
                    try { LibraryFiltersGenreList.Items.Clear(); }
                    catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
                    LibraryFiltersSearchBox.Text = "";

                    bdd.DatabaseQuerys(new string[] { "DELETE FROM covers", "DELETE FROM folders", "DELETE FROM files", "DELETE FROM playlists", "DELETE FROM playlistsItems" });
                }
            }));
            //Debug.WriteLine("--> MediatequeScan SUB 2 <--");


            Dictionary<string, Dictionary<string, object>> DatabaseFiles = bdd.DatabaseQuery("SELECT * FROM files ORDER BY Path ASC", "Path");
            //Debug.WriteLine(JsonConvert.SerializeObject(DatabaseFiles));

            //LibTreeView.Items.Clear();

            //Debug.WriteLine("--> MediatequeScan SUB 3 <--");
            //Debug.WriteLine(MediatequeScanedFiles.Count);
            foreach (string file in MediatequeScanedFiles)
            {
                MediatequeTotalScanedFiles += 1;
                FileInfo fi = new FileInfo(file);

                if (DatabaseFiles.ContainsKey(file))
                {
                    //Debug.WriteLine("--> MediatequeScan SUB 4-1 <--");
                    Dictionary<string, object> dfi = bdd.DatabaseFileInfo(fi.FullName);
                    //Debug.WriteLine(fi.FullName);
                    //Debug.WriteLine("LastUpdate Database = " + (long)DatabaseFiles[fi.FullName]["LastUpdate"]);
                    //Debug.WriteLine("LastUpdate File = " + fi.LastWriteTimeUtc.ToFileTime());

                    if(dfi != null)
                    if (Convert.ToInt64((string)dfi["LastUpdate"]) != Convert.ToInt64(fi.LastWriteTimeUtc.ToFileTime()))
                    {
                        //Debug.WriteLine("--> update <--");
                        MediatequeTotalScanedDuration += Int32.Parse((string)dfi["Duration"]);
                        MediatequeTotalScanedSize += Int32.Parse((string)dfi["Size"]);
                        MediatequeCacheQuerys.Add("UPDATE files SET Name='" + Database.DatabaseEscapeString((string)dfi["Name"])
                            + "', Album='" + Database.DatabaseEscapeString((string)dfi["Album"])
                            + "', Performers='" + Database.DatabaseEscapeString((string)dfi["Performers"])
                            + "', Composers='" + Database.DatabaseEscapeString((string)dfi["Composers"])
                            + "', Genres='" + Database.DatabaseEscapeString((string)dfi["Genres"])
                            + "', Copyright='" + Database.DatabaseEscapeString((string)dfi["Copyright"])
                            + "', AlbumArtists='" + Database.DatabaseEscapeString((string)dfi["AlbumArtists"])
                            + "', Lyrics='" + Database.DatabaseEscapeString((string)dfi["Lyrics"])
                            + "', Duration='" + (string)dfi["Duration"]
                            + "', Size='" + (string)dfi["Size"]
                            + "', Disc='" + (string)dfi["Disc"]
                            + "', DiscCount='" + (string)dfi["DiscCount"]
                            + "', Track='" + (string)dfi["Track"]
                            + "', TrackCount='" + (string)dfi["TrackCount"]
                            + "', Year='" + (string)dfi["Year"]
                            + "', LastUpdate='" + fi.LastWriteTimeUtc.ToFileTime()
                            + "' WHERE Path='" + Database.DatabaseEscapeString(fi.FullName) + "'");
                    }
                    else
                    {
                        MediatequeTotalScanedDuration += Convert.ToInt64(DatabaseFiles[fi.FullName]["Duration"]);
                        MediatequeTotalScanedSize += Convert.ToInt64(DatabaseFiles[fi.FullName]["Size"]);
                    }
                }
                else
                {
                    //Debug.WriteLine("--> MediatequeScan SUB 4-2 <--");
                    PlayListViewItem item = null;
                    
                    string query = "INSERT INTO files("
                        + "Path,"
                        + "Name,"
                        + "Album,"
                        + "Performers,"
                        + "Composers,"
                        + "Genres,"
                        + "Copyright,"
                        + "AlbumArtists,"
                        + "Lyrics,"
                        + "Duration,"
                        + "Size,"
                        + "Disc,"
                        + "DiscCount,"
                        + "Track,"
                        + "TrackCount,"
                        + "Year,"
                        + "LastUpdate"
                        + ") VALUES('";
                    query += Database.DatabaseEscapeString(fi.FullName) + "',";
                    query += "'" + Database.DatabaseEscapeString(fi.Name) + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0','0','0','0','0','0','0',";

                    query += "'" + fi.LastWriteTimeUtc.ToFileTime() + "')";
                    MediatequeTotalScanedFiles += 1;
                    NewFiles.Add(fi.FullName);
                    MediatequeCacheQuerys.Add(query);
                }

                if (MediatequeCacheQuerys.Count >= 100)
                {
                    //Debug.WriteLine("--> MediatequeScan SUB 5 <--");
                    bdd.DatabaseQuerys(MediatequeCacheQuerys.ToArray());
                    MediatequeCacheQuerys.Clear();
                    _ = Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MediatequeBuildNavigationContent(MediatequeRefFolder);
                    }));
                }


            }

            //Debug.WriteLine("--> MediatequeScan SUB 6 <--");
            //Debug.WriteLine(JsonConvert.SerializeObject(MediatequeCacheQuerys.ToArray()));
            bdd.DatabaseQuerys(MediatequeCacheQuerys.ToArray());
            MediatequeCacheQuerys.Clear();

            //Debug.WriteLine("--> MediatequeScan SUB 7 <--");
            foreach (KeyValuePair<string, Dictionary<string, object>> fi in DatabaseFiles.ToList())
            {
                if (!MediatequeScanedFiles.Contains(fi.Key))
                {
                    bdd.DatabaseQuery("DELETE FROM files WHERE Path='" + Database.DatabaseEscapeString(fi.Key) + "'");
                }
            }

            _ = Dispatcher.BeginInvoke(new Action(() => {
                MediatequeBuildNavigationScan();
            }));

            DatabaseFiles.Clear();

            //Debug.WriteLine("--> MediatequeScan SUB 8 <--");
            MediatequeEnableFilters();
            //Debug.WriteLine("--> MediatequeScan SUB 9 <--");
            MediatequeScanning = false;
            _ = Dispatcher.InvokeAsync(new Action(() =>
            {
                MediatequeBuildNavigationPath(MediatequeRefFolder);
                MediatequeBuildNavigationContent(MediatequeRefFolder);
                if (NewFiles.Count > 0)
                {
                    Thread objThread = new Thread(new ParameterizedThreadStart(MediatequeScanTags));
                    objThread.IsBackground = true;
                    objThread.Priority = ThreadPriority.Normal;
                    objThread.Start(NewFiles.ToArray());
                }
            }));
        }

        /// <summary> Fill the Navigation bar in Library pannel when MediatequeScanning files </summary>
        private void MediatequeBuildNavigationScan()
        {
            LibNavigationPathContener.Children.Clear();
            TextBlock tb2 = new TextBlock();
            tb2.Text = "<< " + GetTaduction("LibMediaMediatequeScanning") + " >>, " + GetTaduction("LibMediaFiles") + ": ";
            tb2.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb2);

            TextBlock tb3 = new TextBlock();
            tb3.Text = "" + MediatequeTotalScanedFiles;
            tb3.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb3);

            TextBlock tb4 = new TextBlock();
            tb4.Text = ", " + GetTaduction("LibMediaTotalSize") + ": ";
            tb4.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb4);

            TextBlock tb5 = new TextBlock();
            tb5.Text = "" + BytesLengthToString((long)MediatequeTotalScanedSize);
            tb5.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb5);

            TextBlock tb6 = new TextBlock();
            tb6.Text = ", " + GetTaduction("LibMediaTotalDuration") + ": ";
            tb6.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb6);

            TextBlock tb7 = new TextBlock();
            tb7.Text = "" + displayTime((long)MediatequeTotalScanedDuration);
            tb7.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb7);

            //LibNavigationContent.Orientation = Orientation.Vertical;
        }



        /// <summary> Sub Recursive Function for MediatequeScanning folder </summary>
        private void MediatequeLoadSubDirectories(string dir, Folder fold, bool DatabaseUpdate = true)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                if (di.Name.StartsWith('.')) { continue; }
                Folder fold2 = new Folder() { Name = di.Name, Path = di.FullName, Parent = fold };
                MediatequeLoadFiles(subdirectory, fold2, DatabaseUpdate);
                MediatequeLoadSubDirectories(subdirectory, fold2, DatabaseUpdate);
                if (MediatequeCurrentFolderS == di.FullName) { MediatequeCurrentFolder = fold2; }
                fold.Folders.Add(fold2);
            }
        }

        /// <summary> Sub Function for MediatequeScanning folder, fill the file list </summary>
        private void MediatequeLoadFiles(string dir, Folder fold, bool DatabaseUpdate = true)
        {
            string[] Files = Directory.GetFiles(dir, "*.*");
            bool ok = false;
            string[] extensions = new string[] { ".mp3", ".wma", ".flac", ".ogg", ".aac" };

            // Loop through them to see files  
            foreach (string file in Files)
            {
                ok = false;
                foreach (string ext in extensions)
                {
                    if (file.ToLower().EndsWith(ext)) { ok = true; break; }
                }
                if (ok)
                {
                    fold.Files.Add(file);
                    MediatequeScanedFiles.Add(file);
                    if (DatabaseUpdate)
                    {

                    }
                }
            }
        }

    }
}