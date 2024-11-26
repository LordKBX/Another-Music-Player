using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Media;
using GongSolutions.Wpf.DragDrop;
using NAudio.Wave;
using m3uParser;
using m3uParser.Model;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using AnotherMusicPlayer.MainWindow2Space;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Forms;
using System.Security.AccessControl;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Windows.Controls;

namespace AnotherMusicPlayer
{
    public partial class PlayLists
    {
        private MainWindow2 Parent;
        private bool isBuild = false; 
        private List<string> autoList = new List<string>() { "lastImports", "mostPlayed", "mostRecentlyPlayed", "bestRating" };

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

            Init();
        }

        #region Display Panel functions
        private void DisplayVoidPanel() {
            Parent.PlayListsTabSplitContainer1.Panel1Collapsed = false;
            Parent.PlayListsTabSplitContainer1.Panel2Collapsed = true;
            Parent.PlayListsTabSplitContainer2.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer2.Panel2Collapsed = true;
        }
        private void DisplayListPanel() {
            Parent.PlayListsTabSplitContainer1.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer1.Panel2Collapsed = false;
            Parent.PlayListsTabSplitContainer2.Panel1Collapsed = false;
            Parent.PlayListsTabSplitContainer2.Panel2Collapsed = true;
        }
        private void DisplayRadioPanel() {
            Parent.PlayListsTabSplitContainer1.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer1.Panel2Collapsed = false;
            Parent.PlayListsTabSplitContainer2.Panel1Collapsed = true;
            Parent.PlayListsTabSplitContainer2.Panel2Collapsed = false;
        }
        #endregion

        public void Init()
        {
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

            Parent.PlaylistsTree.Nodes[0].Expand();
            Parent.PlaylistsTree.Nodes[1].Expand();
            Parent.PlaylistsTree.Nodes[2].Expand();

            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT FIndex,Name,Description FROM playlists ORDER BY LOWER(Name) ASC", "FIndex");
            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
            {
                TreeNode item = new TreeNode() { Text = "" + row.Value["Name"], Tag = "" + row.Value["FIndex"], ToolTipText = "" + row.Value["Description"] };
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

                TreeNode item = new TreeNode() { Text = "" + row.Value["Name"], Tag = row.Value, ToolTipText = "" + row.Value["Description"] };
                item.ContextMenuStrip = MakeNodeContextMenu(item, PlayListsNodeContextMenuType.RadioNode);

                if (CRID > 0) {
                    if (radioCats.ContainsKey(CName)) { radioCats[CName].Nodes.Add(item); }
                    else
                    {
                        TreeNode itemCat = new TreeNode() { Text = CName, Tag = row.Value, ToolTipText = "" + row.Value["CDescription"] };
                        itemCat.ContextMenuStrip = MakeNodeContextMenu(itemCat, PlayListsNodeContextMenuType.RadioCategory);
                        itemCat.Nodes.Add(item);
                        radioCats.Add(CName, itemCat);
                    }
                }
                else { Parent.PlaylistsTree.Nodes[2].Nodes.Add(item); }
            }
            radioCats.Clear();
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
                        if (e.Node.Level > 1) { /*return;*/ }
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

                    }
                    else { DisplayVoidPanel(); }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.Node.Index == 2) {  }
                else if (e.Node.Index == 3) {  }
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
            }
            else if (parent.Index == 1)
            {

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
            if (rez == null) { return files; }
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
                files.Add(new PlayListsLineItem(
                    "" + rez[key]["Path"], "" + rez[key]["Name"],
                    (artists.Count > 0) ? string.Join(';', artists) : "", "" + rez[key]["Album"], duration, (float)rating
                    ));
                if (playlistType == AutoPlaylistTypes.MostPlayed || playlistType == AutoPlaylistTypes.MostRecentlyPlayed)
                { files[files.Count - 1].PlayCount = Convert.ToInt32("" + rez[key]["Cpt"]); }
            }

            return files;
        }

        private void OpenAutoPlaylist(AutoPlaylistTypes playlistType, bool startPlay = false)
        {
            DisplayListPanel();
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
    }
}
