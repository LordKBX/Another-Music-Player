using AnotherMusicPlayer.MainWindow2Space;
using NAudio;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TagLib;

namespace AnotherMusicPlayer
{
    /// <summary> Class MediaItem, a storae structure for Music Media Basic MetaData </summary>
    public class MediaItem
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
        public string Artists { 
            get {
                List<string> cpl = Composers.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                List<string> pel = Performers.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                List<string> aal = AlbumArtists.Replace("; ", ";").Replace(" ;", ";").Replace(" ; ", ";").Split(';').ToList();
                foreach (string perf in pel) { if (!cpl.Contains(perf)) { cpl.Add(perf); } }
                foreach (string perf in aal) { if (!cpl.Contains(perf)) { cpl.Add(perf); } }
                return string.Join("; ", cpl);
            }
        }
        public string Copyright { get; set; }
        public uint Disc { get; set; }
        public uint DiscCount { get; set; }
        public string AlbumArtists { get; set; }
        public string Lyrics { get; set; }
        public uint Track { get; set; }
        public uint TrackCount { get; set; }
        public uint Year { get; set; }
        public double Rating { get; set; }
        public uint PlayCount { get; set; }

        public string InnerUID { get; set; }

        public MediaItem()
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
            Disc = DiscCount = Track = TrackCount = Year = PlayCount = 0;
            Rating = 0.0;

            InnerUID = Guid.NewGuid().ToString();
        }
    }

    public partial class FilesTags
    {
        private static Dictionary<byte, double> TableRateWindows = new Dictionary<byte, double>() { { 0, 0.0 }, { 1, 1.0 }, { 64, 2.0 }, { 128, 3.0 }, { 196, 4.0 }, { 255, 5.0 } };
        private static Dictionary<double, byte> ReverseTableRateWindows = new Dictionary<double, byte>() { { 0.0, 0 }, { 1.0, 1 }, { 2.0, 64 }, { 3.0, 128 }, { 4.0, 196 }, { 5.0, 255 } };
        private static Dictionary<byte, double> TableRatePlayer = new Dictionary<byte, double>() { { 0, 0.0 }, { 2, 1.0 }, { 64, 2.0 }, { 128, 3.0 }, { 196, 4.0 }, { 255, 5.0 } };
        private static Dictionary<double, byte> ReverseTableRatePlayer = new Dictionary<double, byte>() { { 0.0, 0 }, { 1.0, 2 }, { 2.0, 64 }, { 3.0, 128 }, { 4.0, 196 }, { 5.0, 255 } };

        /// <summary> Recuperate Media MetaData(cover excluded) </summary>
        public static MediaItem MediaInfo(string FilePath, bool Selected, string OriginPath = null)
        {
            try
            {
                if (System.IO.File.Exists(FilePath) || System.IO.File.Exists(OriginPath))
                {
                    //Debug.WriteLine("MetaData Source: " + (OriginPath ?? FilePath));
                    TagLib.File tags;
                    if (System.IO.File.Exists(FilePath)) { tags = TagLib.File.Create(FilePath, ReadStyle.Average); }
                    else { tags = TagLib.File.Create(OriginPath, ReadStyle.Average); FilePath = OriginPath; }
                    MediaItem item = new MediaItem();
                    item.Name = tags.Tag.Title;
                    item.Album = tags.Tag.Album;
                    item.Path = FilePath;
                    item.OriginPath = OriginPath;
                    item.Selected = (Selected) ? MainWindow2.PlayListSelectionChar : "";
                    item.Duration = (long)0;
                    item.DurationS = "00:00";

                    item.Size = new System.IO.FileInfo(OriginPath ?? FilePath).Length;
                    item.Performers = string.Join(';', tags.Tag.Performers);
                    item.Composers = string.Join(';', tags.Tag.Composers);
                    item.Copyright = tags.Tag.Copyright;
                    item.Disc = tags.Tag.Disc;
                    item.DiscCount = tags.Tag.DiscCount;
                    item.AlbumArtists = tags.Tag.JoinedAlbumArtists;
                    item.Genres = string.Join(';', tags.Tag.Genres).Trim();
                    item.Lyrics = tags.Tag.Lyrics;
                    item.Track = tags.Tag.Track;
                    item.TrackCount = tags.Tag.TrackCount;
                    item.Year = tags.Tag.Year;

                    TagLib.Tag tag = tags.GetTag(TagLib.TagTypes.Id3v2);
                    byte rate1 = 0;

                    try { rate1 = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "Windows Media Player 9 Series", true).Rating; } 
                    catch(Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }

                    if (TableRatePlayer.ContainsKey(rate1)) { item.Rating = TableRatePlayer[rate1]; }
                    else
                    {
                        byte min = 255;
                        foreach (byte i in TableRatePlayer.Keys) { if (rate1 - i >= 0) { min = i; } }
                        if (min == 255) { item.Rating = 5; }
                        else { item.Rating = (double)(TableRatePlayer[min] + 0.5); }
                    }
                    tags.Dispose();

                    try
                    {
                        foreach (string ext in Player.AcceptedExtentions)
                        {
                            if (FilePath.ToLower().EndsWith(ext))
                            {
                                //Debug.WriteLine("File ext ok");
                                item.Duration = (long)((new AudioFileReader(FilePath)).TotalTime.TotalMilliseconds);
                                item.DurationS = App.displayTime(item.Duration); break;
                            }
                        }
                    }
                    catch { }
                    return item;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("--> FilesTags.MediaItem ERROR <--");
                Debug.WriteLine(err.Message);
                Debug.WriteLine(err.ToString());
                //Debug.WriteLine(JsonConvert.SerializeObject(err));
            }
            return null;
        }

        /// <summary> Save Media MetaData </summary>
        public static bool SaveMediaInfo(string FilePath, MediaItem info, string OriginFile = null)
        {
            try
            {
                if (!System.IO.File.Exists(FilePath)) { return false; }
                else
                {
                    //Debug.WriteLine("MetaData Source: " + (OriginPath ?? FilePath));
                    TagLib.File tags;
                    if (!System.IO.File.Exists(FilePath)) { return false; }
                    tags = TagLib.File.Create(FilePath, ReadStyle.Average);
                    tags.Tag.Title = info.Name;
                    tags.Tag.Album = info.Album;
                    tags.Tag.Performers = info.Performers.Split(';');
                    tags.Tag.Composers = info.Composers.Split(';');
                    tags.Tag.AlbumArtists = info.AlbumArtists.Split(';');
                    tags.Tag.Genres = info.Genres.Split(';');
                    tags.Tag.Copyright = info.Copyright;
                    tags.Tag.Lyrics = info.Lyrics;
                    tags.Tag.Disc = info.Disc;
                    tags.Tag.DiscCount = info.DiscCount;
                    tags.Tag.Track = info.Track;
                    tags.Tag.TrackCount = info.TrackCount;
                    tags.Tag.Year = info.Year;

                    TagLib.Tag tag = tags.GetTag(TagLib.TagTypes.Id3v2);
                    byte r2 = 0;
                    if (ReverseTableRateWindows.ContainsKey(info.Rating)) { r2 = ReverseTableRatePlayer[info.Rating]; }
                    else { r2 = (byte)(ReverseTableRatePlayer[Math.Truncate(info.Rating)] + 1); }
                    TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "Windows Media Player 9 Series", true).Rating = r2;

                    if (OriginFile != null)
                    {
                        TagLib.File tags2 = TagLib.File.Create(OriginFile);
                        if (tags.Tag.Pictures.Length > 0)
                        {
                            TagLib.IPicture pic = tags2.Tag.Pictures[0];

                            MemoryStream ms = new MemoryStream(pic.Data.Data);
                            ms.Seek(0, SeekOrigin.Begin);

                            TagLib.Picture pic2 = new TagLib.Picture();
                            pic2.Type = TagLib.PictureType.FrontCover; pic2.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg; pic2.Description = "Cover";
                            pic2.Data = TagLib.ByteVector.FromStream(ms); tags.Tag.Pictures = new TagLib.IPicture[1] { pic2 };
                            ms.Close();
                        }

                        tags.Save();
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("--> FilesTags.SaveMediaInfo ERROR <--");
                Debug.WriteLine(err.Message);
                Debug.WriteLine(err.ToString());
            }
            return true;
        }

        /// <summary> Recuperate Media MetaData(cover excluded) </summary>
        public static PlayListViewItem MediaInfoShort(string FilePath, bool Selected, string OriginPath = null)
        {
            if (System.IO.File.Exists(FilePath) || System.IO.File.Exists(OriginPath))
            {
                //Debug.WriteLine("MetaData Source: " + (OriginPath ?? FilePath));
                TagLib.File tags;
                if (System.IO.File.Exists(FilePath)) { tags = TagLib.File.Create(FilePath); }
                else { tags = TagLib.File.Create(OriginPath, ReadStyle.Average); FilePath = OriginPath; }
                PlayListViewItem item = new PlayListViewItem();
                item.Name = tags.Tag.Title;
                item.Album = tags.Tag.Album;
                item.Path = FilePath;
                item.OriginPath = OriginPath;
                item.Selected = (Selected) ? MainWindow2.PlayListSelectionChar : "";
                item.Duration = (long)0;
                item.DurationS = "00:00";

                item.Performers = tags.Tag.JoinedPerformers;
                item.Composers = tags.Tag.JoinedComposers;
                item.AlbumArtists = tags.Tag.JoinedAlbumArtists;

                tags.Dispose();

                try
                {
                    foreach (string ext in Player.AcceptedExtentions)
                    {
                        if (FilePath.ToLower().EndsWith(ext))
                        {
                            item.Duration = (long)((new AudioFileReader(FilePath)).TotalTime.TotalMilliseconds);
                            item.DurationS = App.displayTime(item.Duration); break;
                        }
                    }
                }
                catch { }
                return item;
            }
            return null;
        }

        private static List<SaveRatingObejct> saveRatingObejcts = new List<SaveRatingObejct>();
        private static Thread? saveRatingThread = null;
        private static bool saveRatingThreadRun = true;
        public static bool SaveRating(string FilePath, double RatingValue)
        {
            if (!System.IO.File.Exists(FilePath)) { return false; }
            if (RatingValue < 0) { RatingValue = 0; }
            if (RatingValue > 5.0) { RatingValue = 5.0; }
            bool found = false;
            foreach (SaveRatingObejct obj in saveRatingObejcts) {
                if (obj.Path == FilePath) { obj.Rate = RatingValue; found = true; break; }
            }
            if (!found) { saveRatingObejcts.Add(new SaveRatingObejct() { Count = 0, Path = FilePath, Rate = RatingValue }); }
            
            if (saveRatingThread == null) {
                Thread objThread = new Thread(new ThreadStart(SaveRatingLoop));
                objThread.IsBackground = true;
                objThread.Priority = ThreadPriority.AboveNormal;
                objThread.Start();
            }
            return true;
        }
        public static void SaveRatingLoop()
        {
            List<SaveRatingObejct> rmList = new List<SaveRatingObejct>();
            while (saveRatingThreadRun)
            {
                try
                {
                    foreach (SaveRatingObejct obj in saveRatingObejcts)
                    {
                        bool passed = false;
                        try
                        {
                            if (Player.GetCurrentFile() == obj.Path) { obj.Count += 1; continue; }

                            TagLib.Id3v2.Tag.DefaultVersion = 3; TagLib.Id3v2.Tag.ForceDefaultVersion = true;

                            TagLib.File fi = TagLib.File.Create(obj.Path, ReadStyle.Average);
                            TagLib.Tag tag = fi.GetTag(TagTypes.Id3v2);
                            TagLib.Id3v2.PopularimeterFrame frame1 = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "Windows Media Player 9 Series", true);
                            if (ReverseTableRateWindows.ContainsKey(obj.Rate)) { frame1.Rating = ReverseTableRatePlayer[obj.Rate]; }
                            else { frame1.Rating = (byte)(ReverseTableRatePlayer[Math.Truncate(obj.Rate)] + 1); }
                            fi.Save();
                            Debug.WriteLine("Ratting for '" + obj.Path + "' saved !");
                            rmList.Add(obj);
                            passed = true;
                        }
                        catch (IOException)
                        {
                            Debug.WriteLine("IOException for '" + obj.Path + "', Count = " + obj.Count);
                            obj.Count += 1;
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine("Generic Exception for '" + obj.Path + "', Count = " + obj.Count);
                            obj.Count += 1;
                        }
                        if (obj.Count >= 800 && !passed)
                        {
                            rmList.Add(obj);
                            Debug.WriteLine("Failed to save rating for file: " + obj.Path);
                            //MessageBox.Show(
                            //    "Failed to save rating for file: " + obj.Path,
                            //    "Error",
                            //    MessageBoxButtons.OK,
                            //    MessageBoxIcon.Error
                            //);
                        }
                    }

                    if (rmList.Count > 0)
                    {
                        foreach (SaveRatingObejct obj in rmList) { saveRatingObejcts.Remove(obj); }
                        rmList.Clear();
                    }

                    Thread.Sleep(500);
                }
                catch (Exception) { }
            }
        }

        public static void WaitTimersEnd()
        {
            saveRatingThreadRun = false;
            Thread.Sleep(500);
        }
    }
}
