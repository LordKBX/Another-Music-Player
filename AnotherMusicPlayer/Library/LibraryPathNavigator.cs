using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Forms;
using AnotherMusicPlayer.MainWindow2Space;

namespace AnotherMusicPlayer
{
    class LibraryPathNavigator
    {
        FlowLayoutPanel Contener;
        string RootPath = "";
        string CurrentPath = "";
        Library Parent;

        public LibraryPathNavigator(Library parent, FlowLayoutPanel contener, string basePath = null)
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
                if (Contener.Controls.Count > 0)
                    Contener.Controls.Clear();
                string workpath = path.Replace(RootPath, "");
                workpath = workpath.TrimStart(MainWindow2.SeparatorChar);
                string[] workTab = workpath.Split(MainWindow2.SeparatorChar);

                int index = 0;
                string newPath = RootPath;

                Label tb = new Label() { Margin = new Padding(3, 7, 3, 7), AutoSize = true };
                //tb.Style = Parent.Parent.FindResource("LibibraryNavigationPathItem") as Style;
                tb.Text = App.GetTranslation("LibraryNavigatorItemHome");
                tb.Tag = RootPath;
                tb.MouseDown += PathClicked;
                tb.ContextMenuStrip = MakeContextMenu(tb, RootPath);
                Contener.Controls.Add(tb);

                foreach (string name in workTab)
                {
                    if (name == "") { continue; }

                    Label tb2 = new Label() { Margin = new Padding(3,5,3,5), AutoSize = true };
                    //tb2.Style = Parent.Parent.FindResource("LibibraryNavigationPathItemAlt") as Style;
                    tb2.Text = "/";
                    Contener.Controls.Add(tb2);

                    newPath += MainWindow2.SeparatorChar + name;
                    Label tb3 = new Label() { Margin = new Padding(3, 7, 3, 7), AutoSize = true };
                    //tb3.Style = Parent.Parent.FindResource("LibibraryNavigationPathItem") as Style;
                    tb3.Text = name;
                    tb3.Tag = newPath;
                    tb3.MouseDown += PathClicked;
                    tb3.ContextMenuStrip = MakeContextMenu(tb3, newPath);

                    Contener.Controls.Add(tb3);

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
                if (Contener.Controls.Count > 0)
                    Contener.Controls.Clear();

                Label tb = new Label();
                //tb.Style = Parent.Parent.FindResource("LibibraryNavigationPathItem") as Style;
                tb.Text = App.GetTranslation("LibraryNavigatorItemHome");
                tb.Tag = RootPath;
                tb.MouseDown += PathClicked;
                tb.ContextMenuStrip = MakeContextMenu(tb, RootPath);
                Contener.Controls.Add(tb);

                Label tb2 = new Label();
                //tb2.Style = Parent.Parent.FindResource("LibibraryNavigationPathItemAlt") as Style;
                tb2.Text = ">>";
                Contener.Controls.Add(tb2);

                Label tb3 = new Label();
                //tb3.Style = Parent.Parent.FindResource("LibibraryNavigationPathItemAlt") as Style;
                tb3.Text = content;
                Contener.Controls.Add(tb3);
            }
            catch (Exception err)
            {
                Debug.WriteLine("--> LibraryPathNavigator.DisplayAlt ERROR <--");
                Debug.WriteLine(JsonConvert.SerializeObject(err));
            }
        }

        private void PathClicked(object sender, MouseEventArgs e)
        {
            string tag = (string)((Label)sender).Tag;
            string name = (string)((Label)sender).Text;
            if (Parent != null)
                Parent.DisplayPath(tag);
            else
                Display(tag);
        }

        private ContextMenuStrip MakeContextMenu(Label parent, string path)
        {
            //ContextMenu cm = new LibraryContextMenu() { Style = Contener.FindResource("CustomContextMenuStyle") as Style };
            bool back = (RootPath != path) ? true : false;
            string backPath = "";
            if (path != RootPath) { path = Directory.GetParent(path).FullName; }

            LibraryContextMenu cm = App.win1.library.MakeContextMenu(parent, "folder", back, backPath);
            cm.EditFolder.Visible = false;
            cm.PlayListsAddFolder.Visible = false;
            for (int i = 0; i < cm.Items.Count; i++)
            {
                if (((ToolStripItem)cm.Items[i]).Name.ToLower() == "addfolder")
                {
                    //((ToolStripItem)cm.Items[i]).Click += LibraryContextMenuAction_Add;
                }
                else if (((ToolStripItem)cm.Items[i]).Name.ToLower() == "addshufflefolder")
                {
                    //((ToolStripItem)cm.Items[i]).Click += LibraryContextMenuAction_AddShuffle;
                }
                else if (((ToolStripItem)cm.Items[i]).Name.ToLower() == "playfolder")
                {
                    //((ToolStripItem)cm.Items[i]).Click += LibraryContextMenuAction_Play;
                }
                else if (((ToolStripItem)cm.Items[i]).Name.ToLower() == "playshufflefolder")
                {
                    //((ToolStripItem)cm.Items[i]).Click += LibraryContextMenuAction_PlayShuffle;
                }
                else { ((ToolStripItem)cm.Items[i]).Visible = false; }
            }
            return cm;
        }



    }
}
