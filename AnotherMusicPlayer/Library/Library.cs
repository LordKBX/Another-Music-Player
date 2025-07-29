using AnotherMusicPlayer.Components;
using AnotherMusicPlayer.MainWindow2Space;
using AnotherMusicPlayer.Styles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using MainWindow2 = AnotherMusicPlayer.MainWindow2Space.MainWindow2;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        /// <summary> Parent object </summary>
        public MainWindow2 Parent;
        /// <summary> Database object </summary>
        public Database Bdd;

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

        Bitmap defaultCover = Properties.Resources.CoverImg;
        BitmapImage defaultBICover = BitmapMagic.BitmapToBitmapImage(Properties.Resources.CoverImg);
        Bitmap FolderIcon = Icons.FromIconKind(IconKind.FolderOpen, 35, new SolidColorBrush(Colors.White));
        System.Drawing.Color ButtonBackColor = System.Drawing.Color.FromArgb(255, 60, 60, 60);
        System.Drawing.Color ButtonForeColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
        System.Drawing.Color ButtonMouseOverBackColor = System.Drawing.Color.FromArgb(255, 90, 90, 90);
        System.Drawing.Color ButtonMouseDownBackColor = System.Drawing.Color.FromArgb(255, 120, 120, 120);
        System.Drawing.Color ButtonBorderColor = System.Drawing.Color.White;

        Button BaseFolderButton = null;

        #region FolderWatcher Creation & Events
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
        { if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower())) { Thread.Sleep(200); Bdd.UpdateFileAsync(e.FullPath, true); } }
        /// <summary> Callback for when Library Watcher detect a file renamed </summary>
        private void LibraryFileRenamed(object source, RenamedEventArgs e)
        { if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower())) { Thread.Sleep(200); Bdd.RenameFileAsync(e.OldFullPath, e.FullPath); } }
        /// <summary> Callback for when Library Watcher detect a file deleted </summary>
        private void LibraryFileDeleted(object sender, FileSystemEventArgs e)
        {
            if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower()))
            { Thread.Sleep(200); Bdd.DeleteFileAsync(e.FullPath); Bdd.DatabaseClearCover(e.FullPath); }
        }
        /// <summary> Callback for when Library Watcher detect a new file </summary>
        private void LibraryFileCreated(object sender, FileSystemEventArgs e)
        {
            if (Player.AcceptedExtentions.Contains(Path.GetExtension(e.Name).ToLower()))
            { Thread.Sleep(200); InsertBddFile(new FileInfo(e.FullPath), true); /*Bdd.UpdateFileAsync(e.FullPath, true);*/ }
        }
        #endregion

        public Library(MainWindow2 parent, string root = null)
        {
            if (!UpdateRootPath((root == null) ? Settings.LibFolder : root)) { throw new InvalidDataException("Root Path Invalid !"); }
            Parent = parent;
            Bdd = App.bdd;

            pathNavigator = new LibraryPathNavigator(this, Parent.LibraryNavigationPathContener, RootPath);

            App.bdd.DatabaseQuerys(new string[] { "UPDATE files SET Genres = NULL WHERE TRIM(Genres) = ''" }, true);

            LoadGenreList();

            Parent.LibraryTabSplitContainer.Panel1Collapsed = false;
            Parent.LibraryTabSplitContainer.Panel2Collapsed = true;
            Parent.LibraryNavigationContentFolders.DataSource = null;
            Parent.LibrarySearchContent.DataSource = null;

            Parent.LibraryFiltersGenreList.Visible = false;
            Parent.LibraryFiltersGenreSearchBox.Visible = false;
            Parent.LibraryFiltersSearchBox.Visible = false;
            Parent.LibraryFiltersGenreSearchBox.Text = "";
            Parent.LibraryFiltersSearchBox.Text = "";

            Parent.LibraryFiltersMode.SelectedIndexChanged += LibraryFiltersMode_SelectionChanged;
            Parent.LibraryFiltersGenreList.SelectedIndexChanged += LibraryFiltersGenreList_SelectionChanged;
            Parent.LibraryFiltersGenreSearchBox.KeyDown += LibraryFiltersSearchBox_KeyDown;
            Parent.LibraryFiltersSearchBox.KeyDown += LibraryFiltersSearchBox_KeyDown;

            Parent.LibraryNavigationContentFolders.AutoGenerateColumns = false;
            Parent.LibraryNavigationContentFolders.ColumnHeadersVisible = false;
            Parent.LibraryNavigationContentFolders.RowHeadersVisible = false;
            Parent.LibraryNavigationContentFolders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Parent.LibraryNavigationContentFolders.MultiSelect = false;
            Parent.LibraryNavigationContentFolders.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = Guid.NewGuid().ToString(),
                HeaderText = "",
                DataPropertyName = "Name",
                Width = 200,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            Parent.LibraryNavigationContentFolders.MouseDoubleClick += BtnFolder_Click;

            CreateWatcher();
            //InvokeScan();
            //ActualizeColors();
        }

        //public void ActualizeColors()
        //{
        //    ButtonBackColor = App.style.GetColor("LibraryFolderButtonBackColor", Dark.LibraryFolderButtonBackColor);
        //    ButtonForeColor = App.style.GetColor("LibraryFolderButtonForeColor", Dark.LibraryFolderButtonForeColor);
        //    SolidColorBrush IconTabBrush = new SolidColorBrush(App.DrawingColorToMediaColor(ButtonForeColor));
        //    FolderIcon = Icons.FromIconKind(IconKind.FolderOpen, App.style.GetValue<int>("LibraryFolderButtonIconSize", Dark.LibraryFolderButtonIconSize), new SolidColorBrush(Colors.White));

        //    ButtonBorderColor = App.style.GetColor("LibraryFolderButtonBorderColor", Dark.LibraryFolderButtonBorderColor);
        //    ButtonMouseOverBackColor = App.style.GetColor("LibraryFolderButtonMouseOverBackColor", Dark.LibraryFolderButtonMouseOverBackColor);
        //    ButtonMouseDownBackColor = App.style.GetColor("LibraryFolderButtonMouseDownBackColor", Dark.LibraryFolderButtonMouseDownBackColor);

        //    BuildBaseFolderButton();
        //}

        //public void BuildBaseFolderButton() 
        //{
        //    if (BaseFolderButton != null) { BaseFolderButton.Dispose(); }
        //    BaseFolderButton = new Button()
        //    {
        //        Text = "",
        //        Tag = null,
        //        Image = FolderIcon,
        //        ImageAlign = System.Drawing.ContentAlignment.TopCenter,
        //        TextImageRelation = TextImageRelation.ImageAboveText,
        //        Width = 90,
        //        Height = 90,
        //        Padding = new Padding(3, 3, 3, 3),
        //        FlatStyle = FlatStyle.Flat,
        //        BackColor = ButtonBackColor,
        //        ForeColor = ButtonForeColor,
        //        Cursor = Cursors.Hand
        //    };
        //    BaseFolderButton.FlatAppearance.MouseOverBackColor = ButtonMouseOverBackColor;
        //    BaseFolderButton.FlatAppearance.MouseDownBackColor = ButtonMouseDownBackColor;
        //    BaseFolderButton.FlatAppearance.CheckedBackColor = ButtonMouseDownBackColor;
        //    BaseFolderButton.FlatAppearance.BorderColor = ButtonBorderColor;
        //    BaseFolderButton.FlatAppearance.BorderSize = 1;
        //    BaseFolderButton.ContextMenuStrip = MakeContextMenu(BaseFolderButton, "folder");
        //}

        //public Button CreateFolderButton(string path) {
        //    if (BaseFolderButton == null) { BuildBaseFolderButton(); }
        //    Button btn = BaseFolderButton.Clone();
        //    btn.Text = path.Substring(path.LastIndexOf(MainWindow2.SeparatorChar) + 1);
        //    btn.Tag = path;
        //    btn.Image = FolderIcon;
        //    btn.Click += BtnFolder_Click;
        //    btn.Cursor = System.Windows.Forms.Cursors.Hand;
        //    //btn.ContextMenuStrip = MakeContextMenu(btn, "folder");
        //    return btn;
        //}

        /// <summary> Update var RootPath and return if value is valid or not </summary>
        public bool UpdateRootPath(string root) { if (Directory.Exists(root)) { RootPath = root; return true; } else { return false; } }

        private Thread DisplayPathThread = null;
        public void DisplayPathAsync(string path = null) {
            if (DisplayPathThread != null) { DisplayPathThread = null; Common.PurgeMemory(); }
            DisplayPathThread = new Thread(() => { Thread.Sleep(300); DisplayPath(path); });
            DisplayPathThread.SetApartmentState(ApartmentState.STA);
            DisplayPathThread.Start();
        }

        public void DisplayPath(string path = null)
        {
            if (path == null) { path = Settings.LibFolder; }
            Debug.WriteLine("--> Library.DisplayPath(" + path + ") <--");
            if (!Directory.Exists(path)) { return; }
            if (Parent.InvokeRequired) { Parent.Invoke(new Action(() => { this.DisplayPath(path); })); return; }
            Debug.WriteLine("Control passed");
            _CurrentPath = path;

            //Parent.LibraryTabSplitContainer.Panel1.Height = Parent.LibraryNavigationContentFolders.Height = Parent.LibraryNavigationContent.Height;

            Parent.LibraryTabSplitContainer.Panel1Collapsed = false;
            Parent.LibraryTabSplitContainer.Panel2Collapsed = false;
            Parent.LibraryTabSplitContainer.SplitterDistance = Parent.LibraryTabSplitContainer.Height / 2;

            Parent.LibraryFiltersSearchBox.Visible = false;
            Parent.LibraryFiltersSearchBox.Text = "";

            Parent.LibraryFiltersMode.SelectedIndex = 0;
            Parent.LibraryFiltersGenreList.SelectedIndex = 0;
            Parent.LibraryFiltersGenreList.Visible = false;

            pathNavigator.Display(path);
            Parent.LibraryNavigationContentFolders.Tag = path;
            if (Parent.LibraryNavigationContentFolders.ContextMenuStrip != null) { Parent.LibraryNavigationContentFolders.ContextMenuStrip.Dispose(); }
            Parent.LibraryNavigationContentFolders.DataSource = null; Common.PurgeMemory();
            Parent.LibraryNavigationContentFolders.AutoScrollOffset = new System.Drawing.Point(0, 0);
            Parent.LibraryNavigationContentFolders.ContextMenuStrip = MakeContextMenu(Parent.LibraryNavigationContentFolders, "folder", (path != Settings.LibFolder) ? true : false, (path != Settings.LibFolder) ? path : null);

            string[] dirs = Directory.GetDirectories(path);
            Debug.WriteLine("dirs.Length = " + dirs.Length);
            Debug.WriteLine("Parent.LibraryTabSplitContainer.Panel1Collapsed = " + ((Parent.LibraryTabSplitContainer.Panel1Collapsed)?"true":"false"));
            Debug.WriteLine("Parent.LibraryNavigationContentFolders.Height = " + Parent.LibraryNavigationContentFolders.Height);
            if (dirs.Length == 0) { 
                Parent.LibraryTabSplitContainer.Panel1Collapsed = true;
                Parent.LibraryTabSplitContainer.SplitterDistance = 0;
            }
            else
            {
                Parent.LibraryTabSplitContainer.Panel1Collapsed = false;
                Parent.LibraryNavigationContent.BackColor = System.Drawing.Color.AliceBlue;
                Parent.LibraryNavigationContent.Visible = true;
                Parent.LibraryNavigationContentFolders.Visible = true;
                List<LibraryFolderObjets> folders = new List<LibraryFolderObjets>();
                foreach (string dir in dirs)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    if (Settings.LibFolderShowHiden == false) { if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden)) { continue; } }
                    if (Settings.LibFolderShowUnixHiden == false) { if (dirInfo.Name[0] == '.') { continue; } }
                    folders.Add(new LibraryFolderObjets() { Name = dirInfo.Name, Path = dirInfo.FullName });
                    //Parent.LibraryNavigationContentFolders.Controls.Add(CreateFolderButton(dir));
                }
                if (folders.Count > 0) { Parent.LibraryNavigationContentFolders.DataSource = folders; }
            }

            string[] filesList = Directory.GetFiles(path);
            bool zerofiles = true;
            if (filesList.Length > 0)
            {
                Parent.LibraryTabSplitContainer2.Panel1Collapsed = true;
                Parent.LibraryTabSplitContainer2.Panel2Collapsed = false;
                Parent.LibraryNavigationContentFiles.Controls.Clear();
                //Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE Path LIKE '" + Database.EscapeString(path + MainWindow2.SeparatorChar) + "%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
                Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE replace(Path, '" + Database.EscapeString(path + MainWindow2.SeparatorChar) + "', '') NOT LIKE '%\\%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
                if(files.Count > 0)
                {
                    List<string> endFiles = new List<string>();
                    foreach (string file in files.Keys.ToArray())
                    {
                        if (Settings.LibFolderShowHiden == false)
                        { FileInfo dirInfo = new FileInfo(file); if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden)) { continue; } }
                        if (Settings.LibFolderShowUnixHiden == false)
                        { string name = file.Replace(path, "").TrimStart(MainWindow2.SeparatorChar); if (name[0] == '.') { continue; } }

                        string fi = file.Replace(path + MainWindow2.SeparatorChar, "");
                        if (!fi.Contains(MainWindow2.SeparatorChar)) { endFiles.Add(file); }
                    }

                    Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> tabInfo = GetTabInfoFromFiles(endFiles.ToArray());
                    if (tabInfo != null && tabInfo.Count > 0)
                    {
                        ContentBlocks(tabInfo, Parent.LibraryNavigationContentFiles, clear: true);
                        Parent.LibraryTabSplitContainer2.Panel2.AutoScrollPosition = new System.Drawing.Point(0, 0);
                        zerofiles = false;
                    }
                }
            }

            if (zerofiles)
            {
                Parent.LibraryTabSplitContainer.Panel2Collapsed = true;
                Parent.LibraryTabSplitContainer.SplitterDistance = Parent.LibraryTabSplitContainer.Height;
            }
            Parent.setLoadingState(false);
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

        public bool ContentBlocks(Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> infoTab, TableLayoutPanel contener, bool uniqueDir = true, bool clear = true)
        {
            Debug.WriteLine("--> ContentBlocks START <--");
            if (infoTab == null) { Debug.WriteLine("--> ContentBlocks NO FILE 0 <--"); return false; }
            if (infoTab.Count == 0) { Debug.WriteLine("--> ContentBlocks NO FILE 1 <--"); return false; }

            Bitmap baseCover = defaultCover;

            try
            {
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
                        if (file.EndsWith("Cover.jpg") || file.EndsWith("Cover.png")) { defaultCover = new Bitmap(file); }
                    }
                }

                //if (clear == true) {
                    contener.Controls.Clear();
                    contener.RowStyles.Clear();
                    contener.RowCount = 1;
                //}
                int contenerHeight = 0;
                bool first = true;
                contener.MinimumSize = new Size(contener.MinimumSize.Width, int.MaxValue);
                foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> albumT in infoTab)
                {
                    AlbumBlock albumBlock = new AlbumBlock(albumT, uniqueDir, defaultCover) { Dock = DockStyle.Top, Margin = new Padding(1) };
                    if (albumBlock.Height > 0)
                    {
                        int ah = ((albumBlock.Height > AlbumBlock.MinHeight) ? albumBlock.Height : AlbumBlock.MinHeight);
                        if (!first) { 
                            contener.RowCount += 1;
                            contener.RowStyles.Add(new RowStyle(SizeType.Absolute, ah));
                        }
                        else { 
                            first = false;
                            if (contener.RowStyles.Count > 0) { contener.RowStyles[0] = new RowStyle(SizeType.Absolute, ah); }
                            else { contener.RowStyles.Add(new RowStyle(SizeType.Absolute, ah)); }
                        }
                        contener.Controls.Add(albumBlock, 0, contener.RowCount - 1);
                        albumBlock.Visible = true;
                        //Debug.WriteLine("ah = " + ah);
                        contenerHeight += ah;
                    }
                }
                if(contenerHeight < AlbumBlock.MinHeight) { contenerHeight = AlbumBlock.MinHeight; }
                contener.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble;
                contener.MinimumSize = new Size(((Panel)contener.Parent.Parent).Width - 20, contenerHeight);
                contener.Height = contenerHeight;
                contener.Width = ((Panel)contener.Parent.Parent).Width - 20;
                //contener.Location = new Point(0,0);
                ((Panel)contener.Parent).Padding = new Padding(0, 0, 0, 0);
                ((Panel)contener.Parent).AutoScroll = true;
                //((Panel)contener.Parent).AutoScrollMinSize = new Size(((Panel)contener.Parent.Parent).Width - 6, contenerHeight);
                ((Panel)contener.Parent).AutoScrollMargin = new Size(0,0);
                //((Panel)contener.Parent).ClientSize = new Size(((Panel)contener.Parent.Parent).Width - 6, contenerHeight);
                ((Panel)contener.Parent).VerticalScroll.Value = 0;
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
        }

        private void BtnFolder_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("--> Folder_Click END <--");
            if (Parent.LibraryNavigationContentFolders.SelectedRows.Count <= 0) { return; }
            LibraryFolderObjets ob = (LibraryFolderObjets)Parent.LibraryNavigationContentFolders.SelectedRows[0].DataBoundItem;

            Parent.setLoadingState(true);
            DisplayPath(ob.Path);
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