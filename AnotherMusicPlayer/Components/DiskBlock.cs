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
        public static int MinimumHeight = 30;
        private int CalcHeight = 0;
        private static int r0Height = 30;
        private static int rxHeight = 30;
        private bool hasLine0 = false;

        private void Init(KeyValuePair<uint, Dictionary<string, MediaItem>>? disk)
        {
            InitializeComponent();
            if (disk != null)
            {
                tableLayoutPanel2.RowCount = 1;
                tableLayoutPanel2.Controls.Clear();
                for(int i = tableLayoutPanel2.RowStyles.Count - 1; i > 0; i--) { tableLayoutPanel2.RowStyles.RemoveAt(i); }
                tableLayoutPanel2.RowCount = 1;
                if (disk?.Key > 0) {
                    hasLine0 = true;
                    tableLayoutPanel1.RowStyles[0].Height = r0Height;
                    label1.Text = "Disk " + disk?.Key;
                }
                else { tableLayoutPanel1.RowStyles[0].Height = 0; label1.Text = ""; label1.Visible = false; }
                
                List<string> brList = new List<string>();
                foreach (MediaItem item in disk?.Value.Values) { 
                    AddTrack(item, false);
                    CalcHeight += rxHeight;
                    brList.Add(item.Path);
                }
                this.Tag = brList;
                if (disk?.Key > 0) { this.ContextMenuStrip = App.win1.library.MakeContextMenu(this, "disk"); }
                ApplyHeight();
            }
        }

        public void AddTrack(MediaItem item, bool ispublic = true) 
        {
            TrackButton button = new TrackButton(item) { Dock = DockStyle.Top };
            if (tableLayoutPanel2.Controls.Count > 0) { 
                tableLayoutPanel2.RowCount += 1;
                tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, rxHeight));
            }
            else { tableLayoutPanel2.RowStyles[0] = new RowStyle(SizeType.Absolute, rxHeight); }
            tableLayoutPanel2.Controls.Add(button);
            if (ispublic) { ApplyHeight(); }
        }

        private void ApplyHeight() {
            int lines = tableLayoutPanel2.RowCount;
            //if (lines > 4) { lines += 1; }
            if (tableLayoutPanel2.Controls.Count < tableLayoutPanel2.RowCount) { lines -= 1; }

            CalcHeight = 6 + ((hasLine0) ? r0Height : 0) + ((lines) * rxHeight) + ((lines) * 1);

            if (CalcHeight > MinimumHeight)
            {
                tableLayoutPanel1.Height = CalcHeight;
                tableLayoutPanel1.MinimumSize = new Size(tableLayoutPanel1.Width, CalcHeight);
                this.MinimumSize = new Size(this.MinimumSize.Width, CalcHeight);
                this.MinimumSize = new Size(this.MinimumSize.Width, CalcHeight);
                this.Height = CalcHeight;
            }
            else
            {
                tableLayoutPanel1.Height = MinimumHeight;
                tableLayoutPanel1.MinimumSize = new Size(tableLayoutPanel1.Width, MinimumHeight);
                this.MinimumSize = new Size(this.MinimumSize.Width, MinimumHeight);
                this.MinimumSize = new Size(this.MinimumSize.Width, MinimumHeight);
                this.Height = MinimumHeight;
            }
            //Debug.WriteLine("CalcHeight = " + CalcHeight);
            //Debug.WriteLine("DiskBlock initialized height " + this.Height);
        }
    }
}
