using AnotherMusicPlayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace AnotherMusicPlayer
{
    public enum DialogBoxIcons { Warning, Info, Error };
    public enum DialogBoxButtons
    {
        Ok = 0,
        OkCancel = 1,
        YesNo = 2
    };

    public partial class DialogBox : Form
    {
        public bool returnState = false;
        public DialogBox(Form owner = null)
        {
            InitializeComponent();
            AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(this);

            MessageIcon.FlatAppearance.BorderSize = 0;
            MessageIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            MessageIcon.BackColor = System.Drawing.Color.Transparent;
            MessageIcon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            MessageIcon.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            MessageIcon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            MessageIcon.Cursor = System.Windows.Forms.Cursors.Default;

            MessageBlock.BackColor = App.style.GetColor("GlobalBackColor");
            MessageBlock.ForeColor = App.style.GetColor("GlobalForeColor");
            MessageBlock.Font = App.style.GetValue<Font>("GlobalFontTitle", AnotherMusicPlayer.Styles.Dark.GlobalFont);

            BtnOK.Text = App.GetTranslation("DialogBoxBtnOk");
            BtnCancel.Text = App.GetTranslation("DialogBoxBtnCancel");
            BtnYes.Text = App.GetTranslation("DialogBoxBtnYes");
            BtnNo.Text = App.GetTranslation("DialogBoxBtnNo");

            Owner = owner ?? App.win1;
            BtnOK.Click += BtnOK_Click;
            BtnCancel.Click += BtnCancel_Click;
            BtnYes.Click += BtnOK_Click;
            BtnNo.Click += BtnCancel_Click;

            this.Load += DialogBox_Loaded;
        }
        private void DialogBox_Loaded(object sender, EventArgs e)
        {
            Left = Owner.Left + ((Owner.Width - Width) / 2);
            Top = Owner.Top + ((Owner.Height - Height) / 2);
        }

        private void BtnOK_Click(object sender, EventArgs e) { returnState = true; Close(); }
        private void BtnCancel_Click(object sender, EventArgs e) { returnState = false; Close(); }

        public static bool ShowDialog(string title, string message, DialogBoxButtons buttons = DialogBoxButtons.Ok, DialogBoxIcons icon = DialogBoxIcons.Info, Form owner = null)
        {
            DialogBox dialog = new DialogBox(owner) { StartPosition = (owner == null) ? FormStartPosition.CenterScreen: FormStartPosition.CenterParent };

            dialog.returnState = false;
            dialog.TitleLabel.Text = title;
            dialog.MessageBlock.Text = message;
            if (icon == DialogBoxIcons.Warning) { dialog.MessageIcon.BackgroundImage = Properties.Resources.dialog_warning; }
            else if (icon == DialogBoxIcons.Error) { dialog.MessageIcon.BackgroundImage = Properties.Resources.dialog_error; }
            else if (icon == DialogBoxIcons.Info) { dialog.MessageIcon.BackgroundImage = Properties.Resources.dialog_info; }
            else { dialog.MessageIcon.BackgroundImage = Properties.Resources.dialog_info; }

            if (buttons == DialogBoxButtons.Ok)
            { dialog.BtnCancel.Visible = false; dialog.BtnYes.Visible = false; dialog.BtnNo.Visible = false; }
            else if (buttons == DialogBoxButtons.OkCancel)
            { dialog.BtnYes.Visible = false; dialog.BtnNo.Visible = false; }
            else if (buttons == DialogBoxButtons.YesNo)
            { dialog.BtnOK.Visible = false; dialog.BtnCancel.Visible = false; }

            dialog.ShowDialog();
            return dialog.returnState;
        }
    }
}
