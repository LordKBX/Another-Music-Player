using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : System.Windows.Window
    {
        // SECTION LOADING SCREEN
        private string _inloading;
        public string InLoading
        {
            get { return _inloading; }
            set { _inloading = value; }
        }
        public bool isLoading() { return dialog1.IsOpen; }

        public void setLoadingState(bool state, string text = "Loading", bool main = false)
        {
            if (state == true)
            {
                if (main == false) { _ = Dispatcher.BeginInvoke(new Action(() => { InLoading = "Running"; dialog1.IsOpen = true; dialog1Text.Text = text; })); }
                else { InLoading = "Running"; dialog1.IsOpen = true; dialog1Text.Text = text; }
            }
            else
            {
                if (main == false) { _ = Dispatcher.BeginInvoke(new Action(() => { InLoading = "Stopped"; dialog1.IsOpen = false; })); }
                else { InLoading = "Stopped"; dialog1.IsOpen = false; }
            }
        }

        // SECTION METADATA SCAN INDICATOR
        private string _MetadataScanning;
        public string MetadataScanning
        {
            get { return _MetadataScanning; }
            set { _MetadataScanning = value; }
        }
        public void setMetadataScanningState(bool state, int nb = 0)
        {
            if (state == true)
            {
                _ = Dispatcher.InvokeAsync(new Action(() => { BtnScanMetadata.Visibility = Visibility.Visible; MetadataScanning = "Visible"; BtnScanMetadataNb.Text = "" + nb; }));
            }
            else
            {
                _ = Dispatcher.InvokeAsync(new Action(() => { BtnScanMetadata.Visibility = Visibility.Hidden; MetadataScanning = "Hidden"; }));
            }
        }

        private enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT }
        private struct WorkingAreaSize
        {
            public double Width { get; set; }
            public double Height { get; set; }
        }
        private struct WorkingAreaPosition
        {
            public double X1 { get; set; }
            public double Y1 { get; set; }
            public double X2 { get; set; }
            public double Y2 { get; set; }
        }

        private TaskBarLocation GetTaskBarLocation()
        {
            TaskBarLocation taskBarLocation = TaskBarLocation.BOTTOM;
            bool taskBarOnTopOrBottom = (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width);
            if (taskBarOnTopOrBottom) { if (Screen.PrimaryScreen.WorkingArea.Top > 0) taskBarLocation = TaskBarLocation.TOP; }
            else { if (Screen.PrimaryScreen.WorkingArea.Left > 0) { taskBarLocation = TaskBarLocation.LEFT; } else { taskBarLocation = TaskBarLocation.RIGHT; } }
            return taskBarLocation;
        }

        private WorkingAreaSize GetWorkingAreaSize()
        {
            return new WorkingAreaSize()
            {
                Width = Screen.PrimaryScreen.WorkingArea.Width,
                Height = Screen.PrimaryScreen.WorkingArea.Height
            };
        }

        private WorkingAreaPosition GetWorkingAreaPosition()
        {
            return new WorkingAreaPosition()
            {
                X1 = Screen.PrimaryScreen.WorkingArea.Left,
                X2 = Screen.PrimaryScreen.WorkingArea.Right,
                Y1 = Screen.PrimaryScreen.WorkingArea.Top,
                Y2 = Screen.PrimaryScreen.WorkingArea.Bottom
            };
        }
    }
}
