using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

using Newtonsoft.Json;
using System.ComponentModel;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary> Callback Event Click on button Open File(s) </summary>
        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            win1.IsEnabled = false;
            bool DoConv = false;
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Audio (*.AIFF;*.AAC;*.FLAC;*.MP3;*.OGG;*.WMA;*.M4A)|*.AIFF;*.AAC;*.FLAC;*.MP3;*.OGG;*.WMA;*.M4A";
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "File Selection";
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                //if (player.IsPlaying()) { player.Stop(); }
                DoConv = Open(openFileDlg.FileNames);
            }
            if (DoConv == false) { Mouse.OverrideCursor = null; win1.IsEnabled = true; }
        }

        /// <summary> Callback Event Click on Play/Pause button </summary>
        private void Play_Button_Click(object sender, RoutedEventArgs e) { Pause(); }
        /// <summary> Callback Event Click on Previous Track button </summary>
        private void Previous_Button_Click(object sender, RoutedEventArgs e) { PreviousTrack(); }
        /// <summary> Callback Event Click on Next Track button </summary>
        private void Next_Button_Click(object sender, RoutedEventArgs e) { NextTrack(); }

        //private void PositionMoins10_Button_Click(object sender, RoutedEventArgs e) { player.Position(null, player.Position() - 10000); }
        //private void PositionPlus10_Button_Click(object sender, RoutedEventArgs e) { player.Position(null, player.Position() + 10000); }

        /// <summary> Callback Event Click on Clear List button </summary>
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            player.StopAll();
            PlayList.Clear();
            player.ClearCurrentFile();
            PlayListIndex = -1;
            UpdateRecordedQueue();
            Settings.LastPlaylistIndex = -1;

            PlayItemNameValue.ToolTip = PlayItemNameValue.Text = "";
            PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = "";
            PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = "";
            PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = "";
            FileCover.Source = Bimage("CoverImg");

            PlayListView.ItemsSource = new ObservableCollection<PlayListViewItemShort>();
            PlayListView.Items.Refresh();
            ClearLeftPannelMediaInfo();
            StopPlaylist();
            Label_PlayListDisplayedNBTracks.Text = "0";
            Label_PlayListNBTracks.Text = "0";
            Label_PlayListIndex.Text = "0";
        }

        /// <summary> Callback Event Click on Shuffle button </summary>
        private void BtnShuffle_Click(object sender, RoutedEventArgs e)
        {
            List<string[]> tmpList = new List<string[]>();
            List<int> pasts = new List<int>();
            Random rnd = new Random();
            string currentFile = (PlayListIndex > -1) ? PlayList[PlayListIndex][0] : null;
            int newIndex = PlayListIndex;

            int index = 0;
            while (tmpList.Count < PlayList.Count)
            {
                index = rnd.Next(0, PlayList.Count);
                if (pasts.Contains(index)) { continue; }
                tmpList.Add(PlayList[index]);
                pasts.Add(index);
                if (PlayList[index][0] == currentFile) { newIndex = tmpList.Count -1; }
            }

            PlayList = tmpList;
            PlayListIndex = newIndex;
            Timer_PlayListIndex = -1;
            UpdateRecordedQueue();
        }

        /// <summary> Callback Event Click on Repeat button </summary>
        private void BtnRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (PlayRepeatStatus <= 0) { PlayRepeatStatus = 1; player.Repeat(true); }
            else if (PlayRepeatStatus == 1) { PlayRepeatStatus = 2; player.Repeat(false); }
            else { PlayRepeatStatus = 0; player.Repeat(false); }
            Settings.LastRepeatStatus = PlayRepeatStatus;
            Settings.SaveSettingsAsync();
        }

        /// <summary> Callback Event Click on Debug Export Vars </summary>
        private void Debug_Button_Click(object sender, RoutedEventArgs e)
        {
            //setLoadingState(true);
            setMetadataScanningState(false);
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jss.Formatting = Formatting.Indented;
            jss.MaxDepth = 1;
            Debug.WriteLine("----------------------                        -----------------------");
            Debug.WriteLine("----------------------// Debug_Button_Click //-----------------------");
            Debug.WriteLine("----------------------                        -----------------------");
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName + "\\PlayList.txt", JsonConvert.SerializeObject(PlayList, jss), System.Text.Encoding.UTF8);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName + "\\PlayListIndex.txt", ""+PlayListIndex, System.Text.Encoding.UTF8);
            string output = "[" ;
            foreach (PlayListViewItemShort item in PlayListView.ItemsSource)
            {
                output += PrintPropreties(item);
            }
            output += "]";
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName + "\\PlayListView.txt", "" + output, System.Text.Encoding.UTF8);

            //setLoadingState(false);
        }

        private string PrintPropreties(object obj)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jss.Formatting = Formatting.Indented;
            jss.MaxDepth = 1;
            string ouput = "\t{";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                ouput += "\t\"" + descriptor.Name + "\" : " + JsonConvert.SerializeObject(descriptor.GetValue(obj), jss) + ",";
            }
            ouput += "\t},";
            return ouput;
        }
    }
}
