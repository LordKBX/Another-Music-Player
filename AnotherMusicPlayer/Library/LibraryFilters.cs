using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Drawing;
using System.Linq;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        private Dictionary<int, List<string>> LibraryFiltersPages = null;
        private int LibraryFiltersPagesIndex = 0;

        private void LibrarySetupFilters() {
            LibraryFiltersMode.SelectedIndex = 0;
            LibraryFiltersMode.SelectionChanged += LibraryFiltersMode_SelectionChanged;
            LibraryFiltersGenreList.SelectionChanged += LibraryFiltersGenreList_SelectionChanged;

            LibraryFiltersPaginationPrevious.Click += LibraryFiltersPaginationPrevious_Click;
            LibraryFiltersPaginationNext.Click += LibraryFiltersPaginationNext_Click;

            LibraryFiltersSearchBox.KeyDown += (sender, e) => {
                //Debug.WriteLine(e.Key.ToString());
                if (e.Key.ToString() == "Return") {
                    LibNavigationContent2.Children.Clear();
                    string tag = (string)((ComboBoxItem)LibraryFiltersMode.SelectedItem).Tag;
                    string var = LibraryFiltersSearchBox.Text;
                    Dictionary<string, Dictionary<string, object>> files = null;
                    if (tag == "Artist") {
                        files = bdd.DatabaseQuery("SELECT * FROM files WHERE Performers LIKE '%" + Database.DatabaseEscapeString(var) + "%' OR Composers LIKE '%" + Database.DatabaseEscapeString(var) + "%' ORDER BY Album, Disc, Track, Name, Path ASC", "Path");
                    }
                    else { files = bdd.DatabaseQuery("SELECT * FROM files WHERE " + tag + " LIKE '%" + Database.DatabaseEscapeString(var) + "%' ORDER BY Album, Disc, Track, Name, Path ASC", "Path"); }
                    
                    int countAlbums = 0;
                    int page = 0;
                    string previousAlbum = null;
                    Dictionary<int, List<string>> pages = new Dictionary<int, List<string>>();
                    pages[page] = new List<string>();
                    foreach (KeyValuePair<string, Dictionary<string, object>> file in files)
                    {
                        if (previousAlbum != (string)file.Value["Album"])
                        {
                            countAlbums += 1; previousAlbum = (string)file.Value["Album"];
                            if (countAlbums > 1)
                            {
                                if ((countAlbums - 1) % 10 == 0) { page += 1; pages[page] = new List<string>(); }
                            }
                        }
                        pages[page].Add(file.Key);
                    }
                    if (countAlbums > 10)
                    {
                        LibraryFiltersPaginationBlock.Visibility = Visibility.Visible;
                        LibraryFiltersPaginationPrevious.IsEnabled = false;
                        LibraryFiltersPaginationNext.IsEnabled = true;
                        LibraryFiltersPaginationDisplay.Text = "1 / " + pages.Count;
                        LibraryBuildNavigationContentBlocks(pages[0].ToArray(), LibNavigationContent2, false);
                        LibraryFiltersPages = pages; LibraryFiltersPagesIndex = 0;
                    }
                    else
                    {
                        LibraryFiltersPaginationBlock.Visibility = Visibility.Collapsed;
                        LibraryBuildNavigationContentBlocks(files.Keys.ToArray(), LibNavigationContent2, false);
                    }
                }
                Dispatcher.BeginInvoke(new Action(() => {
                    LibNavigationContentB.Visibility = Visibility.Collapsed;
                    LibNavigationContent2B.Visibility = Visibility.Visible;
                }));
            };
        }

        private void LibraryFiltersPaginationPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (LibraryFiltersPaginationBlock.Visibility != Visibility.Visible) { return; }
            if (LibraryFiltersPagesIndex > 0)
            {
                LibraryFiltersPagesIndex -= 1;
                if (LibraryFiltersPagesIndex == 0) { LibraryFiltersPaginationPrevious.IsEnabled = false; LibraryFiltersPaginationNext.IsEnabled = true; }
                LibraryFiltersPaginationDisplay.Text = "" + (LibraryFiltersPagesIndex + 1) + " / " + LibraryFiltersPages.Count;
                LibNavigationContent2.Children.Clear();
                LibNavigationContentScroll2.ScrollToVerticalOffset(0);
                LibraryBuildNavigationContentBlocks(LibraryFiltersPages[LibraryFiltersPagesIndex].ToArray(), LibNavigationContent2, false);
            }
        }

        private void LibraryFiltersPaginationNext_Click(object sender, RoutedEventArgs e)
        {
            if (LibraryFiltersPaginationBlock.Visibility != Visibility.Visible) { return; }
            if (LibraryFiltersPagesIndex < LibraryFiltersPages.Count - 1)
            {
                LibraryFiltersPagesIndex += 1;
                if (LibraryFiltersPagesIndex >= LibraryFiltersPages.Count - 1) { LibraryFiltersPaginationPrevious.IsEnabled = true; LibraryFiltersPaginationNext.IsEnabled = false; }
                LibraryFiltersPaginationDisplay.Text = "" + (LibraryFiltersPagesIndex + 1) + " / " + LibraryFiltersPages.Count;
                LibNavigationContent2.Children.Clear();
                LibNavigationContentScroll2.ScrollToVerticalOffset(0);
                LibraryBuildNavigationContentBlocks(LibraryFiltersPages[LibraryFiltersPagesIndex].ToArray(), LibNavigationContent2, false);
            }
        }

        private void LibraryFiltersMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LibraryFiltersPages = null; LibraryFiltersPagesIndex = 0;
            LibraryBuildNavigationPath(LibraryRefFolder);
            string tag = (string)((ComboBoxItem)LibraryFiltersMode.SelectedItem).Tag;
            //Debug.WriteLine(tag);
            if (tag == "") {
                LibraryFiltersGenreList.Visibility = Visibility.Collapsed;
                LibraryFiltersSearchBox.Visibility = Visibility.Collapsed;
                //Debug.WriteLine(v.Name);
                LibraryBuildNavigationPath(LibraryRefFolder);
                LibraryBuildNavigationContent(LibraryRefFolder);

            }
            else if (tag == "Name" || tag == "Artist" || tag == "Album")
            {
                LibraryFiltersGenreList.Visibility = Visibility.Collapsed;
                LibraryFiltersSearchBox.Visibility = Visibility.Visible;
                LibraryFiltersSearchBox.Text = "";
            }
            else if (tag == "Genre")
            {
                LibraryFiltersGenreList.Visibility = Visibility.Visible;
                LibraryFiltersSearchBox.Visibility = Visibility.Collapsed;
                LibraryFiltersGenreList.SelectedIndex = 0;
            }
        }

        private void LibraryFiltersGenreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LibraryBuildNavigationPath(LibraryRefFolder);
            string mode = (string)((ComboBoxItem)LibraryFiltersMode.SelectedItem).Tag;
            string genre = "";
            try { genre = (string)((ComboBoxItem)LibraryFiltersGenreList.SelectedItem).Tag; }
            catch { }
            if (mode != "Genre") { return; }

            LibNavigationContent2.Children.Clear();
            List<UIElement> btl = new List<UIElement>();

            if (genre == null || genre.Trim() == "")
            {
                int i = -1;
                foreach (ComboBoxItem cbi in LibraryFiltersGenreList.Items)
                {
                    i += 1;
                    if ((string)cbi.Tag == "") { continue; }
                    btl.Add(LibraryBuildNavigationContentButtonFilter("genre", (string)cbi.Tag, ""+i));
                }
            }
            else {
                Dictionary<string, Dictionary<string, object>> files = bdd.DatabaseQuery("SELECT * FROM files WHERE Genres LIKE '%"+ genre + "%' ORDER BY Album, Disc, Track, Name, Path ASC", "Path");
                int countAlbums = 0;
                int page = 0;
                string previousAlbum = null;
                Dictionary<int, List<string>> pages = new Dictionary<int, List<string>>();
                pages[page] = new List<string>();
                foreach (KeyValuePair<string, Dictionary<string, object>> file in files) {
                    if (previousAlbum != (string)file.Value["Album"]) {
                        countAlbums += 1; previousAlbum = (string)file.Value["Album"];
                        if (countAlbums > 1)
                        {
                            if ((countAlbums - 1) % 10 == 0) { page += 1; pages[page] = new List<string>(); }
                        }
                    }
                    pages[page].Add(file.Key);
                }
                if (countAlbums > 10) { 
                    LibraryFiltersPaginationBlock.Visibility = Visibility.Visible;
                    LibraryFiltersPaginationPrevious.IsEnabled = false;
                    LibraryFiltersPaginationNext.IsEnabled = true;
                    LibraryFiltersPaginationDisplay.Text = "1 / " + pages.Count;
                    LibraryBuildNavigationContentBlocks(pages[0].ToArray(), LibNavigationContent2, false);
                    LibraryFiltersPages = pages; LibraryFiltersPagesIndex = 0;
                }
                else {
                    LibraryFiltersPaginationBlock.Visibility = Visibility.Collapsed;
                    LibraryBuildNavigationContentBlocks(files.Keys.ToArray(), LibNavigationContent2, false);
                }

            }
            Dispatcher.BeginInvoke(new Action(() => {
                LibNavigationContentB.Visibility = Visibility.Collapsed;
                LibNavigationContent2B.Visibility = Visibility.Visible;
            }));
        }

        /// <summary> Create button for the Content zone in Library pannel </summary>
        private Button LibraryBuildNavigationContentButtonFilter(string type, string name, string path)
        {
            if (type == null || name == null || path == null) { return null; }
            Button bt = new Button() { Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItem2"] };
            StackPanel gr = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Stretch, Orientation = Orientation.Vertical };

            if (name.Length > 30) name = name.Substring(0, 30) + "...";
            AccessText tx = new AccessText() { TextWrapping = TextWrapping.WrapWithOverflow, MaxHeight = 45, Text = name };
            tx.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemText2"];
            gr.Children.Add(tx);

            if (type == "file") {
                PlayListViewItem item = GetMediaInfo(path);

                AccessText tx2 = new AccessText() { TextWrapping = TextWrapping.WrapWithOverflow, MaxHeight = 45, Text = item.Album };
                tx2.Style = (Style)Resources.MergedDictionaries[0]["LibNavigationContentItemText2"];
                gr.Children.Add(tx2);
            }

            bt.Tag = new object[] { type, path };
            if (type == "file") { bt.MouseDoubleClick += LibraryNavigationContentButtonFilter_DoubleClick; }
            else { bt.Click += LibraryNavigationContentButtonFilter_Click; }
            
            bt.ContextMenu = LibMediaCreateContextMenu();

            bt.Content = gr;
            //LibNavigationContent.Children.Add(bt);
            return bt;
        }

        private void LibraryNavigationContentButtonFilter_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LibraryNavigationContentButtonFilter_Click(object sender, RoutedEventArgs e)
        {
            object[] re = (object[])((Button)sender).Tag;
            if ((string)re[1] == "genre") {
                LibraryFiltersGenreList.SelectedIndex = Convert.ToInt32((string)re[1]);
            }

            //LibraryBuildNavigationPath((Folder)re[2]);
            //LibraryBuildNavigationContent((Folder)re[2]);
        }
    }
}