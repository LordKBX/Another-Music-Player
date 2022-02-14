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
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop;
using NAudio.Wave;
using m3uParser;
using m3uParser.Model;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    public partial class PlayLists : IDropTarget
    {
        private MainWindow Parent;
        private bool isBuild = false;

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (Parent.PlaylistsTree.SelectedItem == null) { return; }
            TreeViewItem tvi = (TreeViewItem)Parent.PlaylistsTree.SelectedItem;
            if (!tvi.Name.StartsWith("user_")) { return; }
            string id = (string)tvi.Tag;

            if (Parent.PlaylistsContents.SelectedItems.Count == 0) { return; }
            ObservableCollection<MediaItem> list = (ObservableCollection<MediaItem>)Parent.PlaylistsContents.ItemsSource;
            ObservableCollection<MediaItem> listTmp = new ObservableCollection<MediaItem>();
            int newIndex = dropInfo.InsertIndex - 1;
            if (newIndex < 0) { newIndex = 0; }
            Debug.WriteLine("newIndex = " + newIndex);

            if (Parent.PlaylistsContents.SelectedItems.Count == 1)
            {
                MediaItem target = (MediaItem)dropInfo.DragInfo.SourceItem;
                Debug.WriteLine("target = " + JsonConvert.SerializeObject(target));

                foreach (MediaItem row in list)
                {
                    if (listTmp.Count == newIndex) { listTmp.Add(target); }
                    if (row.Path != target.Path) { listTmp.Add(row); }
                }
            }
            else if (Parent.PlaylistsContents.SelectedItems.Count > 1)
            {
                MediaItem[] selection = new MediaItem[Parent.PlaylistsContents.SelectedItems.Count];
                Parent.PlaylistsContents.SelectedItems.CopyTo(selection, 0);
                Parent.PlaylistsContents.SelectedIndex = -1;
                List<MediaItem> selectionList = new List<MediaItem>(selection);
                Debug.WriteLine("targets = " + JsonConvert.SerializeObject(selection));
                foreach (MediaItem row in list)
                {
                    if (listTmp.Count == newIndex)
                    {
                        foreach (MediaItem row2 in selection) listTmp.Add(row2);
                    }

                    if (!selectionList.Contains(row)) { listTmp.Add(row); }
                }
            }

            Parent.PlaylistsContents.ItemsSource = listTmp;

            List<string> querys = new List<string>();

            int offset = 1;
            foreach (MediaItem row in listTmp) { querys.Add("UPDATE playlistsItems SET LOrder = '" + offset + "' WHERE Path = '" + Database.EscapeString(row.Path) + "'"); offset += 1; }
            Parent.bdd.DatabaseQuerys(querys.ToArray(), true);
        }

        public PlayLists(MainWindow parent)
        {
            if (parent == null) { return; }
            Parent = parent;
            Init();
            isBuild = true;
            Parent.PlaylistsContents.MouseDoubleClick += PlaylistsContents_MouseDoubleClick;
            Parent.PlaylistsContents.ContextMenu = null;
            Parent.PlaylistsContents.SelectionChanged += PlaylistsContents_SelectionChanged;
            Parent.PlaylistsContents.KeyUp += PlaylistsContents_KeyUp;

            Parent.PlaylistsContents.Visibility = Visibility.Visible;
            Parent.PlaylistsContents2Border.Visibility = Visibility.Collapsed;

            Parent.PlaylistsContents2BtnPlay.Click += PlaylistsContents2BtnPlay_Click;
        }

        private void PlaylistsContents_KeyUp(object sender, KeyEventArgs e)
        {
            if (Parent.PlaylistsTree.SelectedItem == null) { Debug.WriteLine("NOPE 1"); return; }
            TreeViewItem tvi = (TreeViewItem)Parent.PlaylistsTree.SelectedItem;
            if (!tvi.Name.StartsWith("user_")) { Debug.WriteLine("NOPE 1"); return; }

            if (e.Key == Key.Delete) { CM_RemoveTrackSelection(null, null); }
        }

        private void PlaylistsContents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parent.PlaylistsContents.ContextMenu = null;
            if (Parent.PlaylistsContents.SelectedItems.Count == 0) { return; }
            else
            {
                TreeViewItem TItem = (Parent.PlaylistsTree.SelectedItem != null) ? (TreeViewItem)Parent.PlaylistsTree.SelectedItem : null;
                if (TItem == null) { return; }
                if (!TItem.Name.StartsWith("user_")) { return; }
                Parent.PlaylistsContents.ContextMenu = MakeContextMenu(Parent.PlaylistsContents, "content");
            }
        }

        private void PlaylistsContents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Parent.PlaylistsContents.SelectedIndex != -1)
            {
                Parent.player.PlaylistClear();
                Parent.player.PlaylistEnqueue(new string[] { ((MediaItem)Parent.PlaylistsContents.SelectedItem).Path }, false, 0, 0, true);
            }
        }

        private List<string> autoList = new List<string>() { "lastImports", "mostPlayed", "mostRecentlyPlayed", "bestRating" };
        public void Init()
        {
            ((TreeViewItem)Parent.PlaylistsTree.Items[0]).Items.Clear();
            ((TreeViewItem)Parent.PlaylistsTree.Items[1]).Items.Clear();
            ((TreeViewItem)Parent.PlaylistsTree.Items[2]).Items.Clear();
            double itemMaxWidth = 148;

            foreach (string archetype in autoList)
            {
                TreeViewItem item = new TreeViewItem()
                {
                    Header = new TextBlock() { Text = Parent.FindResource("PlayListsAuto_" + archetype) as string, TextTrimming = TextTrimming.CharacterEllipsis },
                    Tag = "auto_" + archetype,
                    Name = "auto_" + archetype,
                    MaxWidth = itemMaxWidth
                };
                item.MouseLeftButtonUp += autolistClick;
                item.ContextMenu = MakeContextMenu(item, "list");
                ((TreeViewItem)Parent.PlaylistsTree.Items[0]).Items.Add(item);
            }

            Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery("SELECT FIndex,Name,Description FROM playlists ORDER BY LOWER(Name) ASC", "FIndex");
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
            {
                TreeViewItem item = new TreeViewItem()
                {
                    Header = new TextBlock() { Text = row.Value["Name"] as string, TextTrimming = TextTrimming.CharacterEllipsis },
                    Tag = row.Value["FIndex"] as string,
                    Name = "user_" + row.Value["FIndex"] as string,
                    MaxWidth = itemMaxWidth
                };
                if (row.Value["Description"] != null && row.Value["Description"] as string != "") { item.ToolTip = row.Value["Description"] as string; }
                item.MouseLeftButtonUp += userlistClick;
                item.ContextMenu = MakeContextMenu(item, "listCustom");
                ((TreeViewItem)Parent.PlaylistsTree.Items[1]).Items.Add(item);
            }

            Dictionary<string, Dictionary<string, object>> rez2 = Parent.bdd.DatabaseQuery("SELECT * FROM radios ORDER BY LOWER(Name) ASC", "RID");
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez2)
            {
                TreeViewItem item = new TreeViewItem()
                {
                    Header = new TextBlock() { Text = row.Value["Name"] as string, TextTrimming = TextTrimming.CharacterEllipsis },
                    Tag = row.Value,
                    Name = "radio_" + row.Value["RID"] as string,
                    MaxWidth = itemMaxWidth
                };
                item.MouseLeftButtonUp += radioClick;
                item.MouseDoubleClick += radioDoubleClick;
                item.ContextMenu = MakeContextMenu(item, "radio");
                ((TreeViewItem)Parent.PlaylistsTree.Items[2]).Items.Add(item);
            }
        }

        private void radioDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadioPlayer.Stop();
            PlaylistsContents2BtnPlay_Click(null, null);
        }

        private Dictionary<string, object> CurentRadio = null;
        private async void radioClick(object sender, MouseButtonEventArgs e)
        {
            if (CurentRadio == (Dictionary<string, object>)((TreeViewItem)sender).Tag)
            {
                Parent.PlaylistsContents2Border.Visibility = Visibility.Visible;
                Parent.PlaylistsContents.Visibility = Visibility.Collapsed;
                return;
            }
            CurentRadio = (Dictionary<string, object>)((TreeViewItem)sender).Tag;
            //Debug.WriteLine(JsonConvert.SerializeObject(CurentRadio));

            Parent.PlaylistsContents2Border.Visibility = Visibility.Visible;
            Parent.PlaylistsContents.Visibility = Visibility.Collapsed;

            Debug.WriteLine("CurentRadio[\"FType\"] = " + CurentRadio["FType"]);
            Parent.PlaylistsContents2Label.Text = CurentRadio["Name"] as string;
            Parent.PlaylistsContents2Description.Text = CurentRadio["Description"] as string;
            System.Windows.Media.Imaging.BitmapImage bi = null;
            string logo = CurentRadio["Logo"] as string;
            string[] logoTab = logo.Split(',');
            if (logoTab.Length > 1) { logo = logoTab[1]; }
            try { bi = BitmapMagic.Base64StringToBitmap(logo); }
            catch (Exception err)
            {
                Debug.WriteLine("CurentRadio[\"Logo\"] = " + logo);
                Debug.WriteLine(JsonConvert.SerializeObject(err));
            }
            Parent.RadioCover.Source = (bi ?? MainWindow.Bimage("CoverImg"));
        }

        private async void PlaylistsContents2BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (CurentRadio["FType"] as string == "Stream")
            { Parent.player.OpenStream(CurentRadio["Url"] as string, RadioPlayer.RadioType.Stream, CurentRadio["RID"] as string, CurentRadio["Name"] as string, true, CurentRadio["UrlPrefix"] as string); }
            else if (CurentRadio["FType"] as string == "M3u")
            { Parent.player.OpenStream(CurentRadio["Url"] as string, RadioPlayer.RadioType.M3u, CurentRadio["RID"] as string, CurentRadio["Name"] as string, true, CurentRadio["UrlPrefix"] as string); }
        }

        private void autolistClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Parent.PlaylistsContents.Visibility = Visibility.Visible;
                Parent.PlaylistsContents2Border.Visibility = Visibility.Collapsed;
                Debug.WriteLine("--> autolistClick");
                string archetype = ((string)((TreeViewItem)sender).Tag).Replace("auto_", "");
                Debug.WriteLine("--> autolistClick, Archetype: " + archetype);

                if (archetype == autoList[0]) { Parent.PlaylistsContentsC0.Width = 0; }
                else if (archetype == autoList[1]) { Parent.PlaylistsContentsC0.Width = 50; }
                else if (archetype == autoList[2]) { Parent.PlaylistsContentsC0.Width = 50; }
                if (archetype == autoList[3]) { Parent.PlaylistsContentsC0.Width = 0; }

                try
                {
                    SizeChangedInfo sifo = new SizeChangedInfo(Parent.PlaylistsContents, new Size(0, 0), true, true);
                    System.Reflection.ConstructorInfo[] gd = typeof(System.Windows.SizeChangedEventArgs).GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    SizeChangedEventArgs ea = gd[0].Invoke(new object[] { (Parent.PlaylistsContents as FrameworkElement), sifo }) as SizeChangedEventArgs;
                    ea.RoutedEvent = ListView.SizeChangedEvent;
                    Parent.PlaylistsContents.RaiseEvent(ea);
                }
                catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }

                Dictionary<string, Dictionary<string, object>> rez = autolistData(archetype);
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
            Parent.PlaylistsContents.ContextMenu = null;

            GongSolutions.Wpf.DragDrop.DragDrop.SetIsDragSource(Parent.PlaylistsContents, false);
            GongSolutions.Wpf.DragDrop.DragDrop.SetIsDropTarget(Parent.PlaylistsContents, false);
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(Parent.PlaylistsContents, null);
        }

        private Dictionary<string, Dictionary<string, object>> autolistData(string name, int maxLimit = 50)
        {
            string query = "";
            if (name == autoList[0]) { query = "SELECT * FROM files ORDER BY LastUpdate DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 0; }
            else if (name == autoList[1]) { query = "SELECT * FROM playCounts ORDER BY Cpt DESC, LastPlay DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 50; }
            else if (name == autoList[2]) { query = "SELECT * FROM playCounts ORDER BY LastPlay DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 50; }
            if (name == autoList[3]) { query = "SELECT * FROM files ORDER BY Rating DESC LIMIT " + maxLimit; Parent.PlaylistsContentsC0.Width = 0; }

            return Parent.bdd.DatabaseQuery(query, "Path");
        }

        public void userlistClick(object sender, MouseButtonEventArgs e)
        {
            Parent.PlaylistsContents.Visibility = Visibility.Visible;
            Parent.PlaylistsContents2Border.Visibility = Visibility.Collapsed;
            int id = Convert.ToInt32((string)((TreeViewItem)sender).Tag);
            Debug.WriteLine("--> Item_MouseLeftButtonUp, Id: " + id);
            Parent.PlaylistsContentsC0.Width = 0;
            List<string> files = userlistData(id);

            if (files == null) { Parent.PlaylistsContents.ItemsSource = new ObservableCollection<MediaItem>(); return; }
            Debug.WriteLine("--> Display build");
            fillContentSpace(files.ToArray(), null);

            GongSolutions.Wpf.DragDrop.DragDrop.SetIsDragSource(Parent.PlaylistsContents, true);
            GongSolutions.Wpf.DragDrop.DragDrop.SetIsDropTarget(Parent.PlaylistsContents, true);
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(Parent.PlaylistsContents, this);

            Parent.PlaylistsContents.ContextMenu = null;
        }

        private List<string> userlistData(int id)
        {
            string query = "SELECT PIndex,LOrder,Path FROM playlistsItems WHERE LIndex = " + id + " ORDER BY LOrder ASC";
            Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery(query, "LOrder");

            if (rez == null) { return null; }
            List<string> files = new List<string>();
            foreach (Dictionary<string, object> row in rez.Values) { files.Add(row["Path"] as string); }
            return files;
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

        public bool RecordTracksIntoPlaylist(string[] tracks)
        {
            if (tracks.Length < 1) { return false; }
            List<string> clearTracks = new List<string>();
            foreach (string track in tracks)
            {
                string trackTrimed = track.Trim();
                if (trackTrimed != "") { if (System.IO.File.Exists(trackTrimed)) { clearTracks.Add(trackTrimed); } }
            }
            if (clearTracks.Count < 1) { return false; }
            InsertIntoPlaylistWindow window = new InsertIntoPlaylistWindow(Parent)
            {
                Owner = Parent,
                Title = Parent.FindResource("PlaylistsWindowAddIntoPlaylistTitle") as string,
                Tag = null
            };

            window.ShowDialog();

            int listId = 0;
            if (window.Tag != null)
            {
                string id = (string)window.Tag;
                if (id == "!")
                {
                    string name = window.input1.Text.Trim(); string desc = window.input2.Text.Trim();
                    if (desc == "") { desc = null; }
                    listId = window.maxIndexList + 1;
                    Parent.bdd.DatabaseQuerys(new string[] {
                        "INSERT INTO playlists(FIndex,Name,Description) VALUES('" + listId + "', '" + Database.EscapeString(name) + "', " + ((desc == null)?"NULL":"'"+desc+"'") + ")"
                    }, true);
                    Parent.playLists.Init();
                }
                else { listId = Convert.ToInt32(id); }
            }
            if (listId == 0) { return false; }

            int startOrder = 0;
            Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery("SELECT LOrder FROM playlistsItems WHERE LIndex = '" + listId + "' ORDER BY LOrder DESC LIMIT 1", "LOrder");
            if (rez != null) { foreach (string key in rez.Keys) { startOrder = Convert.ToInt32(key); break; } }

            string query = "INSERT INTO playlistsItems(LIndex, Path, LOrder) VALUES";
            List<string> querys = new List<string>(); int offset = 1;
            foreach (string track in clearTracks) { querys.Add(query + "('" + listId + "', '" + Database.EscapeString(track) + "', '" + (startOrder + offset) + "')"); offset += 1; }
            Parent.bdd.DatabaseQuerys(querys.ToArray(), true);

            if (Parent.PlaylistsTree.SelectedItem != null)
            { if ((string)((TreeViewItem)Parent.PlaylistsTree.SelectedItem).Tag == "" + listId) { Parent.playLists.userlistClick(Parent.PlaylistsTree.SelectedItem, null); } }
            return true;
        }
    }
}
