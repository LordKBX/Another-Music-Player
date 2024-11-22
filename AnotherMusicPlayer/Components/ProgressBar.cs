using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AnotherMusicPlayer
{
    public class NewProgressBar : ProgressBar
    {
        public NewProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, true, null);
        }

        private double lastUpdateTime = 0;
        private int UpdateTimeInterval = 200;

        protected override void OnPaint(PaintEventArgs e)
        {
            double time = App.UnixTimestamp();
            if (lastUpdateTime + UpdateTimeInterval > time) { return; }
            Rectangle rec = e.ClipRectangle;
            Brush back = new SolidBrush(this.BackColor);
            Brush fore = new SolidBrush(this.ForeColor);

            int width = (int)((rec.Width - 2) * ((double)Value / Maximum));
            if (ProgressBarRenderer.IsSupported) { 
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
                e.Graphics.FillRectangle(back, 0, 0, rec.Width - 1, rec.Height - 1);
                e.Graphics.FillRectangle(fore, 0, 0, width - 1, rec.Height - 1);
            }
        }
    }
}
