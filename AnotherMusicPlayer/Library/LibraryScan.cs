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
        List<string> LibraryScanedFiles = new List<string>();

        /// <summary> Asynchronus call for LibraryScanning Library in a new thread </summary>
        private async void LibraryInvokeScan(bool DoClean = false, bool boot = false)
        {
            Debug.WriteLine("LibraryInvokeScan(DoClean="+((DoClean)?"true":"false")+ ", boot=" + ((boot) ? "true" : "false") + ")");
            //boot = false;
            DirectoryInfo di = new DirectoryInfo(Settings.LibFolder); 
            Dictionary<string, Dictionary<string, object>> DatabaseFiles = bdd.DatabaseQuery("SELECT * FROM files ORDER BY Path ASC", "Path");

            if (LibraryWatcher == null) { LibraryCreateWatcher(); }
            else if (LibraryWatcher.Path != Settings.LibFolder) { LibraryCreateWatcher(); }

            if (boot == true & DatabaseFiles.Count > 1)
            {
                LibraryScanedFiles.Clear();
                LibraryLoadFiles(Settings.LibFolder, LibraryRefFolder);
                LibraryLoadSubDirectories(Settings.LibFolder, LibraryRefFolder);

                _ = Dispatcher.InvokeAsync(new Action(() => {
                    LibraryScanning = false;
                    string[] a = Directory.GetFiles(Settings.LibFolder, "*.*");
                    string[] b = Directory.GetDirectories(Settings.LibFolder, "*");
                    List<string> openItems = new List<string>();

                    foreach (string arrItem in a)
                    {
                        openItems.Add(arrItem.Replace(Settings.LibFolder + SeparatorChar, ""));
                    }
                    Folder fold = new Folder() { Name = "Home", Path = Settings.LibFolder, Parent = null };
                    LibraryLoadFiles(Settings.LibFolder, fold);
                    LibraryLoadSubDirectories(Settings.LibFolder, fold);
                    _ = Dispatcher.BeginInvoke(new Action(() =>
                    {
                        LibraryBuildNavigationPath(fold.Folders[0]);

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
                            Thread objThread = new Thread(new ParameterizedThreadStart(LibraryScanTags));
                            objThread.IsBackground = true;
                            objThread.Priority = ThreadPriority.Normal;
                            objThread.Start(DatabaseFiles.Keys.ToList().ToArray());
                        }
                        else
                        {
                            LibraryEnableFilters();
                        }
                    }));

                    //LibraryBuildNavigationContent(LibraryRefFolder);
                    dialog1.IsOpen = false;
                }));
            }
            else
            {
                try
                {
                    setLoadingState(true, "Library scan");
                    Thread objThread = new Thread(new ParameterizedThreadStart(LibraryScan));
                    objThread.IsBackground = true;
                    objThread.Priority = ThreadPriority.Normal;
                    objThread.Start(DoClean);
                }
                catch { }
            }
        }

        private void LibraryEnableFilters() {
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
        private async void LibraryScanTags(object param = null)
        {
            Debug.WriteLine("--> LibraryScanTags <--");
            string[] files = (string[])param;
            setMetadataScanningState(true, files.Length);
            _ = bdd.DatabaseFilesInfo(files, this);
            LibraryEnableFilters();
            setMetadataScanningState(false);
            Debug.WriteLine("--> LibraryScanTags END <--");
        }

        /// <summary> Launch a scan of th Library </summary>
        private async void LibraryScan(object param = null)
        {
            Debug.WriteLine("--> LibraryScan <--");
            bool DoClean = false;
            try { DoClean = (bool)param; } catch { }
            if (Settings.LibFolder == null) { return; }
            if (!System.IO.Directory.Exists(Settings.LibFolder)) { return; }

            LibraryScanedFiles.Clear();

            LibraryLoadFiles(Settings.LibFolder, LibraryRefFolder);
            LibraryLoadSubDirectories(Settings.LibFolder, LibraryRefFolder);

            LibraryScanedFiles = LibraryScanedFiles.Distinct().ToList();
            //Debug.WriteLine("--> LibraryScan SUB 0 <--");
            //Debug.WriteLine(LibraryScanedFiles.Count);

            List<string> LibraryCacheQuerys = new List<string>();
            List<string> NewFiles = new List<string>();

            LibraryScanning = true;

            //Debug.WriteLine("--> LibraryScan SUB 1 <--");
            _ = Dispatcher.BeginInvoke(new Action(() => {
                LibraryFiltersGrid.IsEnabled = false;
                LibNavigationContent.Children.Clear();
                LibraryBuildNavigationScan();

                if (DoClean)
                {
                    LibraryCurrentFolder = null; 
                    LibraryWatcher = null;
                    LibraryTotalScanedSize = 0;
                    LibraryTotalScanedDuration = 0;
                    LibraryTotalScanedFiles = 0;
                    //DatabaseQuery("DELETE FROM files");
                    LibraryScanedFiles = new List<string>();
                    LibraryFiltersMode.SelectedIndex = 0;
                    LibraryFiltersGenreList.SelectedIndex = 0;
                    try { LibraryFiltersGenreList.Items.Clear(); }
                    catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
                    LibraryFiltersSearchBox.Text = "";

                    bdd.DatabaseQuerys(new string[] { "DELETE FROM covers", "DELETE FROM folders", "DELETE FROM files", "DELETE FROM playlists", "DELETE FROM playlistsItems" });
                }
            }));
            //Debug.WriteLine("--> LibraryScan SUB 2 <--");


            Dictionary<string, Dictionary<string, object>> DatabaseFiles = bdd.DatabaseQuery("SELECT * FROM files ORDER BY Path ASC", "Path");
            //Debug.WriteLine(JsonConvert.SerializeObject(DatabaseFiles));

            //LibTreeView.Items.Clear();

            //Debug.WriteLine("--> LibraryScan SUB 3 <--");
            //Debug.WriteLine(LibraryScanedFiles.Count);
            foreach (string file in LibraryScanedFiles)
            {
                LibraryTotalScanedFiles += 1;
                FileInfo fi = new FileInfo(file);

                if (DatabaseFiles.ContainsKey(file))
                {
                    //Debug.WriteLine("--> LibraryScan SUB 4-1 <--");
                    Dictionary<string, object> dfi = bdd.DatabaseFileInfo(fi.FullName);
                    //Debug.WriteLine(fi.FullName);
                    //Debug.WriteLine("LastUpdate Database = " + (long)DatabaseFiles[fi.FullName]["LastUpdate"]);
                    //Debug.WriteLine("LastUpdate File = " + fi.LastWriteTimeUtc.ToFileTime());

                    if(dfi != null)
                    if (Convert.ToInt64((string)dfi["LastUpdate"]) != Convert.ToInt64(fi.LastWriteTimeUtc.ToFileTime()))
                    {
                        //Debug.WriteLine("--> update <--");
                        LibraryTotalScanedDuration += Int32.Parse((string)dfi["Duration"]);
                        LibraryTotalScanedSize += Int32.Parse((string)dfi["Size"]);
                        LibraryCacheQuerys.Add("UPDATE files SET Name='" + Database.DatabaseEscapeString((string)dfi["Name"])
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
                        LibraryTotalScanedDuration += Convert.ToInt64(DatabaseFiles[fi.FullName]["Duration"]);
                        LibraryTotalScanedSize += Convert.ToInt64(DatabaseFiles[fi.FullName]["Size"]);
                    }
                }
                else
                {
                    //Debug.WriteLine("--> LibraryScan SUB 4-2 <--");
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
                    LibraryTotalScanedFiles += 1;
                    NewFiles.Add(fi.FullName);
                    LibraryCacheQuerys.Add(query);
                }

                if (LibraryCacheQuerys.Count >= 100)
                {
                    //Debug.WriteLine("--> LibraryScan SUB 5 <--");
                    bdd.DatabaseQuerys(LibraryCacheQuerys.ToArray());
                    LibraryCacheQuerys.Clear();
                    _ = Dispatcher.BeginInvoke(new Action(() =>
                    {
                        LibraryBuildNavigationContent(LibraryRefFolder);
                    }));
                }


            }

            //Debug.WriteLine("--> LibraryScan SUB 6 <--");
            //Debug.WriteLine(JsonConvert.SerializeObject(LibraryCacheQuerys.ToArray()));
            bdd.DatabaseQuerys(LibraryCacheQuerys.ToArray());
            LibraryCacheQuerys.Clear();

            //Debug.WriteLine("--> LibraryScan SUB 7 <--");
            foreach (KeyValuePair<string, Dictionary<string, object>> fi in DatabaseFiles.ToList())
            {
                if (!LibraryScanedFiles.Contains(fi.Key))
                {
                    bdd.DatabaseQuery("DELETE FROM files WHERE Path='" + Database.DatabaseEscapeString(fi.Key) + "'");
                }
            }

            _ = Dispatcher.BeginInvoke(new Action(() => {
                LibraryBuildNavigationScan();
            }));

            DatabaseFiles.Clear();

            //Debug.WriteLine("--> LibraryScan SUB 8 <--");
            LibraryEnableFilters();
            //Debug.WriteLine("--> LibraryScan SUB 9 <--");
            LibraryScanning = false;
            _ = Dispatcher.InvokeAsync(new Action(() =>
            {
                LibraryBuildNavigationPath(LibraryRefFolder);
                LibraryBuildNavigationContent(LibraryRefFolder);
                if (NewFiles.Count > 0)
                {
                    Thread objThread = new Thread(new ParameterizedThreadStart(LibraryScanTags));
                    objThread.IsBackground = true;
                    objThread.Priority = ThreadPriority.Normal;
                    objThread.Start(NewFiles.ToArray());
                }
            }));
        }

        /// <summary> Fill the Navigation bar in Library pannel when LibraryScanning files </summary>
        private void LibraryBuildNavigationScan()
        {
            LibNavigationPathContener.Children.Clear();
            TextBlock tb2 = new TextBlock();
            tb2.Text = "<< " + GetTranslation("LibMediaLibraryScanning") + " >>, " + GetTranslation("LibMediaFiles") + ": ";
            tb2.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb2);

            TextBlock tb3 = new TextBlock();
            tb3.Text = "" + LibraryTotalScanedFiles;
            tb3.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb3);

            TextBlock tb4 = new TextBlock();
            tb4.Text = ", " + GetTranslation("LibMediaTotalSize") + ": ";
            tb4.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb4);

            TextBlock tb5 = new TextBlock();
            tb5.Text = "" + BytesLengthToString((long)LibraryTotalScanedSize);
            tb5.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb5);

            TextBlock tb6 = new TextBlock();
            tb6.Text = ", " + GetTranslation("LibMediaTotalDuration") + ": ";
            tb6.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb6);

            TextBlock tb7 = new TextBlock();
            tb7.Text = "" + displayTime((long)LibraryTotalScanedDuration);
            tb7.FontSize = 8;
            LibNavigationPathContener.Children.Add(tb7);

            //LibNavigationContent.Orientation = Orientation.Vertical;
        }



        /// <summary> Sub Recursive Function for LibraryScanning folder </summary>
        private void LibraryLoadSubDirectories(string dir, Folder fold, bool DatabaseUpdate = true)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                if (di.Name.StartsWith('.')) { continue; }
                Folder fold2 = new Folder() { Name = di.Name, Path = di.FullName, Parent = fold };
                LibraryLoadFiles(subdirectory, fold2, DatabaseUpdate);
                LibraryLoadSubDirectories(subdirectory, fold2, DatabaseUpdate);
                if (LibraryCurrentFolderS == di.FullName) { LibraryCurrentFolder = fold2; }
                fold.Folders.Add(fold2);
            }
        }

        /// <summary> Sub Function for LibraryScanning folder, fill the file list </summary>
        private void LibraryLoadFiles(string dir, Folder fold, bool DatabaseUpdate = true)
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
                    LibraryScanedFiles.Add(file);
                    if (DatabaseUpdate)
                    {

                    }
                }
            }
        }

    }
}