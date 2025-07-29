using AnotherMusicPlayer.MainWindow2Space;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AnotherMusicPlayer
{
    public partial class RenameWindow : Form
    {
        private string FolderPath;
        private string[] PathTab;
        public bool renamed = false;
        public RenameWindow(MainWindow2 parent, string folderPath, string[] pathTab)
        {
            Owner = parent;
            FolderPath = folderPath;
            PathTab = pathTab;
            InitializeComponent();

            input.Text = pathTab[pathTab.Length - 1];
            this.Load += RenameWindow_Loaded;

            saveBtn.Click += ValidateButton_Click;
            input.TextChanged += (object sender, EventArgs e) => { saveBtn.Enabled = true; };

            #region Window displasment gestion
            MainWIndowHead.MouseDown += FormDragable_MouseDown;
            MainWIndowHead.MouseMove += FormDragable_MouseMove;
            MainWIndowHead.MouseUp += FormDragable_MouseUp;
            TitleLabel.MouseDown += FormDragable_MouseDown;
            TitleLabel.MouseMove += FormDragable_MouseMove;
            TitleLabel.MouseUp += FormDragable_MouseUp;
            #endregion
        }

        private void RenameWindow_Loaded(object sender, EventArgs e)
        {
            Left = Owner.Left + ((Owner.Width - Width) / 2);
            Top = Owner.Top + ((Owner.Height - Height) / 2);
        }

        private void ValidateButton_Click(object sender, System.EventArgs e)
        {
            string tx = input.Text.Trim();
            bool ok = true;
            char[] excludeList = new char[] { '<', '>', ':', '"', '/', '\\', '?', '*', '|' };
            if (tx == "") { ok = false; }
            else
            {
                foreach (char c in excludeList) { if (tx.Contains(c)) { ok = false; break; } }
            }
            if (ok == false)
            {
                MessageBox.Show(
                    "Folder name invalid,\nplease remove the folowing characters:\n < > : \" / \\ | ? *", 
                    "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> list = new List<string>(PathTab);
            list.Remove(PathTab[PathTab.Length - 1]);

            Directory.Move(FolderPath, string.Join(MainWindow2.SeparatorChar, list.ToArray()) + MainWindow2.SeparatorChar + input.Text.Trim());
            renamed = true;
            DialogResult = DialogResult.OK;
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
