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
            UpdateTraduction();
        }

        /// <summary> load settings in parametters panel </summary>
        private void SettingsSetUp()
        {
            //Settings.DeleteSettings(); Settings.SaveSettings();

            if (Settings.Lang.StartsWith("fr-")) { ParamsLanguageVals.SelectedIndex = 1; }
            else { ParamsLanguageVals.SelectedIndex = 0; }
            ParamsLanguageVals.SelectionChanged += ParamsLanguageVals_SelectionChanged;

            if (Settings.ConversionMode == 1) { ParamsConvKeepVals.SelectedIndex = 0; }
            else { ParamsConvKeepVals.SelectedIndex = 1; }
            ParamsConvKeepVals.SelectionChanged += ParamsConvKeepVals_SelectionChanged;

            if (Settings.MemoryUsage == 1) { ParamsMemoryUsage.SelectedIndex = 1; }
            else { ParamsMemoryUsage.SelectedIndex = 0; }
            ParamsMemoryUsage.SelectionChanged += ParamsMemoryUsage_SelectionChanged;

            Int32 i = 0;
            foreach (ComboBoxItem cb in ParamsConvQualityVals.Items)
            {
                if (Convert.ToInt32((string)cb.Tag) == Settings.ConversionBitRate) { ParamsConvQualityVals.SelectedIndex = (int)i; break; }
                i += 1;
            }
            ParamsConvQualityVals.SelectionChanged += ParamsConvQualityVals_SelectionChanged;
            player.ConvQuality(Settings.ConversionBitRate);

            if (System.IO.Directory.Exists(Settings.LibFolder)) { ParamsLibFolderTextBox.Text = Settings.LibFolder; }
            else { Settings.LibFolder = null; }

            win1.Width = (Settings.LastWindowWidth > 500) ? Settings.LastWindowWidth : 500;
            win1.Height = (Settings.LastWindowHeight > 350) ? Settings.LastWindowHeight : 350;

            win1.Left = (Settings.LastWindowLeft < 0) ? 100 : Settings.LastWindowLeft;
            win1.Top = (Settings.LastWindowTop < 0) ? 100 : Settings.LastWindowTop;
        }

        private void ParamsMemoryUsage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.MemoryUsage = ((ComboBox)sender).SelectedIndex;
            Win1_SizeChanged(null, null);
            UpdateLeftPannelMediaInfo();

            MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            args.RoutedEvent = TextBlock.MouseDownEvent;
            args.Source = LibNavigationPathContener.Children[LibNavigationPathContener.Children.Count - 1];
            LibNavigationPathContener.Children[LibNavigationPathContener.Children.Count - 1].RaiseEvent(args);
            TabControler.SelectedIndex = 3;

            Settings.SaveSettings();
        }

        /// <summary> Callback parametter language combobox </summary>
        private void ParamsLanguageVals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            Settings.Lang = (string)item.Tag;
            Settings.SaveSettings();
            UpdateTraduction();
            if (PlayList.Count > 0 && PlayListIndex >= 0) { UpdateLeftPannelMediaInfo(); }
            if (MediatequeScanning) { MediatequeBuildNavigationScan(); }
            MediatequeBuildNavigationContent(MediatequeCurrentFolder ?? MediatequeRefFolder);
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
            player.ConvQuality(Settings.ConversionBitRate);
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
                _ = Dispatcher.InvokeAsync(new Action(() => {
                    MediatequeInvokeScan(true);
                }));
            }
        }

        /// <summary> Open a window for folder selection </summary>
        private string ParamsLibFolderBtn_Click_OpenFolder()
        {
            string path = null;
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.InitialDirectory = Settings.LibFolder; // Use current value for initial dir
            dialog.Title = GetTaduction("ParamsLibFolderSelectorTitle"); // instead of default "Save As"
            dialog.Filter = GetTaduction("ParamsLibFolderSelectorBlockerTitle") + "|*." + GetTaduction("ParamsLibFolderSelectorBlockerType"); // Prevents displaying files
            dialog.FileName = GetTaduction("ParamsLibFolderSelectorBlockerName"); // Filename will then be "select.this.directory"
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                // Remove fake filename from resulting path
                path = path.Replace("\\" + GetTaduction("ParamsLibFolderSelectorBlockerName") + "." + GetTaduction("ParamsLibFolderSelectorBlockerType"), "");
                path = path.Replace("." + GetTaduction("ParamsLibFolderSelectorBlockerType"), "");
                // If user has changed the filename, create the new directory
                if (!System.IO.Directory.Exists(path)) { return null; }
                // Our final value is in path
                //Debug.WriteLine(path);
            }
            return path;
        }
    }
}
