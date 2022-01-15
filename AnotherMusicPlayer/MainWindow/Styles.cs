using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : System.Windows.Window
    {
        public void StyleUpdate() {
            string filePath = BaseDir + "Styles" + Path.DirectorySeparatorChar + Settings.StyleName + ".xaml";
            if (System.IO.File.Exists(filePath))
            {
                Resources.MergedDictionaries[0].Clear();
                Resources.MergedDictionaries[0] = new ResourceDictionary { Source = new Uri(filePath, UriKind.Absolute) };
                win1.Style = FindResource("CustomWindowStyle") as Style;
            }
        }

        public string[] StyleList() {
            string[] liste = Directory.GetFiles(BaseDir + "Styles", "*.xaml");
            List<string> liste2 = new List<string>();
            foreach (string file in liste) { liste2.Add(file.Replace(BaseDir + "Styles" + SeparatorChar, "").Replace(".xaml", "")); }
            return liste2.ToArray();
        }
    }
}
