using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Text.RegularExpressions;

using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : System.Windows.Window
    {
        /// <summary> Object music player </summary>
        public Player player;
        /// <summary> Store the last time(unix) playback was stoped, used for preventing media skip since default playback status is stoped before starting </summary>
        double PlaybackStopLastTime = 0;
        /// <summary> Store the playlist current play index </summary>
        int PlayListIndex = -1;
        /// <summary> Store repeat mode status: x <= 0(none), 1(repeat one), x >= 2(repeat playlist)</summary>
        int PlayRepeatStatus = 0;
        /// <summary> Store if we do not authorise update for PlayBack Position bar </summary>
        bool PreventUpdateSlider = false;

        /// <summary> Store system language reference code </summary>
        string AppLang = System.Globalization.CultureInfo.CurrentCulture.Name;
        /// <summary> Store the application assembly name </summary>
        public string AppName = "";
        /// <summary> Store PlayList Pannel left collumn width(150 or 250) </summary>
        double WindowWidthMode = 150;

        /// <summary> Store PlayList by string[]{ File, ConvertedFile(null if no conversion) }  </summary>
        public List<string[]> PlayList { get; set; }
        /// <summary> Used for storing object dependant of the Operating system </summary>
        private Dictionary<string, object> ListReferences = new Dictionary<string, object>();
        /// <summary>Database object </summary>
        public Database bdd = null;
        /// <summary>Library object </summary>
        public Library library = null;
        /// <summary>PlayLists object </summary>
        public PlayLists playLists = null;

        private double lastTopBarClick = 0;
        private double oldWidth = 0;
        private double oldHeight = 0;
        private double oldTop = 0;
        private double oldLeft = 0;

        private bool isDebug = false;
        [Conditional("DEBUG")]
        private void IsDebugCheck() { isDebug = true; }

        /// <summary> Constructor </summary>
        public MainWindow(Database obdd)
        {
            AppName = System.Windows.Application.Current.MainWindow.GetType().Assembly.GetName().Name;
            IsDebugCheck();
            bdd = obdd;
            // Set DataContext
            this.DataContext = this;

            PlayList = new List<string[]>();//Initialize PlayList
            player = new Player(this);//Create Player object

            Settings.LoadSettings();
            InitializeComponent();//Load and build interface from XAML file "MainWindow.xaml"
            HideDebug();//in release mode hide debug elements
            SettingsInit();//Initialize and load settings panel
            dialog1Image.Source = new BitmapImage(new Uri(BaseDirImg + "loadingx50.png"));
            BtnScanMetadataImage.Source = new BitmapImage(new Uri(BaseDirImg + "loadingx50.png"));
            setLoadingState(true, "Loading", true);
            setMetadataScanningState(false);
            //SettingsSetUp();//Initialize interface elements with stored parametters in settings
            //TabControl t = new TabControl();
            //t.cli
            TabControler.SelectedIndex = 0;

            PlayListIndex = Settings.LastPlaylistIndex;

            Debug.WriteLine("LastPlaylistIndex: " + Settings.LastPlaylistIndex);

            //Resources.MergedDictionaries.Clear();//Ensure a clean MergedDictionaries
            StyleUpdate();
            TranslationUpdate();
            PreviewSetUp();

            FileCover.Source = Bimage("CoverImg");

            EventsPlaybackInit();
            PlayListView_Init();

            TimerInterfaceSetUp();

            this.SizeChanged += Win1_SizeChanged;
            this.LocationChanged += MainWindow_LocationChanged;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;

            //WorkingAreaSize workingAreaSize = GetWorkingAreaSize();
            //MaxWidth = workingAreaSize.Width;
            //MaxHeight = workingAreaSize.Height;

            BtnClose.Click += (object sender, RoutedEventArgs e) => { this.Close(); };
            BtnMaximize.Click += (object sender, RoutedEventArgs e) =>
            {
                WorkingAreaSize workingAreaSize = GetWorkingAreaSize();
                if (Width == workingAreaSize.Width && Height == workingAreaSize.Height)
                {
                    Top = oldTop;
                    Left = oldLeft;
                    Width = oldWidth; Height = oldHeight;
                    BtnMaximize.Tag = "Off";
                }
                else
                {
                    oldWidth = Width;
                    oldHeight = Height;
                    oldTop = Top;
                    oldLeft = Left;
                    BtnMaximize.Tag = "On";

                    WorkingAreaPosition workingAreaPosition = GetWorkingAreaPosition();
                    Top = workingAreaPosition.Y1;
                    Left = workingAreaPosition.X1;

                    Width = MaxWidth = workingAreaSize.Width;
                    Height = MaxHeight = workingAreaSize.Height;
                }
            };
            BtnMinimize.Click += (object sender, RoutedEventArgs e) => { WindowState = (WindowState == WindowState.Minimized) ? WindowState.Normal : WindowState.Minimized; };
            TopBar.MouseDown += TopBar_MouseDown;

            playLists = new PlayLists(this);
            PlayListView.ContextMenu = null;
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double newEnd = UnixTimestamp();
                if (newEnd - lastTopBarClick < 1.0) { WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized; }
                else { DragMove(); }
                lastTopBarClick = newEnd;
            }
        }

        /// <summary> Function unlocking window interface </summary>
        public void UnsetLockScreen() { Mouse.OverrideCursor = null; win1.IsEnabled = true; }

        /// <summary> Callback for event window position changed </summary>
        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            Settings.LastWindowLeft = win1.Left;
            Settings.LastWindowTop = win1.Top;
            Settings.SaveSettings();
        }

        /// <summary> Callback Main window loaded </summary>
        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            KeyboardLocalListenerInit();
            KeyboardGlobalListenerInit();

            ClearLeftPannelMediaInfo();
            LibraryLoadOldPlaylist();

            library = new Library(this, Settings.LibFolder);
            Dispatcher.InvokeAsync(new Action(() =>
            {
                library.DisplayPath(Settings.LibFolder);
            }));

            PlaylistsTreeAuto.IsExpanded = true;
        }

        public static TreeViewItem FindTviFromObjectRecursive(ItemsControl ic, object o)
        {
            //Search for the object model in first level children (recursively)
            if (ic == null) { return null; }
            TreeViewItem tvi = ic.ItemContainerGenerator.ContainerFromItem(o) as TreeViewItem;
            if (tvi != null) return tvi;
            //Loop through user object models
            foreach (object i in ic.Items)
            {
                //Get the TreeViewItem associated with the iterated object model
                TreeViewItem tvi2 = ic.ItemContainerGenerator.ContainerFromItem(i) as TreeViewItem;
                tvi = FindTviFromObjectRecursive(tvi2, o);
                if (tvi != null) return tvi;
            }
            return null;
        }

        /// <summary> Callback Main window closing / exit </summary>
        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Settings.LastPlaylistDuration = player.GetCurrentFilePosition();
            player.Dispose();
            await Settings.SaveSettings();
            bdd.Finalize();
            KeyboardGlobalListenerKill();
        }

        /// <summary> Callback Main window size change </summary>
        private void Win1_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (Settings.MemoryUsage == 1)
            {
                if (win1.ActualWidth > 800 && win1.ActualHeight > 460)
                {
                    if (WindowWidthMode != 250)
                    {
                        Grid1.ColumnDefinitions[0].Width = Grid2.RowDefinitions[0].Height = new System.Windows.GridLength((double)250);
                        WindowWidthMode = FileCover.Width = FileCover.Height = 250;
                    }
                }
                else
                {
                    if (WindowWidthMode != 150)
                    {
                        Grid1.ColumnDefinitions[0].Width = Grid2.RowDefinitions[0].Height = new System.Windows.GridLength((double)150);
                        WindowWidthMode = FileCover.Width = FileCover.Height = 150;
                    }
                }
            }
            else
            {
                Grid1.ColumnDefinitions[0].Width = Grid2.RowDefinitions[0].Height = new System.Windows.GridLength((double)150);
                WindowWidthMode = FileCover.Width = FileCover.Height = 150;
            }
            Grid1.ColumnDefinitions[1].Width = new System.Windows.GridLength(TabControler.ActualWidth - WindowWidthMode - 5);

            foreach (TabItem tab in TabControler.Items)
            {
                tab.Width = ((TabControler.ActualWidth - 4) / 4);
            }
            ((TabItem)(TabControler.Items[TabControler.Items.Count - 1])).Width -= 1;

            //if (LibraryBuildNavigationContentBlockssPanel != null) { LibraryBuildNavigationContentBlockssPanel.Width = TabControler.ActualWidth - 22; }

            Settings.LastWindowWidth = win1.ActualWidth;
            Settings.LastWindowHeight = win1.ActualHeight;
            Settings.SaveSettings();
        }

        /// <summary> Load list of file in the playlist and play first element in list if music currently not played </summary>
        private bool Open(string[] files, bool replace = false, bool random = false, int playIndex = 0, bool autoplay = false)
        {
            Debug.WriteLine("--> Open <--");
            if (replace == true) { player.PlaylistClear(); }
            return player.PlaylistEnqueue(files, random, playIndex, Settings.LastPlaylistDuration, autoplay);
        }

        /// <summary> Change Playlist index and load music file if the new position is accepted </summary>
        private void UpdatePlaylist(int NewPosition, bool DoPlay = false)
        {
            if (PlayList.Count == 0) { return; }
            if (NewPosition < 0) { NewPosition = 0; }
            if (NewPosition >= PlayList.Count) { NewPosition = 0; }
            if (NewPosition != PlayListIndex) { player.PlaylistReadIndex(NewPosition); }
        }

        private MediaItem GetMediaInfo(string path, ObservableCollection<MediaItem> previous_items = null)
        {
            MediaItem item = null;
            try
            {
                Dictionary<string, object> rep = bdd.DatabaseFileInfo(path);
                if (rep != null)
                {
                    item = new MediaItem();
                    item.Path = path;
                    item.Name = (string)rep["Name"];
                    item.Album = (string)rep["Album"];
                    item.AlbumArtists = (string)rep["AlbumArtists"];
                    item.Performers = (string)rep["Performers"];
                    item.Composers = (string)rep["Composers"];
                    item.Duration = Convert.ToInt64((string)rep["Duration"]);
                    item.DurationS = displayTime(Convert.ToInt64((string)rep["Duration"]));

                    item.Lyrics = (string)rep["Lyrics"];
                    item.Genres = (string)rep["Genres"];
                    item.Copyright = (string)rep["Copyright"];
                    item.Disc = Convert.ToUInt32(rep["Disc"]);
                    item.DiscCount = Convert.ToUInt32(rep["DiscCount"]);
                    item.Track = Convert.ToUInt32(rep["Track"]);
                    item.TrackCount = Convert.ToUInt32(rep["TrackCount"]);
                    item.Year = Convert.ToUInt32(rep["Year"]);
                }
                else { item = FilesTags.MediaInfo(path, false); }
            }
            catch { }

            return item;
        }

        private PlayListViewItem GetMediaInfoShort(string path, ObservableCollection<PlayListViewItem> previous_items = null)
        {
            PlayListViewItem item = null;
            try
            {
                Dictionary<string, object> rep = bdd.DatabaseFileInfo(path);
                if (rep != null)
                {
                    item = new PlayListViewItem();
                    item.Path = path;
                    item.Name = (string)rep["Name"];
                    item.Album = (string)rep["Album"];
                    item.AlbumArtists = (string)rep["AlbumArtists"];
                    item.Performers = (string)rep["Performers"];
                    item.Composers = (string)rep["Composers"];
                    item.Duration = Convert.ToInt64((string)rep["Duration"]);
                    item.DurationS = displayTime(Convert.ToInt64((string)rep["Duration"]));
                }
                else { item = FilesTags.MediaInfoShort(path, false); }
            }
            catch { }

            return item;
        }

        public static MediaItem DatabaseItemToMediaItem(Dictionary<string, object> rep)
        {
            MediaItem item = null;
            item = new MediaItem();
            item.Path = (string)rep["Path"];
            item.Name = (string)rep["Name"];
            item.Album = (string)rep["Album"];
            item.AlbumArtists = (string)rep["AlbumArtists"];
            item.Performers = (string)rep["Performers"];
            item.Composers = (string)rep["Composers"];
            item.Lyrics = (string)rep["Lyrics"];
            item.Duration = Convert.ToInt64((string)rep["Duration"]);
            item.DurationS = displayTime(Convert.ToInt64((string)rep["Duration"]));
            item.Genres = (string)rep["Genres"];
            item.Copyright = (string)rep["Copyright"];
            item.Disc = Convert.ToUInt32(rep["Disc"]);
            item.DiscCount = Convert.ToUInt32(rep["DiscCount"]);
            item.Track = Convert.ToUInt32(rep["Track"]);
            item.TrackCount = Convert.ToUInt32(rep["TrackCount"]);
            item.Year = Convert.ToUInt32(rep["Year"]);
            item.Rating = Convert.ToDouble((rep["Rating"] as string).Replace(".", ","));

            return item;
        }

        public static Dictionary<string, object> MediaItemToDatabaseItem(MediaItem rep)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Path", rep.Path);
            ret.Add("Name", rep.Name);
            ret.Add("Album", rep.Album);
            ret.Add("AlbumArtists", rep.AlbumArtists);
            ret.Add("Performers", rep.Performers);
            ret.Add("Composers", rep.Composers);
            ret.Add("Lyrics", rep.Lyrics);
            ret.Add("Duration", rep.Duration);
            ret.Add("DurationS", displayTime(Convert.ToInt64(rep.Duration)));
            ret.Add("Genres", rep.Genres);
            ret.Add("Copyright", rep.Copyright);
            ret.Add("Disc", rep.Disc);
            ret.Add("DiscCount", rep.DiscCount);
            ret.Add("Track", rep.Track);
            ret.Add("TrackCount", rep.TrackCount);
            ret.Add("Year", rep.Year);
            return ret;
        }

        private void UpdateLeftPannelMediaInfo(string path = null)
        {
            //Debug.WriteLine("--> UpdateLeftPannelMediaInfo <--");
            //Debug.WriteLine("--> path = '"+path+"' <--");
            MediaItem item = new MediaItem();
            try
            {
                if (path == null)
                {
                    if (PlayList.Count > 0) { path = PlayList[PlayListIndex][0]; }
                    Debug.WriteLine("--> path = '" + path + "' <--");
                }
                if (path != null)
                {
                    Dictionary<string, object> ret = bdd.DatabaseFileInfo(path);
                    if (ret != null)
                    {
                        item = null;
                        item = DatabaseItemToMediaItem(ret);
                    }
                }

                LeftPannelMediaInfoR1.Height = new GridLength(50);

                LeftPannelMediaInfo.Children.Clear();
                System.Windows.FontWeight fw = System.Windows.FontWeight.FromOpenTypeWeight(800);
                System.Windows.Thickness tc1 = new System.Windows.Thickness(3, 3, 0, 0);
                System.Windows.Thickness tc2 = new System.Windows.Thickness(10, 0, 0, 0);
                System.Windows.Media.SolidColorBrush cl1 = FindResource("ForegroundColor") as SolidColorBrush;
                System.Windows.Media.SolidColorBrush cl2 = FindResource("ForegroundAltColor") as SolidColorBrush;

                System.Windows.Controls.TextBlock t1 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Title2"), Margin = tc1, FontWeight = fw, Foreground = cl1 };
                LeftPannelMediaInfo.Children.Add(t1);
                AccessText a1 = new AccessText() { Text = item.Name, ToolTip = item.Name, Margin = tc2, TextTrimming = System.Windows.TextTrimming.CharacterEllipsis, Foreground = cl2 };
                LeftPannelMediaInfo.Children.Add(a1);

                int nblines = 2;

                if (item.Album != null && item.Album.Trim() != "")
                {
                    System.Windows.Controls.TextBlock t2 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Album2"), Margin = tc1, FontWeight = fw, Foreground = cl1 };
                    LeftPannelMediaInfo.Children.Add(t2);
                    AccessText a2 = new AccessText() { Text = item.Album, ToolTip = item.Album, Margin = tc2, TextTrimming = System.Windows.TextTrimming.CharacterEllipsis, Foreground = cl2 };
                    LeftPannelMediaInfo.Children.Add(a2);
                    nblines += 2;
                }

                string Artists = "";
                if (item.Performers != null && item.Performers.Trim() != "")
                {
                    Artists += item.Performers;
                }
                if (item.Composers != null && item.Composers.Trim() != "")
                {
                    if (Artists != null && Artists != "") { Artists += ", "; }
                    Artists += item.Composers;
                }

                if (Artists != "")
                {
                    System.Windows.Controls.TextBlock t3 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Artist2"), Margin = tc1, FontWeight = fw, Foreground = cl1 };
                    LeftPannelMediaInfo.Children.Add(t3);
                    AccessText a3 = new AccessText() { Text = Artists, ToolTip = Artists, Margin = tc2, TextTrimming = System.Windows.TextTrimming.CharacterEllipsis, Foreground = cl2 };
                    LeftPannelMediaInfo.Children.Add(a3);
                    nblines += 2;
                }

                if (item.Genres != null && item.Genres.Trim() != "")
                {
                    System.Windows.Controls.TextBlock t4 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Genres2"), Margin = tc1, FontWeight = fw, Foreground = cl1 };
                    LeftPannelMediaInfo.Children.Add(t4);
                    AccessText a4 = new AccessText() { Text = item.Genres, ToolTip = item.Genres, Margin = tc2, TextTrimming = System.Windows.TextTrimming.CharacterEllipsis, Foreground = cl2 };
                    LeftPannelMediaInfo.Children.Add(a4);
                    nblines += 2;
                }

                System.Windows.Controls.TextBlock t5 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Duration2"), Margin = tc1, FontWeight = fw, Foreground = cl1 };
                LeftPannelMediaInfo.Children.Add(t5);
                AccessText a5 = new AccessText() { Text = item.DurationS, ToolTip = item.DurationS, Margin = tc2, TextTrimming = System.Windows.TextTrimming.CharacterEllipsis, Foreground = cl2 };
                LeftPannelMediaInfo.Children.Add(a5);
                nblines += 2;

                if (item.Lyrics != null && item.Lyrics.Trim() != "")
                {
                    LeftPannelMediaInfoLyricsValue.Text = item.Lyrics;
                    LeftPannelMediaInfoLyricsLabel.Visibility = Visibility.Visible;
                    LeftPannelMediaInfoLyrics.Visibility = Visibility.Visible;
                }
                else
                {
                    LeftPannelMediaInfoLyricsLabel.Visibility = Visibility.Collapsed;
                    LeftPannelMediaInfoLyrics.Visibility = Visibility.Collapsed;
                }

                FileCover.Source = null;
                FileCover.ToolTip = null;
                System.Windows.Media.Imaging.BitmapImage bi = FilesTags.MediaPicture(item.Path, bdd, true, (Settings.MemoryUsage == 0) ? 150 : 250, (Settings.MemoryUsage == 0) ? 150 : 250);
                FileCover.Source = (bi ?? Bimage("CoverImg"));

                LeftPannelMediaInfoR1.Height = new GridLength(nblines * 18);
            }
            catch { }
        }

        private void ClearLeftPannelMediaInfo()
        {
            try
            {
                LeftPannelMediaInfo.Children.Clear();

                FileCover.Source = null;
                FileCover.ToolTip = null;
                FileCover.Source = Bimage("CoverImg");
                LeftPannelMediaInfoLyricsLabel.Visibility = Visibility.Collapsed;
                LeftPannelMediaInfoLyrics.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        public void RatingRateChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Rating rater = (Rating)sender;
            string filePath = (string)rater.Tag;
            Debug.WriteLine("Rate Changed !");
            Debug.WriteLine("filePath=" + filePath);
            Debug.WriteLine("Old value=" + e.OldValue);
            Debug.WriteLine("New value=" + e.NewValue);
        }

        private void PlayListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayListView.Items.Count == 0) { PlayListView.ContextMenu = null; return; }
            PlayListView.ContextMenu = PlayingQueueContextMenu.MakeContextMenu(this);
            if (PlayListView.SelectedItems.Count == 0) { ((PlayingQueueContextMenu)PlayListView.ContextMenu).RemoveTracks.Visibility = Visibility.Collapsed; }
            else { ((PlayingQueueContextMenu)PlayListView.ContextMenu).RemoveTracks.Visibility = Visibility.Visible; }
        }

        private void PlayListView_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            PlayListView_SelectionChanged(null, null);
        }
    }
}
