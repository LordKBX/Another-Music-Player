﻿#define WINDOWS
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
    public partial class MainWindow : Window
    {

        /// <summary> Load and Initialize settings </summary>
        private void SettingsInit()
        {
            if (Settings.Lang == null) { Settings.Lang = (AppLang.StartsWith("fr-")) ? "fr-FR" : "en-US"; }
            if (!System.IO.Directory.Exists(Settings.LibFolder)) { Settings.LibFolder = null; }
            if (Settings.LibFolder == null) { Settings.LibFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); }
            PlayRepeatStatus = Settings.LastRepeatStatus;
            SettingsSetUp();
            EqualizerInitEvents();
            TranslationUpdate();
            StyleUpdate();
            string[] styles = StyleList();
            List<ComboBoxItem> li = new List<ComboBoxItem>();
            Debug.WriteLine(JsonConvert.SerializeObject(styles));

            int pos = 0;
            foreach (string file in styles)
            {
                li.Add(new ComboBoxItem()
                {
                    Content = file,
                    Tag = file,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
                if (file == Settings.StyleName) { pos = li.Count - 1; }
            }
            ParamsStyleVals.ItemsSource = null;
            ParamsStyleVals.ItemsSource = li;
            ParamsStyleVals.SelectedIndex = pos;
        }

        /// <summary> load settings in parametters panel </summary>
        private void SettingsSetUp()
        {
            //Settings.DeleteSettings(); Settings.SaveSettings();

            if (Settings.Lang.StartsWith("fr-")) { ParamsLanguageVals.SelectedIndex = 1; }
            else { ParamsLanguageVals.SelectedIndex = 0; }
            ParamsLanguageVals.SelectionChanged += ParamsLanguageVals_SelectionChanged;
            ParamsStyleVals.SelectionChanged += ParamsStyleVals_SelectionChanged;

            if (Settings.ConversionMode == 1) { ParamsConvKeepVals.SelectedIndex = 0; }
            else { ParamsConvKeepVals.SelectedIndex = 1; }
            ParamsConvKeepVals.SelectionChanged += ParamsConvKeepVals_SelectionChanged;

            if (Settings.MemoryUsage == 1) { ParamsMemoryUsage.SelectedIndex = 1; }
            else { ParamsMemoryUsage.SelectedIndex = 0; }
            ParamsMemoryUsage.SelectionChanged += ParamsMemoryUsage_SelectionChanged;

            if (Settings.StartUpPlay == true) { ParamsAutoPlay.SelectedIndex = 1; }
            else { ParamsAutoPlay.SelectedIndex = 0; }
            ParamsAutoPlay.SelectionChanged += ParamsAutoPlay_SelectionChanged;

            if (Settings.AlwaysOnTop == true) { ParamsAlwaysOnTop.SelectedIndex = 1; }
            else { ParamsAlwaysOnTop.SelectedIndex = 0; }
            ParamsAlwaysOnTop.SelectionChanged += ParamsAlwaysOnTop_SelectionChanged;

            if (Settings.LibFolderShowUnixHiden == true) { ParamsLibHiddenFilesUnixVals.SelectedIndex = 1; }
            else { ParamsLibHiddenFilesUnixVals.SelectedIndex = 0; }
            ParamsLibHiddenFilesUnixVals.SelectionChanged += ParamsLibHiddenFilesUnixVals_SelectionChanged;

            if (Settings.LibFolderShowHiden == true) { ParamsLibHiddenFilesWindowsVals.SelectedIndex = 1; }
            else { ParamsLibHiddenFilesWindowsVals.SelectedIndex = 0; }
            ParamsLibHiddenFilesWindowsVals.SelectionChanged += ParamsLibHiddenFilesWindowsVals_SelectionChanged;

            Int32 i = 0;
            foreach (ComboBoxItem cb in ParamsConvQualityVals.Items)
            {
                if (Convert.ToInt32((string)cb.Tag) == Settings.ConversionBitRate) { ParamsConvQualityVals.SelectedIndex = (int)i; break; }
                i += 1;
            }
            ParamsConvQualityVals.SelectionChanged += ParamsConvQualityVals_SelectionChanged;
            Player.ConvQuality(Settings.ConversionBitRate);

            if (System.IO.Directory.Exists(Settings.LibFolder)) { ParamsLibFolderTextBox.Text = Settings.LibFolder; }
            else { Settings.LibFolder = null; }

            Width = (Settings.LastWindowWidth > 500) ? Settings.LastWindowWidth : 500;
            Height = (Settings.LastWindowHeight > 350) ? Settings.LastWindowHeight : 350;

            Left = (Settings.LastWindowLeft < 0) ? 100 : Settings.LastWindowLeft;
            Top = (Settings.LastWindowTop < 0) ? 100 : Settings.LastWindowTop;
        }

        private void ParamsAlwaysOnTop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.AlwaysOnTop = (((ComboBox)sender).SelectedIndex == 0) ? false : true;
            MainWindow.Instance.Topmost = Settings.AlwaysOnTop;
            Settings.SaveSettings();
        }

        private void ParamsAutoPlay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.StartUpPlay = (((ComboBox)sender).SelectedIndex == 0) ? false : true;
            Settings.SaveSettings();
        }

        private void ParamsMemoryUsage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.MemoryUsage = ((ComboBox)sender).SelectedIndex;
            Win1_SizeChanged(null, null);
            UpdateLeftPannelMediaInfo();

            MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            args.RoutedEvent = TextBlock.MouseDownEvent;
            args.Source = LibibraryNavigationPathContener.Children[LibibraryNavigationPathContener.Children.Count - 1];
            LibibraryNavigationPathContener.Children[LibibraryNavigationPathContener.Children.Count - 1].RaiseEvent(args);
            TabControler.SelectedIndex = 3;

            Settings.SaveSettings();
        }

        /// <summary> Callback parametter language combobox </summary>
        private void ParamsStyleVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.StyleName = (string)item.Tag;
            Settings.SaveSettings();
            StyleUpdate();
            try { library.DisplayPath(library.CurrentPath); } catch { }
        }

        /// <summary> Callback parametter language combobox </summary>
        private void ParamsLanguageVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.Lang = (string)item.Tag;
            Settings.SaveSettings();
            TranslationUpdate();
            if (PlayList.Count > 0 && PlayListIndex >= 0) { UpdateLeftPannelMediaInfo(); }
            if (!library.Scanning) { library.DisplayPath(library.CurrentPath); }
            playLists.Init();
        }

        /// <summary> Callback parametter conversion mode combobox </summary>
        private void ParamsConvKeepVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.ConversionMode = ((ComboBox)sender).SelectedIndex + 1;
            Settings.SaveSettings();
        }

        /// <summary> Callback parametter language combobox </summary>
        private void ParamsConvQualityVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.ConversionBitRate = Convert.ToInt32((string)item.Tag);
            Settings.SaveSettings();
            Player.ConvQuality(Settings.ConversionBitRate);
        }

        /// <summary> Callback click bouton de selection dossier </summary>
        private void ParamsLibFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = ParamsLibFolderBtn_Click_OpenFolder();
            if (path != null)
            {
                Debug.WriteLine("ParamsLibFolderBtn_Click => " + path);
                ParamsLibFolderTextBox.Text = path;
                Settings.LibFolder = path;
                Settings.SaveSettings();
                _ = Dispatcher.InvokeAsync(new Action(() =>
                {
                    library.Scan(true);
                }));
            }
        }

        private void ParamsLibHiddenFilesWindowsVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.LibFolderShowHiden = ((string)item.Tag == "0") ? false : true;
            Settings.SaveSettings();
            library.DisplayPath(library.CurrentPath);
        }

        private void ParamsLibHiddenFilesUnixVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.LibFolderShowUnixHiden = ((string)item.Tag == "0") ? false : true;
            Settings.SaveSettings();
            library.DisplayPath(library.CurrentPath);
        }

        /// <summary> Open a window for folder selection </summary>
        private string ParamsLibFolderBtn_Click_OpenFolder()
        {
            string path = null;
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = Settings.LibFolder; // Use current value for initial dir
            dialog.Title = GetTranslation("ParamsLibFolderSelectorTitle"); // instead of default "Save As"
            dialog.Filter = GetTranslation("ParamsLibFolderSelectorBlockerTitle") + "|*." + GetTranslation("ParamsLibFolderSelectorBlockerType"); // Prevents displaying files
            dialog.FileName = GetTranslation("ParamsLibFolderSelectorBlockerName"); // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\" + GetTranslation("ParamsLibFolderSelectorBlockerName") + "." + GetTranslation("ParamsLibFolderSelectorBlockerType"), "");
                path = path.Replace("." + GetTranslation("ParamsLibFolderSelectorBlockerType"), "");
                // If user has changed the filename, create the new directory
                if (!System.IO.Directory.Exists(path)) { return null; }
                // Our final value is in path
                //Debug.WriteLine(path);
            }
            return path;
        }
    }
}
