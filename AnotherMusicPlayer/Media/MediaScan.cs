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

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary> Asynchronus call for MediatequeScanning Library in a new thread </summary>
        private async void MediatequeInvokeScan(bool DoClean = false)
        {
            try
            {
                Thread objThread = new Thread(new ParameterizedThreadStart(MediatequeScan));
                objThread.IsBackground = true;
                objThread.Priority = ThreadPriority.AboveNormal;
                objThread.Start(DoClean);
            }
            catch { }
        }

        /// <summary> Launch a scan of th Library </summary>
        private async void MediatequeScan(object param = null)
        {
            bool DoClean = false;
            try { DoClean = (bool)param; } catch { }

            DatabaseInit();
            if (Settings.LibFolder != null)
            {
                if (System.IO.Directory.Exists(Settings.LibFolder))
                {
                    MediatequeCacheQuerys.Clear();
                    MediatequeScanning = true;
                    _ = Dispatcher.BeginInvoke(new Action(() => {
                        LibraryFiltersGrid.IsEnabled = false;
                        LibNavigationContent.Children.Clear();
                        MediatequeBuildNavigationScan();
                    }));
                    if (DoClean)
                    {
                        _ = Dispatcher.BeginInvoke(new Action(() => {
                            MediatequeCurrentFolder = null; MediatequeWatcher = null;
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

                            DatabaseQuerys(new string[] { "DELETE FROM covers", "DELETE FROM folders", "DELETE FROM files", "DELETE FROM playlists", "DELETE FROM playlistsItems" });
                        }));
                    }
                    Dictionary<string, Dictionary<string, object>> DatabaseFiles = DatabaseQuery("SELECT * FROM files ORDER BY Path ASC", "Path");
                    //Debug.WriteLine(JsonConvert.SerializeObject(DatabaseFiles));
                    if (MediatequeWatcher == null) { MediatequeCreateWatcher(); }
                    else if (MediatequeWatcher.Path != Settings.LibFolder) { MediatequeCreateWatcher(); }

                    //LibTreeView.Items.Clear();
                    DirectoryInfo di = new DirectoryInfo(Settings.LibFolder);
                    MediatequeRefFolder = new Folder() { Name = di.Name, Path = di.FullName };

                    MediatequeLoadFiles(Settings.LibFolder, MediatequeRefFolder);
                    MediatequeLoadSubDirectories(Settings.LibFolder, MediatequeRefFolder);

                    List<string> lf = MediatequeScanedFiles.ToList();
                    bool test = true;
                    //if (test == true) { DatabaseTansactionStart(); }

                    foreach (string file in lf)
                    {
                        MediatequeTotalScanedFiles += 1;
                        FileInfo fi = new FileInfo(file);
                        if (DatabaseFiles.ContainsKey(fi.FullName))
                        {
                            //Debug.WriteLine(fi.FullName);
                            //Debug.WriteLine("LastUpdate Database = " + (long)DatabaseFiles[fi.FullName]["LastUpdate"]);
                            //Debug.WriteLine("LastUpdate File = " + fi.LastWriteTimeUtc.ToFileTime());

                            //if (test != true) {
                            if (Convert.ToInt64((string)DatabaseFiles[fi.FullName]["LastUpdate"]) < fi.LastWriteTimeUtc.ToFileTime())
                            {
                                MediatequeScanedFiles.Add(fi.FullName);
                                PlayListViewItem item = player.MediaInfo(fi.FullName, false);
                                MediatequeTotalScanedDuration += item.Duration;
                                MediatequeTotalScanedSize += item.Size;
                                MediatequeCacheQuerys.Add("UPDATE files SET Name='" + DatabaseEscapeString(item.Name)
                                    + "', Album='" + DatabaseEscapeString(item.Album)
                                    + "', Performers='" + DatabaseEscapeString(item.Performers)
                                    + "', Composers='" + DatabaseEscapeString(item.Composers)
                                    + "', Genres='" + DatabaseEscapeString(item.Genres)
                                    + "', Copyright='" + DatabaseEscapeString(item.Copyright)
                                    + "', AlbumArtists='" + DatabaseEscapeString(item.AlbumArtists)
                                    + "', Lyrics='" + DatabaseEscapeString(item.Lyrics)
                                    + "', Duration='" + item.Duration
                                    + "', Size='" + item.Size
                                    + "', Disc='" + item.Disc
                                    + "', DiscCount='" + item.DiscCount
                                    + "', Track='" + item.Track
                                    + "', TrackCount='" + item.TrackCount
                                    + "', Year='" + item.Year
                                    + "', LastUpdate='" + fi.LastWriteTimeUtc.ToFileTime()
                                    + "' WHERE Path='" + DatabaseEscapeString(fi.FullName) + "'");
                            }
                            else
                            {
                                MediatequeTotalScanedDuration += Convert.ToInt64(DatabaseFiles[fi.FullName]["Duration"]);
                                MediatequeTotalScanedSize += Convert.ToInt64(DatabaseFiles[fi.FullName]["Size"]);
                            }
                            //}
                        }
                        else
                        {
                            PlayListViewItem item = null;
                            if (test != true)
                            {
                                item = player.MediaInfo(fi.FullName, false);
                                MediatequeTotalScanedDuration += item.Duration;
                                MediatequeTotalScanedSize += item.Size;
                            }

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
                            query += DatabaseEscapeString(fi.FullName) + "',";
                            if (test == true)
                            {
                                query += "'" + DatabaseEscapeString(fi.Name) + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0','0','0','0','0','0','0',";
                            }
                            else
                            {
                                query += "'" + DatabaseEscapeString(item.Name) + "','";
                                query += DatabaseEscapeString(item.Album) + "','";
                                query += DatabaseEscapeString(item.Performers) + "','";
                                query += DatabaseEscapeString(item.Composers) + "','";
                                query += DatabaseEscapeString(item.Genres) + "','";
                                query += DatabaseEscapeString(item.Copyright) + "','";
                                query += DatabaseEscapeString(item.AlbumArtists) + "','";
                                query += DatabaseEscapeString(item.Lyrics) + "','";
                                query += item.Duration + "','";
                                query += item.Size + "','";
                                query += item.Disc + "','";
                                query += item.DiscCount + "','";
                                query += item.Track + "','";
                                query += item.TrackCount + "','";
                                query += item.Year + "',";
                            }

                            query += "'" + fi.LastWriteTimeUtc.ToFileTime() + "')";

                            //if (test == true) { DatabaseQuerys(new string[]{ query }); }
                            //else { 
                            MediatequeCacheQuerys.Add(query);
                            //}
                        }

                        if (MediatequeCacheQuerys.Count >= 100)
                        {
                            DatabaseQuerys(MediatequeCacheQuerys.ToArray());
                            _ = Dispatcher.BeginInvoke(new Action(() => {
                                MediatequeCacheQuerys.Clear();
                                MediatequeBuildNavigationScan();
                            }));
                        }


                    }

                    //if (test == true) { DatabaseTansactionEnd(); }

                    Debug.WriteLine(JsonConvert.SerializeObject(MediatequeCacheQuerys.ToArray()));
                    DatabaseQuerys(MediatequeCacheQuerys.ToArray());
                    _ = Dispatcher.BeginInvoke(new Action(() => {
                        MediatequeCacheQuerys.Clear();
                    }));



                    //foreach (string fi in MediatequeScanedFiles) {
                    //    if (!MediatequeScanedFiles.Contains(fi)) {
                    //        DatabaseQuery("DELETE FROM files WHERE Path='"+ DatabaseEscapeString(fi) + "'");
                    //    }
                    //}

                    _ = Dispatcher.BeginInvoke(new Action(() => {
                        MediatequeBuildNavigationScan();
                    }));

                    DatabaseFiles.Clear();

                    _ = Dispatcher.BeginInvoke(new Action(() => {
                        if (DoClean || LibraryFiltersGenreList.Items.Count == 0)
                        {
                            Dictionary<string, Dictionary<string, object>> genres = DatabaseQuery("SELECT Genres FROM files GROUP BY Genres ORDER BY Genres ASC", "Genres");
                            List<ComboBoxItem> li = new List<ComboBoxItem>();
                            //li.Add(new ComboBoxItem() { Content = "", Tag = "" });
                            foreach (string key in genres.Keys) { li.Add(new ComboBoxItem() { Content = key, Tag = key }); }
                            LibraryFiltersGenreList.ItemsSource = li;
                            //LibraryFiltersGenreList.Visibility = Visibility.Visible;
                        }

                        LibraryFiltersGrid.IsEnabled = true;

                        //LibNavigationContent.Orientation = Orientation.Horizontal;
                        MediatequeBuildNavigationPath(MediatequeCurrentFolder ?? MediatequeRefFolder);
                        MediatequeBuildNavigationContent(MediatequeCurrentFolder ?? MediatequeRefFolder);
                    }));
                    MediatequeScanning = false;
                }
            }
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