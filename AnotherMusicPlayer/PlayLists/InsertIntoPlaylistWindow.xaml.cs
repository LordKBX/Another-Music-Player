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
using System.Windows.Shapes;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Logique d'interaction pour InsertIntoPlaylistWindow.xaml
    /// </summary>
    public partial class InsertIntoPlaylistWindow : Window
    {
        private MainWindow Parent;
        public int maxIndexList = 0;

        public InsertIntoPlaylistWindow(MainWindow parent)
        {
            Parent = parent;
            InitializeComponent();
            Resources = Parent.Resources;

            // FILLING WINDOW
            Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery("SELECT FIndex,Name,Description, MAX(LOrder) AS Lorder FROM playlists LEFT JOIN playlistsItems ON(LIndex = FIndex) GROUP BY Name ORDER BY Name ASC", "FIndex");
            maxIndexList = 0;
            rez.Add("!", new Dictionary<string, object>() { { "FIndex", "0" }, { "Name", Parent.FindResource("PlaylistsWindowAddIntoPlaylistNewElement") as string } });

            foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
            {
                int index = Convert.ToInt32(row.Value["FIndex"] as string);
                if (index > maxIndexList) { maxIndexList = index; }
                StackPanel sti = new StackPanel() { Orientation = Orientation.Horizontal };
                sti.Children.Add(new MaterialDesignThemes.Wpf.PackIcon()
                {
                    Width = 24,
                    Height = 24,
                    Kind = (index == 0) ? MaterialDesignThemes.Wpf.PackIconKind.PlusThick : MaterialDesignThemes.Wpf.PackIconKind.PlaylistMusic,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Parent.FindResource("TabControl.ForegroundColor") as SolidColorBrush,
                    Margin = new Thickness(0, 0, 3, 0)
                });
                sti.Children.Add(new AccessText() { Text = row.Value["Name"] as string, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Stretch });
                lw.Items.Add(new ListViewItem() { Content = sti, Tag = row.Key, VerticalAlignment = VerticalAlignment.Center });
            }

            Left = Parent.Left + ((Parent.Width - Width) / 2);
            Top = Parent.Top + ((Parent.Height - Height) / 2);


            btnOk.Click += (object sender, RoutedEventArgs e) =>
            {
                if (lw.SelectedItem == null) { return; }
                Tag = ((ListViewItem)lw.SelectedItem).Tag;
                if (Tag as string == "!") { gr2.Visibility = Visibility.Visible; }
                else { Close(); }
            };
            btnCancel.Click += (object sender, RoutedEventArgs e) => { Close(); };

            btnxCancel.Click += (object sender, RoutedEventArgs e) =>
            {
                input1.Text = "";
                input2.Text = "";
                Tag = null;
                lw.SelectedIndex = -1;
                gr2.Visibility = Visibility.Collapsed;
            };
            btnxOk.Click += (object sender, RoutedEventArgs e) =>
            {
                if (input1.Text.Trim().Length < 3)
                {
                    DialogBox.ShowDialog(
                        Parent,
                        Parent.FindResource("PlaylistsWindowAddIntoPlaylistWarningTitle") as string,
                        Parent.FindResource("PlaylistsWindowAddIntoPlaylistWarningNameSize") as string,
                        DialogBoxButtons.Ok,
                        DialogBoxIcons.Warning
                        );
                    return;
                }
                if (input2.Text.Trim() != "")
                {
                    if (input2.Text.Trim().Length < 3)
                    {
                        DialogBox.ShowDialog(
                            Parent,
                            Parent.FindResource("PlaylistsWindowAddIntoPlaylistWarningTitle") as string,
                            Parent.FindResource("PlaylistsWindowAddIntoPlaylistWarningDescriptionSize") as string,
                            DialogBoxButtons.Ok,
                            DialogBoxIcons.Warning
                            );
                        return;
                    }
                }
                Close();
            };
        }
    }
}
