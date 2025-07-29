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
    public partial class TrackButton : System.Windows.Forms.UserControl
    {
        public MediaItem item = null;

        public TrackButton() { Init(null); }
        public TrackButton(MediaItem item) { Init(item); }
        private void Init(MediaItem item)
        {
            InitializeComponent();
            this.item = item;
            if (item != null)
            {
                label1.Text = item.Name;
                App.SetToolTip(label1, item.Name);
                rating21.Rate = item.Rating;
                rating21.Tag = item.Path;
                rating21.RateChanged += Rating21_RateChanged;
                this.DoubleClick += TrackButton_DoubleClick;
                this.label1.DoubleClick += TrackButton_DoubleClick;
                this.tableLayoutPanel1.DoubleClick += TrackButton_DoubleClick;
                this.ContextMenuStrip = App.win1.library.MakeContextMenu(this, "file");
                this.Tag = item.Path;
            }
            //else { throw new Exception("MediaItem not valid"); }
        }

        private void TrackButton_DoubleClick(object sender, EventArgs e)
        {
            if (item != null)
            {
                Player.PlaylistClear();
                Player.PlaylistEnqueue(new string[] { item.Path });
                Player.Play();
            }
        }

        ~TrackButton() 
        {
            App.DelToolTip(label1);
        }

        private void Rating21_RateChanged(Rating2 sender, double value)
        {
            string filePath = (string)sender.Tag;
            //Debug.WriteLine("Library Rate Changed !");
            //Debug.WriteLine("filePath=" + filePath);
            //Debug.WriteLine("New value=" + value);
            FilesTags.SaveRating(filePath, value);
        }
    }
}
