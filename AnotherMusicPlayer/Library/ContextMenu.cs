using AnotherMusicPlayer.Components;
using AnotherMusicPlayer.MainWindow2Space;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        public LibraryContextMenu MakeContextMenu(Control parent, string otype = null, bool back = false, string backPath = "")
        {
            if (otype == null || otype == "") { otype = "track"; }
            string type = otype = otype.ToLower();
            if (type == "file") { type = "track"; }
            Debug.WriteLine("MakeContextMenu(), Name = " + parent.Name + ", type = " + type);
            LibraryContextMenu cm = new LibraryContextMenu();
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (cm.Items[i].Name == "BackFolder" && back == true)
                {
                    cm.Items[i].Click += CM_FolderBack;
                    cm.Items[i].Tag = backPath;
                    cm.Items[i].Visible = true;
                }
                else if (cm.Items[i].Name.ToLower() == "add" + type)
                {
                    cm.Items[i].Tag = parent;
                    if (type == "track") { cm.Items[i].Click += CM_AddTrack; }
                    if (type == "selection") { cm.Items[i].Click += CM_AddTrack; }
                    if (type == "album") { cm.Items[i].Click += CM_AddAlbum; }
                    if (type == "disk") { cm.Items[i].Click += CM_AddAlbum; }
                    if (type == "folder") { cm.Items[i].Click += CM_AddFolder; }
                    cm.Items[i].Visible = true;
                }
                else if (cm.Items[i].Name.ToLower() == "addshuffle" + type)
                {
                    cm.Items[i].Tag = parent;
                    if (type == "album") { cm.Items[i].Click += CM_AddShuffledAlbum; }
                    if (type == "disk") { cm.Items[i].Click += CM_AddShuffledAlbum; }
                    if (type == "folder") { cm.Items[i].Click += CM_AddShuffledFolder; }
                    if (type == "selection") { cm.Items[i].Click += CM_AddShuffledFolder; }
                    cm.Items[i].Visible = true;
                }
                else if (cm.Items[i].Name.ToLower() == "play" + type)
                {
                    cm.Items[i].Tag = parent;
                    if (type == "track") { cm.Items[i].Click += CM_PlayTrack; }
                    if (type == "selection") { cm.Items[i].Click += CM_PlayTrack; }
                    if (type == "album") { cm.Items[i].Click += CM_PlayAlbum; }
                    if (type == "disk") { cm.Items[i].Click += CM_PlayAlbum; }
                    if (type == "folder") { cm.Items[i].Click += CM_PlayFolder; }
                    cm.Items[i].Visible = true;

                }
                else if (cm.Items[i].Name.ToLower() == "playshuffle" + type)
                {
                    cm.Items[i].Tag = parent;
                    if (type == "album") { cm.Items[i].Click += CM_PlayShuffledAlbum; }
                    if (type == "disk") { cm.Items[i].Click += CM_PlayShuffledAlbum; }
                    if (type == "folder") { cm.Items[i].Click += CM_PlayShuffledFolder; }
                    if (type == "selection") { cm.Items[i].Click += CM_PlayShuffledFolder; }
                    cm.Items[i].Visible = true;

                }
                else if (cm.Items[i].Name.ToLower() == "edit" + otype)
                {
                    cm.Items[i].Tag = parent;
                    if (type == "file") { cm.Items[i].Click += CM_EditTrack; }
                    if (type == "track") { cm.Items[i].Click += CM_EditTrack; }
                    if (type == "album") { cm.Items[i].Click += CM_EditAlbum; }
                    if (type == "folder") {
                        bool ok = true;
                        if (parent == null) { Debug.WriteLine("A"); ok = false; }
                        else if (App.win1 == null) { Debug.WriteLine("B"); ok = false; }
                        else if (App.win1.library == null) { Debug.WriteLine("C"); ok = false; }
                        //else if (App.win1.library.CurrentPath == Settings.LibFolder) { Debug.WriteLine("D"); ok = false; }
                        else if (/*parent.Name == "LibraryNavigationContentFolders" || */parent.Name == "LibrarySearchContent") { Debug.WriteLine("E"); ok = false; }

                        if(ok) { cm.Items[i].Click += CM_EditFolder; }
                        else { cm.Items[i].Visible = false; }
                    }
                    cm.Items[i].Visible = true;
                }
                else if (cm.Items[i].Name.ToLower() == "playlistsadd" + type)
                {
                    cm.Items[i].Tag = parent;
                    if (type == "track") { cm.Items[i].Click += CM_AddPlaylistTrack; }
                    if (type == "selection") { cm.Items[i].Click += CM_AddPlaylistTrack; }
                    if (type == "album") { cm.Items[i].Click += CM_AddPlaylistAlbum; }
                    if (type == "disk") { cm.Items[i].Click += CM_AddPlaylistAlbum; }
                    if (type == "folder") { cm.Items[i].Click += CM_AddPlaylistFolder; }
                    cm.Items[i].Visible = true;

                }
                else if (cm.Items[i].Name.ToLower().Contains("radio") && type == "radio")
                {
                    if (cm.Items[i].Name == "RadioEdit") { }
                    else if (cm.Items[i].Name == "RadioDelete") { }
                    cm.Items[i].Visible = true;
                }
                else { cm.Items[i].Visible = false; }
            }
            return cm;
        }

        private void CM_FolderBack(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_FolderBack()");
            string tag = (string)((ToolStripItem)sender).Tag;
            DirectoryInfo di = new DirectoryInfo(tag);
            DisplayPath(di.Parent.FullName);
        }

        private void CM_AddPlaylistTrack(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddPlaylistTrack()");
            ToolStripItem item = (ToolStripItem)sender;
            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { Debug.WriteLine("parent is null");  return; }
            Debug.WriteLine(parent.GetType().Name);
            if (parent.GetType().Name == "ListView")
            {
                ListView view = (ListView)parent;
                if (view.SelectedItems.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                    InsertPlayList ip = new InsertPlayList(App.win1, files.ToArray());
                    ip.ShowDialog();
                }
            }
            else if (parent.GetType() == typeof(DataGridView))
            {
                Debug.WriteLine("Merde2");
                //ListView view = (ListView)parent;
                //if (view.SelectedItems.Count > 0)
                //{
                //    List<string> files = new List<string>();
                //    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                //    InsertPlayList ip = new InsertPlayList(App.win1, files.ToArray());
                //    ip.ShowDialog();
                //}
            }
            else
            {
                string track = (string)parent.Tag;
                InsertPlayList ip = new InsertPlayList(App.win1, new string[] { track });
                ip.ShowDialog();
            }
        }

        private void CM_AddPlaylistAlbum(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddPlaylistAlbum()");
            ToolStripItem item = (ToolStripItem)sender;
            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { return; }

            string[] tracks = (string[])parent.Tag;
            InsertPlayList ip = new InsertPlayList(App.win1, tracks);
            ip.ShowDialog();
        }

        private void CM_AddPlaylistFolder(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddPlaylistFolder()");
            try
            {
                ToolStripItem item = (ToolStripItem)sender;

                Control parent = null;
                if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
                else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
                if (parent == null) { Debug.WriteLine("parent is null"); return; }

                if (parent.GetType() == typeof(DataGridView))
                {
                    Debug.WriteLine("Merde2");
                    if (((DataGridView)parent).SelectedRows.Count > 0)
                    {
                        List<string> files = new List<string>();
                        foreach (DataGridViewRow row in ((DataGridView)parent).SelectedRows)
                        {
                            if (row.DataBoundItem is LibraryFolderObjets itm)
                            {
                                string[] rfiles = getDirectoryMediaFIles(itm.Path);
                                files.AddRange(rfiles);
                            }
                        }

                        InsertPlayList ip = new InsertPlayList(App.win1, files.ToArray());
                        ip.ShowDialog();
                    }
                }

            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); Debug.WriteLine(ex.StackTrace); }
        }

        #region ContextMenu Add PlayBacklist functions
        private void CM_AddTrack(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddTrack()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { return; }

            if (parent.GetType().Name == "ListView")
            {
                ListView view = (ListView)parent.Tag;
                if (view.SelectedItems.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                    Player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
                }
            }
            else
            {
                string track = (string)parent.Tag;
                Player.PlaylistEnqueue(new string[] { track }, false, 0, 0, true);
            }
        }

        private void CM_AddAlbum(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddAlbum()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { return; }

            string[] tracks = (string[])parent.Tag;
            Player.PlaylistEnqueue(tracks, false, 0, 0, true);
        }

        private void CM_AddFolder(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddFolder()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { Debug.WriteLine("parent is null"); return; }

            if (parent.GetType() == typeof(DataGridView))
            {
                Debug.WriteLine("Merde2");
                if(((DataGridView)parent).SelectedRows.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (DataGridViewRow row in ((DataGridView)parent).SelectedRows)
                    {
                        if (row.DataBoundItem is LibraryFolderObjets itm)
                        {
                            string[] rfiles = Directory.GetFiles(itm.Path);
                            files.AddRange(rfiles); 
                        }
                    }
                        
                    Player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
                }
            }
            else
            {
                string folder = folder = parent.Tag as string;
                if (folder == null) { return; }
                Player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), false, 0, 0, true);
            }
        }
        #endregion

        #region ContextMenu Add Shuffle PlayBacklist functions
        private void CM_AddShuffledAlbum(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddShuffledAlbum()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { return; }

            string[] tracks = (string[])parent.Tag;
            Player.PlaylistEnqueue(tracks, true, 0, 0, true);
        }

        private void CM_AddShuffledFolder(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_AddShuffledFolder()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { Debug.WriteLine("parent is null"); return; }

            if (parent.GetType() == typeof(DataGridView))
            {
                Debug.WriteLine("Merde2");
                if (((DataGridView)parent).SelectedRows.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (DataGridViewRow row in ((DataGridView)parent).SelectedRows)
                    {
                        if (row.DataBoundItem is LibraryFolderObjets itm)
                        {
                            string[] rfiles = getDirectoryMediaFIles(itm.Path);
                            files.AddRange(rfiles);
                        }
                    }

                    Player.PlaylistEnqueue(files.ToArray(), true, 0, 0, true);
                }
            }
            else
            {
                string folder = folder = parent.Tag as string;
                if (folder == null) { return; }
                Player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), true, 0, 0, true);
            }
        }
        #endregion

        #region ContextMenu PlayBack functions
        private void CM_PlayTrack(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_PlayTrack()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { return; }

            if (parent.GetType().Name == "ListView")
            {
                ListView view = (ListView)parent;
                if (view.SelectedItems.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (MediaItem itm in view.SelectedItems) { files.Add(itm.Path); }
                    Player.PlaylistClear();
                    Player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
                }
            }
            else
            {
                string track = (string)parent.Tag;
                Player.PlaylistClear();
                Player.PlaylistEnqueue(new string[] { track }, false, 0, 0, true);
            }
        }

        private void CM_PlayAlbum(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_PlayAlbum()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { return; }

            string[] tracks = (string[])parent.Tag;
            Player.PlaylistClear();
            Player.PlaylistEnqueue(tracks, false, 0, 0, true);
        }

        private void CM_PlayFolder(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_PlayFolder()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { Debug.WriteLine("parent is null"); return; }

            if (parent.GetType() == typeof(DataGridView))
            {
                if (((DataGridView)parent).SelectedRows.Count > 0)
                {
                    List<string> files = new List<string>();
                    foreach (DataGridViewRow row in ((DataGridView)parent).SelectedRows)
                    {
                        if (row.DataBoundItem is LibraryFolderObjets itm)
                        {
                            string[] rfiles = getDirectoryMediaFIles(itm.Path);
                            files.AddRange(rfiles);
                        }
                    }

                    Player.StopAll();
                    Player.PlaylistClear();
                    Player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
                }
            }
            else
            {
                string folder = folder = parent.Tag as string;
                if (folder == null) { return; }
                Player.StopAll();
                Player.PlaylistClear();
                Player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), false, 0, 0, true);
            }
        }
        #endregion

        #region ContextMenu PlayBack Shuffled functions
        private void CM_PlayShuffledAlbum(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_PlayShuffledAlbum()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { return; }

            string[] tracks = (string[])parent.Tag;
            Player.PlaylistClear();
            Player.PlaylistEnqueue(tracks, true, 0, 0, true);
        }

        private void CM_PlayShuffledFolder(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_PlayShuffledFolder()");
            try
            {
                ToolStripItem item = (ToolStripItem)sender;

                Control parent = null;
                if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
                else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
                if (parent == null) { Debug.WriteLine("parent is null"); return; }

                if (parent.GetType() == typeof(DataGridView))
                {
                    if (((DataGridView)parent).SelectedRows.Count > 0)
                    {
                        List<string> files = new List<string>();
                        foreach (DataGridViewRow row in ((DataGridView)parent).SelectedRows)
                        {
                            if (row.DataBoundItem is LibraryFolderObjets itm)
                            {
                                string[] rfiles = getDirectoryMediaFIles(itm.Path);
                                files.AddRange(rfiles);
                            }
                        }

                        Player.StopAll();
                        Player.PlaylistClear();
                        Player.PlaylistEnqueue(files.ToArray(), true, 0, 0, true);
                    }
                }
                else
                {
                    string folder = folder = parent.Tag as string;
                    if (folder == null) { return; }
                    Player.StopAll();
                    Player.PlaylistClear();
                    Player.PlaylistEnqueue(getDirectoryMediaFIles(folder, true), true, 0, 0, true);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); Debug.WriteLine(ex.StackTrace); }
        }
        #endregion

        #region ContextMenu Edit functions
        private void CM_EditTrack(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_EditTrack()");
            ToolStripItem item = (ToolStripItem)sender;
            AnotherMusicPlayer.Components.TrackButton btn = (AnotherMusicPlayer.Components.TrackButton)item.Tag;
            string track = "" + ((btn.item!=null)?btn.item.Path:"");
            TagsEditor tags = new TagsEditor(Parent, "track", new string[] { track });

            if (tags.ShowDialog() == DialogResult.OK)
            {
                if (tags.CoverChanged == true) { Bdd.DatabaseClearCover(track); }
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    Thread.Sleep(200);
                    DisplayPath(CurrentPath);
                }));
            }
        }

        private void CM_EditAlbum(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_EditAlbum()");
            ToolStripItem item = (ToolStripItem)sender;
            AlbumBlock btn = (AlbumBlock)item.Tag;
            string[] tracks = (string[])btn.Tag;
            TagsEditor tags = new TagsEditor(Parent, "album", tracks);

            if (tags.ShowDialog() == DialogResult.OK)
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

        }

        private void CM_EditFolder(object sender, EventArgs e)
        {
            Debug.WriteLine("CM_EditFolder()");
            ToolStripItem item = (ToolStripItem)sender;

            Control parent = null;
            if (item.Owner.Parent != null) { parent = item.Owner.Parent; }
            else if (item.Tag != null) { try { parent = (Control)item.Tag; } catch (Exception) { } }
            if (parent == null) { Debug.WriteLine("parent is null"); return; }

            if (parent.GetType() == typeof(DataGridView))
            {
                if (((DataGridView)parent).SelectedRows.Count > 0)
                {
                    List<string> files = new List<string>();
                    DataGridViewRow row = ((DataGridView)parent).SelectedRows[0];
                    if (row.DataBoundItem is LibraryFolderObjets itm)
                    {
                        string folderPath = (itm.Path).Trim().TrimEnd(MainWindow2.SeparatorChar);
                        Debug.WriteLine("--> CM_EditFolder, folderPath='" + folderPath + "'");
                        string[] pathTab = folderPath.Split(MainWindow2.SeparatorChar);
                        Debug.WriteLine("--> CM_EditFolder, pathTab.Length='" + pathTab.Length + "'");

                        RenameWindow win = new RenameWindow(Parent, folderPath, pathTab);
                        if (win.ShowDialog() == DialogResult.OK) { DisplayPath(CurrentPath); }
                    }
                }
            }
            else
            {
                if (item.Tag == null) { return; }
                else if (item.Tag.GetType().Name == "Button")
                {
                    Button btn = (Button)item.Tag;
                    string folderPath = ((string)btn.Tag).Trim().TrimEnd(MainWindow2.SeparatorChar);
                    Debug.WriteLine("--> CM_EditFolder, folderPath='" + folderPath + "'");
                    string[] pathTab = folderPath.Split(MainWindow2.SeparatorChar);
                    Debug.WriteLine("--> CM_EditFolder, pathTab.Length='" + pathTab.Length + "'");

                    RenameWindow win = new RenameWindow(Parent, folderPath, pathTab);
                    if (win.ShowDialog() == DialogResult.OK) { DisplayPath(CurrentPath); }
                }
                else if (item.Tag.GetType().Name == "AlignablePanel") { return; }
            }


        }
        #endregion
    }

    /// <summary>
    /// Logique d'interaction pour LibraryContextMenu.xaml
    /// </summary>
    public partial class LibraryContextMenu : ContextMenuStrip
    {
        private System.Drawing.Color _ForeColor = System.Drawing.Color.Black;
        new public System.Drawing.Color ForeColor {
            get { return _ForeColor; }
            set { base.ForeColor = _ForeColor = value; Update(); }
        }
        private System.Drawing.Color _BackColor = System.Drawing.Color.Black;
        new public System.Drawing.Color BackColor {
            get { return _BackColor; }
            set { base.BackColor = _BackColor = value; Update(); }
        }
        private SolidColorBrush DefaultBrush = new SolidColorBrush(Colors.White);
        private int ButtonIconSize = 32;

        // PARTIE BACK BUTTON
        public ToolStripItem BackFolder = null;
        // ADD IN PLAYLIST ORDONED
        public ToolStripItem AddFolder = null;
        public ToolStripItem AddTrack = null;
        public ToolStripItem AddAlbum = null;
        public ToolStripItem AddDisk = null;
        public ToolStripItem AddSelection = null;
        // ADD IN PLAYLIST RANDOMIZED
        public ToolStripItem AddShuffleFolder = null;
        public ToolStripItem AddShuffleAlbum = null;
        public ToolStripItem AddShuffleDisk = null;
        // PLAYLIST ORDONED
        public ToolStripItem PlayFolder = null;
        public ToolStripItem PlayTrack = null;
        public ToolStripItem PlayAlbum = null;
        public ToolStripItem PlayDisk = null;
        public ToolStripItem PlaySelection = null;
        // REPLACE PLAYLIST RANDOMIZED
        public ToolStripItem PlayShuffleFolder = null;
        public ToolStripItem PlayShuffleAlbum = null;
        public ToolStripItem PlayShuffleDisk = null;
        public ToolStripItem PlayShuffleSelection = null;
        // PARTIE Edition
        public ToolStripItem EditFile = null;
        public ToolStripItem EditTrack = null;
        public ToolStripItem EditFolder = null;
        public ToolStripItem EditAlbum = null;
        // PARTIE ADD INTO A PLAYLIST
        public ToolStripItem PlayListsAddFolder = null;
        public ToolStripItem PlayListsAddTrack = null;
        public ToolStripItem PlayListsAddSelection = null;
        public ToolStripItem PlayListsAddAlbum = null;
        public ToolStripItem PlayListsAddDisk = null;

        public LibraryContextMenu()
        {
            Font = new Font("Segoe UI", 15, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
            base.BackColor = _BackColor = App.style.GetColor("ContextMenuBackColor");
            base.ForeColor = _ForeColor = App.style.GetColor("ContextMenuForeColor");

            RenderMode = ToolStripRenderMode.System;
            // PARTIE BACK BUTTON
            BackFolder = Items.Add(App.GetTranslation("LibraryContextMenuGetBack"), Icons.FromIconKind(IconKind.ArrowLeftTop, ButtonIconSize, DefaultBrush));
            // ADD IN PLAYLIST ORDONED
            AddFolder = Items.Add(App.GetTranslation("LibraryContextMenuAddFolder"), Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush));
            AddTrack = Items.Add(App.GetTranslation("LibraryContextMenuAddTrack"), Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush));
            AddAlbum = Items.Add(App.GetTranslation("LibraryContextMenuAddAlbum"), Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush));
            AddDisk = Items.Add(App.GetTranslation("LibraryContextMenuAddAddDisk"), Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush));
            AddSelection = Items.Add(App.GetTranslation("LibraryContextMenuAddGeneric"), Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush));
            // ADD IN PLAYLIST RANDOMIZED
            AddShuffleFolder = Items.Add(App.GetTranslation("LibraryContextMenuAddShuffleFolder"), Icons.FromIconKind(IconKind.ShuffleVariant, ButtonIconSize, DefaultBrush));
            AddShuffleAlbum = Items.Add(App.GetTranslation("LibraryContextMenuAddShuffleAlbum"), Icons.FromIconKind(IconKind.ShuffleVariant, ButtonIconSize, DefaultBrush));
            AddShuffleDisk = Items.Add(App.GetTranslation("LibraryContextMenuAddShuffleDisk"), Icons.FromIconKind(IconKind.ShuffleVariant, ButtonIconSize, DefaultBrush));
            // PLAYLIST ORDONED
            PlayFolder = Items.Add(App.GetTranslation("LibraryContextMenuPlayFolder"), Icons.FromIconKind(IconKind.FolderPlay, ButtonIconSize, DefaultBrush));
            PlayTrack = Items.Add(App.GetTranslation("LibraryContextMenuPlayTrack"), Icons.FromIconKind(IconKind.PlayBox, ButtonIconSize, DefaultBrush));
            PlayAlbum = Items.Add(App.GetTranslation("LibraryContextMenuPlayAlbum"), Icons.FromIconKind(IconKind.Album, ButtonIconSize, DefaultBrush));
            PlayDisk = Items.Add(App.GetTranslation("LibraryContextMenuPlayDisk"), Icons.FromIconKind(IconKind.Album, ButtonIconSize, DefaultBrush));
            PlaySelection = Items.Add(App.GetTranslation("LibraryContextMenuPlayGeneric"), Icons.FromIconKind(IconKind.Album, ButtonIconSize, DefaultBrush));
            // REPLACE PLAYLIST RANDOMIZED
            PlayShuffleFolder = Items.Add(App.GetTranslation("LibraryContextMenuPlayShuffleFolder"), Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush));
            PlayShuffleAlbum = Items.Add(App.GetTranslation("LibraryContextMenuPlayShuffleAlbum"), Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush));
            PlayShuffleDisk = Items.Add(App.GetTranslation("LibraryContextMenuPlayShuffleDisk"), Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush));
            PlayShuffleSelection = Items.Add(App.GetTranslation("LibraryContextMenuPlayShuffleGeneric"), Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush));
            // PARTIE Edition
            EditFile = Items.Add(App.GetTranslation("LibraryContextMenuEditFile"), Icons.FromIconKind(IconKind.FileEdit, ButtonIconSize, DefaultBrush));
            EditTrack = Items.Add(App.GetTranslation("LibraryContextMenuEditTrack"), Icons.FromIconKind(IconKind.FileEdit, ButtonIconSize, DefaultBrush));
            EditFolder = Items.Add(App.GetTranslation("LibraryContextMenuEditFolder"), Icons.FromIconKind(IconKind.FolderEdit, ButtonIconSize, DefaultBrush));
            EditAlbum = Items.Add(App.GetTranslation("LibraryContextMenuEditAlbum"), Icons.FromIconKind(IconKind.FolderEdit, ButtonIconSize, DefaultBrush));
            // PARTIE ADD INTO A PLAYLIST
            PlayListsAddFolder = Items.Add(App.GetTranslation("LibraryContextMenuPlayListsAddFolder"), Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush));
            PlayListsAddTrack = Items.Add(App.GetTranslation("LibraryContextMenuPlayListsAddTrack"), Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush));
            PlayListsAddSelection = Items.Add(App.GetTranslation("LibraryContextMenuPlayListsAddGeneric"), Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush));
            PlayListsAddAlbum = Items.Add(App.GetTranslation("LibraryContextMenuPlayListsAddAlbum"), Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush));
            PlayListsAddDisk = Items.Add(App.GetTranslation("LibraryContextMenuPlayListsAddDisk"), Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush));

            BackFolder.Name = nameof(BackFolder);

            AddFolder.Name = nameof(AddFolder);
            AddTrack.Name = nameof(AddTrack);
            AddAlbum.Name = nameof(AddAlbum);
            AddDisk.Name = nameof(AddDisk);
            AddSelection.Name = nameof(AddSelection);

            AddShuffleFolder.Name = nameof(AddShuffleFolder);
            AddShuffleAlbum.Name = nameof(AddShuffleAlbum);
            AddShuffleDisk.Name = nameof(AddShuffleDisk);

            PlayFolder.Name = nameof(PlayFolder);
            PlayTrack.Name = nameof(PlayTrack);
            PlayAlbum.Name = nameof(PlayAlbum);
            PlayDisk.Name = nameof(PlayDisk);
            PlaySelection.Name = nameof(PlaySelection);

            PlayShuffleFolder.Name = nameof(PlayShuffleFolder);
            PlayShuffleAlbum.Name = nameof(PlayShuffleAlbum);
            PlayShuffleDisk.Name = nameof(PlayShuffleDisk);
            PlayShuffleSelection.Name = nameof(PlayShuffleSelection);

            EditFile.Name = nameof(EditFile);
            EditTrack.Name = nameof(EditTrack);
            EditFolder.Name = nameof(EditFolder);
            EditAlbum.Name = nameof(EditAlbum);

            PlayListsAddFolder.Name = nameof(PlayListsAddFolder);
            PlayListsAddTrack.Name = nameof(PlayListsAddTrack);
            PlayListsAddSelection.Name = nameof(PlayListsAddSelection);
            PlayListsAddAlbum.Name = nameof(PlayListsAddAlbum);
            PlayListsAddDisk.Name = nameof(PlayListsAddDisk);
        }

        public void Update()
        {
            DefaultBrush = new SolidColorBrush(App.DrawingColorToMediaColor(_ForeColor));
            // PARTIE BACK BUTTON
            BackFolder.ForeColor = _ForeColor; BackFolder.Text = App.GetTranslation("LibraryContextMenuGetBack"); BackFolder.Image = Icons.FromIconKind(IconKind.ArrowLeftTop, ButtonIconSize, DefaultBrush);
            // ADD IN PLAYLIST ORDONED
            AddFolder.ForeColor = _ForeColor; AddFolder.Text = App.GetTranslation("LibraryContextMenuAddFolder"); AddFolder.Image = Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush);
            AddTrack.ForeColor = _ForeColor; AddTrack.Text = App.GetTranslation("LibraryContextMenuAddTrack"); AddTrack.Image = Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush);
            AddAlbum.ForeColor = _ForeColor; AddAlbum.Text = App.GetTranslation("LibraryContextMenuAddAlbum"); AddAlbum.Image = Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush);
            AddDisk.ForeColor = _ForeColor; AddDisk.Text = App.GetTranslation("LibraryContextMenuAddDisk"); AddDisk.Image = Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush);
            AddSelection.ForeColor = _ForeColor; AddSelection.Text = App.GetTranslation("LibraryContextMenuAddGeneric"); AddSelection.Image = Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush);
            // ADD IN PLAYLIST RANDOMIZED
            AddShuffleFolder.ForeColor = _ForeColor; AddShuffleFolder.Text = App.GetTranslation("LibraryContextMenuAddShuffleFolder"); AddShuffleFolder.Image = Icons.FromIconKind(IconKind.ShuffleVariant, ButtonIconSize, DefaultBrush);
            AddShuffleAlbum.ForeColor = _ForeColor; AddShuffleAlbum.Text = App.GetTranslation("LibraryContextMenuAddShuffleAlbum"); AddShuffleAlbum.Image = Icons.FromIconKind(IconKind.ShuffleVariant, ButtonIconSize, DefaultBrush);
            AddShuffleDisk.ForeColor = _ForeColor; AddShuffleDisk.Text = App.GetTranslation("LibraryContextMenuAddShuffleDisk"); AddShuffleDisk.Image = Icons.FromIconKind(IconKind.ShuffleVariant, ButtonIconSize, DefaultBrush);
            // PLAYLIST ORDONED
            PlayFolder.ForeColor = _ForeColor; PlayFolder.Text = App.GetTranslation("LibraryContextMenuPlayFolder"); PlayFolder.Image = Icons.FromIconKind(IconKind.FolderPlay, ButtonIconSize, DefaultBrush);
            PlayTrack.ForeColor = _ForeColor; PlayTrack.Text = App.GetTranslation("LibraryContextMenuPlayTrack"); PlayTrack.Image = Icons.FromIconKind(IconKind.PlayBox, ButtonIconSize, DefaultBrush);
            PlayAlbum.ForeColor = _ForeColor; PlayAlbum.Text = App.GetTranslation("LibraryContextMenuPlayAlbum"); PlayAlbum.Image = Icons.FromIconKind(IconKind.Album, ButtonIconSize, DefaultBrush);
            PlayDisk.ForeColor = _ForeColor; PlayDisk.Text = App.GetTranslation("LibraryContextMenuPlayDisk"); PlayDisk.Image = Icons.FromIconKind(IconKind.Album, ButtonIconSize, DefaultBrush);
            PlaySelection.ForeColor = _ForeColor; PlaySelection.Text = App.GetTranslation("LibraryContextMenuPlayGeneric"); PlaySelection.Image = Icons.FromIconKind(IconKind.Album, ButtonIconSize, DefaultBrush);
            // REPLACE PLAYLIST RANDOMIZED
            PlayShuffleFolder.ForeColor = _ForeColor; PlayShuffleFolder.Text = App.GetTranslation("LibraryContextMenuPlayShuffleFolder"); PlayShuffleFolder.Image = Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush);
            PlayShuffleAlbum.ForeColor = _ForeColor; PlayShuffleAlbum.Text = App.GetTranslation("LibraryContextMenuPlayShuffleAlbum"); PlayShuffleAlbum.Image = Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush);
            PlayShuffleDisk.ForeColor = _ForeColor; PlayShuffleDisk.Text = App.GetTranslation("LibraryContextMenuPlayShuffleDisk"); PlayShuffleDisk.Image = Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush);
            PlayShuffleSelection.ForeColor = _ForeColor; PlayShuffleSelection.Text = App.GetTranslation("LibraryContextMenuPlayShuffleGeneric"); PlayShuffleSelection.Image = Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush);
            // PARTIE Edition
            EditFile.ForeColor = _ForeColor; EditFile.Text = App.GetTranslation("LibraryContextMenuEditFile"); EditFile.Image = Icons.FromIconKind(IconKind.FileEdit, ButtonIconSize, DefaultBrush);
            EditTrack.ForeColor = _ForeColor; EditTrack.Text = App.GetTranslation("LibraryContextMenuEditTrack"); EditTrack.Image = Icons.FromIconKind(IconKind.FileEdit, ButtonIconSize, DefaultBrush);
            EditFolder.ForeColor = _ForeColor; EditFolder.Text = App.GetTranslation("LibraryContextMenuEditFolder"); EditFolder.Image = Icons.FromIconKind(IconKind.FolderEdit, ButtonIconSize, DefaultBrush);
            EditAlbum.ForeColor = _ForeColor; EditAlbum.Text = App.GetTranslation("LibraryContextMenuEditAlbum"); EditAlbum.Image = Icons.FromIconKind(IconKind.FolderEdit, ButtonIconSize, DefaultBrush);
            // PARTIE ADD INTO A PLAYLIST
            PlayListsAddFolder.ForeColor = _ForeColor; PlayListsAddFolder.Text = App.GetTranslation("LibraryContextMenuPlayListsAddFolder"); PlayListsAddFolder.Image = Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush);
            PlayListsAddTrack.ForeColor = _ForeColor; PlayListsAddTrack.Text = App.GetTranslation("LibraryContextMenuPlayListsAddTrack"); PlayListsAddTrack.Image = Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush);
            PlayListsAddSelection.ForeColor = _ForeColor; PlayListsAddSelection.Text = App.GetTranslation("LibraryContextMenuPlayListsAddGeneric"); PlayListsAddSelection.Image = Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush);
            PlayListsAddAlbum.ForeColor = _ForeColor; PlayListsAddAlbum.Text = App.GetTranslation("LibraryContextMenuPlayListsAddAlbum"); PlayListsAddAlbum.Image = Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush);
            PlayListsAddDisk.ForeColor = _ForeColor; PlayListsAddDisk.Text = App.GetTranslation("LibraryContextMenuPlayListsAddDisk"); PlayListsAddDisk.Image = Icons.FromIconKind(IconKind.TableColumnPlusAfter, ButtonIconSize, DefaultBrush);
        }
    }
}
