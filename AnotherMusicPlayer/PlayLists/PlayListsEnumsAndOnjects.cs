using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace AnotherMusicPlayer
{
    public enum AutoPlaylistTypes { LastImports, MostPlayed, MostRecentlyPlayed, BestRating }

    internal class PlayListsLineItem{
        public int PlayCount { get; set; } = 0;
        public int PlaylistId { get; set; } = 0;
        public int PlaylistItemId { get; set; } = 0;
        public int PlaylistOrder { get; set; } = 0;
        public string Path { get; set; }
        public string Name { get; set; }
        public string Artists { get; set; }
        public string Album { get; set; }
        private long DurationValue { get; set; } = 0;
        public string Duration { get { return App.displayTime(DurationValue); } }
        private float _RatingValue = 0;
        private float RatingValue { 
            get { return _RatingValue; } 
            set { _RatingValue = value; DefineRatting(); } 
        }
        public Bitmap Rating { get; set; } = null;

        public PlayListsLineItem(string path, string name, string artists, string album, long duration, float rating) 
        {
            Path = path;
            Name = name;
            Artists = artists;
            Album = album;
            DurationValue = duration;
            RatingValue = rating;
            DefineRatting();
        }

        private void DefineRatting() 
        {
            if (Rating != null) { Rating.Dispose(); Rating = null; }
            string name = "stars_" + ("" + _RatingValue).Replace(".", "_").Replace(",", "_");
            if (name.Split('_').Length == 2) { name += "_0"; }
            try { Rating = (Bitmap)Properties.Resources.ResourceManager.GetObject(name); }
            catch (Exception) { Rating = Properties.Resources.stars_0_5; }
        }
    }

    public class PlayListItem
    {
        public int FIndex { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class WebRadioItem
    {
        public int RID { get; set; } = 0;
        public int Category { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Logo { get; set; } = null;
        public string Url { get; set; } = "";
        public string UrlPrefix { get; set; } = "";
        public string FType { get; set; } = "";
        public int Fragmented { get; set; } = 0;
    }

    public class WebRadioCategoryItem
    {
        public int CRID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Logo { get; set; } = null;
        public List<WebRadioItem> Childs = new List<WebRadioItem>();
    }
}
