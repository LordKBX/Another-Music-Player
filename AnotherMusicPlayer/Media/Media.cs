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

    /// <summary> Class AsyncCoverLoadPacket for storing required data </summary>
    public class AsyncCoverLoadPacket
    {
        public bool nocover = false;
        public System.Windows.Controls.Image sender = null;
        public string coverPath = null;
        public BitmapImage defaultCover = null;
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
            Debug.WriteLine("MediatequeChanged => "+e.Name);
            bdd.UpdateFileAsync(Settings.LibFolder + SeparatorChar + e.Name, true);
            //Dispatcher.InvokeAsync(new Action(() => { MediatequeScan(); }));
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
            //Debug.WriteLine("--> LibMediaCreateContextMenu <--");
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
            if(fold.Path == null) { fold.Path = Settings.LibFolder; }
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
        private void MediatequeBuildNavigationContent(Folder fold)
        {
            if (fold.Path == null) { fold.Path = Settings.LibFolder; }
            if (!isLoading() && !MediatequeScanning) { setLoadingState(true, "Loading"); }
            //setLoadingState(false);
            //return;
            //Debug.WriteLine("--> MediatequeBuildNavigationContent <--");
            try
            {
                if(LibNavigationContent.Children.Count>0)LibNavigationContent.Children.Clear();
                LibNavigationContentB.Visibility = Visibility.Visible;
                LibNavigationContent2B.Visibility = Visibility.Collapsed;
                LibraryFiltersMode.SelectedIndex = 0;
                LibraryFiltersGenreList.SelectedIndex = 0;
                LibraryFiltersPaginationBlock.Visibility = Visibility.Collapsed;

                MediatequeBuildNavigationContentBlockssPanel = new StackPanel() { Orientation = Orientation.Vertical, Width = LibNavigationContent.ActualWidth, Visibility = Visibility.Visible };
                LibNavigationContent.Children.Add(MediatequeBuildNavigationContentBlockssPanel);

                foreach (Folder fl in fold.Folders) { 
                    MediatequeBuildNavigationContentButton("folder", fl.Name, fl.Path, fl); 
                }

                _ = Dispatcher.InvokeAsync(new Action(() =>
                {
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
                        paths = MediatequeCreateList((Folder)((MenuItem)sender).Tag, paths).Distinct().ToList();
                        Dispatcher.BeginInvoke(new Action(() => { Open(paths.ToArray()); }));
                    };
                    ct.Items.Add(mu1);
                    LibNavigationContentScroll.ContextMenu = ct;
                }));

                Dictionary<string, Dictionary<string, object>> files = bdd.DatabaseQuery("SELECT * FROM files WHERE Path LIKE '" + Database.DatabaseEscapeString(fold.Path) + SeparatorChar + "%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");

                List<string> endFiles = new List<string>();
                foreach (string file in files.Keys.ToArray()) {
                    string fi = file.Replace(fold.Path + SeparatorChar, "");
                    if (!fi.Contains(SeparatorChar)) { 
                        endFiles.Add(file);
                    }
                }
                _ = Dispatcher.BeginInvoke(new Action(() =>
                {
                    MediatequeBuildNavigationContentBlocks(endFiles.ToArray(), MediatequeBuildNavigationContentBlockssPanel);
                    LibNavigationContentScroll.ScrollToTop();
                }));
            }
            catch {
                Debug.WriteLine("--> MediatequeBuildNavigationContent ERROR <--");
            }
        }

        private void MediatequeBuildNavigationContentBlocks(string[] files, StackPanel contener, bool uniqueDir = true)
        {
            //Debug.WriteLine("--> MediatequeBuildNavigationContentBlocks START <--");
            while (files.Length > 0 && files[0].Length == 0) { files = files.Where(w => w != files[0]).ToArray(); }
            if (files.Length == 0) { setLoadingState(false); return; }
            if (contener == null) { setLoadingState(false); return; }
            try
            {
                BitmapImage defaultCover = Bimage("CoverImg");
                if (uniqueDir)
                {
                    string folder = files[0].Substring(0, files[0].LastIndexOf(SeparatorChar));
                    string[] t = System.IO.Directory.GetFiles(folder);
                    foreach (string file in t)
                    {
                        if (file.EndsWith("Cover.jpg") || file.EndsWith("Cover.png")) { defaultCover = new BitmapImage(new Uri(file)); }
                    }
                }

                Dictionary<string, Dictionary<string, object>> dataFiles = bdd.DatabaseFilesInfo(files);
                Dictionary<string, Dictionary<uint, Dictionary<string, PlayListViewItem>>> tab = new Dictionary<string, Dictionary<uint, Dictionary<string, PlayListViewItem>>>();
                foreach (string fil in files)
                {
                    if(dataFiles.ContainsKey(fil))
                    {
                        PlayListViewItem it = DatabaseItemToPlayListViewItem(dataFiles[fil]);
                        if (!tab.ContainsKey(it.Album)) { tab.Add(it.Album, new Dictionary<uint, Dictionary<string, PlayListViewItem>>()); }
                        if (!tab[it.Album].ContainsKey(it.Disc)) { tab[it.Album].Add(it.Disc, new Dictionary<string, PlayListViewItem>()); }
                        if (!tab[it.Album][it.Disc].ContainsKey(it.Path)) { tab[it.Album][it.Disc].Add(it.Path, it); }
                    }
                    else {
                        Debug.WriteLine("--> MediatequeBuildNavigationContentBlocks ERROR - FILE '"+ fil + "' do not exist <--");
                    }
                }

                foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, PlayListViewItem>>> albumT in tab)
                {
                    bool nocover = false;
                    string coverPath = albumT.Value.Values.First().First().Value.Path;
                    string al = albumT.Key;
                    if (al == null || al.Trim() == "")
                    {
                        if (uniqueDir) { al = "<UNKWON ALBUM>"; }
                        else { al = albumT.Value.Values.First().First().Value.Name; }
                        nocover = true;
                    }

                    List<string> brList = new List<string>();
                    Button br = new Button()
                    {
                        Style = (Style)Resources.MergedDictionaries[0]["BtnStyle2"]
                    };
                    br.ContextMenu = LibMediaCreateContextMenu("album");
                        Grid gr = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
                        gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150, GridUnitType.Pixel) });
                        gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });
                            System.Windows.Controls.Image im = new System.Windows.Controls.Image()
                            {
                                VerticalAlignment = VerticalAlignment.Top,
                                Style = (Style)Resources.MergedDictionaries[0]["HQImg"]
                            };
                            im.Source = (Settings.MemoryUsage == 1) ?
                                ((player.MediaPicture(coverPath, true, 150, 150, false)) ?? defaultCover) :
                                ((player.MediaPicture(coverPath, true, 50, 50, false)) ?? defaultCover);
                            if (im.Source != Bimage("CoverImg") && Settings.MemoryUsage == 1)
                            {
                                WrapPanel p = new WrapPanel() { Orientation = Orientation.Vertical };
                                Image imp = new Image() { Style = (Style)Resources.MergedDictionaries[0]["HQImg"] };
                                imp.Source = (((nocover) ? null : player.MediaPicture(coverPath, true)) ?? defaultCover);
                                p.Children.Add(imp);
                                im.ToolTip = p;
                            }

                            StackPanel st1 = new StackPanel() { 
                                Orientation = Orientation.Vertical, 
                                HorizontalAlignment= HorizontalAlignment.Left, 
                                Margin = new Thickness(5, 0, 0, 0) 
                            };
                            st1.Children.Add(new AccessText()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                TextAlignment = TextAlignment.Left,
                                Text = al,
                                FontWeight = FontWeights.Bold,
                                MaxWidth = 200,
                                TextTrimming = TextTrimming.CharacterEllipsis
                            });
                            foreach (KeyValuePair<uint, Dictionary<string, PlayListViewItem>> discT in albumT.Value) 
                            {
                                if (albumT.Value.Count > 1)
                                st1.Children.Add(new AccessText()
                                {
                                    HorizontalAlignment = HorizontalAlignment.Stretch,
                                    TextAlignment = TextAlignment.Left,
                                    Text = "Disc " + discT.Key,
                                    FontStyle = FontStyles.Italic,
                                    Margin = new Thickness(0, 5, 0, 0)
                                });

                                List<KeyValuePair<string, PlayListViewItem>> myList = discT.Value.ToList();
                                myList.Sort((pair1, pair2) =>
                                    {
                                    uint trackcmp = pair1.Value.Track - pair2.Value.Track;
                                    if (trackcmp == 0) { return pair1.Value.Name.CompareTo(pair2.Value.Name); }
                                    else { return (int)trackcmp; }
                                    }
                                );
                                WrapPanel st2 = new WrapPanel()
                                {
                                    HorizontalAlignment = HorizontalAlignment.Stretch
                                };
                                foreach (KeyValuePair<string, PlayListViewItem> trackT in myList)
                                {
                                    brList.Add(trackT.Value.Path);
                                    string textName = ((trackT.Value.Track==0)?"":NormalizeNumber((int)trackT.Value.Track, ("" + trackT.Value.TrackCount).Length) + ". ") + trackT.Value.Name;
                                    Button btn = new Button()
                                    {
                                        HorizontalAlignment = HorizontalAlignment.Stretch,
                                        Content = new AccessText()
                                        {
                                            HorizontalAlignment = HorizontalAlignment.Left,
                                            VerticalAlignment = VerticalAlignment.Stretch,
                                            TextAlignment = TextAlignment.Left,
                                            Text = textName,
                                            FontStyle = FontStyles.Normal,
                                            Margin = new Thickness(2),
                                            TextWrapping = TextWrapping.Wrap
                                        },
                                        HorizontalContentAlignment = HorizontalAlignment.Left,
                                        VerticalContentAlignment = VerticalAlignment.Center,
                                        FontStyle = FontStyles.Normal,
                                        Tag = new object[] { "file", trackT.Value.Path },
                                        Cursor = Cursors.Hand,
                                        Width = 200, Height=50,
                                        //ToolTip = textName,
                                        Padding = new Thickness() { Bottom = 0, Left = 0, Right = 0, Top = 0 },
                                        Margin = new Thickness() { Bottom = 0, Left = 0, Right = 5, Top = 5 },
                                        Style = new Style()
                                    };
                                    //btn.Loaded += (object sender, RoutedEventArgs e) => {
                                    //    Button btn = (Button)sender;
                                    //    string path = (string)btn.Tag;
                                    //    Dispatcher.BeginInvoke(new Action(() =>
                                    //    {
                                    //        Dictionary<string, object> dataT = DatabaseFileInfo(path);
                                    //        ((AccessText)btn.Content).Text = NormalizeNumber(Convert.ToInt32(dataT["Track"]), ("" + dataT["TrackCount"]).Length) + ". " + (string)dataT["Name"];
                                    //    }));
                                    //};
                                    btn.Click += MediatequeNavigationContentButtonClick;
                                    btn.ContextMenu = LibMediaCreateContextMenu("file");
                                    st2.Children.Add(btn);
                                }
                                st1.Children.Add(st2);
                    }

                    gr.Children.Add(im); 
                    Grid.SetColumn(im, 0);
                    gr.Children.Add(st1); 
                    Grid.SetColumn(st1, 1);
                    br.Content = gr;
                    br.Tag = new object[] { "album", brList.ToArray() };
                    contener.Children.Add(br);
                    
                }
                dataFiles.Clear();
            }
            catch { Debug.WriteLine("--> MediatequeBuildNavigationContentBlocks ERROR <--"); }
            //Debug.WriteLine("--> MediatequeBuildNavigationContentBlocks END <--");
            setLoadingState(false);
        }

        /// <summary> Create button for the Content zone in Library pannel </summary>
        private void MediatequeBuildNavigationContentButton(string type, string name, string path, Folder fold = null)
        {
            //Debug.WriteLine("--> MediatequeBuildNavigationContentButton <--");
            try
            {
                //Border br = new Border(); br.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemBorder"];
                Button bt = new Button();
                bt.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItem"];
                Grid gr = new Grid();
                gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                gr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });

                PlayListViewItem it = GetMediaInfo(path);

                Image image = new Image();
                if (type == "folder")
                {
                    image.Loaded += (object sender, RoutedEventArgs e) => {
                        _ = Dispatcher.InvokeAsync(new Action(() =>
                        {
                            ((System.Windows.Controls.Image)sender).Source = Bimage("OpenButtonImg");
                        }));
                    };
                }
                if (type == "file")
                {

                    string Artists = "";
                    if (it.Performers != null && it.Performers.Trim() != "") { Artists += it.Performers; }
                    if (it.Performers != null && it.Performers.Trim() != "") { if (Artists != null && Artists != "") { Artists += ", "; }; Artists += it.Composers; }

                    bt.Loaded += (object sender, RoutedEventArgs e) => {
                        _ = Dispatcher.InvokeAsync(new Action(() =>
                        {
                            BitmapImage bi = player.MediaPicture(path);
                            ((Image)((Grid)((System.Windows.Controls.Button)sender).Content).Children[0]).Source = (bi ?? Bimage("CoverImg"));

                            if (Settings.MemoryUsage == 1)
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

                                ((System.Windows.Controls.Button)sender).ToolTip = p;
                            }
                        }));
                    };
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
            catch { Debug.WriteLine("--> MediatequeBuildNavigationContentButton ERROR <--"); }
        }

        /// <summary> Library pannel Content button last time click </summary>
        double MediatequeNavigationContentButtonClick_LastTime = 0;
        /// <summary> Library pannel Content button last reference click </summary>
        string MediatequeNavigationContentButtonClick_LastRef = "";
        /// <summary> Click Callback content button in Library pannel </summary>
        private void MediatequeNavigationContentButtonClick(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("MediatequeNavigationContentButtonClick");
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
            //Debug.WriteLine("MediatequeCT_Open");
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
                return;
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
                Dispatcher.BeginInvoke(new Action(() => { Open((string[])tab[1], false);  }));
            }
        }

    }
}