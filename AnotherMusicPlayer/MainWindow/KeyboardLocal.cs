using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        string PreviousKeyboardKey = "";
        double PreviousKeyboardTime = 0;

        private void KeyboardLocalListenerInit(){
            this.PreviewKeyDown += (s, e) => {  // intercept keyboard event on UI to prevent selected button activation via keyboard
                if (e.OriginalSource.ToString().StartsWith("System.Windows.Controls.TextBox")) { return; }
                double ntime = UnixTimestamp();

                string key = e.Key.ToString(); Debug.WriteLine(key);
                Debug.WriteLine("((" + e.KeyStates.ToString() + "))");
                //List<string> autorised = new List<string>() { "Left", "Right", "Up", "Down" };
                List<string> autorised = new List<string>() { "Space", "Left", "Right", "Up", "Down" };
                if (!autorised.Contains(key)) { e.Handled = true; }
                if ((PreviousKeyboardKey == "LeftCtrl" || PreviousKeyboardKey == "RightCtrl") && (PreviousKeyboardTime + 1 > ntime))
                {
                    if (key == "Left") { PreviousTrack(); }
                    if (key == "Right") { NextTrack(); }
                }
                else
                {
                    if (key == "Space") { Pause(); }

                    if (key == "Left") { LibraryFiltersPaginationPrevious_Click(null, null); }
                    if (key == "Right") { LibraryFiltersPaginationNext_Click(null, null); }
                    if (key == "Up")
                    {
                        if (TabControler.SelectedIndex == 0)
                        {
                            if (PlayListView.SelectedIndex > 0)
                            {
                                PlayListView.SelectedIndex = PlayListView.SelectedIndex - 1;
                                PlayListView.ScrollIntoView(PlayListView.SelectedItem);
                            }
                        }
                        else if (TabControler.SelectedIndex == 1)
                        {
                            LibNavigationContentScroll.ScrollToVerticalOffset(LibNavigationContentScroll.VerticalOffset - 15);
                            LibNavigationContentScroll2.ScrollToVerticalOffset(LibNavigationContentScroll2.VerticalOffset - 15);
                        }
                    }
                    if (key == "Down")
                    {
                        if (TabControler.SelectedIndex == 0)
                        {
                            if (PlayListView.SelectedIndex < PlayListView.Items.Count - 1)
                            {
                                PlayListView.SelectedIndex = PlayListView.SelectedIndex + 1;
                                PlayListView.ScrollIntoView(PlayListView.SelectedItem);
                            }
                        }
                        else if (TabControler.SelectedIndex == 1)
                        {
                            LibNavigationContentScroll.ScrollToVerticalOffset(LibNavigationContentScroll.VerticalOffset + 15);
                            LibNavigationContentScroll2.ScrollToVerticalOffset(LibNavigationContentScroll2.VerticalOffset + 15);
                        }
                    }
                }
                PreviousKeyboardKey = key;
                PreviousKeyboardTime = ntime;
                e.Handled = true;
            };
        }
    }
}
