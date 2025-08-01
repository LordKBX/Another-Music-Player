﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using Control = System.Windows.Forms.Control;
using Button = System.Windows.Forms.Button;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Timer = System.Windows.Forms.Timer;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.IO;
using static AnotherMusicPlayer.Player;
using System.Threading.Tasks;

namespace AnotherMusicPlayer.MainWindow2Space
{
    public partial class MainWindow2 : Form
    {
        private Timer LoadTimer = new Timer();

        /// <summary> Used for storing object dependant of the Operating system </summary>
        private Dictionary<string, object> ListReferences = new Dictionary<string, object>();

        private static Bitmap IconPlayback;
        private static Bitmap IconLibrary;
        private static Bitmap IconPlayLists;
        private static Bitmap IconSettings;

        private static Bitmap IconOpen;
        private static Bitmap IconPrevious;
        private static Bitmap IconPlay;
        private static Bitmap IconPause;
        private static Bitmap IconNext;
        private static Bitmap IconRepeat;
        private static Bitmap IconRepeatOnce;
        private static Bitmap IconRepeatOff;
        private static Bitmap IconShuffle;
        private static Bitmap IconClearList;

        private ObservableCollection<PlayListViewItem> PlayListItems = new ObservableCollection<PlayListViewItem>();
        private int PlaylistIndexAtLoading = 0;

        private TabbedThumbnail customThumbnail;
        private System.Drawing.Icon ThumbnailIconPrev = null;
        private System.Drawing.Icon ThumbnailIconPlay = null;
        private System.Drawing.Icon ThumbnailIconPause = null;
        private System.Drawing.Icon ThumbnailIconNext = null;
        private ThumbnailToolBarButton buttonPrev = null;
        private ThumbnailToolBarButton buttonPlay = null;
        private ThumbnailToolBarButton buttonNext = null;

        /// <summary> Char used in first collumn of PlayListView for displaying current played/selected media </summary>
        public static string PlayListSelectionChar = "▶";
        /// <summary> Short notation for ystem.IO.Path.DirectorySeparatorChar </summary>
        public static char SeparatorChar = System.IO.Path.DirectorySeparatorChar;
        /// <summary> Base Diractory of the application </summary>
        public static string BaseDir = AppDomain.CurrentDomain.BaseDirectory + SeparatorChar;

        private List<Lyrics> lyricsWindowsList = new List<Lyrics>();

        public Library library;
        public PlayLists playLists;

        public void setLoadingState(bool state, string message = null)
        {
            if (this.InvokeRequired) { this.Invoke(() => { setLoadingState(state, message); }); return; }
            if (state)
            {
                OverPanel.Dock = DockStyle.Fill;
                OverPanel.Visible = true;
                if (message == null) { message = "Loading ..."; }
                OverPanelLabel.Text = message;
            }
            else
            {
                OverPanel.Dock = DockStyle.Top;
                OverPanel.Height = 0;
                OverPanel.Visible = false;
            }
        }
        public void setMetadataScanningState(bool state, int nb = 0)
        {
            try
            {
                if (this.InvokeRequired) { this.Invoke(() => { setMetadataScanningState(state, nb); }); return; }
                if (state == true) { GridScanMetadata.Visible = true; GridScanMetadataNb.Text = "" + nb; }
                else { GridScanMetadata.Visible = false; }
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }

        private PlayBackContextMenu playBackContextMenu;

        public void InitIcons()
        {
            Common.SetGlobalColor(this);
            //this.BackColor = App.style.GetColor("WindowBackColor");

            SolidColorBrush PlaybackIconDefaultBrush = new SolidColorBrush(App.DrawingColorToMediaColor(App.style.GetColor("GlobalIconColor")));
            int PlaybackButtonIconSize = App.style.GetValue<int>("GlobalIconSize", 24);

            #region Define Tabs Icons
            int TabIconSize = App.style.GetValue<int>("TabIconSize", 24);
            SolidColorBrush IconTabBrush = new SolidColorBrush(App.DrawingColorToMediaColor(App.style.GetColor("TabIconColor")));
            IconPlayback = Icons.FromIconKind(IconKind.Music, TabIconSize, IconTabBrush);
            IconLibrary = Icons.FromIconKind(IconKind.FolderMusic, TabIconSize, IconTabBrush);
            IconPlayLists = Icons.FromIconKind(IconKind.PlaylistMusic, TabIconSize, IconTabBrush);
            IconSettings = Icons.FromIconKind(IconKind.Cog, TabIconSize, IconTabBrush);

            PlaybackTab.Icon = IconPlayback; Common.SetTabStyle(PlaybackTab);
            LibraryTab.Icon = IconLibrary; Common.SetTabStyle(LibraryTab);
            PlayListsTab.Icon = IconPlayLists; Common.SetTabStyle(PlayListsTab);
            SettingsTab.Icon = IconSettings; Common.SetTabStyle(SettingsTab);
            #endregion

            IconOpen = Icons.FromIconKind(IconKind.FolderOpen, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconPrevious = Icons.FromIconKind(IconKind.SkipBackward, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconPlay = Icons.FromIconKind(IconKind.Play, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconPause = Icons.FromIconKind(IconKind.Pause, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconNext = Icons.FromIconKind(IconKind.SkipForward, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconRepeat = Icons.FromIconKind(IconKind.Repeat, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconRepeatOnce = Icons.FromIconKind(IconKind.RepeatOnce, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconRepeatOff = Icons.FromIconKind(IconKind.RepeatOff, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconShuffle = Icons.FromIconKind(IconKind.Shuffle, PlaybackButtonIconSize, PlaybackIconDefaultBrush);
            IconClearList = Icons.FromIconKind(IconKind.PlaylistRemove, PlaybackButtonIconSize, PlaybackIconDefaultBrush);

            BtnOpen.BackgroundImage = IconOpen;
            BtnPrevious.BackgroundImage = IconPrevious;
            BtnPlayPause.BackgroundImage = (Player.LatestPlayerStatus == PlayerStatus.Play) ? IconPause : IconPlay;
            BtnNext.BackgroundImage = IconNext;
            BtnRepeat.BackgroundImage = (Player.IsRepeat()) ? IconRepeatOnce : (Player.IsLoop()) ? IconRepeat : IconRepeatOff;
            BtnShuffle.BackgroundImage = IconShuffle;
            BtnClearList.BackgroundImage = IconClearList;

            ThumbnailIconPrev = Icon.FromHandle(IconPrevious.GetHicon());
            ThumbnailIconPlay = Icon.FromHandle(IconPlay.GetHicon());
            ThumbnailIconPause = Icon.FromHandle(IconPause.GetHicon());
            ThumbnailIconNext = Icon.FromHandle(IconNext.GetHicon());

            if (buttonPrev == null)
            {
                buttonPrev = new ThumbnailToolBarButton(ThumbnailIconPrev, "IconPrevious");
                buttonPlay = new ThumbnailToolBarButton((Player.LatestPlayerStatus == PlayerStatus.Play) ? ThumbnailIconPause : ThumbnailIconPlay, "IconPlay");
                buttonNext = new ThumbnailToolBarButton(ThumbnailIconNext, "IconNext");
            }
            else
            {
                buttonPrev.Icon = ThumbnailIconPrev;
                buttonPlay.Icon = (Player.LatestPlayerStatus == PlayerStatus.Play) ? ThumbnailIconPlay : ThumbnailIconPause;
                buttonNext.Icon = ThumbnailIconNext;
            }
        }

        /// <summary> Object music player </summary>
        public MainWindow2()
        {
            InitializeComponent(); InitIcons();
            SettingsTabLangComboBox.Items.AddRange(App.Languages.ToArray());
            playBackContextMenu = MakePlayBackContextMenu();

            library = new Library(this);
            playLists = new PlayLists(this);

            PlaylistIndexAtLoading = Settings.LastPlaylistIndex;
            if (Settings.LastRepeatStatus == 0) { Player.Repeat(false); Player.Loop(false); }
            else if (Settings.LastRepeatStatus == 1) { Player.Repeat(true); Player.Loop(false); }
            else { Player.Repeat(false); Player.Loop(true); }
            Debug.WriteLine("PlaylistIndexAtLoading = " + PlaylistIndexAtLoading);

            #region Window displasment gestion
            MainWIndowHead.MouseDown += FormDragable_MouseDown;
            MainWIndowHead.MouseMove += FormDragable_MouseMove;
            MainWIndowHead.MouseUp += FormDragable_MouseUp;
            TitleLabel.MouseDown += FormDragable_MouseDown;
            TitleLabel.MouseMove += FormDragable_MouseMove;
            TitleLabel.MouseUp += FormDragable_MouseUp;
            #endregion

            #region Window resize gestion
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.ResizeRedraw = true;
            GripButton.Tag = "MainWindow";
            GripButton.MouseDown += SizerMouseDown;
            GripButton.MouseMove += SizerMouseMove;
            GripButton.MouseUp += SizerMouseUp;
            #endregion

            try
            {
                SetTitle(null);
                TabControler.Renderer.BorderColor = Color.FromKnownColor(System.Drawing.KnownColor.White);

                #region Define playback elements
                BtnOpen.Click += (object sender, EventArgs e) => { };
                BtnPrevious.Click += (object sender, EventArgs e) => { Player.Stop(Player.GetCurrentFile()); Player.PlaylistPrevious(); };
                BtnPlayPause.Click += (object sender, EventArgs e) => { PlayPause(); };
                BtnNext.Click += (object sender, EventArgs e) => { Player.Stop(Player.GetCurrentFile()); Player.PlaylistNext(); };
                BtnShuffle.Click += (object sender, EventArgs e) => { Player.PlaylistRandomize(); };
                BtnClearList.Click += (object sender, EventArgs e) => { Player.PlaylistClear(); UpdateLeftPannelMediaInfo(null); PlaybackPositionLabel.Text = "--"; };
                BtnRepeat.Click += (object sender, EventArgs e) =>
                {
                    if (Player.IsLoop()) { Player.Repeat(true); Player.Loop(false); }
                    else if (Player.IsRepeat()) { Player.Repeat(false); Player.Loop(false); }
                    else { Player.Repeat(false); Player.Loop(true); }
                    BtnRepeat.BackgroundImage = (Player.IsRepeat()) ? IconRepeatOnce : (Player.IsLoop()) ? IconRepeat : IconRepeatOff;
                };
                playbackProgressBar.Change += PlaybackProgressBar_Change;
                #endregion

                #region Define PlaybackTabDataGridView initial data
                PlaybackTabDataGridView.AutoGenerateColumns = false;
                //PlayListItems.Add(new PlayListViewItem() { Name = "Loading ..." });
                if (PlayListItems.Count > 0) { PlaybackTabDataGridView.DataSource = PlayListItems; }
                #endregion

                #region Define Playback Left Pannel Actions
                PlaybackTabLyricsButton.Visible = false;
                PlaybackTabLyricsButton.Click += PlaybackTabLyricsButton_Click;
                PlaybackTabRatting.RateChanged += PlaybackTabRatting_RateChanged;
                #endregion

                #region Define CustomThumbnail
                customThumbnail = new TabbedThumbnail(this.Handle, this.Handle);

                buttonPrev.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { Player.PlaylistPrevious(); };
                buttonPlay.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { PlayPause(); };
                buttonNext.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { Player.PlaylistNext(); };
                TaskbarManager.Instance.ThumbnailToolBars.AddButtons(this.Handle, new ThumbnailToolBarButton[] { buttonPrev, buttonPlay, buttonNext });
                customThumbnail.SetWindowIcon(Properties.Resources.icon_large);
                #endregion

                #region Initialisation Library Filters
                LibraryFiltersMode.SelectedIndex = 0;
                LibraryFiltersGenreList.Visible = false;
                LibraryFiltersGenreSearchBox.Visible = false;
                LibraryFiltersSearchBox.Visible = false;
                #endregion

                #region Define Player class events
                Player.PlaylistChanged += Player_PlaylistChanged;
                Player.PositionChanged += Player_PositionChanged;
                Player.LengthChanged += Player_LengthChanged;
                Player.PlaylistPositionChanged += Player_PlaylistPositionChanged;
                Player.PlayStoped += Player_PlayStoped;

                Player.Paused += Player_StatusChange;
                Player.Stoped += Player_StatusChange;
                Player.Started += Player_StatusChange;

                RadioPlayer.StatusUpdated += Player_StatusChange;
                RadioPlayer.RadioPlayerPositionChanged += (PlayerPositionChangedEventParams e) => { ChangeDisplayPlaybackPosition(e.Position, 0); };
                #endregion

                PlaybackTabDataGridView.CellDoubleClick += PlaybackTabDataGridView_CellDoubleClick;
                PlaybackTabDataGridView.CellMouseClick += PlaybackTabDataGridView_CellMouseClick;

                KeyboardLocal.Init(this);
                KeyboardGlobal.Init();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }

            LoadTimer.Tick += LoadTimer_Tick;
            LoadTimer.Interval = 200;
            LoadTimer.Start();

            this.FormClosed += MainWindow2_FormClosed;
        }

        public static void PlayPause()
        {
            Debug.WriteLine("Player.Mode=" + Player.Mode);
            if (Player.Mode == Modes.Radio) {
                Debug.WriteLine("RadioPlayer.IsPlaying = " + ((RadioPlayer.IsPlaying)?"True":"False"));
                if (RadioPlayer.IsPlaying) { RadioPlayer.Stop(); } else { Task<bool> a = RadioPlayer.Start(); } 
            }
            else { if (Player.IsPlaying()) { Player.Pause(); } else { Player.Play(); } }
        }

        private void PlaybackTabDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex < 0) { return; }
                else if (e.RowIndex >= PlaybackTabDataGridView.Rows.Count) { return; }
                try
                {
                    PlaybackTabDataGridView.Rows[e.RowIndex].Selected = true;
                    playBackContextMenu.Show(System.Windows.Forms.Cursor.Position);
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
            }
        }

        private void PlaybackTabDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            else if (e.RowIndex >= PlaybackTabDataGridView.Rows.Count) { return; }
            try
            {
                if (Player.Index >= 0)
                {
                    ((PlayListViewItem)PlaybackTabDataGridView.Rows[Player.Index].DataBoundItem).Selected = "";
                    Player.StopAll();
                }
                Player.PlaylistReadIndex(e.RowIndex);
                ((PlayListViewItem)PlaybackTabDataGridView.Rows[e.RowIndex].DataBoundItem).Selected = PlayListSelectionChar;
                PlaybackTabDataGridView.Rows[e.RowIndex].Selected = true;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }

        private void MainWindow2_Resize(object sender, EventArgs e)
        {
        }

        private void MainWindow2_FormClosed(object sender, FormClosedEventArgs e) { KeyboardGlobal.Kill(); }

        private void Player_StatusChange()
        {
            if (this.InvokeRequired) { this.Invoke(() => { Player_StatusChange(); }); return; }
            try
            {
                if (Player.LatestPlayerStatus == PlayerStatus.Stop && RadioPlayer.IsPlaying == false) 
                { UpdateLeftPannelMediaInfo(null); PlaybackPositionLabel.Text = "--"; }
                if (Player.Mode == Modes.Radio)
                {
                    BtnPlayPause.BackgroundImage = (!RadioPlayer.IsPlaying) ? IconPlay : IconPause;
                    buttonPlay.Icon = (!RadioPlayer.IsPlaying) ? ThumbnailIconPlay : ThumbnailIconPause;

                    PlaybackPositionLabel.Text = App.GetTranslation("PlaybackPositionLabelRadio").Replace("%X%", RadioPlayer.GetName());
                    SetTitle(PlaybackPositionLabel.Text);
                    playbackProgressBar.Value = 0;
                    playbackProgressBar.Enabled = false;
                    PlayListViewItem item = new PlayListViewItem() { 
                        Name = PlaybackPositionLabel.Text, Album = "", AlbumArtists="", Composers="", Performers = "",
                        Duration =0, DurationS="--:--", OriginPath = "", Path = Player.GetCurrentFile(), Selected = MainWindow2.PlayListSelectionChar };
                    PlaybackTabDataGridView.DataSource = null;
                    PlaybackTabDataGridView.Invalidate();
                    PlaybackTabDataGridView.DataSource = new ObservableCollection<PlayListViewItem>() { item };
                    PlaybackTabDataGridView.Invalidate();

                    if (RadioPlayer.IsPlaying) { this.UpdateLeftPannelMediaInfo(new PlayListViewItem() { Path = Player.GetCurrentFile(), Name = RadioPlayer.GetName() }); }
                }
                else
                {
                    BtnPlayPause.BackgroundImage = (Player.LatestPlayerStatus == PlayerStatus.Play) ? IconPlay : IconPause;
                    buttonPlay.Icon = (Player.LatestPlayerStatus == PlayerStatus.Play) ? ThumbnailIconPlay : ThumbnailIconPause;
                }

            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }

        private void LoadTimer_Tick(object? sender, EventArgs e)
        {
            if (LoadTimer == null) { return; }
            LoadTimer.Dispose();
            LoadTimer = null;
            SettingsManagment.Init(this);
            Translate();
            //library.DisplayPathAsync();
            LibraryLoadOldPlaylist();
            library.DisplayPath();
            library.InvokeScan();
            this.Resize += MainWindow2_Resize;
        }

        private void PlaybackTabRatting_RateChanged(Rating2 sender, double value)
        {
            if (loadingRating) { return; }
            Debug.WriteLine("New Ratting = " + value);
            if (PlaybackTabRatting.Tag == null) { return; }
            if (PlaybackTabRatting.Tag.GetType() != typeof(string)) { return; }
            string path = ("" + PlaybackTabRatting.Tag).Trim();
            if (!File.Exists(path)) { return; }
            FilesTags.SaveRating(path, value);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        #region MainWindow Events
        private void PlaybackProgressBar_Change(object sender, int value)
        {
            long duration = Player.Length();
            Player.Position(null, Convert.ToInt64(Math.Truncate(Convert.ToDouble(value) * duration / playbackProgressBar.MaxValue)));
        }

        private void PlaybackTabLyricsButton_Click(object sender, EventArgs e)
        {
            if (sender == null) { Debug.WriteLine(" > ER 1 < "); return; }
            if (sender.GetType() != typeof(Button)) { Debug.WriteLine(" > ER 2 < "); return; }
            if (PlaybackTabLyricsButton.Tag == null) { Debug.WriteLine(" > ER 3 < "); return; }
            MediaItem data = PlaybackTabLyricsButton.Tag as MediaItem;
            Lyrics wl = new Lyrics(data);
            wl.Show();
            lyricsWindowsList.Add(wl);
        }

        private void CloseLyricsWindows()
        {
            if (lyricsWindowsList.Count > 0)
                for (int i = lyricsWindowsList.Count - 1; i >= 0; i--)
                { lyricsWindowsList[i].Dispose(); lyricsWindowsList.RemoveAt(i); }
        }
        #endregion

        #region UI Translation
        public void Translate()
        {
            if (this.InvokeRequired) { this.Invoke(() => { this.Translate(); }); return; }
            AnotherMusicPlayer.MainWindow2Space.Translation.Translate(this);
            playLists.Init();

            Graphics g = this.CreateGraphics();
            string drawString = PlaylistsTree.Nodes[0].Nodes[0].Text;
            System.Drawing.Font drawFont = this.Font;

            SizeF layoutSize = new SizeF(500.0F, 50.0F);

            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.NoWrap;
            drawFormat.Alignment = StringAlignment.Near;
            drawFormat.LineAlignment = StringAlignment.Near;

            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString(drawString, drawFont, layoutSize, drawFormat);

            PlayListsTabTableLayoutPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, stringSize.Width + 60);
        }
        public void UpdateStyle() {
            if (this.InvokeRequired) { this.Invoke(() => { UpdateStyle(); }); return; }
            if (Settings.StyleName != App.style.GetType().Name)
            {
                App.UpdateStyle();
                InitIcons();
            }
        }
        public void AlwaysOnTop(bool val) { this.TopMost = val; }
        private void ReplaceElementDualText(Control ctrl, string text) { ctrl.Text = text; App.SetToolTip(ctrl, text); }
        #endregion

        #region Playback change functions
        private void ChangeDisplayPlaybackPosition(long position, long duration)
        {
            if (this.InvokeRequired) { this.Invoke(() => { ChangeDisplayPlaybackPosition(position, duration); }); return; }
            try
            {
                DisplayPlaybackPosition.Text = App.displayTime(position);

                if (Player.Mode == Modes.File)
                {
                    DisplayPlaybackSize.Text = App.displayTime(duration);
                    playbackProgressBar.Value = Convert.ToInt32(Math.Round(Convert.ToDouble(position) * playbackProgressBar.MaxValue / duration, 0, MidpointRounding.ToEven));
                    Settings.LastPlaylistDuration = position;
                    Settings.SaveSettings();
                    if (Settings.AutoCloseLyrics) { CloseLyricsWindows(); }
                }
                else
                {
                    DisplayPlaybackSize.Text = "--:--";
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }

        private void ChangePlaylistPosition(int position)
        {
            try
            {
                if (this.InvokeRequired) { this.Invoke(() => { ChangePlaylistPosition(position); }); return; }
                if (PlaybackTabDataGridView.Rows.Count <= 0) { return; }
                int pos = position;
                if (pos < 0) { pos = 0; }
                else if (pos >= PlaybackTabDataGridView.Rows.Count) { pos = PlaybackTabDataGridView.Rows.Count - 1; }

                if (Player.Mode == Modes.Radio)
                {
                    SetTitle(RadioPlayer.GetName());
                    playbackProgressBar.Value = 0;
                    playbackProgressBar.Enabled = false;
                    PlaybackPositionLabel.Text = App.GetTranslation("PlaybackPositionLabelRadio").Replace("%X%", RadioPlayer.GetName());
                }
                else if (Player.Mode == Modes.File)
                {
                    try
                    {
                        playbackProgressBar.Enabled = true;
                        PlaybackTabDataGridView.Rows[pos].Selected = true;
                        PlaybackTabDataGridView.FirstDisplayedScrollingRowIndex = pos;
                        foreach (PlayListViewItem tm in PlayListItems) { tm.Selected = ""; }
                        PlayListViewItem item = ((PlayListViewItem)PlaybackTabDataGridView.Rows[pos].DataBoundItem);
                        item.Selected = PlayListSelectionChar;
                        string title = item.Name;
                        string arts = item.Artists;
                        if (item.Album != null && item.Album.Trim().Length > 0) { title += " - " + item.Album.Trim(); }
                        else if (arts != null && arts.Trim().Length > 0) { title += " - " + arts.Trim(); }

                        SetTitle(title);
                        UpdateLeftPannelMediaInfo(item);
                        PlaybackPositionLabel.Text = App.GetTranslation("PlaybackPositionLabel").Replace("%X%", "" + Player.PlayList.Count).Replace("%Y%", "" + (pos + 1));
                    }
                    catch (Exception ex1) { Debug.WriteLine(ex1.Message + "\r\n" + ex1.StackTrace); }
                }

                Settings.LastPlaylistIndex = pos;
                Settings.LastPlaylistDuration = 0;
                Settings.SaveSettings();
                if (Settings.AutoCloseLyrics) { CloseLyricsWindows(); }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }
        #endregion

        #region Player event functions
        private void Player_PlayStoped(PlayerPositionChangedEventParams e)
        {
            ChangeDisplayPlaybackPosition(e.Position, e.duration);
            BtnPlayPause.BackgroundImage = (Player.LatestPlayerStatus == PlayerStatus.Play) ? IconPlay : IconPause;
        }
        private void Player_PositionChanged(PlayerPositionChangedEventParams e) { ChangeDisplayPlaybackPosition(e.Position, e.duration); }
        private void Player_LengthChanged(PlayerLengthChangedEventParams e) { ChangeDisplayPlaybackPosition(-1, e.duration); }

        private void Player_PlaylistPositionChanged(PlayerPlaylistPositionChangeParams e) { 
            if (Player.Mode == Modes.Radio) { ChangeDisplayPlaybackPosition(0, 0); } 
            else { ChangePlaylistPosition(e.Position); } 
        }

        private void Player_PlaylistChanged(PlayerPlaylistChangeParams e)
        {
            try
            {
                if (this.InvokeRequired) { this.Invoke(() => { Player_PlaylistChanged(e); }); return; }

                PlayListItems.Clear();
                if (Player.Mode == Modes.Radio)
                { ChangeDisplayPlaybackPosition(0, 0); }
                else if (Player.Mode == Modes.File)
                {
                    try { foreach (string file in e.playlist) { PlayListItems.Add(PlayListViewItem.FromFilePath(file)); } }
                    catch (Exception) { }
                    Debug.WriteLine("PlayListItems.Count = " + PlayListItems.Count);
                    PlaybackTabDataGridView.DataSource = null;
                    if (PlayListItems.Count > 0) { PlaybackTabDataGridView.DataSource = PlayListItems; }
                    ChangePlaylistPosition(PlaylistIndexAtLoading);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }
        #endregion

        private void LibraryLoadOldPlaylist()
        {
            try
            {
                PlayListItems.Clear();
                Thread objThread = new Thread(new ParameterizedThreadStart(LibraryLoadOldPlaylistP2));
                objThread.IsBackground = true;
                objThread.Priority = ThreadPriority.AboveNormal;
                objThread.Start(null);
            }
            catch { }
        }

        private void LibraryLoadOldPlaylistP2(object param = null)
        {
            try
            {
                Dictionary<string, Dictionary<string, object>> LastPlaylist = App.bdd.DatabaseQuery("SELECT MIndex,Path1,Path2 FROM queue ORDER BY MIndex ASC", "MIndex");
                if (LastPlaylist != null)
                {
                    if (LastPlaylist.Count > 0)
                    {
                        Debug.WriteLine("Old PlayList detected");
                        //Debug.WriteLine(JsonConvert.SerializeObject(LastPlaylist));
                        List<string> gl = new List<string>();
                        int fails = 0;
                        bool radio = false;
                        foreach (KeyValuePair<string, Dictionary<string, object>> fi in LastPlaylist)
                        {
                            string path1 = (string)fi.Value["Path1"];
                            string path2 = (fi.Value["Path2"] == null) ? null : ((string)fi.Value["Path2"]).Trim();
                            if (path2 != null && path2 != "")
                            {
                                if (System.IO.File.Exists(path2)) { gl.Add(path2); }
                                else
                                {
                                    if (System.IO.File.Exists(path1)) { gl.Add(path1); } else { fails += 1; }
                                }
                            }
                            else
                            {
                                if (path1.StartsWith("Radio|"))
                                {
                                    Debug.WriteLine(" = = = > RADIO 0000");
                                    gl.Clear();
                                    gl.Add(path1);
                                    radio = true;
                                    break;
                                }
                                else
                                {
                                    if (System.IO.File.Exists(path1)) { gl.Add(path1); }
                                    else { fails += 1; }
                                }
                            }
                        }
                        int newIndex = -1;
                        if (fails > 0) { newIndex = 0; }
                        else { newIndex = PlaylistIndexAtLoading; }

                        if (radio == true)
                        {
                            Debug.WriteLine(" = = = > RADIO");
                            Debug.WriteLine(gl[0]);
                            string[] rtab = gl[0].Split('|');
                            if (rtab[1].Trim() != "")
                            {
                                Dictionary<string, object> CurentRadio = App.bdd.DatabaseQuery("SELECT * FROM radios WHERE RID = " + rtab[1], "RID")[rtab[1]];
                                //Debug.WriteLine(JsonConvert.SerializeObject(CurentRadio));
                                Player.OpenStream(CurentRadio["Url"] as string, (CurentRadio["FType"] as string == "M3u") ? RadioPlayer.RadioType.M3u : RadioPlayer.RadioType.Stream, CurentRadio["RID"] as string, CurentRadio["Name"] as string, Settings.StartUpPlay, CurentRadio["UrlPrefix"] as string);
                            }
                            else { Player.OpenStream(gl[0], RadioPlayer.RadioType.Stream, "", "", Settings.StartUpPlay, ""); }
                        }
                        else { Open(gl.ToArray(), false, false, newIndex, Settings.StartUpPlay, Settings.LastPlaylistDuration); }
                        //player.Stop();
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }

        private bool Open(string[] files, bool replace = false, bool random = false, int playIndex = 0, bool autoplay = false, long statupDuration = 0)
        {
            Debug.WriteLine("--> Open <--");
            if (replace == true) { Player.PlaylistClear(); }
            return Player.PlaylistEnqueue(files, random, playIndex, statupDuration, autoplay);
        }

        private bool loadingRating = false;
        private void UpdateLeftPannelMediaInfo(PlayListViewItem item)
        {
            Dictionary<string, Dictionary<string, object>> data = null;
            string[] rtab = null;
            try
            {
                if (item == null) {
                    string? r = Player.GetCurrentFile();
                    if (r != null && r.Trim().Length > 0) { item = PlayListViewItem.FromFilePath(r); }
                }
                if (item == null) 
                {
                    FileCover.BackgroundImage = Properties.Resources.album_large;
                    ReplaceElementDualText(PlaybackTabTitleLabelValue, "--");
                    PlaybackTabAlbumLabelInfo.Visible = false; PlaybackTabAlbumLabelValue.Visible = false;
                    PlaybackTabArtistsLabelInfo.Visible = false; PlaybackTabArtistsLabelValue.Visible = false;
                    PlaybackTabGenresLabelInfo.Visible = false; PlaybackTabGenresLabelValue.Visible = false;
                    PlaybackTabDurationLabelInfo.Visible = false; PlaybackTabDurationLabelValue.Visible = false;
                    PlaybackTabLyricsButton.Visible = false;
                    PlaybackTabRatting.Visible = false;
                    return;
                }

                string path = item.Path;
                if (path == null)
                {
                    if (Player.PlayList.Count > 0) { path = Player.PlayList[Player.Index]; }
                    Debug.WriteLine("--> path = '" + path + "' <--");
                }

                MediaItem item2 = null;
                if (path.StartsWith("Radio|"))
                {
                    rtab = path.Split('|');
                    if (rtab[1].Trim() != "")
                    {
                        data = App.bdd.DatabaseQuery("SELECT * FROM radios WHERE RID = " + rtab[1], "RID");
                        item.Name = data["" + rtab[1].Trim()]["Name"] as string;
                    }
                }
                else
                {
                    item2 = FilesTags.MediaInfo(item.Path, false);
                }

                BitmapImage bi = null;
                if (item.Path.StartsWith("Radio|"))
                {
                    string logo = data["" + rtab[1].Trim()]["Logo"] as string;
                    string[] logoTab = logo.Split(',');
                    if (logoTab.Length > 1) { logo = logoTab[1]; }
                    try { bi = BitmapMagic.Base64StringToBitmap(logo); }
                    catch (Exception err)
                    {
                        Debug.WriteLine("data[rtab[1].Trim()][\"Logo\"] = " + logo);
                        Debug.WriteLine(JsonConvert.SerializeObject(err));
                    }
                }
                else { bi = FilesTags.MediaPicture(item.Path, App.bdd, true, (Settings.MemoryUsage == 0) ? 150 : 250, (Settings.MemoryUsage == 0) ? 150 : 250); }

                if (bi == null) { FileCover.BackgroundImage = Properties.Resources.album_large; }
                else { FileCover.BackgroundImage = BitmapMagic.BitmapImage2Bitmap(bi); }
                Debug.WriteLine(JsonConvert.SerializeObject(item));
                PlaybackTabTitleLabelValue.Visible = true;
                ReplaceElementDualText(PlaybackTabTitleLabelValue, item.Name);
                if (item.Album.Trim().Length > 0)
                { PlaybackTabAlbumLabelInfo.Visible = true; PlaybackTabAlbumLabelValue.Visible = true; ReplaceElementDualText(PlaybackTabAlbumLabelValue, item.Album); }
                else { PlaybackTabAlbumLabelInfo.Visible = false; PlaybackTabAlbumLabelValue.Visible = false; }

                if (item.Artists.Trim().Length > 0)
                { PlaybackTabArtistsLabelInfo.Visible = true; PlaybackTabArtistsLabelValue.Visible = true; ReplaceElementDualText(PlaybackTabArtistsLabelValue, item.Artists); }
                else { PlaybackTabArtistsLabelInfo.Visible = false; PlaybackTabArtistsLabelValue.Visible = false; }

                if (path.StartsWith("Radio|") || item2.Genres == null || item2.Genres.Trim().Length == 0)
                { PlaybackTabGenresLabelInfo.Visible = false; PlaybackTabGenresLabelValue.Visible = false; }
                else { PlaybackTabGenresLabelInfo.Visible = true; PlaybackTabGenresLabelValue.Visible = true; ReplaceElementDualText(PlaybackTabGenresLabelValue, item2.Genres); }

                if (path.StartsWith("Radio|"))
                { PlaybackTabDurationLabelInfo.Visible = false; PlaybackTabDurationLabelValue.Visible = false; }
                else { PlaybackTabDurationLabelInfo.Visible = true; PlaybackTabDurationLabelValue.Visible = true; ReplaceElementDualText(PlaybackTabDurationLabelValue, item2.DurationS); }

                if (!path.StartsWith("Radio|") && item2.Lyrics != null && item2.Lyrics.Trim().Length > 0)
                { PlaybackTabLyricsButton.Visible = true; PlaybackTabLyricsButton.Tag = item2; }
                else { PlaybackTabLyricsButton.Visible = false; }

                if (!path.StartsWith("Radio|"))
                {
                    loadingRating = true;
                    PlaybackTabRatting.Visible = true;
                    PlaybackTabRatting.Rate = item2.Rating;
                    PlaybackTabRatting.Tag = path;
                    loadingRating = false;
                }
                else { PlaybackTabRatting.Visible = false; }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }
    }
}
