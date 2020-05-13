using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;

using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Documents;

namespace AnotherMusicPlayer
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        /// <summary> Object music player </summary>
        Player player;
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
        public static string AppName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
        /// <summary> Store PlayList Pannel left collumn width(150 or 250) </summary>
        double WindowWidthMode = 150;

        /// <summary> Store PlayList by string[]{ File, ConvertedFile(null if no conversion) }  </summary>
        public List<string[]> PlayList { get; set; }
        /// <summary> Used for storing object dependant of the Operating system </summary>
        private Dictionary<string, object> ListReferences = new Dictionary<string, object>();

        /// <summary> Constructor </summary>
        public MainWindow()
        {
            SettingsInit();//Initialize and load settings
            // Set DataContext
            this.DataContext = this;

            PlayList = new List<string[]>();//Initialize PlayList
            player = new Player(this);//Create Player object

            InitializeComponent();//Load and build interface from XAML file "MainWindow.xaml"
            SettingsSetUp();//Initialize interface elements with stored parametters in settings

            Resources.MergedDictionaries.Clear();//Ensure a clean MergedDictionaries
            Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(BaseDir + "styles.xaml", UriKind.Absolute) });//Load settings file
            //Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(BaseDir + "Traductions" + SeparatorChar + "fr.xaml", UriKind.Absolute) });
            UpdateTraduction();
            PreviewSetUp();

            FileCover.Source = Bimage("CoverImg");
            BtnOpen.Background = new ImageBrush(Bimage("OpenButtonImg"));
            BtnPrevious.Background = new ImageBrush(Bimage("PreviousButtonImg"));
            BtnPlayPause.Background = new ImageBrush(Bimage("PlayButtonImg_Play"));
            BtnNext.Background = new ImageBrush(Bimage("NextButtonImg"));
            BtnClearList.Background = new ImageBrush(Bimage("ClearListButtonImg"));
            BtnShuffle.Background = new ImageBrush(Bimage("ShuffleButtonImg"));
            BtnRepeat.Background = new ImageBrush(Bimage("RepeatButtonImg_None"));

            EventsPlaybackInit();
            PlayListView_Init(); EqualizerInitEvents();
            DatabaseInit();

            TimerInterfaceSetUp();

            win1.Width = (Settings.LastWindowWidth > 500) ? Settings.LastWindowWidth : 500;
            win1.Height = (Settings.LastWindowHeight > 350) ? Settings.LastWindowHeight : 350;

            win1.Left = (Settings.LastWindowLeft < 0) ? 100 : Settings.LastWindowLeft;
            win1.Top = (Settings.LastWindowTop < 0) ? 100 : Settings.LastWindowTop;

            this.SizeChanged += Win1_SizeChanged;
            this.LocationChanged += MainWindow_LocationChanged;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
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
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KeyboardInterceptorSetUp();

            //Settings.LibFolder = "D:\\Music\\";
            MediatequeInvokeScan();
            MediatequeLoadOldPlaylist();
        }

        /// <summary> Callback Main window closing / exit </summary>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (IsInTransaction()) { DatabaseTansactionEnd(); }
            KeyboardInterceptorDestroy();
            Settings.SaveSettings();
        }

        /// <summary> Callback Main window size change </summary>
        private void Win1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (win1.ActualWidth > 800 && win1.ActualHeight > 460)
            {
                if (WindowWidthMode != 250)
                {
                    Grid1.ColumnDefinitions[0].Width = Grid2.RowDefinitions[0].Height = new GridLength((double)250);
                    WindowWidthMode = FileCover.Width = FileCover.Height = 250;
                }
            }
            else
            {
                if (WindowWidthMode != 150)
                {
                    Grid1.ColumnDefinitions[0].Width = Grid2.RowDefinitions[0].Height = new GridLength((double)150);
                    WindowWidthMode = FileCover.Width = FileCover.Height = 150;
                }
            }
            Grid1.ColumnDefinitions[1].Width = new GridLength(TabControler.ActualWidth - WindowWidthMode - 5);

            foreach (TabItem tab in TabControler.Items)
            {
                tab.Width = ((TabControler.ActualWidth - 4) / 4);
            }
            ((TabItem)(TabControler.Items[TabControler.Items.Count - 1])).Width -= 1;
            Settings.LastWindowWidth = win1.ActualWidth;
            Settings.LastWindowHeight = win1.ActualHeight;
            Settings.SaveSettings();
        }

        /// <summary> Load list of file in the playlist and play first element in list if music currently not played </summary>
        private bool Open(string[] files, bool DoPlay = true, int Load = -1)
        {
            bool doConv = false;
            int CountStart = PlayList.Count;

            if (files.Length > 0)
            {
                if (!player.IsPlaying() && MediaTestFileExtention(files[0]) == false) { doConv = true; }

                string NewFile;
                for (int i = 0; i < files.Length; i++)
                {
                    NewFile = null;
                    if (MediaTestFileExtention(files[i]) == false)
                    {
                        if (Settings.ConversionMode == 1) { NewFile = Path.GetTempPath() + Path.ChangeExtension(Path.GetFileName(files[i]), ".mp3"); }
                        else { NewFile = Path.ChangeExtension(files[i], ".mp3"); }

                        if (i == 0)
                        {
                            doConv = true;
                            player.Conv(files[i], NewFile, (Settings.ConversionMode == 1) ? false : true);
                        }
                        else { player.Conv(files[i], NewFile, (Settings.ConversionMode == 1) ? false : true); }
                        //LoadFileAsync(files[i]);
                    }
                    string[] tmp = new string[] { files[i], NewFile };

                    if (!PlayList.Contains(tmp)) {
                        PlayList.Add(tmp);
                    }
                }
            }

            if (PlayListIndex < 0) { PlayListIndex = 0; }
            if (DoPlay == true) {
                if (player.IsPlaying()) { player.StopAll(); }
                FileOpen(PlayList[CountStart][(PlayList[CountStart][1] == null) ? 0 : 1]);
                PlayListIndex = CountStart;
            }
            if (Load >=0) {
                try { 
                    FileOpen(PlayList[Load][(PlayList[Load][1] == null) ? 0 : 1], false);
                    PlayListIndex = Load;
                    Timer_PlayListIndex = -1;
                } catch { }
            }
            if (!player.IsPlaying()) { player.Play(); }

            Timer_PlayListIndex = -1;

            UpdateRecordedQueue();

            return doConv;
        }

        /// <summary> Load music file and play if doPlay = true </summary>
        private void FileOpen(string FilePath, bool doPlay = true)
        {
            PlayListViewItem it = player.MediaInfo(FilePath, false);
            if (it == null) { return; }
            PlaybackStopLastTime = UnixTimestamp();
            player.Open(FilePath, doPlay);

            //PlayItemNameValue.ToolTip = PlayItemNameValue.Text = (it.Name!=null)?it.Name:"";
            //PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = (it.Album != null) ? it.Album : "";
            //PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = (it.Artist != null) ? it.Artist : "";
            //PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = it.DurationS;

            //FileCover.Source = null;
            //FileCover.Source = player.MediaPicture(FilePath);
            //if (FileCover.Source == null) { FileCover.Source = Bimage("CoverImg"); }
        }

        /// <summary> Change Playlist index and load music file if the new position is accepted </summary>
        private void UpdatePlaylist(int NewPosition, bool DoPlay = false)
        {
            if (PlayList.Count == 0) { return; }
            if (NewPosition < 0) { NewPosition = 0; }
            if (NewPosition >= PlayList.Count) { NewPosition = 0; }
            //Debug.WriteLine("NewPosition = " + NewPosition);
            if (NewPosition >= 0 && NewPosition != PlayListIndex)
            {
                //StopPlaylist();
                player.StopAll();
                PlayListIndex = NewPosition;
                Settings.LastPlaylistIndex = PlayListIndex;
                Settings.SaveSettings();
                Debug.WriteLine("LastPlaylistIndex saved");
                FileOpen(PlayList[NewPosition][(PlayList[NewPosition][1] == null) ? 0 : 1], DoPlay);
            }
        }

        private PlayListViewItem GetMediaInfo(string path, ObservableCollection<PlayListViewItem> previous_items = null) {
            PlayListViewItem item = null;
            try {
                if (previous_items == null)
                {
                    if (PlayListView.ItemsSource != null) { previous_items = (ObservableCollection<PlayListViewItem>)PlayListView.ItemsSource; }
                    else { previous_items = new ObservableCollection<PlayListViewItem>(); }
                }

                foreach (PlayListViewItem itm in previous_items) { if (itm.Path == path) { item = itm; break; } }
                if (item == null)
                {
                    Dictionary<string, object> rep = DatabaseFileInfo(path);
                    if (rep != null)
                    {
                        item = new PlayListViewItem();
                        item.Path = path;
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
                    }
                }
                if (item == null) { item = player.MediaInfo(path, false); }
            }
            catch { }
            
            return item;
        }

        private void UpdateLeftPannelMediaInfo(PlayListViewItem item = null)
        {
            try
            {
                if (item == null) { item = GetMediaInfo(PlayList[PlayListIndex][0]); }
                if (item == null) { return; }

                LeftPannelMediaInfo.Inlines.Clear();
                LeftPannelMediaInfo.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
                System.Windows.FontWeight fw = System.Windows.FontWeight.FromOpenTypeWeight(800);
                System.Windows.Thickness tc1 = new Thickness(3,3,0,0);
                System.Windows.Thickness tc2 = new Thickness(10,0,0,0);
                SolidColorBrush cl2 = new SolidColorBrush(Color.FromRgb(149, 149, 149));

                TextBlock t1 = new TextBlock() { Text = GetTaduction("Title2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t1);
                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                AccessText a1 = new AccessText() { Text = item.Name, Margin = tc2, TextWrapping = TextWrapping.WrapWithOverflow, Foreground = cl2 };
                LeftPannelMediaInfo.Inlines.Add(a1);

                if (item.Album != null && item.Album.Trim() != "")
                {
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    TextBlock t2 = new TextBlock() { Text = GetTaduction("Album2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t2);
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    AccessText a2 = new AccessText() { Text = item.Album, Margin = tc2, TextWrapping = TextWrapping.WrapWithOverflow, Foreground = cl2 };
                    LeftPannelMediaInfo.Inlines.Add(a2);
                }

                string Artists = "";
                if (item.Performers != null && item.Performers.Trim() != "") {
                    Artists += item.Performers;
                }
                if (item.Performers != null && item.Performers.Trim() != "") {
                    if (Artists != null && Artists != "") { Artists += ", "; }
                    Artists += item.Composers;
                }

                if (Artists != "")
                {
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    TextBlock t3 = new TextBlock() { Text = GetTaduction("Artist2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t3);
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    AccessText a3 = new AccessText() { Text = item.Performers, Margin = tc2, TextWrapping = TextWrapping.WrapWithOverflow, Foreground = cl2 };
                    LeftPannelMediaInfo.Inlines.Add(a3);
                }

                if (item.Genres != null && item.Genres.Trim() != "")
                {
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    TextBlock t4 = new TextBlock() { Text = GetTaduction("Genres2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t4);
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    AccessText a4 = new AccessText() { Text = item.Genres, Margin = tc2, TextWrapping = TextWrapping.WrapWithOverflow, Foreground = cl2 };
                    LeftPannelMediaInfo.Inlines.Add(a4);
                }

                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                TextBlock t5 = new TextBlock() { Text = GetTaduction("Duration2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t5);
                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                AccessText a5 = new AccessText() { Text = item.DurationS, Margin = tc2, TextWrapping = TextWrapping.WrapWithOverflow, Foreground = cl2 };
                LeftPannelMediaInfo.Inlines.Add(a5);

                FileCover.Source = null;
                FileCover.Source = player.MediaPicture(item.Path);
                if (FileCover.Source == null) { FileCover.Source = Bimage("CoverImg"); }
            }
            catch { }
        }
    }
}
