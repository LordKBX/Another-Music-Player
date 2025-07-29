using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Markup;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace AnotherMusicPlayer
{
    public partial class InsertPlayList : Form
    {
        List<string> clearTracks = new List<string>();

        public InsertPlayList(Form parent, string[] tracks)
        {
            clearTracks = new List<string>();
            foreach (string track in tracks)
            {
                string trackTrimed = track.Trim();
                if (trackTrimed != "") { if (System.IO.File.Exists(trackTrimed)) { clearTracks.Add(trackTrimed); } }
            }
            if (clearTracks.Count < 1) { throw new Exception("No valid Tracks in parametters"); }

            InitializeComponent();
            AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(this);
            Owner = parent;
            DialogResult = DialogResult.Cancel;

            Text = TitleLabel.Text = App.GetTranslation("PlaylistsWindowAddIntoPlaylistTitle");

            CustomListLabel.Text = App.GetTranslation("PlaylistsWindowAddIntoPlaylistName");
            ValidateButton.Text = App.GetTranslation("PlaylistsWindowAddSaveButton");

            ListPlayLists();
            if (CustomListComboBox.Items.Count > 0) { CustomListComboBox.SelectedItem = 0; }

            ValidateButton.Click += ValidateButton_Click;

            #region Window displasment gestion
            MainWIndowHead.MouseDown += FormDragable_MouseDown;
            MainWIndowHead.MouseMove += FormDragable_MouseMove;
            MainWIndowHead.MouseUp += FormDragable_MouseUp;
            TitleLabel.MouseDown += FormDragable_MouseDown;
            TitleLabel.MouseMove += FormDragable_MouseMove;
            TitleLabel.MouseUp += FormDragable_MouseUp;
            #endregion
        }

        private void ListPlayLists(string? searchedName = null) 
        {
            int nsel = -1;
            CustomListComboBox.Items.Clear();
            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT FIndex,Name,Description FROM playlists ORDER BY Name ASC", "FIndex");
            if (rez != null)
            {
                foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
                {
                    if (searchedName != null && searchedName == "" + row.Value["Name"]) { nsel = CustomListComboBox.Items.Count; }
                    DropDownItem item = new DropDownItem() { Content = "" + row.Value["Name"], Data = int.Parse("" + row.Value["FIndex"]) };
                    CustomListComboBox.Items.Add(item);
                }

            }
            if (searchedName != null) { CustomListComboBox.SelectedIndex = nsel; }
        }

        private void CustomListButton_Click(object sender, EventArgs e)
        {
            EditPlayList epl = new EditPlayList(Owner, null) { StartPosition = FormStartPosition.CenterParent };
            if (epl.ShowDialog() == DialogResult.OK) { ListPlayLists(); }
        }

        private void ValidateButton_Click(object sender, System.EventArgs e)
        {
            if (CustomListComboBox.SelectedIndex < 0) { return; }
            int listId = (int)((DropDownItem)CustomListComboBox.SelectedItem).Data;
            int startOrder = 0;
            Dictionary<string, Dictionary<string, object>> rez = App.bdd.DatabaseQuery("SELECT LOrder FROM playlistsItems WHERE LIndex = '" + listId + "' ORDER BY LOrder DESC LIMIT 1", "LOrder");
            if (rez != null) { foreach (string key in rez.Keys) { startOrder = Convert.ToInt32(key); break; } }

            string query = "INSERT INTO playlistsItems(LIndex, Path, LOrder) VALUES";
            List<string> querys = new List<string>(); int offset = 1;
            foreach (string track in clearTracks) { querys.Add(query + "('" + listId + "', '" + Database.EscapeString(track) + "', '" + (startOrder + offset) + "')"); offset += 1; }
            App.bdd.DatabaseQuerys(querys.ToArray(), true);

            App.win1.playLists.Init();

            DialogResult = DialogResult.OK;
            App.win1.playLists.Init();
            Close();
        }

        #region Window displasment gestion
        private Dictionary<string, bool> draggings = new Dictionary<string, bool>();
        private Dictionary<string, System.Drawing.Point> dragCursorPoints = new Dictionary<string, System.Drawing.Point>();
        private Dictionary<string, System.Drawing.Point> dragFormPoints = new Dictionary<string, System.Drawing.Point>();
        private Dictionary<string, Form> dragForms = new Dictionary<string, Form>();

        private void FormDragable_InitTab(object sender, bool active)
        {
            try
            {
                TableLayoutPanel label1 = (TableLayoutPanel)sender;
                string label = label1.Tag as string;
                Control parent = label1.Parent;
                while (parent.GetType().Name == "TableLayoutPanel") { parent = parent.Parent; }

                draggings.Add(label, active);
                dragCursorPoints.Add(label, System.Windows.Forms.Cursor.Position);
                dragFormPoints.Add(label, label1.Location);
                dragForms.Add(label, (Form)parent);
            }
            catch (Exception) { }
        }

        public void FormDragable_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender == null) { return; }
            while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
            string label = ((TableLayoutPanel)sender).Tag as string;
            if (!draggings.ContainsKey(label)) { FormDragable_InitTab(sender, true); }
            else
            {
                draggings[label] = true;
                dragCursorPoints[label] = System.Windows.Forms.Cursor.Position;
                dragFormPoints[label] = dragForms[label].Location;
            }
        }

        public void FormDragable_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (sender == null) { return; }
                while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
                string label = ((TableLayoutPanel)sender).Tag as string;
                if (!draggings.ContainsKey(label)) { FormDragable_InitTab(sender, false); }
                if (draggings[label])
                {
                    System.Drawing.Point dif = System.Drawing.Point.Subtract(System.Windows.Forms.Cursor.Position, new System.Drawing.Size(dragCursorPoints[label]));
                    dragForms[label].Location = System.Drawing.Point.Add(dragFormPoints[label], new System.Drawing.Size(dif));
                }
            }
            catch (Exception) { }
        }

        public void FormDragable_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender == null) { return; }
            while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
            string label = ((TableLayoutPanel)sender).Tag as string;
            draggings[label] = false;
        }

        public void FormDragable_Clear(string id)
        {
            if (draggings.ContainsKey(id))
            {
                draggings.Remove(id);
                dragCursorPoints.Remove(id);
                dragFormPoints.Remove(id);
                dragForms.Remove(id);
            }
        }
        #endregion

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
