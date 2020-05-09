using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Input;
using System.Drawing;
using System.Diagnostics;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private void UpdateSize(string size) { DisplayPlaybackSize.Content = size; }
        private void UpdatePosition(string position) { DisplayPlaybackPosition.Content = position; }
        private void UpdatePositionBar(double position) { 
            DisplayPlaybackPositionBar.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, new DoubleAnimation((double)position, duration));
        }

        private void DisplayPlaybackPositionBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double MousePosition = e.GetPosition(DisplayPlaybackPositionBar).X;

            //DisplayPlaybackPositionBar.Value = DisplayPlaybackPositionBar.Minimum;
            double ratio = MousePosition / DisplayPlaybackPositionBar.ActualWidth;
            double ProgressBarValue = ratio * DisplayPlaybackPositionBar.Maximum;

            // Set the calculated relative value to the progressbar //
            DisplayPlaybackPositionBar.Value = ProgressBarValue;
            long calc = (long)(ratio * player.Length());
            player.Position(null, calc);
            if (!player.IsPlaying()) { player.Play(); }
        }

        private void DisplayPlaybackPositionBar_MouseMove(object sender, MouseEventArgs e)
        {
            double MousePosition = e.GetPosition(DisplayPlaybackPositionBar).X;

            //DisplayPlaybackPositionBar.Value = DisplayPlaybackPositionBar.Minimum;
            double ratio = MousePosition / DisplayPlaybackPositionBar.ActualWidth;
            long calc = (long)(ratio * player.Length());

            DisplayPlaybackPositionBar.ToolTip = displayTime(calc);
        }

    }
}
