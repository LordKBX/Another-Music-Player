using AnotherMusicPlayer.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    public static class App
    {
        public static readonly string AppName = "AnotherMusicPlayer";
        public static Database bdd = new Database();

        private static bool _IsDebug = false;
        public static bool IsDebug { get { return App._IsDebug; } }

        [Conditional("DEBUG")]
        public static void TestDebug() { App._IsDebug = true; }

        public static ResourceDictionary Resources = new ResourceDictionary();
        public static AnotherMusicPlayer.MainWindow2Space.MainWindow2 win1;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            TestDebug();
            Settings.LoadSettings();
            TranslationUpdate();
            Player.INIT();

            win1 = new AnotherMusicPlayer.MainWindow2Space.MainWindow2();
            win1.ShowDialog();
            win1.Dispose();
            Player.StopAll(true);
            FilesTags.WaitTimersEnd();

            Environment.Exit(0);
        }

        public static void UnsetLockScreen() { /*win1.UnsetLockScreen();*/ }

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

        public static void TranslationUpdate()
        {
            if (Resources.MergedDictionaries.Count < 1) { Resources.MergedDictionaries.Add(new ResourceDictionary()); }
            string end = "";
            if (Settings.Lang == null) { end = "en"; }
            else if (Settings.Lang.StartsWith("fr-")) { end = "fr"; }
            else { end = "en"; }
            if (Resources.MergedDictionaries.Count < 1)
            {
                Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/AnotherMusicPlayer;component/Traductions/" + end + ".xaml", UriKind.Absolute) });
            }
            else
            {
                Resources.MergedDictionaries[0].Clear();
                Resources.MergedDictionaries[0] = new ResourceDictionary { Source = new Uri("pack://application:,,,/AnotherMusicPlayer;component/Traductions/" + end + ".xaml", UriKind.Absolute) };
            }
        }

        /// <summary> Get string stored in traduction file </summary>
        public static string GetTranslation(string reference)
        {
            try { return (string)Resources.MergedDictionaries[0][reference]; }
            catch { return ""; }
        }

        private static Dictionary<Control, ToolTip> DefinedToolTipArray = new Dictionary<Control, ToolTip>();

        public static bool SetToolTip(Control control, string tooltip)
        {
            try
            {
                if (DefinedToolTipArray.ContainsKey(control)) { DelToolTip(control); }
                ToolTip tp = new ToolTip();
                tp.SetToolTip(control, tooltip);
                DefinedToolTipArray.Add(control, tp);
                return true;
            }
            catch (Exception/* ex*/) { }
            return false;
        }

        public static bool DelToolTip(Control control)
        {
            try
            {
                if (DefinedToolTipArray.ContainsKey(control))
                {
                    DefinedToolTipArray[control].RemoveAll();
                    DefinedToolTipArray[control].Dispose();
                    DefinedToolTipArray.Remove(control);
                    return true;
                }
                else { return false; }
            }
            catch (Exception/* ex*/) { }
            return false;
        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static BitmapImage BitmapToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        /// <summary> Generate current time Unix Timestamp </summary>
        public static double UnixTimestamp() { return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds; }
        /// <summary> Generate a Unix Timestamp </summary>
        public static double UnixTimestamp(int year, int month, int day, int housr = 0, int minutes = 0, int seconds = 0)
        {
            DateTime date = new DateTime(year, month, day, housr, minutes, seconds, DateTimeKind.Utc);
            return (date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
        }

        public static MediaItem DatabaseItemToMediaItem(Dictionary<string, object> rep)
        {
            MediaItem item = null;
            item = new MediaItem();
            item.Path = (string)rep["Path"];
            item.Name = (string)rep["Name"];
            item.Album = (string)rep["Album"];
            item.AlbumArtists = (string)rep["AlbumArtists"];
            item.Performers = (string)rep["Performers"];
            item.Composers = (string)rep["Composers"];
            item.Lyrics = (string)rep["Lyrics"];
            item.Duration = Convert.ToInt64((string)rep["Duration"]);
            item.DurationS = displayTime(Convert.ToInt64((string)rep["Duration"]));
            item.Genres = (string)rep["Genres"];
            item.Copyright = (string)rep["Copyright"];
            item.Disc = Convert.ToUInt32(rep["Disc"]);
            item.DiscCount = Convert.ToUInt32(rep["DiscCount"]);
            item.Track = Convert.ToUInt32(rep["Track"]);
            item.TrackCount = Convert.ToUInt32(rep["TrackCount"]);
            item.Year = Convert.ToUInt32(rep["Year"]);
            item.Rating = Convert.ToDouble((rep["Rating"] as string).Replace(".", ","));

            return item;
        }

        public static Dictionary<string, object> MediaItemToDatabaseItem(MediaItem rep)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Path", rep.Path);
            ret.Add("Name", rep.Name);
            ret.Add("Album", rep.Album);
            ret.Add("AlbumArtists", rep.AlbumArtists);
            ret.Add("Performers", rep.Performers);
            ret.Add("Composers", rep.Composers);
            ret.Add("Lyrics", rep.Lyrics);
            ret.Add("Duration", rep.Duration);
            ret.Add("DurationS", displayTime(Convert.ToInt64(rep.Duration)));
            ret.Add("Genres", rep.Genres);
            ret.Add("Copyright", rep.Copyright);
            ret.Add("Disc", rep.Disc);
            ret.Add("DiscCount", rep.DiscCount);
            ret.Add("Track", rep.Track);
            ret.Add("TrackCount", rep.TrackCount);
            ret.Add("Year", rep.Year);
            return ret;
        }

        /// <summary> Convert a binary/bytes length in human readable string </summary>
        public static String BytesLengthToString(long byteCount)
        {
            try
            {
                string[] suf = {
                App.GetTranslation("SizeBytesUnit"),
                    App.GetTranslation("SizeBytesKilo"),
                    App.GetTranslation("SizeBytesMega"),
                    App.GetTranslation("SizeBytesGiga"),
                    App.GetTranslation("SizeBytesTera"),
                    App.GetTranslation("SizeBytesPeta"),
                    App.GetTranslation("SizeBytesExa")
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
