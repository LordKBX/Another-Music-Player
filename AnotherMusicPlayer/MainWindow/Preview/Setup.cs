﻿using System;
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
            buttonPrev.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { PreviousTrack(); CustomThumbnail_TabbedThumbnailBitmapRequested(null, null); };
            buttonPlay = new ThumbnailToolBarButton(IconPlay, "IconPlay");
            buttonPlay.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { Pause(); };
            buttonNext = new ThumbnailToolBarButton(IconNext, "IconNext");
            buttonNext.Click += (object sender, ThumbnailButtonClickedEventArgs e) => { NextTrack(); CustomThumbnail_TabbedThumbnailBitmapRequested(null, null); };

            TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(customThumbnail);
            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(windowHandle, new ThumbnailToolBarButton[] { buttonPrev, buttonPlay, buttonNext });
            customThumbnail.TabbedThumbnailBitmapRequested += CustomThumbnail_TabbedThumbnailBitmapRequested;
            customThumbnail.TabbedThumbnailActivated += CustomThumbnail_TabbedThumbnailActivated;
            customThumbnail.TabbedThumbnailClosed += CustomThumbnail_TabbedThumbnailClosed;

            customThumbnail.SetWindowIcon(Properties.Resources.icon_large);

            PreviewControl = new Preview(this);
            PreviewControl.Measure(new System.Windows.Size(customThumbnailRectangle.Width, customThumbnailRectangle.Height));
            PreviewControl.Arrange(new Rect(new System.Windows.Size(customThumbnailRectangle.Width, customThumbnailRectangle.Height)));
            PreviewControl.TabIndex = -10;
        }

        private void CustomThumbnail_TabbedThumbnailClosed(object sender, TabbedThumbnailClosedEventArgs e) { Close(); }

        private void CustomThumbnail_TabbedThumbnailActivated(object sender, TabbedThumbnailEventArgs e) { Show(); }

        private void CustomThumbnail_TabbedThumbnailBitmapRequested(object sender, TabbedThumbnailBitmapRequestedEventArgs e)
        {
            if (player.GetCurrentFile() == PreviewControl.path) { PreviewControl.Update(); }
            else { PreviewControl.UpdateFile(player.GetCurrentFile()); }
            customThumbnail.SetImage(GeneratePreview());
            Task.Delay(1000);
        }

        private void InvalidateThumbnail() { customThumbnail.InvalidatePreview(); }

        private Bitmap GeneratePreview()
        {
            System.Drawing.Bitmap im2 = null;
            RenderTargetBitmap bmp = new RenderTargetBitmap(customThumbnailRectangle.Width, customThumbnailRectangle.Height, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(PreviewControl);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            MemoryStream stm = new MemoryStream();
            encoder.Save(stm); stm.Seek(0, SeekOrigin.Begin);
            im2 = FilesTags.ResizeImage(Image.FromStream(stm), customThumbnailRectangle.Width, customThumbnailRectangle.Height);
            stm.Close();

            return im2;
        }

    }
}