using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Logique d'interaction pour PlayingQueueContextMenu.xaml
    /// </summary>
    public partial class PlayingQueueContextMenu : ContextMenu
    {
        public MainWindow Parent;
        public PlayingQueueContextMenu(MainWindow parent)
        {
            Parent = parent;
            InitializeComponent();
            Resources = Parent.Resources;
        }

        public static ContextMenu MakeContextMenu(MainWindow parent)
        {
            ContextMenu cm = new PlayingQueueContextMenu(parent) { Style = parent.FindResource("CustomContextMenuStyle") as Style };
            for (int i = 0; i < cm.Items.Count; i++)
            {
                ((MenuItem)cm.Items[i]).Tag = parent;
                if (((MenuItem)cm.Items[i]).Name == "ClearPlaylist") { ((MenuItem)cm.Items[i]).Click += CM_Clear; }
                else if (((MenuItem)cm.Items[i]).Name == "RemoveTracks") { ((MenuItem)cm.Items[i]).Click += CM_Remove; ; }
            }

            cm.Tag = parent;
            return cm;
        }

        private static void CM_Remove(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("--> CM_Remove L1");
            MainWindow Parent = (MainWindow)((MenuItem)sender).Tag;
            if (Parent.PlayListView.SelectedItems.Count <= 0) { return; }
            Debug.WriteLine("--> CM_Remove L2");
            ObservableCollection<PlayListViewItem> previous_items = (ObservableCollection<PlayListViewItem>)Parent.PlayListView.ItemsSource;
            if (previous_items.IndexOf((PlayListViewItem)Parent.PlayListView.SelectedItems[0]) <= 0) { return; }
            Debug.WriteLine("--> CM_Remove L3");
            int playlistOffset = Parent.player.Index;
            List<int> indexes = new List<int>();
            foreach (PlayListViewItem row in Parent.PlayListView.SelectedItems) { indexes.Add(playlistOffset + previous_items.IndexOf(row)); }
            Parent.player.PlaylistRemoveIndexes(indexes.ToArray());
        }

        private static void CM_Clear(object sender, RoutedEventArgs e)
        {
            MainWindow parent = (MainWindow)((MenuItem)sender).Tag;
            parent.Clear_Button_Click(null, null);
        }
    }
}
