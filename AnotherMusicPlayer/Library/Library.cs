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
using System.Windows.Media;

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
        AlignablePanel NavigationContener;
        /// <summary> Scrollview of current path content display zone </summary>
        ScrollViewer NavigationContenerScoller;
        /// <summary> Border Scrollview of current path content display zone </summary>
        Border NavigationContenerScollerBorder;

        /// <summary> Zone to display content of current search results </summary>
        ListView SearchResultsContener;
        /// <summary> Border Scrollview of search results display zone </summary>
        Border SearchResultsContenerBorder;
        /// <summary> StackPanel of search results details display zone </summary>
        Border LibibrarySearchContentDetails;

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
            Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.DirectoryName;
            Watcher.Filter = "*.*";
            Watcher.Changed += LibraryFileChanged;
            Watcher.Renamed += LibraryFileRenamed;
            Watcher.Deleted += LibraryFileDeleted;
            Watcher.Created += LibraryFileCreated;
            Watcher.EnableRaisingEvents = true;
        }

        /// <summary> Callback for when Library Watcher detect a change </summary>
        private void LibraryFileChanged(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine("LibraryFileChanged => " + e.FullPath);
            if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower()))
            {
                Thread.Sleep(200);
                Bdd.UpdateFileAsync(e.FullPath, true);
            }
        }

        /// <summary> Callback for when Library Watcher detect a change </summary>
        private void LibraryFileRenamed(object source, RenamedEventArgs e)
        {
            if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower()))
            {
                Debug.WriteLine("--> LibraryFileRenamed");
                Debug.WriteLine("Old Name => " + e.OldFullPath);
                Debug.WriteLine("New Name => " + e.FullPath);
                Thread.Sleep(200);
                Bdd.RenameFileAsync(e.OldFullPath, e.FullPath);
                //Bdd.DeleteFileAsync(e.OldFullPath);
                //Bdd.DatabaseClearCover(e.OldFullPath);
                //Bdd.UpdateFileAsync(e.FullPath, true);
            }
        }

        private void LibraryFileDeleted(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("--> LibraryFileDeleted");
            if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower()))
            {
                Thread.Sleep(200);
                Bdd.DeleteFileAsync(e.FullPath);
                Bdd.DatabaseClearCover(e.FullPath);
            }
        }

        private void LibraryFileCreated(object sender, FileSystemEventArgs e)
        {
            Debug.WriteLine("--> LibraryFileCreated");
            if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower()))
            {
                Thread.Sleep(200);
                InsertBddFile(new FileInfo(e.FullPath), true);
                Bdd.UpdateFileAsync(e.FullPath, true);
            }
        }

        public Library(MainWindow parent, string root = null)
        {
            if (!UpdateRootPath((root == null) ? Settings.LibFolder : root)) { throw new InvalidDataException("Root Path Invalid !"); }
            Parent = parent;
            Bdd = Parent.bdd;
            NavigatorBar = Parent.LibibraryNavigationPathContener;
            NavigationContener = Parent.LibibraryNavigationContent;
            NavigationContenerScoller = Parent.LibibraryNavigationContentScroll;
            NavigationContenerScollerBorder = Parent.LibibraryNavigationContentB;

            SearchResultsContener = Parent.LibibrarySearchContent;
            SearchResultsContenerBorder = Parent.LibibraryNavigationContent2B;
            LibibrarySearchContentDetails = Parent.LibibrarySearchContentDetails;

            FilterSelector = Parent.LibraryFiltersMode;
            FilterGenreSelector = Parent.LibraryFiltersGenreList;
            FiltersSearchInput = Parent.LibraryFiltersSearchBox;

            pathNavigator = new LibraryPathNavigator(this, NavigatorBar, RootPath);

            Parent.bdd.DatabaseQuerys(new string[] { "UPDATE files SET Genres = NULL WHERE TRIM(Genres) = ''" }, true);

            LoadGenreList();

            FilterSelector.SelectionChanged += FilterSelector_SelectionChanged;
            FilterGenreSelector.SelectionChanged += FilterGenreSelector_SelectionChanged;
            FiltersSearchInput.KeyDown += FiltersSearchInput_KeyDown;

            Parent.LibibrarySearchContentC1.Width = Parent.LibibrarySearchContentC2.Width = Parent.LibibrarySearchContentC3.Width = LibibrarySearchContent_CalcCollumnWidth();
            SearchResultsContener.SizeChanged += (sender, e) =>
            {
                Debug.WriteLine("LibibrarySearchContent.SizeChanged");
                Parent.LibibrarySearchContentC1.Width = Parent.LibibrarySearchContentC2.Width = Parent.LibibrarySearchContentC3.Width = LibibrarySearchContent_CalcCollumnWidth();
            };
            SearchResultsContener.SelectionChanged += (sender, e) =>
            {
                if (SearchResultsContener.SelectedItems.Count <= 0 || SearchResultsContener.SelectedItems.Count > 1)
                {
                    Parent.LibibrarySearchContentGridRow2.Height = new GridLength(0);
                    if (SearchResultsContener.SelectedItems.Count > 1) { Parent.LibibrarySearchContent.ContextMenu = MakeContextMenu(Parent.LibibrarySearchContent, "selection", false); }
                    else Parent.LibibrarySearchContent.ContextMenu = null;
                }
                else
                {
                    Parent.LibibrarySearchContentGridRow2.Height = new GridLength(120);
                    Parent.SearchFileCover.Source = FilesTags.MediaPicture(((MediaItem)SearchResultsContener.SelectedItem).Path, Parent.bdd, true, 150, 150);
                    Parent.SearchMediaInfoTitle.Text = ((MediaItem)SearchResultsContener.SelectedItem).Name;
                    string artists = ((MediaItem)SearchResultsContener.SelectedItem).Composers;
                    artists += ((artists.Length > 0) ? ", " : "") + ((MediaItem)SearchResultsContener.SelectedItem).Performers;
                    Parent.SearchMediaInfoArtists.Text = artists;
                    Parent.SearchMediaInfoAlbum.Text = ((MediaItem)SearchResultsContener.SelectedItem).Album;
                    Parent.SearchMediaInfoLyrics.Text = ((MediaItem)SearchResultsContener.SelectedItem).Lyrics;


                    Parent.LibibrarySearchContent.ContextMenu = MakeContextMenu(Parent.LibibrarySearchContent, "selection", false);
                    for (int i = 0; i < Parent.LibibrarySearchContent.ContextMenu.Items.Count; i++)
                    {
                        if (((MenuItem)Parent.LibibrarySearchContent.ContextMenu.Items[i]).Name == "PlayShuffleSelection")
                        { ((MenuItem)Parent.LibibrarySearchContent.ContextMenu.Items[i]).Visibility = Visibility.Collapsed; break; }
                    }
                }
            };

            CreateWatcher();
            InvokeScan();
        }

        private double LibibrarySearchContent_CalcCollumnWidth()
        {
            double calc = (Parent.LibibrarySearchContent.ActualWidth - Parent.LibibrarySearchContentC4.Width - Parent.LibibrarySearchContentC5.Width - 20) / 3;
            return (calc > 0) ? calc : 0;
        }

        /// <summary> Update var RootPath and return if value is valid or not </summary>
        public bool UpdateRootPath(string root) { if (Directory.Exists(root)) { RootPath = root; return true; } else { return false; } }

        public void DisplayPath(string path = null)
        {
            if (path == null) { path = Settings.LibFolder; }
            if (!Directory.Exists(path)) { return; }
            _CurrentPath = path;

            NavigationContenerScollerBorder.Visibility = Visibility.Visible;
            SearchResultsContenerBorder.Visibility = Visibility.Collapsed;

            FiltersSearchInput.Visibility = Visibility.Collapsed;
            FiltersSearchInput.Text = "";

            FilterSelector.SelectedIndex = 0;
            FilterGenreSelector.SelectedIndex = 0;
            FilterGenreSelector.Visibility = Visibility.Collapsed;

            pathNavigator.Display(path);
            SearchResultsContener.ItemsSource = null;
            NavigationContener.Tag = path;
            NavigationContener.Children.Clear();
            NavigationContenerScoller.ContextMenu = null;
            NavigationContenerScoller.ContextMenu = MakeContextMenu(NavigationContener, "folder", (path != Settings.LibFolder) ? true : false, (path != Settings.LibFolder) ? path : null);

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                //if (NavigationContener.Children.Count > 5) { continue; }
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

                Button button = new Button() { Content = tab[tab.Length - 1], Tag = dir, Style = Parent.FindResource("LibibraryNavigationContentFolderButton") as Style };
                button.ContextMenu = MakeContextMenu(button, "folder");
                button.Click += BtnFolder_Click;

                NavigationContener.Children.Add(button);
            }

            //Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE Path LIKE '" + Database.EscapeString(path + MainWindow.SeparatorChar) + "%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
            Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE replace(Path, '" + Database.EscapeString(path + MainWindow.SeparatorChar) + "', '') NOT LIKE '%\\%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
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

            Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> tabInfo = GetTabInfoFromFiles(endFiles.ToArray());
            ContentBlocks(tabInfo, panel);
            NavigationContenerScoller.ScrollToHome();
            Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() =>
            {
                Parent.setLoadingState(false);
            }));
        }

        public Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> GetTabInfoFromFiles(string[] files)
        {
            if (files == null) { Debug.WriteLine("--> GetTabInfoFromFiles NO FILE 0 <--"); return null; }
            if (files.Length == 0) { Debug.WriteLine("--> GetTabInfoFromFiles NO FILE 1 <--"); return null; }

            while (files.Length > 0 && files[0].Length == 0) { files = files.Where(w => w != files[0]).ToArray(); }
            if (files.Length == 0) { Debug.WriteLine("--> GetTabInfoFromFiles NO FILE 2 <--"); return null; }

            Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> tab = new Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>>();
            Dictionary<string, Dictionary<string, object>> dataFiles = Bdd.DatabaseFilesInfo(files);
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
                    Debug.WriteLine("--> GetTabInfoFromFiles ERROR - FILE '" + fil + "' do not exist <--");
                }
            }
            return tab;
        }

        public bool ContentBlocks(Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> infoTab, StackPanel contener, bool uniqueDir = true, bool clear = true)
        {
            Debug.WriteLine("--> ContentBlocks START <--");
            if (infoTab == null) { Debug.WriteLine("--> ContentBlocks NO FILE 0 <--"); return false; }
            if (infoTab.Count == 0) { Debug.WriteLine("--> ContentBlocks NO FILE 1 <--"); return false; }
            try
            {
                BitmapImage defaultCover = MainWindow.Bimage("CoverImg");
                if (uniqueDir)
                {
                    MediaItem item1 = null;
                    foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> album in infoTab)
                    {
                        foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> disk in album.Value)
                        { foreach (KeyValuePair<string, MediaItem> track in disk.Value) { item1 = track.Value; break; }; break; }
                        break;
                    }
                    string folder = item1.Path.Substring(0, item1.Path.LastIndexOf(MainWindow.SeparatorChar));
                    string[] t = System.IO.Directory.GetFiles(folder);
                    foreach (string file in t)
                    {
                        if (file.EndsWith("Cover.jpg") || file.EndsWith("Cover.png")) { defaultCover = new BitmapImage(new Uri(file)); }
                    }
                }

                if (clear == true) { contener.Children.Clear(); }

                foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> albumT in infoTab)
                {
                    string coverPath = albumT.Value.Values.First().First().Value.Path;
                    string al = albumT.Key;

                    List<string> brList = new List<string>();
                    Button br = new Button() { Style = Parent.FindResource("LibibraryNavigationContentAlbum") as Style };
                    if (al == null || al.Trim() == "")
                    {
                        if (uniqueDir) { al = "<UNKWON ALBUM>"; }
                        else { al = albumT.Value.Values.First().First().Value.Name; }
                        br.ContextMenu = new ContextMenu();
                    }
                    else { br.ContextMenu = MakeContextMenu(br, "album"); }
                    Grid gr = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
                    gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(150, GridUnitType.Pixel) });
                    gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });
                    System.Windows.Controls.Image im = new System.Windows.Controls.Image()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MaxWidth = 150
                    };
                    im.Source = FilesTags.MediaPicture(coverPath, Bdd, true, 150, 150, false) ?? defaultCover;
                    if (im.Source != defaultCover && Settings.MemoryUsage == 1)
                    {
                        WrapPanel p = new WrapPanel() { Orientation = Orientation.Vertical };
                        Image imp = new Image() { Style = Parent.FindResource("HQImg") as Style };
                        string ret = Bdd.DatabaseGetCover(coverPath);
                        imp.Source = FilesTags.MediaPicture(coverPath, Bdd, true, 0, 0, false) ?? defaultCover;
                        p.Children.Add(imp);
                        im.ToolTip = p;
                    }

                    if (((BitmapImage)im.Source).PixelWidth < 150)
                    {
                        double marge = (150 - ((BitmapImage)im.Source).PixelWidth) / 2;
                        im.Margin = new Thickness(marge, 0, marge, 0);
                    }

                    if (((BitmapImage)im.Source).PixelHeight < 150)
                    {
                        double marge = (150 - ((BitmapImage)im.Source).PixelHeight) / 2;
                        im.Margin = new Thickness(0, marge, 0, marge);
                    }

                    if (((BitmapImage)im.Source).PixelHeight > 150) { im.Height = 150; }

                    StackPanel st0 = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Style = Parent.FindResource("thumbnailBlock") as Style
                    };
                    st0.Children.Add(im);

                    StackPanel st1 = new StackPanel()
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(5, 0, 0, 0)
                    };
                    st1.Children.Add(new AccessText()
                    {
                        Style = Parent.FindResource("LibibraryNavigationContentAlbumTitle") as Style,
                        Text = al,
                        ToolTip = al
                    });
                    foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> discT in albumT.Value)
                    {
                        if (albumT.Value.Count > 1)
                            st1.Children.Add(new AccessText()
                            {
                                Style = Parent.FindResource("LibibraryNavigationContentAlbumDisk") as Style,
                                Text = "Disc " + discT.Key
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
                            StackPanel pan = new StackPanel() { VerticalAlignment = VerticalAlignment.Stretch };
                            pan.Children.Add(new AccessText()
                            {
                                Text = textName,
                                Style = Parent.FindResource("LibibraryNavigationContentFolderButtonTrackButtonAccessText") as Style
                            });
                            Rating rt = new Rating()
                            {
                                Rate = trackT.Value.Rating,
                                Tag = trackT.Value.Path,
                                Style = Parent.FindResource("LibibraryNavigationContentFolderButtonTrackButtonStars") as Style,
                                StarBackgroundColor = Parent.FindResource("TrackButton.StarsBackground") as SolidColorBrush,
                                StarSelectionForegroundColor = Parent.FindResource("TrackButton.StarsSelectionForeground") as SolidColorBrush,
                                StarForegroundColor = Parent.FindResource("TrackButton.StarsForeground") as SolidColorBrush,
                            };
                            rt.setAltLeftClick();
                            rt.RateChanged += Library_RateChanged;
                            pan.Children.Add(rt);

                            Button btn = new Button()
                            {
                                Content = pan,
                                Tag = trackT.Value.Path,
                                //ToolTip = textName,
                                Style = Parent.FindResource("LibibraryNavigationContentFolderButtonTrackButton") as Style
                            };
                            //btn.Click += BtnTrack_Click;
                            btn.MouseDoubleClick += BtnTrack_MouseDoubleClick;
                            btn.ContextMenu = MakeContextMenu(btn, "file");
                            st2.Children.Add(btn);
                        }
                        st1.Children.Add(st2);
                    }
                    br.Tag = brList.ToArray();

                    gr.Children.Add(st0);
                    Grid.SetColumn(st0, 0);
                    gr.Children.Add(st1);
                    Grid.SetColumn(st1, 1);
                    br.Content = gr;
                    contener.Children.Add(br);

                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("--> ContentBlocks ERROR <--");
                Debug.WriteLine(JsonConvert.SerializeObject(err));
                return false;
            }
            Debug.WriteLine("--> ContentBlocks END <--");
            return true;
        }

        private void Library_RateChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Rating rater = (Rating)sender;
            string filePath = (string)rater.Tag;
            Debug.WriteLine("Library Rate Changed !");
            Debug.WriteLine("filePath=" + filePath);
            Debug.WriteLine("Old value=" + e.OldValue);
            Debug.WriteLine("New value=" + e.NewValue);
            FilesTags.SaveRating(filePath, e.NewValue);
            //rater.ToolTip = e.NewValue;
            //throw new NotImplementedException();
        }

        private void BtnTrack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("--> BtnTrack_MouseDoubleClick END <--");
            string track = (string)((Button)sender).Tag;
            Player.PlaylistClear();
            Player.PlaylistEnqueue(new string[] { track });
        }

        private void BtnTrack_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("--> BtnTrack_Click END <--");
            //throw new NotImplementedException();
        }

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("--> BtnFolder_Click END <--");
            Button btn = (Button)sender;
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                Parent.setLoadingState(true);
            }));
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                DisplayPath(btn.Tag as string);
            }));
        }

        private string[] getDirectoryMediaFIles(string path, bool subdir = false)
        {
            string[] files = Directory.GetFiles(path, "*.*", (subdir is true) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            List<string> tracks = new List<string>();
            foreach (string file in files)
            {
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