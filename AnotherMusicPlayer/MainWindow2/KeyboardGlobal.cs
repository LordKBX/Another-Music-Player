using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Collections.Generic;

namespace AnotherMusicPlayer.MainWindow2Space
{
    public static class KeyboardGlobal
    {
        private static GlobalKeyboardListener _listener = null;
        /// <summary>
        /// Initialize Global Keyboard listener for Media Buttons
        /// </summary>
        public static void Init()
        {
            if (_listener != null) { return; }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _listener = new GlobalKeyboardListener();
                _listener.OnKeyPressed += (sender, e) =>
                {
                    try
                    {
                        string re = e.KeyPressed.ToString();
                        //Debug.WriteLine(re);
                        //Debug.WriteLine("Focus = " + ((this.IsActive) ? "True" : "False") );

                        if (re == "MediaPlayPause")
                        {
                            try { if (Player.IsPlaying()) { Player.Pause(); } else { Player.Play(); } }
                            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
                        }
                        if (re == "MediaPreviousTrack") { Player.Stop(Player.GetCurrentFile()); Player.PlaylistPrevious(); }
                        if (re == "MediaNextTrack") { Player.Stop(Player.GetCurrentFile()); Player.PlaylistNext(); }
                    }
                    catch(Exception ex0) { Debug.WriteLine(ex0.Message + "\r\n" + ex0.StackTrace); }
                };
                _listener.HookKeyboard();
            }
        }

        /// <summary>
        /// Destroy Global Keyboard listener
        /// </summary>
        public static void Kill()
        {
            if (_listener == null) { return; }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { _listener.UnHookKeyboard(); } 
        }
    }

    /// <summary>
    /// Global Keyboard Listener Class For Windows OS
    /// </summary>
    public class GlobalKeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public GlobalKeyboardListener()
        {
            _proc = HookCallback;
        }

        public void HookKeyboard()
        {
            _hookID = SetHook(_proc);
        }

        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (OnKeyPressed != null) { OnKeyPressed(this, new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(vkCode))); }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }

    /// <summary>
    /// Return Class Object For Global Keyboard Listener Class For Windows OS
    /// </summary>
    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }

        public KeyPressedArgs(Key key)
        {
            KeyPressed = key;
        }
    }
}