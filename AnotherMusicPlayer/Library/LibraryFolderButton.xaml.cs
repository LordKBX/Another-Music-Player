using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Logique d'interaction pour LibraryFolderButton.xaml
    /// </summary>
    public partial class LibraryFolderButton : Button
    {
        public string Path;
        //public PackIcon Icon;
        public LibraryFolderButton(string title, string path)
        {
            InitializeComponent();
            displayTitle.Text = title;
            Path = path;
        }
    }
}
