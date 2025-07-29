using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherMusicPlayer.Styles
{
    public interface Style
    {
        public static Font GlobalFont;
        public static Font GlobalFontTitle;
        public static Color GlobalBackColor;
        public static Color GlobalForeColor;
        public static Color GlobalIconColor;
        public static int GlobalIconSize;

        public static Color GlobalTrackIconBackColor;

        public static Color GlobalButtonBackColor;
        public static Color GlobalButtonForeColor;
        public static Font GlobalButtonFont;
        public static Cursor GlobalButtonCursor;
        public static FlatStyle GlobalButtonFlatStyle;
        public static Color GlobalButtonFlatAppearanceBorderColor;
        public static uint GlobalButtonFlatAppearanceBorderSize;
        public static Color GlobalButtonFlatAppearanceCheckedBackColor;
        public static Color GlobalButtonFlatAppearanceMouseDownBackColor;
        public static Color GlobalButtonFlatAppearanceMouseOverBackColor;

        public static Cursor GripButtonCursor;
        public static Color GripButtonBackColor;

        public static Color GlobalTextBoxBackColor;
        public static Color GlobalTextBoxForeColor;
        public static BorderStyle GlobalTextBoxBorderStyle;
        public static int GlobalTextBoxMinHeight;
        public static Font GlobalTextBoxFont;

        public static Color WindowBackColor;
        public static Color WindowButtonBackColor;

        public static FlatStyle ComboBoxFlatStyle;
        public static Color ComboBoxBackColor;
        public static Color ComboBoxForeColor;

        public static FlatStyle CheckBoxFlatStyle;
        public static Cursor CheckBoxCursor;
        public static Color CheckBoxBackColor;
        public static Color CheckBoxForeColor;
        public static Color CheckBoxFlatAppearanceBorderColor;
        public static uint CheckBoxFlatAppearanceBorderSize;
        public static Color CheckBoxFlatAppearanceCheckedBackColor;
        public static Color CheckBoxFlatAppearanceMouseDownBackColor;
        public static Color CheckBoxFlatAppearanceMouseOverBackColor;

        public static Color TabIconColor;
        public static int TabIconSize;
        public static Color TabBackColor;
        public static Color TabBackColorSelected;
        public static Color TabBackColorOver;

        public static Font GridViewFont;
        public static BorderStyle GridViewBorderStyle;
        public static DataGridViewCellBorderStyle GridViewCellBorderStyle;
        public static DataGridViewHeaderBorderStyle GridViewHeaderBorderStyle;
        public static int GridViewColumnHeaderHeight;
        public static Color GridViewRowBackColor;
        public static Color GridViewRowBackColorAlt;
        public static Color GridViewRowBackColorSelection;
        public static Color GridViewRowForeColor;
        public static Color GridViewRowForeColorAlt;
        public static Color GridViewRowForeColorSelection;
        public static Color GridViewColumnHeaderBackColor;
        public static Color GridViewColumnHeaderForeColor;

        public static Color PlaybackProgressBarBackColor;
        public static Color PlaybackProgressBarForeColor;

        public static Color LibraryFolderButtonBackColor;
        public static Color LibraryFolderButtonForeColor;
        public static Color LibraryFolderButtonBorderColor;
        public static Color LibraryFolderButtonMouseOverBackColor;
        public static Color LibraryFolderButtonMouseDownBackColor;
        public static int LibraryFolderButtonIconSize;

        public Color GetColor(string reference, Color? defaultColor = null);

        public T GetValue<T>(string reference, T defaultValue);
    }
}
