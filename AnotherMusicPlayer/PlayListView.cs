using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary> Initialisation variables and events configuration of PlayListView </summary>
        private void PlayListView_Init()
        {
            PlayListView.ItemsSource = new ObservableCollection<PlayListViewItemShort>();
            PlayListViewC1.Width = 25;
            PlayListViewC2.Width = PlayListViewC3.Width = PlayListViewC4.Width = 200;
            if (PlayListView.ActualWidth > 500) { PlayListViewC5.Width = 60; } else { PlayListViewC5.Width = 70; }
            //PlayListViewC6.Width = 200;

            ((INotifyPropertyChanged)PlayListViewC1).PropertyChanged += (sender, e) => { PlayListViewC1.Width = 25; };
            ((INotifyPropertyChanged)PlayListViewC5).PropertyChanged += (sender, e) => { 
                if (PlayListView.ActualWidth > 500) { PlayListViewC5.Width = 60; } else { PlayListViewC5.Width = 70; } 
            };

            ((INotifyPropertyChanged)PlayListViewC2).PropertyChanged += (sender, e) => {
                if (PlayListView.ActualWidth > 500) { if (PlayListView_IsCollumnWidth(e)) { PlayListViewC2.Width = PlayListView_CalcCollumnWidth(); } }
                else { if (PlayListView_IsCollumnWidth(e)) { PlayListViewC2.Width = PlayListView.ActualWidth - 25 - 70; } }
            };
            ((INotifyPropertyChanged)PlayListViewC3).PropertyChanged += (sender, e) => {
                if (PlayListView.ActualWidth > 500)
                {
                    if (PlayListView_IsCollumnWidth(e)) { PlayListViewC3.Width = PlayListView_CalcCollumnWidth(); }
                    PlayListViewC3.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle"];
                }
                else { PlayListViewC3.Width = 0; PlayListViewC3.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle2"]; }
            };
            ((INotifyPropertyChanged)PlayListViewC4).PropertyChanged += (sender, e) => {
                if (PlayListView.ActualWidth > 500)
                {
                    if (PlayListView_IsCollumnWidth(e)) { PlayListViewC4.Width = PlayListView_CalcCollumnWidth() + 10; }
                    PlayListViewC4.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle"];
                }
                else { PlayListViewC4.Width = 0; PlayListViewC4.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle2"]; }
            };

            PlayListView.MouseDoubleClick += PlayListView_DblClick;
            PlayListView.SizeChanged += (sender, e) => { PlayListViewC2.Width = PlayListViewC3.Width = PlayListViewC4.Width = PlayListView_CalcCollumnWidth(); };

            Label_PlayListDisplayedNBTracks.Text = "0";
            Label_PlayListNBTracks.Text = "0";
            Label_PlayListIndex.Text = "0";
        }

        /// <summary> Callback PlayListView for DoubleClick Event </summary>
        private void PlayListView_DblClick(object sender, EventArgs e) { 
            UpdatePlaylist(PlayListIndex + PlayListView.SelectedIndex, true);
        }
        /// <summary> Calculate Big Collumns width if PlayListView Width > 500(px) </summary>
        private double PlayListView_CalcCollumnWidth() { double calc = (PlayListView.ActualWidth - 95) / 3; return (calc > 0)?calc:0; }
        /// <summary> Test if PropertyChangedEventArgs contains ActualWidth Property </summary>
        private bool PlayListView_IsCollumnWidth(PropertyChangedEventArgs e) { return (e.PropertyName == "ActualWidth"); }
    }

}
