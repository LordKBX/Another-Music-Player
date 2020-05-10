using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Data;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Data.SQLite;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace AnotherMusicPlayer
{

    public class Folder
    {
        public List<string> Files = new List<string>();
        public List<Folder> Folders = new List<Folder>();
        public string Name = null;
        public string Path = null;
        public Folder Parent = null;
    }

    public partial class MainWindow : Window
    {
        private Folder MediatequeRefFolder = new Folder();
        private Folder MediatequeCurrentFolder = null;
        private string MediatequeCurrentFolderS = null;
        private FileSystemWatcher MediatequeWatcher = null;
        private double MediatequeTotalScanedSize = 0;
        private double MediatequeTotalScanedDuration = 0;

        Dictionary<string, Dictionary<string, object>> MediatequeBddFiles = new Dictionary<string, Dictionary<string, object>>();
        List<string> MediatequeScanedFiles = new List<string>();
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
        private async void MediatequeScan(object param) {
            try { MediatequeScan((bool)param); }
            catch { }
        }
        private async void MediatequeScan(bool DoClean = false)
        {
            MediatequeBddInit();
            if (Settings.LibFolder != null)
            {
                if (System.IO.Directory.Exists(Settings.LibFolder))
                {
                    Dispatcher.BeginInvoke(new Action(() => {
                        LibNavigationPathContener.Children.Clear();
                        LibNavigationContent.Children.Clear();

                        TextBlock tb2 = new TextBlock();
                        tb2.Text = "<< Scan en cours >>, Fichiers trouvés: ";
                        tb2.FontSize = 8;
                        LibNavigationPathContener.Children.Add(tb2);

                        TextBlock tb3 = new TextBlock();
                        tb3.Text = "" + MediatequeScanedFiles.Count;
                        tb3.FontSize = 8;
                        LibNavigationPathContener.Children.Add(tb3);

                        TextBlock tb4 = new TextBlock();
                        tb4.Text = ", Taille Totale: ";
                        tb4.FontSize = 8;
                        LibNavigationPathContener.Children.Add(tb4);

                        TextBlock tb5 = new TextBlock();
                        tb5.Text = "" + MediatequeTotalScanedSize;
                        tb5.FontSize = 8;
                        LibNavigationPathContener.Children.Add(tb5);

                        TextBlock tb6 = new TextBlock();
                        tb6.Text = ", Durée Totale: ";
                        tb6.FontSize = 8;
                        LibNavigationPathContener.Children.Add(tb6);

                        TextBlock tb7 = new TextBlock();
                        tb7.Text = "" + MediatequeTotalScanedDuration;
                        tb7.FontSize = 8;
                        LibNavigationPathContener.Children.Add(tb7);

                        LibNavigationContent.Orientation = Orientation.Vertical;
                    }));
                    if (DoClean) {
                        MediatequeCurrentFolder = null; MediatequeWatcher = null;
                        MediatequeTotalScanedSize = 0;
                        MediatequeTotalScanedDuration = 0;
                        //MediatequeBddQuery("DELETE FROM files");
                        MediatequeScanedFiles = new List<string>();
                    }
                    MediatequeBddFiles = MediatequeBddQuery("SELECT * FROM files ORDER BY Path ASC","Path");
                    Debug.WriteLine(JsonConvert.SerializeObject(MediatequeBddFiles));
                    if (MediatequeWatcher == null) { MediatequeCreateWatcher(); }
                    else if (MediatequeWatcher.Path != Settings.LibFolder) { MediatequeCreateWatcher(); }

                    //LibTreeView.Items.Clear();
                    DirectoryInfo di = new DirectoryInfo(Settings.LibFolder);
                    MediatequeRefFolder = new Folder() { Name = di.Name, Path = di.FullName };

                    MediatequeBddTansactionStart();
                    MediatequeLoadFiles(Settings.LibFolder, MediatequeRefFolder);
                    MediatequeLoadSubDirectories(Settings.LibFolder, MediatequeRefFolder);
                    MediatequeBddTansactionEnd();

                    MediatequeBddTansactionStart();
                    foreach (string fi in MediatequeScanedFiles) {
                        if (!MediatequeScanedFiles.Contains(fi)) {
                            MediatequeBddQuery("DELETE FROM files WHERE Path='"+ MediatequeBddEscapeString(fi) + "'");
                        }
                    }
                    MediatequeBddTansactionEnd();

                    Dispatcher.BeginInvoke(new Action(() => {
                        LibNavigationContent.Orientation = Orientation.Horizontal;
                        MediatequeBuildNavigationPath(MediatequeCurrentFolder ?? MediatequeRefFolder);
                        MediatequeBuildNavigationContent(MediatequeCurrentFolder ?? MediatequeRefFolder);
                    }));
                }
            }
        }
        private void MediatequeCreateWatcher()
        {
            MediatequeWatcher = new FileSystemWatcher();
            MediatequeWatcher.Path = Settings.LibFolder;
            MediatequeWatcher.IncludeSubdirectories = true;
            MediatequeWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            MediatequeWatcher.Filter = "*.*";
            MediatequeWatcher.Changed += new FileSystemEventHandler(MediatequeChanged);
            MediatequeWatcher.EnableRaisingEvents = true;
        }

        private Dictionary<string, object> MediatequeBddFileInfo(string path) {
            if (MediatequeBddFiles.ContainsKey(path)) { return MediatequeBddFiles[path]; }
            else { return null; }
        }

        private void MediatequeChanged(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine(e.Name);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MediatequeScan();
            }));
        }

        private void MediatequeLoadSubDirectories(string dir, Folder fold)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                if (di.Name.StartsWith('.')) { continue; }
                Folder fold2 = new Folder() { Name = di.Name, Path = di.FullName, Parent = fold };
                MediatequeLoadFiles(subdirectory, fold2);
                MediatequeLoadSubDirectories(subdirectory, fold2);
                if (MediatequeCurrentFolderS == di.FullName) { MediatequeCurrentFolder = fold2; }
                fold.Folders.Add(fold2);
            }
        }

        private void MediatequeLoadFiles(string dir, Folder fold)
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
                    //MediatequeScanedFiles.Add(fi.FullName);
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
                        else {
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
                        MediatequeBddTansactionEnd();
                        MediatequeBddTansactionStart();
                    }

                    Dispatcher.BeginInvoke(new Action(() => {
                        TextBlock tb2 = new TextBlock();
                        tb2.Text = fi.FullName.Replace(Settings.LibFolder, "");
                        LibNavigationContent.Children.Insert(0, tb2);
                        if (LibNavigationContent.Children.Count > 30) { LibNavigationContent.Children.RemoveAt(30); }

                        LibNavigationPathContener.Children.RemoveAt(1);
                        TextBlock tb3 = new TextBlock();
                        tb3.Text = "" + MediatequeScanedFiles.Count;
                        tb3.FontSize = 8;
                        LibNavigationPathContener.Children.Insert(1, tb3);

                        LibNavigationPathContener.Children.RemoveAt(3);
                        TextBlock tb5 = new TextBlock();
                        tb5.Text = "" + BytesLengthToString((long)MediatequeTotalScanedSize);
                        tb5.FontSize = 8;
                        LibNavigationPathContener.Children.Insert(3, tb5);

                        LibNavigationPathContener.Children.RemoveAt(5);
                        TextBlock tb7 = new TextBlock();
                        tb7.Text = "" + displayTime((long)MediatequeTotalScanedDuration);
                        tb7.FontSize = 8;
                        LibNavigationPathContener.Children.Insert(5, tb7);
                    }));
                }
            }
        }









        private ContextMenu LibMediaCreateContextMenu()
        {//AddImg
            ContextMenu ct = new ContextMenu();
            MenuItem mu = new MenuItem()
            {
                Header = (string)Resources.MergedDictionaries[1]["ParamsLibItemContextMenuItem1"],
                Icon = AddImg
            };
            mu.Click += MediatequeContextMenuClick;
            ct.Items.Add(mu);
            return ct;
        }

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

        private void MediatequeBuildNavigationPathClick(object sender, MouseButtonEventArgs e)
        {
            object[] ob = (object[])((TextBlock)sender).Tag;
            Folder v = (Folder)(ob[2]);
            MediatequeCurrentFolder = v;
            Debug.WriteLine(v.Name);
            MediatequeBuildNavigationPath(v);
            MediatequeBuildNavigationContent(v);
        }


        private void MediatequeBuildNavigationContent(Folder fold) {
            LibNavigationContent.Children.Clear();

            foreach (Folder fl in fold.Folders) { MediatequeBuildNavigationContentButton("folder", fl.Name, fl.Path, fl); }
            foreach (string fi in fold.Files) { MediatequeBuildNavigationContentButton("file", fi, fold.Path + SeparatorChar + fi); }
        }

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

        double MediatequeNavigationContentButtonClick_Last = 0;
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
                if (MediatequeNavigationContentButtonClick_Last + 1 > tmpt)
                {
                    Debug.WriteLine(((Grid)sender).Tag);
                    if (System.IO.File.Exists((string)re[1]))
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Open(new string[] { (string)re[1] });
                        }));
                    }
                }
            }
            MediatequeNavigationContentButtonClick_Last = tmpt;
        }

        public void MediatequeContextMenuClick(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("MediatequeContextMenuClick");
            MenuItem mi = (MenuItem)sender;
            ContextMenu ct = (ContextMenu)mi.Parent;
            object[] tab;
            try
            {
                Grid gr = (Grid)ct.PlacementTarget;
                tab = (object[])gr.Tag;
            }
            catch
            {
                TextBlock tx = (TextBlock)ct.PlacementTarget;
                tab = (object[])tx.Tag;
            }

            if ((string)tab[0] == "folder") {
                List<string> paths = new List<string>();
                Folder fold = (Folder)tab[2];
                paths = MediatequeContextMenuClickParseFolder(fold, paths);
                Debug.WriteLine(JsonConvert.SerializeObject(paths.ToArray()));
                Open(paths.ToArray());
            }
            else if ((string)tab[0] == "file")
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Open(new string[] { (string)tab[1] });
                }));
            }
        }

        private List<string> MediatequeContextMenuClickParseFolder(Folder fold, List<string> tab)
        {
            if (fold.Folders != null)
            {
                foreach (Folder fl in fold.Folders)
                {
                    tab = MediatequeContextMenuClickParseFolder(fl, tab);
                }
            }
            foreach (string fi in fold.Files) {
                tab.Add(fi);
            }
            return tab;
        }


    }
}