using AnotherMusicPlayer.MainWindow2Space;
using App;
using ByteDev.Strings;
using m3uParser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TagLib;
using MessageBox = System.Windows.Forms.MessageBox;

namespace AnotherMusicPlayer
{
    public partial class TagsEditor : Form
    {
        private new MainWindow2 Parent;
        private string Mode;
        private TagLib.File Tags;
        private TagLib.IPicture[] pictures;
        Bitmap BitmapCover;
        List<string> AutorizedCoverFileExtention = new List<string> { ".bmp", ".jpg", ".jpeg", ".png" };
        static string DefaultCover = null;
        public bool CoverChanged = false;
        public bool Saved = false;
        private List<string> Files;
        private new bool IsInitialized = false;

        public string tTitle = "";
        public string tAlbum = "";
        public string tPerformers = "";
        public string tComposers = "";
        public string tAlbumArtists = "";

        private ToolStripMenuItem CoverCMPreview = new ToolStripMenuItem() { Text = "Preview Cover" };
        private ToolStripMenuItem CoverCMClear = new ToolStripMenuItem() { Text = "Clear Cover" };
        private ToolStripMenuItem CoverCMAdd = new ToolStripMenuItem() { Text = "Add Cover" };
        private ContextMenuStrip CoverContextMenu = new ContextMenuStrip() { };

        public TagsEditor(MainWindow2 parent, string mode = "track", string[] files = null)
        {
            InitializeComponent();

            if (parent == null) { Debug.WriteLine("TagsEditor, parent is null"); return; }
            if (mode == null) { Debug.WriteLine("TagsEditor, mode is null"); return; }
            if (files == null) { Debug.WriteLine("TagsEditor, files is null"); return; }
            CoverContextMenu.Items.AddRange(new ToolStripItem[] { CoverCMPreview, CoverCMClear, CoverCMAdd });
            mode = mode.ToLower();
            if (mode == "file") { mode = "track"; }
            else if (mode == "folder" || mode == "album") { mode = "album"; }
            else { mode = "track"; }
            Debug.WriteLine("TagsEditor, mode = " + mode);

            Files = new List<string>();

            if (mode == "track")
            {
                if (!System.IO.File.Exists(files[0])) { return; }
                Files.Add(files[0]);
            }
            else
            {
                foreach (string file in files)
                {
                    if (System.IO.File.Exists(file)) { Files.Add(file); }
                }
            }
            if (Files.Count == 0) { return; }
            Tags = TagLib.File.Create(Files[0]);
            Owner = Parent = parent;
            Mode = mode;

            if (DefaultCover == null) { DefaultCover = MainWindow2.BaseDir + "icons" + MainWindow2.SeparatorChar + "album_small.png"; }


            if (mode == "album")
            {
                LyricsLabel.Visible = false;
                LyricsInput.Visible = false;

                TitleLabel.Visible = false;
                TitleInput.Visible = false;

                PerformersLabel.Visible = false;
                PerformersInput.Visible = false;

                ComposersLabel.Visible = false;
                ComposersInput.Visible = false;

                DiscLabel.Visible = false;
                DiscGrid.Visible = false;

                TrackLabel.Visible = false;
                TrackGrid.Visible = false;
            }

            if (mode == "track")
            {
                TitleInput.Text = "" + (Tags.Tag.Title ?? System.IO.Path.GetFileName(files[0]));
                TitleInput.TextChanged += TextInput_TextChanged;

                PerformersInput.Text = Tags.Tag.JoinedPerformers;
                PerformersInput.TextChanged += TextInput_TextChanged;

                ComposersInput.Text = Tags.Tag.JoinedComposers;
                ComposersInput.TextChanged += TextInput_TextChanged;

                LyricsInput.Text = Tags.Tag.Lyrics;
                LyricsInput.TextChanged += TextInput_TextChanged;
            }
            AlbumInput.Text = Tags.Tag.Album;
            AlbumInput.TextChanged += TextInput_TextChanged;

            GenresInput.Text = Tags.Tag.JoinedGenres;
            GenresInput.TextChanged += TextInput_TextChanged;

            AlbumArtistsInput.Text = Tags.Tag.JoinedAlbumArtists;
            AlbumArtistsInput.TextChanged += TextInput_TextChanged;

            YearInput.Text = "" + Tags.Tag.Year;
            YearInput.TextChanged += TextInput_TextChanged;

            DiscInput.Text = "" + Tags.Tag.Disc;
            DiscInput.TextChanged += TextInput_TextChanged;

            DiscCountInput.Text = "" + Tags.Tag.DiscCount;
            DiscCountInput.TextChanged += TextInput_TextChanged;

            TrackInput.Text = "" + Tags.Tag.Track;
            TrackInput.TextChanged += TextInput_TextChanged;

            TrackCountInput.Text = "" + Tags.Tag.TrackCount;
            TrackCountInput.TextChanged += TextInput_TextChanged;

            CopyrightInput.Text = Tags.Tag.Copyright;
            CopyrightInput.TextChanged += TextInput_TextChanged;

            if (Tags.Tag.Pictures.Length > 0) { changeCoverPreview(Tags.Tag.Pictures[0]); }
            else { changeCoverPreview(null, DefaultCover); }

            SaveButton.Click += SaveButton_Click;
            SaveButton.Enabled = false;
            Cover.DragDrop += Cover_Drop;

            Cover.ContextMenuStrip = CoverContextMenu;
            CoverCMPreview.Click += CoverCMPreview_Click;
            CoverCMClear.Click += CoverCMClear_Click;
            CoverCMAdd.Click += CoverCMAdd_Click;

            Tags.Dispose();
            Translation();
            IsInitialized = true;
            this.ShowInTaskbar = false;

            #region Window displasment gestion
            MainWIndowHead.MouseDown += FormDragable_MouseDown;
            MainWIndowHead.MouseMove += FormDragable_MouseMove;
            MainWIndowHead.MouseUp += FormDragable_MouseUp;
            TitleLabel.MouseDown += FormDragable_MouseDown;
            TitleLabel.MouseMove += FormDragable_MouseMove;
            TitleLabel.MouseUp += FormDragable_MouseUp;
            #endregion
            DialogResult = DialogResult.Cancel;

            if (!App.scheduller.HasFunctionality("SaveTags")) 
            { App.scheduller.AddFunctionality("SaveTags", (Func<SchedullerTaskItem, (bool, string)>)SaveTags); }
        }

        #region Window displasment gestion
        private Dictionary<string, bool> draggings = new Dictionary<string, bool>();
        private Dictionary<string, System.Drawing.Point> dragCursorPoints = new Dictionary<string, System.Drawing.Point>();
        private Dictionary<string, System.Drawing.Point> dragFormPoints = new Dictionary<string, System.Drawing.Point>();
        private Dictionary<string, Form> dragForms = new Dictionary<string, Form>();

        private void FormDragable_InitTab(object sender, bool active)
        {
            try
            {
                TableLayoutPanel label1 = (TableLayoutPanel)sender;
                string label = label1.Tag as string;
                Control parent = label1.Parent;
                while (parent.GetType().Name == "TableLayoutPanel") { parent = parent.Parent; }

                draggings.Add(label, active);
                dragCursorPoints.Add(label, System.Windows.Forms.Cursor.Position);
                dragFormPoints.Add(label, label1.Location);
                dragForms.Add(label, (Form)parent);
            }
            catch (Exception) { }
        }

        public void FormDragable_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (sender == null) { return; }
                while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
                string label = ((TableLayoutPanel)sender).Tag as string;
                if (!draggings.ContainsKey(label)) { FormDragable_InitTab(sender, true); }
                else
                {
                    draggings[label] = true;
                    dragCursorPoints[label] = System.Windows.Forms.Cursor.Position;
                    dragFormPoints[label] = dragForms[label].Location;
                }
            }
            catch (IndexOutOfRangeException) { }
            catch (Exception) { }
        }

        public void FormDragable_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (sender == null) { return; }
                while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
                string label = ((TableLayoutPanel)sender).Tag as string;
                if (!draggings.ContainsKey(label)) { FormDragable_InitTab(sender, false); }
                if (draggings[label])
                {
                    System.Drawing.Point dif = System.Drawing.Point.Subtract(System.Windows.Forms.Cursor.Position, new System.Drawing.Size(dragCursorPoints[label]));
                    dragForms[label].Location = System.Drawing.Point.Add(dragFormPoints[label], new System.Drawing.Size(dif));
                }
            }
            catch (IndexOutOfRangeException) { }
            catch (Exception) { }
        }

        public void FormDragable_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (sender == null) { return; }
                while (sender.GetType().Name != "TableLayoutPanel") { sender = ((Control)sender).Parent; }
                string label = ((TableLayoutPanel)sender).Tag as string;
                draggings[label] = false;
            }
            catch (IndexOutOfRangeException) { }
            catch (Exception) { }
        }

        public void FormDragable_Clear(string id)
        {
            try
            {
                if (draggings.ContainsKey(id))
                {
                    draggings.Remove(id);
                    dragCursorPoints.Remove(id);
                    dragFormPoints.Remove(id);
                    dragForms.Remove(id);
                }
            }
            catch (IndexOutOfRangeException) { }
            catch (Exception) { }
        }
        #endregion

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TextInput_TextChanged(object sender, EventArgs e)
        {
            SaveButton.Enabled = true;
        }

        private void Translation() {
            this.Text = WindowTitleLabel.Text = App.GetTranslation("EditorTagWindowTitle");
            LyricsLabel.Text = App.GetTranslation("EditorTagLabelLyrics");
            TitleLabel.Text = App.GetTranslation("EditorTagLabelTitle");

            AlbumLabel.Text = App.GetTranslation("EditorTagLabelAlbum");

            ComposersLabel.Text = App.GetTranslation("EditorTagLabelComposers");
            App.SetToolTip(ComposersLabel, App.GetTranslation("EditorTagSeparateEntrys"));
            App.SetToolTip(ComposersInput, App.GetTranslation("EditorTagSeparateEntrys"));

            PerformersLabel.Text = App.GetTranslation("EditorTagLabelPerformers");
            App.SetToolTip(PerformersLabel, App.GetTranslation("EditorTagSeparateEntrys"));
            App.SetToolTip(PerformersInput, App.GetTranslation("EditorTagSeparateEntrys"));

            AlbumArtistsLabel.Text = App.GetTranslation("EditorTagLabelAlbumArtists");
            App.SetToolTip(AlbumArtistsLabel, App.GetTranslation("EditorTagSeparateEntrys"));
            App.SetToolTip(AlbumArtistsInput, App.GetTranslation("EditorTagSeparateEntrys"));

            GenresLabel.Text = App.GetTranslation("EditorTagLabelGenres");
            App.SetToolTip(GenresLabel, App.GetTranslation("EditorTagSeparateEntrys"));
            App.SetToolTip(GenresInput, App.GetTranslation("EditorTagSeparateEntrys"));

            YearLabel.Text = App.GetTranslation("EditorTagLabelYear");
            DiscLabel.Text = App.GetTranslation("EditorTagLabelDisc");
            TrackLabel.Text = App.GetTranslation("EditorTagLabelTrack");
            CopyrightLabel.Text = App.GetTranslation("EditorTagLabelCopyright");

            SaveButton.Text = App.GetTranslation("EditorTagSave");

            App.SetToolTip(Cover, App.GetTranslation("EditorTagCoverTooltip"));
        }

        private void CoverCMPreview_Click(object sender, EventArgs e)
        {
            int maxw = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth - 100);
            int maxh = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight - 100);
            int width = (BitmapCover.Width < maxw) ? BitmapCover.Width : maxw;
            int height = (BitmapCover.Height < maxw) ? BitmapCover.Height : maxh;
            Form win = new Form();
            win.FormBorderStyle = FormBorderStyle.FixedDialog;
            win.Width = width;
            win.Height = height;
            Panel pan = new Panel() { AutoScroll = true };
            pan.Controls.Add(new Button() { 
                Text = "", Anchor = AnchorStyles.Top | AnchorStyles.Left, BackgroundImage = BitmapCover, 
                BackgroundImageLayout = ImageLayout.Center, Width = BitmapCover.Width, Height = BitmapCover.Height
            });
            win.Controls.Add(pan);
            win.Owner = this;
            win.ShowInTaskbar = false;
            win.ShowDialog();
        }

        private void CoverCMClear_Click(object sender, EventArgs e)
        {
            pictures = new TagLib.IPicture[0] { };
            changeCoverPreview(null, DefaultCover);
            CoverChanged = true;
            SaveButton.Enabled = true;
        }

        private void CoverCMAdd_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            string tx = "";
            AutorizedCoverFileExtention.ForEach(new Action<string>((ext) =>
            {
                if (tx != "") tx += ";";
                tx += "*" + ext.ToUpper();
            }));
            openFileDlg.Filter = "Picture (" + tx + ")|" + tx;
            openFileDlg.Multiselect = false;
            openFileDlg.Title = "File Selection";
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true) { ReplaceCover(openFileDlg.FileNames[0]); }
        }

        private void Cover_Drop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Debug.WriteLine("--> Cover_Drop");
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                Debug.WriteLine("--> FILES !!");
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
                if (files.Length > 1) { System.Windows.Forms.MessageBox.Show("Too many files, only 1 file allowed"); return; }
                ReplaceCover(files[0]);
            }
        }

        private void ReplaceCover(string file)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.Length > 200 * 1024 && false)
            {
                MessageBox.Show("File too large, maximum size 200 Kio", "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            else
            {
                string ext = System.IO.Path.GetExtension(file).ToLower();
                if (!AutorizedCoverFileExtention.Contains(ext)) { MessageBox.Show("Invalid file format"); return; }
                changeCoverPreview(null, file);

                int max = 800;
                double width = ((Bitmap)Cover.BackgroundImage).Width, height = ((Bitmap)Cover.BackgroundImage).Height;
                if (width > max) { height = (height / width) * max; width = max; }
                if (height > max) { width = (width / height) * max; height = max; }
                Debug.WriteLine("Picture Old Size: " + ((Bitmap)Cover.BackgroundImage).Width + "x" + ((Bitmap)Cover.BackgroundImage).Height);
                Debug.WriteLine("Picture Calc Size: " + width + "x" + height);

                System.Drawing.Image im = System.Drawing.Image.FromFile(file).GetThumbnailImage(Convert.ToInt32(width), Convert.ToInt32(height), null, IntPtr.Zero);

                MemoryStream ms2 = new MemoryStream();
                im.Save(ms2, GetEncoderInfo("image/jpeg"),
                    new EncoderParameters(1) { Param = new EncoderParameter[] { new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L) } }); // <-- Error occurs on this line
                ms2.Position = 0;
                pictures = new TagLib.IPicture[1] {
                        new TagLib.Picture() {
                            Type = TagLib.PictureType.FrontCover, MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                            Description = "Cover", Data = TagLib.ByteVector.FromStream(ms2)
                        }
                    };
                ms2.Close();

                CoverChanged = true;
                SaveButton.Enabled = true;
            }
        }

        private void changeCoverPreview(TagLib.IPicture pic = null, string path = null)
        {
            if (pic != null)
            {
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapCover = new Bitmap(ms);
                ms.Close();
            }
            else if (path != null)
            {
                if (!System.IO.File.Exists(path)) { return; }
                string ext = System.IO.Path.GetExtension(path).ToLower();
                if (!AutorizedCoverFileExtention.Contains(ext)) { return; }
                BitmapCover = new Bitmap(path);
            }
            Cover.BackgroundImage = BitmapCover;
            Cover.Tag = path;
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (YearInput.Enabled && !YearInput.Text.IsDigits()) { MessageBox.Show("Invalid Year value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            if (DiscInput.Enabled && !DiscInput.Text.IsDigits()) { MessageBox.Show("Invalid Disc number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            if (DiscCountInput.Enabled && !DiscCountInput.Text.IsDigits()) { MessageBox.Show("Invalid Disc total number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            if (TrackInput.Enabled && !TrackInput.Text.IsDigits()) { MessageBox.Show("Invalid Track number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            if (TrackCountInput.Enabled && !TrackCountInput.Text.IsDigits()) { MessageBox.Show("Invalid Tracks total number value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            tTitle = TitleInput.Text.Trim();
            tAlbum = AlbumInput.Text.Trim();
            tPerformers = PerformersInput.Text.Trim();
            tComposers = ComposersInput.Text.Trim();
            tAlbumArtists = AlbumArtistsInput.Text.Trim();

            MJObject details = new MJObject();
            details["Mode"] = Mode;
            details["Lyrics"] = LyricsInput.Text.Trim();
            details["Title"] = tTitle;
            details["Performers"] = tPerformers;
            details["Composers"] = tComposers;
            details["Album"] = tAlbum;
            details["Genres"] = GenresInput.Text.Trim();
            details["AlbumArtists"] = tAlbumArtists;
            details["Year"] = YearInput.Text.Trim();
            details["Disc"] = DiscInput.Text.Trim();
            details["DiscCount"] = DiscCountInput.Text.Trim();
            details["Track"] = TrackInput.Text.Trim();
            details["TrackCount"] = TrackCountInput.Text.Trim();
            details["Copyright"] = CopyrightInput.Text.Trim();
            details["CoverChanged"] = CoverChanged;

            App.scheduller.AddTask(new SchedullerTaskItem()
            {
                Action = "SaveTags",
                ActionResume = "Save Tags for " + Files.ToArray(),
                ExtraData = details,
                File = string.Join(';', Files),
                Time = DateTime.Now,
                _Status = SchedullerTaskItemStatus.Pending,
                ExtraObject = (CoverChanged)?pictures:null
            });

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static (bool, string) SaveTags(SchedullerTaskItem item)
        {
            string[] Files = item.File.Split(';');
            TagLib.File Tags;
            MJObject details = item.ExtraData;
            string Mode = details.GetValueStrict<string>("Mode", "track");
            bool CoverChanged = details.GetValueStrict<bool>("CoverChanged", false);

            try
            {
                if (Mode == "track")
                {
                    if (!System.IO.File.Exists(Files[0])) { return (false, "File not found"); }
                    using (Tags = TagLib.File.Create(Files[0]))
                    {
                        Tags.RemoveTags(TagTypes.AllTags);
                        Tags.Save();
                        Tags.Dispose();
                    }

                    using (Tags = TagLib.File.Create(Files[0]))
                    {
                        Tags.Tag.Lyrics = details.GetValueStrict<string>("Lyrics", "");
                        Tags.Tag.Title = details.GetValueStrict<string>("Title", "");
                        Tags.Tag.Performers = details.GetValueStrict<string>("Performers", "").Split(';');
                        Tags.Tag.Composers = details.GetValueStrict<string>("Composers", "").Split(';');
                        Tags.Tag.Album = details.GetValueStrict<string>("Album", "");
                        Tags.Tag.Genres = details.GetValueStrict<string>("Genres", "").Split(';');
                        Tags.Tag.AlbumArtists = details.GetValueStrict<string>("AlbumArtists", "").Split(';');
                        Tags.Tag.Year = Convert.ToUInt32(details.GetValueStrict<string>("Year", "0"));
                        Tags.Tag.Disc = Convert.ToUInt32(details.GetValueStrict<string>("Disc", "0"));
                        Tags.Tag.DiscCount = Convert.ToUInt32(details.GetValueStrict<string>("DiscCount", "0"));
                        Tags.Tag.Track = Convert.ToUInt32(details.GetValueStrict<string>("Track", "0"));
                        Tags.Tag.TrackCount = Convert.ToUInt32(details.GetValueStrict<string>("TrackCount", "0"));
                        Tags.Tag.Copyright = details.GetValueStrict<string>("Copyright", "");

                        if (CoverChanged == true)
                        {
                            Tags.Tag.Pictures = (IPicture[])item.ExtraObject;
                        }
                        Tags.Save();
                        Tags.Dispose();
                    }
                    return (true, "");
                }
                else if (Mode == "album")
                {
                    foreach (string file in Files)
                    {
                        string tf = file.Trim();
                        if (tf.Length == 0) { continue; }
                        if (item.ParsedFiles.Contains(tf)) { continue; }
                        if (!System.IO.File.Exists(tf)) { item.ParsedFiles.Add(tf); item.Errors += 1; continue; }

                        try
                        {
                            using (Tags = TagLib.File.Create(file))
                            {
                                Tags.Tag.Album = details.GetValueStrict<string>("Album", "");
                                Tags.Tag.Genres = details.GetValueStrict<string>("Genres", "").Split(';');
                                Tags.Tag.AlbumArtists = details.GetValueStrict<string>("AlbumArtists", "").Split(';');
                                Tags.Tag.Year = Convert.ToUInt32(details.GetValueStrict<string>("Year", "0"));
                                Tags.Tag.Copyright = details.GetValueStrict<string>("Copyright", "");

                                if (CoverChanged == true)
                                {
                                    Tags.Tag.Pictures = (IPicture[])item.ExtraObject;
                                }
                                Tags.Save();
                                Tags.Dispose();
                                item.ParsedFiles.Add(tf);
                            }
                        }
                        catch (IOException) { }
                        catch (Exception)  { }
                    }

                    if (item.ParsedFiles.Count == Files.Length)
                    {
                        if (item.Errors > 0) { item.ActionResume = " - "+ item.Errors + " Errors"; }
                        return (true, "");
                    }
                }
            }
            catch (IOException ex)
            {
                return (false, ex.Message + "\r\n" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                return (false, ex.Message + "\r\n" + ex.StackTrace);
            }
            return (false, "Unknow error");
        }

    }
}
