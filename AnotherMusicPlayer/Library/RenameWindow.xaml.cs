using AnotherMusicPlayer.MainWindow2Space;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Logique d'interaction pour RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        private string FolderPath;
        private string[] PathTab;
        public bool renamed = false;
        public RenameWindow(/*MainWindow parent, */string folderPath, string[] pathTab)
        {
            /*Owner = parent;*/
            FolderPath = folderPath;
            PathTab = pathTab;
            InitializeComponent();
            Resources = Owner.Resources;
            Style = FindResource("CustomWindowStyle") as Style;

            input.Text = pathTab[pathTab.Length - 1];
            Loaded += RenameWindow_Loaded;

            BtnClose.Click += (object sender, RoutedEventArgs e) => { Close(); };
            saveBtn.Click += (object sender, RoutedEventArgs e) =>
            {
                string tx = input.Text.Trim();
                bool ok = true;
                char[] excludeList = new char[] { '<', '>', ':', '"', '/', '\\', '?', '*', '|' };
                if (tx == "") { ok = false; }
                else
                {
                    foreach (char c in excludeList) { if (tx.Contains(c)) { ok = false; break; } }
                }
                if (ok == false)
                {
                    MessageBox.Show("Folder name invalid,\nplease remove the folowing characters:\n < > : \" / \\ | ? *", "Error !", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<string> list = new List<string>(PathTab);
                list.Remove(PathTab[PathTab.Length - 1]);

                Directory.Move(FolderPath, string.Join(MainWindow2.SeparatorChar, list.ToArray()) + MainWindow2.SeparatorChar + input.Text.Trim());
                renamed = true;
                Close();
            };
            input.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                saveBtn.IsEnabled = true;
            };
        }

        private void RenameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Left = Owner.Left + ((Owner.Width - Width) / 2);
            Top = Owner.Top + ((Owner.Height - Height) / 2);
        }
    }
}
