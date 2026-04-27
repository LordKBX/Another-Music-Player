using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Shapes;

namespace AnotherMusicPlayer
{
    public enum AutoPlaylistTypes { LastImports, MostPlayed, MostRecentlyPlayed, BestRating, StarValue }

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
            List<string> props = new List<string>() { "Name", "Artists", "Album", "RatingValue" };
            if (property.PropertyType == typeof(Bitmap))
            {
                try
                {
                    List<PlayListsLineItem> itemsList = (List<PlayListsLineItem>)this.Items;
                    itemsList.Sort(new Comparison<PlayListsLineItem>(delegate (PlayListsLineItem x, PlayListsLineItem y)
                    {
                        // Compare x to y if x is not null. If x is, but y isn't, we compare y
                        // to x and reverse the result. If both are null, they're equal.
                        if (x.RatingValue > y.RatingValue) { return -1 * (direction == ListSortDirection.Descending ? -1 : 1); }
                        else if (x.RatingValue < y.RatingValue) { return 1 * (direction == ListSortDirection.Descending ? -1 : 1); }
                        else {
                            int cmp = x.Name.CompareTo(y.Name);
                            if (cmp == 0) {
                                cmp = x.Artists.CompareTo(y.Artists);
                                if (cmp == 0)
                                {
                                    return x.Album.CompareTo(y.Album) * (direction == ListSortDirection.Descending ? -1 : 1);
                                }
                                else return cmp * (direction == ListSortDirection.Descending ? -1 : 1);
                            }
                            else return cmp * (direction == ListSortDirection.Descending ? -1 : 1);
                        }
                    }));
                }
                catch(Exception ex) { MessageBox.Show("Error while sorting by rating: " + ex.Message); return; }
            }
            else
            {
                if (props.Contains(property.Name))
                {
                    props.Remove(property.Name);
                    props.Insert(0, property.Name);

                    List<PlayListsLineItem> itemsList = (List<PlayListsLineItem>)this.Items;
                    itemsList.Sort(new Comparison<PlayListsLineItem>(delegate (PlayListsLineItem x, PlayListsLineItem y)
                    {
                        foreach (string prop in props)
                        {
                            int cmp = -1;
                            if (prop == "Name") { cmp = x.Name.CompareTo(y.Name); }
                            if (prop == "Artists") { cmp = x.Artists.CompareTo(y.Artists); }
                            if (prop == "Album") { cmp = x.Album.CompareTo(y.Album); }
                            if (prop == "RatingValue") { cmp = x.RatingValue.CompareTo(y.RatingValue); }
                            if (cmp != 0) { return cmp * (direction == ListSortDirection.Descending ? -1 : 1); }
                        }
                        return 0;
                    }));
                }
                else
                {
                    List<T> itemsList = (List<T>)this.Items;
                    if (property.PropertyType.GetInterface("IComparable") != null)
                    {
                        itemsList.Sort(new Comparison<T>(delegate (T x, T y)
                        {
                            // Compare x to y if x is not null. If x is, but y isn't, we compare y
                            // to x and reverse the result. If both are null, they're equal.
                            if (property.GetValue(x) != null)
                                return ((IComparable)property.GetValue(x)).CompareTo(property.GetValue(y)) * (direction == ListSortDirection.Descending ? -1 : 1);
                            else if (property.GetValue(y) != null)
                                return ((IComparable)property.GetValue(y)).CompareTo(property.GetValue(x)) * (direction == ListSortDirection.Descending ? 1 : -1);
                            else
                                return 0;
                        }));
                    }
                }
            }

            isSorted = true;
            sortProperty = property;
            sortDirection = direction;
        }
    }
}
