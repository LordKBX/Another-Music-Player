using System;
using System.IO;
using System.Windows;
using System.Timers;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TagLib.Ape;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary> Convert milliseconds times in human readable string </summary>
        public static string displayTime(long time)
        {
            string ret = ""; int Days = 0, Hours = 0, Minutes = 0;
            int ms = (int)(time % 1000); long TotalSeconds = (time - ms) / 1000, reste;
            if (TotalSeconds >= 86400) { reste = (TotalSeconds % 86400); Days = (int)((TotalSeconds - reste) / 86400); TotalSeconds = reste; }
            if (TotalSeconds >= 3600) { reste = (TotalSeconds % 3600); Hours = (int)((TotalSeconds - reste) / 3600); TotalSeconds = reste; }
            if (TotalSeconds >= 60) { reste = (TotalSeconds % 60); Minutes = (int)((TotalSeconds - reste) / 60); TotalSeconds = reste; }

            if (Days > 0) { ret += ((Days < 10) ? "0" : "") + Days + "d "; }
            if (Hours > 0) { ret += ((Hours < 10) ? "0" : "") + Hours + ":"; } //ret += ((Minutes < 10) ? "0" : "") + Minutes + ":" + ((TotalSeconds < 10) ? "0" : "") + TotalSeconds;
            return ret + ((Minutes < 10) ? "0" : "") + Minutes + ":" + ((TotalSeconds < 10) ? "0" : "") + TotalSeconds;
        }

        /// <summary> Generate current time Unix Timestamp </summary>
        public static double UnixTimestamp() { return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds; }
        /// <summary> Generate a Unix Timestamp </summary>
        public static double UnixTimestamp(int year, int month, int day, int housr=0, int minutes=0, int seconds=0) {
            DateTime date = new DateTime(year, month, day, housr, minutes, seconds, DateTimeKind.Utc);
            return (date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
        }

        /// <summary> Timer for periodic interface updates </summary>
        Timer TimerInterface = null;
        /// <summary> Create and start Timer for periodic interface updates </summary>
        private void TimerInterfaceSetUp()
        {
            if (TimerInterface != null) { return; }
            TimerInterface = new System.Timers.Timer(100);
            TimerInterface.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            TimerInterface.Start();
        }

        /// <summary> Store timer number of cycle, used for périodic Garbage Collector summon </summary>
        private int Timer_Count = 0;
        /// <summary> Store the last know status of playback repeat property by the timer </summary>
        private int Timer_PlayRepeatStatus = 0;
        /// <summary> Store the last know playlist index by the timer </summary>
        private int Timer_PlayListIndex = 0;
        /// <summary> Store the last Playing status by the timer </summary>
        private bool Timer_IsPlaying = false;

        /// <summary> Callback of the timer </summary>
        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //Win1_SizeChanged(null, null);

                // Section update display button Play/Pause
                if (player.IsPlaying()) {
                    if (Timer_IsPlaying == false)
                    {
                        Timer_IsPlaying = true; BtnPlayPause.Background = null; PreviewCtrlPause.ImageSource = null;
                        BtnPlayPause.Background = new ImageBrush(Bimage("PlayButtonImg_Pause"));
                        PreviewCtrlPause.ImageSource = Bimage("MiniPlayButtonImg_Pause");
                    }
                }
                else
                {
                    if (Timer_IsPlaying == true)
                    {
                        Timer_IsPlaying = false; BtnPlayPause.Background = null; PreviewCtrlPause.ImageSource = null;
                        BtnPlayPause.Background = new ImageBrush(Bimage("PlayButtonImg_Play"));
                        PreviewCtrlPause.ImageSource = Bimage("MiniPlayButtonImg_Play");
                    }
                }

                // Section PlayBack Repeat Status
                if (PlayRepeatStatus == 0)
                {
                    if (Timer_PlayRepeatStatus != PlayRepeatStatus) { BtnRepeat.Background = null; BtnRepeat.Background = new ImageBrush(Bimage("RepeatButtonImg_None")); }
                }
                else if (PlayRepeatStatus == 1)
                {
                    if (Timer_PlayRepeatStatus != PlayRepeatStatus) { BtnRepeat.Background = null; BtnRepeat.Background = new ImageBrush(Bimage("RepeatButtonImg_One")); }
                }
                else
                {
                    if (Timer_PlayRepeatStatus != PlayRepeatStatus) { BtnRepeat.Background = null; BtnRepeat.Background = new ImageBrush(Bimage("RepeatButtonImg_All")); }
                }
                Timer_PlayRepeatStatus = PlayRepeatStatus;

                // Section PlayList
                if (Timer_PlayListIndex != PlayListIndex)
                {
                    ObservableCollection<PlayListViewItem> previous_items;
                    if (PlayListView.ItemsSource != null) { previous_items = (ObservableCollection<PlayListViewItem>)PlayListView.ItemsSource; } else { previous_items = new ObservableCollection<PlayListViewItem>(); }
                    Timer_PlayListIndex = PlayListIndex;
                    ObservableCollection<PlayListViewItem> tmp = new ObservableCollection<PlayListViewItem>();
                    int min = (PlayListIndex != -1) ? PlayListIndex : 0;
                    int max = PlayListIndex + 100; //int max = PlayList.Count;  // test full list
                    string file; PlayListViewItem item;
                    for (int i = min; i < max; i++)
                    {
                        if (PlayList.Count <= i) { break; }
                        else
                        {
                            file = PlayList[i][0];
                            item = GetMediaInfo(file, previous_items);
                            if (item != null)
                            {
                                if (item.Name == null || item.Name == "") { item.Name = Path.GetFileName(item.Path); }
                                if (PlayListIndex == i) { item.Selected = PlayListSelectionChar; } else { item.Selected = ""; }
                                tmp.Add(item);
                                if (i == min) {
                                    UpdateLeftPannelMediaInfo(item);
                                }
                            }
                        }
                    }
                    //Debug.WriteLine(JsonConvert.SerializeObject(tmp));
                    try { player.MediaPicture(PlayList[PlayListIndex + 1][0]); } catch { }
                    PlayListView.ItemsSource = tmp;
                    PlayListView.Items.Refresh();

                    Label_PlayListDisplayedNBTracks.Text = "" + tmp.Count;
                    Label_PlayListNBTracks.Text = "" + PlayList.Count;
                    Label_PlayListIndex.Text = "" + ( PlayListIndex + 1);
                }

                // Garbage Collector périodic summon
                if (Timer_Count >= 50)
                { 
                    Timer_Count = 0; 
                    try { GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers(); } catch { }
                    try { if (MediatequeScanning == true) { MediatequeBuildNavigationContent(MediatequeCurrentFolder ?? MediatequeRefFolder); } } catch { }
                }

                Timer_Count += 1;
            }));
        }
    }
}
