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
using static System.Net.Mime.MediaTypeNames;
using AnotherMusicPlayer.Components;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Runtime.ConstrainedExecution;
using System.ComponentModel;

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
        BitmapImage defaultBICover = App.BitmapToBitmapImage(Properties.Resources.CoverImg);
        Bitmap FolderIcon = Icons.FromIconKind(IconKind.FolderOpen, 35, new SolidColorBrush(Colors.White));
        System.Drawing.Color ButtonBackColor = System.Drawing.Color.FromArgb(255, 60, 60, 60);
        System.Drawing.Color ButtonForeColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
        //System.Drawing.Color ButtonMouseOverBackColor = Common.LightenDrawingColor(ButtonBackColor, 30);
        System.Drawing.Color ButtonMouseOverBackColor = System.Drawing.Color.FromArgb(255, 90, 90, 90);
        //System.Drawing.Color ButtonMouseDownBackColor = Common.LightenDrawingColor(ButtonBackColor, 30);
        System.Drawing.Color ButtonMouseDownBackColor = System.Drawing.Color.FromArgb(255, 120, 120, 120);
        System.Drawing.Color ButtonBorderColor = System.Drawing.Color.White;

        Button BaseFolderButton = null;

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

            pathNavigator = new LibraryPathNavigator(this, Parent.LibraryNavigationPathContener, RootPath);

            App.bdd.DatabaseQuerys(new string[] { "UPDATE files SET Genres = NULL WHERE TRIM(Genres) = ''" }, true);

            LoadGenreList();

            Parent.LibraryTabSplitContainer.Panel1Collapsed = false;
            Parent.LibraryTabSplitContainer.Panel2Collapsed = true;
            Parent.LibraryNavigationContentFolders.Controls.Clear();
            Parent.LibrarySearchContent.Controls.Clear();
            if (Parent.LibraryNavigationContent.Controls.Count > 1)
            {
                for (int i = Parent.LibraryNavigationContent.Controls.Count - 1; i >= 1; i++)
                {
                    Parent.LibraryNavigationContent.Controls.RemoveAt(i);
                    Parent.LibraryNavigationContent.RowStyles.RemoveAt(i);
                }
                Parent.LibraryNavigationContent.RowCount = 1;
            }

            Parent.LibraryFiltersGenreList.Visible = false;
            Parent.LibraryFiltersGenreSearchBox.Visible = false;
            Parent.LibraryFiltersSearchBox.Visible = false;
            Parent.LibraryFiltersGenreSearchBox.Text = "";
            Parent.LibraryFiltersSearchBox.Text = "";

            Parent.LibraryFiltersMode.SelectedIndexChanged += LibraryFiltersMode_SelectionChanged;
            Parent.LibraryFiltersGenreList.SelectedIndexChanged += LibraryFiltersGenreList_SelectionChanged;
            Parent.LibraryFiltersGenreSearchBox.KeyDown += LibraryFiltersSearchBox_KeyDown;
            Parent.LibraryFiltersSearchBox.KeyDown += LibraryFiltersSearchBox_KeyDown;

            CreateWatcher();
            InvokeScan();
        }

        private double LibrarySearchContent_CalcCollumnWidth()
        {
            //double calc = (Parent.LibrarySearchContent.ActualWidth - Parent.LibrarySearchContentC4.Width - Parent.LibrarySearchContentC5.Width - 20) / 3;
            //return (calc > 0) ? calc : 0;
            return 0;
        }

        public void BuildBaseFolderButton() 
        {
            BaseFolderButton = new Button()
            {
                Text = "",
                Tag = null,
                Image = FolderIcon,
                ImageAlign = System.Drawing.ContentAlignment.TopCenter,
                TextImageRelation = TextImageRelation.ImageAboveText,
                Width = 90,
                Height = 90,
                Padding = new Padding(3, 3, 3, 3),
                FlatStyle = FlatStyle.Flat,
                BackColor = ButtonBackColor,
                ForeColor = ButtonForeColor
            };
            BaseFolderButton.FlatAppearance.MouseOverBackColor = ButtonMouseOverBackColor;
            BaseFolderButton.FlatAppearance.MouseDownBackColor = ButtonMouseDownBackColor;
            BaseFolderButton.FlatAppearance.CheckedBackColor = ButtonMouseDownBackColor;
            BaseFolderButton.FlatAppearance.BorderColor = ButtonBorderColor;
            BaseFolderButton.FlatAppearance.BorderSize = 1;
            BaseFolderButton.ContextMenuStrip = MakeContextMenu(BaseFolderButton, "folder");
        }

        public Button CreateFolderButton(string path) {
            if (BaseFolderButton == null) { BuildBaseFolderButton(); }
            Button btn = BaseFolderButton.Clone();
            btn.Text = path.Substring(path.LastIndexOf(MainWindow2.SeparatorChar) + 1);
            btn.Tag = path;
            btn.Image = FolderIcon;
            btn.Click += BtnFolder_Click;
            return btn;
        }

        /// <summary> Update var RootPath and return if value is valid or not </summary>
        public bool UpdateRootPath(string root) { if (Directory.Exists(root)) { RootPath = root; return true; } else { return false; } }

        public void DisplayPath(string path = null)
        {
            if (path == null) { path = Settings.LibFolder; }
            if (!Directory.Exists(path)) { return; }
            _CurrentPath = path;

            Parent.LibraryFiltersSearchBox.Visible = false;
            Parent.LibraryFiltersSearchBox.Text = "";

            Parent.LibraryFiltersMode.SelectedIndex = 0;
            Parent.LibraryFiltersGenreList.SelectedIndex = 0;
            Parent.LibraryFiltersGenreList.Visible = false;

            pathNavigator.Display(path);
            Parent.LibraryNavigationContentFolders.SuspendLayout();
            Parent.LibraryNavigationContentFolders.Tag = path;
            Parent.LibraryNavigationContentFolders.Controls.Clear();
            Parent.LibraryNavigationContentFolders.AutoScrollOffset = new System.Drawing.Point(0, 0);
            Parent.LibraryNavigationContentFolders.ContextMenuStrip = MakeContextMenu(Parent.LibraryNavigationContent, "folder", (path != Settings.LibFolder) ? true : false, (path != Settings.LibFolder) ? path : null);
            Parent.LibraryNavigationContentFolders.ContextMenuStrip.BackColor = System.Drawing.Color.FromArgb(255, 30, 30, 30);
            Parent.LibraryNavigationContentFolders.ContextMenuStrip.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);

            if (Parent.LibraryNavigationContent.Controls.Count > 1)
            {
                for (int i = Parent.LibraryNavigationContent.Controls.Count - 1; i >= 1; i--)
                {
                    Parent.LibraryNavigationContent.Controls.RemoveAt(i);
                    Parent.LibraryNavigationContent.RowStyles.RemoveAt(i);
                }
                Parent.LibraryNavigationContent.RowCount = 1;
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                if (Settings.LibFolderShowHiden == false)
                { DirectoryInfo dirInfo = new DirectoryInfo(dir); if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden)) { continue; } }
                if (Settings.LibFolderShowUnixHiden == false) 
                { string name = dir.Replace(path, "").TrimStart(MainWindow2.SeparatorChar); if (name[0] == '.') { continue; } }

                Parent.LibraryNavigationContentFolders.Controls.Add(CreateFolderButton(dir));
            }

            string[] filesList = Directory.GetFiles(path);
            if (filesList.Length > 0)
            {
                //Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE Path LIKE '" + Database.EscapeString(path + MainWindow2.SeparatorChar) + "%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
                Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT * FROM files WHERE replace(Path, '" + Database.EscapeString(path + MainWindow2.SeparatorChar) + "', '') NOT LIKE '%\\%' ORDER BY LOWER(Album) ASC, Disc ASC, Track ASC, Name ASC, Path ASC", "Path");
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
                ContentBlocks(tabInfo, Parent.LibraryNavigationContent, clear: false);
            }
            Parent.LibraryNavigationContentFolders.ResumeLayout();
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

                if (clear == true && contener.Controls.Count > 1) {
                    for (int i = contener.Controls.Count - 1; i >= 1; i++) {
                        contener.Controls.RemoveAt(i);
                        contener.RowStyles.RemoveAt(i);
                    }
                    contener.RowCount = 1;
                }

                foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> albumT in infoTab)
                {
                    AlbumBlock albumBlock = new AlbumBlock(albumT, uniqueDir, defaultCover) { Dock = DockStyle.Top };
                    contener.RowCount += 1;
                    contener.RowStyles.Add(new RowStyle(SizeType.AutoSize, 50));
                    contener.Controls.Add(albumBlock);
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