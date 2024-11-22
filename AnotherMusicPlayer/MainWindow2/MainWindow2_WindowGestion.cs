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
using Timer = System.Windows.Forms.Timer;
using ByteDev.Strings;
using CustomExtensions;
using Manina.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.ObjectModel;
using Sprache;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using NAudio.Gui;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Markup;

namespace AnotherMusicPlayer.MainWindow2Space
{
    public partial class MainWindow2 : Form
    {
        private void MainWindow2_SizeChanged(object sender, EventArgs e)
        {
            TabControler.TabSize = new System.Drawing.Size((TabControler.Width) / TabControler.Tabs.Count, 50);
            if (Width > 800 && Height > 700)
            { PlaybackTabMainTableLayoutPanel.ColumnStyles[0].Width = 250; PlaybackTabLeftTableLayoutPanel.RowStyles[0].Height = 250; }
            else
            { PlaybackTabMainTableLayoutPanel.ColumnStyles[0].Width = 150; PlaybackTabLeftTableLayoutPanel.RowStyles[0].Height = 150; }
            if(PlaybackTabRatting != null)
            PlaybackTabRatting.Margin = new Padding(Convert.ToInt32(Math.Truncate((PlaybackTabLeftBottomFlowLayoutPanel.Width - PlaybackTabRatting.Width) / 2.0)), 3, 0, 0);
        }

        public void SetTitle(string title)
        {
            if (title != null && title.Trim() != "")
            {
                this.Text = title;
                try { customThumbnail.Title = title; } catch { }
                this.TitleLabel.Text = title;
            }
            else
            {
                try { customThumbnail.Title = App.AppName; } catch { }
                this.TitleLabel.Text = App.AppName;
            }
            App.SetToolTip(this.TitleLabel, this.TitleLabel.Text);
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
        public void CloseButton_Click(object? sender, EventArgs? e) { Close(); }
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
