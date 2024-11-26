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
}
