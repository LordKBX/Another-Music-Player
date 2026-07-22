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
using System.Windows.Forms;

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
        public static FormWindowState LastWindowState { get; set; } = FormWindowState.Normal;

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

        // RepeatButton Section
        public static bool DisplayLiveLyrics { get; set; } = true;

        // Normalize Volume Section
        public static bool NormalizeVolume { get; set; } = true;

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
            string v6 = App.bdd.DatabaseGetParam("DisplayLiveLyrics", "1");
            string v7 = App.bdd.DatabaseGetParam("NormalizeVolume", "1");
            LibFolderShowHiden = (Convert.ToInt32(v1) == 0) ? false : true;
            LibFolderShowUnixHiden = (Convert.ToInt32(v2) == 0) ? false : true;
            StartUpPlay = (Convert.ToInt32(v3) == 0) ? false : true;
            AlwaysOnTop = (Convert.ToInt32(v4) == 0) ? false : true;
            AutoCloseLyrics = (Convert.ToInt32(v5) == 0) ? false : true;
            DisplayLiveLyrics = (Convert.ToInt32(v6) == 0) ? false : true;
            NormalizeVolume = (Convert.ToInt32(v7) == 0) ? false : true;

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
            FormWindowState? v = EnumHelper<FormWindowState>.Parse2(App.bdd.DatabaseGetParam("LastWindowState", "Normal"));
            if (v == null) { v = FormWindowState.Normal; }
            LastWindowState = (FormWindowState)v;

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
                List<ParamObject> Params = new List<ParamObject>();
                Params.Add(new ParamObject() { Name = "Lang", TypeName = "TEXT", Value = Lang });
                Params.Add(new ParamObject() { Name = "ConversionMode", TypeName = "INT", Value = "" + ConversionMode });
                Params.Add(new ParamObject() { Name = "ConversionBitRate", TypeName = "INT", Value = "" + ConversionBitRate });
                Params.Add(new ParamObject() { Name = "MemoryUsage", TypeName = "INT", Value = "" + MemoryUsage });

                Params.Add(new ParamObject() { Name = "LibFolder", TypeName = "TEXT", Value = LibFolder });
                Params.Add(new ParamObject() { Name = "LibFolderShowHiden", TypeName = "INT", Value = "" + (LibFolderShowHiden?1:0) });
                Params.Add(new ParamObject() { Name = "LibFolderShowUnixHiden", TypeName = "INT", Value = "" + (LibFolderShowUnixHiden ? 1 : 0) });
                Params.Add(new ParamObject() { Name = "StartUpPlay", TypeName = "INT", Value = "" + (StartUpPlay ? 1 : 0) });
                Params.Add(new ParamObject() { Name = "AlwaysOnTop", TypeName = "INT", Value = "" + (AlwaysOnTop ? 1 : 0) });
                Params.Add(new ParamObject() { Name = "AutoCloseLyrics", TypeName = "INT", Value = "" + (AutoCloseLyrics ? 1 : 0) });
                Params.Add(new ParamObject() { Name = "DisplayLiveLyrics", TypeName = "INT", Value = "" + (DisplayLiveLyrics ? 1 : 0) });
                Params.Add(new ParamObject() { Name = "NormalizeVolume", TypeName = "INT", Value = "" + (NormalizeVolume ? 1 : 0) });

                Params.Add(new ParamObject() { Name = "StyleName", TypeName = "TEXT", Value = StyleName });

                Params.Add(new ParamObject() { Name = "EqualizerPreset", TypeName = "TEXT", Value = EqualizerPreset });
                Params.Add(new ParamObject() { Name = "EqualizerBand1", TypeName = "FLOAT", Value = "" + EqualizerBand1 });
                Params.Add(new ParamObject() { Name = "EqualizerBand2", TypeName = "FLOAT", Value = "" + EqualizerBand2 });
                Params.Add(new ParamObject() { Name = "EqualizerBand3", TypeName = "FLOAT", Value = "" + EqualizerBand3 });
                Params.Add(new ParamObject() { Name = "EqualizerBand4", TypeName = "FLOAT", Value = "" + EqualizerBand4 });
                Params.Add(new ParamObject() { Name = "EqualizerBand5", TypeName = "FLOAT", Value = "" + EqualizerBand5 });
                Params.Add(new ParamObject() { Name = "EqualizerBand6", TypeName = "FLOAT", Value = "" + EqualizerBand6 });
                Params.Add(new ParamObject() { Name = "EqualizerBand7", TypeName = "FLOAT", Value = "" + EqualizerBand7 });
                Params.Add(new ParamObject() { Name = "EqualizerBand8", TypeName = "FLOAT", Value = "" + EqualizerBand8 });
                Params.Add(new ParamObject() { Name = "EqualizerBand9", TypeName = "FLOAT", Value = "" + EqualizerBand9 });
                Params.Add(new ParamObject() { Name = "EqualizerBand10", TypeName = "FLOAT", Value = "" + EqualizerBand10 });

                Params.Add(new ParamObject() { Name = "LastWindowWidth", TypeName = "INT", Value = "" + LastWindowWidth });
                Params.Add(new ParamObject() { Name = "LastWindowHeight", TypeName = "INT", Value = "" + LastWindowHeight });
                Params.Add(new ParamObject() { Name = "LastWindowState", TypeName = "TEXT", Value = EnumHelper<FormWindowState>.GetDisplayValue(LastWindowState) });

                Params.Add(new ParamObject() { Name = "LastWindowLeft", TypeName = "INT", Value = "" + LastWindowLeft });
                Params.Add(new ParamObject() { Name = "LastWindowTop", TypeName = "INT", Value = "" + LastWindowTop });

                Params.Add(new ParamObject() { Name = "LastPlaylistIndex", TypeName = "INT", Value = "" + LastPlaylistIndex });
                Params.Add(new ParamObject() { Name = "LastPlaylistDuration", TypeName = "INT", Value = "" + LastPlaylistDuration });
                Params.Add(new ParamObject() { Name = "LastRepeatStatus", TypeName = "INT", Value = "" + LastRepeatStatus });

                App.bdd.DatabaseSaveParams(Params);
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

    public class ParamObject { 
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string Value { get; set; }
    }
}
