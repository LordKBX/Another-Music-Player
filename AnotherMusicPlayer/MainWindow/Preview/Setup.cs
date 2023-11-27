using System;
using System.Windows;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Windows.Interop;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Controls;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary>Custom Window Thumbnail object </summary>
        private TabbedThumbnail customThumbnail;
        private System.Drawing.Rectangle customThumbnailRectangle = new System.Drawing.Rectangle(0, 0, 550, 200);

        private System.Drawing.Icon IconPrev = null;
        private System.Drawing.Icon IconPlay = null;
        private System.Drawing.Icon IconPause = null;
        private System.Drawing.Icon IconNext = null;

        private ThumbnailToolBarButton buttonPrev = null;
        private ThumbnailToolBarButton buttonPlay = null;
        private ThumbnailToolBarButton buttonNext = null;

        private Preview PreviewControl = null;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        /// <summary> Setup preview for Windows Aero Peek </summary>
        private void PreviewSetUp()
        {
            PreviewControl = new Preview(this);
            PreviewControl.Width = customThumbnailRectangle.Width;
            PreviewControl.Height = customThumbnailRectangle.Height;
            PreviewControl.Measure(new System.Windows.Size(customThumbnailRectangle.Width, customThumbnailRectangle.Height));
            PreviewControl.Arrange(new Rect(new System.Windows.Size(customThumbnailRectangle.Width, customThumbnailRectangle.Height)));
            PreviewControl.UpdateLayout();

            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            customThumbnail = new TabbedThumbnail(windowHandle, windowHandle);

            IntPtr Hicon1 = Properties.Resources.previous_24.GetHicon();
            IconPrev = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(Hicon1).Clone();
            DestroyIcon(Hicon1);

            IntPtr Hicon2 = Properties.Resources.play_24.GetHicon();
            IconPlay = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(Hicon2).Clone();
            DestroyIcon(Hicon2);

            IntPtr Hicon3 = Properties.Resources.pause_24.GetHicon();
            IconPause = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(Hicon3).Clone();
            DestroyIcon(Hicon3);

            IntPtr Hicon4 = Properties.Resources.next_24.GetHicon();
            IconNext = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(Hicon4).Clone();
            DestroyIcon(Hicon4);

            buttonPrev = new ThumbnailToolBarButton(IconPrev, "test");
            buttonPrev.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { PreviousTrack(); };
            buttonPlay = new ThumbnailToolBarButton(IconPlay, "IconPlay");
            buttonPlay.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { Pause(); };
            buttonNext = new ThumbnailToolBarButton(IconNext, "IconNext");
            buttonNext.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { NextTrack(); };

            TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(customThumbnail);
            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(windowHandle, new ThumbnailToolBarButton[] { buttonPrev, buttonPlay, buttonNext });
            customThumbnail.TabbedThumbnailBitmapRequested += CustomThumbnail_TabbedThumbnailBitmapRequested;
            customThumbnail.TabbedThumbnailActivated += CustomThumbnail_TabbedThumbnailActivated;
            customThumbnail.TabbedThumbnailMaximized += CustomThumbnail_TabbedThumbnailMaximized;
            customThumbnail.TabbedThumbnailClosed += CustomThumbnail_TabbedThumbnailClosed;
            this.StateChanged += MainWindow_StateChanged;

            customThumbnail.SetWindowIcon(Properties.Resources.icon_large);
        }

        private void CustomThumbnail_TabbedThumbnailMaximized(object sender, TabbedThumbnailEventArgs e)
        { CustomThumbnail_TabbedThumbnailActivated(sender, e); }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized) { Opacity = 0; CustomThumbnailDisplayed = true; }
            if (this.WindowState == WindowState.Normal) { Opacity = 1; CustomThumbnailDisplayed = false; }
            if (this.WindowState == WindowState.Maximized) { Opacity = 1; CustomThumbnailDisplayed = false; }
            
        }

        private void CustomThumbnail_TabbedThumbnailClosed(object sender, TabbedThumbnailClosedEventArgs e) { Close(); }

        private void CustomThumbnail_TabbedThumbnailActivated(object sender, TabbedThumbnailEventArgs e)
        {
            Opacity = 1; CustomThumbnailDisplayed = false;
            this.Topmost = true;
            Show();
            this.Topmost = false;
        }

        private bool CustomThumbnailDisplayed = false;
        private int CustomThumbnailCouter = 0;
        private void CustomThumbnail_TabbedThumbnailBitmapRequested(object sender, TabbedThumbnailBitmapRequestedEventArgs e)
        {
            if (sender != null) { Opacity = 0; }
            if (player.GetCurrentFile() == PreviewControl.path) { PreviewControl.Update(); }
            else { PreviewControl.UpdateFile(player.GetCurrentFile()); }
            //customThumbnail.InvalidatePreview();
            if (CustomThumbnailDisplayed == false || CustomThumbnailCouter >= 10)
            {
                Bitmap bt = GeneratePreview();
                //Bitmap bt = GeneratePreview(MainWindow.Instance);
                customThumbnail.SetImage(bt);
                bt.Dispose();
                CustomThumbnailCouter = 0;
                CustomThumbnailDisplayed = true;
            }
            Task.Delay(100);
            CustomThumbnailCouter += 1;
        }

        private void InvalidateThumbnail() { customThumbnail.InvalidatePreview(); }

        private Bitmap GeneratePreview(Window? ctl = null)
        {
            System.Drawing.Bitmap im2 = null;
            int width = 0;
            int height = 0;
            RenderTargetBitmap bmp;
            //if (ctl != null)
            //{
            //    string sw = "" + ctl.Width;
            //    string sh = "" + ctl.Height;
            //    sw = sw.Substring(0, sw.IndexOf(","));
            //    sh = sh.Substring(0, sh.IndexOf(","));

            //    width = int.Parse(sw);
            //    height = int.Parse(sh);
            //    bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);

            //    bmp.Render(ctl);
            //}
            //else
            //{
                width = customThumbnailRectangle.Width;
                height = customThumbnailRectangle.Height;
                bmp = new RenderTargetBitmap(customThumbnailRectangle.Width, customThumbnailRectangle.Height, 96, 96, PixelFormats.Pbgra32);

                bmp.Render(PreviewControl);
            //}

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            MemoryStream stm = new MemoryStream();
            encoder.Save(stm); stm.Seek(0, SeekOrigin.Begin);
            im2 = FilesTags.ResizeImage(System.Drawing.Image.FromStream(stm), width, height);
            stm.Close();
            stm.Dispose();

            return im2;
        }

    }
}