using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json;

namespace AnotherMusicPlayer
{
    class PlayLists
    {
        private MainWindow Parent;
        private bool isBuild = false;

        public PlayLists(MainWindow parent)
        {
            if (parent == null) { return; }
            Parent = parent;
            Init();
            isBuild = true;
            Parent.PlaylistsContents.MouseDoubleClick += PlaylistsContents_MouseDoubleClick;
        }

        private void PlaylistsContents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Parent.PlaylistsContents.SelectedIndex != -1)
            {
                Parent.player.PlaylistClear();
                Parent.player.PlaylistEnqueue(new string[] { ((MediaItem)Parent.PlaylistsContents.SelectedItem).Path });
            }
        }

        private List<string> autoList = new List<string>() { "lastImports", "mostPlayed", "mostRecentlyPlayed", "bestRating" };
        public void Init()
        {
            int capacity = 150;
            List<TreeViewItem> ListItems = new List<TreeViewItem>(capacity);
            for (int i = 0; i < capacity; i++) { ListItems.Add(null); }

            //TreeViewItem[] col = new TreeViewItem[150] {
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            //    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
            //};
            TreeViewItem[] col = ListItems.ToArray();
            Parent.PlaylistsTree.Items.CopyTo(col, 0);
            int cpt = 0;
            foreach (TreeViewItem item in col)
            {
                if (item == null) { break; }
                if (cpt > 0) { Parent.PlaylistsTree.Items.Remove(item); }
                cpt += 1;
            }

            col = ListItems.ToArray();
            ((TreeViewItem)Parent.PlaylistsTree.Items[0]).Items.CopyTo(col, 0);
            foreach (TreeViewItem item in col)
            {
                if (item == null) { break; }
                ((TreeViewItem)Parent.PlaylistsTree.Items[0]).Items.Remove(item);
            }

            foreach (string archetype in autoList)
            {
                TreeViewItem item = new TreeViewItem()
                {
                    Header = Parent.FindResource("PlayListsAuto_" + archetype) as string,
                    Style = Parent.FindResource("PlaylistsTreeStyleItem") as Style,
                    Tag = archetype
                };
                item.MouseLeftButtonUp += autolistClick;
                ((TreeViewItem)Parent.PlaylistsTree.Items[0]).Items.Add(item);
                cpt += 1;
            }

            Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery("SELECT FIndex,Name,Description FROM playlists ORDER BY Name", "FIndex");
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
            {
                TreeViewItem item = new TreeViewItem()
                {
                    Header = row.Value["Name"] as string,
                    ToolTip = row.Value["Description"] as string,
                    Style = Parent.FindResource("PlaylistsTreeStyleItem") as Style,
                    Tag = Convert.ToInt32(row.Value["FIndex"])
                };
                item.MouseLeftButtonUp += userlistClick;
                Parent.PlaylistsTree.Items.Add(item);
            }
        }

        private void autolistClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Debug.WriteLine("--> autolistClick");
                string archetype = (string)((TreeViewItem)sender).Tag;
                Debug.WriteLine("--> autolistClick, Archetype: " + archetype);
                int maxLimit = 50;
                string query = "";
                if (archetype == autoList[0]) { query = "SELECT * FROM files ORDER BY LastUpdate DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 0; }
                else if (archetype == autoList[1]) { query = "SELECT * FROM playCounts ORDER BY Cpt DESC, LastPlay DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 50; }
                else if (archetype == autoList[2]) { query = "SELECT * FROM playCounts ORDER BY LastPlay DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 50; }
                if (archetype == autoList[3]) { query = "SELECT * FROM files ORDER BY Rating DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 0; }

                Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery(query, "Path");
                Debug.WriteLine("--> QUERY");
                List<string> files = new List<string>();
                Dictionary<string, int> countList = new Dictionary<string, int>();
                foreach (string key in rez.Keys)
                {
                    files.Add(key);
                    if (archetype == "mostPlayed" || archetype == "mostRecentlyPlayed") { countList.Add(key, Convert.ToInt32(rez[key]["Cpt"] as string)); }
                }
                Debug.WriteLine("--> Display build");
                fillContentSpace(files.ToArray(), (countList.Count > 0) ? countList : null);
            }
            catch (System.NullReferenceException err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
            catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
        }

        private void userlistClick(object sender, MouseButtonEventArgs e)
        {
            int id = (int)((TreeViewItem)sender).Tag;
            Debug.WriteLine("--> Item_MouseLeftButtonUp, Id: " + id);
            string query = "SELECT PIndex,LOrder,Path FROM playlistsItems WHERE LIndex = " + id + " ORDER BY LOrder ASC";
            Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery(query, "LOrder");
            Parent.PlaylistsContentsC0.Width = 0;

            if (rez == null) { Parent.PlaylistsContents.ItemsSource = new ObservableCollection<MediaItem>(); return; }
            List<string> files = new List<string>();
            foreach (Dictionary<string, object> row in rez.Values) { files.Add(row["Path"] as string); }
            Debug.WriteLine("--> Display build");
            fillContentSpace(files.ToArray(), null);
        }

        private void fillContentSpace(string[] files, Dictionary<string, int> countList = null)
        {
            try
            {
                Parent.PlaylistsContents.ItemsSource = new ObservableCollection<MediaItem>();
                if (files.Length == 0) { return; }
                ObservableCollection<MediaItem> tmp = new ObservableCollection<MediaItem>();
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        Dictionary<string, object> data = Parent.bdd.DatabaseFileInfo(file, true);
                        if (data == null) { continue; }
                        Debug.WriteLine("--> Add in table");
                        tmp.Add(new MediaItem()
                        {
                            Path = data["Path"] as string,
                            Name = data["Name"] as string,
                            Album = data["Album"] as string,
                            Size = Convert.ToInt64(data["Size"] as string),
                            Duration = Convert.ToInt64(data["Duration"] as string),
                            //DurationS = MainWindow.displayTime(Convert.ToInt64(data["Duration"] as string)),
                            DurationS = "00:00",
                            Genres = data["Genres"] as string,
                            Performers = data["Performers"] as string,
                            Composers = data["Composers"] as string,
                            Copyright = data["Copyright"] as string,
                            AlbumArtists = data["AlbumArtists"] as string,
                            Lyrics = data["Lyrics"] as string,
                            Disc = Convert.ToUInt32(data["Disc"] as string),
                            DiscCount = Convert.ToUInt32(data["DiscCount"] as string),
                            Track = Convert.ToUInt32(data["Track"] as string),
                            TrackCount = Convert.ToUInt32(data["TrackCount"] as string),
                            Year = Convert.ToUInt32(data["Year"] as string),
                            Rating = Convert.ToDouble((data["Rating"] as string).Replace('.', ',')),
                            PlayCount = Convert.ToUInt32((countList == null) ? 0 : ((countList.ContainsKey(file)) ? countList[file] : 0))
                        });
                    }
                }
                if (tmp.Count > 0)
                {
                    Parent.PlaylistsContents.ItemsSource = tmp;
                    Parent.PlaylistsContents.ScrollIntoView(tmp[0]);
                }
            }
            catch (System.NullReferenceException err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
            catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
        }
    }
}
