using System;
using System.IO;
using System.Numerics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;

using System.Diagnostics;
using Newtonsoft.Json;
using System.Linq;
using System.Windows.Documents;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using Advexp;
using Advexp.DynamicSettings;
using Advexp.LocalDynamicSettings;

namespace MediaPlayer
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

        string AppLang = "en";

        int WindowWidthMode = 150;

        public ObservableCollection<PlayListViewItem> PlayList { get; set; }
        public PlayListViewItem PlayItem { get; set; }
        public static RoutedCommand GlobalCommand = new RoutedCommand();

        /// <summary>
        /// Used for storing object dependant of the Operating system
        /// </summary>
        private Dictionary<string, object> ListReferences = new Dictionary<string, object>();


        public MainWindow()
        {
            AppLang = System.Globalization.CultureInfo.CurrentCulture.Name;
            ParamsInit();

            this.DataContext = this;

            PlayList = new ObservableCollection<PlayListViewItem>();
            PlayItem = new PlayListViewItem();
            player = new Player2();


            InitializeComponent();

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

            TimerBtnPlayPauseInit();

            win1.SizeChanged += Win1_SizeChanged;

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;

            ParamsSetup();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                LowLevelKeyboardListener _listener = new LowLevelKeyboardListener(this);
                _listener.OnKeyPressed += _listener_OnKeyPressed;
                _listener.HookKeyboard();
                ListReferences.Add("KeyboardListener", _listener);
            }

            //Settings.LibFolder = "D:\\Music\\";
            ScanLibrary();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                ((LowLevelKeyboardListener)ListReferences["KeyboardListener"]).UnHookKeyboard(); 
            }
        }

        private void ScanLibrary()
        {
            if (Settings.LibFolder != null)
            {
                if (System.IO.Directory.Exists(Settings.LibFolder))
                {
                    LibTreeView.Items.Clear();
                    LastLibScan = UnixTimestamp();
                    DirectoryInfo di = new DirectoryInfo(Settings.LibFolder);
                    TreeViewItem tds = new TreeViewItem();
                    tds.Tag = tds.Header = di.FullName;
                    LoadFiles(Settings.LibFolder, tds);
                    LoadSubDirectories(Settings.LibFolder, tds);
                    LibTreeView.Items.Add(tds);
                }
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Settings.SaveSettings();
        }

        private void LoadSubDirectories(string dir, TreeViewItem td)
        {
            // Get all subdirectories  
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            // Loop through them to see if they have any other subdirectories  
            foreach (string subdirectory in subdirectoryEntries)
            {

                DirectoryInfo di = new DirectoryInfo(subdirectory);
                TreeViewItem tds = new TreeViewItem();
                tds.Tag = di.FullName;
                tds.Header = di.Name;
                LoadFiles(subdirectory, tds);
                LoadSubDirectories(subdirectory, tds);
                td.Items.Add(tds);
            }
        }
        
        private void LoadFiles(string dir, TreeViewItem td)
        {
            string[] Files = Directory.GetFiles(dir, "*.*");
            bool ok = false;
            string[] extensions = new string[] { ".mp3", ".wma", ".flac", ".ogg", ".aac" };

            // Loop through them to see files  
            foreach (string file in Files)
            {
                ok = false;
                foreach (string ext in extensions) {
                    if (file.ToLower().EndsWith(ext)) { ok = true; break; }
                }
                if (ok)
                {
                    FileInfo fi = new FileInfo(file);
                    TreeViewItem tds = new TreeViewItem();
                    tds.Tag = fi.FullName;
                    tds.Header = fi.Name;
                    td.Items.Add(tds);
                }
            }
        }




        void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            string re = e.KeyPressed.ToString();
            //Debug.WriteLine(re);
            if (re == "MediaPlayPause") { Pause(); }
            if (re == "MediaPreviousTrack") { PreviousTrack(); }
            if (re == "MediaNextTrack") { NextTrack(); }
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
            else {
                if (WindowWidthMode != 150)
                {
                    WindowWidthMode = 150;
                    Grid1.ColumnDefinitions[0].Width = 
                        Grid2.RowDefinitions[0].Height = new GridLength((double)150);
                    FileCover.Width = FileCover.Height = 150;
                }
            }
            Grid1.ColumnDefinitions[1].Width = new GridLength(TabControler.ActualWidth - WindowWidthMode - 5);

            foreach (TabItem tab in TabControler.Items) {
                tab.Width = ((TabControler.ActualWidth-4) / 4);
            }
            ((TabItem)(TabControler.Items[TabControler.Items.Count - 1])).Width -= 1;
        }

        private void fileOpen(string FilePath, string OriginPath = null, bool doPlay = true)
        {
            PlayListViewItem it = player.MediaInfo(FilePath, doPlay, OriginPath);
            lastEnd = UnixTimestamp();
            if (doPlay) {
                player.StopAll();
                int last = PlayListIndex;
                Dispatcher.BeginInvoke(new Action(() => {
                    try
                    {
                        PlayList[last].Selected = PlayListSelectionChar;
                        PlayListView.Items.Refresh();
                    }
                    catch (System.Exception er) { /* Debug.WriteLine(JsonConvert.SerializeObject(er)); */ }
                }));
            }
            player.Open(FilePath, doPlay);
            MediaLastOpen = FilePath;
            PlayListIndex = UpdateListView(it);

            PlayItemNameValue.ToolTip = PlayItemNameValue.Text = it.Name;
            PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = it.Album;
            PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = it.Artist;
            PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = it.Duration;

            FileCover.Source = null;
            if (it.Cover != null) { FileCover.Source = it.Cover; }
            else { FileCover.Source = Bimage("CoverImg"); }
            PlayListView.SelectedIndex = PlayListIndex;
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception) { }
        }

        private int UpdateListView(PlayListViewItem item, bool replace = false)
        {
            if (item == null) { return -1; }
            int Max = this.PlayListView.Items.Count;
            for (int i = 0; i < Max; i++)
            {
                try {
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
                            PlayList[i].Cover = item.Cover;
                            PlayListView.Items.Refresh();
                            //Debug.WriteLine(JsonConvert.SerializeObject(PlayList[i]));
                        }
                        return i;
                    }
                }
                catch (System.Reflection.TargetParameterCountException er) { /* Debug.WriteLine(JsonConvert.SerializeObject(er)); */ }
                catch (System.Exception er) { /* Debug.WriteLine(JsonConvert.SerializeObject(er)); */ }
            }

            PlayList.Add(item);
            PlayListView.SelectedIndex = PlayList.Count - 1;
            return PlayListView.SelectedIndex;
        }

        public delegate void updatePlaylistCb(int index, bool DoPlay = false);
        private void updatePlaylist(int NewPosition, bool DoPlay = false) {
            Debug.WriteLine("NewPosition = " + NewPosition);
            if (NewPosition >= 0 && NewPosition < PlayList.Count && NewPosition != PlayListIndex)
            {
                string FilePath = "";
                string OriginPath = "";
                for (int i = 0; i < PlayList.Count; i++)
                {
                    if (i == NewPosition) { PlayList[i].Selected = PlayListSelectionChar; FilePath = PlayList[i].Path; OriginPath = PlayList[i].OriginPath; } else { PlayList[i].Selected = ""; }
                }
                PlayListView.Items.Refresh();
                PlayListIndex = NewPosition;
                PlayListView.SelectedIndex = NewPosition;
                fileOpen(FilePath, OriginPath, DoPlay);
            }
        }

        private void Items_CurrentChanged(object sender, EventArgs e) { Items_CurrentChanged(); }
        private void Items_CurrentChanged()
        {
            Debug.WriteLine("Items_CurrentChanged");
            updatePlaylist(PlayListView.SelectedIndex, true);
        }

        private void LibTreeView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void LibTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string file = (string)((TreeViewItem)LibTreeView.SelectedItem).Tag;
            if (System.IO.File.Exists(file))
            {
                if (PlayListView.Items.Count > 0) { if (player.IsPlaying()) { UpdateListView(player.MediaInfo(file, false), true); } else { fileOpen(file); } }
                else { fileOpen(file); }
            }
            else if(System.IO.Directory.Exists(file))
            {
                foreach (TreeViewItem item in ((TreeViewItem)LibTreeView.SelectedItem).Items) {
                    file = (string)item.Tag;
                    if (System.IO.File.Exists(file))
                    {
                        if (PlayListView.Items.Count > 0) { if (player.IsPlaying()) { UpdateListView(player.MediaInfo(file, false), true); } else { fileOpen(file); } }
                        else { fileOpen(file); }
                    }
                }
            }
        }
    }
}
