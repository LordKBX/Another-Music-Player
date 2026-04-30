using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomExtensions;

namespace AnotherMusicPlayer
{
    public partial class MediaInfo : Form
    {
        private TagLib.File Tags;
        private Font fontNormal = App.win1.Font;
        private Font fontBold = new Font(App.win1.Font, FontStyle.Bold);
        Bitmap BitmapCover;

        public MediaInfo(string filePath)
        {
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

            AddLineL("Year", string.Join("; ", Tags.Tag.Year));
            AddLineL("Disc", "" + Tags.Tag.Disc + " / " + (Tags.Tag.DiscCount <= 0 ? "??" : Tags.Tag.DiscCount));
            AddLineL("Track", "" + Tags.Tag.Track + " / " + (Tags.Tag.TrackCount <= 0 ? "??" : Tags.Tag.TrackCount));
            AddLineL("Copyright", Tags.Tag.Copyright);

            AddLineR("Path", filePath);
            AddLineR("Title", Tags.Tag.Title);
            AddLineR("Album", Tags.Tag.Album);
            AddLineR("AlbumArtists", string.Join("; ", Tags.Tag.AlbumArtists));
            AddLineR("Composers", string.Join("; ", Tags.Tag.Composers));
            AddLineR("Performers", string.Join("; ", Tags.Tag.Performers));
            AddLineR("Genres", string.Join("; ", Tags.Tag.Genres));

            flowLayoutPanelRight.Controls.Add(new Label() { Font = fontBold, Text = "Lyrics" });
            int cw = flowLayoutPanelRight.Width - 20;
            RichTextBox lb1 = new RichTextBox()
            {
                ReadOnly = true,
                Font = fontNormal,
                Text = Tags.Tag.Lyrics,
                MinimumSize = new Size(cw, 120),
                Tag = "dataBlock",
                BackColor = Color.LightGray
            };
            lb1.Width = cw;
            flowLayoutPanelRight.Controls.Add(lb1);
            App.SetToolTip(lb1, "Lyrics");
        }

        private void AddLineL(string cat, string data) 
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
