using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Data;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace MediaPlayer
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
        private Folder LibFolders = new Folder();
        private Folder LibCurrentFolder = null;
        private string LibCurrentFolderS = null;
        FileSystemWatcher LibFolderWatcher = null;

        private void ScanLibrary()
        {
            if (Settings.LibFolder != null)
            {
                if (System.IO.Directory.Exists(Settings.LibFolder))
                {
                    if (LibFolderWatcher == null)
                    {
                        LibFolderWatcher = new FileSystemWatcher();
                        LibFolderWatcher.Path = Settings.LibFolder;
                        LibFolderWatcher.IncludeSubdirectories = true;
                        LibFolderWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                        LibFolderWatcher.Filter = "*.*";
                        LibFolderWatcher.Changed += new FileSystemEventHandler(OnLibraryChanged);
                        LibFolderWatcher.EnableRaisingEvents = true;
                    }

                    //LibTreeView.Items.Clear();
                    LastLibScan = UnixTimestamp();
                    DirectoryInfo di = new DirectoryInfo(Settings.LibFolder);
                    LibFolders = new Folder() { Name = di.Name, Path = di.FullName };
                    LibLoadFiles(Settings.LibFolder, LibFolders);
                    LibLoadSubDirectories(Settings.LibFolder, LibFolders);
                    LibBuildNavigationPath(LibCurrentFolder ?? LibFolders);
                    LibBuildNavigationContent(LibCurrentFolder ?? LibFolders);
                }
            }
        }

        private void OnLibraryChanged(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine(e.Name);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ScanLibrary();
            }));
        }

        private void LibLoadSubDirectories(string dir, Folder fold)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                if (di.Name.StartsWith('.')) { continue; }
                Folder fold2 = new Folder() { Name = di.Name, Path = di.FullName, Parent = fold };
                LibLoadFiles(subdirectory, fold2);
                LibLoadSubDirectories(subdirectory, fold2);
                if (LibCurrentFolderS == di.FullName) { LibCurrentFolder = fold2; }
                fold.Folders.Add(fold2);
            }
        }

        private void LibLoadFiles(string dir, Folder fold)
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
                    fold.Files.Add(new FileInfo(file).Name);
                }
            }
        }

        private void LibBuildNavigationPath(Folder fold) {
            LibNavigationPathContener.Children.Clear();
            LibCurrentFolder = fold;
            LibCurrentFolderS = fold.Path;
            string basePath = "Home/"+((fold.Path == Settings.LibFolder)?"": fold.Path.Replace(Settings.LibFolder, "").Replace(System.IO.Path.DirectorySeparatorChar, '/').Replace("//", "/"));
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
                    tb3.MouseDown += Tb3_MouseDown;

                    ContextMenu ct = new ContextMenu();
                    MenuItem mu = new MenuItem() { Header = (string)Resources.MergedDictionaries[1]["ParamsLibItemContextMenuItem1"] };
                    mu.Click += LibContextMenuClick;
                    ct.Items.Add(mu);
                    tb3.ContextMenu = ct;

                    LibNavigationPathContener.Children.Add(tb3);
                    l1 += 1;
                }
            }

        }

        private void Tb3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            object[] ob = (object[])((TextBlock)sender).Tag;
            Folder v = (Folder)(ob[2]);
            LibCurrentFolder = v;
            Debug.WriteLine(v.Name);
            LibBuildNavigationPath(v);
            LibBuildNavigationContent(v);
        }


        private void LibBuildNavigationContent(Folder fold) {
            LibNavigationContent.Children.Clear();

            foreach (Folder fl in fold.Folders) { LibBuildNavigationContentButton("folder", fl.Name, fl.Path, fl); }
            foreach (string fi in fold.Files) { LibBuildNavigationContentButton("file", fi, fold.Path + System.IO.Path.DirectorySeparatorChar + fi); }
        }

        private void LibBuildNavigationContentButton(string type, string name, string path, Folder fold = null)
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
            gr.MouseLeftButtonDown += LibNavigationContentButtonClick;

            ContextMenu ct = new ContextMenu();
            MenuItem mu = new MenuItem() { Header = (string)Resources.MergedDictionaries[1]["ParamsLibItemContextMenuItem1"] };
            mu.Click += LibContextMenuClick;
            ct.Items.Add(mu);
            gr.ContextMenu = ct;

            br.Child = gr;
            LibNavigationContent.Children.Add(br);
        }

        double Last_LibNavigationContentButtonClick = 0;
        private void LibNavigationContentButtonClick(object sender, MouseButtonEventArgs e)
        {
            double tmpt = UnixTimestamp();
            object[] re = (object[])((Grid)sender).Tag;
            if ((string)re[0] == "folder")
            {
                LibBuildNavigationPath((Folder)re[2]);
                LibBuildNavigationContent((Folder)re[2]);
            }
            else
            {
                if (Last_LibNavigationContentButtonClick + 1 > tmpt)
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
            Last_LibNavigationContentButtonClick = tmpt;
        }

        public void LibContextMenuClick(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("LibContextMenuClick");
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
                paths = LibContextMenuClickParseFolder(fold, paths);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Open(paths.ToArray());
                }));
            }
            else if ((string)tab[0] == "file")
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Open(new string[] { (string)tab[1] });
                }));
            }
        }

        private List<string> LibContextMenuClickParseFolder(Folder fold, List<string> tab)
        {
            if (fold.Folders != null)
            {
                foreach (Folder fl in fold.Folders)
                {
                    tab = LibContextMenuClickParseFolder(fl, tab);
                }
            }
            foreach (string fi in fold.Files) {
                tab.Add(fold.Path + System.IO.Path.DirectorySeparatorChar + fi);
            }
            return tab;
        }





        /*
        private void LibTreeView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void LibTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string file = (string)((TreeViewItem)LibTreeView.SelectedItem).Tag;
            if (System.IO.File.Exists(file))
            {
                if (PlayListView.Items.Count > 0)
                {
                    if (player.IsPlaying()) { UpdateListView(player.MediaInfo(file, false), true); }
                    else { updatePlaylist(UpdateListView(player.MediaInfo(file, false)), true); }
                }
                else { fileOpen(file); }
            }
            else if (System.IO.Directory.Exists(file))
            {
                foreach (TreeViewItem item in ((TreeViewItem)LibTreeView.SelectedItem).Items)
                {
                    file = (string)item.Tag;
                    if (System.IO.File.Exists(file))
                    {
                        if (PlayListView.Items.Count > 0) { if (player.IsPlaying()) { UpdateListView(player.MediaInfo(file, false), true); } else { fileOpen(file); } }
                        else { fileOpen(file); }
                    }
                }
            }
        }
        */

    }
}