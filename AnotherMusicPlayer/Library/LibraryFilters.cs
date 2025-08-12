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
            if (Parent.LibraryFiltersGenreList.Items.Count > 0)
                Parent.LibraryFiltersGenreList.Items.Clear();
            Debug.WriteLine(JsonConvert.SerializeObject(genresD.Keys));
            Parent.LibraryFiltersGenreList.Items.Add(new DropDownItem() { Value = "------", Data = null });
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

                Parent.LibraryFiltersGenreList.Items.Add(new DropDownItem() { 
                    Value = ((key == "0") ? "<N/A>" : key.Substring(0, 1).ToUpper() + key.Substring(1)) + " (" + cpt + ")", 
                    Data = (key == "0") ? "" : key
                });
            }
            Parent.LibraryFiltersGenreList.SelectedIndex = 0;
        }

        /// <summary> Callback for index change on search type selector </summary>
        private void LibraryFiltersMode_SelectionChanged(object sender, EventArgs e)
        {
            string tag = "";
            if (Parent.LibraryFiltersMode.SelectedItem.GetType() == typeof(string)) { tag = (string)Parent.LibraryFiltersMode.SelectedItem; }
            else if (Parent.LibraryFiltersMode.SelectedItem.GetType() == typeof(DropDownItem)) { tag = (string)((DropDownItem)Parent.LibraryFiltersMode.SelectedItem).Data; }
            Debug.WriteLine("tag = " + tag);
            if (tag == "")
            {
                Parent.LibraryFiltersSearchBox.Visible = false;
                Parent.LibraryFiltersSearchBox.Text = "";

                Parent.LibraryFiltersGenreList.SelectedIndex = 0;
                Parent.LibraryFiltersGenreList.Visible = false;
                if (CurrentPath != RootPath) { DisplayPath(RootPath); }
            }
            else
            {
                Parent.LibraryTabSplitContainer.Panel1Collapsed = true;
                Parent.LibraryTabSplitContainer.Panel2Collapsed = false;
                Parent.LibraryTabSplitContainer2.Panel1Collapsed = false;
                Parent.LibraryTabSplitContainer2.Panel2Collapsed = true;

                if (tag == "Name" || tag == "Artist" || tag == "Album"
                    || tag == App.GetTranslation("LibraryFiltersMode_name") 
                    || tag == App.GetTranslation("LibraryFiltersMode_artist") 
                    || tag == App.GetTranslation("LibraryFiltersMode_album"))
                {
                    Parent.LibraryFiltersGenreList.Visible = false;

                    Parent.LibraryFiltersSearchBox.Visible = true;
                    Parent.LibraryFiltersSearchBox.Text = "";
                }
                else if (tag == "Genre")
                {
                    Parent.LibraryFiltersGenreList.Visible = true;
                    Parent.LibraryFiltersSearchBox.Visible = false;
                    Parent.LibraryFiltersGenreList.SelectedIndex = 0;
                }
            }
        }

        /// <summary> Callback for index change on search media genre selector </summary>
        private void LibraryFiltersGenreList_SelectionChanged(object sender, EventArgs e)
        {
            string mode = "";
            string genre = "";
            if (Parent.LibraryFiltersMode.SelectedItem.GetType() == typeof(string)) { mode = (string)Parent.LibraryFiltersMode.SelectedItem; }
            else if (Parent.LibraryFiltersMode.SelectedItem.GetType() == typeof(DropDownItem)) { mode = (string)((DropDownItem)Parent.LibraryFiltersMode.SelectedItem).Data; }
            if (Parent.LibraryFiltersGenreList.SelectedItem.GetType() == typeof(string)) { genre = (string)Parent.LibraryFiltersGenreList.SelectedItem; }
            else if (Parent.LibraryFiltersGenreList.SelectedItem.GetType() == typeof(DropDownItem)) { genre = (string)((DropDownItem)Parent.LibraryFiltersGenreList.SelectedItem).Data; }
            
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
                    Parent.LibrarySearchContent.DataSource = list;
                    Parent.LibraryNavigationPathContener.AutoScrollOffset = new System.Drawing.Point(0, 0);
                }
                Parent.setLoadingState(false);
            }
        }

        /// <summary> Callback key down on search input for text search </summary>
        private void LibraryFiltersSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine(e.Key.ToString());
            if (e.KeyData == Keys.Return)
            {
                string tag = "" + Parent.LibraryFiltersMode.SelectedItem;
                string var = Parent.LibraryFiltersSearchBox.Text.ToLower();
                Dictionary<string, Dictionary<string, object>> files = null;
                if (tag == App.GetTranslation("LibraryFiltersMode_name")) { tag = "Name"; }
                else if (tag == App.GetTranslation("LibraryFiltersMode_artist")) { tag = "Artist"; }
                else if (tag == App.GetTranslation("LibraryFiltersMode_album")) { tag = "Album"; }

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
                    Parent.LibrarySearchContent.DataSource = list;
                    Parent.LibraryNavigationPathContener.AutoScrollOffset = new System.Drawing.Point(0, 0);
                }
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