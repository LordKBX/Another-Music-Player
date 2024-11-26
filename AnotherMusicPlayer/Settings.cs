#define WINDOWS
using System;
using System.IO;
using System.Numerics;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;

using System.Diagnostics;
using Newtonsoft.Json;
using System.Linq;
using System.Windows.Documents;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace AnotherMusicPlayer
{
    /// <summary> Class storing application settings </summary>
    internal class Settings
    {
        // Lang section
        public static string Lang { get; set; } = null;

        // Conversion section
        public static Int32 ConversionMode { get; set; } = 1;
        public static Int32 ConversionBitRate { get; set; } = 128;

        // Memery usage Section
        public static Int32 MemoryUsage { get; set; } = 1;

        // Library Section
        public static string LibFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public static bool LibFolderShowHiden { get; set; } = false;
        public static bool LibFolderShowUnixHiden { get; set; } = false;

        // Style Section
        public static string StyleName { get; set; } = "Dark";

        // Equalizer Section
        public static string EqualizerPreset { get; set; } = null;
        public static float EqualizerBand1 { get; set; } = 0;
        public static float EqualizerBand2 { get; set; } = 0;
        public static float EqualizerBand3 { get; set; } = 0;
        public static float EqualizerBand4 { get; set; } = 0;
        public static float EqualizerBand5 { get; set; } = 0;
        public static float EqualizerBand6 { get; set; } = 0;
        public static float EqualizerBand7 { get; set; } = 0;
        public static float EqualizerBand8 { get; set; } = 0;
        public static float EqualizerBand9 { get; set; } = 0;
        public static float EqualizerBand10 { get; set; } = 0;
        public static void UpdateEqualizer(int Band, float Gain)
        {
            if (Band == 0) { EqualizerBand1 = Gain; }
            else if (Band == 1) { EqualizerBand2 = Gain; }
            else if (Band == 2) { EqualizerBand3 = Gain; }
            else if (Band == 3) { EqualizerBand4 = Gain; }
            else if (Band == 4) { EqualizerBand5 = Gain; }
            else if (Band == 5) { EqualizerBand6 = Gain; }
            else if (Band == 6) { EqualizerBand7 = Gain; }
            else if (Band == 7) { EqualizerBand8 = Gain; }
            else if (Band == 8) { EqualizerBand9 = Gain; }
            else if (Band == 9) { EqualizerBand10 = Gain; }
        }

        // WindowSize section
        public static double LastWindowWidth { get; set; } = 550;
        public static double LastWindowHeight { get; set; } = 400;

        // WindowPosition section
        public static double LastWindowLeft { get; set; } = 100;
        public static double LastWindowTop { get; set; } = 100;

        // Last play Section
        public static Int32 LastPlaylistIndex { get; set; } = 0;
        public static long LastPlaylistDuration { get; set; } = 0;

        // auto play at statrt-up
        public static bool StartUpPlay { get; set; } = true;

        // auto play at statrt-up
        public static bool AlwaysOnTop { get; set; } = false;

        // RepeatButton Section
        public static Int32 LastRepeatStatus { get; set; } = 0;

        // RepeatButton Section
        public static bool AutoCloseLyrics { get; set; } = true;

        //private static MainWindow window = null;

        public static bool LoadSettings()
        {
            //window = (MainWindow)System.Windows.Application.Current.Windows[0];
            //Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
            Lang = App.bdd.DatabaseGetParam("Lang", App.Languages[0]);

            ConversionMode = Convert.ToInt32(App.bdd.DatabaseGetParam("ConversionMode", "1"));
            ConversionBitRate = Convert.ToInt32(App.bdd.DatabaseGetParam("ConversionBitRate", "128"));

            MemoryUsage = Convert.ToInt32(App.bdd.DatabaseGetParam("MemoryUsage", "1"));

            LibFolder = App.bdd.DatabaseGetParam("LibFolder", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            string v1 = App.bdd.DatabaseGetParam("LibFolderShowHiden", "0");
            string v2 = App.bdd.DatabaseGetParam("LibFolderShowUnixHiden", "0");
            string v3 = App.bdd.DatabaseGetParam("StartUpPlay", "1");
            string v4 = App.bdd.DatabaseGetParam("AlwaysOnTop", "0");
            string v5 = App.bdd.DatabaseGetParam("AutoCloseLyrics", "0");
            LibFolderShowHiden = (Convert.ToInt32(v1) == 0) ? false : true;
            LibFolderShowUnixHiden = (Convert.ToInt32(v2) == 0) ? false : true;
            StartUpPlay = (Convert.ToInt32(v3) == 0) ? false : true;
            AlwaysOnTop = (Convert.ToInt32(v4) == 0) ? false : true;
            AutoCloseLyrics = (Convert.ToInt32(v5) == 0) ? false : true;

            StyleName = App.bdd.DatabaseGetParam("StyleName", "Dark");

            EqualizerPreset = App.bdd.DatabaseGetParam("EqualizerPreset", null);
            EqualizerBand1 = EqualizerBand2 = EqualizerBand3 = EqualizerBand4 = EqualizerBand5 = EqualizerBand6 = EqualizerBand7 = EqualizerBand8 = EqualizerBand9 = EqualizerBand10 = 0;
            try
            {
                EqualizerBand1 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand1", "0"));
                EqualizerBand2 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand2", "0"));
                EqualizerBand3 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand3", "0"));
                EqualizerBand4 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand4", "0"));
                EqualizerBand5 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand5", "0"));
                EqualizerBand6 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand6", "0"));
                EqualizerBand7 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand7", "0"));
                EqualizerBand8 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand8", "0"));
                EqualizerBand9 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand9", "0"));
                EqualizerBand10 = float.Parse(App.bdd.DatabaseGetParam("EqualizerBand10", "0"));
            }
            catch { }

            LastWindowWidth = Convert.ToDouble(App.bdd.DatabaseGetParam("LastWindowWidth", "550"));
            LastWindowHeight = Convert.ToDouble(App.bdd.DatabaseGetParam("LastWindowHeight", "400"));

            LastWindowLeft = Convert.ToDouble(App.bdd.DatabaseGetParam("LastWindowLeft", "100"));
            LastWindowTop = Convert.ToDouble(App.bdd.DatabaseGetParam("LastWindowTop", "100"));

            LastPlaylistIndex = Convert.ToInt32(App.bdd.DatabaseGetParam("LastPlaylistIndex", "0"));
            LastPlaylistDuration = Convert.ToInt64(App.bdd.DatabaseGetParam("LastPlaylistDuration", "0"));
            LastRepeatStatus = Convert.ToInt32(App.bdd.DatabaseGetParam("LastRepeatStatus", "0"));
            //}));
            return true;
        }

        public static async Task<bool> SaveSettings()
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() =>
            {
                App.bdd.DatabaseSaveParam("Lang", Lang, "TEXT");

                App.bdd.DatabaseSaveParam("ConversionMode", "" + ConversionMode, "INT");
                App.bdd.DatabaseSaveParam("ConversionBitRate", "" + ConversionBitRate, "INT");

                App.bdd.DatabaseSaveParam("MemoryUsage", "" + MemoryUsage, "INT");

                App.bdd.DatabaseSaveParam("LibFolder", LibFolder, "TEXT");
                App.bdd.DatabaseSaveParam("LibFolderShowHiden", "" + ((LibFolderShowHiden) ? 1 : 0), "INT");
                App.bdd.DatabaseSaveParam("LibFolderShowUnixHiden", "" + ((LibFolderShowUnixHiden) ? 1 : 0), "INT");
                App.bdd.DatabaseSaveParam("StartUpPlay", "" + ((StartUpPlay) ? 1 : 0), "INT");
                App.bdd.DatabaseSaveParam("AlwaysOnTop", "" + ((AlwaysOnTop) ? 1 : 0), "INT");
                App.bdd.DatabaseSaveParam("AutoCloseLyrics", "" + ((AutoCloseLyrics) ? 1 : 0), "INT");

                App.bdd.DatabaseSaveParam("StyleName", StyleName, "TEXT");

                App.bdd.DatabaseSaveParam("EqualizerPreset", EqualizerPreset, "TEXT");
                App.bdd.DatabaseSaveParam("EqualizerBand1", "" + EqualizerBand1, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand2", "" + EqualizerBand2, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand3", "" + EqualizerBand3, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand4", "" + EqualizerBand4, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand5", "" + EqualizerBand5, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand6", "" + EqualizerBand6, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand7", "" + EqualizerBand7, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand8", "" + EqualizerBand8, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand9", "" + EqualizerBand9, "FLOAT");
                App.bdd.DatabaseSaveParam("EqualizerBand10", "" + EqualizerBand10, "FLOAT");

                App.bdd.DatabaseSaveParam("LastWindowWidth", "" + LastWindowWidth, "INT");
                App.bdd.DatabaseSaveParam("LastWindowHeight", "" + LastWindowHeight, "INT");

                App.bdd.DatabaseSaveParam("LastWindowLeft", "" + LastWindowLeft, "INT");
                App.bdd.DatabaseSaveParam("LastWindowTop", "" + LastWindowTop, "INT");

                App.bdd.DatabaseSaveParam("LastPlaylistIndex", "" + LastPlaylistIndex, "INT");
                App.bdd.DatabaseSaveParam("LastPlaylistDuration", "" + LastPlaylistDuration, "INT");
                App.bdd.DatabaseSaveParam("LastRepeatStatus", "" + LastRepeatStatus, "INT", true);
            }));
            return true;
        }

        public static void SaveSettingsAsync()
        {
            _ = Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() =>
            {
                Settings.SaveSettings();
            }));
        }
    }
}
