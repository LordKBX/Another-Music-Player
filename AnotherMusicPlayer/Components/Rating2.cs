using NAudio.Gui;
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
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Color = System.Drawing.Color;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Rectangle = System.Drawing.Rectangle;

namespace AnotherMusicPlayer
{
    public delegate void Rating2RateChangedNotify(Rating2 sender, double value);
    public delegate void Rating2ZoomChangedNotify(Rating2 sender, double value);

    public partial class Rating2 : UserControl
    {
        private double Max = 5.0;
        private double StarCaseWidth = 20.0;
        private Rectangle[] Stars = new Rectangle[5];
        System.Drawing.Point LastPos = new System.Drawing.Point(0, 0);

        #region IsReadOnly
        private bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            set => _IsReadOnly = value;
        }
        #endregion  

        #region Zoom Property
        private double _Zoom = 1.0;
        public double Zoom
        {
            get { return _Zoom; }
            set { setZoom(value); }
        }
        public event Rating2ZoomChangedNotify ZoomChanged;

        public void setZoom(double multiply)
        {
            double caseSize = StarCaseWidth * multiply;
            //PointCollection StarMatrix = StarPoints;
            //for (int i = 0; i < 5; i++) { StarMatrix[i] = new Point(StarPoints[i].X * multiply, StarPoints[i].Y * multiply); }
            //foreach (Polygon star in Grid1.Children) { star.Width = caseSize; star.Points = StarMatrix; }
            //foreach (Polygon star in Grid2.Children) { star.Width = caseSize; star.Points = StarMatrix; }
            //_Zoom = multiply; ZoomChanged?.Invoke(this, _Zoom);
            CalculatePositions();
            reDraw();
        }
        #endregion

        #region Rate Property
        private double _Rate = 0;
        public double Rate
        {
            get { return _Rate; }
            set { setRate(value); }
        }
        public event Rating2RateChangedNotify RateChanged;
        #endregion

        static public Bitmap DrawImage(double value) {
            double val = value;
            if (val < 0) { val = 0.0; }
            if (val > 5.0) { val = 5.0; }
            if (val == 0.5) { return Properties.Resources.stars_0_5; }
            else if (val == 1.0) { return Properties.Resources.stars_1_0; }
            else if (val == 1.5) { return Properties.Resources.stars_1_5; }
            else if (val == 2.0) { return Properties.Resources.stars_2_0; }
            else if (val == 2.5) { return Properties.Resources.stars_2_5; }
            else if (val == 3.0) { return Properties.Resources.stars_3_0; }
            else if (val == 3.5) { return Properties.Resources.stars_3_5; }
            else if (val == 4.0) { return Properties.Resources.stars_4_0; }
            else if (val == 4.5) { return Properties.Resources.stars_4_5; }
            else if (val >= 5) { return Properties.Resources.stars_5_0; }
            return Properties.Resources.stars_0_0;
        }

        public Rating2()
        {
            InitializeComponent();
            tableLayoutPanel2.Width = 0;
            this.MouseMove += StarGrid_MouseMove;
            this.MouseLeave += Ratting2_MouseLeave;
            this.MouseUp += StarGrid_MouseUp;
            this.MouseDown += Ratting2_MouseDown;
            this.Resize += Rating2_Resize;
            this.BackColorChanged += Rating2_BackColorChanged;
            CalculatePositions();
        }

        private void Rating2_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Rating2_BackColorChanged => " + this.BackColor.ToString());
                this.FirstLayer.BackColor = this.BackColor;
                this.SecondLayer.BackColor = this.BackColor;
                this.tableLayoutPanel2.BackColor = this.BackColor;
                this.button1.BackColor = this.BackColor;
                this.button2.BackColor = this.BackColor;
                this.button3.BackColor = this.BackColor;
                this.button4.BackColor = this.BackColor;
                this.button5.BackColor = this.BackColor;
                this.button6.BackColor = this.BackColor;
                this.button7.BackColor = this.BackColor;
                this.button8.BackColor = this.BackColor;
                this.button9.BackColor = this.BackColor;
                this.button10.BackColor = this.BackColor;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void CalculatePositions()
        {
            int caseSize = Convert.ToInt32(StarCaseWidth * _Zoom);
            adjustStartButtons(button1, caseSize);
            adjustStartButtons(button2, caseSize);
            adjustStartButtons(button3, caseSize);
            adjustStartButtons(button4, caseSize);
            adjustStartButtons(button5, caseSize);
            adjustStartButtons(button6, caseSize);
            adjustStartButtons(button7, caseSize);
            adjustStartButtons(button8, caseSize);
            adjustStartButtons(button9, caseSize);
            adjustStartButtons(button10, caseSize);


            Stars[0] = new Rectangle(button1.Location.X, button1.Location.Y, button1.Width, button1.Height);
            Stars[1] = new Rectangle(button2.Location.X, button2.Location.Y, button2.Width, button2.Height);
            Stars[2] = new Rectangle(button3.Location.X, button3.Location.Y, button3.Width, button3.Height);
            Stars[3] = new Rectangle(button4.Location.X, button4.Location.Y, button4.Width, button4.Height);
            Stars[4] = new Rectangle(button5.Location.X, button5.Location.Y, button5.Width, button5.Height);
        }
        private void adjustStartButtons(Button btn, int size)
        {
            btn.Width = size; btn.Height = size;

        }

        private int getStarIndex(System.Drawing.Point pos)
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                if (Stars[i].Contains(pos)) { return i; }
            }
            return -1;
        }

        private (double, double) getStarValue(System.Drawing.Point pos)
        {
            int id = getStarIndex(pos);
            if (id == -1) { return (-1, -1); }
            //Debug.WriteLine("---------------------");
            //Debug.WriteLine("id = " + id);
            double note = id + 1;

            int val_min = Stars[id].X;
            int val_max = Stars[id].X + Stars[id].Width;
            double val_moy = val_min + (Stars[id].Width / 2.0);
            double val = pos.X;
            double tigger = Stars[id].Width * 0.2;
            //Debug.WriteLine("tigger = " + tigger);
            //Debug.WriteLine("val_min = " + val_min);
            //Debug.WriteLine("val_max = " + val_max);
            //Debug.WriteLine("val_moy = " + val_moy);
            //Debug.WriteLine("val = " + val);

            if (val >= val_moy + tigger) { val = val_max; note = id + 1; }
            else if (val <= val_moy - tigger) { val = val_min; note = id; }
            else { val = val_moy; note = id + 0.5; }
            //Debug.WriteLine("note = " + note);

            return (note, val);
        }

        private void Rating2_Resize(object sender, EventArgs e)
        { 
            CalculatePositions(); 
        }

        private bool IsDown = false;
        private void Ratting2_MouseDown(object sender, MouseEventArgs e)
        {
            IsDown = true; StarGrid_MouseUp(sender, e); 
        }

        private void Ratting2_MouseLeave(object sender, EventArgs e)
        {
            if (IsDown) { return; } reDraw(); 
        }

        private void StarGrid_MouseUp(object sender, EventArgs e)
        {
            if (_IsReadOnly) { return; }
            (double, double) v = getStarValue(LastPos);
            if (v.Item1 == -1) { return; }

            setRate(v.Item1);
            IsDown = false;
        }

        private void StarGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_IsReadOnly) { return; }
            (double, double) v = getStarValue(e.Location);
            if(v.Item1 == -1) { return; }

            LastPos = e.Location;
            SecondLayer.Width = Convert.ToInt32(v.Item2);
        }

        public bool setRate(double rate)
        {
            if (rate < 0 || rate > Max) { return false; }
            _Rate = rate; reDraw();
            RateChanged?.Invoke(this, _Rate);
            return true;
        }

        public void reDraw() {
            double val = 0;
            if (_Rate == 5) { val = Stars[4].X + Stars[4].Width; }
            else if (_Rate == 0) { val = Stars[0].X; }
            else
            {
                int id = Convert.ToInt32(Math.Truncate(_Rate));
                bool offset = _Rate - id >= 0.5;
                int val_min = Stars[id].X;
                int val_max = Stars[id].X + Stars[id].Width;
                double val_moy = val_min + (Stars[id].Width / 2.0);
                val = (offset ? val_moy : val_min);
            }


            SecondLayer.Width = Convert.ToInt32(val); 
        }
    }

    public class SaveRatingObejct {
        public uint Count { get; set; }
        public double Rate { get; set; }
        public string Path { get; set; }
    }
}
