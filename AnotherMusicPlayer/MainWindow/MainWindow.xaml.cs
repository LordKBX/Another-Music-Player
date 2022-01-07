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

using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public static string AppName = System.Windows.Application.Current.MainWindow.GetType().Assembly.GetName().Name;
        /// <summary> Store PlayList Pannel left collumn width(150 or 250) </summary>
        double WindowWidthMode = 150;

        /// <summary> Store PlayList by string[]{ File, ConvertedFile(null if no conversion) }  </summary>
        public List<string[]> PlayList { get; set; }
        /// <summary> Used for storing object dependant of the Operating system </summary>
        private Dictionary<string, object> ListReferences = new Dictionary<string, object>();
        /// <summary>Database object </summary>
        public Database bdd = null;

        /// <summary> Constructor </summary>
        public MainWindow(Database obdd)
        {
            bdd = obdd;
            // Set DataContext
            this.DataContext = this;  

            PlayList = new List<string[]>();//Initialize PlayList
            player = new Player(this);//Create Player object

            Settings.LoadSettings();

            Resources.Clear();
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

            //Settings.LibFolder = "D:\\Music\\";
            LibrarySetupFilters();
            LibraryInvokeScan(false, true);
            LibraryLoadOldPlaylist();
        }

        /// <summary> Callback Main window closing / exit </summary>
        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
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
            else {
                Grid1.ColumnDefinitions[0].Width = Grid2.RowDefinitions[0].Height = new System.Windows.GridLength((double)150);
                WindowWidthMode = FileCover.Width = FileCover.Height = 150;
            }
            Grid1.ColumnDefinitions[1].Width = new System.Windows.GridLength(TabControler.ActualWidth - WindowWidthMode - 5);

            foreach (TabItem tab in TabControler.Items)
            {
                tab.Width = ((TabControler.ActualWidth - 4) / 4);
            }
            ((TabItem)(TabControler.Items[TabControler.Items.Count - 1])).Width -= 1;

            if (LibraryBuildNavigationContentBlockssPanel != null) { LibraryBuildNavigationContentBlockssPanel.Width = TabControler.ActualWidth - 22; }

            Settings.LastWindowWidth = win1.ActualWidth;
            Settings.LastWindowHeight = win1.ActualHeight;
            Settings.SaveSettings();
        }

        /// <summary> Load list of file in the playlist and play first element in list if music currently not played </summary>
        private bool Open(string[] files, bool DoPlay = true, int Load = -1)
        {
            Debug.WriteLine("--> Open <--");
            bool doConv = false;
            int CountStart = PlayList.Count;

            if (files.Length > 0)
            {
                //Debug.WriteLine("--> Open : P1 <--");
                if (!player.IsPlaying() && MediaTestFileExtention(files[0]) == false) { doConv = true; }

                string NewFile;
                for (int i = 0; i < files.Length; i++)
                {
                    NewFile = null;
                    if (MediaTestFileExtention(files[i]) == false)
                    {
                        if (Settings.ConversionMode == 1) { NewFile = System.IO.Path.GetTempPath() + System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(files[i]), ".mp3"); }
                        else { NewFile = System.IO.Path.ChangeExtension(files[i], ".mp3"); }

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
            //Debug.WriteLine("--> Open : P2 <--");

            if (PlayListIndex < 0) { PlayListIndex = 0; Load = 0; }
            if (DoPlay == true) {
                if (player.IsPlaying()) { player.StopAll(); }
                FileOpen(PlayList[CountStart][(PlayList[CountStart][1] == null) ? 0 : 1]);
                PlayListIndex = CountStart;
            }
            //Debug.WriteLine("--> Open : P3 <--");
            if (Load >=0) {
                try {
                    if (PlayList.Count >= Load) {
                        FileOpen(PlayList[Load][(PlayList[Load][1] == null) ? 0 : 1], false);
                        //Debug.WriteLine("--> Open : P3-2 <--");
                        PlayListIndex = Load;
                        Timer_PlayListIndex = -1;
                    }
                } catch {
                    Debug.WriteLine("--> Open : P3-3 <--");
                }
            }
            if (!player.IsPlaying()) { player.Play(); }

            Timer_PlayListIndex = -1;

            //Debug.WriteLine("--> Open : P4 <--");
            UpdateRecordedQueue();
            //Debug.WriteLine("--> Open : P5 <--");

            return doConv;
        }

        /// <summary> Load music file and play if doPlay = true </summary>
        private void FileOpen(string FilePath, bool doPlay = true)
        {
            Debug.WriteLine("--> FileOpen >> " + FilePath);
            if (player.TestFile(FilePath)){
                //Debug.WriteLine("--> FileOpen : P2 <--");
                PlaybackStopLastTime = UnixTimestamp();
                //Debug.WriteLine("--> FileOpen : P3 <--");
                player.Open(FilePath, doPlay);
            }
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
                //Debug.WriteLine("LastPlaylistIndex saved: "+ Settings.LastPlaylistIndex);
                FileOpen(PlayList[NewPosition][(PlayList[NewPosition][1] == null) ? 0 : 1], DoPlay);
            }
        }

        private PlayListViewItem GetMediaInfo(string path, ObservableCollection<PlayListViewItem> previous_items = null) {
            PlayListViewItem item = null;
            try {
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

        private PlayListViewItemShort GetMediaInfoShort(string path, ObservableCollection<PlayListViewItemShort> previous_items = null) {
            PlayListViewItemShort item = null;
            try {
                Dictionary<string, object> rep = bdd.DatabaseFileInfo(path);
                if (rep != null)
                {
                    item = new PlayListViewItemShort();
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

        public static PlayListViewItem DatabaseItemToPlayListViewItem(Dictionary<string, object> rep)
        {
            PlayListViewItem item = null;
            item = new PlayListViewItem();
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

            return item;
        }

        public static Dictionary<string, object> PlayListViewItemToDatabaseItem(PlayListViewItem rep)
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

        private async void UpdateLeftPannelMediaInfo(string path = null)
        {
            //Debug.WriteLine("--> UpdateLeftPannelMediaInfo <--");
            //Debug.WriteLine("--> path = '"+path+"' <--");
            PlayListViewItem item = new PlayListViewItem();
            try
            {
                if (path == null) {
                    if(PlayList.Count > 0) { path = PlayList[PlayListIndex][0]; }
                    Debug.WriteLine("--> path = '" + path + "' <--");
                }
                if (path != null) {
                    Dictionary<string, object>  ret = bdd.DatabaseFileInfo(path);
                    if (ret != null) {
                        item = null;
                        item = DatabaseItemToPlayListViewItem(ret);
                    }
                }

                LeftPannelMediaInfo.Inlines.Clear();
                LeftPannelMediaInfo.LineStackingStrategy = System.Windows.LineStackingStrategy.BlockLineHeight;
                System.Windows.FontWeight fw = System.Windows.FontWeight.FromOpenTypeWeight(800);
                System.Windows.Thickness tc1 = new System.Windows.Thickness(3,3,0,0);
                System.Windows.Thickness tc2 = new System.Windows.Thickness(10,0,0,0);
                System.Windows.Media.SolidColorBrush cl2 = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(149, 149, 149));

                System.Windows.Controls.TextBlock t1 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Title2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t1);
                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                AccessText a1 = new AccessText() { Text = item.Name, Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                LeftPannelMediaInfo.Inlines.Add(a1);

                if (item.Album != null && item.Album.Trim() != "")
                {
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    System.Windows.Controls.TextBlock t2 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Album2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t2);
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    AccessText a2 = new AccessText() { Text = item.Album, Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                    LeftPannelMediaInfo.Inlines.Add(a2);
                }

                string Artists = "";
                if (item.Performers != null && item.Performers.Trim() != "") {
                    Artists += item.Performers;
                }
                if (item.Composers != null && item.Composers.Trim() != "") {
                    if (Artists != null && Artists != "") { Artists += ", "; }
                    Artists += item.Composers;
                }

                if (Artists != "")
                {
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    System.Windows.Controls.TextBlock t3 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Artist2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t3);
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    AccessText a3 = new AccessText() { Text = Artists, Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                    LeftPannelMediaInfo.Inlines.Add(a3);
                }

                if (item.Genres != null && item.Genres.Trim() != "")
                {
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    System.Windows.Controls.TextBlock t4 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Genres2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t4);
                    LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                    AccessText a4 = new AccessText() { Text = item.Genres, Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                    LeftPannelMediaInfo.Inlines.Add(a4);
                }

                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                System.Windows.Controls.TextBlock t5 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Duration2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t5);
                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                AccessText a5 = new AccessText() { Text = item.DurationS, Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                LeftPannelMediaInfo.Inlines.Add(a5);

                FileCover.Source = null;
                FileCover.ToolTip = null;
                System.Windows.Media.Imaging.BitmapImage bi = FilesTags.MediaPicture(item.Path, bdd, true, 50, 50);
                FileCover.Source = (bi ?? Bimage("CoverImg"));


                //if (bi != null && Settings.MemoryUsage == 1)
                //{
                //    System.Windows.Controls.WrapPanel p = new System.Windows.Controls.WrapPanel() { };
                //    System.Windows.Controls.Image im = new System.Windows.Controls.Image() { MaxHeight = 400, MaxWidth = 400, Style = (System.Windows.Style)Resources.MergedDictionaries[0]["HQImg"] };
                //    im.Source = FilesTags.MediaPicture(item.Path, bdd, true, 400, 400);
                //    p.Children.Add(im);
                //    FileCover.ToolTip = p;
                //}
            }
            catch { }
        }

        private void ClearLeftPannelMediaInfo()
        {
            try
            {
                LeftPannelMediaInfo.Inlines.Clear();
                LeftPannelMediaInfo.LineStackingStrategy = System.Windows.LineStackingStrategy.BlockLineHeight;
                System.Windows.FontWeight fw = System.Windows.FontWeight.FromOpenTypeWeight(800);
                System.Windows.Thickness tc1 = new System.Windows.Thickness(3,3,0,0);
                System.Windows.Thickness tc2 = new System.Windows.Thickness(10,0,0,0);
                System.Windows.Media.SolidColorBrush cl2 = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(149, 149, 149));

                System.Windows.Controls.TextBlock t1 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Title2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t1);
                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                AccessText a1 = new AccessText() { Text = "", Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                LeftPannelMediaInfo.Inlines.Add(a1);

                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                System.Windows.Controls.TextBlock t3 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Artist2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t3);
                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                AccessText a3 = new AccessText() { Text = "", Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                LeftPannelMediaInfo.Inlines.Add(a3);

                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                System.Windows.Controls.TextBlock t5 = new System.Windows.Controls.TextBlock() { Text = GetTranslation("Duration2"), Margin = tc1, FontWeight = fw }; LeftPannelMediaInfo.Inlines.Add(t5);
                LeftPannelMediaInfo.Inlines.Add(new LineBreak());
                AccessText a5 = new AccessText() { Text = "00:00:00", Margin = tc2, TextWrapping = System.Windows.TextWrapping.WrapWithOverflow, Foreground = cl2 };
                LeftPannelMediaInfo.Inlines.Add(a5);

                FileCover.Source = null;
                FileCover.ToolTip = null;
                FileCover.Source = Bimage("CoverImg");
            }
            catch { }
        }


    }
}
