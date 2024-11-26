using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2Space
{
    internal static class SettingsManagment
    {
        private static MainWindow2 window;
        private static bool EventsDefined = false;
        private static Dictionary<string, Label> equalizerLabels = new Dictionary<string, Label>();

        public static void Init(MainWindow2 window) { 
            SettingsManagment.window = window; 
            LoadSettings();
            SetEvents();
        }

        public static void LoadSettings()
        {
            if (Settings.Lang == App.Languages[0]) { window.SettingsTabLangComboBox.SelectedIndex = 0; }
            else if (Settings.Lang == App.Languages[1]) { window.SettingsTabLangComboBox.SelectedIndex = 1; }
            else  { window.SettingsTabLangComboBox.SelectedIndex = -1; }

            Type tr = typeof(Properties.Resources);
            PropertyInfo[] props = tr.GetProperties(BindingFlags.Static | BindingFlags.Public);
            int selection = -1;
            foreach (PropertyInfo pi in props)
            {
                if (pi.Name.StartsWith("Style_"))
                {
                    string nm = pi.Name.Replace("Style_", "");
                    window.SettingsTabStyleComboBox.Items.Add(nm);
                    if (nm == Settings.StyleName) { selection = window.SettingsTabStyleComboBox.Items.Count - 1;  }
                }
            }
            window.SettingsTabStyleComboBox.SelectedIndex = selection;

            window.SettingsTabAutoPlayComboBox.SelectedIndex = (Settings.StartUpPlay) ? 1 : 0;
            window.SettingsTabAlwaysOnTopComboBox.SelectedIndex = (Settings.AlwaysOnTop) ? 1 : 0;
            window.SettingsTabAutoCloseLyricsComboBox.SelectedIndex = (Settings.AutoCloseLyrics) ? 1 : 0;

            window.SettingsTabConvModeComboBox.SelectedIndex = (Settings.ConversionMode - 1 >= 0) ? Settings.ConversionMode - 1 : 0;

            List<Int32> quals = Player.ConvQualityList.ToList();
            window.SettingsTabConvQualityComboBox.SelectedIndex = (quals.Contains(Settings.ConversionBitRate)) ? quals.IndexOf(Settings.ConversionBitRate) : 0;

            window.SettingsTabLibraryFolderTextBox.Text = Settings.LibFolder;
            window.SettingsTabLibraryUnixHiddenFileComboBox.SelectedIndex = (Settings.LibFolderShowUnixHiden) ? 1 : 0;
            window.SettingsTabLibraryWindowsHiddenFileComboBox.SelectedIndex = (Settings.LibFolderShowHiden) ? 1 : 0;

            List<string> keys = Player.EqualizerPresetsTab.Keys.ToList();
            keys.Insert(0, "Perso");
            int idof = keys.IndexOf(Settings.EqualizerPreset);
            int wantedId = (idof >= 0) ? idof : 0;

            if (window.SettingsTabEqualizerComboBox.Items.Count == 0)
            {
                for (int i = 0; i < keys.Count; i++) { window.SettingsTabEqualizerComboBox.Items.Add(keys[i]); }
            }
            window.SettingsTabEqualizerComboBox.SelectedIndex = (wantedId >= 0 && wantedId < window.SettingsTabEqualizerComboBox.Items.Count) ? wantedId : 0;

            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar01, window.SettingsTabEqualizerLabel01, Settings.EqualizerBand1);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar02, window.SettingsTabEqualizerLabel02, Settings.EqualizerBand2);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar03, window.SettingsTabEqualizerLabel03, Settings.EqualizerBand3);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar04, window.SettingsTabEqualizerLabel04, Settings.EqualizerBand4);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar05, window.SettingsTabEqualizerLabel05, Settings.EqualizerBand5);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar06, window.SettingsTabEqualizerLabel06, Settings.EqualizerBand6);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar07, window.SettingsTabEqualizerLabel07, Settings.EqualizerBand7);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar08, window.SettingsTabEqualizerLabel08, Settings.EqualizerBand8);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar09, window.SettingsTabEqualizerLabel09, Settings.EqualizerBand9);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar10, window.SettingsTabEqualizerLabel10, Settings.EqualizerBand10);
        }

        private static void SetEqualizerBandValue(TrackBar bar, Label label, float value) 
        { label.Text = "" + Math.Round(value, 2); bar.Value = Convert.ToInt32(Math.Truncate(value * 20)); }

        private static void SetEvents() 
        {
            EventsDefined = true;
            try {
                window.SettingsTabLangComboBox.SelectedIndexChanged += SettingsTabLangComboBox_SelectedIndexChanged;
                window.SettingsTabStyleComboBox.SelectedIndexChanged += SettingsTabStyleComboBox_SelectedIndexChanged;
                window.SettingsTabAutoPlayComboBox.SelectedIndexChanged += SettingsTabAutoPlayComboBox_SelectedIndexChanged;
                window.SettingsTabAlwaysOnTopComboBox.SelectedIndexChanged += SettingsTabAlwaysOnTopComboBox_SelectedIndexChanged;
                window.SettingsTabAutoCloseLyricsComboBox.SelectedIndexChanged += SettingsTabAutoCloseLyricsComboBox_SelectedIndexChanged;

                window.SettingsTabConvModeComboBox.SelectedIndexChanged += SettingsTabConvModeComboBox_SelectedIndexChanged;
                window.SettingsTabConvQualityComboBox.SelectedIndexChanged += SettingsTabConvQualityComboBox_SelectedIndexChanged;
                window.SettingsTabLibraryFolderTextBox.TextChanged += SettingsTabLibraryFolderTextBox_TextChanged;
                window.SettingsTabLibraryUnixHiddenFileComboBox.SelectedIndexChanged += SettingsTabLibraryUnixHiddenFileComboBox_SelectedIndexChanged;
                window.SettingsTabLibraryWindowsHiddenFileComboBox.SelectedIndexChanged += SettingsTabLibraryWindowsHiddenFileComboBox_SelectedIndexChanged;

                window.SettingsTabEqualizerComboBox.SelectedIndexChanged += SettingsTabEqualizerComboBox_SelectedIndexChanged;
                window.SettingsTabEqualizerButton.Click += SettingsTabEqualizerButton_Click;

                window.SettingsTabEqualizerTrackBar01.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar02.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar03.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar04.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar05.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar06.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar07.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar08.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar09.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };
                window.SettingsTabEqualizerTrackBar10.ValueChanged += (object sender, EventArgs e) => { SettingsTabEqualizerTrackBar_ValueChanged((TrackBar)sender); };

                equalizerLabels.Add("01", window.SettingsTabEqualizerLabel01);
                equalizerLabels.Add("02", window.SettingsTabEqualizerLabel02);
                equalizerLabels.Add("03", window.SettingsTabEqualizerLabel03);
                equalizerLabels.Add("04", window.SettingsTabEqualizerLabel04);
                equalizerLabels.Add("05", window.SettingsTabEqualizerLabel05);
                equalizerLabels.Add("06", window.SettingsTabEqualizerLabel06);
                equalizerLabels.Add("07", window.SettingsTabEqualizerLabel07);
                equalizerLabels.Add("08", window.SettingsTabEqualizerLabel08);
                equalizerLabels.Add("09", window.SettingsTabEqualizerLabel09);
                equalizerLabels.Add("10", window.SettingsTabEqualizerLabel10);
            }
            catch (Exception ex) { }
        }

        private static void SettingsTabLangComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                if (window.SettingsTabLangComboBox.SelectedIndex >= 0) { Settings.Lang = App.Languages[window.SettingsTabLangComboBox.SelectedIndex]; }
                else { return; }
                Settings.SaveSettings();
                window.Translate();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }

        private static void SettingsTabStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.StyleName = (string)window.SettingsTabStyleComboBox.SelectedItem;
            Settings.SaveSettings();
            window.UpdateStyle();
        }

        private static void SettingsTabAutoPlayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.StartUpPlay = (window.SettingsTabAutoPlayComboBox.SelectedIndex == 1);
            Settings.SaveSettings();
        }

        private static void SettingsTabAlwaysOnTopComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.AlwaysOnTop = (window.SettingsTabAlwaysOnTopComboBox.SelectedIndex == 1);
            Settings.SaveSettings();
            window.AlwaysOnTop(Settings.AlwaysOnTop);
        }

        private static void SettingsTabAutoCloseLyricsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.AutoCloseLyrics = (window.SettingsTabAutoCloseLyricsComboBox.SelectedIndex == 1);
            Settings.SaveSettings();
        }

        private static void SettingsTabConvModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = window.SettingsTabConvModeComboBox.SelectedIndex;
            Settings.ConversionMode = index + 1;
            Settings.SaveSettings();
        }

        private static void SettingsTabConvQualityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = window.SettingsTabConvQualityComboBox.SelectedIndex;
            if (index >= 0 && index < Player.ConvQualityList.Length) { Settings.ConversionBitRate = index; }
            Settings.SaveSettings();
        }

        private static void SettingsTabLibraryFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            string trimed = window.SettingsTabLibraryFolderTextBox.Text.Trim();
            if (trimed != Settings.LibFolder && Directory.Exists(trimed)) 
            {
                Settings.LibFolder = trimed;
                Settings.SaveSettings();
                App.win1.library.InvokeScan();
            }
        }

        private static void SettingsTabLibraryUnixHiddenFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool rez = (window.SettingsTabLibraryUnixHiddenFileComboBox.SelectedIndex == 1);
            if (rez != Settings.LibFolderShowUnixHiden)
            {
                Settings.LibFolderShowUnixHiden = rez;
                Settings.SaveSettings();
                App.win1.library.DisplayPath(App.win1.library.CurrentPath);
            }
        }

        private static void SettingsTabLibraryWindowsHiddenFileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool rez = (window.SettingsTabLibraryWindowsHiddenFileComboBox.SelectedIndex == 1);
            if (rez != Settings.LibFolderShowHiden)
            {
                Settings.LibFolderShowHiden = rez;
                Settings.SaveSettings();
                App.win1.library.DisplayPath(App.win1.library.CurrentPath);
            }
        }

        private static bool EqualizerTrackSuspended = false;
        private static bool EqualizerComboboxSuspended = false;
        private static void SettingsTabEqualizerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EqualizerComboboxSuspended) { return; }
            int id = window.SettingsTabEqualizerComboBox.SelectedIndex;
            if (id == 0) { id = 1; }
            List<string> keys = Player.EqualizerPresetsTab.Keys.ToList();
            keys.Insert(0, "Perso");
            float[] vals = Player.EqualizerPresetsTab[keys[id]];
            List<(int, float)> data = new List<(int, float)>();
            for (int i = 0; i < 10; i++) { data.Add((i, vals[i])); Settings.UpdateEqualizer(i, vals[i]); }
            EqualizerTrackSuspended = true;
            Player.UpdateEqualizer(data);
            Settings.SaveSettings();

            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar01, window.SettingsTabEqualizerLabel01, vals[0]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar02, window.SettingsTabEqualizerLabel02, vals[1]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar03, window.SettingsTabEqualizerLabel03, vals[2]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar04, window.SettingsTabEqualizerLabel04, vals[3]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar05, window.SettingsTabEqualizerLabel05, vals[4]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar06, window.SettingsTabEqualizerLabel06, vals[5]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar07, window.SettingsTabEqualizerLabel07, vals[6]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar08, window.SettingsTabEqualizerLabel08, vals[7]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar09, window.SettingsTabEqualizerLabel09, vals[8]);
            SetEqualizerBandValue(window.SettingsTabEqualizerTrackBar10, window.SettingsTabEqualizerLabel10, vals[9]);
            EqualizerTrackSuspended = false;
        }

        private static void SettingsTabEqualizerButton_Click(object sender, EventArgs e)
        { window.SettingsTabEqualizerComboBox.SelectedIndex = 1; }

        private static void SettingsTabEqualizerTrackBar_ValueChanged(TrackBar sender)
        {
            if (EqualizerTrackSuspended) { return; }
            EqualizerComboboxSuspended = true;
            window.SettingsTabEqualizerComboBox.SelectedIndex = 0;
            string name = sender.Name;
            string id = name.Substring(name.Length - 2);
            float calc = sender.Value / 20.0F;
            equalizerLabels[id].Text = "" + calc;
            if (id == "01") { Settings.EqualizerBand1 = calc; Player.UpdateEqualizer(0, calc); }
            else if (id == "02") { Settings.EqualizerBand2 = calc; Player.UpdateEqualizer(1, calc); }
            else if (id == "03") { Settings.EqualizerBand3 = calc; Player.UpdateEqualizer(2, calc); }
            else if (id == "04") { Settings.EqualizerBand4 = calc; Player.UpdateEqualizer(3, calc); }
            else if (id == "05") { Settings.EqualizerBand5 = calc; Player.UpdateEqualizer(4, calc); }
            else if (id == "06") { Settings.EqualizerBand6 = calc; Player.UpdateEqualizer(5, calc); }
            else if (id == "07") { Settings.EqualizerBand7 = calc; Player.UpdateEqualizer(6, calc); }
            else if (id == "08") { Settings.EqualizerBand8 = calc; Player.UpdateEqualizer(7, calc); }
            else if (id == "09") { Settings.EqualizerBand9 = calc; Player.UpdateEqualizer(8, calc); }
            else if (id == "10") { Settings.EqualizerBand10 = calc; Player.UpdateEqualizer(9, calc); }
            Settings.SaveSettings();
            EqualizerComboboxSuspended = false;
        }
    }
}
