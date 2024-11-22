using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Drawing;
using System.Linq;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        private Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> searchResults = null;

        /// <summary> Load media list of genres in the correspondant ComboBox </summary>
        public void LoadGenreList()
        {
            //Bdd.DatabaseQuerys(new string[] { "UPDATE files SET Genres = REPLACE(REPLACE(REPLACE(REPLACE(Genres, ',', ';'), ' ,', ';'), ', ', ';'), '; ';')" }, true);
            Dictionary<string, Dictionary<string, object>> genresD = Bdd.DatabaseQuery("SELECT Genres FROM files GROUP BY Genres ORDER BY TRIM(Genres) ASC", "Genres");
            if (FilterGenreSelector.Items.Count > 0)
                FilterGenreSelector.Items.Clear();
            Debug.WriteLine(JsonConvert.SerializeObject(genresD.Keys));
            FilterGenreSelector.Items.Add(new DropDownItem() { Value = "------", Data = null });
            List<string> genres = new List<string>();
            foreach (string key in genresD.Keys)
            {
                string[] proc = key.Trim().ToLower().Replace(',', ';').Split(';');
                foreach (string t in proc)
                {
                    string tt = t.Trim(' ');
                    if (tt.Length > 0)
                    {
                        if (!genres.Contains(tt)) { genres.Add(tt); }
                    }
                }
            }
            genres.Sort();

            foreach (string key in genres)
            {
                if (key == "0") { continue; }// Filter for Genre "" return to mutch results makinng app interface freeze
                Dictionary<string, Dictionary<string, object>> Counts = Bdd.DatabaseQuery("SELECT COUNT(Path) AS nb FROM files WHERE LOWER(Genres) LIKE '%;" + Database.EscapeString(key) + "' OR LOWER(Genres) LIKE '" + Database.EscapeString(key) + ";%' OR LOWER(Genres) LIKE '%;" + Database.EscapeString(key) + ";%' OR LOWER(Genres) = '" + Database.EscapeString(key) + "'", "nb");
                int cpt = 0;
                foreach (string k in Counts.Keys) { cpt = Convert.ToInt32(Counts[k]["nb"]); break; }

                FilterGenreSelector.Items.Add(new DropDownItem() { 
                    Value = ((key == "0") ? "<N/A>" : key.Substring(0, 1).ToUpper() + key.Substring(1)) + " (" + cpt + ")", 
                    Data = (key == "0") ? "" : key
                });
            }
            FilterGenreSelector.SelectedIndex = 0;
        }

        /// <summary> Callback for index change on search type selector </summary>
        private void FilterSelector_SelectionChanged(object sender, EventArgs e)
        {
            string tag = "";
            if (FilterSelector.SelectedItem.GetType() == typeof(string)) { tag = (string)FilterSelector.SelectedItem; }
            else if (FilterSelector.SelectedItem.GetType() == typeof(DropDownItem)) { tag = (string)((DropDownItem)FilterSelector.SelectedItem).Data; }
            //Debug.WriteLine(tag);
            if (tag == "")
            {
                FiltersSearchInput.Visible = false;
                FiltersSearchInput.Text = "";

                FiltersGenreInput.Visible = false;
                FiltersGenreInput.Text = "";

                FilterGenreSelector.SelectedIndex = 0;
                FilterGenreSelector.Visible = false;

            }
            else if (tag == "Name" || tag == "Artist" || tag == "Album")
            {
                FilterGenreSelector.Visible = false;
                FilterGenreSelector.SelectedIndex = 0;

                FiltersSearchInput.Visible = true;
                FiltersSearchInput.Text = "";
            }
            else if (tag == "Genre")
            {
                FilterGenreSelector.Visible = true;
                FiltersGenreInput.Visible = true;
                FiltersSearchInput.Visible = false;
                FilterGenreSelector.SelectedIndex = 0;
            }
        }

        /// <summary> Callback for index change on search media genre selector </summary>
        private void FilterGenreSelector_SelectionChanged(object sender, EventArgs e)
        {
            string mode = "";
            string genre = "";
            if (FilterSelector.SelectedItem.GetType() == typeof(string)) { mode = (string)FilterSelector.SelectedItem; }
            else if (FilterSelector.SelectedItem.GetType() == typeof(DropDownItem)) { mode = (string)((DropDownItem)FilterSelector.SelectedItem).Data; }
            if (FilterGenreSelector.SelectedItem.GetType() == typeof(string)) { genre = (string)FilterGenreSelector.SelectedItem; }
            else if (FilterGenreSelector.SelectedItem.GetType() == typeof(DropDownItem)) { genre = (string)((DropDownItem)FilterGenreSelector.SelectedItem).Data; }
            
            if (mode != "Genre") { return; }
            if (genre == null) { DisplayPath(RootPath); }
            else
            {
                Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT Path FROM files WHERE LOWER(Genres) LIKE '%;" + genre + "' OR LOWER(Genres) LIKE '" + genre + ";%' OR LOWER(Genres) LIKE '%;" + genre + ";%' OR LOWER(Genres) LIKE '" + genre + "' ORDER BY Album, Disc, Track, Name, Path ASC", "Path");
                List<string> paths = new List<string>();
                foreach (KeyValuePair<string, Dictionary<string, object>> file in files) { paths.Add(file.Key); }

                pathNavigator.DisplayAlt("Search Genre: " + genre);
                Debug.WriteLine(JsonConvert.SerializeObject(paths));

                Parent.LibraryTabSplitContainer.Panel1Collapsed = true;
                Parent.LibraryTabSplitContainer.Panel2Collapsed = false;

                Parent.setLoadingState(true);
                Task.Delay(250);

                searchResults = GetTabInfoFromFiles(paths.ToArray());
                if (searchResults != null)
                {
                    ObservableCollection<MediaItem> list = new ObservableCollection<MediaItem>();
                    foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> album in searchResults)
                    {
                        foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> disk in album.Value)
                        {
                            foreach (KeyValuePair<string, MediaItem> track in disk.Value)
                            {
                                list.Add(track.Value);
                            }
                        }
                    }
                    //SearchResultsContener.ItemsSource = list;
                    SearchResultsContener.AutoScrollOffset = new System.Drawing.Point(0, 0);
                }
                Parent.setLoadingState(false);
            }
        }

        /// <summary> Callback key down on search input for text search </summary>
        private void FiltersSearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            //Debug.WriteLine(e.Key.ToString());
            if (e.Key.ToString() == "Return")
            {
                Parent.LibibrarySearchContentGridRow2.Height = new GridLength(0);

                string tag = (string)((ComboBoxItem)FilterSelector.SelectedItem).Tag;
                string var = FiltersSearchInput.Text.ToLower();
                Dictionary<string, Dictionary<string, object>> files = null;
                if (tag == "Artist")
                {
                    files = Bdd.DatabaseQuery("SELECT * FROM files WHERE LOWER(Performers) LIKE '%" + Database.EscapeString(var) + "%' OR LOWER(Composers) LIKE '%" + Database.EscapeString(var) + "%' ORDER BY Album, Disc, Track, Name, Path ASC", "Path");
                }
                else { files = Bdd.DatabaseQuery("SELECT * FROM files WHERE LOWER(" + tag + ") LIKE '%" + Database.EscapeString(var) + "%' ORDER BY Album, Disc, Track, Name, Path ASC", "Path"); }
                List<string> paths = new List<string>();
                foreach (KeyValuePair<string, Dictionary<string, object>> file in files) { paths.Add(file.Key); }

                Parent.setLoadingState(true);
                Task.Delay(250);

                searchResults = GetTabInfoFromFiles(paths.ToArray());
                if (searchResults != null)
                {
                    ObservableCollection<MediaItem> list = new ObservableCollection<MediaItem>();
                    foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> album in searchResults)
                    {
                        foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> disk in album.Value)
                        {
                            foreach (KeyValuePair<string, MediaItem> track in disk.Value)
                            {
                                list.Add(track.Value);
                            }
                        }
                    }
                    SearchResultsContener.ItemsSource = list;
                    SearchResultsContener.ScrollIntoView(list[0]);
                }
                Parent.setLoadingState(false);
            }
            */
        }

        private Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> getSearchSlice(uint start, uint end)
        {
            if (searchResults == null) { return null; }
            Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>> slicetab = new Dictionary<string, Dictionary<uint, Dictionary<string, MediaItem>>>();
            int count = 0;
            foreach (KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> pair in searchResults)
            {
                if (count >= start && count <= end) { slicetab.Add(pair.Key, pair.Value); }
                count += 1;
            }

            return slicetab;
        }

    }
}