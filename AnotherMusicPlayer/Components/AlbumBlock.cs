﻿using AnotherMusicPlayer.MainWindow2Space;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace AnotherMusicPlayer.Components
{
    public partial class AlbumBlock : UserControl
    {
        public static int MinHeight = 150;
        public AlbumBlock(KeyValuePair<string, Dictionary<uint, Dictionary<string, MediaItem>>> albumT, bool uniqueDir, Bitmap defaultCover)
        {
            InitializeComponent();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 1;
            int calcHeight = 30;

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
            BitmapImage bi = BitmapMagic.BitmapToBitmapImage(defaultCover);
            try { bi = FilesTags.MediaPicture(coverPath, App.bdd, true, 150, 150, false); } catch (Exception) { }
            button1.BackgroundImage = BitmapMagic.BitmapImage2Bitmap(bi);
            List<string> brList = new List<string>();

            foreach (KeyValuePair<uint, Dictionary<string, MediaItem>> disk in albumT.Value)
            {
                DiskBlock block = new DiskBlock(disk) { Dock = DockStyle.Top };
                if (tableLayoutPanel1.Controls.Count > 0) { tableLayoutPanel1.RowCount += 1; }
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, block.Height));
                tableLayoutPanel1.Controls.Add(block);
                brList.AddRange((List<string>)block.Tag);
                calcHeight += block.Height;
            }
            this.ContextMenuStrip = App.win1.library.MakeContextMenu(this, "album");
            this.Tag = brList.ToArray();
            calcHeight += 10;
            Debug.WriteLine("calcHeight: " + calcHeight + ", minHeight: " + MinHeight);
            if (calcHeight > MinHeight) { 
                this.MinimumSize = new Size(this.MinimumSize.Width, calcHeight);
                this.Height = calcHeight;
            }
        }
    }
}
