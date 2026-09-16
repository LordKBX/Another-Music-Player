using AnotherMusicPlayer.MainWindow2Space;
using CustomExtensions;
using m3uParser;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AnotherMusicPlayer
{
    public partial class MediaInfoWindow : Form
    {
        private Font fontNormal = App.win1.Font;
        private Font fontBold = new Font(App.win1.Font, FontStyle.Bold);
        Bitmap BitmapCover;
        MediaInfoWindowSourceType sourceType;
        string sourceInfo;

        public MediaInfoWindow(MainWindow2 parent, string filePath, MediaInfoWindowSourceType sourceType = MediaInfoWindowSourceType.PlayingQueue, string sourceInfo = "")
        {
            this.Owner = parent;
            this.sourceType = sourceType;
            this.sourceInfo = sourceInfo;
            if (filePath == null || !File.Exists(filePath)) { throw new Exception("File not found!"); }
            InitializeComponent();
            MinimumSize = new Size(600, 300);

            MediaItem item = FilesTags.MediaInfo(filePath, false);

            this.Resize += MediaInfo_Resize;

            BitmapCover = Properties.Resources.album_large;
            try { BitmapCover = BitmapMagic.BitmapImage2Bitmap(FilesTags.MediaPicture(item.Path, App.bdd, true, 250, 250)); }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }

            Cover.BackgroundImage = BitmapCover;

                flowLayoutPanelRight.AutoScroll = true;
            flowLayoutPanelRight.Controls.Clear();

            AddLine1L("Year", string.Join("; ", item.Year));
            AddLine1L("Disc", "" + item.Disc + " / " + (item.DiscCount <= 0 ? "??" : item.DiscCount));
            AddLine1L("Track", "" + item.Track + " / " + (item.TrackCount <= 0 ? "??" : item.TrackCount));
            AddLine2L("Copyright", item.Copyright);

            AddLineR("Path", filePath);
            AddLineR("Title", item.Name);
            AddLineR("Album", item.Album);
            AddLineR("AlbumArtists", string.Join("; ", item.AlbumArtists));
            AddLineR("Composers", string.Join("; ", item.Composers));
            AddLineR("Performers", string.Join("; ", item.Performers));
            AddLineR("Genres", string.Join("; ", item.Genres));
            flowLayoutPanelRight.Controls.Add(new Label() { 
                Font = fontBold, Text = "Lyrics",
                Tag = "dataLine"
            });
            int cw = flowLayoutPanelRight.Width - 20;
            RichTextBox lb1 = new RichTextBox()
            {
                ReadOnly = true,
                //Enabled = (Tags.Tag.Lyrics == null || Tags.Tag.Lyrics.Trim().Length == 0)?false:true,
                Font = fontNormal,
                Text = item.Lyrics,
                MinimumSize = new Size(cw, 120),
                Tag = "dataBlock",
                BackColor = App.style.GetColor("GlobalTextBoxBackColor"),
                ForeColor = App.style.GetColor("GlobalTextBoxForeColor")
            };
            lb1.Width = cw;
            flowLayoutPanelRight.Controls.Add(lb1);
            App.SetToolTip(lb1, "Lyrics");

            #region Rate element

            flowLayoutPanelLeft.Controls.Add(new Label() { Font = fontBold, Text = "Rating", Tag = "dataLine" });
            Rating2 ratingObject = new Rating2()
            {
                MinimumSize = new Size(150, 40),
                Rate = item.Rating, IsReadOnly = true,
                Margin = new Padding(5, 0, 0, 0)
            };
            flowLayoutPanelLeft.Controls.Add(ratingObject);
            App.SetToolTip(ratingObject, "" + item.Rating + " / 5");
            #endregion

            AddButton(App.GetTranslation("LibraryContextMenuOpenFolder", "Open folder"), "OpenFolder", (s, e) =>
            {
                try
                { Process.Start("explorer.exe", "/select,\"" + filePath + "\""); }
                catch (Exception ex)
                { MessageBox.Show("Error opening folder: " + ex.Message); }
            });

            AddButton(App.GetTranslation("LibraryContextMenuRenameFile", "Rename File"), "RenameFile", (s, e) =>
            {
                try
                {
                    //filePath
                    FileInfo fi = new FileInfo(filePath);
                    if(fi.Exists)
                    {
                        RenameWindow rw = new RenameWindow(this, fi.Name, new string[] { }, true);
                        if (rw.ShowDialog() == DialogResult.OK)
                        {
                            App.bdd.DatabaseQuerys(new string[] { "UPDATE files SET Path = '" + Database.EscapeString(rw.FolderPath) + "' WHERE Path='" + Database.EscapeString(filePath) + "'" }, true);
                            App.bdd.DatabaseQuerys(new string[] { "UPDATE playCounts SET Path = '" + Database.EscapeString(rw.FolderPath) + "' WHERE Path='" + Database.EscapeString(filePath) + "'" }, true);
                            App.bdd.DatabaseQuerys(new string[] { "UPDATE playlistsItems SET Path = '" + Database.EscapeString(rw.FolderPath) + "' WHERE Path='" + Database.EscapeString(filePath) + "'" }, true);
                            App.bdd.DatabaseQuerys(new string[] { "UPDATE queue SET Path1 = '" + Database.EscapeString(rw.FolderPath) + "' WHERE Path1='" + Database.EscapeString(filePath) + "'" }, true);
                            App.bdd.DatabaseQuerys(new string[] { "UPDATE queue SET Path2 = '" + Database.EscapeString(rw.FolderPath) + "' WHERE Path2='" + Database.EscapeString(filePath) + "'" }, true);
                            App.bdd.DatabaseQuerys(new string[] { "DELETE FROM covers WHERE LIKE '" + Database.EscapeString(filePath).Replace("\\\\", "\\").Replace("\\", "/") + "|'%" }, true);

                            if (sourceType == MediaInfoWindowSourceType.PlayingQueue)
                            {
                                try
                                {

                                    if (App.win1.PlaybackTabDataGridView.SelectedRows.Count == 1)
                                    {
                                        int id = App.win1.PlaybackTabDataGridView.SelectedRows[0].Index;

                                        if (Player.Index == id)
                                        {
                                            ((PlayListViewItem)App.win1.PlaybackTabDataGridView.Rows[Player.Index].DataBoundItem).Selected = "";
                                            Player.StopAll();
                                            Player.ClearCurrentFile();
                                        }
                                        ((PlayListViewItem)App.win1.PlaybackTabDataGridView.Rows[id].DataBoundItem).Path = rw.FolderPath;
                                        Player.PlayList[id] = rw.FolderPath;
                                        Player.SavePlaylist();
                                        if (Player.Index == id) { Player.PlaylistReadIndex(id); }
                                    }
                                }
                                catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
                            }
                            if (sourceType == MediaInfoWindowSourceType.Library || App.win1.library.CurrentPath == fi.DirectoryName)
                            {
                                ((MainWindow2)Owner).library.DisplayPath(App.win1.library.CurrentPath);
                            }
                            if (sourceType == MediaInfoWindowSourceType.Playlist && App.win1.TabControler.SelectedTab.Name == "PlayListsTab")
                            {
                                TreeNodeMouseClickEventArgs ev = new TreeNodeMouseClickEventArgs(App.win1.PlaylistsTree.SelectedNode, MouseButtons.Left, 1, 0, 0);
                                App.win1.playLists.PlaylistsTree_NodeMouseClick(App.win1.PlaylistsTree.SelectedNode, ev);
                            }


                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("File does not exist.");
                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace); }
            });

            AddButton(App.GetTranslation("LibraryContextMenuDeleteFile"), "CancelButton", (s, e) =>
            {
                try
                {
                    //filePath
                    FileInfo fi = new FileInfo(filePath);
                    if(fi.Exists)
                    {
                        if (DialogBox.ShowDialog(
                            App.GetTranslation("PlayListsContextMenuTrackDelete","Deleting file"),
                            App.GetTranslation("PlayListsContextMenuTrackDeleteConfirmMessage", "Do you confirm ?").Replace("%X%", fi.Name),
                            DialogBoxButtons.YesNo, DialogBoxIcons.Warning, this))
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(filePath,
                                Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

                            App.bdd.DeleteFileAsync(fi.FullName);

                            if (sourceType == MediaInfoWindowSourceType.PlayingQueue)
                            {
                                try {
                                    if (App.win1.PlaybackTabDataGridView.SelectedRows.Count == 1)
                                    {
                                        int id = App.win1.PlaybackTabDataGridView.SelectedRows[0].Index;

                                        if (Player.Index == id)
                                        {
                                            Player.StopAll();
                                            App.win1.PlaybackTabDataGridView.Rows.RemoveAt(Player.Index);
                                            App.win1.PlayListItems.RemoveAt(Player.Index);
                                        }
                                        Player.PlaylistRemoveIndex(id);
                                        Player.SavePlaylist();
                                        if (Player.Index == id) { Player.PlaylistReadIndex(id); }
                                    }
                                }
                                catch(Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
                            }
                            if(sourceType == MediaInfoWindowSourceType.Library || App.win1.library.CurrentPath == fi.DirectoryName)
                            {
                                ((MainWindow2)Owner).library.DisplayPath(App.win1.library.CurrentPath);
                            }
                            if(sourceType == MediaInfoWindowSourceType.Playlist && App.win1.TabControler.SelectedTab.Name == "PlayListsTab")
                            {
                                TreeNodeMouseClickEventArgs ev = new TreeNodeMouseClickEventArgs(App.win1.PlaylistsTree.SelectedNode, MouseButtons.Left, 1, 0, 0);
                                App.win1.playLists.PlaylistsTree_NodeMouseClick(App.win1.PlaylistsTree.SelectedNode, ev);
                            }

                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("File does not exist.");
                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace); }
            });

            flowLayoutPanelRight.Controls[0].Focus();

            SetStyle();
            MediaInfo_Resize(null, null);
        }

        public void SetStyle(Control ctl = null) 
        {
            bool lv0 = false;
            if (ctl == null) { ctl = this; lv0 = true; }

            if (ctl.Name == "Cover") { ctl.BackColor = App.style.GetColor("GlobalTrackIconBackColor"); }
            else
            {
                ctl.BackColor = App.style.GetColor("GlobalBackColor");
                try { ctl.ForeColor = App.style.GetColor("GlobalForeColor"); } catch (Exception) { }
            }

            if (ctl.Controls != null && ctl.Controls.Count > 0) { foreach (Control ctl2 in ctl.Controls) { SetStyle(ctl2); } }
            if (lv0)
            {
                AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(Controls.Find("OpenFolder", true)[0]);
                AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(Controls.Find("RenameFile", true)[0]);
                AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(Controls.Find("CancelButton", true)[0]);

                Controls.Find("OpenFolder", true)[0].Font = App.style.GetValue<Font>("GlobalFontSmall", AnotherMusicPlayer.Styles.Dark.GlobalFontSmall);
                Controls.Find("RenameFile", true)[0].Font = App.style.GetValue<Font>("GlobalFontSmall", AnotherMusicPlayer.Styles.Dark.GlobalFontSmall);
                Controls.Find("CancelButton", true)[0].Font = App.style.GetValue<Font>("GlobalFontSmall", AnotherMusicPlayer.Styles.Dark.GlobalFontSmall);
            }
        }

        private void AddLine1L(string cat, string data) 
        {
            TableLayoutPanel table = new TableLayoutPanel()
            {
                MinimumSize = new Size(flowLayoutPanelLeft.Width - 20, fontBold.Height + 8),
                MaximumSize = new Size(flowLayoutPanelLeft.Width - 20, fontBold.Height + 8),
                RowCount = 1, ColumnCount = 2,
                Margin = new Padding(0)
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            table.Controls.Add(new Label() { Font = fontBold, Text = cat.Capitalize(), Tag = "dataLine" }, 0, 0);
            Label lb1 = new Label()
            {
                Font = fontNormal,
                Text = data,
                TextAlign = ContentAlignment.TopLeft,
                Tag = "dataLine"
            };
            table.Controls.Add(lb1, 1, 0);
            App.SetToolTip(lb1, data);

            flowLayoutPanelLeft.Controls.Add(table);
        }

        private void AddLine2L(string cat, string data) 
        {
            flowLayoutPanelLeft.Controls.Add(new Label() { Font = fontBold, Text = cat.Capitalize(), Tag = "dataLine" });
            Label lb1 = new Label()
            {
                Font = fontNormal,
                Text = data,
                TextAlign = ContentAlignment.TopLeft,
                Tag = "dataLine"
            };
            flowLayoutPanelLeft.Controls.Add(lb1);
            App.SetToolTip(lb1, data);
        }

        private void AddLineR(string cat, string data) 
        {
            flowLayoutPanelRight.Controls.Add(new Label() { Font = fontBold, Text = cat.Capitalize(), MinimumSize = new Size(flowLayoutPanelRight.Width - 20, 20) });
            Label lb1 = new Label()
            {
                Font = fontNormal,
                Text = data,
                TextAlign = ContentAlignment.TopLeft,
                MinimumSize = new Size(flowLayoutPanelRight.Width - 20, 20),
                Tag = "dataLine"
            };
            flowLayoutPanelRight.Controls.Add(lb1);
            App.SetToolTip(lb1, data);
        }

        private void AddButton(string text, string tag, EventHandler clickEvent)
        {
            Button btn = new Button()
            {
                MinimumSize = new Size(150, 40),
                Text = text, Tag = tag, Name = tag,
                Margin = new Padding(5, 0, 0, 3)
            };
            btn.Click += clickEvent;
            flowLayoutPanelLeft.Controls.Add(btn);
        }   

        private void changeCoverPreview(TagLib.IPicture pic)
        {
            MemoryStream ms = new MemoryStream(pic.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapCover = new Bitmap(ms);
            ms.Close();
            Cover.BackgroundImage = BitmapCover;
        }

        private void MediaInfo_Resize(object sender, EventArgs e)
        {
            Type st = typeof(string);
            foreach (Control ctl in flowLayoutPanelLeft.Controls)
            {
                Size nz = new Size(flowLayoutPanelLeft.Width - 20, ctl.Font.Height + 8);
                ctl.MinimumSize = nz; ctl.Width = nz.Width; 
            }
            foreach (Control ctl in flowLayoutPanelRight.Controls)
            {
                Size nz = new Size(flowLayoutPanelRight.Width - 20, ctl.Font.Height + 8);
                if (ctl.Tag != null && ctl.Tag.GetType() == st)
                {
                    if (("" + ctl.Tag) == "dataLine") { ctl.MinimumSize = nz; ctl.Width = nz.Width; }
                    if (("" + ctl.Tag) == "dataBlock") { ctl.MinimumSize = new Size(nz.Width, 195); ctl.Width = nz.Width; }
                }
            }
        }
    }

    public enum MediaInfoWindowSourceType { PlayingQueue, Library, Playlist }
}
