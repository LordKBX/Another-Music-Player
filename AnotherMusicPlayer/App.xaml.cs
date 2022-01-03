using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        public App() {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Database bdd = new Database();
                InitializeComponent();
                MainWindow win1 = new MainWindow(bdd);
                win1.ShowDialog();
            }));
        }
    }
}
