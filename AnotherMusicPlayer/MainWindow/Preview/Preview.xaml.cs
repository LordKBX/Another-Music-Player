using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Logique d'interaction pour Preview.xaml
    /// </summary>
    public partial class Preview : UserControl
    {
        private new MainWindow Parent = null;
        private MediaItem item = null;
        public string path = null;
        public Preview(MainWindow parent)
        {
            Parent = parent;
            InitializeComponent();
            Resources = Parent.Resources;
            Style = FindResource("CustomUserControl") as Style;
            FileCover.Source = Parent.FileCover.Source;
            foreach (FrameworkElement elem in PanelText.Children)
            {
                //if (elem.GetType().Name == "TextBlock") { ((TextBlock)elem).FontSize = 20; }
                if (elem.GetType().Name == "AccessText") { ((AccessText)elem).FontSize = 27; ((AccessText)elem).FontWeight = FontWeights.Bold; }
            }

            UpdateFile(Parent.player.GetCurrentFile());
        }

        public void UpdateFile(string filePath)
        {
            item = null;
            path = filePath;
            try
            {
                FileCover.Source = Parent.FileCover.Source;
                Dictionary<string, object> ret = Parent.bdd.DatabaseFileInfo(path);
                if (ret != null) { item = MainWindow.DatabaseItemToMediaItem(ret); }
                else { item = FilesTags.MediaInfo(path, false); }
                Update();
                System.Windows.Media.Imaging.BitmapImage bi = FilesTags.MediaPicture(item.Path, Parent.bdd, true, 150, 150);
                FileCover.Source = (bi ?? MainWindow.Bimage("CoverImg"));
            }
            catch
            {
                TitleValue.Text = "";
                AlbumValue.Text = "";
                ArtistsValue.Text = "";
                AlbumValue.Visibility = Visibility.Collapsed;
                ArtistsValue.Visibility = Visibility.Collapsed;
                DisplayPlaybackPosition.Content = "00:00";
                DisplayPlaybackSize.Content = "00:00";
                DisplayPlaybackPositionBar.Value = 0;
                FileCover.Source = MainWindow.Bimage("CoverImg");
            }
        }

        public void Update()
        {
            if (item != null)
            {
                TitleValue.Text = item.Name;
                if (item.Album != null && item.Album.Trim() != "") { AlbumValue.Text = item.Album; AlbumValue.Visibility = Visibility.Visible; }
                else { AlbumValue.Visibility = Visibility.Collapsed; }

                string ar = "";
                if (item.Performers != null) { ar += item.Performers.Trim().Replace(";", ", "); }
                if (item.Composers != null)
                {
                    if (ar != "") { ar += ", "; }
                    ar += item.Composers.Trim().Replace(";", ", ");
                }
                if (ar.Trim() != "") { ArtistsValue.Text = ar; ArtistsValue.Visibility = Visibility.Visible; }
                else { ArtistsValue.Visibility = Visibility.Collapsed; }

                //DurationValue.Text = item.DurationS;

                DisplayPlaybackPosition.Content = Parent.DisplayPlaybackPosition.Content;
                DisplayPlaybackSize.Content = item.DurationS;
                DisplayPlaybackPositionBar.Value = Parent.lastPlaybackPosition * 1000 / item.Duration;
            }
        }
    }
}
