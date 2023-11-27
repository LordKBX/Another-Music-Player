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
        public ContextMenu MakeContextMenu(object parent, string otype = null, bool back = false, string backPath = "")
        {
            if (otype == null || otype == "") { otype = "track"; }
            string type = otype = otype.ToLower();
            if (type == "file") { type = "track"; }
            ContextMenu cm = new LibraryContextMenu() { Style = Parent.FindResource("CustomContextMenuStyle") as Style };
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (((MenuItem)cm.Items[i]).Name == "BackFolder" && back == true)
                {
                    ((MenuItem)cm.Items[i]).Click += CM_FolderBack;
                    ((MenuItem)cm.Items[i]).Tag = backPath;
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "add" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_AddTrack; }
                    if (type == "selection") { ((MenuItem)cm.Items[i]).Click += CM_AddTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_AddAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_AddFolder; }
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "addshuffle" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_AddShuffledAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_AddShuffledFolder; }
                    if (type == "selection") { ((MenuItem)cm.Items[i]).Click += CM_AddShuffledFolder; }
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "play" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_PlayTrack; }
                    if (type == "selection") { ((MenuItem)cm.Items[i]).Click += CM_PlayTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_PlayAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_PlayFolder; }

                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "playshuffle" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_PlayShuffledAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_PlayShuffledFolder; }
                    if (type == "selection") { ((MenuItem)cm.Items[i]).Click += CM_PlayShuffledFolder; }

                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "edit" + otype)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "file") { ((MenuItem)cm.Items[i]).Click += CM_EditTrack; }
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_EditTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_EditAlbum; }
                    if (type == "folder") {
                        if (MainWindow.Instance.library.CurrentPath == Settings.LibFolder && parent.GetType().Name == "AlignablePanel") 
                        { ((MenuItem)cm.Items[i]).Visibility = Visibility.Collapsed; }
                        else { ((MenuItem)cm.Items[i]).Click += CM_EditFolder; }
                    }
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "playlistsadd" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_AddPlaylistTrack; }
                    if (type == "selection") { ((MenuItem)cm.Items[i]).Click += CM_AddPlaylistTrack; }
                    if (type == "album") { ((MenuItem)cm.Items[i]).Click += CM_AddPlaylistAlbum; }
                    if (type == "folder") { ((MenuItem)cm.Items[i]).Click += CM_AddPlaylistFolder; }

                }
                else { ((MenuItem)cm.Items[i]).Visibility = Visibility.Collapsed; }
            }
            return cm;
        }

        private void CM_FolderBack(object sender, RoutedEventArgs e)
        {
            string tag = (string)((MenuItem)sender).Tag;
            DirectoryInfo di = new DirectoryInfo(tag);
            DisplayPath(di.Parent.FullName);
        }

        private void CM_AddPlaylistTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (item.Tag.GetType().Name == "ListView")
            {
                ListView view = (ListView)item.Tag;
                if (view.SelectedItems.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                    Parent.playLists.RecordTracksIntoPlaylist(files.ToArray());
                }
            }
            else
            {
                string track = (string)((Button)item.Tag).Tag;
                Parent.playLists.RecordTracksIntoPlaylist(new string[] { track });
            }
        }

        private void CM_AddPlaylistAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.playLists.RecordTracksIntoPlaylist(tracks);
        }

        private void CM_AddPlaylistFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;
                string folder = null;

                if (item.Tag == null) { return; }
                else if (item.Tag.GetType().Name == "Button") { folder = ((Button)item.Tag).Tag as string; }
                else if (item.Tag.GetType().Name == "AlignablePanel") { folder = ((AlignablePanel)item.Tag).Tag as string; }
                else if (item.Tag.GetType().Name == "TextBlock") { folder = ((TextBlock)item.Tag).Tag as string; }

                if (folder == null) { return; }
                string[] tracks = getDirectoryMediaFIles(folder, true);
                Parent.playLists.RecordTracksIntoPlaylist(tracks);
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); Debug.WriteLine(ex.StackTrace); }
        }

        #region ContextMenu Add Playlist functions
        private void CM_AddTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (item.Tag.GetType().Name == "ListView")
            {
                ListView view = (ListView)item.Tag;
                if (view.SelectedItems.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                    Parent.player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
                }
            }
            else
            {
                string track = (string)((Button)item.Tag).Tag;
                Parent.player.PlaylistEnqueue(new string[] { track }, false, 0, 0, true);
            }
        }

        private void CM_AddAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistEnqueue(tracks, false, 0, 0, true);
        }

        private void CM_AddFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string folder = null;
            if (item.Tag == null) { return; }
            else if (item.Tag.GetType().Name == "Button") { folder = ((Button)item.Tag).Tag as string; }
            else if (item.Tag.GetType().Name == "AlignablePanel") { folder = ((AlignablePanel)item.Tag).Tag as string; }
            else if (item.Tag.GetType().Name == "TextBlock") { folder = ((TextBlock)item.Tag).Tag as string; }
            if (folder == null) { return; }
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), false, 0, 0, true);
        }
        #endregion

        #region ContextMenu Add Shuffle Playlist functions
        private void CM_AddShuffledAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistEnqueue(tracks, true, 0, 0, true);
        }

        private void CM_AddShuffledFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (item.Tag.GetType().Name == "ListView")
            {
                ListView view = (ListView)item.Tag;
                if (view.SelectedItems.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                    Parent.player.PlaylistEnqueue(files.ToArray(), true, 0, 0, true);
                }
            }
            else
            {
                string folder = null;
                if (item.Tag == null) { return; }
                else if (item.Tag.GetType().Name == "Button") { folder = ((Button)item.Tag).Tag as string; }
                else if (item.Tag.GetType().Name == "AlignablePanel") { folder = ((AlignablePanel)item.Tag).Tag as string; }
                else if (item.Tag.GetType().Name == "TextBlock") { folder = ((TextBlock)item.Tag).Tag as string; }
                if (folder == null) { return; }
                Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), true, 0, 0, true);
            }
        }
        #endregion

        #region ContextMenu Play functions
        private void CM_PlayTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (item.Tag.GetType().Name == "ListView")
            {
                ListView view = (ListView)item.Tag;
                if (view.SelectedItems.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                    Parent.player.PlaylistClear();
                    Parent.player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
                }
            }
            else
            {
                string track = (string)((Button)item.Tag).Tag;
                Parent.player.PlaylistClear();
                Parent.player.PlaylistEnqueue(new string[] { track }, false, 0, 0, true);
            }
        }

        private void CM_PlayAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(tracks, false, 0, 0, true);
        }

        private void CM_PlayFolder(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string folder = null;
            if (item.Tag == null) { return; }
            else if (item.Tag.GetType().Name == "Button") { folder = ((Button)item.Tag).Tag as string; }
            else if (item.Tag.GetType().Name == "AlignablePanel") { folder = ((AlignablePanel)item.Tag).Tag as string; }
            else if (item.Tag.GetType().Name == "TextBlock") { folder = ((TextBlock)item.Tag).Tag as string; }
            if (folder == null) { return; }
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), false, 0, 0, true);
        }
        #endregion

        #region ContextMenu Play Shuffled functions
        private void CM_PlayShuffledAlbum(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string[] tracks = (string[])((Button)item.Tag).Tag;
            Parent.player.PlaylistClear();
            Parent.player.PlaylistEnqueue(tracks, true, 0, 0, true);
        }

        private void CM_PlayShuffledFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender; 
                Debug.WriteLine(item.Tag.GetType().Name);
                if (item.Tag.GetType().Name == "ListView")
                {
                    ListView view = (ListView)item.Tag;
                    if (view.SelectedItems.Count > 0)
                    {
                        List<string> files = new List<string>();
                        foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                        Parent.player.PlaylistClear();
                        Parent.player.PlaylistEnqueue(files.ToArray(), true, 0, 0, true);
                    }
                }
                else
                {
                    string folder = null;
                    if (item.Tag == null) { return; }
                    else if (item.Tag.GetType().Name == "Button") { folder = ((Button)item.Tag).Tag as string; }
                    else if (item.Tag.GetType().Name == "AlignablePanel") { folder = ((AlignablePanel)item.Tag).Tag as string; }
                    else if (item.Tag.GetType().Name == "TextBlock") { folder = ((TextBlock)item.Tag).Tag as string; }
                    else { Debug.WriteLine(item.Tag.GetType().Name); }

                    if (folder == null) { return; }
                    Parent.player.PlaylistClear();
                    Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), true, 0, 0, true);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); Debug.WriteLine(ex.StackTrace); }
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
            if (item.Tag == null) { return; }
            else if (item.Tag.GetType().Name == "Button")
            {
                Button btn = (Button)item.Tag;
                string folderPath = ((string)btn.Tag).Trim().TrimEnd(MainWindow.SeparatorChar);
                Debug.WriteLine("--> CM_EditFolder, folderPath='" + folderPath + "'");
                string[] pathTab = folderPath.Split(MainWindow.SeparatorChar);
                Debug.WriteLine("--> CM_EditFolder, pathTab.Length='" + pathTab.Length + "'");

                RenameWindow win = new RenameWindow(Parent, folderPath, pathTab);
                win.ShowDialog();
                if (win.renamed == true)
                {
                    DisplayPath(CurrentPath);
                }
                //throw new System.NotImplementedException(); }
            }
            else if (item.Tag.GetType().Name == "AlignablePanel") { return; }
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
