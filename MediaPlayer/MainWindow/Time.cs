using System;
using System.Windows;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private void TimerBtnPlayPauseInit()
        {
            ButtonPlayTimer = new System.Timers.Timer(100);
            ButtonPlayTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            ButtonPlayTimer.Start();
        }

        private double LastLibScan = UnixTimestamp();
        private int LastPlayRepeatStatus = 0;
        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Win1_SizeChanged(null, null);
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

                if (LastLibScan + 3600 <= UnixTimestamp()) {
                    ScanLibrary();
                }
            }));
        }
    }
}
