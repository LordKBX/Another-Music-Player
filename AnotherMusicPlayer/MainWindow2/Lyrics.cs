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

namespace AnotherMusicPlayer.MainWindow2Space
{
    public partial class Lyrics : Form
    {
        public Lyrics(MediaItem data)
        {
            InitializeComponent();
            AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(this);
            string title = data.Name;
            string arts = data.Artists;
            if (data.Album != null && data.Album.Trim().Length > 0) { title += " - " + data.Album.Trim(); }
            else if (arts != null && arts.Trim().Length > 0) { title += " - " + arts.Trim(); }
            Text = this.TitleLabel.Text = title;
            richTextBox1.Text = data.Lyrics;
            App.SetToolTip(this.TitleLabel, this.TitleLabel.Text);

            MinimizeButton.Click += MinimizeButton_Click;
            MaximizeButton.Click += MaximizeButton_Click;
            CloseButton.Click += CloseButton_Click;

            #region Window displasment gestion
            MainWIndowHead.MouseDown += FormDragable_MouseDown;
            MainWIndowHead.MouseMove += FormDragable_MouseMove;
            MainWIndowHead.MouseUp += FormDragable_MouseUp;
            TitleLabel.MouseDown += FormDragable_MouseDown;
            TitleLabel.MouseMove += FormDragable_MouseMove;
            TitleLabel.MouseUp += FormDragable_MouseUp;
            #endregion

            #region Window resize gestion
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.ResizeRedraw = true;
            GripButton.Tag = "MainWindow";
            GripButton.MouseDown += SizerMouseDown;
            GripButton.MouseMove += SizerMouseMove;
            GripButton.MouseUp += SizerMouseUp;
            #endregion
        }


        #region Generic Window Button
        public void MinimizeButton_Click(object? sender, EventArgs? e) { WindowState = FormWindowState.Minimized; }
        public void MaximizeButton_Click(object? sender, EventArgs? e)
        {
            if (WindowState == FormWindowState.Maximized) { this.MaximumSize = new System.Drawing.Size(0, 0); WindowState = FormWindowState.Normal; }
            else
            {
                Screen screen = Screen.FromControl(this);
                int x = screen.WorkingArea.X - screen.Bounds.X;
                int y = screen.WorkingArea.Y - screen.Bounds.Y;
                this.MaximizedBounds = new Rectangle(x, y, screen.WorkingArea.Width, screen.WorkingArea.Height);
                this.MaximumSize = screen.WorkingArea.Size;
                WindowState = FormWindowState.Maximized;
            }
        }
        public void CloseButton_Click(object? sender, EventArgs? e) { App.DelToolTip(this.TitleLabel);  Close(); }
        #endregion

        #region Window displasment gestion
        private Dictionary<string, bool> draggings = new Dictionary<string, bool>();
        private Dictionary<string, System.Drawing.Point> dragCursorPoints = new Dictionary<string, System.Drawing.Point>();
        private Dictionary<string, System.Drawing.Point> dragFormPoints = new Dictionary<string, System.Drawing.Point>();
        private Dictionary<string, Form> dragForms = new Dictionary<string, Form>();

        private void FormDragable_InitTab(object sender, bool active)
        {
            try
            {
                TableLayoutPanel label1 = (TableLayoutPanel)sender;
                string label = label1.Tag as string;
                Control parent = label1.Parent;
                while (parent.GetType().Name == "TableLayoutPanel") { parent = parent.Parent; }

                draggings.Add(label, active);
                dragCursorPoints.Add(label, System.Windows.Forms.Cursor.Position);
                dragFormPoints.Add(label, label1.Location);
                dragForms.Add(label, (Form)parent);
            }
            catch (Exception) { }
        }

        public void FormDragable_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender == null) { return; }
            while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
            string label = ((TableLayoutPanel)sender).Tag as string;
            if (!draggings.ContainsKey(label)) { FormDragable_InitTab(sender, true); }
            else
            {
                draggings[label] = true;
                dragCursorPoints[label] = System.Windows.Forms.Cursor.Position;
                dragFormPoints[label] = dragForms[label].Location;
            }
        }

        public void FormDragable_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (sender == null) { return; }
                while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
                string label = ((TableLayoutPanel)sender).Tag as string;
                if (!draggings.ContainsKey(label)) { FormDragable_InitTab(sender, false); }
                if (draggings[label])
                {
                    System.Drawing.Point dif = System.Drawing.Point.Subtract(System.Windows.Forms.Cursor.Position, new System.Drawing.Size(dragCursorPoints[label]));
                    dragForms[label].Location = System.Drawing.Point.Add(dragFormPoints[label], new System.Drawing.Size(dif));
                }
            }
            catch (Exception) { }
        }

        public void FormDragable_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender == null) { return; }
            while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
            string label = ((TableLayoutPanel)sender).Tag as string;
            draggings[label] = false;
        }

        public void FormDragable_Clear(string id)
        {
            if (draggings.ContainsKey(id))
            {
                draggings.Remove(id);
                dragCursorPoints.Remove(id);
                dragFormPoints.Remove(id);
                dragForms.Remove(id);
            }
        }
        #endregion

        #region Window resize gestion
        private bool IsResizing = false;
        private int ResizePosX = 0;
        private int ResizePosY = 0;
        private int ResizeSizeW = 0;
        private int ResizeSizeH = 0;

        private void SizerInitTab(object sender, bool active, int x, int y)
        {
            try
            {
                System.Windows.Forms.Button label1 = (System.Windows.Forms.Button)sender;
                string label = label1.Tag as string;
                if (IsResizing) { return; }
                IsResizing = true;
                ResizePosX = x;
                ResizePosY = y;
                ResizeSizeW = this.Width;
                ResizeSizeH = this.Height;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void SizerMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                string label = ((System.Windows.Forms.Button)sender).Tag as string;
                if (!IsResizing) { SizerInitTab(sender, true, Cursor.Position.X, Cursor.Position.Y); }
                else
                {
                    IsResizing = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
        public void SizerMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                Debug.WriteLine("SizerMouseMove");
                if (!IsResizing) { return; }
                if (IsResizing)
                {
                    Debug.WriteLine("Calculate");
                    this.Width = Cursor.Position.X - ResizePosX + ResizeSizeW;
                    this.Height = Cursor.Position.Y - ResizePosY + ResizeSizeH;


                    Debug.WriteLine("ResizeSizeW = " + ResizeSizeW);
                    Debug.WriteLine("ResizeSizeH = " + ResizeSizeH);
                    Debug.WriteLine("Width = " + Width);
                    Debug.WriteLine("Height = " + Height);

                    this.ResizeRedraw = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
        public void SizerMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try { IsResizing = false; }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void SizerClear(string id)
        {
            if (IsResizing) { IsResizing = false; }
        }
        #endregion
    }
}
