using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.AxHost;

namespace CustomControl
{
    public delegate void BoolPresenterNotify(bool state);
    public delegate void BoolPresenterNotifyValue(object? sender, bool state);

    public partial class BoolPresenter : UserControl
    {
        public static string StaticStringTrue = "True";
        public static string StaticStringFalse = "False";

        private bool FreezeEvent = false;

        private string? _StringTrue = null;
        public string StringTrue
        {
            get { return _StringTrue ?? StaticStringTrue; }
            set
            {
                if (value != _StringTrue)
                {
                    _StringTrue = value;
                    checkBoxTrue.Text = _StringTrue;
                }
            }
        }

        private string? _StringFalse = null;
        public string StringFalse
        {
            get { return _StringFalse ?? StaticStringFalse; }
            set
            {
                if (value != _StringFalse)
                {
                    _StringFalse = value;
                    checkBoxFalse.Text = _StringFalse;
                }
            }
        }

        private bool _Value = false;
        public bool Value
        {
            get { return _Value; }
            set
            {
                //if (value != _Value)
                //{
                _Value = value;
                if (!FreezeEvent)
                {
                    Change?.Invoke(value);
                    ChangeWithRef?.Invoke(this, value);
                }
                checkBoxTrue.Checked = _Value;
                checkBoxFalse.Checked = !_Value;
                checkBoxTrue.Text = StringTrue;
                checkBoxFalse.Text = StringFalse;
                //}
            }
        }

        public event BoolPresenterNotify? Change;
        public event BoolPresenterNotifyValue? ChangeWithRef;

        public BoolPresenter()
        {
            InitializeComponent();
            AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(this);
            FreezeEvent = true;
            checkBoxFalse.Font = checkBoxTrue.Font = Font;
            checkBoxFalse.Text = StringFalse;
            checkBoxTrue.Text = StringTrue;
            checkBoxFalse.Checked = !_Value;
            checkBoxTrue.Checked = _Value;
            FreezeEvent = false;
        }

        public void ReloadStrings()
        {
            checkBoxFalse.Text = StringFalse;
            checkBoxTrue.Text = StringTrue;
            checkBoxFalse.Font = checkBoxTrue.Font = Font;
        }

        private void checkBoxTrue_Click(object sender, EventArgs e)
        { Value = true; checkBoxTrue.Checked = true; checkBoxFalse.Checked = false; checkBoxFalse.Text = StringFalse; checkBoxTrue.Text = StringTrue; }

        private void checkBoxFalse_Click(object sender, EventArgs e)
        { Value = false; checkBoxTrue.Checked = false; checkBoxFalse.Checked = true; checkBoxFalse.Text = StringFalse; checkBoxTrue.Text = StringTrue; }
    }
}
