using System;
using System.Collections.Generic;
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
    public partial class PlayLists
    {
        private ContextMenu MakeContextMenu(FrameworkElement parent, string otype = null)
        {
            if (otype == null || otype == "") { otype = "track"; }
            string type = otype = otype.ToLower();
            if (type == "file") { type = "track"; }
            ContextMenu cm = new PlayListsContextMenu() { Style = Parent.FindResource("CustomContextMenuStyle") as Style };
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (((MenuItem)cm.Items[i]).Name == "PlayList" && type == "list")
                {
                    ((MenuItem)cm.Items[i]).Click += CM_PlayList;
                    ((MenuItem)cm.Items[i]).Tag = parent.Tag;
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "add" + type)
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    if (type == "track") { ((MenuItem)cm.Items[i]).Click += CM_AddTrack; }
                }
                else { ((MenuItem)cm.Items[i]).Visibility = Visibility.Collapsed; }
            }
            return cm;
        }

        private void CM_PlayList(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string list_id = (string)item.Tag;
            if (list_id.StartsWith("auto_"))
            {
                Dictionary<string, Dictionary<string, object>> rez = autolistData(list_id.Replace("auto_", ""));
                List<string> files = new List<string>();
                foreach (string key in rez.Keys) { files.Add(key); }
                Parent.player.PlaylistClear();
                Parent.player.PlaylistEnqueue(files.ToArray());
            }
            else
            {
                int id = Convert.ToInt32(list_id.Replace("auto_", ""));
                List<string> files = userlistData(id);
                if (files != null)
                {
                    Parent.player.PlaylistClear();
                    Parent.player.PlaylistEnqueue(files.ToArray());
                }
            }
        }

        #region ContextMenu Add Playlist functions
        private void CM_AddTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string track = (string)((Button)item.Tag).Tag;
            Parent.player.PlaylistEnqueue(new string[] { track });
        }
        #endregion
    }

    /// <summary>
    /// Logique d'interaction pour ContextMenu.xaml
    /// </summary>
    public partial class PlayListsContextMenu : ContextMenu
    {
        public PlayListsContextMenu()
        {
            InitializeComponent();
        }
    }
}
