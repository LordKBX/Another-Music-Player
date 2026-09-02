using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace AnotherMusicPlayer
{
    /// <summary> Class MediaItem, a storage structure for Music Media Basic MetaData </summary>
    public class PlayListViewItem
    {
        private Dictionary<string, byte[]> storage = new Dictionary<string, byte[]>();

        public string InnerUID { get; set; }
        public string Selected { get; set; }
        public long Duration { get; set; }
        public string DurationS { get; set; }

        public string Path
        {
            get { return Read("Path"); }
            set { Write("Path", ""+ value); }
        }
        public string OriginPath
        {
            get { return Read("OriginPath"); }
            set { Write("OriginPath", ""+ value); }
        }
        public string Name
        {
            get { return Read("Name"); }
            set { Write("Name", "" + value); }
        }
        public string Album
        {
            get { return Read("Album"); }
            set { Write("Album", "" + value); }
        }
        public string Performers
        {
            get { return Read("Performers"); }
            set { Write("Performers", "" + value); }
        }
        public string Composers
        {
            get { return Read("Composers"); }
            set { Write("Composers", "" + value); }
        }
        public string AlbumArtists
        {
            get { return Read("AlbumArtists"); }
            set { Write("AlbumArtists", "" + value); }
        }

        public string Artists
        {
            get
            {
                List<string> list = new List<string>();
                string cp = Composers;
                string pf = Performers;
                string aa = AlbumArtists;

                if (cp.Trim().Length > 0) 
                { 
                    List<string> cpl = cp.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                    foreach (string comp in cpl) { if (!list.Contains(comp)) { list.Add(comp); } }
                }
                if (pf.Trim().Length > 0) 
                { 
                    List<string> pel = pf.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                    foreach (string perf in pel) { if (!list.Contains(perf)) { list.Add(perf); } }
                }
                if (aa.Trim().Length > 0) 
                { 
                    List<string> aal = aa.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                    foreach (string artist in aal) { if (!list.Contains(artist)) { list.Add(artist); } }
                }
                return (list.Count > 0)?string.Join("; ", list).Trim().Trim(';').Trim() : "";
            }
        }

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

        private string Read(string name) {
            if (storage.ContainsKey(name)) {
                return Decompress(storage[name]);
            }
            else { return null; }
        }
        private void Write(string name, string data) {
            if (storage.ContainsKey(name))
            {
                storage[name] = Compress(data);
            }
            else { storage.Add(name, Compress(data)); }
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

        public static byte[] Compress(string data)
        {
            using (MemoryStream ms = new MemoryStream())
            using (GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true))
            {
                byte[] buffer = UTF8Encoding.UTF8.GetBytes(data);
                zs.Write(buffer, 0, buffer.Length);
                zs.Flush();
                return ms.ToArray();
            }
        }

        public static string Decompress(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, true))
            {
                byte[] buffer = new byte[50];
                List<byte> bytes = new List<byte>();
                int readSize = 0;
                int ireadSize = 50;
                while (zs.CanRead && ireadSize >= 50)
                {
                    for (int i = 0; i < 50; i++) { buffer[i] = 0; }
                    ireadSize = zs.Read(buffer, 0, 50);
                    readSize += ireadSize;
                    bytes.AddRange(buffer);
                }
                for (int i = bytes.Count - 1; i >= readSize; i--)
                {
                    bytes.RemoveAt(i);
                }

                // Utilisation de System.Text.Json pour la désérialisation
                return UTF8Encoding.UTF8.GetString(bytes.ToArray());
            }
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
            System.Windows.Media.Color ncolor = System.Windows.Media.Color.FromArgb(input.A, input.R, input.G, input.B) + System.Windows.Media.Color.FromScRgb(quantity, quantity, quantity, quantity);
            return System.Drawing.Color.FromArgb(ncolor.A, ncolor.R, ncolor.G, ncolor.B);
        }

        static public System.Drawing.Color DarkenDrawingColor(System.Drawing.Color input, int quantity)
        {
            System.Windows.Media.Color ncolor = System.Windows.Media.Color.FromArgb(input.A, input.R, input.G, input.B) - System.Windows.Media.Color.FromScRgb(quantity, quantity, quantity, quantity);
            return System.Drawing.Color.FromArgb(ncolor.A, ncolor.R, ncolor.G, ncolor.B);
        }

        static public bool StringContainsUnicode(string input)
        {
            const int MaxAnsiCode = 255;

            return input.Any(c => c > MaxAnsiCode);
        }

        static public void PurgeMemory() 
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    public static class ControlExtensions
    {
        public static T Clone<T>(this T controlToClone)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;
        }
    }
}
