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
        private async void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            win1.IsEnabled = false;
            bool DoConv = false;
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            List<string> exts = new List<string>(Player.AcceptedExtentions);
            exts.AddRange(Player.AcceptedExtentionsFotConversion);
            exts.Sort();
            string extString = "*" + string.Join(";*", exts.ToArray()).ToUpper();
            openFileDlg.Filter = "Audio (" + extString + ")|" + extString + "| Web Radio File (*.M3U;*.M3U8)|*.M3U;*.M3U8";
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "File Selection";
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                string st = openFileDlg.FileNames[0].ToLower();
                if (st.EndsWith(".m3u") || st.EndsWith(".m3u8"))
                {
                    RadioPlayer.Init(st, RadioPlayer.RadioType.M3u);
                    bool ret = await RadioPlayer.Start();
                    if (ret == false)
                    {
                        if (RadioPlayer.filefound == false) { DialogBox.ShowDialog(this, "ERROR", "File not found", DialogBoxButtons.Ok, DialogBoxIcons.Error); }
                        else if (RadioPlayer.corrupted == true) { DialogBox.ShowDialog(this, "ERROR", "Corrupted or Invalid file format", DialogBoxButtons.Ok, DialogBoxIcons.Error); }
                        else { DialogBox.ShowDialog(this, "ERROR", "Stream ressource unavaillable", DialogBoxButtons.Ok, DialogBoxIcons.Error); }
                    }
                }
                else { DoConv = Open(openFileDlg.FileNames); }
            }
            if (DoConv == false) { Mouse.OverrideCursor = null; win1.IsEnabled = true; }
        }

        /// <summary> Callback Event Click on Play/Pause button </summary>
        private void Play_Button_Click(object sender, RoutedEventArgs e) { Pause(); }
        /// <summary> Callback Event Click on Previous Track button </summary>
        private void Previous_Button_Click(object sender, RoutedEventArgs e) { PreviousTrack(); CustomThumbnail_TabbedThumbnailBitmapRequested(null, null); }
        /// <summary> Callback Event Click on Next Track button </summary>
        private void Next_Button_Click(object sender, RoutedEventArgs e) { NextTrack(); CustomThumbnail_TabbedThumbnailBitmapRequested(null, null); }

        //private void PositionMoins10_Button_Click(object sender, RoutedEventArgs e) { player.Position(null, player.Position() - 10000); }
        //private void PositionPlus10_Button_Click(object sender, RoutedEventArgs e) { player.Position(null, player.Position() + 10000); }

        /// <summary> Callback Event Click on Clear List button </summary>
        public void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            player.PlaylistClear();

            //PlayItemNameValue.ToolTip = PlayItemNameValue.Text = "";
            //PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = "";
            //PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = "";
            //PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = "";
            //FileCover.Source = Bimage("CoverImg");

            DisplayPlaybackPositionBar.Value = 0;
            DisplayPlaybackPositionBar.IsIndeterminate = false;
            PlayListView.ItemsSource = new ObservableCollection<PlayListViewItem>();
            PlayListView.Items.Refresh();
            ClearLeftPannelMediaInfo();
            Label_PlayListDisplayedNBTracks.Text = "0";
            Label_PlayListNBTracks.Text = "0";
            Label_PlayListIndex.Text = "0";
        }

        /// <summary> Callback Event Click on Shuffle button </summary>
        private void BtnShuffle_Click(object sender, RoutedEventArgs e)
        {
            player.PlaylistRandomize();
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
            /*
            setMetadataScanningState(false);
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jss.Formatting = Formatting.Indented;
            jss.MaxDepth = 1;
            Debug.WriteLine("----------------------                        -----------------------");
            Debug.WriteLine("----------------------// Debug_Button_Click //-----------------------");
            Debug.WriteLine("----------------------                        -----------------------");
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName + "\\PlayList.txt", JsonConvert.SerializeObject(PlayList, jss), System.Text.Encoding.UTF8);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName + "\\PlayListIndex.txt", "" + PlayListIndex, System.Text.Encoding.UTF8);
            string output = "[";
            foreach (PlayListViewItem item in PlayListView.ItemsSource)
            {
                output += PrintPropreties(item);
            }
            output += "]";
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName + "\\PlayListView.txt", "" + output, System.Text.Encoding.UTF8);
            */
            DebugBaseDir();
            StyleUpdate();
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
