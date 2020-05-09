using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;

using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Security.Cryptography;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        private void PlayListViewInit()
        {
            PlayListView.ItemsSource = PlayListDisplayed;
            PlayListViewC1.Width = 25;
            PlayListViewC2.Width = PlayListViewC3.Width = PlayListViewC4.Width = 200;
            if (PlayListView.ActualWidth > 500) { PlayListViewC5.Width = 60; } else { PlayListViewC5.Width = 70; }
            //PlayListViewC6.Width = 200;

            ((INotifyPropertyChanged)PlayListViewC1).PropertyChanged += (sender, e) => { PlayListViewC1.Width = 25; };
            ((INotifyPropertyChanged)PlayListViewC5).PropertyChanged += (sender, e) => { 
                if (PlayListView.ActualWidth > 500)
                {
                    PlayListViewC5.Width = 60;
                }
                else { PlayListViewC5.Width = 70; }
            };

            ((INotifyPropertyChanged)PlayListViewC2).PropertyChanged += (sender, e) => {
                if (PlayListView.ActualWidth > 500)
                {
                    if (IsCollumnWidth(e)) { PlayListViewC2.Width = CalcCollumnWidth(); }
                }
                else { if (IsCollumnWidth(e)) { PlayListViewC2.Width = PlayListView.ActualWidth - 25 - 70; } }
            };
            ((INotifyPropertyChanged)PlayListViewC3).PropertyChanged += (sender, e) => {
                if (PlayListView.ActualWidth > 500)
                {
                    if (IsCollumnWidth(e)) { PlayListViewC3.Width = CalcCollumnWidth(); }
                    PlayListViewC3.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle"];
                }
                else { PlayListViewC3.Width = 0; PlayListViewC3.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle2"]; }
            };
            ((INotifyPropertyChanged)PlayListViewC4).PropertyChanged += (sender, e) => {
                if (PlayListView.ActualWidth > 500)
                {
                    if (IsCollumnWidth(e)) { PlayListViewC4.Width = CalcCollumnWidth() + 10; }
                    PlayListViewC4.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle"];
                }
                else { PlayListViewC4.Width = 0; PlayListViewC4.HeaderContainerStyle = (Style)Resources.MergedDictionaries[0]["ListViewHeaderStyle2"]; }
            };

            PlayListView.MouseDoubleClick += Items_CurrentChanged;
            PlayListView.SizeChanged += (sender, e) => { PlayListViewC2.Width = PlayListViewC3.Width = PlayListViewC4.Width = CalcCollumnWidth(); };

            PlayListView.SelectionChanged += PlayListView_SelectionChanged;

            Label_PlayListDisplayedNBTracks.Text = "0";
            Label_PlayListNBTracks.Text = "0";
            Label_PlayListIndex.Text = "0";
        }

        private void PlayListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListViewItem item = new ListViewItem();
            try
            {
                //System.Diagnostics.Debug.WriteLine("e.AddedItems = " + e.AddedItems[0]);
                //PlayListView.ScrollIntoView(e.AddedItems[0]);
            }
            catch { }
        }

        private double CalcCollumnWidth() {
            double calc = (PlayListView.ActualWidth - 95) / 3;
            return (calc > 0)?calc:0; 
        }
        private bool IsCollumnWidth(PropertyChangedEventArgs e) { return (e.PropertyName == "ActualWidth"); }
    }

}
