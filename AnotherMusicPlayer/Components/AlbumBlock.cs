using AnotherMusicPlayer.MainWindow2Space;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TagLib.Riff;

namespace AnotherMusicPlayer.Components
{
    public partial class AlbumBlock : UserControl
    {
        public AlbumBlock(KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> albumT, bool uniqueDir, Bitmap defaultCover)
        {
            InitializeComponent();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ControlAdded += (object sender, ControlEventArgs e) => { RefreshHeight(); };
            tableLayoutPanel1.ControlRemoved += (object sender, ControlEventArgs e) => { RefreshHeight(); };
            tableLayoutPanel1.Resize += (object sender, EventArgs e) => { RefreshHeight(); };
            if (uniqueDir)
            {
                MediaItem item1 = null;
                foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> disk in albumT.Value)
                { foreach (KeyValuePair<string, MediaItem> track in disk.Value) { item1 = track.Value; break; }; break; }

                string folder = item1.Path.Substring(0, item1.Path.LastIndexOf(MainWindow2.SeparatorChar));
                string[] t = System.IO.Directory.GetFiles(folder);
                foreach (string file in t)
                {
                    if (file.EndsWith("Cover.jpg") || file.EndsWith("Cover.png")) { defaultCover = new Bitmap(file); }
                }
            }

            string coverPath = albumT.Value.Values.First().First().Value.Path;
            string al = albumT.Key;
            if (al == null || al.Trim() == "") { if (uniqueDir) { al = "<UNKWON ALBUM>"; } else { al = albumT.Value.Values.First().First().Value.Name; } }
            label1.Text = al;

            button1.BackgroundImage = App.BitmapImage2Bitmap(FilesTags.MediaPicture(coverPath, App.bdd, true, 150, 150, false), defaultCover);
            List<string> brList = new List<string>();

            foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> disk in albumT.Value)
            {
                DiskBlock block = new DiskBlock(disk) { Dock = DockStyle.Top };
                if (tableLayoutPanel1.Controls.Count > 0) { tableLayoutPanel1.RowCount += 1; }
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize, 50));
                tableLayoutPanel1.Controls.Add(block);
            }
            this.ContextMenuStrip = App.win1.library.MakeContextMenu(this, "album");
            this.Tag = brList.ToArray();
            RefreshHeight();
        }

        private void RefreshHeight()
        {
            if (this.Parent != null)
            {
                this.Width = this.Parent.Width - 30;
            }
            if (tableLayoutPanel1.Controls.Count > 1)
            {
                int height = 0;
                for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++) 
                { height += tableLayoutPanel1.Controls[i].Height + tableLayoutPanel1.Controls[i].Margin.Top + tableLayoutPanel1.Controls[i].Margin.Bottom; }

                tableLayoutPanel1.Height = height;
                this.Height = 6 + tableLayoutPanel1.Height;
            }
        }
    }
}
