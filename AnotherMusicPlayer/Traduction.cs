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

        /// <summary> Reload traduction file with selection of the file based on Settings.Lang </summary>
        private void UpdateTraduction()
        {
            if (Resources.MergedDictionaries.Count < 2) { Resources.MergedDictionaries.Add(new ResourceDictionary()); }
            string end = "";
            if (Settings.Lang.StartsWith("fr-")) { end = "fr"; }
            else { end = "en"; }
            Resources.MergedDictionaries[1].Clear();
            Resources.MergedDictionaries[1] = new ResourceDictionary { Source = new Uri(BaseDir + "Traductions" + Path.DirectorySeparatorChar + end + ".xaml", UriKind.Absolute) };
        }

        /// <summary> Get string stored in traduction file </summary>
        private string GetTaduction(string reference) {
            try { return (string)Resources.MergedDictionaries[1][reference]; }
            catch { return ""; }
        }
    }
}
