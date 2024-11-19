using AnotherMusicPlayer.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    public static class App
    {
        public static readonly string AppName = "AnotherMusicPlayer";
        public static Database bdd = new Database();

        private static bool _IsDebug = false;
        public static bool IsDebug { get { return App._IsDebug; } }

        [Conditional("DEBUG")]
        public static void TestDebug() { App._IsDebug = true; }

        private static ResourceDictionary Resources = new ResourceDictionary();
        private static AnotherMusicPlayer.MainWindow2.MainWindow2 win1;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            TestDebug();
            Settings.LoadSettings();
            TranslationUpdate();
            Player.INIT();

            win1 = new AnotherMusicPlayer.MainWindow2.MainWindow2();
            win1.ShowDialog();
            win1.Dispose();
            Environment.Exit(0);
        }

        public static void UnsetLockScreen() { /*win1.UnsetLockScreen();*/ }

        /// <summary> Convert milliseconds times in human readable string </summary>
        public static string displayTime(long time)
        {
            string ret = ""; int Days = 0, Hours = 0, Minutes = 0;
            int ms = (int)(time % 1000); long TotalSeconds = (time - ms) / 1000, reste;
            if (TotalSeconds >= 86400) { reste = (TotalSeconds % 86400); Days = (int)((TotalSeconds - reste) / 86400); TotalSeconds = reste; }
            if (TotalSeconds >= 3600) { reste = (TotalSeconds % 3600); Hours = (int)((TotalSeconds - reste) / 3600); TotalSeconds = reste; }
            if (TotalSeconds >= 60) { reste = (TotalSeconds % 60); Minutes = (int)((TotalSeconds - reste) / 60); TotalSeconds = reste; }

            if (Days > 0) { ret += ((Days < 10) ? "0" : "") + Days + "d "; }
            if (Hours > 0) { ret += ((Hours < 10) ? "0" : "") + Hours + ":"; } //ret += ((Minutes < 10) ? "0" : "") + Minutes + ":" + ((TotalSeconds < 10) ? "0" : "") + TotalSeconds;
            return ret + ((Minutes < 10) ? "0" : "") + Minutes + ":" + ((TotalSeconds < 10) ? "0" : "") + TotalSeconds;
        }

        public static void TranslationUpdate()
        {
            if (Resources.MergedDictionaries.Count < 1) { Resources.MergedDictionaries.Add(new ResourceDictionary()); }
            string end = "";
            if (Settings.Lang == null) { end = "en"; }
            else if (Settings.Lang.StartsWith("fr-")) { end = "fr"; }
            else { end = "en"; }
            if (Resources.MergedDictionaries.Count < 1)
            {
                Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/AnotherMusicPlayer;component/Traductions/" + end + ".xaml", UriKind.Absolute) });
            }
            else
            {
                Resources.MergedDictionaries[0].Clear();
                Resources.MergedDictionaries[0] = new ResourceDictionary { Source = new Uri("pack://application:,,,/AnotherMusicPlayer;component/Traductions/" + end + ".xaml", UriKind.Absolute) };
            }
        }

        /// <summary> Get string stored in traduction file </summary>
        public static string GetTranslation(string reference)
        {
            try { return (string)Resources.MergedDictionaries[0][reference]; }
            catch { return ""; }
        }

        private static Dictionary<Control, ToolTip> DefinedToolTipArray = new Dictionary<Control, ToolTip>();

        public static bool SetToolTip(Control control, string tooltip)
        {
            try
            {
                if (DefinedToolTipArray.ContainsKey(control)) { DelToolTip(control); }
                ToolTip tp = new ToolTip();
                tp.SetToolTip(control, tooltip);
                DefinedToolTipArray.Add(control, tp);
                return true;
            }
            catch (Exception/* ex*/) { }
            return false;
        }

        public static bool DelToolTip(Control control)
        {
            try
            {
                if (DefinedToolTipArray.ContainsKey(control))
                {
                    DefinedToolTipArray[control].RemoveAll();
                    DefinedToolTipArray[control].Dispose();
                    DefinedToolTipArray.Remove(control);
                    return true;
                }
                else { return false; }
            }
            catch (Exception/* ex*/) { }
            return false;
        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
