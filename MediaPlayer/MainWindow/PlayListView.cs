using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

namespace MediaPlayer
{
    public partial class MainWindow : Window
    {
        private void PlayListViewInit()
        {
            PlayListViewC1.Width = 25;
            PlayListViewC2.Width = PlayListViewC3.Width = PlayListViewC4.Width = 200;
            PlayListViewC5.Width = 60;
            //PlayListViewC6.Width = 200;

            ((INotifyPropertyChanged)PlayListViewC1).PropertyChanged += (sender, e) => { if (IsCollumnWidth(e)) { PlayListViewC1.Width = 25; } };
            ((INotifyPropertyChanged)PlayListViewC2).PropertyChanged += (sender, e) => { if (IsCollumnWidth(e)) { PlayListViewC2.Width = CalcCollumnWidth(); } };
            ((INotifyPropertyChanged)PlayListViewC3).PropertyChanged += (sender, e) => { if (IsCollumnWidth(e)) { PlayListViewC3.Width = CalcCollumnWidth(); } };
            ((INotifyPropertyChanged)PlayListViewC4).PropertyChanged += (sender, e) => { if (IsCollumnWidth(e)) { PlayListViewC4.Width = CalcCollumnWidth() + 10; } };
            ((INotifyPropertyChanged)PlayListViewC5).PropertyChanged += (sender, e) => { if (IsCollumnWidth(e)) { PlayListViewC5.Width = 60; } };
            //((INotifyPropertyChanged)PlayListViewC6).PropertyChanged += (sender, e) => { if (IsCollumnWidth(e)){ PlayListViewC6.Width = CalcCollumnWidth(); } };

            PlayListView.MouseDoubleClick += Items_CurrentChanged;
            PlayListView.SizeChanged += (sender, e) => { PlayListViewC2.Width = PlayListViewC3.Width = PlayListViewC4.Width = CalcCollumnWidth(); };

            //PlayListView.SelectionChanged += PlayListView_SelectionChanged;
            //PlayListView.Loaded += (s, e) => PlayListView.ScrollIntoView(PlayListView.SelectedItem);
            PlayListView.SelectionChanged += PlayListView_SelectionChanged;
        }

        private void PlayListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListViewItem item = new ListViewItem();
            System.Diagnostics.Debug.WriteLine("e.AddedItems = " + e.AddedItems[0]);
            PlayListView.ScrollIntoView(e.AddedItems[0]);
        }

        private double CalcCollumnWidth() {
            double calc = (PlayListView.ActualWidth - 95) / 3;
            return (calc > 0)?calc:0; 
        }
        private bool IsCollumnWidth(PropertyChangedEventArgs e) { return (e.PropertyName == "ActualWidth"); }
    }

}
