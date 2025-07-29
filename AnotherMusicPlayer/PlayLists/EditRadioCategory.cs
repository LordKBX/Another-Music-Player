using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace AnotherMusicPlayer
{
    public partial class EditRadioCategory : Form
    {
        public WebRadioCategoryItem originItem = null;
        private string LogoInfo = null;
        private Bitmap image = null;

        public EditRadioCategory(Form parent, WebRadioCategoryItem item = null)
        {
            InitializeComponent();
            AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(this);
            Owner = parent;
            DialogResult = DialogResult.Cancel;
            originItem = item;

            Text = TitleLabel.Text = App.GetTranslation((item == null) ? "PlaylistsWindowEditRadioCategoryAddWindowLabel" : "PlaylistsWindowEditRadioCategoryEditWindowLabel");

            NameLabel.Text = App.GetTranslation("PlaylistsWindowEditRadioCategoryAddNameLabel");
            DescriptionLabel.Text = App.GetTranslation("PlaylistsWindowEditRadioCategoryAddDescriptionLabel");
            LogoLabel.Text = App.GetTranslation("PlaylistsWindowEditRadioCategoryAddLogoLabel");
            ValidateButton.Text = App.GetTranslation("PlaylistsWindowEditRadioCategoryAddSaveButton");

            NameTextBox.Text = (item != null) ? item.Name : "";
            DescriptionTextBox.Text = (item != null) ? item.Description : "";
            if (item != null)
            {
                LogoInfo = item.Logo;
                if (LogoInfo != null && LogoInfo.Trim() != "")
                {
                    LogoButton.BackgroundImage = image = BitmapMagic.Base64StringToTrueBitmap(LogoInfo);
                    BtnClear.Visible = true;
                }
                else { BtnClear.Visible = false; }
            }
            else { BtnClear.Visible = false; }

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

        private void LogoButtonAction_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image (*.JPG;*.JPEG;*.GIF;*.PNG;*.BMP)|*.JPG;*.JPEG;*.GIF;*.PNG;*.BMP";
            ofd.Multiselect = false;
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string st = ofd.FileNames[0];
                try
                {
                    Bitmap btm = new Bitmap(st);
                    using (var stream = new MemoryStream()) { btm.Save(stream, System.Drawing.Imaging.ImageFormat.Png); }
                    image = btm;
                    LogoButton.BackgroundImage = btm;
                    LogoInfo = BitmapMagic.BitmapToBase64String(btm, ImageFormat.Png);
                    LogoButton.Tag = "Used";
                    BtnClear.Visible = true;
                }
                catch (Exception)
                {
                    LogoButton.BackgroundImage = Properties.Resources.dot;
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            LogoButton.BackgroundImage = Properties.Resources.dot;
            LogoInfo = null;
            image = null;
            BtnClear.Visible = false;
        }

        private void ValidateButton_Click(object sender, System.EventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            if (name.Length < 3)
            {
                DialogBox.ShowDialog(App.GetTranslation("PlaylistsWindowAddIntoPlaylistWarningTitle"),
                App.GetTranslation("PlaylistsWindowAddIntoPlaylistWarningNameSize"),
                DialogBoxButtons.YesNo, DialogBoxIcons.Error, Owner); return;
            }

            if (originItem == null)
            {
                int index = 1;
                Dictionary<string, Dictionary<string, object>> rez2 = App.bdd.DatabaseQuery("SELECT CRID FROM radiosCategories ORDER BY CRID DESC LIMIT 1", "CRID");
                if (rez2 != null && rez2.Count > 0)
                {
                    Debug.WriteLine(JsonConvert.SerializeObject(rez2, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    string[] keys = rez2.Keys.ToArray();
                    index = int.Parse("" + keys[0]) + 1;
                }
                originItem = new WebRadioCategoryItem() { CRID = index, Name = name, Description = description, Logo = LogoInfo };
                App.bdd.DatabaseQuery("INSERT INTO radiosCategories(CRID, Name, Description, Logo) Values('" + index + "', '" + Database.EscapeString(name) + "', '" + Database.EscapeString(description) + "', " + ((LogoInfo == null) ? "NULL" : "'" + LogoInfo + "'") + ")", null, true);
            }
            else
            {
                originItem.Name = name;
                originItem.Description = description;
                App.bdd.DatabaseQuery("UPDATE radiosCategories SET Name = '" + Database.EscapeString(name) + "', Description = '" + Database.EscapeString(description) + "', Logo = " + ((LogoInfo == null) ? "NULL" : "'" + LogoInfo + "'") + " WHERE CRID = " + originItem.CRID, null, true);
            }

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
