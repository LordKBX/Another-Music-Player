using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
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
            Dictionary<string, Dictionary<string, object>> genres = Bdd.DatabaseQuery("SELECT Genres, COUNT(Path) AS nb FROM files GROUP BY Genres ORDER BY TRIM(Genres) ASC", "Genres");
            if (FilterGenreSelector.Items.Count > 0)
                FilterGenreSelector.Items.Clear();
            Debug.WriteLine(JsonConvert.SerializeObject(genres.Keys));
            FilterGenreSelector.Items.Add(new ComboBoxItem()
            {
                Content = "------",
                Tag = null,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            });

            foreach (string key in genres.Keys)
            {
                if (key == "0") { continue; }// Filter for Genre "" return to mutch results makinng app interface freeze
                FilterGenreSelector.Items.Add(new ComboBoxItem()
                {
                    Content = ((key == "0") ? "<N/A>" : key) + " (" + genres[key]["nb"] + ")",
                    Tag = (key == "0") ? "" : key,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
            }
            FilterGenreSelector.SelectedIndex = 0;
        }

        /// <summary> Callback for index change on search type selector </summary>
        private void FilterSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tag = (string)((ComboBoxItem)FilterSelector.SelectedItem).Tag;
            //Debug.WriteLine(tag);
            if (tag == "")
            {
                FiltersSearchInput.Visibility = Visibility.Collapsed;
                FiltersSearchInput.Text = "";

                FilterGenreSelector.SelectedIndex = 0;
                FilterGenreSelector.Visibility = Visibility.Collapsed;

            }
            else if (tag == "Name" || tag == "Artist" || tag == "Album")
            {
                FilterGenreSelector.Visibility = Visibility.Collapsed;
                FilterGenreSelector.SelectedIndex = 0;

                FiltersSearchInput.Visibility = Visibility.Visible;
                FiltersSearchInput.Text = "";
            }
            else if (tag == "Genre")
            {
                FilterGenreSelector.Visibility = Visibility.Visible;
                FiltersSearchInput.Visibility = Visibility.Collapsed;
                FilterGenreSelector.SelectedIndex = 0;
            }
        }

        /// <summary> Callback for index change on search media genre selector </summary>
        private void FilterGenreSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string mode = (string)((ComboBoxItem)FilterSelector.SelectedItem).Tag;
            string genre = null;
            try { genre = (string)((ComboBoxItem)FilterGenreSelector.SelectedItem).Tag; }
            catch { }
            if (mode != "Genre") { return; }
            if (genre == null) { DisplayPath(RootPath); }
            else
            {
                e.Handled = true;
                Dictionary<string, Dictionary<string, object>> files = Bdd.DatabaseQuery("SELECT Path FROM files WHERE Genres LIKE '%" + genre + "%' ORDER BY Album, Disc, Track, Name, Path ASC", "Path");
                List<string> paths = new List<string>();
                foreach (KeyValuePair<string, Dictionary<string, object>> file in files) { paths.Add(file.Key); }

                pathNavigator.DisplayAlt("Search Genre: " + genre);
                Debug.WriteLine(JsonConvert.SerializeObject(paths));

                SearchResultsContener.ItemsSource = null;
                NavigationContenerScollerBorder.Visibility = Visibility.Collapsed;
                Parent.LibibrarySearchContentGridRow2.Height = new GridLength(0);
                SearchResultsContenerBorder.Visibility = Visibility.Visible;

                Parent.setLoadingState(true);
                Task.Delay(250);

                searchResults = GetTabInfoFromFiles(paths.ToArray());
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
                Parent.setLoadingState(false);
            }
        }

        /// <summary> Callback key down on search input for text search </summary>
        private void FiltersSearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine(e.Key.ToString());
            if (e.Key.ToString() == "Return")
            {
                SearchResultsContener.ItemsSource = null;
                NavigationContenerScollerBorder.Visibility = Visibility.Collapsed;
                Parent.LibibrarySearchContentGridRow2.Height = new GridLength(0);
                SearchResultsContenerBorder.Visibility = Visibility.Visible;

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
                Parent.setLoadingState(false);
            }

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