using CustomExtensions;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace AnotherMusicPlayer
{
    public partial class MediaInfoWindow : Form
    {
        private Font fontNormal = App.win1.Font;
        private Font fontBold = new Font(App.win1.Font, FontStyle.Bold);
        Bitmap BitmapCover;

        public MediaInfoWindow(Form parent, string filePath)
        {
            this.Owner = parent;
            if (filePath == null || !File.Exists(filePath)) { throw new Exception("File not found!"); }
            InitializeComponent();
            MinimumSize = new Size(600, 300);

            MediaItem item = FilesTags.MediaInfo(filePath, false);

            this.Resize += MediaInfo_Resize;

            BitmapCover = Properties.Resources.album_large;
            try { BitmapCover = BitmapMagic.BitmapImage2Bitmap(FilesTags.MediaPicture(item.Path, App.bdd, true, 250, 250)); }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }

            Cover.BackgroundImage = BitmapCover;

                flowLayoutPanelRight.AutoScroll = true;
            flowLayoutPanelRight.Controls.Clear();

            AddLine1L("Year", string.Join("; ", item.Year));
            AddLine1L("Disc", "" + item.Disc + " / " + (item.DiscCount <= 0 ? "??" : item.DiscCount));
            AddLine1L("Track", "" + item.Track + " / " + (item.TrackCount <= 0 ? "??" : item.TrackCount));
            AddLine2L("Copyright", item.Copyright);

            AddLineR("Path", filePath);
            AddLineR("Title", item.Name);
            AddLineR("Album", item.Album);
            AddLineR("AlbumArtists", string.Join("; ", item.AlbumArtists));
            AddLineR("Composers", string.Join("; ", item.Composers));
            AddLineR("Performers", string.Join("; ", item.Performers));
            AddLineR("Genres", string.Join("; ", item.Genres));
            flowLayoutPanelRight.Controls.Add(new Label() { 
                Font = fontBold, Text = "Lyrics"
            });
            int cw = flowLayoutPanelRight.Width - 20;
            RichTextBox lb1 = new RichTextBox()
            {
                ReadOnly = true,
                //Enabled = (Tags.Tag.Lyrics == null || Tags.Tag.Lyrics.Trim().Length == 0)?false:true,
                Font = fontNormal,
                Text = item.Lyrics,
                MinimumSize = new Size(cw, 120),
                Tag = "dataBlock",
                BackColor = App.style.GetColor("GlobalTextBoxBackColor"),
                ForeColor = App.style.GetColor("GlobalTextBoxForeColor")
            };
            lb1.Width = cw;
            flowLayoutPanelRight.Controls.Add(lb1);
            App.SetToolTip(lb1, "Lyrics");

            #region Rate element

            flowLayoutPanelLeft.Controls.Add(new Label() { Font = fontBold, Text = "Rating" });
            Rating2 ratingObject = new Rating2()
            {
                MinimumSize = new Size(150, 40),
                Rate = item.Rating, IsReadOnly = true,
                Margin = new Padding(5, 0, 0, 0)
            };
            flowLayoutPanelLeft.Controls.Add(ratingObject);
            App.SetToolTip(ratingObject, "" + item.Rating + " / 5");
            #endregion

            Button openFolder = new Button()
            {
                MinimumSize = new Size(150, 40),
                Text = "Open folder",
                Margin = new Padding(5, 0, 0, 0)
            };
            openFolder.Click += (s, e) =>
            {
                try
                { Process.Start("explorer.exe", "/select,\"" + filePath + "\""); }
                catch (Exception ex)
                { MessageBox.Show("Error opening folder: " + ex.Message); }
            };
            flowLayoutPanelLeft.Controls.Add(openFolder);

            flowLayoutPanelRight.Controls[0].Focus();

            SetStyle();
        }

        public void SetStyle(Control ctl = null) 
        {
            if (ctl == null) { ctl = this; }
            //AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(this);

            if (ctl.Name == "Cover") { ctl.BackColor = App.style.GetColor("GlobalTrackIconBackColor"); }
            else
            {
                ctl.BackColor = App.style.GetColor("GlobalBackColor");
                try { ctl.ForeColor = App.style.GetColor("GlobalForeColor"); } catch (Exception) { }
            }

            if (ctl.Controls != null && ctl.Controls.Count > 0) { foreach (Control ctl2 in ctl.Controls) { SetStyle(ctl2); } }
        }

        private void AddLine1L(string cat, string data) 
        {
            TableLayoutPanel table = new TableLayoutPanel()
            {
                MinimumSize = new Size(flowLayoutPanelLeft.Width - 20, fontBold.Height + 4),
                MaximumSize = new Size(flowLayoutPanelLeft.Width - 20, fontBold.Height + 4),
                RowCount = 1, ColumnCount = 2,
                Margin = new Padding(0)
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            table.Controls.Add(new Label() { Font = fontBold, Text = cat.Capitalize() }, 0, 0);
            Label lb1 = new Label()
            {
                Font = fontNormal,
                Text = data,
                TextAlign = ContentAlignment.TopLeft,
                Tag = "dataLine"
            };
            table.Controls.Add(lb1, 1, 0);
            App.SetToolTip(lb1, data);

            flowLayoutPanelLeft.Controls.Add(table);
        }

        private void AddLine2L(string cat, string data) 
        {
            flowLayoutPanelLeft.Controls.Add(new Label() { Font = fontBold, Text = cat.Capitalize() });
            Label lb1 = new Label()
            {
                Font = fontNormal,
                Text = data,
                TextAlign = ContentAlignment.TopLeft,
                Tag = "dataLine"
            };
            flowLayoutPanelLeft.Controls.Add(lb1);
            App.SetToolTip(lb1, data);
        }

        private void AddLineR(string cat, string data) 
        {
            flowLayoutPanelRight.Controls.Add(new Label() { Font = fontBold, Text = cat.Capitalize(), MinimumSize = new Size(flowLayoutPanelRight.Width - 20, 20) });
            Label lb1 = new Label()
            {
                Font = fontNormal,
                Text = data,
                TextAlign = ContentAlignment.TopLeft,
                MinimumSize = new Size(flowLayoutPanelRight.Width - 20, 20),
                Tag = "dataLine"
            };
            flowLayoutPanelRight.Controls.Add(lb1);
            App.SetToolTip(lb1, data);
        }

        private void changeCoverPreview(TagLib.IPicture pic)
        {
            MemoryStream ms = new MemoryStream(pic.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapCover = new Bitmap(ms);
            ms.Close();
            Cover.BackgroundImage = BitmapCover;
        }

        private void MediaInfo_Resize(object sender, EventArgs e)
        {
            Type st = typeof(string);
            Size nz = new Size(flowLayoutPanelRight.Width - 20, 20);
            foreach (Control ctl in flowLayoutPanelRight.Controls)
            {
                if (ctl.Tag != null && ctl.Tag.GetType() == st)
                {
                    if (("" + ctl.Tag) == "dataLine") { ctl.MinimumSize = nz; ctl.Width = nz.Width; }
                    if (("" + ctl.Tag) == "dataBlock") { ctl.MinimumSize = new Size(nz.Width, 120); ctl.Width = nz.Width; }
                }
            }
        }
    }
}
