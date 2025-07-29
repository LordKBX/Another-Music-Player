using NAudio.Gui;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace AnotherMusicPlayer.MainWindow2Space
{
    static class Translation
    {
        public static void Translate(MainWindow2 window)
        {
            if (window.InvokeRequired) { window.Invoke(() => { Translate(window); }); return; }
            try {
                App.TranslationUpdate();
                // Main Tab Controler Tab labels
                window.PlaybackTab.Text = App.GetTranslation("TabPlaylist");
                window.LibraryTab.Text = App.GetTranslation("TabLibrary");
                window.PlayListsTab.Text = App.GetTranslation("TabRecordedPlaylists");
                window.SettingsTab.Text = App.GetTranslation("TabParams");

                // Window Buttons
                App.SetToolTip(window.MinimizeButton, App.GetTranslation("WindowBtnMinimize"));
                App.SetToolTip(window.MaximizeButton, App.GetTranslation("WindowBtnMaximize"));
                App.SetToolTip(window.CloseButton, App.GetTranslation("WindowBtnClose"));

                // Right panel buttons
                App.SetToolTip(window.BtnOpen, App.GetTranslation("BtnOpenToolTip"));
                App.SetToolTip(window.BtnPrevious, App.GetTranslation("BtnPreviousToolTip"));
                App.SetToolTip(window.BtnPlayPause, App.GetTranslation("BtnPlayPauseToolTip"));
                App.SetToolTip(window.BtnNext, App.GetTranslation("BtnNexToolTip"));
                App.SetToolTip(window.BtnRepeat, App.GetTranslation("BtnRepeatToolTip"));
                App.SetToolTip(window.BtnShuffle, App.GetTranslation("BtnShuffleToolTip"));
                App.SetToolTip(window.BtnClearList, App.GetTranslation("BtnClearListToolTip"));
                // Playback status
                App.SetToolTip(window.DisplayPlaybackPosition, App.GetTranslation("Position"));
                App.SetToolTip(window.DisplayPlaybackSize, App.GetTranslation("Duration"));
                App.SetToolTip(window.playbackProgressBar, App.GetTranslation("Progress"));

                window.PlaybackPositionLabel.Text = App.GetTranslation("PlaybackPositionLabel").Replace("%X%", "" + Player.PlayList.Count).Replace("%Y%", "" + (Settings.LastPlaylistIndex + 1));

                // Playback Tab left panel info
                window.PlaybackTabTitleLabelInfo.Text = App.GetTranslation("Title2");
                window.PlaybackTabAlbumLabelInfo.Text = App.GetTranslation("Album2");
                window.PlaybackTabArtistsLabelInfo.Text = App.GetTranslation("Artist2");
                window.PlaybackTabGenresLabelInfo.Text = App.GetTranslation("Genres2");
                window.PlaybackTabDurationLabelInfo.Text = App.GetTranslation("Duration2");
                window.PlaybackTabLyricsButton.Text = App.GetTranslation("Lyrics");

                // Library Tab
                window.LibraryFiltersModeLabel.Text = App.GetTranslation("LibraryFiltersModeLabel");
                window.LibraryFoldersLabel.Text = App.GetTranslation("LibraryFoldersLabel", "AA");
                window.LibraryFiltersMode.Items[0] = App.GetTranslation("LibraryFiltersMode_none");
                window.LibraryFiltersMode.Items[1] = App.GetTranslation("LibraryFiltersMode_name");
                window.LibraryFiltersMode.Items[2] = App.GetTranslation("LibraryFiltersMode_artist");
                window.LibraryFiltersMode.Items[3] = App.GetTranslation("LibraryFiltersMode_album");
                window.LibraryFiltersMode.Items[4] = App.GetTranslation("LibraryFiltersMode_genre");
                window.LibraryFiltersGenreSearchBox.PlaceholderText = " " + App.GetTranslation("LibraryFiltersGenreSearchBox.PlaceholderText");
                window.LibraryFiltersSearchBox.PlaceholderText = " " + App.GetTranslation("LibraryFiltersSearchBox.PlaceholderText");

                // Playlists Tab
                window.PlaylistsTree.Nodes[0].Text = App.GetTranslation("PlayListsAuto");
                window.PlaylistsTree.Nodes[1].Text = App.GetTranslation("PlayListsUser");
                window.PlaylistsTree.Nodes[2].Text = App.GetTranslation("PlayListsRadio");

                window.PlayListsTabVoidLabel.Text = App.GetTranslation("PlayListsTabVoidLabel");

                // settings Tab
                window.SettingsTabLangGroupBox.Text = App.GetTranslation("ParamsLanguageLabel");

                window.SettingsTabStyleGroupBox.Text = App.GetTranslation("ParamsStyleLabel");

                window.SettingsTabAutoPlayGroupBox.Text = App.GetTranslation("ParamsAutoPlayLabel");
                window.SettingsTabAutoPlayComboBox.Items[0] = App.GetTranslation("ParamsAutoPlay0");
                window.SettingsTabAutoPlayComboBox.Items[1] = App.GetTranslation("ParamsAutoPlay1");

                window.SettingsTabAlwaysOnTopGroupBox.Text = App.GetTranslation("ParamsAlwaysOnTopLabel");
                window.SettingsTabAlwaysOnTopComboBox.Items[0] = App.GetTranslation("ParamsAlwaysOnTop0");
                window.SettingsTabAlwaysOnTopComboBox.Items[1] = App.GetTranslation("ParamsAlwaysOnTop1");

                window.SettingsTabAutoCloseLyricsGroupBox.Text = App.GetTranslation("ParamsAutoCloseLyricsLabel");
                window.SettingsTabAutoCloseLyricsComboBox.Items[0] = App.GetTranslation("ParamsAutoCloseLyrics0");
                window.SettingsTabAutoCloseLyricsComboBox.Items[1] = App.GetTranslation("ParamsAutoCloseLyrics1");

                window.SettingsTabConvGroupBox.Text = App.GetTranslation("ParamsConvKeepLabel");
                window.SettingsTabConvModeLabel.Text = App.GetTranslation("ParamsConvModeLabel");
                window.SettingsTabConvModeComboBox.Items[0] = App.GetTranslation("ParamsConvKeepVals_TEMP");
                window.SettingsTabConvModeComboBox.Items[1] = App.GetTranslation("ParamsConvKeepVals_REPLACE");
                window.SettingsTabConvQualityLabel.Text = App.GetTranslation("ParamsConvQualityLabel");
                window.SettingsTabConvQualityComboBox.Items[0] = App.GetTranslation("ParamsConvQualityVals_96");
                window.SettingsTabConvQualityComboBox.Items[1] = App.GetTranslation("ParamsConvQualityVals_128");
                window.SettingsTabConvQualityComboBox.Items[2] = App.GetTranslation("ParamsConvQualityVals_192");
                window.SettingsTabConvQualityComboBox.Items[3] = App.GetTranslation("ParamsConvQualityVals_256");
                window.SettingsTabConvQualityComboBox.Items[4] = App.GetTranslation("ParamsConvQualityVals_320");

                window.SettingsTabLibraryGroupBox.Text = App.GetTranslation("ParamsLibFolderLabel");
                App.SetToolTip(window.SettingsTabLibraryFolderButton, App.GetTranslation("ParamsLibFolderSelectorTitle"));
                window.SettingsTabLibraryUnixHiddenFileLabel.Text = App.GetTranslation("ParamsLibHiddenFilesUnixLabel");
                window.SettingsTabLibraryUnixHiddenFileComboBox.Items[0] = App.GetTranslation("ParamsLibHiddenFilesUnix0");
                window.SettingsTabLibraryUnixHiddenFileComboBox.Items[1] = App.GetTranslation("ParamsLibHiddenFilesUnix1");
                window.SettingsTabLibraryWindowsHiddenFileLabel.Text = App.GetTranslation("ParamsLibHiddenFilesWindowsLabel");
                window.SettingsTabLibraryWindowsHiddenFileComboBox.Items[0] = App.GetTranslation("ParamsLibHiddenFilesWindows0");
                window.SettingsTabLibraryWindowsHiddenFileComboBox.Items[1] = App.GetTranslation("ParamsLibHiddenFilesWindows1");

                window.SettingsTabEqualizerGroupBox.Text = App.GetTranslation("ParamsEqualizerLabel");
                window.SettingsTabEqualizerButton.Text = App.GetTranslation("ParamsEqualizerResetLabel");

                window.SettingsTabEqualizerComboBox.Items[0] = App.GetTranslation("ParamsEqualizerPresets_Perso");
                window.SettingsTabEqualizerComboBox.Items[1] = App.GetTranslation("ParamsEqualizerPresets_Flat");
                window.SettingsTabEqualizerComboBox.Items[2] = App.GetTranslation("ParamsEqualizerPresets_Classic");
                window.SettingsTabEqualizerComboBox.Items[3] = App.GetTranslation("ParamsEqualizerPresets_Club");
                window.SettingsTabEqualizerComboBox.Items[4] = App.GetTranslation("ParamsEqualizerPresets_Dance");
                window.SettingsTabEqualizerComboBox.Items[5] = App.GetTranslation("ParamsEqualizerPresets_FullBass");
                window.SettingsTabEqualizerComboBox.Items[6] = App.GetTranslation("ParamsEqualizerPresets_FullBassTreble");
                window.SettingsTabEqualizerComboBox.Items[7] = App.GetTranslation("ParamsEqualizerPresets_FullTreble");
                window.SettingsTabEqualizerComboBox.Items[8] = App.GetTranslation("ParamsEqualizerPresets_Headphones");
                window.SettingsTabEqualizerComboBox.Items[9] = App.GetTranslation("ParamsEqualizerPresets_LargeHall");
                window.SettingsTabEqualizerComboBox.Items[10] = App.GetTranslation("ParamsEqualizerPresets_Live");
                window.SettingsTabEqualizerComboBox.Items[11] = App.GetTranslation("ParamsEqualizerPresets_Party");
                window.SettingsTabEqualizerComboBox.Items[12] = App.GetTranslation("ParamsEqualizerPresets_Pop");
                window.SettingsTabEqualizerComboBox.Items[13] = App.GetTranslation("ParamsEqualizerPresets_Reggae");
                window.SettingsTabEqualizerComboBox.Items[14] = App.GetTranslation("ParamsEqualizerPresets_Rock");
                window.SettingsTabEqualizerComboBox.Items[15] = App.GetTranslation("ParamsEqualizerPresets_Ska");
                window.SettingsTabEqualizerComboBox.Items[16] = App.GetTranslation("ParamsEqualizerPresets_Soft");
                window.SettingsTabEqualizerComboBox.Items[17] = App.GetTranslation("ParamsEqualizerPresets_SoftRock");
                window.SettingsTabEqualizerComboBox.Items[18] = App.GetTranslation("ParamsEqualizerPresets_Techno");
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace); }
        }
    }
}
