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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player2 player;
        string MediaLastOpen = "";
        double lastEnd = 0;
        int PlayListIndex = -1;
        int PlayRepeatStatus = 0;
        System.Timers.Timer ButtonPlayTimer = null;
        bool PreventUpdateSlider = false;
        bool lastPlayStatus = false;

        string AppLang = System.Globalization.CultureInfo.CurrentCulture.Name;
        public static string AppName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;

        int WindowWidthMode = 150;

        public ObservableCollection<PlayListViewItem> PlayList { get; set; }
        public List<string[]> PlayList2 { get; set; }
        public ObservableCollection<PlayListViewItem> PlayListDisplayed { get; set; }
        public PlayListViewItem PlayItem { get; set; }
        public static RoutedCommand GlobalCommand = new RoutedCommand();

        Duration duration;

        /// <summary>
        /// Used for storing object dependant of the Operating system
        /// </summary>
        private Dictionary<string, object> ListReferences = new Dictionary<string, object>();


        public MainWindow()
        {
            duration = new Duration(TimeSpan.FromMilliseconds(100));

            SettingsInit();

            this.DataContext = this;

            PlayList = new ObservableCollection<PlayListViewItem>();
            PlayList2 = new List<string[]>();
            PlayItem = new PlayListViewItem();
            player = new Player2();


            InitializeComponent();
            SettingsSetUp();

            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(BaseDir + "styles.xaml", UriKind.Absolute) });
            //Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(BaseDir + "Traductions" + System.IO.Path.DirectorySeparatorChar + "fr.xaml", UriKind.Absolute) });
            Traduction();
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
            PlayListViewInit();

            TimerUpdateButtonsSetUp();

            this.SizeChanged += Win1_SizeChanged;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KeyboardInterceptorSetUp();

            //Settings.LibFolder = "D:\\Music\\";
            MediatequeInvokeScan();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            KeyboardInterceptorDestroy();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Settings.SaveSettings();
        }

        private void Win1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (win1.ActualWidth > 800 && win1.ActualHeight > 460)
            {
                if (WindowWidthMode != 250)
                {
                    WindowWidthMode = 250;
                    Grid1.ColumnDefinitions[0].Width =
                        Grid2.RowDefinitions[0].Height = new GridLength((double)250);
                    FileCover.Width = FileCover.Height = 250;
                }
            }
            else
            {
                if (WindowWidthMode != 150)
                {
                    WindowWidthMode = 150;
                    Grid1.ColumnDefinitions[0].Width =
                        Grid2.RowDefinitions[0].Height = new GridLength((double)150);
                    FileCover.Width = FileCover.Height = 150;
                }
            }
            Grid1.ColumnDefinitions[1].Width = new GridLength(TabControler.ActualWidth - WindowWidthMode - 5);

            foreach (TabItem tab in TabControler.Items)
            {
                tab.Width = ((TabControler.ActualWidth - 4) / 4);
            }
            ((TabItem)(TabControler.Items[TabControler.Items.Count - 1])).Width -= 1;
        }

        private void fileOpen(string FilePath, bool doPlay = true)
        {
            PlayListViewItem it = player.MediaInfo(FilePath, false);
            if (it == null) { return; }
            lastEnd = UnixTimestamp();
            player.Open(FilePath, doPlay);

            PlayItemNameValue.ToolTip = PlayItemNameValue.Text = (it.Name!=null)?it.Name:"";
            PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = (it.Album != null) ? it.Album : "";
            PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = (it.Artist != null) ? it.Artist : "";
            PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = it.DurationS;

            FileCover.Source = null;
            FileCover.Source = player.MediaPicture(FilePath);
            if (FileCover.Source == null) { FileCover.Source = Bimage("CoverImg"); }
        }

        public delegate void updatePlaylistCb(int index, bool DoPlay = false);
        private void updatePlaylist(int NewPosition, bool DoPlay = false)
        {
            if (PlayList2.Count == 0) { return; }
            Debug.WriteLine("NewPosition = " + NewPosition);
            if (NewPosition >= 0 && NewPosition != PlayListIndex)
            {
                //StopPlaylist();
                player.StopAll();
                if (NewPosition >= PlayList2.Count) { NewPosition = 0; }
                PlayListIndex = NewPosition;
                fileOpen(PlayList2[NewPosition][(PlayList2[NewPosition][1] == null) ? 0 : 1], DoPlay);
            }
        }

        private void Items_CurrentChanged(object sender, EventArgs e) { Items_CurrentChanged(); }
        private void Items_CurrentChanged()
        {
            Debug.WriteLine("Items_CurrentChanged");
            updatePlaylist(PlayListIndex + PlayListView.SelectedIndex, true);
        }
    }
}
