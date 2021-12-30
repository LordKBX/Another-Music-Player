using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace AnotherMusicPlayer
{
    /// <summary> Class PlayListViewItem, a storae structure for Music Media Basic MetaData </summary>
    public class PlayListViewItem
    {
        public string Selected { get; set; }
        public string Path { get; set; }
        public string OriginPath { get; set; }
        public string Name { get; set; }
        public string Album { get; set; }
        public long Size { get; set; }
        public long Duration { get; set; }
        public string DurationS { get; set; }
        public string Genres { get; set; }
        public string Performers { get; set; }
        public string Composers { get; set; }
        public string Copyright { get; set; }
        public uint Disc { get; set; }
        public uint DiscCount { get; set; }
        public string AlbumArtists { get; set; }
        public string Lyrics { get; set; }
        public uint Track { get; set; }
        public uint TrackCount { get; set; }
        public uint Year { get; set; }

        public PlayListViewItem()
        {
            Path = OriginPath
                = Selected
                = Name 
                = Album 
                = DurationS 
                = Performers
                = Composers
                = Copyright
                = AlbumArtists
                = Lyrics
                = "";
            Duration = Size = 0;
            Disc = DiscCount = Track = TrackCount = Year = 0;
        }
    }


    public partial class MainWindow : Window
    {
        /// <summary> Char used in first collumn of PlayListView for displaying current played/selected media </summary>
        public static string PlayListSelectionChar = "▶";
        /// <summary> Short notation for ystem.IO.Path.DirectorySeparatorChar </summary>
        public static char SeparatorChar = System.IO.Path.DirectorySeparatorChar;

        /// <summary> Base Diractory of the application </summary>
        public static string BaseDir = AppDomain.CurrentDomain.BaseDirectory + SeparatorChar;
        /// <summary> Base Diractory of the icon folder </summary>
        public static string BaseDirImg = AppDomain.CurrentDomain.BaseDirectory + SeparatorChar + "icons" + SeparatorChar;

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

        /// <summary> Add(+) image for ContextMenuItem </summary>
        public static System.Windows.Controls.Image ContextMenuItemImage_add = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(BaseDirImg + "add.png")) };
        public static System.Windows.Controls.Image ContextMenuItemImage_back = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(BaseDirImg + "goback.png")) };

        /// <summary> Dictionary contening Image URI indexed by a string code name </summary>
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

        /// <summary> Create BitmapImage from ImagesUriList </summary>
        public static BitmapImage Bimage(string index) {
            if (ImagesUriList.ContainsKey(index)) { return new BitmapImage(ImagesUriList[index]); } else { return null; }
        }

        /// <summary> Convert a binary/bytes length in human readable string </summary>
        public static String BytesLengthToString(long byteCount)
        {
            try
            {
                MainWindow mw = null;
                foreach (Window aa in Application.Current.Windows)
                {
                    try {
                        mw.GetTaduction("SizeBytesUnit");
                        mw = ((MainWindow)aa);
                    }
                    catch { }
                }
                if (mw == null) { return null; }
                string[] suf = {
                mw.GetTaduction("SizeBytesUnit"),
                    mw.GetTaduction("SizeBytesKilo"),
                    mw.GetTaduction("SizeBytesMega"),
                    mw.GetTaduction("SizeBytesGiga"),
                    mw.GetTaduction("SizeBytesTera"),
                    mw.GetTaduction("SizeBytesPeta"),
                    mw.GetTaduction("SizeBytesExa")
                }; //Longs run out around EB
                if (byteCount == 0)
                    return "0" + suf[0];
                long bytes = Math.Abs(byteCount);
                int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
                double num = Math.Round(bytes / Math.Pow(1024, place), 1);
                return (Math.Sign(byteCount) * num).ToString() + suf[place];
            }
            catch { return ""; }
        }

        /// <summary> Normalize a int into a fixed string length by adding 0 before the number until wanted string length reached </summary>
        public string NormalizeNumber(int number, int length)
        {
            string tstr = "" + number;
            int dif = length - tstr.Length;
            if (dif <= 0) { return tstr; }
            for (int i = 0; i < dif; i++) { tstr = '0' + tstr; }
            return tstr;
        }

        /// <summary> Return string hash of a byte array </summary>
        public static string bytesHash(byte[] data)
        {
            return Convert.ToBase64String(SHA512.Create().ComputeHash(data)); ;
        }

        /// <summary> Return file string hash </summary>
        public static string FileHash(string path)
        {
            using (FileStream s = System.IO.File.OpenRead(path)) { return Convert.ToBase64String(SHA512.Create().ComputeHash(s)); }
        }

        

    }
}
