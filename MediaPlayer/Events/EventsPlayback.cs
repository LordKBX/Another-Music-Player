using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MediaPlayer
{
    public partial class MainWindow : Window
    {
        #region Media Navigation Functions
        /// <summary>
        /// Play/Pause current media
        /// </summary>
        public void Pause() { if (player.IsPlaying()) { player.Pause(); } else { player.Resume(); } }

        /// <summary>
        /// Go to the previous media in PlayList
        /// </summary>
        public void PreviousTrack() { updatePlaylist(PlayListIndex - 1, true); }

        /// <summary>
        /// Go to the next media in PlayList
        /// </summary>
        public void NextTrack() { updatePlaylist(PlayListIndex + 1, true); }
        #endregion

        #region PlayBack Events
        /// <summary>
        /// Initialize Playback Events
        /// </summary>
        private void EventsPlaybackInit()
        {
            player.LengthChanged += Player_LengthChanged;
            player.PositionChanged += Player_PositionChanged;
            player.PlayStoped += Player_PlayStoped;
        }

        /// <summary>
        /// Event Callback when the played media length change(generaly when a new media is played)
        /// </summary>
        private void Player_LengthChanged(object sender, MediaLengthChangedEventParams e) 
        { 
            Dispatcher.BeginInvoke(new Action(() => { UpdateSize(displayTime((long)(e.Duration))); })); 
        }

        /// <summary>
        /// Event Callback when the media playing position chnaged
        /// </summary>
        private void Player_PositionChanged(object sender, MediaPositionChangedEventParams e)
        {
            Dispatcher.BeginInvoke(new Action(() => { UpdatePosition(displayTime((long)(e.Position))); }));
            if (PreventUpdateSlider) { return; }
            float BarCalc = (e.Position > e.Duration) ? 1000 : ((1000 * e.Position) / e.Duration);
            Dispatcher.BeginInvoke(new Action(() => { UpdatePositionBar((double)BarCalc); }));
        }

        /// <summary>
        /// Event Callback when the played media is stoped(not paused)
        /// </summary>
        private void Player_PlayStoped(object sender, MediaPositionChangedEventParams e)
        {
            //Debug.WriteLine("Player_PlayStoped");
            //Wait a second befor allowing to proceed with the nex event, by default loading a new media in the player generate a stoped event
            double newEnd = UnixTimestamp();
            if (newEnd <= lastEnd + 1) { return; }
            lastEnd = newEnd;

            if (PlayRepeatStatus <= 0)
            {
                if (PlayListIndex + 1 < PlayList2.Count) { Dispatcher.BeginInvoke(new Action(() => { updatePlaylist(PlayListIndex + 1, true); })); }
                else { Dispatcher.BeginInvoke(new Action(() => { StopPlaylist(); })); }
            }
            else if (PlayRepeatStatus == 1) { }
            else {
                if (PlayListIndex + 1 < PlayList2.Count) { Dispatcher.BeginInvoke(new Action(() => { updatePlaylist(PlayListIndex + 1, true); })); }
                else { Dispatcher.BeginInvoke(new Action(() => { updatePlaylist(0, true); })); }
            }
        }
        #endregion

        #region Utils
        /// <summary>
        /// Test if the file is of the correct File extention
        /// </summary>
        private bool MediaTestFileExtention(string FilePath)
        {
            List<string> extentions = player.AcceptedExtentions();
            foreach (string ext in extentions) { if (FilePath.EndsWith(ext)) { return true; } }
            return false;
        }

        /// <summary>
        /// Stop all playing media and return PlayList at first position
        /// </summary>
        private void StopPlaylist()
        {
            Debug.WriteLine("StopPlaylist");
            player.StopAll();
            UpdatePosition(displayTime(0));
            UpdateSize(displayTime(0));
            UpdatePositionBar(0);
            if (PlayList2.Count > 0) { updatePlaylist(0, false); }
        }
        #endregion
    }
}
