using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Newtonsoft.Json;

namespace AnotherMusicPlayer
{
    class LibraryPathNavigator
    {
        WrapPanel Contener;
        string RootPath = "";
        string CurrentPath = "";
        Library Parent;

        public LibraryPathNavigator(Library parent, WrapPanel contener, string basePath = null)
        {
            Contener = contener;
            Parent = parent;
            if (Directory.Exists(basePath)) { RootPath = basePath; }
            else
            {
                RootPath = Settings.LibFolder;
            }
        }

        public bool UpdateRootPath(string basePath = null)
        {
            if (Directory.Exists(basePath)) { RootPath = basePath; return true; }
            return false;
        }

        public void Display(string path = null)
        {
            Debug.WriteLine("--> LibraryPathNavigator.Display(" + path + ") <--");
            try
            {
                if (path == null) { path = CurrentPath; }
                if (Contener.Children.Count > 0)
                    Contener.Children.Clear();
                string workpath = path.Replace(RootPath, "");
                workpath = workpath.TrimStart(MainWindow.SeparatorChar);
                string[] workTab = workpath.Split(MainWindow.SeparatorChar);

                int index = 0;
                string newPath = RootPath;

                TextBlock tb = new TextBlock();

                tb.Style = Parent.Parent.FindResource("LibibraryNavigationPathItem") as Style;
                tb.Text = Parent.Parent.FindResource("LibraryNavigatorItemHome") as string;
                tb.Tag = RootPath;
                tb.MouseLeftButtonDown += PathClicked;
                tb.ContextMenu = MakeContextMenu();
                Contener.Children.Add(tb);

                foreach (string name in workTab)
                {
                    if (name == "") { continue; }

                    TextBlock tb2 = new TextBlock();
                    tb2.Style = Parent.Parent.FindResource("LibibraryNavigationPathItemAlt") as Style;
                    tb2.Text = "/";
                    Contener.Children.Add(tb2);

                    newPath += MainWindow.SeparatorChar + name;
                    TextBlock tb3 = new TextBlock();
                    tb3.Style = Parent.Parent.FindResource("LibibraryNavigationPathItem") as Style;
                    tb3.Text = name;
                    tb3.Tag = newPath;
                    tb3.MouseLeftButtonDown += PathClicked;
                    tb3.ContextMenu = MakeContextMenu();

                    Contener.Children.Add(tb3);

                    index += 1;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("--> LibraryPathNavigator.Display ERROR <--");
                Debug.WriteLine(JsonConvert.SerializeObject(err));
            }
        }

        public void DisplayAlt(string content = null)
        {
            Debug.WriteLine("--> LibraryPathNavigator.DisplayAlt(" + content + ") <--");
            if (content == null) { return; }
            try
            {
                if (Contener.Children.Count > 0)
                    Contener.Children.Clear();

                TextBlock tb = new TextBlock();
                tb.Style = Parent.Parent.FindResource("LibibraryNavigationPathItem") as Style;
                tb.Text = Parent.Parent.FindResource("LibraryNavigatorItemHome") as string;
                tb.Tag = RootPath;
                tb.MouseLeftButtonDown += PathClicked;
                tb.ContextMenu = MakeContextMenu();
                Contener.Children.Add(tb);

                TextBlock tb2 = new TextBlock();
                tb2.Style = Parent.Parent.FindResource("LibibraryNavigationPathItemAlt") as Style;
                tb2.Text = ">>";
                Contener.Children.Add(tb2);

                TextBlock tb3 = new TextBlock();
                tb3.Style = Parent.Parent.FindResource("LibibraryNavigationPathItemAlt") as Style;
                tb3.Text = content;
                Contener.Children.Add(tb3);
            }
            catch (Exception err)
            {
                Debug.WriteLine("--> LibraryPathNavigator.DisplayAlt ERROR <--");
                Debug.WriteLine(JsonConvert.SerializeObject(err));
            }
        }

        private void PathClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string tag = (string)((TextBlock)sender).Tag;
            string name = (string)((TextBlock)sender).Text;
            if (Parent != null)
                Parent.DisplayPath(tag);
            else
                Display(tag);
        }

        private ContextMenu MakeContextMenu()
        {
            ContextMenu cm = new LibraryContextMenu() { Style = Contener.FindResource("CustomContextMenuStyle") as Style };
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (((MenuItem)cm.Items[i]).Name.ToLower() == "addfolder")
                {
                    //((MenuItem)cm.Items[i]).Click += LibraryContextMenuAction_Add;
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "addshufflefolder")
                {
                    //((MenuItem)cm.Items[i]).Click += LibraryContextMenuAction_AddShuffle;
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "playfolder")
                {
                    //((MenuItem)cm.Items[i]).Click += LibraryContextMenuAction_Play;
                }
                else if (((MenuItem)cm.Items[i]).Name.ToLower() == "playshufflefolder")
                {
                    //((MenuItem)cm.Items[i]).Click += LibraryContextMenuAction_PlayShuffle;
                }
                else { ((MenuItem)cm.Items[i]).Visibility = Visibility.Collapsed; }
            }
            return cm;
        }



    }
}
