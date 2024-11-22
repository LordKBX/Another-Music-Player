using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using AnotherMusicPlayer;

namespace AnotherMusicPlayer.MainWindow2Space
{
    public static class KeyboardLocal
    {
        private static Keys PreviousKeyboardKey = Keys.None;
        private static double PreviousKeyboardTime = 0;
        private static MainWindow2 parent = null;

        public static void Init(MainWindow2 form)
        {
            parent = form;
            form.PreviewKeyDown += LocalControlEvent;
            List<Control> lcl = ListSubControls(form);
            Type buttonType = typeof(Button);
            foreach (Control ctrl in lcl) 
            { 
                try { 
                    ctrl.PreviewKeyDown += LocalControlEvent;
                    if (ctrl.GetType() == buttonType) { ctrl.TabStop = false; }
                } catch (Exception) { } 
            }
        }

        private static List<Control> ListSubControls(Control ctrl)
        {
            List<Control> lcl = new List<Control>();
            foreach (Control ctrlSub in ctrl.Controls) {
                lcl.Add(ctrlSub);
                if (ctrlSub.Controls.Count > 0) { lcl.AddRange(ListSubControls(ctrlSub)); }
            }
            return lcl;
        }

        private static void LocalControlEvent(object? sender, PreviewKeyDownEventArgs e) 
        {  // intercept keyboard event on UI to prevent selected button activation via keyboard
            if (sender == null) { return; }
            if (sender.GetType() == typeof(TextBox)) { return; }
            try
            {
                double ntime = App.UnixTimestamp();

                List<Keys> autorised = new List<Keys>() { Keys.Space, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
                if (!autorised.Contains(e.KeyCode)) { return; }
                if ((PreviousKeyboardKey == Keys.LControlKey || PreviousKeyboardKey == Keys.RControlKey) && (PreviousKeyboardTime + 1 > ntime))
                {
                    if (e.KeyCode == Keys.Left) { Player.Stop(Player.GetCurrentFile()); Player.PlaylistPrevious(); }
                    if (e.KeyCode == Keys.Right) { Player.Stop(Player.GetCurrentFile()); Player.PlaylistNext(); }
                }
                else
                {
                    if (e.KeyCode == Keys.Space)
                    {
                        if (Player.IsPlaying()) { Player.Pause(); } else { Player.Play(); }
                    }

                    if (e.KeyCode == Keys.Left) { Player.PlayTimeRewind(5); }
                    if (e.KeyCode == Keys.Right) { Player.PlayTimeAdvance(5); }
                    if (e.KeyCode == Keys.Up)
                    {
                        if (parent.TabControler.SelectedIndex == 0)
                        {
                            if (parent.PlaybackTabDataGridView.SelectedRows.Count > 0)
                            {
                                if (parent.PlaybackTabDataGridView.SelectedRows[0].Index > 0)
                                { parent.PlaybackTabDataGridView.Rows[parent.PlaybackTabDataGridView.SelectedRows[0].Index - 1].Selected = true; }
                            }
                        }
                        else if (parent.TabControler.SelectedIndex == 1)
                        {
                            if (parent.LibraryTabSplitContainer.Panel1Collapsed) { parent.LibibrarySearchContent.VerticalScroll.Value -= 15; }
                            else { parent.LibibraryNavigationContent.VerticalScroll.Value -= 15; }
                        }
                    }
                    if (e.KeyCode == Keys.Down)
                    {
                        if (parent.TabControler.SelectedIndex == 0)
                        {
                            if (parent.PlaybackTabDataGridView.SelectedRows.Count > 0)
                            {
                                if (parent.PlaybackTabDataGridView.SelectedRows[0].Index < parent.PlaybackTabDataGridView.Rows.Count - 1)
                                { parent.PlaybackTabDataGridView.Rows[parent.PlaybackTabDataGridView.SelectedRows[0].Index + 1].Selected = true; }
                            }
                        }
                        else if (parent.TabControler.SelectedIndex == 1)
                        {
                            if (parent.LibraryTabSplitContainer.Panel1Collapsed) { parent.LibibrarySearchContent.VerticalScroll.Value += 15; }
                            else { parent.LibibraryNavigationContent.VerticalScroll.Value += 15; }
                        }
                    }
                }
                PreviousKeyboardKey = e.KeyCode;
                PreviousKeyboardTime = ntime;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }
    }
}
