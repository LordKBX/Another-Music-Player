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

        // RepeatButton Section
        public static Int32 LastRepeatStatus { get; set; } = 0;

        private static MainWindow window = null;

        public static bool LoadSettings()
        {
            window = (MainWindow)System.Windows.Application.Current.Windows[0];
            //Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {
            Lang = window.bdd.DatabaseGetParam("Lang", "en-US");

            ConversionMode = Convert.ToInt32(window.bdd.DatabaseGetParam("ConversionMode", "1"));
            ConversionBitRate = Convert.ToInt32(window.bdd.DatabaseGetParam("ConversionBitRate", "128"));

            MemoryUsage = Convert.ToInt32(window.bdd.DatabaseGetParam("MemoryUsage", "1"));

            LibFolder = window.bdd.DatabaseGetParam("LibFolder", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            string v1 = window.bdd.DatabaseGetParam("LibFolderShowHiden", "0");
            string v2 = window.bdd.DatabaseGetParam("LibFolderShowUnixHiden", "0");
            string v3 = window.bdd.DatabaseGetParam("StartUpPlay", "1");
            LibFolderShowHiden = (Convert.ToInt32(v1) == 0) ? false : true;
            LibFolderShowUnixHiden = (Convert.ToInt32(v2) == 0) ? false : true;
            StartUpPlay = (Convert.ToInt32(v3) == 0) ? false : true;

            StyleName = window.bdd.DatabaseGetParam("StyleName", "Dark");

            EqualizerPreset = window.bdd.DatabaseGetParam("EqualizerPreset", null);
            EqualizerBand1 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand1", "0")));
            EqualizerBand2 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand2", "0")));
            EqualizerBand3 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand3", "0")));
            EqualizerBand4 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand4", "0")));
            EqualizerBand5 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand5", "0")));
            EqualizerBand6 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand6", "0")));
            EqualizerBand7 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand7", "0")));
            EqualizerBand8 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand8", "0")));
            EqualizerBand9 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand9", "0")));
            EqualizerBand10 = ((float)Convert.ToDecimal(window.bdd.DatabaseGetParam("EqualizerBand10", "0")));

            LastWindowWidth = Convert.ToDouble(window.bdd.DatabaseGetParam("LastWindowWidth", "550"));
            LastWindowHeight = Convert.ToDouble(window.bdd.DatabaseGetParam("LastWindowHeight", "400"));

            LastWindowLeft = Convert.ToDouble(window.bdd.DatabaseGetParam("LastWindowLeft", "100"));
            LastWindowTop = Convert.ToDouble(window.bdd.DatabaseGetParam("LastWindowTop", "100"));

            LastPlaylistIndex = Convert.ToInt32(window.bdd.DatabaseGetParam("LastPlaylistIndex", "0"));
            LastPlaylistDuration = Convert.ToInt64(window.bdd.DatabaseGetParam("LastPlaylistDuration", "0"));
            LastRepeatStatus = Convert.ToInt32(window.bdd.DatabaseGetParam("LastRepeatStatus", "0"));
            //}));
            return true;
        }

        public static async Task<bool> SaveSettings()
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() =>
            {
                window.bdd.DatabaseSaveParam("Lang", Lang, "TEXT");

                window.bdd.DatabaseSaveParam("ConversionMode", "" + ConversionMode, "INT");
                window.bdd.DatabaseSaveParam("ConversionBitRate", "" + ConversionBitRate, "INT");

                window.bdd.DatabaseSaveParam("MemoryUsage", "" + MemoryUsage, "INT");

                window.bdd.DatabaseSaveParam("LibFolder", LibFolder, "TEXT");
                window.bdd.DatabaseSaveParam("LibFolderShowHiden", "" + ((LibFolderShowHiden) ? 1 : 0), "INT");
                window.bdd.DatabaseSaveParam("LibFolderShowUnixHiden", "" + ((LibFolderShowUnixHiden) ? 1 : 0), "INT");
                window.bdd.DatabaseSaveParam("StartUpPlay", "" + ((StartUpPlay) ? 1 : 0), "INT");

                window.bdd.DatabaseSaveParam("StyleName", StyleName, "TEXT");

                window.bdd.DatabaseSaveParam("EqualizerPreset", EqualizerPreset, "TEXT");
                window.bdd.DatabaseSaveParam("EqualizerBand1", "" + EqualizerBand1, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand2", "" + EqualizerBand2, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand3", "" + EqualizerBand3, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand4", "" + EqualizerBand4, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand5", "" + EqualizerBand5, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand6", "" + EqualizerBand6, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand7", "" + EqualizerBand7, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand8", "" + EqualizerBand8, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand9", "" + EqualizerBand9, "FLOAT");
                window.bdd.DatabaseSaveParam("EqualizerBand10", "" + EqualizerBand10, "FLOAT");

                window.bdd.DatabaseSaveParam("LastWindowWidth", "" + LastWindowWidth, "INT");
                window.bdd.DatabaseSaveParam("LastWindowHeight", "" + LastWindowHeight, "INT");

                window.bdd.DatabaseSaveParam("LastWindowLeft", "" + LastWindowLeft, "INT");
                window.bdd.DatabaseSaveParam("LastWindowTop", "" + LastWindowTop, "INT");

                window.bdd.DatabaseSaveParam("LastPlaylistIndex", "" + LastPlaylistIndex, "INT");
                window.bdd.DatabaseSaveParam("LastPlaylistDuration", "" + LastPlaylistDuration, "INT");
                window.bdd.DatabaseSaveParam("LastRepeatStatus", "" + LastRepeatStatus, "INT", true);
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
