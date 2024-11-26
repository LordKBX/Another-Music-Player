using AnotherMusicPlayer.MainWindow2Space;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;

namespace AnotherMusicPlayer.MainWindow2Space
{
    public partial class MainWindow2
    {
        public PlayBackContextMenu MakePlayBackContextMenu()
        {
            PlayBackContextMenu cm = new PlayBackContextMenu();
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (cm.Items[i].Name == "PlayTrack")
                {
                    cm.Items[i].Click += (object sender, EventArgs e) => {
                        if (App.win1.PlaybackTabDataGridView.SelectedRows.Count <= 0) { return; }
                        int id = App.win1.PlaybackTabDataGridView.SelectedRows[0].Index;

                        if (Player.Index >= 0)
                        {
                            ((PlayListViewItem)App.win1.PlaybackTabDataGridView.Rows[Player.Index].DataBoundItem).Selected = "";
                            Player.StopAll();
                        }
                        Player.PlaylistReadIndex(id);
                        App.win1.PlaybackTabDataGridView.Rows[id].Selected = true;
                    };
                }
                else if (cm.Items[i].Name == "RemoveTrack")
                {
                    cm.Items[i].Click += (object sender, EventArgs e) => {
                        if (App.win1.PlaybackTabDataGridView.SelectedRows.Count <= 0) { return; }
                        int id = App.win1.PlaybackTabDataGridView.SelectedRows[0].Index;

                        if (Player.Index  == id)
                        {
                            ((PlayListViewItem)App.win1.PlaybackTabDataGridView.Rows[Player.Index].DataBoundItem).Selected = "";
                            Player.StopAll();
                        }
                        Player.PlaylistRemoveIndex(id);
                    };
                }
            }
            return cm;
        }
    }

    /// <summary>
    /// Logique d'interaction pour LibraryContextMenu.xaml
    /// </summary>
    public partial class PlayBackContextMenu : ContextMenuStrip
    {
        private System.Drawing.Color _ForeColor = System.Drawing.Color.Black;
        new public System.Drawing.Color ForeColor {
            get { return _ForeColor; }
            set { base.ForeColor = _ForeColor = value; Update(); }
        }
        private System.Drawing.Color _BackColor = System.Drawing.Color.Black;
        new public System.Drawing.Color BackColor {
            get { return _BackColor; }
            set { base.BackColor = _BackColor = value; Update(); }
        }
        private SolidColorBrush DefaultBrush = new SolidColorBrush(Colors.White);
        private int ButtonIconSize = 32;

        // ITEMS
        public ToolStripItem PlayTrack = null;
        public ToolStripItem RemoveTrack = null;

        public PlayBackContextMenu()
        {
            Font = new Font("Segoe UI", 15, System.Drawing.FontStyle.Regular, GraphicsUnit.Point);
            base.BackColor = _BackColor = App.style.GetColor("ContextMenuBackColor");
            base.ForeColor = _ForeColor = App.style.GetColor("ContextMenuForeColor");

            RenderMode = ToolStripRenderMode.System;

            PlayTrack = Items.Add(App.GetTranslation("PlayingQueueCMPlay"), Icons.FromIconKind(IconKind.PlayCircle, ButtonIconSize, DefaultBrush));
            RemoveTrack = Items.Add(App.GetTranslation("PlayingQueueCMRemove"), Icons.FromIconKind(IconKind.PlaylistMinus, ButtonIconSize, DefaultBrush));

            PlayTrack.Name = nameof(PlayTrack);
            RemoveTrack.Name = nameof(RemoveTrack);
        }

        public void Update()
        {
            DefaultBrush = new SolidColorBrush(App.DrawingColorToMediaColor(_ForeColor));

            PlayTrack.ForeColor = _ForeColor;
            PlayTrack.Text = App.GetTranslation("PlayingQueueCMPlay");
            PlayTrack.Image = Icons.FromIconKind(IconKind.PlayCircle, ButtonIconSize, DefaultBrush);

            RemoveTrack.ForeColor = _ForeColor;
            RemoveTrack.Text = App.GetTranslation("PlayingQueueCMRemove");
            RemoveTrack.Image = Icons.FromIconKind(IconKind.PlaylistMinus, ButtonIconSize, DefaultBrush);
        }
    }
}
