using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;

namespace AnotherMusicPlayer
{
    public enum DialogBoxIcons
    {
        Warning = PackIconKind.Warning,
        Info = PackIconKind.AlertCircle,
        Error = PackIconKind.CloseOctagon
    };
    public enum DialogBoxButtons
    {
        Ok = 0,
        OkCancel = 1,
        YesNo = 2
    };

    /// <summary>
    /// Logique d'interaction pour DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window
    {

        public bool returnState;
        public DialogBox(MainWindow owner)
        {
            Owner = owner;
            Resources.Clear();
            Resources.MergedDictionaries.Clear();//Ensure a clean MergedDictionaries
            InitializeComponent();

            BtnOk.Click += BtnOk_Click;
            BtnCancel.Click += BtnCancel_Click;
            BtnYes.Click += BtnOk_Click;
            BtnNo.Click += BtnCancel_Click;

            Left = Owner.Left + ((Owner.Width - Width) / 2);
            Top = Owner.Top + ((Owner.Height - Height) / 2);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            returnState = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            returnState = false;
            Close();
        }

        public static bool ShowDialog(MainWindow owner, string title, string message, DialogBoxButtons buttons = DialogBoxButtons.Ok, DialogBoxIcons icon = DialogBoxIcons.Warning)
        {
            DialogBox dialog = new DialogBox(owner);
            string style = MainWindow.BaseDir + "Styles" + MainWindow.SeparatorChar + Settings.StyleName + ".xaml";
            if (System.IO.File.Exists(style))
                dialog.Resources.MergedDictionaries.Add(new System.Windows.ResourceDictionary { Source = new Uri(style, UriKind.Absolute) });//Load style file
            string lang = MainWindow.BaseDir + "Traductions" + MainWindow.SeparatorChar + Settings.Lang.Split('-')[0] + ".xaml";
            if (System.IO.File.Exists(lang))
                dialog.Resources.MergedDictionaries.Add(new System.Windows.ResourceDictionary { Source = new Uri(lang, UriKind.Absolute) });//Load style file
            dialog.Style = dialog.FindResource("CustomWindowStyle") as Style;

            dialog.returnState = false;
            dialog.TitleBlock.Text = title;
            dialog.MessageBlock.Text = message;
            dialog.MessageIcon.Kind = (PackIconKind)icon;
            if (icon == DialogBoxIcons.Error) { dialog.MessageIcon.Foreground = dialog.FindResource("DailogBoxS.IconColor.Error") as SolidColorBrush; }
            if (icon == DialogBoxIcons.Info) { dialog.MessageIcon.Foreground = dialog.FindResource("DailogBoxS.IconColor.Info") as SolidColorBrush; }
            if (icon == DialogBoxIcons.Warning) { dialog.MessageIcon.Foreground = dialog.FindResource("DailogBoxS.IconColor.Warning") as SolidColorBrush; }

            if (buttons == DialogBoxButtons.Ok)
            {
                dialog.BtnCancel.Visibility = Visibility.Collapsed;
                dialog.BtnYes.Visibility = Visibility.Collapsed;
                dialog.BtnNo.Visibility = Visibility.Collapsed;
            }
            else if (buttons == DialogBoxButtons.OkCancel)
            {
                dialog.BtnYes.Visibility = Visibility.Collapsed;
                dialog.BtnNo.Visibility = Visibility.Collapsed;
            }
            else if (buttons == DialogBoxButtons.YesNo)
            {
                dialog.BtnOk.Visibility = Visibility.Collapsed;
                dialog.BtnCancel.Visibility = Visibility.Collapsed;
            }

            dialog.ShowDialog();
            return dialog.returnState;
        }
    }
}
