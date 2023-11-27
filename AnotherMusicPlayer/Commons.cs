using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Diagnostics;

namespace AnotherMusicPlayer
{
    /// <summary> Class MediaItem, a storae structure for Music Media Basic MetaData </summary>
    public class PlayListViewItem
    {
        public string Selected { get; set; }
        public string Path { get; set; }
        public string OriginPath { get; set; }
        public string Name { get; set; }
        public string Album { get; set; }
        public long Duration { get; set; }
        public string DurationS { get; set; }
        public string Performers { get; set; }
        public string Composers { get; set; }
        public string AlbumArtists { get; set; }

        public string InnerUID { get; set; }

        public PlayListViewItem()
        {
            Path = OriginPath
                = Selected
                = Name
                = Album
                = DurationS
                = Performers
                = Composers
                = AlbumArtists
                = "";
            Duration = 0;

            InnerUID = Guid.NewGuid().ToString();
        }
    }

    public class Common
    {
        public static Int32 TimeStamp() { return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }

        public static bool IsFileLocked(string path)
        {
            FileInfo file = new FileInfo(path);
            try { using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None)) { stream.Close(); } }
            catch (IOException) { return true; }
            return false; //file is not locked
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(toEncode);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }

        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            return System.Text.ASCIIEncoding.UTF8.GetString(encodedDataAsBytes);
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
        public static bool IsDebug = false;

        [Conditional("DEBUG")]
        static public void DebugBaseDir()
        {
            IsDebug = true;
            Debug.WriteLine(">>> DebugBaseDir() <<<");
            MainWindow.BaseDir = "D:\\CODES\\VS\\MediaPlayer\\AnotherMusicPlayer";
        }

        /// <summary> Dictionary contening Image URI indexed by a string code name </summary>
        private static Dictionary<string, Uri> ImagesUriList = new Dictionary<string, Uri> {
            { "CoverImg", new Uri(BaseDirImg + "album_small.png") },
            { "OpenButtonImg", new Uri(BaseDirImg + "file.png") }
        };

        /// <summary> Create BitmapImage from ImagesUriList </summary>
        public static BitmapImage Bimage(string index)
        {
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
                    try
                    {
                        mw.GetTranslation("SizeBytesUnit");
                        mw = ((MainWindow)aa);
                    }
                    catch { }
                }
                if (mw == null) { return null; }
                string[] suf = {
                mw.GetTranslation("SizeBytesUnit"),
                    mw.GetTranslation("SizeBytesKilo"),
                    mw.GetTranslation("SizeBytesMega"),
                    mw.GetTranslation("SizeBytesGiga"),
                    mw.GetTranslation("SizeBytesTera"),
                    mw.GetTranslation("SizeBytesPeta"),
                    mw.GetTranslation("SizeBytesExa")
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
        public static string NormalizeNumber(int number, int length)
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
