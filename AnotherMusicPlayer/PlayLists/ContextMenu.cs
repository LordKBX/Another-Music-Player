using AnotherMusicPlayer.MainWindow2Space;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Linq;

namespace AnotherMusicPlayer
{
    public partial class PlayLists
    {
        private ContextMenuStrip MakeNodeContextMenu(TreeNode parent, PlayListsNodeContextMenuType otype)
        {
            ContextMenuStrip cm = new PlayListsNodeContextMenu();
            for (int i = 0; i < cm.Items.Count; i++)
            {
                cm.Items[i].Tag = (parent, otype);

                if (cm.Items[i].Name == "PlayListPlay" && 
                    (otype == PlayListsNodeContextMenuType.Auto || otype == PlayListsNodeContextMenuType.CustomNode))
                { cm.Items[i].Click += (otype == PlayListsNodeContextMenuType.CustomNode)?NodeContextMenu_PlayCustom: NodeContextMenu_Play; }
                else if (cm.Items[i].Name == "PlayListPlayShuffled" && 
                    (otype == PlayListsNodeContextMenuType.Auto || otype == PlayListsNodeContextMenuType.CustomNode))
                { cm.Items[i].Click += (otype == PlayListsNodeContextMenuType.CustomNode)? NodeContextMenu_PlayShuffledCustom : NodeContextMenu_PlayShuffled; }
                // Custom part
                else if (cm.Items[i].Name == "PlayListCustomDelete" && (otype == PlayListsNodeContextMenuType.CustomNode))
                { cm.Items[i].Click += NodeContextMenu_DeleteCustom; }
                else if (cm.Items[i].Name == "PlayListCustomRename" && (otype == PlayListsNodeContextMenuType.CustomNode))
                { cm.Items[i].Click += NodeContextMenu_RenameCustom; }
                else if (cm.Items[i].Name == "PlayListCustomAdd" && (otype == PlayListsNodeContextMenuType.Custom))
                { cm.Items[i].Click += NodeContextMenu_AddCustom; }
                // Radio part
                else if (cm.Items[i].Name == "PlayRadio" && (otype == PlayListsNodeContextMenuType.RadioNode))
                { cm.Items[i].Click += NodeContextMenu_PlayRadio; }
                // Radio Category
                else if (cm.Items[i].Name == "AddCategoryRadio" && (otype == PlayListsNodeContextMenuType.Radio))
                { cm.Items[i].Click += NodeContextMenu_AddRadioCategory; }
                else if (cm.Items[i].Name == "DeleteCategoryRadio" && (otype == PlayListsNodeContextMenuType.RadioCategory))
                { cm.Items[i].Click += NodeContextMenu_DeleteRadioCategory; }
                else if (cm.Items[i].Name == "RenameCategoryRadio" && (otype == PlayListsNodeContextMenuType.RadioCategory))
                { cm.Items[i].Click += NodeContextMenu_RenameCategoryRadio; }
                // Radio Node
                else if (cm.Items[i].Name == "AddRadio" && (otype == PlayListsNodeContextMenuType.Radio || otype == PlayListsNodeContextMenuType.RadioCategory))
                { cm.Items[i].Click += NodeContextMenu_AddRadio; }
                else if (cm.Items[i].Name == "DeleteRadio" && (otype == PlayListsNodeContextMenuType.RadioNode))
                { cm.Items[i].Click += NodeContextMenu_DeleteRadio; }
                else if (cm.Items[i].Name == "EditRadio" && (otype == PlayListsNodeContextMenuType.RadioNode))
                { cm.Items[i].Click += NodeContextMenu_EditRadio; }
                // Hide all items not tested
                else { cm.Items[i].Visible = false; cm.Items[i].Tag = null; }
            }

            cm.Tag = parent;
            return cm;
        }

        private void NodeContextMenu_Play(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { Debug.WriteLine("ER 0"); return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

            if (data.Item2 == PlayListsNodeContextMenuType.Auto)
            {
                AutoPlaylistTypes playlistType = AutoPlaylistTypes.LastImports;
                if (data.Item1.Index == 0) { playlistType = AutoPlaylistTypes.LastImports; }
                if (data.Item1.Index == 1) { playlistType = AutoPlaylistTypes.MostPlayed; }
                if (data.Item1.Index == 2) { playlistType = AutoPlaylistTypes.MostRecentlyPlayed; }
                if (data.Item1.Index == 3) { playlistType = AutoPlaylistTypes.BestRating; }
                List<PlayListsLineItem> files = autolistData(playlistType, 100);
                List<string> paths = new List<string>();
                foreach (PlayListsLineItem pitem in files) { paths.Add(pitem.Path); }
                Player.StopAll();
                Player.PlaylistClear();
                Player.PlaylistEnqueue(paths.ToArray(), false, 0, 0, true);
            }
            else if (data.Item2 == PlayListsNodeContextMenuType.CustomNode)
            { }
        }
        private void NodeContextMenu_PlayCustom(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { Debug.WriteLine("ER 0"); return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;

            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 1"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 2"); return; }

            OpenCustomPlaylist(data, true);
        }

        private void NodeContextMenu_PlayShuffled(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { Debug.WriteLine("ER 0"); return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

            if (data.Item2 == PlayListsNodeContextMenuType.Auto)
            {
                AutoPlaylistTypes playlistType = AutoPlaylistTypes.LastImports;
                if (data.Item1.Index == 0) { playlistType = AutoPlaylistTypes.LastImports; }
                if (data.Item1.Index == 1) { playlistType = AutoPlaylistTypes.MostPlayed; }
                if (data.Item1.Index == 2) { playlistType = AutoPlaylistTypes.MostRecentlyPlayed; }
                if (data.Item1.Index == 3) { playlistType = AutoPlaylistTypes.BestRating; }
                List<PlayListsLineItem> files = autolistData(playlistType, 100);
                List<string> paths = new List<string>();
                foreach (PlayListsLineItem pitem in files) { paths.Add(pitem.Path); }
                Player.StopAll();
                Player.PlaylistClear();
                Player.PlaylistEnqueue(paths.ToArray(), true, 0, 0, true);
            }
            else if (data.Item2 == PlayListsNodeContextMenuType.CustomNode)
            { }
        }
        private void NodeContextMenu_PlayShuffledCustom(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { Debug.WriteLine("ER 0"); return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;

            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 1"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 2"); return; }

            OpenCustomPlaylist(data, true, true);
        }

        private void NodeContextMenu_DeleteCustom(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { Debug.WriteLine("ER 0"); return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;

            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 1"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 2"); return; }
            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT FIndex,Name,Description FROM playlists WHERE FIndex=" + data, "FIndex");
            if (rez == null) { Debug.WriteLine("ER 3"); return; }
            string[] keys = rez.Keys.ToArray();

            if (DialogBox.ShowDialog(App.GetTranslation("PlayListsContextMenuPlayListDeleteConfirmTitle"), 
                App.GetTranslation("PlayListsContextMenuPlayListDeleteConfirmMessage").Replace("%X%", "" + rez[keys[0]]["Name"]), 
                DialogBoxButtons.YesNo, DialogBoxIcons.Warning, this.Parent))
            {
                App.bdd.DatabaseQuery("DELETE FROM playlists WHERE FIndex = " + data, null, true);
                App.bdd.DatabaseQuery("DELETE FROM playlistsItems WHERE LIndex = " + data, null, true);
                App.win1.playLists.Init();
            }
        }
        private void NodeContextMenu_RenameCustom(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { Debug.WriteLine("ER 1"); return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

            int data = 0;
            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 2"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 3"); return; }
            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT FIndex,Name,Description FROM playlists WHERE FIndex="+data, "FIndex");
            if (rez == null) { Debug.WriteLine("ER 4"); return; }
            string[] keys = rez.Keys.ToArray();
            PlayListItem itemx = new PlayListItem() { FIndex = data, Name = "" + rez[keys[0]]["Name"], Description = "" + rez[keys[0]]["Description"] };
            EditPlayList epl = new EditPlayList(this.Parent, itemx) { StartPosition = FormStartPosition.CenterParent };
            epl.ShowDialog();
        }
        private void NodeContextMenu_AddCustom(object sender, EventArgs e)
        {
            EditPlayList epl = new EditPlayList(this.Parent) { StartPosition = FormStartPosition.CenterParent };
            epl.ShowDialog();
        }
        
        private void NodeContextMenu_PlayRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;
            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 2"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 3"); return; }
            App.win1.playLists.PlayRadioNode(data);
        }
        private void NodeContextMenu_AddRadioCategory(object sender, EventArgs e)
        {
            EditRadioCategory erc = new EditRadioCategory(this.Parent);
            erc.ShowDialog();
        }
        private void NodeContextMenu_DeleteRadioCategory(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;
            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 2"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 3"); return; }

            WebRadioCategoryItem catData = App.win1.playLists.getRadioCategoryInfo(data, true);

            if (DialogBox.ShowDialog(App.GetTranslation("PlayListsContextMenuPlayListDeleteRadioCategoryConfirmTitle"),
                App.GetTranslation("PlayListsContextMenuPlayListDeleteRadioCategoryConfirmMessage").Replace("%X%", catData.Name),
                DialogBoxButtons.YesNo, DialogBoxIcons.Warning, this.Parent))
            {
                App.bdd.DatabaseQuery("DELETE FROM radiosCategories WHERE CRID = " + data, null, true);
                App.bdd.DatabaseQuery("DELETE FROM radios WHERE Category = " + data, null, true);
                App.win1.playLists.Init();
            }
        }
        private void NodeContextMenu_RenameCategoryRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;
            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 2"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 3"); return; }

            WebRadioCategoryItem catData = App.win1.playLists.getRadioCategoryInfo(data, true);

            EditRadioCategory erc = new EditRadioCategory(this.Parent, catData);
            erc.ShowDialog();
        }
        private void NodeContextMenu_AddRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;
            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { }

            EditRadio er = new EditRadio(this.Parent, null, data);
            er.ShowDialog();
        }
        private void NodeContextMenu_DeleteRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;
            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 2"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 3"); return; }
            WebRadioItem radio = App.win1.playLists.getRadioInfo(data);
            if (radio == null) { return; }

            if (DialogBox.ShowDialog(App.GetTranslation("PlayListsContextMenuPlayListDeleteRadioConfirmTitle", "ERROR"),
                App.GetTranslation("PlayListsContextMenuPlayListDeleteRadioConfirmMessage", "ERROR").Replace("%X%", radio.Name),
                DialogBoxButtons.YesNo, DialogBoxIcons.Warning, this.Parent))
            {
                App.bdd.DatabaseQuery("DELETE FROM radios WHERE RID = " + data, null, true);
                App.win1.playLists.Init();
            }
        }
        private void NodeContextMenu_EditRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) rawdata = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;
            int data = 0;
            try { data = (int)rawdata.Item1.Tag; } catch (Exception ex) { Debug.WriteLine("ER 2"); Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); return; }
            if (data <= 0) { Debug.WriteLine("ER 3"); return; }
            WebRadioItem radio = App.win1.playLists.getRadioInfo(data);
            if (radio == null) { return; }
            EditRadio er = new EditRadio(this.Parent, radio);
            er.ShowDialog();
        }

        private void CM_RemoveTrackSelection(object sender, EventArgs e)
        {
            //TreeViewItem TItem = (Parent.PlaylistsTree.SelectedItem != null) ? (TreeViewItem)Parent.PlaylistsTree.SelectedItem : null;
            //if (TItem == null) { return; }
            //if (!TItem.Name.StartsWith("user_")) { return; }

            //if (Parent.PlaylistsContents.SelectedItems.Count == 0) { return; }

            //bool ret = DialogBox.ShowDialog(
            //    Parent.FindResource("PlayListsContextMenuPlayListDeleteConfirmTitle") as string,
            //    (Parent.PlaylistsContents.SelectedItems.Count > 1) ?
            //        Parent.FindResource("PlayListsContextMenuTrackDeleteConfirmMessage2") as string :
            //        (Parent.FindResource("PlayListsContextMenuTrackDeleteConfirmMessage") as string).Replace("%X%", ((MediaItem)Parent.PlaylistsContents.SelectedItem).Name),
            //    DialogBoxButtons.YesNo,
            //    DialogBoxIcons.Warning,
            //    );

            //if (ret == true)
            //{
            //    List<string> querys = new List<string>();
            //    foreach (MediaItem row in Parent.PlaylistsContents.SelectedItems)
            //    {
            //        querys.Add("DELETE FROM playlistsItems WHERE LIndex = '" + ((string)TItem.Tag) + "' AND Path = '" + Database.EscapeString(row.Path) + "'");
            //    }
            //    Parent.bdd.DatabaseQuerys(querys.ToArray(), true);
            //    userlistClick(TItem, null);
            //}
        }

        private ContextMenuStrip MakeCellContextMenu(Control parent)
        {
            ContextMenuStrip cm = new PlayListsCellContextMenu();
            for (int i = 0; i < cm.Items.Count; i++)
            {
                cm.Items[i].Tag = parent;
                if (cm.Items[i].Name == "PlayTrack") { cm.Items[i].Click += PlayTrack_Click; }
                else if (cm.Items[i].Name == "RemoveTrack") { cm.Items[i].Click += RemoveTrack_Click; ; }
            }

            return cm;
        }

        private void RemoveTrack_Click(object sender, EventArgs e)
        {
            ToolStripItem parentItem = (ToolStripItem)sender;
            if (parentItem.Tag == null) { return; }
            Control parent = (Control)parentItem.Tag;
            PlayListsLineItem item = (PlayListsLineItem)parent.Tag;
            if(item.PlaylistId <= 0) { return; }

            if (DialogBox.ShowDialog(App.GetTranslation("PlayListsContextMenuTrackDeleteConfirmTitle", "ERROR"),
                App.GetTranslation("PlayListsContextMenuTrackDeleteConfirmMessage", "ERROR").Replace("%X%", item.Name),
                DialogBoxButtons.YesNo, DialogBoxIcons.Warning, this.Parent))
            {
                App.bdd.DatabaseQuery("DELETE FROM playlistsItems WHERE PIndex = " + item.PlaylistItemId, null, true);
                App.win1.playLists.Init();
            }
        }

        private void PlayTrack_Click(object sender, EventArgs e)
        {
            ToolStripItem parentItem = (ToolStripItem)sender;
            if (parentItem.Tag == null) { return; }
            Control parent = (Control)parentItem.Tag;
            PlayListsLineItem item = (PlayListsLineItem)parent.Tag;

            Player.StopAll();
            Player.PlaylistClear();
            Player.PlaylistEnqueue(new string[] { item.Path }, false, 0, 0, true);
        }
    }

    public enum PlayListsNodeContextMenuType { Auto, Custom, Radio, CustomNode, RadioCategory, RadioNode }

    public class PlayListsNodeContextMenu : ContextMenuStrip
    {
        private System.Drawing.Color _ForeColor = System.Drawing.Color.Black;
        new public System.Drawing.Color ForeColor
        {
            get { return _ForeColor; }
            set { base.ForeColor = _ForeColor = value; Update(); }
        }
        private System.Drawing.Color _BackColor = System.Drawing.Color.Black;
        new public System.Drawing.Color BackColor
        {
            get { return _BackColor; }
            set { base.BackColor = _BackColor = value; Update(); }
        }
        private SolidColorBrush DefaultBrush = new SolidColorBrush(Colors.White);
        private int ButtonIconSize = 32;


        // Items
        public ToolStripItem PlayListPlay = null; // For Auto & Custom
        public ToolStripItem PlayListPlayShuffled = null; // For Auto & Custom

        public ToolStripItem PlayListCustomDelete = null;
        public ToolStripItem PlayListCustomAdd = null;
        public ToolStripItem PlayListCustomRename = null;

        public ToolStripItem PlayRadio = null;

        public ToolStripItem AddCategoryRadio = null;
        public ToolStripItem DeleteCategoryRadio = null;
        public ToolStripItem RenameCategoryRadio = null;

        public ToolStripItem AddRadio = null;
        public ToolStripItem DeleteRadio = null;
        public ToolStripItem EditRadio = null;

        public PlayListsNodeContextMenu()
        {
            base.BackColor = _BackColor = App.style.GetColor("ContextMenuBackColor");
            base.ForeColor = _ForeColor = App.style.GetColor("ContextMenuForeColor");

            Font = new Font("Segoe UI", 15, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
            RenderMode = ToolStripRenderMode.System;

            // For Auto & Custom
            PlayListPlay = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListPlay"), Icons.FromIconKind(IconKind.PlaylistPlay, ButtonIconSize, DefaultBrush));
            PlayListPlayShuffled = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListPlayShuffled"), Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush));
            
            PlayListCustomDelete = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListDeleteCustom"), Icons.FromIconKind(IconKind.PlaylistRemove, ButtonIconSize, DefaultBrush));
            PlayListCustomAdd = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListAddCustom"), Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush));
            PlayListCustomRename = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListRenameCustom"), Icons.FromIconKind(IconKind.PlaylistEdit, ButtonIconSize, DefaultBrush));

            //FolderEdit
            PlayRadio = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListPlayRadio"), Icons.FromIconKind(IconKind.Play, ButtonIconSize, DefaultBrush));
            
            AddCategoryRadio = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListAddRadioCategory"), Icons.FromIconKind(IconKind.FolderPlus, ButtonIconSize, DefaultBrush));
            DeleteCategoryRadio = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListDeleteRadioCategory"), Icons.FromIconKind(IconKind.FolderRemove, ButtonIconSize, DefaultBrush));
            RenameCategoryRadio = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListRenameRadioCategory"), Icons.FromIconKind(IconKind.FolderEdit, ButtonIconSize, DefaultBrush));
            
            AddRadio = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListAddRadio"), Icons.FromIconKind(IconKind.Radio, ButtonIconSize, DefaultBrush));
            DeleteRadio = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListDeleteRadio"), Icons.FromIconKind(IconKind.RadioOff, ButtonIconSize, DefaultBrush));
            EditRadio = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListEditRadio"), Icons.FromIconKind(IconKind.Pencil, ButtonIconSize, DefaultBrush));

            PlayListPlay.Name = nameof(PlayListPlay);
            PlayListPlayShuffled.Name = nameof(PlayListPlayShuffled);
            PlayListCustomDelete.Name = nameof(PlayListCustomDelete);
            PlayListCustomAdd.Name = nameof(PlayListCustomAdd);
            PlayListCustomRename.Name = nameof(PlayListCustomRename);
            PlayRadio.Name = nameof(PlayRadio);
            AddCategoryRadio.Name = nameof(AddCategoryRadio);
            DeleteCategoryRadio.Name = nameof(DeleteCategoryRadio);
            RenameCategoryRadio.Name = nameof(RenameCategoryRadio);
            AddRadio.Name = nameof(AddRadio);
            DeleteRadio.Name = nameof(DeleteRadio);
            EditRadio.Name = nameof(EditRadio);
        }

        public void Update()
        {
            DefaultBrush = new SolidColorBrush(App.DrawingColorToMediaColor(_ForeColor));

            // For Auto & Custom
            PlayListPlay.Text = App.GetTranslation("PlayListsContextMenuPlayListPlay"); 
            PlayListPlay.Image = Icons.FromIconKind(IconKind.PlaylistPlay, ButtonIconSize, DefaultBrush);
            PlayListPlayShuffled.Text = App.GetTranslation("PlayListsContextMenuPlayListPlayShuffled");
            PlayListPlayShuffled.Image = Icons.FromIconKind(IconKind.BowlMix, ButtonIconSize, DefaultBrush);

            PlayListCustomDelete.Text = App.GetTranslation("PlayListsContextMenuPlayListDeleteCustom");
            PlayListCustomDelete.Image = Icons.FromIconKind(IconKind.PlaylistRemove, ButtonIconSize, DefaultBrush);
            PlayListCustomAdd.Text = App.GetTranslation("PlayListsContextMenuPlayListAddCustom");
            PlayListCustomAdd.Image = Icons.FromIconKind(IconKind.PlaylistPlus, ButtonIconSize, DefaultBrush);
            PlayListCustomRename.Text = App.GetTranslation("PlayListsContextMenuPlayListRenameCustom");
            PlayListCustomRename.Image = Icons.FromIconKind(IconKind.PlaylistEdit, ButtonIconSize, DefaultBrush);

            PlayRadio.Text = App.GetTranslation("PlayListsContextMenuPlayListPlayRadio");
            PlayRadio.Image = Icons.FromIconKind(IconKind.Play, ButtonIconSize, DefaultBrush);

            AddCategoryRadio.Text = App.GetTranslation("PlayListsContextMenuPlayListRadioAddCategory");
            AddCategoryRadio.Image = Icons.FromIconKind(IconKind.FolderPlus, ButtonIconSize, DefaultBrush);

            DeleteCategoryRadio.Text = App.GetTranslation("PlayListsContextMenuPlayListDeleteRadioCategory");
            DeleteCategoryRadio.Image = Icons.FromIconKind(IconKind.FolderRemove, ButtonIconSize, DefaultBrush);

            RenameCategoryRadio.Text = App.GetTranslation("PlayListsContextMenuPlayListRenameRadioCategory");
            RenameCategoryRadio.Image = Icons.FromIconKind(IconKind.FolderEdit, ButtonIconSize, DefaultBrush);

            AddRadio.Text = App.GetTranslation("PlayListsContextMenuPlayListAddRadio");
            AddRadio.Image = Icons.FromIconKind(IconKind.Radio, ButtonIconSize, DefaultBrush);

            DeleteRadio.Text = App.GetTranslation("PlayListsContextMenuPlayListDeleteRadio");
            DeleteRadio.Image = Icons.FromIconKind(IconKind.RadioOff, ButtonIconSize, DefaultBrush);

            EditRadio.Text = App.GetTranslation("PlayListsContextMenuPlayListEditRadio");
            EditRadio.Image = Icons.FromIconKind(IconKind.Pencil, ButtonIconSize, DefaultBrush);
        }
    }

    public class PlayListsCellContextMenu : ContextMenuStrip
    {
        private System.Drawing.Color _ForeColor = System.Drawing.Color.Black;
        new public System.Drawing.Color ForeColor
        {
            get { return _ForeColor; }
            set { base.ForeColor = _ForeColor = value; Update(); }
        }
        private System.Drawing.Color _BackColor = System.Drawing.Color.Black;
        new public System.Drawing.Color BackColor
        {
            get { return _BackColor; }
            set { base.BackColor = _BackColor = value; Update(); }
        }
        private SolidColorBrush DefaultBrush = new SolidColorBrush(Colors.White);
        private int ButtonIconSize = 32;


        // Items
        public ToolStripItem PlayTrack = null;
        public ToolStripItem RemoveTrack = null;

        public PlayListsCellContextMenu()
        {
            base.BackColor = _BackColor = App.style.GetColor("ContextMenuBackColor");
            base.ForeColor = _ForeColor = App.style.GetColor("ContextMenuForeColor");

            Font = new Font("Segoe UI", 15, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
            RenderMode = ToolStripRenderMode.System;

            // For Auto & Custom
            PlayTrack = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListPlayTrack"), Icons.FromIconKind(IconKind.Play, ButtonIconSize, DefaultBrush));

            RemoveTrack = Items.Add(App.GetTranslation("PlayListsContextMenuTrackDelete"), Icons.FromIconKind(IconKind.Delete, ButtonIconSize, DefaultBrush));

            PlayTrack.Name = nameof(PlayTrack);
            RemoveTrack.Name = nameof(RemoveTrack);
        }

        public void Update()
        {
            DefaultBrush = new SolidColorBrush(App.DrawingColorToMediaColor(_ForeColor));

            PlayTrack.Text = App.GetTranslation("PlayListsContextMenuPlayListPlayTrack");
            PlayTrack.Image = Icons.FromIconKind(IconKind.Play, ButtonIconSize, DefaultBrush);

            RemoveTrack.Text = App.GetTranslation("PlayListsContextMenuTrackDelete");
            RemoveTrack.Image = Icons.FromIconKind(IconKind.Delete, ButtonIconSize, DefaultBrush);
        }
    }
}
