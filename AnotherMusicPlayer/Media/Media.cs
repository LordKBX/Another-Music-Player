using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Media.Imaging;
using System.Linq;

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
        /// <summary> status if currently MediatequeScanning Library </summary>
        private bool MediatequeScanning = false;

        /// <summary> list of newly scanned files(absolute path) </summary>
        List<string> MediatequeScanedFiles = new List<string>();

        List<string> MediatequeCacheQuerys = new List<string>();

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

        /// <summary> GetCallback when Mediateque Watcher detect a change </summary>
        private void MediatequeChanged(object source, FileSystemEventArgs e)
        {
            //Debug.WriteLine(e.Name);
            Dispatcher.BeginInvoke(new Action(() => { MediatequeScan(); }));
        }

        /// <summary> Get Basic Metadata of a media file if stored in database </summary>
        private Dictionary<string, object> DatabaseFileInfo(string path) {
            try
            {
                if (!MediatequeScanning) {
                    Dictionary<string, Dictionary<string, object>> rt = DatabaseQuery("SELECT * FROM files WHERE Path='" + DatabaseEscapeString(path) + "' ORDER BY Path ASC", "Path");

                    if (Int32.Parse((string)rt[path]["Size"]) <= 0) 
                    {
                        FileInfo fi = new FileInfo(path);
                        PlayListViewItem item = player.MediaInfo(path, false);

                        DatabaseQuerys(new string[]{"UPDATE files SET Name='" + DatabaseEscapeString(item.Name??fi.Name)
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
                            + "' WHERE Path='" + DatabaseEscapeString(path) + "'" }, true);

                        rt = DatabaseQuery("SELECT * FROM files WHERE Path='"+ DatabaseEscapeString(path) + "' ORDER BY Path ASC", "Path");
                        return rt[path];
                    }
                    return rt[path];
                }
            }
            catch { }
            return null;
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

        /// <summary> Create context menu for objects in Library </summary>
        private ContextMenu LibMediaCreateContextMenu(string type = "folder")
        {//ContextMenuItemImage_add
            ContextMenu ct = new ContextMenu();
            MenuItem mu = new MenuItem()
            {
                Header = GetTaduction((type == "folder")? "ParamsLibItemContextMenuItem_AddFolderToPlayingQueue" : ((type == "file") ? "ParamsLibItemContextMenuItem_AddTrackToPlayingQueue" : "ParamsLibItemContextMenuItem_AddAlbumToPlayingQueue")),
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
            LibraryFiltersMode.SelectedIndex = 0;
            LibraryFiltersGenreList.SelectedIndex = 0;
        }

        StackPanel MediatequeBuildNavigationContentBlockssPanel = null;
        /// <summary> Fill the Content zone in Library pannel with folders and files </summary>
        private void MediatequeBuildNavigationContent(Folder fold) {
            LibNavigationContent.Children.Clear();
            LibNavigationContentB.Visibility = Visibility.Visible;
            LibNavigationContent2B.Visibility = Visibility.Collapsed;
            LibraryFiltersMode.SelectedIndex = 0;
            LibraryFiltersGenreList.SelectedIndex = 0;
            LibraryFiltersPaginationBlock.Visibility = Visibility.Collapsed;

            ContextMenu ct = new ContextMenu();

            if (fold.Parent != null)
            {
                MenuItem mu0 = new MenuItem()
                {
                    Header = GetTaduction("ParamsLibItemContextMenuItem_GetBack"),
                    Icon = ContextMenuItemImage_back,
                    Tag = fold.Parent
                };
                mu0.Click += (sender, e) => {
                    MediatequeBuildNavigationPath((Folder)((MenuItem)sender).Tag);
                    MediatequeBuildNavigationContent((Folder)((MenuItem)sender).Tag);
                };
                ct.Items.Add(mu0);
            }

            MenuItem mu1 = new MenuItem()
            {
                Header = GetTaduction("ParamsLibItemContextMenuItem_GenericAddToPlayingQueue"),
                Icon = ContextMenuItemImage_add,
                Tag = fold
            };
            mu1.Click += (sender, e) => {
                List<string> paths = new List<string>();
                paths = MediatequeCreateList((Folder)((MenuItem)sender).Tag, paths);
                Dispatcher.BeginInvoke(new Action(() => { Open(paths.ToArray()); }));
            };
            ct.Items.Add(mu1);

            LibNavigationContentScroll.ContextMenu = ct;

            try
            {
                foreach (Folder fl in fold.Folders) { MediatequeBuildNavigationContentButton("folder", fl.Name, fl.Path, fl); }

                Debug.WriteLine(fold.Path);
                Dictionary<string, Dictionary<string, object>> files = DatabaseQuery("SELECT * FROM files WHERE Path LIKE '" + DatabaseEscapeString(fold.Path) + SeparatorChar + "%' ORDER BY Album, Disc, Track, Name, Path ASC", "Path");

                List<string> endFiles = new List<string>();
                foreach (string file in files.Keys.ToArray()) {
                    if (!file.Replace(fold.Path + SeparatorChar, "").Contains(SeparatorChar)) { endFiles.Add(file); }
                }

                MediatequeBuildNavigationContentBlockssPanel = new StackPanel(){ Orientation = Orientation.Vertical, Width = LibNavigationContent.ActualWidth };
                LibNavigationContent.Children.Add(MediatequeBuildNavigationContentBlockssPanel);
                MediatequeBuildNavigationContentBlocks(endFiles.ToArray(), MediatequeBuildNavigationContentBlockssPanel);

            }
            catch { }
        }

        private void MediatequeBuildNavigationContentBlocks(string[] files, StackPanel contener, bool uniqueDir = true)
        {
            Button br = null;
            List<string> brList = null;
            string album = null;
            uint disc = 0;
            StackPanel lastPanel = null;
            BitmapImage defaultCover = Bimage("CoverImg");
            if (uniqueDir) {
                string folder = files[0].Substring(0, files[0].LastIndexOf(SeparatorChar));
                defaultCover = (System.IO.File.Exists(folder + SeparatorChar + "Cover.jpg")) ? new BitmapImage(new Uri(folder + SeparatorChar + "Cover.jpg"))
                    : ((System.IO.File.Exists(folder + SeparatorChar + "Cover.png")) ? new BitmapImage(new Uri(folder + SeparatorChar + "Cover.png")) : Bimage("CoverImg"));
            }

            foreach (string fil in files)
            {
                PlayListViewItem it = GetMediaInfo(fil);
                bool nocover = false;
                string al = it.Album; if (it.Album == null || it.Album.Trim() == "") {
                    if (uniqueDir) { al = "<UNKWON ALBUM>"; }
                    else { al = fil.Substring(0, fil.LastIndexOf(SeparatorChar)); }
                     nocover = true;
                }

                string name = it.Name;
                if (name == null || name == "")
                {
                    string[] tb = (it.Path).Split(System.IO.Path.DirectorySeparatorChar);
                    name = tb[tb.Length - 1];
                }


                if (album != al)
                {
                    if (br != null)
                    {
                        br.Tag = new object[]{ "album", brList.ToArray() };
                    }
                    brList = new List<string>();
                    disc = 0;
                    br = new Button()
                    {
                        Style = (Style)Resources.MergedDictionaries[0]["BtnStyle2"]
                    };
                    br.ContextMenu = LibMediaCreateContextMenu("album");
                    Grid gr = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
                    gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150, GridUnitType.Pixel) });
                    gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });

                    System.Windows.Controls.Image im = new System.Windows.Controls.Image()
                    {
                        Source = (Settings.MemoryUsage == 1)?
                            (((nocover)?null:player.MediaPicture(fil, true, 150, 150, false)) ?? defaultCover):
                            (((nocover) ? null : player.MediaPicture(fil, true, 50, 50, false)) ?? defaultCover),
                        VerticalAlignment = VerticalAlignment.Top, Style = (Style)Resources.MergedDictionaries[0]["HQImg"]
                    };
                    if (im.Source != Bimage("CoverImg") && Settings.MemoryUsage == 1)
                    {
                        WrapPanel p = new WrapPanel() { Orientation = Orientation.Vertical };
                        Image imp = new Image() { Style = (Style)Resources.MergedDictionaries[0]["HQImg"] };
                        imp.Source = (((nocover) ? null : player.MediaPicture(fil, true)) ?? defaultCover);
                        p.Children.Add(imp);
                        im.ToolTip = p;
                    }
                    gr.Children.Add(im); Grid.SetColumn(im, 0);

                    StackPanel st1 = new StackPanel() { Orientation = Orientation.Vertical, Margin = new Thickness(5, 0, 0, 0) };
                    st1.Children.Add(new AccessText()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        TextAlignment = TextAlignment.Left,
                        Text = al,
                        FontWeight = FontWeights.Bold
                    });


                    lastPanel = st1;
                    gr.Children.Add(st1); Grid.SetColumn(st1, 1);
                    br.Content = gr;
                    album = al;
                    if (contener != null) { contener.Children.Add(br); }
                }
                if (disc != it.Disc && it.DiscCount > 1)
                {
                    disc = it.Disc;
                    lastPanel.Children.Add(new AccessText()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        TextAlignment = TextAlignment.Left,
                        Text = "Disc " + it.Disc,
                        FontStyle = FontStyles.Italic,
                        Margin = new Thickness(0, 5, 0, 0)
                    });
                }


                Button btn = new Button()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Content = new AccessText()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Left,
                        Text = NormalizeNumber((int)it.Track, ("" + it.TrackCount).Length) + ". " + name,
                        FontStyle = FontStyles.Normal,
                        Margin = new Thickness(2)
                    },
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Top,
                    FontStyle = FontStyles.Normal,
                    Tag = new object[] { "file", it.Path },
                    Cursor = Cursors.Hand
                };
                btn.Click += MediatequeNavigationContentButtonClick;
                btn.ContextMenu = LibMediaCreateContextMenu("file");
                lastPanel.Children.Add(btn);
                brList.Add(it.Path);
            }

            if (br != null) { br.Tag = new object[] { "album", brList.ToArray() }; }
        }

        /// <summary> Create button for the Content zone in Library pannel </summary>
        private void MediatequeBuildNavigationContentButton(string type, string name, string path, Folder fold = null)
        {
            //Border br = new Border(); br.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemBorder"];
            Button bt = new Button();
            bt.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItem"];
            Grid gr = new Grid();
            gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
            gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });

            PlayListViewItem it = GetMediaInfo(path);
            BitmapImage bi = player.MediaPicture(path);

            Image image = new Image();
            if (type == "folder") { 
                image.Source = Bimage("OpenButtonImg"); 
            }
            if (type == "file") {
                image.Source = (bi ?? Bimage("CoverImg"));


                string Artists = "";
                if (it.Performers != null && it.Performers.Trim() != "") { Artists += it.Performers; }
                if (it.Performers != null && it.Performers.Trim() != "") { if (Artists != null && Artists != "") { Artists += ", "; };  Artists += it.Composers; }

                if(Settings.MemoryUsage == 1)
                {
                    WrapPanel p = new WrapPanel() { Orientation = Orientation.Vertical };
                    Image imp = new Image() { Style = (Style)Resources.MergedDictionaries[0]["HQImg"] };
                    imp.Source = (bi ?? Bimage("CoverImg"));
                    imp.MaxHeight = imp.MaxWidth = 300;
                    p.Children.Add(imp);
                    p.Children.Add(new AccessText() { MaxWidth = 300, TextWrapping = TextWrapping.WrapWithOverflow, Text = GetTaduction("Title2") + " " + ((it != null) ? (it.Name ?? name) : name) });
                    if (it.Album != null && it.Album.Trim() != "")
                    {
                        p.Children.Add(new AccessText() { MaxWidth = 300, TextWrapping = TextWrapping.WrapWithOverflow, Text = GetTaduction("Album2") + " " + it.Album });
                    }

                    if (Artists != "") { p.Children.Add(new AccessText() { MaxWidth = 300, TextWrapping = TextWrapping.WrapWithOverflow, Text = GetTaduction("Artist2") + " " + Artists }); }
                    if (it.Genres != null && it.Genres.Trim() != "") { p.Children.Add(new AccessText() { TextWrapping = TextWrapping.WrapWithOverflow, Text = GetTaduction("Genres2") + " " + it.Genres }); }
                    p.Children.Add(new AccessText() { TextWrapping = TextWrapping.WrapWithOverflow, Text = GetTaduction("Duration2") + " " + it.DurationS });

                    bt.ToolTip = p;
                }
            }
            image.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemImg"];
            gr.Children.Add(image);

            string txname = (it != null) ? (it.Name ?? name) : name;
            if (txname.Length > 30) txname = txname.Substring(0, 30) + "...";
            AccessText tx = new AccessText() { TextWrapping = TextWrapping.WrapWithOverflow, MaxHeight = 45, Text = txname };
            tx.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemText"];
            //gr.Children.Add(tx);
            //Grid.SetRow(tx, 1);

            Viewbox vb = new Viewbox();
            vb.Child = tx;
            vb.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemViewBox"];
            gr.Children.Add(vb);
            Grid.SetRow(vb, 1);

            bt.Tag = new object[] { type, path, fold };
            bt.Click += MediatequeNavigationContentButtonClick;
            bt.ContextMenu = LibMediaCreateContextMenu();

            //br.Child = gr;
            bt.Content = gr;
            LibNavigationContent.Children.Add(bt);
        }

        /// <summary> Library pannel Content button last time click </summary>
        double MediatequeNavigationContentButtonClick_LastTime = 0;
        /// <summary> Library pannel Content button last reference click </summary>
        string MediatequeNavigationContentButtonClick_LastRef = "";
        /// <summary> Click Callback content button in Library pannel </summary>
        private void MediatequeNavigationContentButtonClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MediatequeNavigationContentButtonClick");
            double tmpt = UnixTimestamp();
            object[] re = (object[])((Button)sender).Tag;
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
                            Open(new string[] { (string)re[1] }, true);
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
            Debug.WriteLine("MediatequeCT_Open");
            MenuItem mi = (MenuItem)sender;
            ContextMenu ct = (ContextMenu)mi.Parent;
            object[] tab;
            try
            {   // With ContextMenu from Library pannel Content zone
                Button gr = (Button)ct.PlacementTarget;
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
                Dispatcher.BeginInvoke(new Action(() => { Open(paths.ToArray(), false); }));
            }
            else if ((string)tab[0] == "file")
            {
                Dispatcher.BeginInvoke(new Action(() => { Open(new string[] { (string)tab[1] }, false); }));
            }
            else if ((string)tab[0] == "album")
            {
                Dispatcher.BeginInvoke(new Action(() => { Open((string[])tab[1], false); }));
            }
        }

        /// <summary> Record Playing queue in database </summary>
        public void UpdateRecordedQueue()
        {
            DatabaseQuery("DELETE FROM queue;");
            int index = 1;
            List<string> querys = new List<string>();
            foreach (string[] line in PlayList)
            {
                string query = "INSERT INTO queue(MIndex, Path1, Path2) VALUES('";
                query += NormalizeNumber(index, 10) + "','";
                query += DatabaseEscapeString(line[0]) + "',";
                query += ((line[1] == null) ? "NULL" : "'" + DatabaseEscapeString(line[1]) + "'");
                query += ")";
                index += 1;
                querys.Add(query);
            }
            DatabaseQuerys(querys.ToArray());
        }

    }
}