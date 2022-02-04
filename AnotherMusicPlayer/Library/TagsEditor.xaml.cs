using System;
using System.IO;
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
using System.Drawing.Imaging;
using System.Diagnostics;
using TagLib;
using Newtonsoft.Json;
using System.Threading;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Logique d'interaction pour TagsEditor.xaml
    /// </summary>
    public partial class TagsEditor : Window
    {
        private MainWindow Parent;
        private string Mode;
        private TagLib.File Tags;
        private TagLib.IPicture[] pictures;
        BitmapImage BitmapCover;
        List<string> AutorizedCoverFileExtention = new List<string> { ".bmp", ".jpg", ".jpeg", ".png" };
        string DefaultCover;
        public bool CoverChanged = false;
        public bool Saved = false;
        private List<string> Files;
        private bool IsInitialized = false;

        public TagsEditor(MainWindow parent, string mode = "track", string[] files = null)
        {
            if (parent == null) { return; }
            if (mode == null) { return; }
            if (files == null) { return; }
            mode = mode.ToLower();
            if (mode == "file") { mode = "track"; }
            if (mode != "track" && mode != "album") { mode = "track"; }
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
            Parent = parent;
            Mode = mode;

            InitializeComponent();
            try
            {
                Resources.MergedDictionaries[0].Clear();
                Resources.MergedDictionaries[0] = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/AnotherMusicPlayer;component/Styles/" + Settings.StyleName + ".xaml", UriKind.Absolute)
                };
                Style = FindResource("CustomWindowStyle") as Style;
            }
            catch { }
            try
            {
                Resources.MergedDictionaries[1].Clear();
                Resources.MergedDictionaries[1] = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/AnotherMusicPlayer;component/Traductions/" + Settings.Lang.Split('-')[0] + ".xaml", UriKind.Absolute)
                };
                Style = FindResource("CustomWindowStyle") as Style;
            }
            catch { }

            DefaultCover = MainWindow.BaseDir + "icons" + MainWindow.SeparatorChar + "album_small.png";

            if (mode == "album")
            {
                LyricsLabel.Visibility = Visibility.Collapsed;
                LyricsInput.Visibility = Visibility.Collapsed;

                TitleLabel.Visibility = Visibility.Collapsed;
                TitleInput.Visibility = Visibility.Collapsed;

                PerformersLabel.Visibility = Visibility.Collapsed;
                PerformersInput.Visibility = Visibility.Collapsed;

                ComposersLabel.Visibility = Visibility.Collapsed;
                ComposersInput.Visibility = Visibility.Collapsed;

                DiscLabel.Visibility = Visibility.Collapsed;
                DiscGrid.Visibility = Visibility.Collapsed;

                TrackLabel.Visibility = Visibility.Collapsed;
                TrackGrid.Visibility = Visibility.Collapsed;

                this.MinHeight = this.MaxHeight = 340;
            }
            else { this.MinHeight = this.MaxHeight = 570; }

            if (mode == "track")
            {
                TitleInput.Text = Tags.Tag.Title ?? System.IO.Path.GetFileName(files[0]);
                TitleInput.TextChanged += TextInput_TextChanged;

                PerformersInput.Text = Tags.Tag.JoinedPerformers;
                PerformersInput.TextChanged += TextInput_TextChanged;
                PerformersInput.ToolTip = FindResource("EditorTagSeparateEntrys") as string;

                ComposersInput.Text = Tags.Tag.JoinedComposers;
                ComposersInput.TextChanged += TextInput_TextChanged;
                ComposersInput.ToolTip = FindResource("EditorTagSeparateEntrys") as string;

                LyricsInput.Text = Tags.Tag.Lyrics;
                LyricsInput.TextChanged += TextInput_TextChanged;
            }
            AlbumInput.Text = Tags.Tag.Album;
            AlbumInput.TextChanged += TextInput_TextChanged;

            GenresInput.Text = Tags.Tag.JoinedGenres;
            GenresInput.TextChanged += TextInput_TextChanged;
            GenresInput.ToolTip = FindResource("EditorTagSeparateEntrys") as string;

            AlbumArtistsInput.Text = Tags.Tag.JoinedAlbumArtists;
            AlbumArtistsInput.TextChanged += TextInput_TextChanged;
            AlbumArtistsInput.ToolTip = FindResource("EditorTagSeparateEntrys") as string;

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
            Cover.Drop += Cover_Drop;

            CoverCMPreview.Click += CoverCMPreview_Click;
            CoverCMClear.Click += CoverCMClear_Click;
            CoverCMAdd.Click += CoverCMAdd_Click;

            Tags.Dispose();
            IsInitialized = true;
        }

        public bool? ShowDialog2(Window owner = null)
        {
            this.ShowInTaskbar = false;
            this.Owner = owner;
            //this.Topmost = true;
            if (IsInitialized) { return this.ShowDialog(); }
            return false;
        }

        private void CoverCMPreview_Click(object sender, RoutedEventArgs e)
        {
            double maxw = System.Windows.SystemParameters.PrimaryScreenWidth - 100;
            double maxh = System.Windows.SystemParameters.PrimaryScreenHeight - 100;
            double width = (BitmapCover.PixelWidth < maxw) ? BitmapCover.PixelWidth : maxw;
            double height = (BitmapCover.PixelHeight < maxw) ? BitmapCover.PixelHeight : maxh;
            Window win = new Window();
            win.WindowStyle = WindowStyle.ToolWindow;
            win.Width = win.MinWidth = win.MaxWidth = width;
            win.Height = win.MinHeight = win.MaxHeight = height;
            win.Content = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            StackPanel st = new StackPanel();
            Image imp = new Image() { Style = Parent.FindResource("HQImg") as Style };
            imp.Source = BitmapCover;
            st.Children.Add(imp);
            ((ScrollViewer)win.Content).Content = st;
            win.Owner = this;
            win.ShowInTaskbar = false;
            win.ShowDialog();
        }

        private void CoverCMClear_Click(object sender, RoutedEventArgs e)
        {
            pictures = new TagLib.IPicture[0] { };
            changeCoverPreview(null, DefaultCover);
            CoverChanged = true;
            SaveButton.IsEnabled = true;
        }

        private void CoverCMAdd_Click(object sender, RoutedEventArgs e)
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

        private void Cover_Drop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("--> Cover_Drop");
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Debug.WriteLine("--> FILES !!");
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 1) { MessageBox.Show("Too many files, only 1 file allowed"); return; }
                ReplaceCover(files[0]);
            }
        }

        private void ReplaceCover(string file)
        {
            FileInfo fi = new FileInfo(file);
            if (fi.Length > 200 * 1024 && false)
            {
                MessageBox.Show("File too large, maximum size 200 Kio", "Error !", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
            else
            {
                string ext = System.IO.Path.GetExtension(file).ToLower();
                if (!AutorizedCoverFileExtention.Contains(ext)) { MessageBox.Show("Invalid file format"); return; }
                changeCoverPreview(null, file);

                int max = 800;
                double width = ((BitmapImage)Cover.Source).PixelWidth, height = ((BitmapImage)Cover.Source).PixelHeight;
                if (width > max) { height = (height / width) * max; width = max; }
                if (height > max) { width = (width / height) * max; height = max; }
                Debug.WriteLine("Picture Old Size: " + ((BitmapImage)Cover.Source).PixelWidth + "x" + ((BitmapImage)Cover.Source).PixelHeight);
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
                SaveButton.IsEnabled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == "track")
            {
                using (Tags = TagLib.File.Create(Files[0]))
                {
                    Tags.RemoveTags(TagTypes.AllTags);
                    Tags.Save();
                    Tags.Dispose();
                }

                using (Tags = TagLib.File.Create(Files[0]))
                {
                    Tags.Tag.Lyrics = LyricsInput.Text;
                    Tags.Tag.Title = TitleInput.Text;
                    Tags.Tag.Performers = PerformersInput.Text.Split(';');
                    Tags.Tag.Composers = ComposersInput.Text.Split(';');
                    Tags.Tag.Album = AlbumInput.Text;
                    Tags.Tag.Genres = GenresInput.Text.Split(';');
                    Tags.Tag.AlbumArtists = AlbumArtistsInput.Text.Split(';');
                    Tags.Tag.Year = Convert.ToUInt32(YearInput.Text);
                    Tags.Tag.Disc = Convert.ToUInt32(DiscInput.Text);
                    Tags.Tag.DiscCount = Convert.ToUInt32(DiscCountInput.Text);
                    Tags.Tag.Track = Convert.ToUInt32(TrackInput.Text);
                    Tags.Tag.TrackCount = Convert.ToUInt32(TrackCountInput.Text);
                    Tags.Tag.Copyright = CopyrightInput.Text;

                    if (Cover.Tag != DefaultCover && CoverChanged == true)
                    {
                        Tags.Tag.Pictures = pictures;
                    }
                    Tags.Save();
                    Tags.Dispose();
                }
            }
            else if (Mode == "album")
            {
                foreach (string file in Files)
                {
                    using (Tags = TagLib.File.Create(file))
                    {
                        Tags.Tag.Album = AlbumInput.Text;
                        Tags.Tag.Genres = GenresInput.Text.Split(';');
                        Tags.Tag.AlbumArtists = AlbumArtistsInput.Text.Split(';');
                        Tags.Tag.Year = Convert.ToUInt32(YearInput.Text);
                        Tags.Tag.Copyright = CopyrightInput.Text;

                        if (Cover.Tag != DefaultCover && CoverChanged == true)
                        {
                            Tags.Tag.Pictures = pictures;
                        }
                        Tags.Save();
                        Tags.Dispose();
                    }
                }
            }

            Saved = true;
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

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.IsEnabled = true;
        }

        private void changeCoverPreview(TagLib.IPicture pic = null, string path = null)
        {
            if (pic != null)
            {
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapCover = new BitmapImage();
                BitmapCover.BeginInit();
                BitmapCover.StreamSource = ms;
                BitmapCover.EndInit();
                BitmapCover.Freeze();
            }
            else if (path != null)
            {
                if (!System.IO.File.Exists(path)) { return; }
                string ext = System.IO.Path.GetExtension(path).ToLower();
                if (!AutorizedCoverFileExtention.Contains(ext)) { return; }
                BitmapCover = new BitmapImage(new Uri(path));
                BitmapCover.Freeze();
            }
            Cover.Source = BitmapCover;
            Cover.Tag = path;
        }
    }
}
