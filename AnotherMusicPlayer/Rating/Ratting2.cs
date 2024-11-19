using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Color = System.Drawing.Color;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace AnotherMusicPlayer
{
    public delegate void Ratting2RateChangedNotify(double value);
    public delegate void Ratting2ZoomChangedNotify(double value);

    public partial class Ratting2 : UserControl
    {
        private double Max = 5.0;
        private double StarCaseWidth = 20.0;

        #region IsReadOnly
        private bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            set => _IsReadOnly = value;
        }
        #endregion  

        #region Zoom Property
        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { ((Rating)d).setZoom((double)e.NewValue); }

        private double _Zoom = 1.0;
        public double Zoom
        {
            get { return _Zoom; }
            set { setZoom(value); }
        }
        public event Ratting2ZoomChangedNotify ZoomChanged;

        public void setZoom(double multiply)
        {
            double caseSize = StarCaseWidth * multiply;
            //PointCollection StarMatrix = StarPoints;
            //for (int i = 0; i < 5; i++) { StarMatrix[i] = new Point(StarPoints[i].X * multiply, StarPoints[i].Y * multiply); }
            //foreach (Polygon star in Grid1.Children) { star.Width = caseSize; star.Points = StarMatrix; }
            //foreach (Polygon star in Grid2.Children) { star.Width = caseSize; star.Points = StarMatrix; }
            //_Zoom = multiply; ZoomChanged?.Invoke(_Zoom);
            reDraw();
        }
        #endregion

        #region Rate Property
        /// <summary>
        /// StarForegroundColor Dependency Property
        /// </summary>
        private double _Rate = 0;
        public double Rate
        {
            get { return _Rate; }
            set { setRate(value); }
        }
        public event Ratting2RateChangedNotify RateChanged;

        /// <summary>
        /// Handles changes to the StarForegroundColor property.
        /// </summary>
        private static void OnRateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { ((Rating)d).setRate((double)e.NewValue); }

        #endregion


        public Ratting2()
        {
            InitializeComponent();
            tableLayoutPanel2.Width = 0;
            this.MouseMove += StarGrid_MouseMove;
            this.MouseUp += StarGrid_MouseUp;
            this.MouseDown += StarGrid_MouseMove;
        }

        private void StarGrid_MouseUp(object sender, EventArgs e) {
            if (_IsReadOnly) { return; }
            double caseW = StarCaseWidth * _Zoom;
            double posx = (int)(LastPos.X / caseW) + ((((LastPos.X / caseW) - (int)(LastPos.X / caseW)) >= 0.5) ? 0.5 : 0);
            setRate(posx); 
        }

        System.Drawing.Point LastPos = new System.Drawing.Point(0, 0);
        private void StarGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_IsReadOnly) { return; }
            LastPos = e.Location;
            double caseW = StarCaseWidth * _Zoom;
            double posx = (int)(LastPos.X / caseW) + ((((LastPos.X / caseW) - (int)(LastPos.X / caseW)) >= 0.5) ? 0.5 : 0);
            SecondLayer.Width = Convert.ToInt32(Math.Truncate(posx * caseW));
        }

        public bool setRate(double rate)
        {
            if (rate < 0 || rate > Max) { return false; }
            _Rate = rate; reDraw();
            RateChanged?.Invoke(_Rate);
            return true;
        }

        public void reDraw() { SecondLayer.Width = Convert.ToInt32(Math.Truncate(StarCaseWidth * _Zoom * _Rate)); }
    }
}
