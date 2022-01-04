using System;
using System.IO;
using NAudio;
using NAudio.Wave;
using TagLib;

namespace AnotherMusicPlayer
{
    class FilesTags
    {
        /// <summary> Recuperate Media MetaData(cover excluded) </summary>
        public static PlayListViewItem MediaInfo(string FilePath, bool Selected, string OriginPath = null)
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
                item.Selected = (Selected) ? MainWindow.PlayListSelectionChar : "";
                item.Duration = (long)0;
                item.DurationS = "00:00";

                item.Size = new System.IO.FileInfo(OriginPath ?? FilePath).Length;
                item.Performers = tags.Tag.JoinedPerformers;
                item.Composers = tags.Tag.JoinedComposers;
                item.Copyright = tags.Tag.Copyright;
                item.Disc = tags.Tag.Disc;
                item.DiscCount = tags.Tag.DiscCount;
                item.AlbumArtists = tags.Tag.JoinedAlbumArtists;
                item.Genres = tags.Tag.FirstGenre;
                item.Lyrics = tags.Tag.Lyrics;
                item.Track = tags.Tag.Track;
                item.TrackCount = tags.Tag.TrackCount;
                item.Year = tags.Tag.Year;

                tags.Dispose();

                try
                {
                    foreach (string ext in Player.AcceptedExtentions)
                    {
                        if (FilePath.EndsWith(ext))
                        {
                            AudioFileReader fir = new AudioFileReader(FilePath);
                            item.Duration = (long)fir.TotalTime.TotalMilliseconds;
                            fir.Dispose();
                            item.DurationS = MainWindow.displayTime(item.Duration);
                            break;
                        }
                    }
                }
                catch { }
                return item;
            }
            return null;
        }

        /// <summary> Recuperate Media MetaData(cover excluded) </summary>
        public static PlayListViewItemShort MediaInfoShort(string FilePath, bool Selected, string OriginPath = null)
        {
            if (System.IO.File.Exists(FilePath) || System.IO.File.Exists(OriginPath))
            {
                //Debug.WriteLine("MetaData Source: " + (OriginPath ?? FilePath));
                TagLib.File tags;
                if (System.IO.File.Exists(FilePath)) { tags = TagLib.File.Create(FilePath); }
                else { tags = TagLib.File.Create(OriginPath, ReadStyle.Average); FilePath = OriginPath; }
                PlayListViewItemShort item = new PlayListViewItemShort();
                item.Name = tags.Tag.Title;
                item.Album = tags.Tag.Album;
                item.Path = FilePath;
                item.OriginPath = OriginPath;
                item.Selected = (Selected) ? MainWindow.PlayListSelectionChar : "";
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
                        if (FilePath.EndsWith(ext))
                        {
                            AudioFileReader fir = new AudioFileReader(FilePath);
                            item.Duration = (long)fir.TotalTime.TotalMilliseconds;
                            fir.Dispose();
                            item.DurationS = MainWindow.displayTime(item.Duration);
                            break;
                        }
                    }
                }
                catch { }
                return item;
            }
            return null;
        }

    }
}
