using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        #region Media Navigation Functions
        /// <summary> Play/Pause current media </summary>
        public void Pause(bool UpdateOnly = false)
        {
            if (player.IsPlaying())
            {
                if (UpdateOnly == false) { player.Pause(); }
                if (player.Mode == Player.Modes.Radio) { DisplayPlaybackPositionBar.IsIndeterminate = false; }
            }
            else
            {
                if (UpdateOnly == false) { player.Resume(); }
                if (player.Mode == Player.Modes.Radio) { DisplayPlaybackPositionBar.IsIndeterminate = true; }
            }
        }

        /// <summary> Go to the previous media in PlayList </summary>
        public void PreviousTrack() { UpdatePlaylist(PlayListIndex - 1, true); }

        /// <summary> Go to the next media in PlayList </summary>
        public void NextTrack() { UpdatePlaylist(PlayListIndex + 1, true); }
        #endregion

        #region PlayBack Events
        /// <summary> Initialize Playback Events </summary>
        private void EventsPlaybackInit()
        {
            player.LengthChanged += Player_LengthChanged;
            player.PositionChanged += Player_PositionChanged;
            player.PlayStoped += Player_PlayStoped;
            player.PlaylistChanged += Player_PlaylistChanged;
            player.PlaylistPositionChanged += Player_PlaylistPositionChanged;
        }

        private void Player_PlaylistPositionChanged(object sender, PlayerPlaylistPositionChangeParams e)
        {
            Debug.WriteLine("Player_PlaylistPositionChanged");
            Debug.WriteLine(JsonConvert.SerializeObject(e));
            PlayListIndex = e.Position;
            if (PlayListIndex < player.PlayList.Count)
            {
                string item = player.PlayList[PlayListIndex];
                if (item.StartsWith("Radio|"))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CustomThumbnail_TabbedThumbnailBitmapRequested(null, null);
                        string[] rtab = item.Trim().Split('|');
                        if (rtab[1].Trim() != "")
                        {
                            Dictionary<string, Dictionary<string, object>> data = bdd.DatabaseQuery("SELECT * FROM radios WHERE RID = " + rtab[1], "RID");
                        }
                        else { }
                        DisplayPlaybackPositionBar.Value = 0;
                        if (RadioPlayer.IsPlaying) { DisplayPlaybackPositionBar.IsIndeterminate = true; }
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DisplayPlaybackPositionBar.IsIndeterminate = false;
                    }));
                    MediaItem Fi = DatabaseItemToMediaItem(bdd.DatabaseFileInfo(item));
                    string ar = "";
                    if (Fi.Performers != null && Fi.Performers.Trim() != "") { ar += Fi.Performers.Replace(";", ", "); }
                    if (Fi.Composers != null && Fi.Composers.Trim() != "")
                    {
                        if (ar != "") { ar += ", "; }
                        ar += Fi.Composers.Replace(";", ", ");
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SetTitle(Fi.Name + ((Fi.Album != null && Fi.Album.Trim() != "") ? " - " + Fi.Album : "") + ((ar != "") ? " - " + ar : ""));
                        CustomThumbnail_TabbedThumbnailBitmapRequested(null, null);
                    }));
                }
            }
            Settings.LastPlaylistIndex = PlayListIndex;
            Settings.SaveSettings();
        }

        private void Player_PlaylistChanged(object sender, PlayerPlaylistChangeParams e)
        {
            Debug.WriteLine("Player_PlaylistChanged");
            Debug.WriteLine(JsonConvert.SerializeObject(e));
            PlayList.Clear();
            foreach (string item in e.playlist)
            {
                PlayList.Add(new string[] { item, "" });
            }
            UpdateRecordedQueue();
            Timer_Elapsed(null, null);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                CustomThumbnail_TabbedThumbnailBitmapRequested(null, null);
            }));
        }

        /// <summary> Event Callback when the played media length change(generaly when a new media is played) </summary>
        private void Player_LengthChanged(object sender, PlayerLengthChangedEventParams e)
        {
            Dispatcher.BeginInvoke(new Action(() => { UpdateSize(displayTime((long)(e.duration))); }));
        }

        public long lastPlaybackPosition = 0;
        /// <summary> Event Callback when the media playing position chnaged </summary>
        private void Player_PositionChanged(object sender, PlayerPositionChangedEventParams e)
        {
            lastPlaybackPosition = e.Position;
            Dispatcher.BeginInvoke(new Action(() => { UpdatePosition(displayTime((long)(e.Position))); }));
            if (PreventUpdateSlider) { return; }
            float BarCalc = (e.Position > e.duration) ? 1000 : ((1000 * e.Position) / e.duration);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdatePositionBar((double)BarCalc);
            }));
        }

        /// <summary> Event Callback when the played media is stoped(not paused) </summary>
        private void Player_PlayStoped(object sender, PlayerPositionChangedEventParams e)
        {
            //Debug.WriteLine("Player_PlayStoped");
            //Wait a second befor allowing to proceed with the nex event, by default loading a new media in the player generate a stoped event
            double newEnd = UnixTimestamp();
            if (newEnd <= PlaybackStopLastTime + 1) { return; }
            PlaybackStopLastTime = newEnd;

            if (PlayRepeatStatus <= 0)
            {
                if (PlayListIndex + 1 < PlayList.Count) { Dispatcher.BeginInvoke(new Action(() => { UpdatePlaylist(PlayListIndex + 1, true); })); }
                else { Dispatcher.BeginInvoke(new Action(() => { StopPlaylist(); })); }
            }
            else if (PlayRepeatStatus == 1) { UpdatePlaylist(PlayListIndex, true); }
            else
            {
                if (PlayListIndex + 1 < PlayList.Count) { Dispatcher.BeginInvoke(new Action(() => { UpdatePlaylist(PlayListIndex + 1, true); })); }
                else { Dispatcher.BeginInvoke(new Action(() => { UpdatePlaylist(0, true); })); }
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CustomThumbnail_TabbedThumbnailBitmapRequested(null, null);
            }));
        }
        #endregion

        #region Utils
        /// <summary> Test if the file is of the correct File extention </summary>
        private bool MediaTestFileExtention(string FilePath)
        {
            string[] extentions = Player.AcceptedExtentions;
            foreach (string ext in extentions) { if (FilePath.ToLower().EndsWith(ext)) { return true; } }
            return false;
        }

        /// <summary> Stop all playing media and return PlayList at first position </summary>
        private void StopPlaylist()
        {
            Debug.WriteLine("StopPlaylist");
            player.StopAll();
            UpdatePosition(displayTime(0));
            UpdateSize(displayTime(0));
            UpdatePositionBar(0);
            if (PlayList.Count > 0) { UpdatePlaylist(0, false); }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CustomThumbnail_TabbedThumbnailBitmapRequested(null, null);
            }));
        }
        #endregion
    }
}
