using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnotherMusicPlayer
{
    /// <summary> Class Folder for storing a hierarchical structure of Folder + Files </summary>
    public class Folder
    {
        /// <summary> list of Files in full path </summary>
        public List<string> Files = new List<string>();
        /// <summary> List of Object Folder </summary>
        public List<Folder> Folders = new List<Folder>();
        /// <summary> Folder name </summary>
        public string Name = null;
        /// <summary> Folder full path </summary>
        public string Path = null;
        /// <summary> Reference of Folder Parent object </summary>
        public Folder Parent = null;
    }

    public partial class MainWindow : Window
    {
        /// <summary> Object Folder of scaned Library </summary>
        private Folder MediatequeRefFolder = new Folder();
        /// <summary> Object Folder of current position in Library </summary>
        private Folder MediatequeCurrentFolder = null;
        /// <summary> String Path of current position in Library </summary>
        private string MediatequeCurrentFolderS = null;
        /// <summary> Object Watcher for detecting Library Modifications </summary>
        private FileSystemWatcher MediatequeWatcher = null;
        /// <summary> Counter of Total number of scaned files </summary>
        private double MediatequeTotalScanedFiles = 0;
        /// <summary> Counter of Total scaned Size </summary>
        private double MediatequeTotalScanedSize = 0;
        /// <summary> Counter of Total scaned Duration </summary>
        private double MediatequeTotalScanedDuration = 0;
        /// <summary> status if currently scanning Library </summary>
        private bool Scanning = false;

        /// <summary> list of know files in database and their basic metadata </summary>
        Dictionary<string, Dictionary<string, object>> MediatequeBddFiles = new Dictionary<string, Dictionary<string, object>>();
        /// <summary> list of newly scanned files(absolute path) </summary>
        List<string> MediatequeScanedFiles = new List<string>();

        /// <summary> Asynchronus call for scanning Library in a new thread </summary>
        private async void MediatequeInvokeScan(bool DoClean = false) {
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

            MediatequeBddInit();
            if (Settings.LibFolder != null)
            {
                if (System.IO.Directory.Exists(Settings.LibFolder))
                {
                    Scanning = true;
                    Dispatcher.BeginInvoke(new Action(() => {
                        LibNavigationContent.Children.Clear();
                        MediatequeBuildNavigationScan();
                    }));
                    if (DoClean) {
                        MediatequeCurrentFolder = null; MediatequeWatcher = null;
                        MediatequeTotalScanedSize = 0;
                        MediatequeTotalScanedDuration = 0;
                        MediatequeTotalScanedFiles = 0;
                        //MediatequeBddQuery("DELETE FROM files");
                        MediatequeScanedFiles = new List<string>();
                    }
                    MediatequeBddFiles = MediatequeBddQuery("SELECT * FROM files ORDER BY Path ASC","Path");
                    //Debug.WriteLine(JsonConvert.SerializeObject(MediatequeBddFiles));
                    if (MediatequeWatcher == null) { MediatequeCreateWatcher(); }
                    else if (MediatequeWatcher.Path != Settings.LibFolder) { MediatequeCreateWatcher(); }

                    //LibTreeView.Items.Clear();
                    DirectoryInfo di = new DirectoryInfo(Settings.LibFolder);
                    MediatequeRefFolder = new Folder() { Name = di.Name, Path = di.FullName };

                    if (!IsInTransaction()) { MediatequeBddTansactionStart(); }
                    MediatequeLoadFiles(Settings.LibFolder, MediatequeRefFolder);
                    MediatequeLoadSubDirectories(Settings.LibFolder, MediatequeRefFolder);
                    if (IsInTransaction()) { MediatequeBddTansactionEnd(); }

                    if (!IsInTransaction()) { MediatequeBddTansactionStart(); }
                    foreach (string fi in MediatequeScanedFiles) {
                        if (!MediatequeScanedFiles.Contains(fi)) {
                            MediatequeBddQuery("DELETE FROM files WHERE Path='"+ MediatequeBddEscapeString(fi) + "'");
                        }
                    }
                    if (IsInTransaction()) { MediatequeBddTansactionEnd(); }

                    MediatequeBddFiles = MediatequeBddQuery("SELECT * FROM files ORDER BY Path ASC", "Path");

                    Dispatcher.BeginInvoke(new Action(() => {
                        LibNavigationContent.Orientation = Orientation.Horizontal;
                        MediatequeBuildNavigationPath(MediatequeCurrentFolder ?? MediatequeRefFolder);
                        MediatequeBuildNavigationContent(MediatequeCurrentFolder ?? MediatequeRefFolder);
                    }));
                    Scanning = false;
                }
            }
        }

        /// <summary> Create a Library folder watcher </summary>
        private void MediatequeCreateWatcher()
        {
            if (MediatequeWatcher != null) { MediatequeWatcher.Dispose(); }
            MediatequeWatcher = new FileSystemWatcher();
            MediatequeWatcher.Path = Settings.LibFolder;
            MediatequeWatcher.IncludeSubdirectories = true;
            MediatequeWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            MediatequeWatcher.Filter = "*.*";
            MediatequeWatcher.Changed += new FileSystemEventHandler(MediatequeChanged);
            MediatequeWatcher.EnableRaisingEvents = true;
        }

        /// <summary> Fill the Navigation bar in Library pannel when scanning files </summary>
        private void MediatequeBuildNavigationScan()
        {
            LibNavigationPathContener.Children.Clear();
            TextBlock tb2 = new TextBlock();
            tb2.Text = "<< " + GetTaduction("LibMediaScanning") + " >>, " + GetTaduction("LibMediaFiles") + ": ";
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

            LibNavigationContent.Orientation = Orientation.Vertical;
        }

        /// <summary> Get Basic Metadata of a media file if stored in database </summary>
        private Dictionary<string, object> MediatequeBddFileInfo(string path) {
            if (MediatequeBddFiles.ContainsKey(path)) { return MediatequeBddFiles[path]; }
            else { return null; }
        }

        /// <summary> GetCallback when Mediateque Watcher detect a change </summary>
        private void MediatequeChanged(object source, FileSystemEventArgs e)
        {
            //Debug.WriteLine(e.Name);
            Dispatcher.BeginInvoke(new Action(() => { MediatequeScan(); }));
        }

        /// <summary> Recursive function for filling a List<string> with all the files Stored in the hierarchy of an Folder object </summary>
        private List<string> MediatequeCreateList(Folder fold, List<string> liste)
        {
            if (fold.Folders != null)
            {
                foreach (Folder fl in fold.Folders) { liste = MediatequeCreateList(fl, liste); }
            }
            foreach (string fi in fold.Files) { liste.Add(fi); }
            return liste;
        }

        /// <summary> Sub Recursive Function for scanning folder </summary>
        private void MediatequeLoadSubDirectories(string dir, Folder fold, bool BddUpdate = true)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                if (di.Name.StartsWith('.')) { continue; }
                Folder fold2 = new Folder() { Name = di.Name, Path = di.FullName, Parent = fold };
                MediatequeLoadFiles(subdirectory, fold2, BddUpdate);
                MediatequeLoadSubDirectories(subdirectory, fold2, BddUpdate);
                if (MediatequeCurrentFolderS == di.FullName) { MediatequeCurrentFolder = fold2; }
                fold.Folders.Add(fold2);
            }
        }

        /// <summary> Sub Function for scanning folder, fill the file list </summary>
        private void MediatequeLoadFiles(string dir, Folder fold, bool BddUpdate = true)
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
                    FileInfo fi = new FileInfo(file);
                    fold.Files.Add(fi.FullName);
                    MediatequeTotalScanedFiles += 1;
                    if (BddUpdate)
                    {
                        if (MediatequeBddFiles.ContainsKey(fi.FullName))
                        {
                            //Debug.WriteLine(fi.FullName);
                            //Debug.WriteLine("LastUpdate BDD = " + (long)MediatequeBddFiles[fi.FullName]["LastUpdate"]);
                            //Debug.WriteLine("LastUpdate File = " + fi.LastWriteTimeUtc.ToFileTime());
                            if (Convert.ToInt64((string)MediatequeBddFiles[fi.FullName]["LastUpdate"]) < fi.LastWriteTimeUtc.ToFileTime())
                            {
                                MediatequeScanedFiles.Add(fi.FullName);
                                PlayListViewItem item = player.MediaInfo(fi.FullName, false);
                                MediatequeTotalScanedDuration += item.Duration;
                                MediatequeTotalScanedSize += item.Size;
                                MediatequeBddQuery("UPDATE files SET Name='" + MediatequeBddEscapeString(item.Name)
                                    + "', Album='" + MediatequeBddEscapeString(item.Album)
                                    + "', Artists='" + MediatequeBddEscapeString(item.Artist)
                                    + "', Duration='" + item.Duration
                                    + "', Size='" + item.Size
                                    + "', LastUpdate='" + fi.LastWriteTimeUtc.ToFileTime()
                                    + "' WHERE Path='" + MediatequeBddEscapeString(fi.FullName) + "'");
                            }
                            else
                            {
                                MediatequeTotalScanedDuration += Convert.ToInt64(MediatequeBddFiles[fi.FullName]["Duration"]);
                                MediatequeTotalScanedSize += Convert.ToInt64(MediatequeBddFiles[fi.FullName]["Size"]);
                            }
                        }
                        else
                        {
                            MediatequeScanedFiles.Add(fi.FullName);
                            PlayListViewItem item = player.MediaInfo(fi.FullName, false);
                            MediatequeTotalScanedDuration += item.Duration;
                            MediatequeTotalScanedSize += item.Size;
                            string query = "INSERT INTO files(Path,Name,Album,Artists,Duration,Size,LastUpdate) VALUES('";
                            query += MediatequeBddEscapeString(fi.FullName) + "','";
                            query += MediatequeBddEscapeString(item.Name) + "','";
                            query += MediatequeBddEscapeString(item.Album) + "','";
                            query += MediatequeBddEscapeString(item.Artist) + "','";
                            query += item.Duration + "','";
                            query += item.Size + "','";
                            query += fi.LastWriteTimeUtc.ToFileTime() + "')";
                            MediatequeBddQuery(query);
                        }

                        if (MediatequeScanedFiles.Count % 100 == 0)
                        {
                            if (IsInTransaction()) { MediatequeBddTansactionEnd(); }
                            if (!IsInTransaction()) { MediatequeBddTansactionStart(); }
                        }

                        Dispatcher.BeginInvoke(new Action(() => {
                            MediatequeBuildNavigationScan();

                            TextBlock t = new TextBlock();
                            t.Text = fi.FullName;
                            LibNavigationContent.Children.Insert(0, t);
                            if (LibNavigationContent.Children.Count > 100) { LibNavigationContent.Children.RemoveAt(100); }
                        }));
                    }
                }
            }
        }



        /// <summary> Create context menu for objects in Library </summary>
        private ContextMenu LibMediaCreateContextMenu()
        {//ContextMenuItemImage_add
            ContextMenu ct = new ContextMenu();
            MenuItem mu = new MenuItem()
            {
                Header = GetTaduction("ParamsLibItemContextMenuItem1"),
                Icon = ContextMenuItemImage_add
            };
            mu.Click += MediatequeCT_Open;
            ct.Items.Add(mu);
            return ct;
        }

        /// <summary> Fill the Navigation bar in Library pannel with current location </summary>
        private void MediatequeBuildNavigationPath(Folder fold) {
            LibNavigationPathContener.Children.Clear();
            MediatequeCurrentFolder = fold;
            MediatequeCurrentFolderS = fold.Path;
            string basePath = "Home/"+((fold.Path == Settings.LibFolder)?"": fold.Path.Replace(Settings.LibFolder, "").Replace(SeparatorChar, '/').Replace("//", "/"));
            string[] tabPath = basePath.Split('/');
            List<Folder> tabFold = new List<Folder>();
            Folder last = fold;
            tabFold.Insert(0, last);
            while (true) {
                last = last.Parent;
                if (last != null) { tabFold.Insert(0, last); }
                else { break; }
            }
            //Debug.WriteLine(Settings.LibFolder);
            //Debug.WriteLine(path);
            //Debug.WriteLine(basePath);

            int l1 = 0;
            string newPath = "";
            foreach (string pa in tabPath)
            {
                if (pa != "")
                {
                    if (l1>0)
                    {
                        if (pa == "Home") { break; }
                        TextBlock tb2 = new TextBlock();
                        tb2.Text = "/";
                        LibNavigationPathContener.Children.Add(tb2);
                        newPath += "/";
                    }

                    newPath += pa;
                    TextBlock tb3 = new TextBlock();
                    tb3.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationPathItem"];
                    tb3.Text = pa;
                    tb3.Tag = new object[] { "folder", tabFold[l1].Path, tabFold[l1] };
                    tb3.MouseDown += MediatequeBuildNavigationPathClick;
                    tb3.ContextMenu = LibMediaCreateContextMenu();

                    LibNavigationPathContener.Children.Add(tb3);
                    l1 += 1;
                }
            }

        }

        /// <summary> Callback click Navigation item in Navigation bar, used for changing location in library </summary>
        private void MediatequeBuildNavigationPathClick(object sender, MouseButtonEventArgs e)
        {
            object[] ob = (object[])((TextBlock)sender).Tag;
            Folder v = (Folder)(ob[2]);
            MediatequeCurrentFolder = v;
            //Debug.WriteLine(v.Name);
            MediatequeBuildNavigationPath(v);
            MediatequeBuildNavigationContent(v);
        }


        /// <summary> Fill the Content zone in Library pannel with folders and files </summary>
        private void MediatequeBuildNavigationContent(Folder fold) {
            LibNavigationContent.Children.Clear();

            foreach (Folder fl in fold.Folders) { MediatequeBuildNavigationContentButton("folder", fl.Name, fl.Path, fl); }
            foreach (string fi in fold.Files) { MediatequeBuildNavigationContentButton("file", fi, fold.Path + SeparatorChar + fi); }
        }

        /// <summary> Create button for the Content zone in Library pannel </summary>
        private void MediatequeBuildNavigationContentButton(string type, string name, string path, Folder fold = null)
        {
            Border br = new Border();
            br.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemBorder"];
            Grid gr = new Grid();
            gr.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItem"];
            gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
            gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            gr.ToolTip = name;

            Image image = new Image();
            if (type == "folder") { image.Source = Bimage("OpenButtonImg"); }
            if (type == "file") { image.Source = Bimage("CoverImg"); }
            image.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemImg"];
            gr.Children.Add(image);

            AccessText tx = new AccessText();
            tx.Text = name;
            tx.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemText"];
            //gr.Children.Add(tx);
            //Grid.SetRow(tx, 1);

            Viewbox vb = new Viewbox();
            vb.Child = tx;
            gr.Children.Add(vb);
            Grid.SetRow(vb, 1);

            gr.Tag = new object[] { type, path, fold };
            gr.MouseLeftButtonDown += MediatequeNavigationContentButtonClick;
            gr.ContextMenu = LibMediaCreateContextMenu();

            br.Child = gr;
            LibNavigationContent.Children.Add(br);
        }

        /// <summary> Library pannel Content button last time click </summary>
        double MediatequeNavigationContentButtonClick_LastTime = 0;
        /// <summary> Library pannel Content button last reference click </summary>
        string MediatequeNavigationContentButtonClick_LastRef = "";
        /// <summary> Click Callback content button in Library pannel </summary>
        private void MediatequeNavigationContentButtonClick(object sender, MouseButtonEventArgs e)
        {
            double tmpt = UnixTimestamp();
            object[] re = (object[])((Grid)sender).Tag;
            if ((string)re[0] == "folder")
            {
                MediatequeBuildNavigationPath((Folder)re[2]);
                MediatequeBuildNavigationContent((Folder)re[2]);
            }
            else
            {
                if (MediatequeNavigationContentButtonClick_LastTime + 1 > tmpt && MediatequeNavigationContentButtonClick_LastRef == (string)re[1])
                {
                    //Debug.WriteLine(((Grid)sender).Tag);
                    if (System.IO.File.Exists((string)re[1]))
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Open(new string[] { (string)re[1] });
                        }));
                    }
                }
            }
            MediatequeNavigationContentButtonClick_LastTime = tmpt;
            MediatequeNavigationContentButtonClick_LastRef = (string)re[1];
        }

        /// <summary> Click Callback on ContextMenuItem </summary>
        public void MediatequeCT_Open(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("MediatequeCT_Open");
            MenuItem mi = (MenuItem)sender;
            ContextMenu ct = (ContextMenu)mi.Parent;
            object[] tab;
            try
            {   // With ContextMenu from Library pannel Content zone
                Grid gr = (Grid)ct.PlacementTarget;
                tab = (object[])gr.Tag; gr = null;
            }
            catch
            {
                // With ContextMenu from Library pannel Navigation bar
                TextBlock tx = (TextBlock)ct.PlacementTarget;
                tab = (object[])tx.Tag;
            }

            if ((string)tab[0] == "folder") {
                List<string> paths = new List<string>();
                Folder fold = (Folder)tab[2];
                paths = MediatequeCreateList(fold, paths);
                //Debug.WriteLine(JsonConvert.SerializeObject(paths.ToArray()));
                Dispatcher.BeginInvoke(new Action(() => { Open(paths.ToArray()); }));
            }
            else if ((string)tab[0] == "file")
            {
                Dispatcher.BeginInvoke(new Action(() => { Open(new string[] { (string)tab[1] }); }));
            }
        }

        public void UpdateRecordedQueue()
        {
            if (!IsInTransaction()) { MediatequeBddTansactionStart(); }
            MediatequeBddQuery("DELETE FROM queue;");
            int index = 1;
            foreach (string[] line in PlayList)
            {
                if (!IsInTransaction()) { MediatequeBddTansactionStart(); }
                string query = "INSERT INTO queue(MIndex, Path1, Path2) VALUES('";
                query += NormalizeNumber(index, 10) + "','";
                query += line[0] + "',";
                query += ((line[1] == null) ? "NULL" : "'" + line[1] + "'");
                query += ")";
                MediatequeBddQuery(query);
                index += 1;
            }
            if (IsInTransaction()) { MediatequeBddTansactionEnd(); }
        }

    }
}