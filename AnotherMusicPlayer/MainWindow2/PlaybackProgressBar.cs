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

namespace AnotherMusicPlayer.MainWindow2
{
    public partial class PlaybackProgressBar : UserControl
    {
        public delegate void PlaybackProgressBarNotifyValue(object? sender, double value);

        private bool _FreezeEvent = true;
        //public bool FreezeEvent
        //{
        //    get { return _FreezeEvent; }
        //    set { _FreezeEvent = value; }
        //}

        private double _MinValue = 0;
        public double MinValue
        {
            get { return _MinValue; }
        }

        private double _MaxValue = 100;
        public double MaxValue
        {
            get { return _MaxValue; }
        }

        private double _Value = 0;
        public double Value
        {
            get { return _Value; }
            set
            {
                //if (value != _Value)
                //{
                if (value > _MaxValue) { _Value = MaxValue; } else if (value < _MinValue) { _Value = MinValue; } else { _Value = Math.Round(value, 2, MidpointRounding.ToEven); }
                if (!_FreezeEvent) { Change?.Invoke(this, _Value); }
                Calculate();
                //}
            }
        }

        public event PlaybackProgressBarNotifyValue? Change;

        public PlaybackProgressBar()
        {
            InitializeComponent();
            this.SizeChanged += PlaybackProgressBar_SizeChanged;
            this.BackColorChanged += (object sender, EventArgs e) => { panel1.BackColor = this.BackColor; };
            this.ForeColorChanged += (object sender, EventArgs e) => { panel2.BackColor = this.ForeColor; };
            this.MouseDown += PlaybackProgressBar_MouseDown;
            panel1.MouseDown += PlaybackProgressBar_MouseDown;
            panel2.MouseDown += PlaybackProgressBar_MouseDown;
            Calculate();
        }

        private void PlaybackProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) { return; }
            if (sender == null) { return; }
            if (sender.GetType() != typeof(PlaybackProgressBar) && sender.GetType() != typeof(Panel)) { return; }

            Point pt = this.PointToClient(Cursor.Position);
            Debug.WriteLine("PlaybackProgressBar_MouseDown(" + ((Control)sender).Name + "), pt.X = " + pt.X);
            double value = Math.Round(Convert.ToDouble(pt.X) * _MaxValue / panel1.Width, 2, MidpointRounding.ToEven);
            Debug.WriteLine("PlaybackProgressBar_MouseDown(" + ((Control)sender) + "), _Value = " + value);
            _Value = value;
            Change?.Invoke(this, value);
            Calculate();
        }

        private void Calculate() 
        {
            panel2.Width = Convert.ToInt32(Math.Truncate(Math.Round(panel1.Width * _Value / _MaxValue, 0, MidpointRounding.ToEven)));
        }

        private void PlaybackProgressBar_SizeChanged(object sender, EventArgs e)
        {
            panel2.Height = panel1.Height;
            Calculate();
        }
    }
}
