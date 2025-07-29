using System;
using System.Collections.Generic;
using AnotherMusicPlayer.MainWindow2Space;
using System.Windows.Forms;
using System.Linq;
using Button = System.Windows.Forms.Button;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Diagnostics;

namespace AnotherMusicPlayer
{
    public partial class PlayLists
    {
        private MainWindow2 Parent;
        private bool isBuild = false; 
        private List<string> autoList = new List<string>() { "lastImports", "mostPlayed", "mostRecentlyPlayed", "bestRating" };
        bool IsAuto = false;
        bool IsCustom = false;
        bool IsRadio = false;
        private ContextMenuStrip DataGridViewContextMenu = null;

        public PlayLists(MainWindow2 parent)
        {
            if (parent == null) { return; }
            Parent = parent;
            isBuild = true;
            Parent.PlaylistsTree.Nodes[0].ContextMenuStrip = null;
            Parent.PlaylistsTree.Nodes[1].ContextMenuStrip = MakeNodeContextMenu(Parent.PlaylistsTree.Nodes[1], PlayListsNodeContextMenuType.Custom);
            Parent.PlaylistsTree.Nodes[2].ContextMenuStrip = MakeNodeContextMenu(Parent.PlaylistsTree.Nodes[2], PlayListsNodeContextMenuType.Radio);
            Parent.PlaylistsTree.NodeMouseClick += PlaylistsTree_NodeMouseClick;
            Parent.PlaylistsTree.NodeMouseDoubleClick += PlaylistsTree_NodeMouseDoubleClick;
            Parent.PlayListsTabRadioButton.Click += PlayListsTabRadioButton_Click;
            Parent.PlayListsTabDataGridView.CellMouseClick += PlayListsTabDataGridView_CellMouseClick;
            Parent.PlayListsTabDataGridView.CellMouseDoubleClick += PlayListsTabDataGridView_CellMouseDoubleClick;

            DataGridViewContextMenu = MakeCellContextMenu(Parent.PlayListsTabDataGridView);

            Init();
        }

        private void PlayListsTabDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Parent.PlayListsTabDataGridView.Rows.Count) { return; }
            PlayListsLineItem item = (PlayListsLineItem)Parent.PlayListsTabDataGridView.Rows[e.RowIndex].DataBoundItem;

            Player.StopAll();
            Player.PlaylistClear();
            Player.PlaylistEnqueue(new string[] { item.Path }, false, 0, 0, true);
        }

        private void PlayListsTabDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!IsCustom) { return; }
            if (e.RowIndex < 0 || e.RowIndex >= Parent.PlayListsTabDataGridView.Rows.Count) { return; }

            if (e.Button == MouseButtons.Right)
            {
                PlayListsLineItem item = (PlayListsLineItem)Parent.PlayListsTabDataGridView.Rows[e.RowIndex].DataBoundItem;
                Parent.PlayListsTabDataGridView.Tag = item;
                DataGridViewContextMenu.Show(System.Windows.Forms.Cursor.Position);
            }
        }

        private void PlayListsTabRadioButton_Click(object sender, EventArgs e)
        {
            if (sender == null) { return; }
            if (sender.GetType() != typeof(Button)) { return; }
            Button btn = (Button)sender;
            if (btn.Tag == null) { Debug.WriteLine("TAG IS NULL"); return; }
            if (btn.Tag.GetType() != typeof(int)) { Debug.WriteLine("TAG Invalid Type ("+ btn.Tag.GetType().FullName+ ")"); return; }
            PlayRadioNode((int)btn.Tag);
        }

        #region Display Panel functions
        private void DisplayVoidPanel() {
            Parent.PlayListsTabSplitContainer1.Panel1Collapsed = false;
            Parent.PlayListsTabSplitContainer1.Panel2Collapsed = true;
            Parent.PlayListsTabSplitContainer2.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer2.Panel2Collapsed = true; 
            IsAuto = false;
            IsCustom = false;
            IsRadio = false;
        }
        private void DisplayListPanel() {
            Parent.PlayListsTabSplitContainer1.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer1.Panel2Collapsed = false;
            Parent.PlayListsTabSplitContainer2.Panel1Collapsed = false;
            Parent.PlayListsTabSplitContainer2.Panel2Collapsed = true;
            IsAuto = true;
            IsCustom = true;
            IsRadio = false;
        }
        private void DisplayRadioPanelRadio() {
            Parent.PlayListsTabSplitContainer1.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer1.Panel2Collapsed = false;
            Parent.PlayListsTabSplitContainer2.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer2.Panel2Collapsed = false;
            Parent.PlayListsTabRadioPanelFlowLayoutPanel.Visible = false;
            Parent.PlayListsTabRadioButton.Visible = true;
            IsAuto = false;
            IsCustom = false;
            IsRadio = true;
        }
        private void DisplayRadioPanelCategory() {
            Parent.PlayListsTabSplitContainer1.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer1.Panel2Collapsed = false;
            Parent.PlayListsTabSplitContainer2.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer2.Panel2Collapsed = false;
            Parent.PlayListsTabRadioButton.Visible = false;
            IsAuto = false;
            IsCustom = false;
            IsRadio = true;
        }
        #endregion

        public void Init()
        {
            if (Parent.PlaylistsTree.InvokeRequired) { Parent.PlaylistsTree.Invoke(() => { Init(); }); return; }
            Parent.PlaylistsTree.Nodes[0].Nodes.Clear(); Parent.PlaylistsTree.Nodes[1].Nodes.Clear(); Parent.PlaylistsTree.Nodes[2].Nodes.Clear();
            DisplayVoidPanel();
            Parent.PlayListsTabDataGridView.AutoGenerateColumns = false;

            foreach (string archetype in autoList)
            {
                TreeNode item = new TreeNode()
                {
                    Text = App.GetTranslation("PlayListsAuto_" + archetype),
                    Tag = "auto_" + archetype,
                    Name = "auto_" + archetype
                };
                item.ContextMenuStrip = MakeNodeContextMenu(item, PlayListsNodeContextMenuType.Auto);
                Parent.PlaylistsTree.Nodes[0].Nodes.Add(item);
            }

            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT FIndex,Name,Description FROM playlists ORDER BY LOWER(Name) ASC", "FIndex");
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
            {
                TreeNode item = new TreeNode() { Text = "" + row.Value["Name"], Tag = int.Parse("" + row.Value["FIndex"]), ToolTipText = "" + row.Value["Description"] };
                item.ContextMenuStrip = MakeNodeContextMenu(item, PlayListsNodeContextMenuType.CustomNode);
                Parent.PlaylistsTree.Nodes[1].Nodes.Add(item);
            }

            Dictionary<string, TreeNode> radioCats = new Dictionary<string, TreeNode>();
            Dictionary<string, Dictionary<string, object>> rez2 = App.bdd.DatabaseQuery(
                "SELECT radios.RID,radios.Category,radios.Name,radios.Url,radios.UrlPrefix,radios.FType,radios.Fragmented,radios.Logo,radios.Description, "
                + "radiosCategories.CRID,radiosCategories.Name AS CName, radiosCategories.Description AS CDescription, radiosCategories.Logo AS CLogo FROM radios "
                + "LEFT JOIN radiosCategories ON(radios.Category = radiosCategories.CRID) "
                + "ORDER BY radios.Category ASC, LOWER(radios.Name) ASC", "RID");
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez2)
            {
                int CRID = int.Parse("" + row.Value["Category"]);
                string CName = "" + row.Value["CName"];

                TreeNode item = new TreeNode() {
                    Text = "" + row.Value["Name"], Tag = int.Parse("" + row.Value["RID"]), ToolTipText = "" + row.Value["Description"],
                    ImageKey = "radio_icon.png", SelectedImageKey = "radio_icon.png"
                };
                item.ContextMenuStrip = MakeNodeContextMenu(item, PlayListsNodeContextMenuType.RadioNode);

                if (CRID > 0) {
                    if (!radioCats.ContainsKey(CName))
                    {
                        TreeNode itemCat = new TreeNode() {
                            Text = CName, Tag = int.Parse("" + row.Value["CRID"]), ToolTipText = "" + row.Value["CDescription"],
                            ImageKey = "folder_open_trimed.png", SelectedImageKey = "folder_open_trimed.png"
                        };
                        itemCat.ContextMenuStrip = MakeNodeContextMenu(itemCat, PlayListsNodeContextMenuType.RadioCategory);
                        itemCat.Nodes.Add(item);
                        radioCats.Add(CName, itemCat);
                        Parent.PlaylistsTree.Nodes[2].Nodes.Add(itemCat);
                    }
                    else {
                        TreeNode itemCat = radioCats[CName];
                        itemCat.Nodes.Add(item);
                        Parent.PlaylistsTree.Nodes[2].Nodes.Add(itemCat);
                    }
                }
                else { Parent.PlaylistsTree.Nodes[2].Nodes.Add(item); }
            }
            Dictionary<string, Dictionary<string, object>> rez3 = App.bdd.DatabaseQuery("SELECT * FROM radiosCategories ORDER BY LOWER(Name) ASC", "CRID");
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez3)
            {
                string Name = "" + row.Value["Name"];
                int CRID = int.Parse("0" + row.Value["CRID"]);
                Debug.WriteLine(Name);
                if (!radioCats.ContainsKey(Name))
                {
                    Debug.WriteLine("RRRRRrrrrrrrrrrrr");
                    TreeNode itemCat = new TreeNode()
                    {
                        Text = Name,
                        Tag = CRID,
                        ToolTipText = "" + row.Value["Description"],
                        ImageKey = "folder_open_trimed.png",
                        SelectedImageKey = "folder_open_trimed.png"
                    };
                    itemCat.ContextMenuStrip = MakeNodeContextMenu(itemCat, PlayListsNodeContextMenuType.RadioCategory);
                    Parent.PlaylistsTree.Nodes[2].Nodes.Add(itemCat);
                }
            }
            radioCats.Clear();

            Parent.PlaylistsTree.Nodes[0].Expand();
            Parent.PlaylistsTree.Nodes[1].Expand();
            Parent.PlaylistsTree.Nodes[2].Expand();
        }

        private void PlaylistsTree_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            TreeNode parent = e.Node.Parent;
            if (e.Node.Level > 0) { while (parent.Parent != null) { parent = parent.Parent; } }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.Node.Level == 0) { DisplayVoidPanel(); }
                else
                {
                    if (parent.Index == 2)
                    {
                        if (e.Node.Level > 0) {
                            int id = 0;
                            if (e.Node.Tag == null) { return; }
                            if (e.Node.Tag.GetType() == typeof(int)) { id = (int)e.Node.Tag; }
                            if (id == 0) { return; }
                            if (e.Node.ImageKey == "radio_icon.png")// RadioNode
                            { DisplayRadioNode(id); }
                            else if (e.Node.ImageKey == "folder_open_trimed.png")// CategoryNode
                            { DisplayRadioCategory(id); }
                        }
                    }
                    else if (parent.Index == 0)
                    {
                        if (e.Node.Index == 0) { OpenAutoPlaylist(AutoPlaylistTypes.LastImports); }
                        else if (e.Node.Index == 1) { OpenAutoPlaylist(AutoPlaylistTypes.MostPlayed); }
                        else if (e.Node.Index == 2) { OpenAutoPlaylist(AutoPlaylistTypes.MostRecentlyPlayed); }
                        else if (e.Node.Index == 3) { OpenAutoPlaylist(AutoPlaylistTypes.BestRating); }
                        else { DisplayVoidPanel(); }
                    }
                    else if (parent.Index == 1)
                    {
                        int playlistId = (int)e.Node.Tag;
                        OpenCustomPlaylist(playlistId);
                    }
                    else { DisplayVoidPanel(); }
                }
            }
        }

        private void PlaylistsTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0) { return; }
            TreeNode parent = e.Node.Parent;
            while (parent.Parent != null) { parent = parent.Parent; }

            if (parent.Index == 0)
            {
                AutoPlaylistTypes playlistType = AutoPlaylistTypes.LastImports;
                if (e.Node.Index == 0) { playlistType = AutoPlaylistTypes.LastImports; }
                if (e.Node.Index == 1) { playlistType = AutoPlaylistTypes.MostPlayed; }
                if (e.Node.Index == 2) { playlistType = AutoPlaylistTypes.MostRecentlyPlayed; }
                if (e.Node.Index == 3) { playlistType = AutoPlaylistTypes.BestRating; }
                List<PlayListsLineItem> files = autolistData(playlistType, 100);
                List<string> paths = new List<string>();
                foreach (PlayListsLineItem item in files) { paths.Add(item.Path); }
                Player.StopAll();
                Player.PlaylistClear();
                Player.PlaylistEnqueue(paths.ToArray(), false, 0, 0, true);
            }
            else if (parent.Index == 2)
            {
                if (e.Node.ImageKey != "radio_icon.png") { return; }
                int id = 0;
                if (e.Node.Tag == null) { return; }
                if (e.Node.Tag.GetType() == typeof(int)) { id = (int)e.Node.Tag; }
                if (id == 0) { return; }
                PlayRadioNode(id);
            }
            else if (parent.Index == 1)
            {
                int playlistId = (int)e.Node.Tag;
                OpenCustomPlaylist(playlistId, true);
            }
        }

        private List<PlayListsLineItem> autolistData(AutoPlaylistTypes playlistType, int maxLimit = 50)
        {
            string query = "";
            if (playlistType == AutoPlaylistTypes.LastImports) { query = "SELECT * FROM files ORDER BY InsertionDate DESC, LastUpdate DESC LIMIT " + maxLimit; }
            else if (playlistType == AutoPlaylistTypes.MostPlayed) { query = "SELECT files.*,playCounts.Cpt FROM files JOIN playCounts ON(playCounts.Path = files.Path) WHERE files.Path in (SELECT Path FROM playCounts ORDER BY Cpt DESC, LastPlay DESC LIMIT "+ maxLimit + ") ORDER BY playCounts.Cpt DESC, playCounts.LastPlay DESC"; }
            else if (playlistType == AutoPlaylistTypes.MostRecentlyPlayed) { query = "SELECT files.*,playCounts.Cpt FROM files JOIN playCounts ON(playCounts.Path = files.Path) WHERE files.Path in (SELECT Path FROM playCounts ORDER BY LastPlay DESC LIMIT " + maxLimit + ") ORDER BY playCounts.LastPlay DESC"; }
            if (playlistType == AutoPlaylistTypes.BestRating) { query = "SELECT * FROM files ORDER BY REPLACE(Rating, '.', ',') DESC LIMIT " + maxLimit; }

            List<PlayListsLineItem> files = new List<PlayListsLineItem>();
            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery(query, "Path");
            return ParseFilesQueryDate(rez, (playlistType == AutoPlaylistTypes.MostPlayed || playlistType == AutoPlaylistTypes.MostRecentlyPlayed));
        }

        private List<PlayListsLineItem> ParseFilesQueryDate(Dictionary<string, Dictionary<string, object>> rez, bool ShowColumn0 = false)
        {
            List<PlayListsLineItem> files = new List<PlayListsLineItem>();

            if (rez == null || rez.Count == 0) { return files; }
            foreach (string key in rez.Keys)
            {
                List<string> artists = new List<string>();
                if (rez[key]["Composers"] != null)
                {
                    string[] cl = ((string)rez[key]["Composers"]).Split(';');
                    foreach (string c in cl) { if (c != "" && !artists.Contains(c)) { artists.Add(c); } }
                }
                if (rez[key]["Performers"] != null)
                {
                    string[] cl = ((string)rez[key]["Performers"]).Split(';');
                    foreach (string c in cl) { if (c != "" && !artists.Contains(c)) { artists.Add(c); } }
                }
                if (rez[key]["AlbumArtists"] != null)
                {
                    string[] cl = ((string)rez[key]["AlbumArtists"]).Split(';');
                    foreach (string c in cl) { if (c != "" && !artists.Contains(c)) { artists.Add(c); } }
                }
                long duration = 0;
                if (rez[key]["Duration"] != null) { duration = Convert.ToInt64("" + rez[key]["Duration"]); }
                double rating = 0;
                if (rez[key]["Rating"] != null) { rating = Convert.ToDouble(("" + rez[key]["Rating"]).Replace(".", ",")); }

                int pindex = 0;
                int lindex = 0;
                int lorder = 0;
                if (rez[key].ContainsKey("PIndex")) { pindex = int.Parse("0" + rez[key]["PIndex"]); }
                if (rez[key].ContainsKey("LIndex")) { lindex = int.Parse("0" + rez[key]["LIndex"]); }
                if (rez[key].ContainsKey("LOrder")) { lorder = int.Parse("0" + rez[key]["LOrder"]); }


                files.Add(new PlayListsLineItem(
                    "" + rez[key]["Path"], "" + rez[key]["Name"],
                    (artists.Count > 0) ? string.Join(';', artists) : "", "" + rez[key]["Album"], duration, (float)rating
                    )
                { PlaylistId = lindex, PlaylistItemId = pindex, PlaylistOrder = lorder });
                if (ShowColumn0) { files[files.Count - 1].PlayCount = Convert.ToInt32("" + rez[key]["Cpt"]); }
            }

            return files;

        }

        private void OpenAutoPlaylist(AutoPlaylistTypes playlistType, bool startPlay = false)
        {
            DisplayListPanel();
            IsAuto = true; IsCustom = false;
            Parent.setLoadingState(true);
            Parent.PlayListsTabDataGridView.ReadOnly = false;
            Parent.PlayListsTabDataGridView.DataSource = null;
            Parent.PlayListsTabDataGridView.Invalidate();
            List<PlayListsLineItem> files = autolistData(playlistType, 100);
            if (playlistType == AutoPlaylistTypes.LastImports) { Parent.PlayListsTabDataGridView.Columns[0].Visible = false; }
            else if (playlistType == AutoPlaylistTypes.MostPlayed) { Parent.PlayListsTabDataGridView.Columns[0].Visible = true; }
            else if (playlistType == AutoPlaylistTypes.MostRecentlyPlayed) { Parent.PlayListsTabDataGridView.Columns[0].Visible = true; }
            else if (playlistType == AutoPlaylistTypes.BestRating) { Parent.PlayListsTabDataGridView.Columns[0].Visible = false; }

            if (files == null) { return; }
            if (files.Count > 0) { Parent.PlayListsTabDataGridView.DataSource = files; }
            Parent.PlayListsTabDataGridView.Invalidate();
            Parent.setLoadingState(false);
        }

        private void OpenCustomPlaylist(int playlistId, bool startPlay = false, bool Shuffled = false)
        {
            if (!startPlay)
            {
                DisplayListPanel();
                IsAuto = false; IsCustom = true;
                Parent.setLoadingState(true);
                Parent.PlayListsTabDataGridView.Columns[0].Visible = false;
                Parent.PlayListsTabDataGridView.ReadOnly = false;
                Parent.PlayListsTabDataGridView.DataSource = null;
                Parent.PlayListsTabDataGridView.Invalidate();
            }

            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT PIndex,LIndex,LOrder,files.* FROM playlistsItems JOIN files ON(files.Path = playlistsItems.Path) WHERE LIndex = "+ playlistId + " ORDER BY LOrder ASC, PIndex ASC", "Path");
            List<PlayListsLineItem> files = ParseFilesQueryDate(rez, false);
            foreach (PlayListsLineItem item in files) { item.PlaylistId = playlistId; }
            if (files.Count > 0) { 
                Parent.PlayListsTabDataGridView.DataSource = files;
                Parent.PlayListsTabDataGridView.Invalidate();
            }
            if (startPlay)
            {
                List<string> paths = new List<string>();
                foreach (PlayListsLineItem item in files) { paths.Add(item.Path); }
                Player.StopAll();
                Player.PlaylistClear();
                Player.PlaylistEnqueue(paths.ToArray(), Shuffled, 0, 0, false);
            }
            Parent.setLoadingState(false); 
        }

        private WebRadioItem getRadioInfo(int radioId) {
            try {
                Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT * FROM radios WHERE RID = "+ radioId, "RID");
                if (rez == null || rez.Count == 0) { return null; }
                WebRadioItem item = new WebRadioItem();
                foreach (string key in rez.Keys)
                {
                    item.RID = int.Parse("0" + rez[key]["RID"]);
                    item.Category = int.Parse("0" + rez[key]["Category"]);
                    item.Fragmented = int.Parse("0" + rez[key]["Fragmented"]);

                    item.Name = "" + rez[key]["Name"];
                    item.Description = "" + rez[key]["Description"];
                    item.Logo = "" + rez[key]["Logo"];
                    if (item.Logo.Trim().Length == 0) { item.Logo = null; }
                    item.Url = "" + rez[key]["Url"];
                    item.UrlPrefix = "" + rez[key]["UrlPrefix"];
                    if (item.UrlPrefix.Trim().Length == 0) { item.UrlPrefix = null; }
                    item.FType = "" + rez[key]["FType"];

                    break;
                }
                return item;
            }
            catch (Exception) {  }
            return null;
        }

        private WebRadioCategoryItem getRadioCategoryInfo(int categoryId, bool childless = false) {
            try {
                Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery(
                    "SELECT radiosCategories.CRID, radiosCategories.Name AS CName, radiosCategories.Description AS CDescription, radiosCategories.Logo AS CLogo,radios.* "
                    + "FROM radiosCategories LEFT JOIN radios ON(CRID = Category) WHERE CRID = " + categoryId, "CRID");
                if (rez == null || rez.Count == 0) { return null; }
                string[] keys = rez.Keys.ToArray();

                WebRadioCategoryItem category = new WebRadioCategoryItem();
                category.CRID = int.Parse("0" + rez[keys[0]]["CRID"]);
                category.Name = "" + rez[keys[0]]["CName"];
                category.Description = "" + rez[keys[0]]["CDescription"];
                category.Logo = ("" + rez[keys[0]]["CLogo"]).Trim();
                if (category.Logo.Length == 0) { category.Logo = null; }

                if (!childless)
                {
                    foreach (string key in rez.Keys)
                    {
                        WebRadioItem item = new WebRadioItem();
                        item.RID = int.Parse("0" + rez[key]["RID"]);
                        if (item.RID == 0) { continue; }
                        item.Category = int.Parse("0" + rez[key]["Category"]);
                        item.Fragmented = int.Parse("0" + rez[key]["Fragmented"]);

                        item.Name = "" + rez[key]["Name"];
                        item.Description = "" + rez[key]["Description"];
                        item.Logo = "" + rez[key]["Logo"];
                        if (item.Logo.Trim().Length == 0) { item.Logo = null; }
                        item.Url = "" + rez[key]["Url"];
                        item.UrlPrefix = "" + rez[key]["UrlPrefix"];
                        if (item.UrlPrefix.Trim().Length == 0) { item.UrlPrefix = null; }
                        item.FType = "" + rez[key]["FType"];
                        category.Childs.Add(item);
                    }
                }
                return category;
            }
            catch (Exception) {  }
            return null;
        }

        private void PlayRadioNode(int radioId) {
            WebRadioItem data = getRadioInfo(radioId);
            if (data == null) { return; }

            Player.StopAll();
            if (data.FType == "Stream")
            { Player.OpenStream(data.Url, RadioPlayer.RadioType.Stream, ""+data.RID, data.Name, true, data.UrlPrefix); }
            else if (data.FType == "M3u")
            { Player.OpenStream(data.Url, RadioPlayer.RadioType.M3u, ""+data.RID, data.Name, true, data.UrlPrefix); }
        }

        private void DisplayRadioNode(int radioId)
        {
            WebRadioItem data = getRadioInfo(radioId);
            if (data == null) { return; }
            Bitmap image = Properties.Resources.radio_icon;
            bool imageFile = false;
            if (data.Logo != null && data.Logo.Trim().Length > 0) { image = BitmapMagic.Base64StringToTrueBitmap(data.Logo); imageFile = true; }
            if (image == null) { image = Properties.Resources.folder_open_icon_white; imageFile = false; }

            Parent.PlayListsTabRadioPanelIcon.BackgroundImage = image;
            Parent.PlayListsTabRadioPanelIcon.BackgroundImageLayout = (imageFile) ? ImageLayout.Zoom : ImageLayout.Center;
            Parent.PlayListsTabRadioPanelTitleLabel.Text = data.Name;
            Parent.PlayListsTabRadioPanelRightPanelDescriptionLabel.Text = data.Description;
            Parent.PlayListsTabRadioButton.Tag = radioId;

            DisplayRadioPanelRadio();
        }

        private void DisplayRadioCategory(int CategoryId) {
            WebRadioCategoryItem data = getRadioCategoryInfo(CategoryId);
            if (data == null) { return; }

            Bitmap image = Properties.Resources.folder_open_icon_white;
            bool imageFile = false;
            if (data.Logo != null && data.Logo.Trim().Length > 0) { image = BitmapMagic.Base64StringToTrueBitmap(data.Logo); imageFile = true; }
            if (image == null) { image = Properties.Resources.folder_open_icon_white; imageFile = false; }

            Parent.PlayListsTabRadioPanelIcon.BackgroundImage = image;
            Parent.PlayListsTabRadioPanelIcon.BackgroundImageLayout = (imageFile) ? ImageLayout.Zoom : ImageLayout.Center;
            Parent.PlayListsTabRadioPanelTitleLabel.Text = data.Name;
            Parent.PlayListsTabRadioPanelRightPanelDescriptionLabel.Text = data.Description;
            DisplayRadioPanelCategory();

            Parent.PlayListsTabRadioPanelFlowLayoutPanel.Controls.Clear();
            Parent.PlayListsTabRadioPanelFlowLayoutPanel.Visible = true;
            foreach (WebRadioItem radio in data.Childs)
            {
                TableLayoutPanel panel = new TableLayoutPanel()
                {
                    MinimumSize = new System.Drawing.Size(100, 130),
                    MaximumSize = new System.Drawing.Size(100, 130),
                    Margin = new Padding(3), RowCount = 1, ColumnCount = 2,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                    Tag = radio.RID, Cursor = Cursors.Hand
                };
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                panel.Disposed += (object sender, EventArgs e) => { if (sender == null) { return; }; App.DelToolTip((Control)sender); };
                Button logoBtn = new Button()
                {
                    MinimumSize = new System.Drawing.Size(98, 98),
                    MaximumSize = new System.Drawing.Size(98, 98),
                    BackColor = System.Drawing.Color.Black, Enabled = false,
                    BackgroundImageLayout = ImageLayout.Zoom, Margin = new Padding(0)
                };
                logoBtn.BackgroundImage = (radio.Logo == null || radio.Logo.Trim().Length == 0) ? Properties.Resources.radio_icon : BitmapMagic.Base64StringToTrueBitmap(radio.Logo);
                panel.Controls.Add(logoBtn, 0, 0);
                Label label = new Label() { 
                    Anchor = AnchorStyles.Left | AnchorStyles.Right, Font = Parent.PlayListsTabRadioPanelRightPanelDescriptionLabel.Font,
                    Text = radio.Name, AutoSize = true, AutoEllipsis = true, Margin = new Padding(0,0,1,0), Cursor = Cursors.Hand
                };
                panel.Controls.Add(label, 0, 1);
                Parent.PlayListsTabRadioPanelFlowLayoutPanel.Controls.Add(panel);
                App.SetToolTip(panel, radio.Name + ((radio.Description.Trim().Length > 0)?" - "+radio.Description:""));
                panel.Click += RadioCategoryRadioPanel_Click;
            }

        }

        private void RadioCategoryRadioPanel_Click(object sender, EventArgs e)
        {
            if (sender == null) { return; }
            if (sender.GetType() != typeof(TableLayoutPanel)) { return; }
            TableLayoutPanel panel = (TableLayoutPanel)sender;
            if (panel.Tag  == null) { return; }
            if (panel.Tag.GetType() != typeof(int)) { return; }
            DisplayRadioNode((int)panel.Tag);
        }
    }
}
