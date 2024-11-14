using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string AppName = System.Windows.Application.Current.GetType().Assembly.GetName().Name;
        public static MainWindow win1;
        public static Database bdd = new Database();

        private static bool _IsDebug = false;
        public static bool IsDebug { get { return _IsDebug; } }
        [Conditional("DEBUG")]
        public static void TestDebug() { _IsDebug = true; }

        public App()
        {
            TestDebug();
            Player.INIT();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                InitializeComponent();
                if (IsDebug)
                {
                    AnotherMusicPlayer.MainWindow2.MainWindow2 mw2 = new AnotherMusicPlayer.MainWindow2.MainWindow2();
                    mw2.ShowDialog();
                    mw2.Dispose();
                }

                win1 = new MainWindow(bdd, this);
                win1.ShowDialog();
            }));
        }

        public static void UnsetLockScreen() { win1.UnsetLockScreen(); }
    }
}
