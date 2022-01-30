using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        private ContextMenu MakeContextMenu(object parent, string otype = null, bool back = false)
        {
            if (otype == null || otype == "") { otype = "track"; }
            string type = otype = otype.ToLower();
            if (type == "file") { type = "track"; }
            ContextMenu cm = new LibraryContextMenu() { Style = Parent.FindResource("CustomContextMenuStyle") as Style };
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (((MenuItem)cm.Items[i]).Name == "BackFolder" && back == true)
                {
                    //((MenuItem)cm.Items[i]).Click += LibraryContextMenuAction_Back;
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "add" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_AddTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_AddAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_AddFolder; }
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "addshuffle" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_AddShuffledAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_AddShuffledFolder; ; }
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "play" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_PlayTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_PlayAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_PlayFolder; }

                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "playshuffle" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_PlayShuffledAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_PlayShuffledFolder; }

                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "edit" + otype)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "file") { ((MenuItem)cm.Items[i]).Click += CM_EditTrack; ; }
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_EditTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_EditAlbum; ; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_EditFolder; ; }

                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "playlistsadd" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_AddPlaylistTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_AddPlaylistAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_AddPlaylistFolder; }

                }
                else { ((MenuItem)cm.Items[i]).Visibility = Visibility.Collapsed; }
            }
            return cm;
        }

        private bool RecordTracksIntoPlaylist(string[] tracks)
        {
            if (tracks.Length < 1) { return false; }
            List<string> clearTracks = new List<string>();
            foreach (string track in tracks)
            {
                string trackTrimed = track.Trim();
                if (trackTrimed != "") { if (System.IO.File.Exists(trackTrimed)) { clearTracks.Add(trackTrimed); } }
            }
            if (clearTracks.Count < 1) { return false; }
            Window window = new Window()
            {
                Width = 300,
                Height = 300,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.ToolWindow,
                Owner = Parent,
                Style = Parent.FindResource("CustomWindowStyle") as Style,
                Title = Parent.FindResource("LibraryWindowAddIntoPlaylistTitle") as string,
                Tag = null
            };
            Grid gr1 = new Grid();
            StackPanel st1 = new StackPanel() { Orientation = Orientation.Vertical };
            ListView lw = new ListView() { MaxHeight = 150 }; st1.Children.Add(lw);
            StackPanel st2 = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Stretch, Width = 290.0 };
            Button btnOk = new Button() { Content = new AccessText() { Text = "OK" }, Style = Parent.FindResource("DefaultBtnStyle") as Style, Width = 135.0 };
            st2.Children.Add(btnOk);
            Button btnCancel = new Button() { Content = new AccessText() { Text = "Cancel" }, Style = Parent.FindResource("DefaultBtnStyle") as Style, Width = 135.0 };
            st2.Children.Add(btnCancel); st1.Children.Add(st2); gr1.Children.Add(st1);

            // SUB WINDOW FOR INPUTING NEW LIST NAME
            Grid gr2 = new Grid() { Visibility = Visibility.Collapsed };
            Grid gr3 = new Grid() { Background = Parent.FindResource("Library.AddIntoPlaylist.OverNew.Background") as SolidColorBrush };
            Border br1 = new Border()
            {
                Opacity = 1.0,
                MinWidth = 200,
                Background = Parent.FindResource("Library.AddIntoPlaylist.OverNew.Block.Background") as SolidColorBrush,
                BorderBrush = Parent.FindResource("Library.AddIntoPlaylist.OverNew.Block.Border") as SolidColorBrush,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            StackPanel stx1 = new StackPanel() { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Stretch };
            TextBlock tx = new TextBlock() { Text = "Nouvelle liste", HorizontalAlignment = HorizontalAlignment.Stretch };
            stx1.Children.Add(tx);
            TextBox input = new TextBox()
            {
                Text = "ex: liste",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Style = Parent.FindResource("InputStyle") as Style
            };
            stx1.Children.Add(input);

            StackPanel stx2 = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Stretch, Width = 200.0 };
            Button btnxOk = new Button() { Content = new AccessText() { Text = "Ok" }, Style = Parent.FindResource("DefaultBtnStyle") as Style, Width = 90.0 };
            stx2.Children.Add(btnxOk);
            Button btnxCancel = new Button() { Content = new AccessText() { Text = "Cancel" }, Style = Parent.FindResource("DefaultBtnStyle") as Style, Width = 90.0 };
            stx2.Children.Add(btnxCancel); stx1.Children.Add(stx2); br1.Child = stx1;
            gr3.Children.Add(br1); gr2.Children.Add(gr3); gr1.Children.Add(gr2);

            // FILLING WINDOW
            window.Content = gr1;
            Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery("SELECT FIndex,Name,Description, MAX(LOrder) AS Lorder FROM playlists LEFT JOIN playlistsItems ON(LIndex = FIndex) GROUP BY Name ORDER BY Name ASC", "FIndex");
            int maxIndexList = 0;
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
            {
                int index = Convert.ToInt32(row.Value["FIndex"] as string);
                if (index > maxIndexList) { maxIndexList = index; }
                lw.Items.Add(new ListViewItem() { Content = new AccessText() { Text = row.Value["Name"] as string }, Tag = row.Value["FIndex"] as string });
            }

            StackPanel st3 = new StackPanel() { Orientation = Orientation.Horizontal };
            st3.Children.Add(new MaterialDesignThemes.Wpf.PackIcon()
            {
                Width = 24,
                Height = 24,
                Kind = MaterialDesignThemes.Wpf.PackIconKind.PlusThick,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Parent.FindResource("TabControl.ForegroundColor") as SolidColorBrush
            });
            st3.Children.Add(new AccessText() { Text = Parent.FindResource("LibraryWindowAddIntoPlaylistNewElement") as string });
            lw.Items.Add(new ListViewItem() { Content = st3, Tag = "!", HorizontalContentAlignment = HorizontalAlignment.Stretch });

            window.Left = Parent.Left + ((Parent.Width - window.Width) / 2);
            window.Top = Parent.Top + ((Parent.Height - window.Height) / 2);


            btnOk.Click += (object sender, RoutedEventArgs e) =>
            {
                if (lw.SelectedItem == null) { return; }
                window.Tag = ((ListViewItem)lw.SelectedItem).Tag;
                if (window.Tag as string == "!") { gr2.Visibility = Visibility.Visible; }
                else { window.Close(); }
            };
            btnCancel.Click += (object sender, RoutedEventArgs e) => { window.Close(); };

            btnxCancel.Click += (object sender, RoutedEventArgs e) => { gr2.Visibility = Visibility.Collapsed; };
            btnxOk.Click += (object sender, RoutedEventArgs e) =>
            {
                if (input.Text.Length < 3) { MessageBox.Show("Please give a name with a minimum 3 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
                window.Close();
            };

            window.ShowDialog();

            int listId = 0;
            if (window.Tag != null)
            {
                string id = (string)window.Tag;
                if (id == "!")
                {
                    string name = input.Text;
                    listId = maxIndexList + 1;
                    Parent.bdd.DatabaseQuerys(new string[] { "INSERT INTO playlists(FIndex,Name,Description) VALUES('" + listId + "', '" + Database.EscapeString(name) + "', NULL)" }, true);
                }
                else { listId = Convert.ToInt32(id); }
            }
            if (listId == 0) { return false; }

            rez = Parent.bdd.DatabaseQuery("SELECT LOrder FROM playlistsItems WHERE LIndex = '" + listId + "' ORDER BY LOrder DESC LIMIT 1", "LOrder");

            int startOrder = 0;
            if (rez != null) { foreach (string key in rez.Keys) { startOrder = Convert.ToInt32(key); break; } }

            string query = "INSERT INTO playlistsItems(LIndex, Path, LOrder) VALUES";
            List<string> querys = new List<string>();
            int offset = 1;
            foreach (string track in clearTracks) { querys.Add(query + "('" + listId + "', '" + Database.EscapeString(track) + "', '" + (startOrder + offset) + "')"); offset += 1; }
            Parent.bdd.DatabaseQuerys(querys.ToArray(), true);

            return true;
        }

        private void CM_AddPlaylistTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string track = (string)((Button)item.Tag).Tag;
            RecordTracksIntoPlaylist(new string[] { track });
        }

        private void CM_AddPlaylistAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            RecordTracksIntoPlaylist(tracks);
        }

        private void CM_AddPlaylistFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string folder = null;
            try { folder = ((LibraryFolderButton)item.Tag).Path; }
            catch { }
            if (folder == null) { return; }
            string[] tracks = getDirectoryMediaFIles(folder, true);
            RecordTracksIntoPlaylist(tracks);
        }

        #region ContextMenu Add Playlist functions
        private void CM_AddTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string track = (string)((Button)item.Tag).Tag;
            Parent.player.PlaylistEnqueue(new string[] { track });
        }

        private void CM_AddAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistEnqueue(tracks);
        }

        private void CM_AddFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string folder = null;
            try { folder = ((LibraryFolderButton)item.Tag).Path; }
            catch { }
            if (folder == null) { return; }
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true));
        }
        #endregion

        #region ContextMenu Add Shuffle Playlist functions
        private void CM_AddShuffledAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistEnqueue(tracks, true);
        }

        private void CM_AddShuffledFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string folder = null;
            try { folder = ((LibraryFolderButton)item.Tag).Path; }
            catch
            {
                try { }
                catch { }
            }
            if (folder == null) { return; }
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), true);
        }
        #endregion

        #region ContextMenu Play functions
        private void CM_PlayTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string track = (string)((Button)item.Tag).Tag;
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(new string[] { track });
        }

        private void CM_PlayAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(tracks);
        }

        private void CM_PlayFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string folder = null;
            try { folder = ((LibraryFolderButton)item.Tag).Path; }
            catch
            {
                try { }
                catch { }
            }
            if (folder == null) { return; }
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true));
        }
        #endregion

        #region ContextMenu Play Shuffled functions
        private void CM_PlayShuffledAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(tracks, true);
        }

        private void CM_PlayShuffledFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string folder = null;
            try { folder = ((LibraryFolderButton)item.Tag).Path; }
            catch
            {
                try { }
                catch { }
            }
            if (folder == null) { return; }
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), true);
        }
        #endregion

        #region ContextMenu Edit functions
        private void CM_EditTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Button btn = (Button)item.Tag;
            string track = (string)btn.Tag;
            TagsEditor tags = new TagsEditor(Parent, "track", new string[] { track });
            e.Handled = true;
            bool? ret = tags.ShowDialog2(Parent);

            if (tags.Saved)
            {
                if (tags.CoverChanged == true) { Bdd.DatabaseClearCover(track); }
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    Thread.Sleep(200);
                    DisplayPath(CurrentPath);
                }));
            }
            //throw new System.NotImplementedException();
        }

        private void CM_EditAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Button btn = (Button)item.Tag;
            string[] tracks = (string[])btn.Tag;
            TagsEditor tags = new TagsEditor(Parent, "album", tracks);
            e.Handled = true;
            bool? ret = tags.ShowDialog2(Parent);

            if (tags.Saved)
            {
                if (tags.CoverChanged == true)
                {
                    foreach (string track in tracks) Bdd.DatabaseClearCover(track);
                }
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    Thread.Sleep(500);
                    DisplayPath(CurrentPath);
                }));
            }
            //throw new System.NotImplementedException();
        }

        private void CM_EditFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Button btn = (Button)item.Tag;
            string folderPath = ((string)btn.Tag).Trim().TrimEnd(MainWindow.SeparatorChar);
            Debug.WriteLine("--> CM_EditFolder, folderPath='" + folderPath + "'");
            string[] pathTab = folderPath.Split(MainWindow.SeparatorChar);
            Debug.WriteLine("--> CM_EditFolder, pathTab.Length='" + pathTab.Length + "'");

            Window win = new Window();
            win.Owner = Parent;
            win.Style = Parent.FindResource("CustomWindowStyle") as Style;
            win.Resources.MergedDictionaries.Add(Parent.Resources.MergedDictionaries[0]);
            win.Title = Parent.FindResource("RenemaWindowTitle") as string;
            win.WindowStyle = WindowStyle.ToolWindow;
            win.Width = win.MinWidth = win.MaxWidth = 300;
            win.Height = win.MinHeight = win.MaxHeight = 200;
            StackPanel st = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Style = win.FindResource("CustomWindowBackgroundStyleAlt") as Style
            };
            st.Children.Add(new TextBlock()
            {
                Text = Parent.FindResource("RenemaWindowLabel") as string,
                Style = win.FindResource("EditorInputLabel") as Style,
                Margin = new Thickness(5, 5, 5, 3)
            });

            TextBox input = new TextBox()
            {
                Text = pathTab[pathTab.Length - 1],
                Style = win.FindResource("InputStyle") as Style,
                Margin = new Thickness(5, 0, 5, 3)
            };
            st.Children.Add(input);

            Button saveBtn = new Button()
            {
                Content = Parent.FindResource("EditorTagSave") as string,
                Style = win.FindResource("EditorButton1") as Style,
                IsEnabled = false,
                Margin = new Thickness(5, 0, 5, 3)
            };
            saveBtn.Click += (object sender, RoutedEventArgs e) =>
            {
                Regex rx = new Regex("(<|>|:|\"|/|\\|?|*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if (input.Text.Trim() == "" || input.Text.Contains("|", StringComparison.Ordinal) || rx.IsMatch(input.Text))
                {
                    MessageBox.Show("Folder name invalid,\nplease remove the folowing characters:\n < > : \" / \\ | ? *", "Error !", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                List<string> list = new List<string>(pathTab);
                list.Remove(pathTab[pathTab.Length - 1]);

                Directory.Move(folderPath, string.Join(MainWindow.SeparatorChar, list.ToArray()) + MainWindow.SeparatorChar + input.Text.Trim());
                win.Close();
            };
            input.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                saveBtn.IsEnabled = true;
            };

            st.Children.Add(saveBtn);

            win.Content = st;
            win.ShowInTaskbar = false;
            win.ShowDialog();

            //throw new System.NotImplementedException();
        }
        #endregion
    }

    /// <summary>
    /// Logique d'interaction pour LibraryContextMenu.xaml
    /// </summary>
    public partial class LibraryContextMenu : ContextMenu
    {
        public LibraryContextMenu()
        {
            InitializeComponent();
        }
    }
}
