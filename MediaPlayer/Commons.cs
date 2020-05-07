using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace MediaPlayer
{
    public class PlayListViewItem
    {
        public string Path { get; set; }
        public string OriginPath { get; set; }
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public long Duration { get; set; }
        public string DurationS { get; set; }
        public string Selected { get; set; }
        public object ToolTip { get; set; }

        public PlayListViewItem()
        {
            Path = OriginPath = Name = Album = Artist = Selected = DurationS = "test";
            Duration = 0;
            ToolTip = null;
        }
    }

    public partial class MainWindow : Window
    {
        public static string PlayListSelectionChar = "▶";

        public static string BaseDir = AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar;
        public static string BaseDirImg = AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "icons" + System.IO.Path.DirectorySeparatorChar;

        //public static BitmapImage CoverImg = new BitmapImage(new Uri(BaseDirImg + "album_small.png"));

        //public static BitmapImage OpenButtonImg = new BitmapImage(new Uri(BaseDirImg + "file.png"));
        //public static BitmapImage PreviousButtonImg = new BitmapImage(new Uri(BaseDirImg + "previous.png"));
        //public static BitmapImage PlayButtonImg_Play = new BitmapImage(new Uri(BaseDirImg + "play.png"));
        //public static BitmapImage PlayButtonImg_Pause = new BitmapImage(new Uri(BaseDirImg + "pause.png"));
        //public static BitmapImage NextButtonImg = new BitmapImage(new Uri(BaseDirImg + "next.png"));
        //public static BitmapImage ClearListButtonImg = new BitmapImage(new Uri(BaseDirImg + "clear_list.png"));
        //public static BitmapImage ShuffleButtonImg = new BitmapImage(new Uri(BaseDirImg + "shuffle.png"));

        //public static BitmapImage RepeatButtonImg_None = new BitmapImage(new Uri(BaseDirImg + "repeat_none.png"));
        //public static BitmapImage RepeatButtonImg_One = new BitmapImage(new Uri(BaseDirImg + "repeat_one.png"));
        //public static BitmapImage RepeatButtonImg_All = new BitmapImage(new Uri(BaseDirImg + "repeat_all.png"));

        //public static BitmapImage MiniPreviousButtonImg = new BitmapImage(new Uri(BaseDirImg + "previous_24.png"));
        //public static BitmapImage MiniPlayButtonImg_Play = new BitmapImage(new Uri(BaseDirImg + "play_24.png"));
        //public static BitmapImage MiniPlayButtonImg_Pause = new BitmapImage(new Uri(BaseDirImg + "pause_24.png"));
        //public static BitmapImage MiniNextButtonImg = new BitmapImage(new Uri(BaseDirImg + "next_24.png"));

        private static Dictionary<string, Uri> ImagesUriList = new Dictionary<string, Uri> {
            { "CoverImg", new Uri(BaseDirImg + "album_small.png") },
            { "OpenButtonImg", new Uri(BaseDirImg + "file.png") },
            { "PreviousButtonImg", new Uri(BaseDirImg + "previous.png") },
            { "PlayButtonImg_Play", new Uri(BaseDirImg + "play.png") },
            { "PlayButtonImg_Pause", new Uri(BaseDirImg + "pause.png") },
            { "NextButtonImg", new Uri(BaseDirImg + "next.png") },
            { "ClearListButtonImg", new Uri(BaseDirImg + "clear_list.png") },
            { "ShuffleButtonImg", new Uri(BaseDirImg + "shuffle.png") },

            { "RepeatButtonImg_None", new Uri(BaseDirImg + "repeat_none.png") },
            { "RepeatButtonImg_One", new Uri(BaseDirImg + "repeat_one.png") },
            { "RepeatButtonImg_All", new Uri(BaseDirImg + "repeat_all.png") },

            { "MiniPreviousButtonImg", new Uri(BaseDirImg + "previous_24.png") },
            { "MiniPlayButtonImg_Play", new Uri(BaseDirImg + "play_24.png") },
            { "MiniPlayButtonImg_Pause", new Uri(BaseDirImg + "pause_24.png") },
            { "MiniNextButtonImg", new Uri(BaseDirImg + "next_24.png") }
        };
        public static BitmapImage Bimage(string index) {
            if (ImagesUriList.ContainsKey(index)) { return new BitmapImage(ImagesUriList[index]); } else { return null; }
        }
    }
}
