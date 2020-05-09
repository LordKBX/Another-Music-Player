using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        int WindowWidthMode = 150;

        public ObservableCollection<PlayListViewItem> PlayList { get; set; }
        public List<string[]> PlayList2 { get; set; }
        public ObservableCollection<PlayListViewItem> PlayListDisplayed { get; set; }
        public PlayListViewItem PlayItem { get; set; }
        public static RoutedCommand GlobalCommand = new RoutedCommand();

        /// <summary>
        /// Used for storing object dependant of the Operating system
        /// </summary>
        private Dictionary<string, object> ListReferences = new Dictionary<string, object>();


        public MainWindow()
        {
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
            MediatequeScan();
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

        //private void fileOpen(string FilePath, string OriginPath = null, bool doPlay = true)
        //{
        //    PlayListViewItem it = player.MediaInfo(FilePath, doPlay, OriginPath);
        //    lastEnd = UnixTimestamp();
        //    if (doPlay)
        //    {
        //        player.StopAll();
        //        int last = PlayListIndex;
        //        Dispatcher.BeginInvoke(new Action(() => {
        //            try
        //            {
        //                PlayList[last].Selected = PlayListSelectionChar;
        //                //PlayListView.Items.Refresh();
        //            }
        //            catch /*  (System.Exception er) {Debug.WriteLine(JsonConvert.SerializeObject(er));} */ { }
        //        }));
        //    }
        //    player.Open(FilePath, doPlay);
        //    MediaLastOpen = FilePath;
        //    PlayListIndex = UpdateListView(it);

        //    PlayItemNameValue.ToolTip = PlayItemNameValue.Text = it.Name;
        //    PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = it.Album;
        //    PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = it.Artist;
        //    PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = it.DurationS;

        //    FileCover.Source = null;
        //    FileCover.Source = player.MediaPicture(FilePath);
        //    if (FileCover.Source == null) { FileCover.Source = Bimage("CoverImg"); }
        //    //PlayListView.SelectedIndex = PlayListIndex;
        //}

        private void fileOpen(string FilePath, bool doPlay = true)
        {
            PlayListViewItem it = player.MediaInfo(FilePath, false);
            lastEnd = UnixTimestamp();
            player.Open(FilePath, doPlay);

            PlayItemNameValue.ToolTip = PlayItemNameValue.Text = it.Name;
            PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = it.Album;
            PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = it.Artist;
            PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = it.DurationS;

            FileCover.Source = null;
            FileCover.Source = player.MediaPicture(FilePath);
            if (FileCover.Source == null) { FileCover.Source = Bimage("CoverImg"); }
        }

        private int UpdateListView(PlayListViewItem item, bool replace = false)
        {
            if (item == null) { return -1; }
            int Max = PlayList.Count;
            for (int i = 0; i < Max; i++)
            {
                try
                {
                    bool doReplace = false;
                    if (item.OriginPath != null)
                    {
                        if (PlayList[i].OriginPath == item.OriginPath) { doReplace = true; Debug.WriteLine("Replace by OriginPath"); }
                    }
                    else { if (PlayList[i].Path == item.Path) { doReplace = true; Debug.WriteLine("Replace by Path"); } }

                    if (doReplace)
                    {
                        if (replace)
                        {
                            PlayList[i].Path = item.Path;
                            PlayList[i].Name = item.Name;
                            PlayList[i].Album = item.Album;
                            PlayList[i].Artist = item.Artist;
                            PlayList[i].Duration = item.Duration;
                            //PlayListView.Items.Refresh();
                            //Debug.WriteLine(JsonConvert.SerializeObject(PlayList[i]));
                        }
                        return i;
                    }
                }
                //catch (System.Reflection.TargetParameterCountException er) { Debug.WriteLine(JsonConvert.SerializeObject(er)); }
                //catch (System.Exception er) { Debug.WriteLine(JsonConvert.SerializeObject(er)); }
                catch { }
            }

            PlayList.Add(item);
            //PlayListView.SelectedIndex = PlayList.Count - 1;
            return PlayList.Count - 1;
        }

        public delegate void updatePlaylistCb(int index, bool DoPlay = false);
        private void updatePlaylist(int NewPosition, bool DoPlay = false)
        {
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
