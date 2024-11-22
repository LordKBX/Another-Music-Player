using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Linq;
using MaterialDesignColors.ColorManipulation;

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
        public string Artists
        {
            get
            {
                List<string> list = new List<string>();
                if (Composers.Trim().Length > 0) 
                { 
                    List<string> cpl = Composers.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                    foreach (string comp in cpl) { if (!list.Contains(comp)) { list.Add(comp); } }
                }
                if (Performers.Trim().Length > 0) 
                { 
                    List<string> pel = Performers.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                    foreach (string perf in pel) { if (!list.Contains(perf)) { list.Add(perf); } }
                }
                if (AlbumArtists.Trim().Length > 0) 
                { 
                    List<string> aal = AlbumArtists.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                    foreach (string artist in aal) { if (!list.Contains(artist)) { list.Add(artist); } }
                }
                return (list.Count > 0)?string.Join("; ", list):"";
            }
        }

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

        public static PlayListViewItem FromFilePath(string path) {
            if (!File.Exists(path)) { return null; }
            Dictionary<string, object> ret = App.bdd.DatabaseFileInfo(path);
            if (ret != null)
            {
                PlayListViewItem item = new PlayListViewItem();
                item.Selected = "";
                item.Path = path;
                item.OriginPath = path;
                item.Name = "" + ret["Name"];
                item.Album = "" + ret["Album"];
                item.Duration = long.Parse("" + ret["Duration"]);
                item.DurationS = App.displayTime(item.Duration);
                item.Performers = "" + ret["Performers"];
                item.Composers = "" + ret["Composers"];
                item.AlbumArtists = "" + ret["AlbumArtists"];

                return item;
            }
            else
            { return FilesTags.MediaInfoShort(path, false); }
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

        static public System.Drawing.Color LightenDrawingColor(System.Drawing.Color input, int quantity)
        {
            System.Windows.Media.Color ncolor = System.Windows.Media.Color.FromArgb(input.A, input.R, input.G, input.B).Lighten(quantity);
            return System.Drawing.Color.FromArgb(ncolor.A, ncolor.R, ncolor.G, ncolor.B);
        }

        static public System.Drawing.Color DarkenDrawingColor(System.Drawing.Color input, int quantity)
        {
            System.Windows.Media.Color ncolor = System.Windows.Media.Color.FromArgb(input.A, input.R, input.G, input.B).Darken(quantity);
            return System.Drawing.Color.FromArgb(ncolor.A, ncolor.R, ncolor.G, ncolor.B);
        }
    }
}
