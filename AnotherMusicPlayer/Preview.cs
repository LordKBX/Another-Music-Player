﻿using System.Windows;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary> Setup preview for Windows Aero Peek </summary>
        private void PreviewSetUp() {
            PreviewCtrlPause.Click += (sender, e) => { Pause(); };
            PreviewCtrlPrev.Click += (sender, e) => { PreviousTrack(); };
            PreviewCtrlNext.Click += (sender, e) => { NextTrack(); };

            PreviewCtrlPause.ImageSource = Bimage("MiniPlayButtonImg_Play");
            PreviewCtrlPrev.ImageSource = Bimage("MiniPreviousButtonImg");
            PreviewCtrlNext.ImageSource = Bimage("MiniNextButtonImg");
        }
    }
}