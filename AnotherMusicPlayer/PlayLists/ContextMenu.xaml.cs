using GongSolutions.Wpf.DragDrop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class PlayLists : IDropTarget
    {
        private ContextMenu MakeContextMenu(FrameworkElement parent, string otype = null)
        {
            if (otype == null || otype == "") { otype = "track"; }
            string type = otype = otype.ToLower().Trim();

            ContextMenu cm = new PlayListsContextMenu() { Style = Parent.FindResource("CustomContextMenuStyle") as Style };
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (((MenuItem)cm.Items[i]).Name == "PlayListPlay" && type.StartsWith("list"))
                {
                    ((MenuItem)cm.Items[i]).Click += CM_PlayList;
                    ((MenuItem)cm.Items[i]).Tag = parent;
                }
                else if (((MenuItem)cm.Items[i]).Name == "PlayListDelete" && type == "listcustom")
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    ((MenuItem)cm.Items[i]).Click += CM_DeleteList;
                }
                else if (((MenuItem)cm.Items[i]).Name == "RemoveTrack" && type == "content")
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    ((MenuItem)cm.Items[i]).Click += CM_RemoveTrackSelection;
                }
                else if (((MenuItem)cm.Items[i]).Name == "AddCategoryRadio" && type == "radioroot")
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    ((MenuItem)cm.Items[i]).Header = Parent.FindResource("PlayListsContextMenuRadioAddCategory") as string;
                    ((MenuItem)cm.Items[i]).Click += CM_AddCategoryRadio;
                }
                else if (((MenuItem)cm.Items[i]).Name == "AddRadio" && type == "radioroot")
                {
                    ((MenuItem)cm.Items[i]).Tag = parent;
                    ((MenuItem)cm.Items[i]).Header = Parent.FindResource("PlayListsContextMenuRadioAddRadio") as string;
                    ((MenuItem)cm.Items[i]).Click += CM_AddRadio;
                }
                else { ((MenuItem)cm.Items[i]).Visibility = Visibility.Collapsed; }
            }

            cm.Tag = parent;
            return cm;
        }

        private void CM_AddRadio(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(" ==> CM_AddRadio");
            //Dictionary<string, object> data = (Dictionary<string, object>)((TreeViewItem)sender).Tag;
            //Convert.ToInt32(data["Category"])

            AddRadio ar = new AddRadio(Parent, false, 0);
            ar.ShowDialog();
            if (ar.Saved) { Init(); }
        }

        private void CM_AddCategoryRadio(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(" ==> CM_AddCategoryRadio");
            AddRadio ar = new AddRadio(Parent, true);
            ar.ShowDialog();
            if (ar.Saved) { Init(); }
        }

        private void CM_RemoveTrackSelection(object sender, RoutedEventArgs e)
        {
            TreeViewItem TItem = (Parent.PlaylistsTree.SelectedItem != null) ? (TreeViewItem)Parent.PlaylistsTree.SelectedItem : null;
            if (TItem == null) { return; }
            if (!TItem.Name.StartsWith("user_")) { return; }

            if (Parent.PlaylistsContents.SelectedItems.Count == 0) { return; }

            bool ret = DialogBox.ShowDialog(
                Parent,
                Parent.FindResource("PlayListsContextMenuPlayListDeleteConfirmTitle") as string,
                (Parent.PlaylistsContents.SelectedItems.Count > 1) ?
                    Parent.FindResource("PlayListsContextMenuTrackDeleteConfirmMessage2") as string :
                    (Parent.FindResource("PlayListsContextMenuTrackDeleteConfirmMessage") as string).Replace("%X%", ((MediaItem)Parent.PlaylistsContents.SelectedItem).Name),
                DialogBoxButtons.YesNo,
                DialogBoxIcons.Warning
                );

            if (ret == true)
            {
                List<string> querys = new List<string>();
                foreach (MediaItem row in Parent.PlaylistsContents.SelectedItems)
                {
                    querys.Add("DELETE FROM playlistsItems WHERE LIndex = '" + ((string)TItem.Tag) + "' AND Path = '" + Database.EscapeString(row.Path) + "'");
                }
                Parent.bdd.DatabaseQuerys(querys.ToArray(), true);
                userlistClick(TItem, null);
            }
        }

        private void CM_DeleteList(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string list_id = (string)((TreeViewItem)item.Tag).Tag;
            string list_name = ((TextBlock)((TreeViewItem)item.Tag).Header).Text;

            if (list_id.StartsWith("auto_")) { return; }
            int id = Convert.ToInt32(list_id);

            bool ret = DialogBox.ShowDialog(
                Parent,
                Parent.FindResource("PlayListsContextMenuPlayListDeleteConfirmTitle") as string,
                (Parent.FindResource("PlayListsContextMenuPlayListDeleteConfirmMessage") as string).Replace("%X%", list_name),
                DialogBoxButtons.YesNo,
                DialogBoxIcons.Warning
                );

            if (ret == true)
            {
                Parent.bdd.DatabaseQuerys(new string[] { "DELETE FROM playlists WHERE FIndex = " + id, "DELETE FROM playlistsItems WHERE LIndex = " + id }, true);
                Parent.playLists.Init();
            }
        }

        private void CM_PlayList(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string list_id = "";
            if (item.Tag.GetType().Name == "TreeViewItem")
            {
                list_id = (string)(((TreeViewItem)item.Tag).Tag);
            }
            else { list_id = (string)item.Tag; }
            Debug.WriteLine("list_id = " + list_id);

            if (list_id.StartsWith("auto_"))
            {
                Dictionary<string, Dictionary<string, object>> rez = autolistData(list_id.Replace("auto_", ""));
                List<string> files = new List<string>();
                foreach (string key in rez.Keys) { files.Add(key); }
                Player.PlaylistClear();
                Player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
            }
            else
            {
                int id = Convert.ToInt32(list_id.Replace("user_", ""));
                List<string> files = userlistData(id);
                if (files != null)
                {
                    Player.PlaylistClear();
                    Player.PlaylistEnqueue(files.ToArray(), false, 0, 0, true);
                }
            }
        }

        #region ContextMenu Add Playlist functions
        private void CM_AddTrack(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string track = (string)((Button)item.Tag).Tag;
            Player.PlaylistEnqueue(new string[] { track });
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
