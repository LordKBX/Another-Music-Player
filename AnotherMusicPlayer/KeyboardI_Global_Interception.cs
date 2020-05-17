using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Collections.Generic;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initialize Global Keyboard listener for Media Buttons
        /// </summary>
        private void KeyboardInterceptorSetUp()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                GlobalKeyboardListener _listener = new GlobalKeyboardListener(this);
                _listener.OnKeyPressed += (sender, e) => { 
                    string re = e.KeyPressed.ToString(); 
                    //Debug.WriteLine(re);
                    //Debug.WriteLine("Focus = " + ((this.IsActive) ? "True" : "False") );
                    
                    if (re == "MediaPlayPause") { Pause(); }
                    if (re == "MediaPreviousTrack") { PreviousTrack(); }
                    if (re == "MediaNextTrack") { NextTrack(); }
                };
                _listener.HookKeyboard();
                ListReferences.Add("KeyboardListener", _listener);
            }
        }

        /// <summary>
        /// Destroy Global Keyboard listener
        /// </summary>
        private void KeyboardInterceptorDestroy()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ((GlobalKeyboardListener)ListReferences["KeyboardListener"]).UnHookKeyboard();
            }
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

        private MainWindow parent;

        public GlobalKeyboardListener(MainWindow origin)
        {
            parent = origin;
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