using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
            ContextMenu cm = new LibraryContextMenu() { Style = Parent.FindResource("LibraryCustomContextMenuStyle") as Style };
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
                else { ((MenuItem)cm.Items[i]).Visibility = Visibility.Collapsed; }
            }
            return cm;
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
            catch
            {
                try { }
                catch { }
            }
            if (folder == null) { return; }
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder));
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
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder), true);
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
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder));
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
            Parent.player.PlaylistEnqueue(getDirectoryMediaFIles(folder), true);
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
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
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
                if (tags.CoverChanged == true) { 
                    foreach(string track in tracks)Bdd.DatabaseClearCover(track);
                }
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
                    Thread.Sleep(500);
                    DisplayPath(CurrentPath);
                }));
            }
            //throw new System.NotImplementedException();
        }

        private void CM_EditFolder(object sender, RoutedEventArgs e)
        {
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
