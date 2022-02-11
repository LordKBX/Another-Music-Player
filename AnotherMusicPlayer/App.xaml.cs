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
        public MainWindow win1;
        public App()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Database bdd = new Database();
                InitializeComponent();
                win1 = new MainWindow(bdd, this);
                win1.ShowDialog();
            }));
        }

    }
}
