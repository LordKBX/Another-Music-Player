using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
                tableLayoutPanel2.RowStyles.Clear();
                tableLayoutPanel2.RowCount = 1;
                tableLayoutPanel2.Controls.Clear();
                if (disk?.Key > 0) { label1.Text = "Disk " + disk?.Key; }
                else { tableLayoutPanel1.RowStyles[0].Height = 0; label1.Text = ""; label1.Visible = false; }
                List<string> brList = new List<string>();
                foreach (MediaItem item in disk?.Value.Values) { 
                    AddTrack(item);
                    brList.Add(item.Path);
                }
                this.Tag = brList;
                if (disk?.Key > 0) { this.ContextMenuStrip = App.win1.library.MakeContextMenu(this, "disk"); }
            }
        }

        public void AddTrack(MediaItem item) 
        {
            TrackButton button = new TrackButton(item) { Dock = DockStyle.Top };
            if (tableLayoutPanel2.Controls.Count > 1) { tableLayoutPanel2.RowCount += 1; }
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.AutoSize, 27));
            tableLayoutPanel2.Controls.Add(button);
        }
    }
}
