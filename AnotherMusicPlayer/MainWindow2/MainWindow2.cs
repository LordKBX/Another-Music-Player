using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using Control = System.Windows.Forms.Control;
using Button = System.Windows.Forms.Button;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using ByteDev.Strings;
using CustomExtensions;
using System.Windows.Controls;
using Manina.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2
{
    public partial class MainWindow2 : Form
    {
        private System.Windows.Forms.Timer? LoadTimer = new System.Windows.Forms.Timer();

        private static SolidColorBrush DefaultBrush = new SolidColorBrush(Colors.White);

        private static readonly int ButtonIconSize = 24;
        private static Bitmap IconPlayback = Icons.FromIconKind(IconKind.Music, ButtonIconSize, DefaultBrush);
        private static Bitmap IconLibrary = Icons.FromIconKind(IconKind.FolderMusic, ButtonIconSize, DefaultBrush);
        private static Bitmap IconPlayLists = Icons.FromIconKind(IconKind.PlaylistMusic, ButtonIconSize, DefaultBrush);
        private static Bitmap IconSettings = Icons.FromIconKind(IconKind.Cog, ButtonIconSize, DefaultBrush);

        private static Bitmap IconOpen = Icons.FromIconKind(IconKind.FolderOpen, ButtonIconSize, DefaultBrush);
        private static Bitmap IconPrevious = Icons.FromIconKind(IconKind.SkipBackward, ButtonIconSize, DefaultBrush);
        private static Bitmap IconPlay = Icons.FromIconKind(IconKind.Play, ButtonIconSize, DefaultBrush);
        private static Bitmap IconPause = Icons.FromIconKind(IconKind.Pause, ButtonIconSize, DefaultBrush);
        private static Bitmap IconNext = Icons.FromIconKind(IconKind.SkipForward, ButtonIconSize, DefaultBrush);
        private static Bitmap IconRepeat = Icons.FromIconKind(IconKind.Repeat, ButtonIconSize, DefaultBrush);
        private static Bitmap IconRepeatOnce = Icons.FromIconKind(IconKind.RepeatOnce, ButtonIconSize, DefaultBrush);
        private static Bitmap IconRepeatOff = Icons.FromIconKind(IconKind.RepeatOff, ButtonIconSize, DefaultBrush);
        private static Bitmap IconShuffle = Icons.FromIconKind(IconKind.Shuffle, ButtonIconSize, DefaultBrush);
        private static Bitmap IconClearList = Icons.FromIconKind(IconKind.PlaylistRemove, ButtonIconSize, DefaultBrush);

        /// <summary> Object music player </summary>
        public MainWindow2()
        {
            InitializeComponent();

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

            try
            {
                playbackProgressBar.Value = 0;

                PlaybackTab.Icon = IconPlayback;
                LibraryTab.Icon = IconLibrary;
                PlayListsTab.Icon = IconPlayLists;
                SettingsTab.Icon = IconSettings;

                BtnOpen.BackgroundImage = IconOpen;
                BtnPrevious.BackgroundImage = IconPrevious;
                BtnPlayPause.BackgroundImage = (Player.LatestPlayerStatus == PlayerStatus.Play) ? IconPlay : IconPause;
                BtnNext.BackgroundImage = IconNext;
                BtnRepeat.BackgroundImage = (Player.IsRepeat()) ? IconRepeatOnce : IconRepeat;
                BtnShuffle.BackgroundImage = IconShuffle;
                BtnClearList.BackgroundImage = IconClearList;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }

            LoadTimer.Tick += LoadTimer_Tick;
            LoadTimer.Interval = 1000;
            LoadTimer.Start();
        }

        #region Generic Window Button
        public void MinimizeButton_Click(object? sender, EventArgs? e) { WindowState = FormWindowState.Minimized; }
        public void MaximizeButton_Click(object? sender, EventArgs? e)
        {
            if (WindowState == FormWindowState.Maximized) { this.MaximumSize = new Size(0, 0); WindowState = FormWindowState.Normal; }
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
        public void CloseButton_Click(object? sender, EventArgs? e) { Close(); }
        #endregion

        #region Window displasment gestion
        private Dictionary<string, bool> draggings = new Dictionary<string, bool>();
        private Dictionary<string, Point> dragCursorPoints = new Dictionary<string, Point>();
        private Dictionary<string, Point> dragFormPoints = new Dictionary<string, Point>();
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
                    Point dif = Point.Subtract(System.Windows.Forms.Cursor.Position, new Size(dragCursorPoints[label]));
                    dragForms[label].Location = Point.Add(dragFormPoints[label], new Size(dif));
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

        private void LoadTimer_Tick(object? sender, EventArgs e)
        {
            if (LoadTimer == null) { return; }
        }

        private void MainWindow2_SizeChanged(object sender, EventArgs e)
        {
            TabControler.TabSize = new Size((TabControler.Width) / TabControler.Tabs.Count, 50);
        }
    }
}
