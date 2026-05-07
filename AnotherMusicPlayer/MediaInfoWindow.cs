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

namespace AnotherMusicPlayer
{
    public partial class MediaInfoWindow : Form
    {
        private TagLib.File Tags;
        private Font fontNormal = App.win1.Font;
        private Font fontBold = new Font(App.win1.Font, FontStyle.Bold);
        Bitmap BitmapCover;

        public MediaInfoWindow(Form parent, string filePath)
        {
            this.Owner = parent;
            if (filePath == null || !File.Exists(filePath)) { throw new Exception("File not found!"); }
            InitializeComponent();
            MinimumSize = new Size(600, 300);
            Tags = TagLib.File.Create(filePath);
            this.Resize += MediaInfo_Resize;

            BitmapCover = Properties.Resources.album_large;
            if (Tags.Tag.Pictures.Length > 0) { changeCoverPreview(Tags.Tag.Pictures[0]); }
            else { Cover.BackgroundImage = BitmapCover; }

                flowLayoutPanelRight.AutoScroll = true;
            flowLayoutPanelRight.Controls.Clear();

            AddLine1L("Year", string.Join("; ", Tags.Tag.Year));
            AddLine1L("Disc", "" + Tags.Tag.Disc + " / " + (Tags.Tag.DiscCount <= 0 ? "??" : Tags.Tag.DiscCount));
            AddLine1L("Track", "" + Tags.Tag.Track + " / " + (Tags.Tag.TrackCount <= 0 ? "??" : Tags.Tag.TrackCount));
            AddLine2L("Copyright", Tags.Tag.Copyright);

            AddLineR("Path", filePath);
            AddLineR("Title", Tags.Tag.Title);
            AddLineR("Album", Tags.Tag.Album);
            AddLineR("AlbumArtists", string.Join("; ", Tags.Tag.AlbumArtists));
            AddLineR("Composers", string.Join("; ", Tags.Tag.Composers));
            AddLineR("Performers", string.Join("; ", Tags.Tag.Performers));
            AddLineR("Genres", string.Join("; ", Tags.Tag.Genres));

            flowLayoutPanelRight.Controls.Add(new Label() { 
                Font = fontBold, Text = "Lyrics"
            });
            int cw = flowLayoutPanelRight.Width - 20;
            RichTextBox lb1 = new RichTextBox()
            {
                ReadOnly = true,
                Enabled = (Tags.Tag.Lyrics == null || Tags.Tag.Lyrics.Trim().Length == 0)?false:true,
                Font = fontNormal,
                Text = Tags.Tag.Lyrics,
                MinimumSize = new Size(cw, 120),
                Tag = "dataBlock",
                BackColor = Color.LightGray
            };
            lb1.Width = cw;
            flowLayoutPanelRight.Controls.Add(lb1);
            App.SetToolTip(lb1, "Lyrics");

            #region Rate element
            TagLib.Tag tag = Tags.GetTag(TagLib.TagTypes.Id3v2);
            byte rate1 = 0; double rate = 0;

            try { rate1 = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "Windows Media Player 9 Series", true).Rating; }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }

            if (FilesTags.TableRatePlayer.ContainsKey(rate1)) { rate = FilesTags.TableRatePlayer[rate1]; }
            else
            {
                byte min = 255;
                foreach (byte i in FilesTags.TableRatePlayer.Keys) { if (rate1 - i >= 0) { min = i; } }
                if (min == 255) { rate = 5; }
                else { rate = (double)(FilesTags.TableRatePlayer[min] + 0.5); }
            }

            flowLayoutPanelLeft.Controls.Add(new Label() { Font = fontBold, Text = "Rating" });
            Rating2 ratingObject = new Rating2()
            {
                MinimumSize = new Size(150, 40),
                Rate = rate, IsReadOnly = true,
                Margin = new Padding(5, 0, 0, 0)
            };
            flowLayoutPanelLeft.Controls.Add(ratingObject);
            App.SetToolTip(ratingObject, "" + rate + " / 5");
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
            flowLayoutPanelRight.Controls.Add(new Label() { Font = fontBold, Text = cat.Capitalize() });
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
