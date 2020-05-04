using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            win1.IsEnabled = false;
            bool DoConv = false;
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Audio (*.AAC;*.FLAC;*.MP3;*.OGG;*.WMA)|*.AAC;*.FLAC;*.MP3;*.OGG;*.WMA";
            openFileDlg.Multiselect = true;
            openFileDlg.Title = "File Selection";
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                //if (player.IsPlaying()) { player.Stop(); }
                if (MediaTestFileExtention(openFileDlg.FileName) == false)
                {
                    DoConv = true;
                    UpdateListView(player.MediaInfo(null, false, openFileDlg.FileName), true);
                    LoadFileAsync(openFileDlg.FileName);
                }
                else
                {
                    if (PlayList.Count > 0)
                    {
                        if (player.IsPlaying()) { UpdateListView(player.MediaInfo(openFileDlg.FileName, false), true); }
                        else {
                            updatePlaylist(UpdateListView(player.MediaInfo(openFileDlg.FileName, false)), true);
                        }
                    }
                    else { fileOpen(openFileDlg.FileName); }
                }
                
                if (openFileDlg.FileNames.Length > 1)
                {
                    for (int i = 1; i < openFileDlg.FileNames.Length; i++)
                    {
                        if (MediaTestFileExtention(openFileDlg.FileNames[i]) == false)
                        {
                            DoConv = true;
                            UpdateListView(player.MediaInfo(null, false, openFileDlg.FileNames[i]), true);
                            LoadFileAsync(openFileDlg.FileNames[i]);
                        }
                        else { UpdateListView(player.MediaInfo(openFileDlg.FileNames[i], false), true); }
                    }
                }
            }
            if (DoConv == false) { Mouse.OverrideCursor = null; win1.IsEnabled = true; }
        }

        private async void LoadFileAsync(string FileName) {
            string[] ra = FileName.Split(Path.DirectorySeparatorChar);
            string end = Path.GetTempPath() + Path.ChangeExtension(ra[ra.Length - 1], ".mp3");
            await player.Conv(FileName, end);

            if (PlayListView.Items.Count > 0)
            {
                int position = UpdateListView(player.MediaInfo(end, false, FileName), true);
                if (!player.IsPlaying()) {
                    for (int i = 0; i < PlayList.Count; i++)
                    {
                        if (i == position) { PlayList[i].Selected = PlayListSelectionChar; } else { PlayList[i].Selected = ""; }
                    }
                    fileOpen(end, FileName);
                }
            }
            else { fileOpen(end, FileName); }
            Mouse.OverrideCursor = null;
            win1.IsEnabled = true;
        }


        private void Play_Button_Click(object sender, RoutedEventArgs e) { Pause(); }
        private void Previous_Button_Click(object sender, RoutedEventArgs e) { PreviousTrack(); }
        private void Next_Button_Click(object sender, RoutedEventArgs e) { NextTrack(); }

        private void PositionMoins10_Button_Click(object sender, RoutedEventArgs e) { player.Position(null, player.Position() - 10000); }
        private void PositionPlus10_Button_Click(object sender, RoutedEventArgs e) { player.Position(null, player.Position() + 10000); }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            player.StopAll();
            PlayList.Clear();
            PlayListIndex = -1;
            PlayListView.SelectedItem = -1;
            PlayListView.Items.Refresh();

            PlayItemNameValue.ToolTip = PlayItemNameValue.Text = "";
            PlayItemAlbumValue.ToolTip = PlayItemAlbumValue.Text = "";
            PlayItemArtistsValue.ToolTip = PlayItemArtistsValue.Text = "";
            PlayItemDurationValue.ToolTip = PlayItemDurationValue.Text = "";
            FileCover.Source = CoverImg;
        }

        private void BtnShuffle_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PlayListViewItem> tmpList = new ObservableCollection<PlayListViewItem>();
            List<int> pasts = new List<int>();
            Random rnd = new Random();
            string currentFile = (PlayListIndex > -1) ? PlayList[PlayListIndex].Path : null;

            int index = 0;
            while (tmpList.Count < PlayList.Count)
            {
                index = rnd.Next(0, PlayList.Count);
                if (pasts.Contains(index)) { continue; }
                tmpList.Add(PlayList[index]);
                pasts.Add(index);
            }

            PlayList.Clear();
            for (int i = 0; i < tmpList.Count; i++)
            {
                PlayList.Add(tmpList[i]);
                if (currentFile != null)
                {
                    if (tmpList[i].Path == currentFile) { PlayListIndex = i; }
                }
            }
            PlayListView.Items.Refresh();
            if (currentFile != null) { PlayListView.SelectedIndex = PlayListIndex; }
        }

        private void BtnRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (PlayRepeatStatus <= 0) { PlayRepeatStatus = 1; player.Repeat(true); }
            else if (PlayRepeatStatus == 1) { PlayRepeatStatus = 2; player.Repeat(false); }
            else { PlayRepeatStatus = 0; player.Repeat(false); }
        }

        private void ParamsLibFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = OpenFolder();
            if (path != null)
            {
                ParamsLibFolderTextBox.Text = path;
                Settings.LibFolder = path;
                Settings.SaveSettings();
            }
        }

        private string OpenFolder()
        {
            string path = null;
            ResourceDictionary res1 = Resources.MergedDictionaries[1];
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = Settings.LibFolder; // Use current value for initial dir
            dialog.Title = (string)res1["ParamsLibFolderSelectorTitle"]; // instead of default "Save As"
            dialog.Filter = ((string)res1["ParamsLibFolderSelectorBlockerTitle"]) + "|*." + ((string)res1["ParamsLibFolderSelectorBlockerType"]); // Prevents displaying files
            dialog.FileName = (string)res1["ParamsLibFolderSelectorBlockerName"]; // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\"+((string)res1["ParamsLibFolderSelectorBlockerName"]) +"."+((string)res1["ParamsLibFolderSelectorBlockerType"]), "");
                path = path.Replace("."+ ((string)res1["ParamsLibFolderSelectorBlockerType"]), "");
                // If user has changed the filename, create the new directory
                if (!System.IO.Directory.Exists(path)) { return null; }
                // Our final value is in path
                Debug.WriteLine(path);
            }
            return path;
        }

    }
}
