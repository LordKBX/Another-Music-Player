using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using MainWindow2 = AnotherMusicPlayer.MainWindow2Space.MainWindow2;
using AnotherMusicPlayer;
using AnotherMusicPlayer.MainWindow2Space;
using System.Drawing;
using MaterialDesignColors.ColorManipulation;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        /// <summary> Parent object </summary>
        public MainWindow2 Parent;
        /// <summary> Database object </summary>
        public Database Bdd;
        /// <summary> Zone to display current path(relative to library folder) </summary>
        FlowLayoutPanel NavigatorBar;
        /// <summary> Zone to display content of current path </summary>
        FlowLayoutPanel NavigationContener;

        /// <summary> Zone to display content of current search results </summary>
        FlowLayoutPanel SearchResultsContener;

        /// <summary> Combobox serving to select filtering/search mode </summary>
        ComboBox FilterSelector;
        /// <summary> Combobox serving to select genre for filtering/search mode </summary>
        ComboBox FilterGenreSelector;
        /// <summary> Input text zone serving to communication genre searched for filtering/search mode </summary>
        TextBox FiltersGenreInput;
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

        Bitmap FolderIcon = Icons.FromIconKind(IconKind.FolderOpen, 35, new SolidColorBrush(Colors.White));
        System.Drawing.Color ButtonBackColor = System.Drawing.Color.FromArgb(255, 60, 60, 60);
        //System.Drawing.Color ButtonMouseOverBackColor = Common.LightenDrawingColor(ButtonBackColor, 30);
        System.Drawing.Color ButtonMouseOverBackColor = System.Drawing.Color.FromArgb(255, 90, 90, 90);
        //System.Drawing.Color ButtonMouseDownBackColor = Common.LightenDrawingColor(ButtonBackColor, 30);
        System.Drawing.Color ButtonMouseDownBackColor = System.Drawing.Color.FromArgb(255, 120, 120, 120);
        System.Drawing.Color ButtonBorderColor = System.Drawing.Color.White;

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

        public Library(MainWindow2 parent, string root = null)
        {
            if (!UpdateRootPath((root == null) ? Settings.LibFolder : root)) { throw new InvalidDataException("Root Path Invalid !"); }
            Parent = parent;
            Bdd = App.bdd;
            NavigatorBar = Parent.LibibraryNavigationPathContener;
            NavigationContener = Parent.LibibraryNavigationContent;

            SearchResultsContener = Parent.LibibrarySearchContent;

            FilterSelector = Parent.LibraryFiltersMode;
            FilterGenreSelector = Parent.LibraryFiltersGenreList;
            FiltersGenreInput = Parent.LibraryFiltersGenreSearchBox;
            FiltersSearchInput = Parent.LibraryFiltersSearchBox;

            pathNavigator = new LibraryPathNavigator(this, NavigatorBar, RootPath);

            App.bdd.DatabaseQuerys(new string[] { "UPDATE files SET Genres = NULL WHERE TRIM(Genres) = ''" }, true);

            LoadGenreList();

            Parent.LibraryTabSplitContainer.Panel1Collapsed = false;
            Parent.LibraryTabSplitContainer.Panel2Collapsed = true;
            Parent.LibibraryNavigationContent.Controls.Clear();
            Parent.LibibrarySearchContent.Controls.Clear();

            Parent.LibraryFiltersGenreList.Visible = false;
            Parent.LibraryFiltersGenreSearchBox.Visible = false;
            Parent.LibraryFiltersSearchBox.Visible = false;
            FiltersGenreInput.Text = "";
            FiltersSearchInput.Text = "";

            FilterSelector.SelectedIndexChanged += FilterSelector_SelectionChanged;
            FilterGenreSelector.SelectedIndexChanged += FilterGenreSelector_SelectionChanged;
            FiltersGenreInput.KeyDown += FiltersSearchInput_KeyDown;
            FiltersSearchInput.KeyDown += FiltersSearchInput_KeyDown;

            CreateWatcher();
            InvokeScan();
        }

        private double LibibrarySearchContent_CalcCollumnWidth()
        {
            //double calc = (Parent.LibibrarySearchContent.ActualWidth - Parent.LibibrarySearchContentC4.Width - Parent.LibibrarySearchContentC5.Width - 20) / 3;
            //return (calc > 0) ? calc : 0;
            return 0;
        }

        /// <summary> Update var RootPath and return if value is valid or not </summary>
        public bool UpdateRootPath(string root) { if (Directory.Exists(root)) { RootPath = root; return true; } else { return false; } }

        public void DisplayPath(string path = null)
        {
            if (path == null) { path = Settings.LibFolder; }
            if (!Directory.Exists(path)) { return; }
            _CurrentPath = path;

            FiltersSearchInput.Visible = false;
            FiltersSearchInput.Text = "";

            FilterSelector.SelectedIndex = 0;
            FilterGenreSelector.SelectedIndex = 0;
            FilterGenreSelector.Visible = false;

            pathNavigator.Display(path);
            Parent.LibibraryNavigationContent.Tag = path;
            Parent.LibibraryNavigationContent.Controls.Clear();
            Parent.LibibraryNavigationContent.AutoScrollOffset = new System.Drawing.Point(0, 0);
            Parent.LibibraryNavigationContent.ContextMenuStrip = MakeContextMenu(Parent.LibibraryNavigationContent, "folder", (path != Settings.LibFolder) ? true : false, (path != Settings.LibFolder) ? path : null);
            Parent.LibibraryNavigationContent.ContextMenuStrip.BackColor = System.Drawing.Color.FromArgb(255, 30, 30, 30);
            Parent.LibibraryNavigationContent.ContextMenuStrip.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            Parent.LibibraryNavigationContent.SuspendLayout();

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                //if (NavigationContener.Controls.Count > 5) { continue; }
                if (Settings.LibFolderShowHiden == false)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden)) { continue; }
                }
                if (Settings.LibFolderShowUnixHiden == false)
                {
                    string name = dir.Replace(path, "").TrimStart(MainWindow2.SeparatorChar);
                    if (name[0] == '.') { continue; }
                }
                string[] tab = dir.Split(MainWindow2.SeparatorChar);

                Button button = new Button()
                {
                    Text = tab[tab.Length - 1],
                    Tag = dir,
                    Image = FolderIcon,
                    ImageAlign = System.Drawing.ContentAlignment.TopCenter,
                    TextImageRelation = TextImageRelation.ImageAboveText,
                    Width = 90, Height = 90,
                    Padding = new Padding(3,3,3,3),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = ButtonBackColor
                };
                button.FlatAppearance.MouseOverBackColor = ButtonMouseOverBackColor;
                button.FlatAppearance.MouseDownBackColor = ButtonMouseDownBackColor;
                button.FlatAppearance.CheckedBackColor = ButtonMouseDownBackColor;
                button.FlatAppearance.BorderColor = ButtonBorderColor;
                button.FlatAppearance.BorderSize = 1;
                button.ContextMenuStrip = MakeContextMenu(button, "folder");
                button.ContextMenuStrip.BackColor = System.Drawing.Color.FromArgb(255, 30, 30, 30);
                button.ContextMenuStrip.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
                button.Click += BtnFolder_Click;

                NavigationContener.Controls.Add(button);
            }

            string[] filesList = Directory.GetFiles(path);
            if (filesList.Length > 0)
            {
                //Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE Path LIKE '" + Database.EscapeString(path + MainWindow2.SeparatorChar) + "%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
                Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE replace(Path, '" + Database.EscapeString(path + MainWindow2.SeparatorChar) + "', '') NOT LIKE '%\\%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
                List<string> endFiles = new List<string>();
                foreach (string file in files.Keys.ToArray())
                {
                    string fi = file.Replace(path + MainWindow2.SeparatorChar, "");
                    if (!fi.Contains(MainWindow2.SeparatorChar))
                    {
                        endFiles.Add(file);
                    }
                }

                FlowLayoutPanel panel = new FlowLayoutPanel() { 
                    FlowDirection = System.Windows.Forms.FlowDirection.TopDown, 
                    MinimumSize = new System.Drawing.Size(100, 50), 
                    Dock = DockStyle.Top,
                    Visible = true, WrapContents = false, AutoScroll = false, 
                    AutoSizeMode = AutoSizeMode.GrowAndShrink, AutoSize = true
                };
                NavigationContener.Controls.Add(panel);

                Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> tabInfo = GetTabInfoFromFiles(endFiles.ToArray());
                ContentBlocks(tabInfo, panel);
            }
            NavigationContener.ResumeLayout();
            Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() => { Parent.setLoadingState(false); }));
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
                    MediaItem it = App.DatabaseItemToMediaItem(dataFiles[fil]);
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

        public bool ContentBlocks(Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> infoTab, FlowLayoutPanel contener, bool uniqueDir = true, bool clear = true)
        {
            Debug.WriteLine("--> ContentBlocks START <--");
            if (infoTab == null) { Debug.WriteLine("--> ContentBlocks NO FILE 0 <--"); return false; }
            if (infoTab.Count == 0) { Debug.WriteLine("--> ContentBlocks NO FILE 1 <--"); return false; }
            try
            {
                BitmapImage defaultCover = App.BitmapToBitmapImage(Properties.Resources.CoverImg);
                if (uniqueDir)
                {
                    MediaItem item1 = null;
                    foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> album in infoTab)
                    {
                        foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> disk in album.Value)
                        { foreach (KeyValuePair<string, MediaItem> track in disk.Value) { item1 = track.Value; break; }; break; }
                        break;
                    }
                    string folder = item1.Path.Substring(0, item1.Path.LastIndexOf(MainWindow2.SeparatorChar));
                    string[] t = System.IO.Directory.GetFiles(folder);
                    foreach (string file in t)
                    {
                        if (file.EndsWith("Cover.jpg") || file.EndsWith("Cover.png")) { defaultCover = new BitmapImage(new Uri(file)); }
                    }
                }

                if (clear == true) { contener.Controls.Clear(); }

                foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> albumT in infoTab)
                {
                    string coverPath = albumT.Value.Values.First().First().Value.Path;
                    string al = albumT.Key;

                    List<string> brList = new List<string>();
                    Button br = new Button() { /*Style = Parent.FindResource("LibibraryNavigationContentAlbum") as Style*/ };
                    if (al == null || al.Trim() == "")
                    {
                        if (uniqueDir) { al = "<UNKWON ALBUM>"; }
                        else { al = albumT.Value.Values.First().First().Value.Name; }
                        br.ContextMenuStrip = new ContextMenuStrip();
                    }
                    else { br.ContextMenuStrip = MakeContextMenu(br, "album"); }
                    TableLayoutPanel gr = new TableLayoutPanel() { };
                    gr.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
                    gr.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 150));
                    Button im = new Button() { MinimumSize = new System.Drawing.Size(150, 150), BackgroundImageLayout = ImageLayout.Zoom, BackColor = System.Drawing.Color.Black };
                    im.BackgroundImage = App.BitmapImage2Bitmap(FilesTags.MediaPicture(coverPath, Bdd, true, 150, 150, false) ?? defaultCover);
                    //if (im.Source != defaultCover && Settings.MemoryUsage == 1)
                    //{
                    //    FlowLayoutPanel p = new FlowLayoutPanel() { 
                    //        FlowDirection = System.Windows.Forms.FlowDirection.TopDown, 
                    //        Visible = true, WrapContents = false, AutoScroll = false,
                    //        AutoSizeMode = AutoSizeMode.GrowAndShrink, AutoSize = true,
                    //        Dock = DockStyle.Top
                    //    };
                    //    Image imp = new Image() { Style = Parent.FindResource("HQImg") as Style };

                    //    string ret = Bdd.DatabaseGetCover(coverPath);
                    //    imp.Source = FilesTags.MediaPicture(coverPath, Bdd, true, 0, 0, false) ?? defaultCover;
                    //    p.Controls.Add(imp);
                    //    im.ToolTip = p;
                    //}

                    FlowLayoutPanel st0 = new FlowLayoutPanel() { 
                        FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight, 
                        Visible = true, WrapContents = false, AutoScroll = false 
                    };
                    //StackPanel st0 = new StackPanel()
                    //{
                    //    Orientation = Orientation.Horizontal,
                    //    VerticalAlignment = VerticalAlignment.Top,
                    //    HorizontalAlignment = HorizontalAlignment.Center,
                    //    Style = Parent.FindResource("thumbnailBlock") as Style
                    //};
                    st0.Controls.Add(im);

                    FlowLayoutPanel st1 = new FlowLayoutPanel()
                    {
                        FlowDirection = System.Windows.Forms.FlowDirection.TopDown,
                        WrapContents = false,
                        Margin = new Padding(5, 0, 0, 0),
                        MinimumSize = new System.Drawing.Size(100, 50),
                        Dock = DockStyle.Top
                    };
                    Label lb = new Label() { Text = al };
                    lb.Disposed += (object sender, EventArgs e) => { App.DelToolTip((Label)sender); };
                    App.SetToolTip(lb, al);
                    st1.Controls.Add(lb);

                    foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> discT in albumT.Value)
                    {
                        if (albumT.Value.Count > 1)
                            st1.Controls.Add(new Label() { Text = "Disc " + discT.Key });

                        List<KeyValuePair<string, MediaItem>> myList = discT.Value.ToList();
                        myList.Sort((pair1, pair2) =>
                        {
                            uint trackcmp = pair1.Value.Track - pair2.Value.Track;
                            if (trackcmp == 0) { return pair1.Value.Name.CompareTo(pair2.Value.Name); }
                            else { return (int)trackcmp; }
                        }
                        );
                        FlowLayoutPanel st2 = new FlowLayoutPanel()
                        {
                            FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight, 
                            Dock = DockStyle.Fill
                        };
                        foreach (KeyValuePair<string, MediaItem> trackT in myList)
                        {
                            brList.Add(trackT.Value.Path);
                            string textName = ((trackT.Value.Track == 0) ? "" : App.NormalizeNumber((int)trackT.Value.Track, ("" + trackT.Value.TrackCount).Length) + ". ") + trackT.Value.Name;
                            FlowLayoutPanel pan = new FlowLayoutPanel() { FlowDirection = System.Windows.Forms.FlowDirection.TopDown, WrapContents = false, AutoSize = true };
                            pan.Disposed += (object sender, EventArgs e) => { App.DelToolTip((FlowLayoutPanel)sender); };
                            App.SetToolTip(pan, textName);
                            pan.Controls.Add(new Label() { Text = textName });
                            Rating2 rt = new Rating2()
                            {
                                Rate = trackT.Value.Rating,
                                Tag = trackT.Value.Path,
                                //BackgroundColor = Parent.FindResource("TrackButton.StarsBackground") as SolidColorBrush
                            };
                            rt.RateChanged += Library_RateChanged;
                            pan.Controls.Add(rt);

                            //Button btn = new Button()
                            //{
                            //    Content = pan,
                            //    Tag = trackT.Value.Path,
                            //    //ToolTip = textName,
                            //    //Style = Parent.FindResource("LibibraryNavigationContentFolderButtonTrackButton") as Style//
                            //};
                            //pan.Click += BtnTrack_Click;
                            pan.MouseDoubleClick += BtnTrack_MouseDoubleClick;
                            pan.ContextMenuStrip = MakeContextMenu(pan, "file");
                            st2.Controls.Add(pan);
                        }
                        st1.Controls.Add(st2);
                    }
                    br.Tag = brList.ToArray();

                    gr.Controls.Add(st0);
                    gr.SetColumn(st0, 0);
                    gr.Controls.Add(st1);
                    gr.SetColumn(st1, 1);
                    br.Controls.Add(gr);
                    contener.Controls.Add(br);
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

        private void Library_RateChanged(Rating2 sender, double value)
        {
            string filePath = (string)sender.Tag;
            Debug.WriteLine("Library Rate Changed !");
            Debug.WriteLine("filePath=" + filePath);
            Debug.WriteLine("New value=" + value);
            FilesTags.SaveRating(filePath, value);
            //rater.ToolTip = e.NewValue;
            //throw new NotImplementedException();
        }

        private void BtnTrack_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Debug.WriteLine("--> BtnTrack_MouseDoubleClick END <--");
            string track = (string)((Button)sender).Tag;
            Player.PlaylistClear();
            Player.PlaylistEnqueue(new string[] { track });
        }

        private void BtnTrack_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("--> BtnTrack_Click END <--");
            //throw new NotImplementedException();
        }

        private void BtnFolder_Click(object sender, EventArgs e)
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