using System;
using System.Windows;
using System.Diagnostics;

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

        // SECTION HIDE IN RELEASE MODE
        [Conditional("RELEASE")]
        private void HideDebug()
        {
            BtnDebug.Visibility = Visibility.Collapsed;
            SideBtnsGrid.RowDefinitions[7].Height = new GridLength(0);
        }
    }
}
