using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AnotherMusicPlayer
{
    public enum AutoPlaylistTypes { LastImports, MostPlayed, MostRecentlyPlayed, BestRating, StarValue }

    internal class PlayListsLineItem : ICloneable
    {
        public int PlayCount { get; set; } = 0;
        public int PlaylistId { get; set; } = 0;
        public int PlaylistItemId { get; set; } = 0;
        public int PlaylistOrder { get; set; } = 0;
        public string Path { get; set; }
        public string Name { get; set; }
        public string Artists { get; set; }
        public string Album { get; set; }
        public long DurationValue { get; set; } = 0;
        public string Duration { get { return App.displayTime(DurationValue); } }
        private float _RatingValue = 0;
        public float RatingValue { 
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
            _RatingValue = rating;
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

        public object Clone()
        {
            return new PlayListsLineItem(this.Path, this.Name, Artists, this.Album, this.DurationValue, this.RatingValue);
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

    public class SortableBindingList<T> : BindingList<T>
    {
        public bool isSorted = false;
        public PropertyDescriptor sortProperty = null;
        public ListSortDirection sortDirection = ListSortDirection.Ascending;
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<string> props = new List<string>() { "PlayCount", "Name", "Artists", "Album", "DurationValue", "RatingValue" };
            string propName = property.Name;
            if (property.Name == "Duration") { propName = "DurationValue"; }
            if (property.Name == "Rating") { propName = "RatingValue"; }

            if (props.Contains(propName))
            {
                props.Remove(propName);
                props.Insert(0, propName);
            }

            List<PlayListsLineItem> itemsList = (List<PlayListsLineItem>)this.Items;
            itemsList.Sort(new Comparison<PlayListsLineItem>(delegate (PlayListsLineItem x, PlayListsLineItem y)
            {
                foreach (string prop in props)
                {
                    int cmp = -1;
                    if (prop == "PlayCount") { cmp = x.PlayCount.CompareTo(y.PlayCount); }
                    if (prop == "Name") { cmp = x.Name.CompareTo(y.Name); }
                    if (prop == "Artists") { cmp = x.Artists.CompareTo(y.Artists); }
                    if (prop == "Album") { cmp = x.Album.CompareTo(y.Album); }
                    if (prop == "DurationValue") { cmp = x.DurationValue.CompareTo(y.DurationValue); }
                    if (prop == "RatingValue") { cmp = x.RatingValue.CompareTo(y.RatingValue); }
                    if (cmp != 0) { return cmp * (direction == ListSortDirection.Descending ? -1 : 1); }
                }
                return 0;
            }));

            isSorted = true;
            sortProperty = property;
            sortDirection = direction;
        }
    }
}
