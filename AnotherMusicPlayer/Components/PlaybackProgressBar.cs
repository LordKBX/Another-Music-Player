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
using TagLib;

namespace AnotherMusicPlayer
{
    public partial class PlaybackProgressBar : UserControl
    {
        public delegate void PlaybackProgressBarNotifyValue(object sender, int value);

        private bool _FreezeEvent = true;
        //public bool FreezeEvent
        //{
        //    get { return _FreezeEvent; }
        //    set { _FreezeEvent = value; }
        //}

        public int MinValue
        {
            get { return progressBar1.Minimum; }
            set
            {
                if (value >= progressBar1.Maximum) { return; }
                progressBar1.Minimum = value;
            }
        }

        public int MaxValue
        {
            get { return progressBar1.Maximum; }
            set { 
                if(value <= progressBar1.Minimum) { return; }
                progressBar1.Maximum = value;
            }
        }

        private int _Value = 0;
        public int Value
        {
            get { return _Value; }
            set
            {
                //if (value != _Value)
                //{
                if (value > MaxValue) { _Value = MaxValue; } 
                else if (value < MinValue) { _Value = MinValue; } 
                else { _Value = value; }
                if (!_FreezeEvent) { Change?.Invoke(this, _Value); }
                progressBar1.Value = _Value;
                //}
            }
        }

        public event PlaybackProgressBarNotifyValue? Change;

        public PlaybackProgressBar()
        {
            InitializeComponent();
            this.SizeChanged += PlaybackProgressBar_SizeChanged;
            this.BackColorChanged += (object sender, EventArgs e) => { progressBar1.BackColor = this.BackColor; };
            this.ForeColorChanged += (object sender, EventArgs e) => { progressBar1.ForeColor = this.ForeColor; };
            this.MouseDown += PlaybackProgressBar_MouseDown;
            progressBar1.MouseDown += PlaybackProgressBar_MouseDown;
            Calculate();
        }

        private void PlaybackProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) { return; }
            if (sender == null) { return; }
            if (sender.GetType() != typeof(PlaybackProgressBar) && sender.GetType() != typeof(NewProgressBar)) { return; }

            Point pt = this.PointToClient(Cursor.Position);
            Debug.WriteLine("PlaybackProgressBar_MouseDown(" + ((Control)sender).Name + "), pt.X = " + pt.X);
            double value = Convert.ToDouble(pt.X) * MaxValue / Width;
            Debug.WriteLine("PlaybackProgressBar_MouseDown(" + ((Control)sender) + "), _Value = " + value);
            _Value = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.ToEven));
            Change?.Invoke(this, _Value);
            Calculate();
        }

        private void Calculate() 
        {
            progressBar1.Value = _Value;
        }

        private void PlaybackProgressBar_SizeChanged(object sender, EventArgs e)
        {
        }
    }
}
