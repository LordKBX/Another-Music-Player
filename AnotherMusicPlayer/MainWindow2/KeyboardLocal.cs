using AnotherMusicPlayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2Space
{
    public static class KeyboardLocal
    {
        private static Keys PreviousKeyboardKey = Keys.None;
        private static double PreviousKeyboardTime = 0;
        private static MainWindow2 parent = null;

        private static List<Func<object, PreviewKeyDownEventArgs, bool>> PreviewKeyDownFunctions = new List<Func<object, PreviewKeyDownEventArgs, bool>>();
        public static bool AddPreviewKeyDownFunction(Func<object, PreviewKeyDownEventArgs, bool> func) { try { PreviewKeyDownFunctions.Add(func); } catch (Exception) { return false; } return true; }
        public static void ClearPreviewKeyDownFunctions() { PreviewKeyDownFunctions.Clear(); }

        private static List<Func<object, KeyEventArgs, bool>> KeyDownFunctions = new List<Func<object, KeyEventArgs, bool>>();
        public static bool AddKeyDownFunction(Func<object, KeyEventArgs, bool> func) { try { KeyDownFunctions.Add(func); } catch (Exception) { return false; } return true; }
        public static void ClearKeyDownFunctions() { KeyDownFunctions.Clear(); }

        private static List<Func<object, KeyEventArgs, bool>> KeyUpFunctions = new List<Func<object, KeyEventArgs, bool>>();
        public static bool AddKeyUpFunction(Func<object, KeyEventArgs, bool> func) { try { KeyUpFunctions.Add(func); } catch (Exception) { return false; } return true; }
        public static void ClearKeyUpFunctions() { KeyUpFunctions.Clear(); }

        public static void Init(MainWindow2 form)
        {
            if (parent != null) { return; }

            parent = form;
            form.PreviewKeyDown += LocalControl_PreviewKeyDownEvent;
            form.KeyDown += LocalControl_KeyDownEvent;
            form.KeyUp += LocalControl_KeyUpEvent;

            List<Control> lcl = ListSubControls(form);
            Type buttonType = typeof(Button);
            foreach (Control ctrl in lcl) 
            { 
                try { 
                    ctrl.PreviewKeyDown += LocalControl_PreviewKeyDownEvent;
                    if (ctrl.GetType() == buttonType) { ctrl.TabStop = false; }
                    ctrl.KeyDown += LocalControl_KeyDownEvent;
                    ctrl.KeyUp += LocalControl_KeyUpEvent;
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

        private static void LocalControl_PreviewKeyDownEvent(object sender, PreviewKeyDownEventArgs e) 
        {  // intercept keyboard event on UI to prevent selected button activation via keyboard
            if (sender == null) { return; }
            if (sender.GetType() == typeof(TextBox)) { return; }
            if (parent.InvokeRequired) { parent.Invoke(() => { LocalControl_PreviewKeyDownEvent(sender, e); }); return; }
            try
            {
                foreach (Func<object, PreviewKeyDownEventArgs, bool> func in PreviewKeyDownFunctions) { func(sender, e); }

                if(e.Control && e.KeyCode == Keys.R) {
                    parent.Left = Screen.PrimaryScreen.Bounds.Left;
                    parent.Top = Screen.PrimaryScreen.Bounds.Top;
                    parent.Width = 1024;
                    parent.Height = 800;
                }
                double ntime = App.UnixTimestamp();

                List<Keys> autorised = new List<Keys>() { Keys.Space, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
                if (!autorised.Contains(e.KeyCode)) { return; }
                if ((PreviousKeyboardKey == Keys.LControlKey || PreviousKeyboardKey == Keys.RControlKey) && (PreviousKeyboardTime + 1 > ntime))
                {
                    if (e.KeyCode == Keys.Left) { if (Player.Mode == Player.Modes.Radio) { return; }; Player.Stop(Player.GetCurrentFile()); Player.PlaylistPrevious(); }
                    if (e.KeyCode == Keys.Right) { if (Player.Mode == Player.Modes.Radio) { return; }; Player.Stop(Player.GetCurrentFile()); Player.PlaylistNext(); }
                }
                else
                {
                    if (e.KeyCode == Keys.Space)
                    {
                        MainWindow2.PlayPause();
                    }

                    if (e.KeyCode == Keys.Left) { Player.PlayTimeRewind(5); }
                    if (e.KeyCode == Keys.Right) { Player.PlayTimeAdvance(5); }
                    if (e.KeyCode == Keys.Up)
                    {
                        if (parent.TabControler.SelectedIndex == 0)
                        {
                            //if (parent.PlaybackTabDataGridView.SelectedRows.Count > 0)
                            //{
                            //    int index = parent.PlaybackTabDataGridView.SelectedRows[0].Index - 1;
                            //    if (index >= 0 && index < parent.PlaybackTabDataGridView.Rows.Count){ parent.PlaybackTabDataGridView.Rows[index].Selected = true; }
                            //}
                        }
                        else if (parent.TabControler.SelectedIndex == 1)
                        {
                            if (parent.LibraryTabSplitContainer.Panel1Collapsed) 
                            { parent.LibrarySearchContent.AutoScrollOffset = new System.Drawing.Point(0, parent.LibrarySearchContent.AutoScrollOffset.Y - 15); }
                            else { 
                                parent.LibraryNavigationContent.VerticalScroll.Value -= 15; 
                            }
                        }
                    }
                    if (e.KeyCode == Keys.Down)
                    {
                        if (parent.TabControler.SelectedIndex == 0)
                        {
                            //if (parent.PlaybackTabDataGridView.SelectedRows.Count > 0)
                            //{
                            //    int index = parent.PlaybackTabDataGridView.SelectedRows[0].Index + 1;
                            //    if (index >= 0 && index < parent.PlaybackTabDataGridView.Rows.Count) { parent.PlaybackTabDataGridView.Rows[index].Selected = true; }
                            //}
                        }
                        else if (parent.TabControler.SelectedIndex == 1)
                        {
                            if (parent.LibraryTabSplitContainer.Panel1Collapsed) 
                            { parent.LibrarySearchContent.AutoScrollOffset = new System.Drawing.Point(0, parent.LibrarySearchContent.AutoScrollOffset.Y + 15); }
                            else { parent.LibraryNavigationContent.VerticalScroll.Value += 15; }
                        }
                    }
                }
                PreviousKeyboardKey = e.KeyCode;
                PreviousKeyboardTime = ntime;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }

        private static void LocalControl_KeyDownEvent(object sender, KeyEventArgs e)
        {
            foreach (Func<object, KeyEventArgs, bool> func in KeyDownFunctions) { func(sender, e); }
        }

        private static void LocalControl_KeyUpEvent(object sender, KeyEventArgs e)
        {
            foreach (Func<object, KeyEventArgs, bool> func in KeyUpFunctions) { func(sender, e); }
        }

        public static string KeyCodeToUnicode(Keys key)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }

            uint virtualKeyCode = (uint)key;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            StringBuilder result = new StringBuilder();
            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, (int)5, (uint)0, inputLocaleIdentifier);

            return result.ToString();
        }

        [DllImport("user32.dll")]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

    }
}
