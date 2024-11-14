using System;
using System.Runtime;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, float[]> EqualizerPresetsTab = new Dictionary<string, float[]>() {
            { "Flat", new float[10]{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f } },
            { "Classic", new float[10]{ -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -7.2f, -7.2f, -7.2f, -9.6f } },
            { "Club", new float[10]{ -1.11022e-15f, -1.11022e-15f, 8.0f, 5.6f, 5.6f, 5.6f, 3.2f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f } },
            { "Dance", new float[10]{ 9.6f, 7.2f, 2.4f, -1.11022e-15f, -1.11022e-15f, -5.6f, -7.2f, -7.2f, -1.11022e-15f, -1.11022e-15f } },
            { "FullBass", new float[10]{ -8.0f, 9.6f, 9.6f, 5.6f, 1.6f, -4.0f, -8.0f, -10.4f, -11.2f, -11.2f } },
            { "FullBassTreble", new float[10]{ 7.2f, 5.6f, -1.11022e-15f, -7.2f, -4.8f, 1.6f, 8.0f, 11.2f, 12.0f, 12.0f } },
            { "FullTreble", new float[10]{ -9.6f, -9.6f, -9.6f, -4.0f, 2.4f, 11.2f, 16.0f, 16.0f, 16.0f, 16.8f } },
            { "Headphones", new float[10]{ 4.8f, 11.2f, 5.6f, -3.2f, -2.4f, 1.6f, 4.8f, 9.6f, 12.8f, 14.4f } },
            { "LargeHall", new float[10]{ 10.4f, 10.4f, 5.6f, 5.6f, -1.11022e-15f, -4.8f, -4.8f, -4.8f, -1.11022e-15f, -1.11022e-15f } },
            { "Live", new float[10]{ -4.8f, -1.11022e-15f, 4.0f, 5.6f, 5.6f, 5.6f, 4.0f, 2.4f, 2.4f, 2.4f } },
            { "Party", new float[10]{ 7.2f, 7.2f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, 7.2f, 7.2f } },
            { "Pop", new float[10]{ -1.6f, 4.8f, 7.2f, 8.0f, 5.6f, -1.11022e-15f, -2.4f, -2.4f, -1.6f, -1.6f } },
            { "Reggae", new float[10]{ -1.11022e-15f, -1.11022e-15f, -1.11022e-15f, -5.6f, -1.11022e-15f, 6.4f, 6.4f, -1.11022e-15f, -1.11022e-15f, -1.11022e-15f } },
            { "Rock", new float[10]{ 8.0f, 4.8f, -5.6f, -8.0f, -3.2f, 4.0f, 8.8f, 11.2f, 11.2f, 11.2f } },
            { "Ska", new float[10]{ -2.4f, -4.8f, -4.0f, -1.11022e-15f, 4.0f, 5.6f, 8.8f, 9.6f, 11.2f, 9.6f } },
            { "Soft", new float[10]{ 4.8f, 1.6f, -1.11022e-15f, -2.4f, -1.11022e-15f, 4.0f, 8.0f, 9.6f, 11.2f, 12.0f } },
            { "SoftRock", new float[10]{ 4.0f, 4.0f, 2.4f, -1.11022e-15f, -4.0f, -5.6f, -3.2f, -1.11022e-15f, 2.4f, 8.8f } },
            { "Techno", new float[10]{ 8.0f, 5.6f, -1.11022e-15f, -5.6f, -4.8f, -1.11022e-15f, 8.0f, 9.6f, 9.6f, 8.8f } },
        };
        private int EqualizerBandFocusNb = 0;

        /// <summary> Function initialisation events on Equalizer UI </summary>
        private void EqualizerInitEvents()
        {
            //set minimum gain
            EqualizerBand1.Minimum = EqualizerBand2.Minimum = EqualizerBand3.Minimum = EqualizerBand4.Minimum
                = EqualizerBand5.Minimum = EqualizerBand6.Minimum = EqualizerBand7.Minimum = EqualizerBand8.Minimum
                = EqualizerBand9.Minimum = EqualizerBand10.Minimum = Player.MinimumGain;
            //set maximum gain
            EqualizerBand1.Maximum = EqualizerBand2.Maximum = EqualizerBand3.Maximum = EqualizerBand4.Maximum
                = EqualizerBand5.Maximum = EqualizerBand6.Maximum = EqualizerBand7.Maximum = EqualizerBand8.Maximum
                = EqualizerBand9.Maximum = EqualizerBand10.Maximum = Player.MaximumGain;

            //set event for changed value
            EqualizerBand1.ValueChanged += EqualizerBand_ValueChanged; EqualizerBand2.ValueChanged += EqualizerBand_ValueChanged;
            EqualizerBand3.ValueChanged += EqualizerBand_ValueChanged; EqualizerBand4.ValueChanged += EqualizerBand_ValueChanged;
            EqualizerBand5.ValueChanged += EqualizerBand_ValueChanged; EqualizerBand6.ValueChanged += EqualizerBand_ValueChanged;
            EqualizerBand7.ValueChanged += EqualizerBand_ValueChanged; EqualizerBand8.ValueChanged += EqualizerBand_ValueChanged;
            EqualizerBand9.ValueChanged += EqualizerBand_ValueChanged; EqualizerBand10.ValueChanged += EqualizerBand_ValueChanged;

            EqualizerBand1.GotFocus += EqualizerBand_GotFocus; EqualizerBand2.GotFocus += EqualizerBand_GotFocus;
            EqualizerBand3.GotFocus += EqualizerBand_GotFocus; EqualizerBand4.GotFocus += EqualizerBand_GotFocus;
            EqualizerBand5.GotFocus += EqualizerBand_GotFocus; EqualizerBand6.GotFocus += EqualizerBand_GotFocus;
            EqualizerBand7.GotFocus += EqualizerBand_GotFocus; EqualizerBand8.GotFocus += EqualizerBand_GotFocus;
            EqualizerBand9.GotFocus += EqualizerBand_GotFocus; EqualizerBand10.GotFocus += EqualizerBand_GotFocus;

            EqualizerBand1.LostFocus += EqualizerBand_LostFocus; EqualizerBand2.LostFocus += EqualizerBand_LostFocus;
            EqualizerBand3.LostFocus += EqualizerBand_LostFocus; EqualizerBand4.LostFocus += EqualizerBand_LostFocus;
            EqualizerBand5.LostFocus += EqualizerBand_LostFocus; EqualizerBand6.LostFocus += EqualizerBand_LostFocus;
            EqualizerBand7.LostFocus += EqualizerBand_LostFocus; EqualizerBand8.LostFocus += EqualizerBand_LostFocus;
            EqualizerBand9.LostFocus += EqualizerBand_LostFocus; EqualizerBand10.LostFocus += EqualizerBand_LostFocus;

            if (Settings.EqualizerPreset == null || Settings.EqualizerPreset == "Perso")
            {
                EqualizerBand1.Value = Settings.EqualizerBand1; EqualizerBand2.Value = Settings.EqualizerBand2; EqualizerBand3.Value = Settings.EqualizerBand3;
                EqualizerBand4.Value = Settings.EqualizerBand4; EqualizerBand5.Value = Settings.EqualizerBand5; EqualizerBand6.Value = Settings.EqualizerBand6;
                EqualizerBand7.Value = Settings.EqualizerBand7; EqualizerBand8.Value = Settings.EqualizerBand8;
                EqualizerBand9.Value = Settings.EqualizerBand9; EqualizerBand10.Value = Settings.EqualizerBand10;
            }
            else
            {
                if (EqualizerPresetsTab.ContainsKey(Settings.EqualizerPreset))
                {
                    float[] preset = EqualizerPresetsTab[Settings.EqualizerPreset];
                    int i = 0;
                    foreach (var itm in ParamsEqualizerPresets.Items)
                    {
                        if (itm.GetType().Name != "ComboBoxItem") { continue; }
                        if ((string)((ComboBoxItem)itm).Tag == Settings.EqualizerPreset) { ParamsEqualizerPresets.SelectedIndex = i; }
                        i += 1;
                    }

                    EqualizerBand1.Value = preset[0]; EqualizerBand2.Value = preset[1]; EqualizerBand3.Value = preset[2];
                    EqualizerBand4.Value = preset[3]; EqualizerBand5.Value = preset[4]; EqualizerBand6.Value = preset[5];
                    EqualizerBand7.Value = preset[6]; EqualizerBand8.Value = preset[7];
                    EqualizerBand9.Value = preset[8]; EqualizerBand10.Value = preset[9];
                }
            }

            // set click event for reseting equalizer
            EqualizerReset.Click += (sender, e) =>
            {
                EqualizerBand1.Value = 0; EqualizerBand2.Value = 0; EqualizerBand3.Value = 0;
                EqualizerBand4.Value = 0; EqualizerBand5.Value = 0; EqualizerBand6.Value = 0;
                EqualizerBand7.Value = 0; EqualizerBand8.Value = 0; EqualizerBand9.Value = 0;
                EqualizerBand10.Value = 0; ParamsEqualizerPresets.SelectedIndex = 1;
            };

            ParamsEqualizerPresets.SelectionChanged += ParamsEqualizerPresets_SelectionChanged;
        }

        private void EqualizerBand_LostFocus(object sender, RoutedEventArgs e) { EqualizerBandFocusNb -= 1; }

        private void EqualizerBand_GotFocus(object sender, RoutedEventArgs e) { EqualizerBandFocusNb += 1; }

        private void ParamsEqualizerPresets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tag = (string)((ComboBoxItem)e.AddedItems[0]).Tag;
            //Debug.WriteLine("ParamsEqualizerPresets_SelectionChanged");
            //Debug.WriteLine(tag);
            if (EqualizerPresetsTab.ContainsKey(tag))
            {
                float[] preset = EqualizerPresetsTab[tag];

                EqualizerBand1.Value = preset[0]; EqualizerBand2.Value = preset[1]; EqualizerBand3.Value = preset[2];
                EqualizerBand4.Value = preset[3]; EqualizerBand5.Value = preset[4]; EqualizerBand6.Value = preset[5];
                EqualizerBand7.Value = preset[6]; EqualizerBand8.Value = preset[7];
                EqualizerBand9.Value = preset[8]; EqualizerBand10.Value = preset[9];

                Settings.EqualizerPreset = tag;
                Settings.SaveSettings();
            }
        }

        /// <summary> Callback for equalizer sliders when value changed </summary>
        private void EqualizerBand_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int col = Convert.ToInt32(((Slider)sender).Tag);
            NumberFormatInfo format = new NumberFormatInfo() { NumberDecimalDigits = 2, NumberGroupSizes = new int[] { 2, 2, 3 } };
            float NewValue = (float)Math.Round(e.NewValue, 2);
            Player.UpdateEqualizer(col, NewValue);
            if (EqualizerBandFocusNb > 0) { Settings.EqualizerPreset = "Perso"; }

            if (col == 0) { EqualizerLabel1.Text = Convert.ToString(NewValue); Settings.EqualizerBand1 = NewValue; }
            if (col == 1) { EqualizerLabel2.Text = Convert.ToString(NewValue); Settings.EqualizerBand2 = NewValue; }
            if (col == 2) { EqualizerLabel3.Text = Convert.ToString(NewValue); Settings.EqualizerBand3 = NewValue; }
            if (col == 3) { EqualizerLabel4.Text = Convert.ToString(NewValue); Settings.EqualizerBand4 = NewValue; }
            if (col == 4) { EqualizerLabel5.Text = Convert.ToString(NewValue); Settings.EqualizerBand5 = NewValue; }
            if (col == 5) { EqualizerLabel6.Text = Convert.ToString(NewValue); Settings.EqualizerBand6 = NewValue; }
            if (col == 6) { EqualizerLabel7.Text = Convert.ToString(NewValue); Settings.EqualizerBand7 = NewValue; }
            if (col == 7) { EqualizerLabel8.Text = Convert.ToString(NewValue); Settings.EqualizerBand8 = NewValue; }
            if (col == 8) { EqualizerLabel9.Text = Convert.ToString(NewValue); Settings.EqualizerBand9 = NewValue; }
            if (col == 9) { EqualizerLabel10.Text = Convert.ToString(NewValue); Settings.EqualizerBand10 = NewValue; }
            if (EqualizerBandFocusNb > 0) { ParamsEqualizerPresets.SelectedIndex = 0; }
            Settings.SaveSettings();
        }
    }
}
