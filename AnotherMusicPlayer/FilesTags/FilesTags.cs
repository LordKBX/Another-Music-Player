using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using TagLib;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography;

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
                else { tags = TagLib.File.Create(OriginPath); }
                PlayListViewItem item = new PlayListViewItem();
                item.Name = tags.Tag.Title;
                item.Album = tags.Tag.Album;
                item.Path = FilePath;
                item.OriginPath = OriginPath;
                item.Selected = (Selected) ? MainWindow.PlayListSelectionChar : "";
                item.Duration = (long)0;
                item.Size = new System.IO.FileInfo(OriginPath ?? FilePath).Length;
                item.DurationS = "00:00";

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
                    if (System.IO.File.Exists(FilePath))
                    {
                        foreach (string ext in Player.AcceptedExtentions)
                        {
                            if (FilePath.EndsWith(ext))
                            {
                                AudioFileReader fir = new AudioFileReader(FilePath);
                                item.Duration = (long)fir.TotalTime.TotalMilliseconds;
                                fir.Dispose();
                                item.DurationS = MainWindow.displayTime(item.Duration);
                            }
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
