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

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string displayTime(long time)
        {
            string ret = "";
            int Days = 0, Hours = 0, Minutes = 0, Seconds = 0;

            int ms = (int)(time % 1000);
            long TotalSeconds = (time - ms) / 1000;

            if (TotalSeconds >= 86400)
            {
                long reste = (TotalSeconds % 86400);
                Days = (int)((TotalSeconds - reste) / 86400);
                TotalSeconds = reste;
            }

            if (TotalSeconds >= 3600)
            {
                long reste = (TotalSeconds % 3600);
                Hours = (int)((TotalSeconds - reste) / 3600);
                TotalSeconds = reste;
            }

            if (TotalSeconds >= 60)
            {
                long reste = (TotalSeconds % 60);
                Minutes = (int)((TotalSeconds - reste) / 60);
                TotalSeconds = reste;
            }

            Seconds = (int)(TotalSeconds);

            if (Days > 0) { ret += ((Days < 10) ? "0" : "") + Days + "d "; }
            if (Hours > 0) { ret += ((Hours < 10) ? "0" : "") + Hours + ":"; }
            ret += ((Minutes < 10) ? "0" : "") + Minutes + ":" + ((Seconds < 10) ? "0" : "") + Seconds;

            return ret;
        }

        public static double UnixTimestamp() { return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }

        private void TimerUpdateButtonsSetUp()
        {
            ButtonPlayTimer = new System.Timers.Timer(100);
            ButtonPlayTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            ButtonPlayTimer.Start();
        }

        private double LastLibScan = UnixTimestamp();
        private int LastPlayRepeatStatus = 0;
        private int Timer_Count = 0;
        private int Timer_LastIndex = 0;
        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //Win1_SizeChanged(null, null);
                if (player.IsPlaying()) {
                    if (lastPlayStatus == false)
                    {
                        lastPlayStatus = true;
                        BtnPlayPause.Background = null;
                        PreviewCtrlPause.ImageSource = null;
                        BtnPlayPause.Background = new ImageBrush(Bimage("PlayButtonImg_Pause"));
                        PreviewCtrlPause.ImageSource = Bimage("MiniPlayButtonImg_Pause");
                    }
                }
                else
                {
                    if (lastPlayStatus == true)
                    {
                        lastPlayStatus = false;
                        BtnPlayPause.Background = null;
                        PreviewCtrlPause.ImageSource = null;
                        BtnPlayPause.Background = new ImageBrush(Bimage("PlayButtonImg_Play"));
                        PreviewCtrlPause.ImageSource = Bimage("MiniPlayButtonImg_Play");
                    }
                }
                if (PlayRepeatStatus == 0)
                {
                    if (LastPlayRepeatStatus != PlayRepeatStatus) { BtnRepeat.Background = null; BtnRepeat.Background = new ImageBrush(Bimage("RepeatButtonImg_None")); }
                }
                else if (PlayRepeatStatus == 1)
                {
                    if (LastPlayRepeatStatus != PlayRepeatStatus) { BtnRepeat.Background = null; BtnRepeat.Background = new ImageBrush(Bimage("RepeatButtonImg_One")); }
                }
                else
                {
                    if (LastPlayRepeatStatus != PlayRepeatStatus) { BtnRepeat.Background = null; BtnRepeat.Background = new ImageBrush(Bimage("RepeatButtonImg_All")); }
                }
                LastPlayRepeatStatus = PlayRepeatStatus;

                if (Timer_LastIndex != PlayListIndex)
                {
                    ObservableCollection<PlayListViewItem> previous_items;
                    if (PlayListView.ItemsSource != null) { previous_items = (ObservableCollection<PlayListViewItem>)PlayListView.ItemsSource; }
                    else { previous_items = new ObservableCollection<PlayListViewItem>(); }

                    //Debug.WriteLine("PlayListDisplayed loading");
                    Timer_LastIndex = PlayListIndex;
                    ObservableCollection<PlayListViewItem> tmp = new ObservableCollection<PlayListViewItem>();
                    int min = (PlayListIndex != -1) ? PlayListIndex : 0;
                    int max = PlayListIndex + 25;
                    string file;
                    PlayListViewItem item;
                    for (int i = min; i < max; i++)
                    {
                        if (PlayList2.Count <= i) { break; }
                        else
                        {
                            file = PlayList2[i][(PlayList2[i][1] != null)?1:0];
                            item = null;
                            foreach (PlayListViewItem itm in previous_items) {
                                if (itm.Path == file) { item = itm; break; }
                            }
                            if (item == null) { item = player.MediaInfo(file, false); }
                            if (item.Name == null || item.Name == "") { item.Name = Path.GetFileName(item.Path); }
                            if (PlayListIndex == i) { item.Selected = PlayListSelectionChar; } else { item.Selected = ""; }

                            tmp.Add(item); 
                        }
                    }
                    //Debug.WriteLine(JsonConvert.SerializeObject(tmp));
                    
                    PlayListView.ItemsSource = tmp;
                    PlayListView.Items.Refresh();

                    Label_PlayListDisplayedNBTracks.Text = "" + tmp.Count;
                    Label_PlayListNBTracks.Text = "" + PlayList2.Count;
                    Label_PlayListIndex.Text = "" + ( PlayListIndex + 1);
                }

                if (Timer_Count >= 50)
                {
                    Timer_Count = 0;
                    try
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    catch (Exception) { }
                }

                Timer_Count += 1;
            }));
        }
    }
}
