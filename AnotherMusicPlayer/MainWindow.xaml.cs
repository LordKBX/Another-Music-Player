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
            player = new Player();//Create Player object

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
            PlayListView_Init();

            TimerInterfaceSetUp();

            win1.Width = Settings.LastWindowWidth;
            win1.Height = Settings.LastWindowHeight;
            win1.Left = Settings.LastWindowLeft;
            win1.Top = Settings.LastWindowTop;

            this.SizeChanged += Win1_SizeChanged;
            this.LocationChanged += MainWindow_LocationChanged;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;

            MediatequeBddInit();
            Dictionary<string, Dictionary<string, object>> LastPlaylist = MediatequeBddQuery("SELECT MIndex,Path1,Path2 FROM queue ORDER BY MIndex ASC", "Index");
            if (LastPlaylist != null)
            {
                if (LastPlaylist.Count > 0)
                {
                    Debug.WriteLine("Old PlayList detected");
                    List<string> gl = new List<string>();
                    int fails = 0;
                    foreach (KeyValuePair<string, Dictionary<string, object>> fi in LastPlaylist)
                    {
                        string path1 = (string)fi.Value["Path1"];
                        string path2 = (fi.Value["Path2"] == null)?null:(string)fi.Value["Path2"];
                        if (path2 != null)
                        {
                            if (System.IO.File.Exists(path2)) { gl.Add(path2); }
                            else
                            {
                                if (System.IO.File.Exists(path1)) { gl.Add(path1); } else { fails += 1; }
                            }
                        }
                        else if (System.IO.File.Exists(path1)) { gl.Add(path1); }
                        else { fails += 1; }
                    }

                    if (fails > 0) { PlayListIndex = 0; }
                    else { PlayListIndex = Settings.LastPlaylistIndex; }
                    Open(gl.ToArray(), true);
                    player.Stop();
                }
            }
        }

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
        }

        /// <summary> Callback Main window closing / exit </summary>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
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
        private bool Open(string[] files, bool DoPlay = true)
        {
            bool doConv = false;
            int startIndex = PlayListIndex;

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

                    if (!PlayList.Contains(tmp)) { PlayList.Add(tmp); }
                }
            }
            Timer_PlayListIndex = -1;

            if (PlayListIndex < 0) { PlayListIndex = 0; }
            if (!player.IsPlaying() && DoPlay == true) { FileOpen(PlayList[PlayListIndex][(PlayList[PlayListIndex][1] == null) ? 0 : 1]); }
            
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

            PlayItemNameValue.ToolTip = PlayItemNameValue.Text = (it.Name!=null)?it.Name:"";
            PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = (it.Album != null) ? it.Album : "";
            PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = (it.Artist != null) ? it.Artist : "";
            PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = it.DurationS;

            FileCover.Source = null;
            FileCover.Source = player.MediaPicture(FilePath);
            if (FileCover.Source == null) { FileCover.Source = Bimage("CoverImg"); }
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
                Debug.WriteLine("LastPlaylistIndex saved");
                FileOpen(PlayList[NewPosition][(PlayList[NewPosition][1] == null) ? 0 : 1], DoPlay);
            }
        }
    }
}
