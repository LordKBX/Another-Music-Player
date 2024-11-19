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
using System.Runtime.InteropServices;

namespace AnotherMusicPlayer
{

    public partial class MainWindow : Window
    {

        /// <summary> Generate current time Unix Timestamp </summary>
        public static double UnixTimestamp() { return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds; }
        /// <summary> Generate a Unix Timestamp </summary>
        public static double UnixTimestamp(int year, int month, int day, int housr = 0, int minutes = 0, int seconds = 0)
        {
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
            //return;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //Win1_SizeChanged(null, null);

                // Section update display button Play/Pause
                bool playing = false;
                if (Player.Mode == Player.Modes.File) { playing = Player.IsPlaying(); }
                else { playing = RadioPlayer.IsPlaying; }
                if (playing)
                {
                    if (Timer_IsPlaying == false)
                    {
                        Timer_IsPlaying = true;
                        BtnPlayPause.Tag = "Pause";
                        try { buttonPlay.Icon = IconPause; CustomThumbnail_TabbedThumbnailBitmapRequested(null, null); }
                        catch { }
                    }
                }
                else
                {
                    if (Timer_IsPlaying == true)
                    {
                        Timer_IsPlaying = false;
                        BtnPlayPause.Tag = "Play";
                        try { buttonPlay.Icon = IconPlay; CustomThumbnail_TabbedThumbnailBitmapRequested(null, null); }
                        catch { }
                    }
                }

                // Section PlayBack Repeat Status
                if (PlayRepeatStatus == 0)
                {
                    if (Timer_PlayRepeatStatus != PlayRepeatStatus)
                    {
                        BtnRepeat.Tag = "Off";
                        Player.Repeat(false);
                    }
                }
                else if (PlayRepeatStatus == 1)
                {
                    if (Timer_PlayRepeatStatus != PlayRepeatStatus)
                    {
                        BtnRepeat.Tag = "One";
                        Player.Repeat(true);
                    }
                }
                else
                {
                    if (Timer_PlayRepeatStatus != PlayRepeatStatus)
                    {
                        BtnRepeat.Tag = "All";
                        Player.Repeat(false);
                    }
                }
                Timer_PlayRepeatStatus = PlayRepeatStatus;

                // Section PlayList
                if (Timer_PlayListIndex != PlayListIndex || sender == null)
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
                            if (file.StartsWith("Radio|"))
                            {
                                string[] rtab = file.Split('|');
                                item = new PlayListViewItem() { Name = file, Album = rtab[2], DurationS = "∞", OriginPath = file };
                                if (rtab[1].Trim() != "")
                                {
                                    Dictionary<string, Dictionary<string, object>> data = bdd.DatabaseQuery("SELECT * FROM radios WHERE RID = " + rtab[1], "RID");
                                    item.Name = data["" + rtab[1].Trim()]["Name"] as string;
                                }
                                tmp.Add(item);
                                UpdateLeftPannelMediaInfo(file);
                                break;
                            }
                            else
                            {
                                item = GetMediaInfoShort(file, previous_items);
                                if (item != null)
                                {
                                    if (item.Name == null || item.Name == "") { item.Name = Path.GetFileName(item.Path); }
                                    if (PlayListIndex == i) { item.Selected = PlayListSelectionChar; } else { item.Selected = ""; }
                                    tmp.Add(item);
                                    if (i == min)
                                    {
                                        UpdateLeftPannelMediaInfo(file);
                                    }
                                }
                            }
                        }
                    }

                    //Debug.WriteLine(JsonConvert.SerializeObject(tmp));
                    //try { FilesTags.MediaPicture(PlayList[PlayListIndex + 1][0], bdd); } catch { try { FilesTags.MediaPicture(PlayList[0][0], bdd); } catch { } }

                    //Debug.WriteLine("--> BEFORE PlayListView.ItemsSource UPDATE <--");
                    try
                    {
                        if (tmp.Count <= 0) { return; }
                        ((ObservableCollection<PlayListViewItem>)PlayListView.ItemsSource).Clear();
                        PlayListView.ItemsSource = tmp;
                        PlayListView.Items.Refresh();
                        PlayListView.ScrollIntoView(PlayListView.Items[0]);
                    }
                    catch { Debug.WriteLine("PlayListView.ItemsSource error"); }

                    Label_PlayListDisplayedNBTracks.Text = "" + tmp.Count;
                    Label_PlayListNBTracks.Text = "" + PlayList.Count;
                    Label_PlayListIndex.Text = "" + (PlayListIndex + 1);

                    previous_items.Clear();
                }

                // Garbage Collector périodic summon
                if (Timer_Count >= 50)
                {
                    Timer_Count = 0;
                    try
                    {
                        GC.Collect(); GC.WaitForPendingFinalizers();
                        GC.Collect(); GC.WaitForPendingFinalizers();
                    }
                    catch { }
                }
                CustomThumbnail_TabbedThumbnailBitmapRequested(null, null);
                Timer_Count += 1;
            }));
        }
    }
}
