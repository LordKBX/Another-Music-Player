using AnotherMusicPlayer.MainWindow2Space;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;

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

                if (cm.Items[i].Name == "PlayListPlay" && (otype == PlayListsNodeContextMenuType.Auto || otype == PlayListsNodeContextMenuType.CustomNode))
                { cm.Items[i].Click += NodeContextMenu_Play; }
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
            if (item.Tag == null) { return; }
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
        private void NodeContextMenu_DeleteCustom(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_RenameCustom(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_AddCustom(object sender, EventArgs e)
        {

        }
        
        private void NodeContextMenu_PlayRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_AddRadioCategory(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_DeleteRadioCategory(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_RenameCategoryRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_AddRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_DeleteRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }
        private void NodeContextMenu_EditRadio(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            if (item.Tag == null) { return; }
            (TreeNode, PlayListsNodeContextMenuType) data = ((TreeNode, PlayListsNodeContextMenuType))item.Tag;

        }

        private void CM_AddRadio(object sender, EventArgs e)
        {
            Debug.WriteLine(" ==> CM_AddRadio");
            //Dictionary<string, object> data = (Dictionary<string, object>)((TreeViewItem)sender).Tag;
            //Convert.ToInt32(data["Category"])

            AddRadio ar = new AddRadio(/*Parent, */false, 0);
            ar.ShowDialog();
            if (ar.Saved) { Init(); }
        }

        private void CM_AddCategoryRadio(object sender, EventArgs e)
        {
            Debug.WriteLine(" ==> CM_AddCategoryRadio");
            AddRadio ar = new AddRadio(/*Parent, */true);
            ar.ShowDialog();
            if (ar.Saved) { Init(); }
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

        private void CM_DeleteList(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            //string list_id = (string)((TreeViewItem)item.Tag).Tag;
            //string list_name = ((TextBlock)((TreeViewItem)item.Tag).Header).Text;

            //if (list_id.StartsWith("auto_")) { return; }
            //int id = Convert.ToInt32(list_id);

            //bool ret = DialogBox.ShowDialog(
            //    App.GetTranslation("PlayListsContextMenuPlayListDeleteConfirmTitle") as string,
            //    (App.GetTranslation("PlayListsContextMenuPlayListDeleteConfirmMessage") as string).Replace("%X%", list_name),
            //    DialogBoxButtons.YesNo,
            //    DialogBoxIcons.Warning, Parent
            //    );

            //if (ret == true)
            //{
            //    App.bdd.DatabaseQuerys(new string[] { "DELETE FROM playlists WHERE FIndex = " + id, "DELETE FROM playlistsItems WHERE LIndex = " + id }, true);
            //    App.win1.playLists.Init();
            //}
        }

        #region ContextMenu Add Playlist functions
        private void CM_AddTrack(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            //string track = (string)((Button)item.Tag).Tag;
            //Player.PlaylistEnqueue(new string[] { track });
        }
        #endregion
    }

    public enum PlayListsNodeContextMenuType { Auto, Custom, Radio, CustomNode, RadioCategory, RadioNode }

    /// <summary>
    /// Logique d'interaction pour ContextMenu.xaml
    /// </summary>
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
            
            PlayListCustomDelete = Items.Add(App.GetTranslation("PlayListsContextMenuPlayListDelete"), Icons.FromIconKind(IconKind.PlaylistRemove, ButtonIconSize, DefaultBrush));
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

            PlayListCustomDelete.Text = App.GetTranslation("PlayListsContextMenuPlayListDelete");
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
}
