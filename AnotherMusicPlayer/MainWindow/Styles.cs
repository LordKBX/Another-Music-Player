using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Reflection;
using System.Resources;
using System.Collections;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : System.Windows.Window
    {
        public void StyleUpdate()
        {
            string filePath = "pack://application:,,,/AnotherMusicPlayer;component/Styles/" + Settings.StyleName + ".xaml";
            try
            {
                //Debug.WriteLine("pack://application:,,,/AnotherMusicPlayer;component/Styles/" + Settings.StyleName + ".xaml");
                Resources.MergedDictionaries[0].Clear();
                Resources.MergedDictionaries[0].BeginInit();
                Resources.MergedDictionaries[0] = new ResourceDictionary { Source = new Uri(filePath, UriKind.Absolute) };
                Resources.MergedDictionaries[0].EndInit();
                win1.Style = FindResource("CustomWindowStyle") as Style;
            }
            catch { }
        }

        public string[] StyleList()
        {
            List<string> liste = new List<string>();
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Stream stream = asm.GetManifestResourceStream(asm.GetName().Name + ".g.resources");
                using (ResourceReader reader = new ResourceReader(stream))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        string str = (string)entry.Key;
                        if (str.StartsWith("styles/"))
                        {
                            str = str.Replace("styles/", "").Replace(".baml", "");
                            str = str.ToUpper()[0] + str.Substring(1);
                            liste.Add(str);
                        }
                    }
                }
            }
            catch { }
            return liste.ToArray();
        }
    }
}
