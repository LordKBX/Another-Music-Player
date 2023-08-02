using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
    /// Logique d'interaction pour AddRadio.xaml
    /// </summary>
    public partial class AddRadio : Window
    {
        private new MainWindow Parent = null;
        private bool CategoryMode = false;
        BitmapImage DefaultCover;
        BitmapImage BitmapCover;
        string Base64Cover = "";
        List<string> AutorizedCoverFileExtention = new List<string> { ".bmp", ".jpg", ".jpeg", ".png" };
        public bool Saved = false;

        public AddRadio(MainWindow parent, bool categoryMode = false, int categoryId = 0)
        {
            Owner = Parent = parent;
            CategoryMode = categoryMode;
            InitializeComponent();
            Resources = Parent.Resources;
            Style = FindResource("CustomWindowStyle") as Style;

            if (categoryMode) { CategoryRadioGrid.Visibility = Visibility.Visible; RadioGrid.Visibility = Visibility.Hidden; }
            else
            {
                CategoryRadioGrid.Visibility = Visibility.Hidden; RadioGrid.Visibility = Visibility.Visible;

                CategoryInput.Items.Add(new ComboBoxItem() { Content = Parent.FindResource("PlayListsRadioDefault") as string, Tag = "" + 0 });

                Dictionary<string, Dictionary<string, object>> rez = Parent.bdd.DatabaseQuery("SELECT CRID, Name FROM radiosCategories ORDER BY Name ASC", "Name");

                foreach (KeyValuePair<string, Dictionary<string, object>> row in rez)
                {
                    CategoryInput.Items.Add(new ComboBoxItem() { Content = row.Key, Tag = row.Value["CRID"] as string });
                    if ("" + categoryId == row.Value["CRID"] as string) { CategoryInput.SelectedIndex = CategoryInput.Items.Count - 1; }
                }
                if (CategoryInput.SelectedIndex == -1) { CategoryInput.SelectedIndex = 0; }
            }

            BtnClose.Click += (object sender, RoutedEventArgs e) => { Close(); };
            TopBar.MouseDown += TopBar_MouseDown;

            RadioCover.Source = DefaultCover = MainWindow.Bimage("CoverImg");
            RadioCover.Drop += Cover_Drop;

            BtnSave.Click += BtnSave_Click;
        }

        private void TypeLabel1_MouseDown(object sender, MouseButtonEventArgs e) { TypeToogle.IsChecked = false; }
        private void TypeLabel2_MouseDown(object sender, MouseButtonEventArgs e) { TypeToogle.IsChecked = true; }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); } }

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

                Bitmap bmp = FilesTags.ResizeImage(new System.Drawing.Bitmap(file), 250, 250);
                BitmapCover = FilesTags.ConvertBitmapToBitmapImage(bmp);
                BitmapCover.Freeze();
                RadioCover.Source = BitmapCover;
                RadioCover.Tag = file;

                Base64Cover = BitmapMagic.BitmapToBase64String(bmp, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryMode)
            {
                string Name = CategoryNameInput.Text.Trim();
                string Description = CategoryDescriptionInput.Text.Trim();
                if (Name == "") { return; }

                Parent.bdd.DatabaseQuerys(new string[] { "INSERT INTO radiosCategories(Name, Description, Logo) VALUES('" + Database.EscapeString(Name) + "','" + Database.EscapeString(Description) + "','" + Database.EscapeString(Base64Cover) + "')" }, true);
            }
            else
            {
                string Category = ((ComboBoxItem)CategoryInput.SelectedItem).Tag as string;
                string Name = NameInput.Text.Trim();
                string Description = DescriptionInput.Text.Trim();
                string Url = UrlInput.Text.Trim();
                string UrlPrefix = UrlPrefixInput.Text.Trim();
                string UrlType = (TypeToogle.IsChecked == true) ? "M3u" : "Stream";


                Parent.bdd.DatabaseQuerys(new string[] {
                    "INSERT INTO radios(Name, Description, Logo, Url, UrlPrefix, FType, Fragmented, Category) " +
                    "VALUES(" +
                    "'" + Database.EscapeString(Name) + "'," +
                    "'" + Database.EscapeString(Description) + "'," +
                    "'" + Database.EscapeString(Base64Cover) + "'," +
                    "'" + Database.EscapeString(Url) + "'," +
                    "'" + Database.EscapeString(UrlPrefix) + "'," +
                    "'" + Database.EscapeString(UrlType) + "'," +
                    "0," + Category + ")"
                }, true);
            }

            Saved = true;
            Close();
        }


    }
}
