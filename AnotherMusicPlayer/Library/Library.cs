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
using System.Windows.Threading;
using System.Threading.Tasks;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        /// <summary> Parent object </summary>
        public MainWindow Parent;
        /// <summary> Database object </summary>
        public Database Bdd;
        /// <summary> Zone to display current path(relative to library folder) </summary>
        WrapPanel NavigatorBar;
        /// <summary> Zone to display content of current path </summary>
        WrapPanel NavigationContener;
        /// <summary> Zone to display content of current search results </summary>
        StackPanel SearchResultsContener;
        /// <summary> Scrollview of current path content display zone </summary>
        ScrollViewer NavigationContenerScoller;
        /// <summary> Scrollview of search results display zone </summary>
        ScrollViewer SearchResultsContenerScoller;
        /// <summary> Border Scrollview of current path content display zone </summary>
        Border NavigationContenerScollerBorder;
        /// <summary> Border Scrollview of search results display zone </summary>
        Border SearchResultsContenerScollerBorder;
        /// <summary> Combobox serving to select filtering/search mode </summary>
        ComboBox FilterSelector;
        /// <summary> Combobox serving to select genre for filtering/search mode </summary>
        ComboBox FilterGenreSelector;
        /// <summary> Input text zone serving to communication title searched for filtering/search mode </summary>
        TextBox FiltersSearchInput;
        /// <summary> Library root path </summary>
        string RootPath;

        /// <summary> Path Navigator builder object </summary>
        LibraryPathNavigator pathNavigator;
        /// <summary> Object Watcher for detecting Library Modifications </summary>
        private FileSystemWatcher Watcher = null;
        /// <summary> status if currently LibraryScanning Library </summary>
        private bool _Scanning = false;
        public bool Scanning
        {
            get { return _Scanning; }
            set { }
        }

        private string _CurrentPath = null;
        public string CurrentPath
        {
            get { return _CurrentPath; }
            set { }
        }

        /// <summary> Create a Library watcher </summary>
        private void CreateWatcher()
        {
            if (Watcher != null) { Watcher.Dispose(); }
            Watcher = new FileSystemWatcher();
            Watcher.Path = Settings.LibFolder;
            Watcher.IncludeSubdirectories = true;
            Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            Watcher.Filter = "*.*";
            Watcher.Changed += new FileSystemEventHandler(LibraryChanged);
            Watcher.EnableRaisingEvents = true;
        }

        /// <summary> Callback for when Library Watcher detect a change </summary>
        private void LibraryChanged(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine("LibraryChanged => " + e.Name);
            if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower()))
            { 
                Bdd.UpdateFileAsync(Settings.LibFolder + MainWindow.SeparatorChar + e.Name, true); 
            }
        }

        public Library(MainWindow parent, string root = null)
        {
            if (!UpdateRootPath((root == null) ? Settings.LibFolder : root)) { throw new InvalidDataException("Root Path Invalid !"); }
            Parent = parent;
            Bdd = Parent.bdd;
            NavigatorBar = Parent.LibibraryNavigationPathContener;
            NavigationContener = Parent.LibibraryNavigationContent;
            SearchResultsContener = Parent.LibibraryNavigationContent2;
            NavigationContenerScoller = Parent.LibibraryNavigationContentScroll;
            SearchResultsContenerScoller = Parent.LibibraryNavigationContentScroll2;
            NavigationContenerScollerBorder = Parent.LibibraryNavigationContentB;
            SearchResultsContenerScollerBorder = Parent.LibibraryNavigationContent2B;
            FilterSelector = Parent.LibraryFiltersMode;
            FilterGenreSelector = Parent.LibraryFiltersGenreList;
            FiltersSearchInput = Parent.LibraryFiltersSearchBox;

            pathNavigator = new LibraryPathNavigator(this, NavigatorBar, RootPath);

            Parent.bdd.DatabaseQuerys(new string[] { "UPDATE files SET Genres = NULL WHERE TRIM(Genres) = ''" }, true);

            LoadGenreList();

            FilterSelector.SelectionChanged += FilterSelector_SelectionChanged;
            FilterGenreSelector.SelectionChanged += FilterGenreSelector_SelectionChanged;
            FiltersSearchInput.KeyDown += FiltersSearchInput_KeyDown;

            CreateWatcher();
            //Scan();
            InvokeScan();
        }

        /// <summary> Update var RootPath and return if value is valid or not </summary>
        public bool UpdateRootPath(string root) { if (Directory.Exists(root)) { RootPath = root; return true; } else { return false; } }

        public void DisplayPath(string path = null) {
            if(path == null) { path = Settings.LibFolder; }
            if (!Directory.Exists(path)){ return; }
            _CurrentPath = path;

            NavigationContenerScollerBorder.Visibility = Visibility.Visible;
            SearchResultsContenerScollerBorder.Visibility = Visibility.Collapsed;

            FiltersSearchInput.Visibility = Visibility.Collapsed;
            FiltersSearchInput.Text = "";

            FilterSelector.SelectedIndex = 0;
            FilterGenreSelector.SelectedIndex = 0;
            FilterGenreSelector.Visibility = Visibility.Collapsed;

            pathNavigator.Display(path);
            SearchResultsContener.Children.Clear();
            NavigationContener.Children.Clear();
            NavigationContener.ContextMenu = null;
            NavigationContener.ContextMenu = MakeContextMenu(NavigationContener, "folder", (path != Settings.LibFolder) ? true : false);

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs) {
                if (Settings.LibFolderShowHiden == false)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden)) { continue; }
                }
                if (Settings.LibFolderShowUnixHiden == false)
                {
                    string name = dir.Replace(path, "").TrimStart(MainWindow.SeparatorChar);
                    if (name[0] == '.') { continue; }
                }
                string[] tab = dir.Split(MainWindow.SeparatorChar);
                LibraryFolderButton btn = new LibraryFolderButton(tab[tab.Length - 1], dir) { 
                    Style = Parent.FindResource("LibibraryNavigationContentItem") as Style
                };
                btn.Icon.Style = Parent.FindResource("LibibraryNavigationContentItemPackIcon") as Style;
                btn.Click += BtnFolder_Click;
                btn.ContextMenu = MakeContextMenu(btn, "folder");
                NavigationContener.Children.Add(btn);
            }

            Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE Path LIKE '" + Database.EscapeString(path) + MainWindow.SeparatorChar + "%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
            List<string> endFiles = new List<string>();
            foreach (string file in files.Keys.ToArray())
            {
                string fi = file.Replace(path + MainWindow.SeparatorChar, "");
                if (!fi.Contains(MainWindow.SeparatorChar))
                {
                    endFiles.Add(file);
                }
            }

            StackPanel panel = new StackPanel() { Orientation = Orientation.Vertical, Width = NavigationContener.ActualWidth, Visibility = Visibility.Visible };
            NavigationContener.Children.Add(panel);

            ContentBlocks(endFiles.ToArray(), panel);
            NavigationContenerScoller.ScrollToHome();
            Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() => {
                Parent.setLoadingState(false);
            }));
        }

        private async Task<bool> ContentBlocks(string[] files, StackPanel contener, bool uniqueDir = true)
        {
            Debug.WriteLine("--> ContentBlocks START <--");
            if (files.Length == 0) { Debug.WriteLine("--> ContentBlocks NO FILE 1 <--"); return false; }

            while (files.Length > 0 && files[0].Length == 0) { files = files.Where(w => w != files[0]).ToArray(); }
            if (files.Length == 0) { Debug.WriteLine("--> ContentBlocks NO FILE 2 <--"); return false; }
            try
            {
                BitmapImage defaultCover = MainWindow.Bimage("CoverImg");
                if (uniqueDir)
                {
                    string folder = files[0].Substring(0, files[0].LastIndexOf(MainWindow.SeparatorChar));
                    string[] t = System.IO.Directory.GetFiles(folder);
                    foreach (string file in t)
                    {
                        if (file.EndsWith("Cover.jpg") || file.EndsWith("Cover.png")) { defaultCover = new BitmapImage(new Uri(file)); }
                    }
                }

                Dictionary<string, Dictionary<string, object>> dataFiles = Bdd.DatabaseFilesInfo(files);
                Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> tab = new Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>>();
                foreach (string fil in files)
                {
                    if (dataFiles.ContainsKey(fil))
                    {
                        MediaItem it = MainWindow.DatabaseItemToMediaItem(dataFiles[fil]);
                        if (!tab.ContainsKey(it.Album)) { tab.Add(it.Album, new Dictionary<uint, Dictionary<string, MediaItem>>()); }
                        if (!tab[it.Album].ContainsKey(it.Disc)) { tab[it.Album].Add(it.Disc, new Dictionary<string, MediaItem>()); }
                        if (!tab[it.Album][it.Disc].ContainsKey(it.Path)) { tab[it.Album][it.Disc].Add(it.Path, it); }
                    }
                    else
                    {
                        Debug.WriteLine("--> LibraryBuildNavigationContentBlocks ERROR - FILE '" + fil + "' do not exist <--");
                    }
                }

                foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> albumT in tab)
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
                        Style = Parent.FindResource("BtnStyle2") as Style
                    };
                    br.ContextMenu = MakeContextMenu(br, "album");
                    Grid gr = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
                    gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150, GridUnitType.Pixel) });
                    gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });
                    System.Windows.Controls.Image im = new System.Windows.Controls.Image()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        Style = Parent.FindResource("HQImg") as Style
                    };
                    im.Source = (Settings.MemoryUsage == 1) ?
                        ((FilesTags.MediaPicture(coverPath, Bdd, true, 150, 150, false)) ?? defaultCover) :
                        ((FilesTags.MediaPicture(coverPath, Bdd, true, 50, 50, false)) ?? defaultCover);
                    if (im.Source != MainWindow.Bimage("CoverImg") && Settings.MemoryUsage == 1)
                    {
                        WrapPanel p = new WrapPanel() { Orientation = Orientation.Vertical };
                        Image imp = new Image() { Style = Parent.FindResource("HQImg") as Style };
                        imp.Source = (((nocover) ? null : FilesTags.MediaPicture(coverPath, Bdd, true)) ?? defaultCover);
                        p.Children.Add(imp);
                        im.ToolTip = p;
                    }

                    StackPanel st1 = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(5, 0, 0, 0)
                    };
                    st1.Children.Add(new AccessText()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        TextAlignment = TextAlignment.Left,
                        Text = al,
                        ToolTip = al,
                        FontWeight = FontWeights.Bold,
                        //MaxWidth = 200,
                        TextTrimming = TextTrimming.CharacterEllipsis
                    });
                    foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> discT in albumT.Value)
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

                        List<KeyValuePair<string, MediaItem>> myList = discT.Value.ToList();
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
                        foreach (KeyValuePair<string, MediaItem> trackT in myList)
                        {
                            brList.Add(trackT.Value.Path);
                            string textName = ((trackT.Value.Track == 0) ? "" : MainWindow.NormalizeNumber((int)trackT.Value.Track, ("" + trackT.Value.TrackCount).Length) + ". ") + trackT.Value.Name;
                            Button btn = new Button()
                            {
                                Content = new AccessText()
                                {
                                    Text = textName,
                                    Style = Parent.FindResource("LibibraryNavigationContentItemTrackButtonAccessText") as Style
                                },
                                Tag = trackT.Value.Path,
                                //ToolTip = textName,
                                Style = Parent.FindResource("LibibraryNavigationContentItemTrackButton") as Style
                            };
                            //btn.Click += BtnTrack_Click;
                            btn.MouseDoubleClick += BtnTrack_MouseDoubleClick;
                            btn.ContextMenu = MakeContextMenu(btn, "file");
                            st2.Children.Add(btn);
                        }
                        st1.Children.Add(st2);
                    }
                    br.Tag = brList.ToArray();

                    gr.Children.Add(im);
                    Grid.SetColumn(im, 0);
                    gr.Children.Add(st1);
                    Grid.SetColumn(st1, 1);
                    br.Content = gr;
                    contener.Children.Add(br);

                }
                dataFiles.Clear();
            }
            catch (Exception err){
                Debug.WriteLine("--> ContentBlocks ERROR <--");
                Debug.WriteLine(JsonConvert.SerializeObject(err));
                return false;
            }
            Debug.WriteLine("--> ContentBlocks END <--");
            return true;
        }

        private void BtnTrack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("--> BtnTrack_MouseDoubleClick END <--");
            string track = (string)((Button)sender).Tag;
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(new string[] { track });
        }

        private void BtnTrack_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("--> BtnTrack_Click END <--");
            //throw new NotImplementedException();
        }

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("--> BtnFolder_Click END <--");
            LibraryFolderButton btn = (LibraryFolderButton)sender;
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
                Parent.setLoadingState(true);
            }));
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
                DisplayPath(btn.Path);
            }));
        }

        private string[] getDirectoryMediaFIles(string path, bool subdir = false) {
            string[] files = Directory.GetFiles(path, "*.*", (subdir is true) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            List<string> tracks = new List<string>();
            foreach (string file in files) {
                string ext = Path.GetExtension(file).ToLower();
                if (Player.AcceptedExtentions.Contains(ext))
                {
                    tracks.Add(file);
                }
            }
            return tracks.ToArray();
        }
    }
}