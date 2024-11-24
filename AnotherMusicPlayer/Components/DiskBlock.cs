using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AnotherMusicPlayer.Components
{
    [ToolboxBitmap(typeof(System.Windows.Forms.UserControl))]
    [Designer(typeof(ControlDesigner))]
    [Docking(DockingBehavior.Ask)]
    [DefaultEvent("Resized")]
    [DefaultProperty("AutoSize")]
    public partial class DiskBlock : UserControl
    {

        public DiskBlock() { Init(null); }
        public DiskBlock(KeyValuePair<uint, Dictionary<string, MediaItem>> disk) { Init(disk); }
        private void Init(KeyValuePair<uint, Dictionary<string, MediaItem>>? disk)
        {
            InitializeComponent();
            if (disk != null)
            {
                flowLayoutPanel1.Controls.Clear();
                if (disk?.Key > 0) { label1.Text = "Disk " + disk?.Key; }
                else { tableLayoutPanel1.RowStyles[0].Height = 0; label1.Text = ""; label1.Visible = false; }
                foreach (MediaItem item in disk?.Value.Values) { AddTrack(item); }

                flowLayoutPanel1.ControlAdded += (object sender, ControlEventArgs e) => { RefreshHeight(); };
                flowLayoutPanel1.ControlRemoved += (object sender, ControlEventArgs e) => { RefreshHeight(); };
                flowLayoutPanel1.SizeChanged += (object sender, EventArgs e) => { RefreshHeight(); };
                this.Resize += (object sender, EventArgs e) => { RefreshHeight(); };
                RefreshHeight();
            }
        }

        public void AddTrack(MediaItem item) 
        {
            TrackButton button = new TrackButton(item);
            flowLayoutPanel1.Controls.Add(button);
        }

        private void RefreshHeight()
        {
            if (flowLayoutPanel1.Controls.Count > 1 && flowLayoutPanel1.Controls[0] is Control control)
            {
                int InitialHeight = Convert.ToInt32(this.MinimumSize.Height - tableLayoutPanel1.RowStyles[0].Height);
                flowLayoutPanel1.Height =
                  InitialHeight * (int)Math.Ceiling(
                    flowLayoutPanel1.Controls.Count / Math.Floor(
                      flowLayoutPanel1.ClientSize.Width / (double)control.Width)) + 6;
                tableLayoutPanel1.Height = 6 + flowLayoutPanel1.Height;
                this.Height = 6 + tableLayoutPanel1.Height;
            }
        }
    }
}
