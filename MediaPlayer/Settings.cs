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
using Advexp;
using Advexp.DynamicSettings;
using Advexp.LocalDynamicSettings;

namespace MediaPlayer
{
    class Settings : Advexp.Settings<Settings>
    {
        [Setting(Name = "Local.Lang", Default = null)]
        public static string Lang { get; set; } = null;

        [Setting(Name = "Local.ConversionMode", Default = 1)]
        public static Int32 ConversionMode { get; set; } = 1;

        [Setting(Name = "Local.ConversionBitRate", Default = "128")]
        public static Int32 ConversionBitRate { get; set; } = 128;

        [Setting(Name = "Local.LibFolder", Default = null)]
        public static string LibFolder { get; set; } = null;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private void SettingsInit()
        {
            Settings.LoadSettings();
            if (Settings.Lang == null) { if (AppLang.StartsWith("fr-")) { Settings.Lang = "fr-FR"; } else { Settings.Lang = "en-US"; } }
        }

        private void SettingsSetUp()
        {
            //Settings.DeleteSettings(); Settings.SaveSettings();

            if (Settings.Lang == null) { Settings.Lang = (AppLang.StartsWith("fr-")) ? "fr-FR" : "en-US"; }
            if (Settings.Lang.StartsWith("fr-")) { ParamsLanguageVals.SelectedIndex = 1; }
            else { ParamsLanguageVals.SelectedIndex = 0; }
            ParamsLanguageVals.SelectionChanged += ParamsLanguageVals_SelectionChanged;

            if (Settings.ConversionMode == 1) { ParamsConvKeepVals.SelectedIndex = 0; }
            else { ParamsConvKeepVals.SelectedIndex = 1; }
            ParamsConvKeepVals.SelectionChanged += ParamsConvKeepVals_SelectionChanged;

            Int32 i = 0;
            foreach (ComboBoxItem cb in ParamsConvQualityVals.Items)
            {
                if (Convert.ToInt32((string)cb.Tag) == Settings.ConversionBitRate) { ParamsConvQualityVals.SelectedIndex = (int)i; break; }
                i += 1;
            }
            ParamsConvQualityVals.SelectionChanged += ParamsConvQualityVals_SelectionChanged;
            player.ConvQuality(Settings.ConversionBitRate);

            if (Settings.LibFolder != null)
            {
                if (System.IO.Directory.Exists(Settings.LibFolder)) { ParamsLibFolderTextBox.Text = Settings.LibFolder; }
                else { Settings.LibFolder = null; }
            }
            else {
                ParamsLibFolderTextBox.Text = Settings.LibFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                Settings.SaveSettings();
            }
        }

        private void ParamsLanguageVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.Lang = (string)item.Tag;
            Settings.SaveSettings();
            Traduction();
        }

        private void ParamsConvKeepVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.ConversionMode = ((ComboBox)sender).SelectedIndex + 1;
            Settings.SaveSettings();
        }

        private void ParamsConvQualityVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.ConversionBitRate = Convert.ToInt32((string)item.Tag);
            Settings.SaveSettings();
            player.ConvQuality(Settings.ConversionBitRate);
        }
    }
}
