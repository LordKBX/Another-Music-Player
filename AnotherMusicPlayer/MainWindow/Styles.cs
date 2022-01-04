using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : System.Windows.Window
    {
        public void StylesUpdate() {
            Resources.MergedDictionaries[1].Clear();
            Resources.MergedDictionaries[1] = new ResourceDictionary { Source = new Uri(BaseDir + "Traductions" + Path.DirectorySeparatorChar + Settings.StyleName + ".xaml", UriKind.Absolute) };
        }
    }
}
