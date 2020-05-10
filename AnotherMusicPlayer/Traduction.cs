using System;
using System.Windows;
using System.ComponentModel;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Windows.Controls;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        private void Traduction()
        {
            if (Resources.MergedDictionaries.Count < 2) { Resources.MergedDictionaries.Add(new ResourceDictionary()); }
            string end = "";
            if (Settings.Lang.StartsWith("fr-")) { end = "fr"; }
            else { end = "en"; }
            Resources.MergedDictionaries[1].Clear();
            Resources.MergedDictionaries[1] = new ResourceDictionary { Source = new Uri(BaseDir + "Traductions" + Path.DirectorySeparatorChar + end + ".xaml", UriKind.Absolute) };
        }

        private string GetTaductionString(string reference) {
            try { return (string)Resources.MergedDictionaries[1][reference]; }
            catch { return ""; }
        }
    }
}
