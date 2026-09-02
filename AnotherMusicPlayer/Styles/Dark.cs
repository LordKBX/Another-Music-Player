using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherMusicPlayer.Styles
{
    internal class Dark: AnotherMusicPlayer.Styles.Style
    {
        internal static Type StyleType = typeof(Dark);
        internal Type styleType = typeof(Dark);

        public static Font GlobalFont = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Point);
        public static Font GlobalFontTitle = new Font("Segoe UI", 15, FontStyle.Regular, GraphicsUnit.Point);
        public static Font GlobalFontTitleBold = new Font("Segoe UI", 15, FontStyle.Bold, GraphicsUnit.Point);
        public static Color GlobalBackColor = Color.FromArgb(255, 30, 30, 30);
        public static Color GlobalForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Color GlobalIconColor = Color.FromArgb(255, 255, 255, 255);
        public static int GlobalIconSize = 24;

        public static Color ContextMenuBackColor = Color.FromArgb(255, 30, 30, 30);
        public static Color ContextMenuForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Color ContextMenuOverBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color ContextMenuPushBackColor = Color.FromArgb(255, 90, 90, 90);

        public static Color GlobalTrackIconBackColor = Color.FromArgb(255, 0, 0, 0);

        public static Color GlobalButtonBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color GlobalButtonForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Font GlobalButtonFont = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Point);
        public static Cursor GlobalButtonCursor = Cursors.Hand;
        public static FlatStyle GlobalButtonFlatStyle = FlatStyle.Flat;
        public static Color GlobalButtonFlatAppearanceBorderColor = Color.FromArgb(255, 255, 255, 255);
        public static uint GlobalButtonFlatAppearanceBorderSize = 1;
        public static Color GlobalButtonFlatAppearanceCheckedBackColor = Color.FromArgb(255, 110, 110, 110);
        public static Color GlobalButtonFlatAppearanceMouseDownBackColor = Color.FromArgb(255, 110, 110, 110);
        public static Color GlobalButtonFlatAppearanceMouseOverBackColor = Color.FromArgb(255, 80, 80, 80);

        public static Color ValidateButtonBackColor = Color.FromArgb(255, 34, 139, 34);
        public static Color ValidateButtonForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Font ValidateButtonFont = GlobalFont;
        public static Cursor ValidateButtonCursor = Cursors.Hand;
        public static FlatStyle ValidateButtonFlatStyle = FlatStyle.Flat;
        public static Color ValidateButtonFlatAppearanceBorderColor = Color.FromArgb(255, 255, 255, 255);
        public static uint ValidateButtonFlatAppearanceBorderSize = 0;
        public static Color ValidateButtonFlatAppearanceCheckedBackColor = Color.FromArgb(255, 50, 201, 50);
        public static Color ValidateButtonFlatAppearanceMouseDownBackColor = Color.FromArgb(255, 50, 201, 50);
        public static Color ValidateButtonFlatAppearanceMouseOverBackColor = Color.FromArgb(255, 40, 160, 40);

        public static Color CancelButtonBackColor = Color.FromArgb(255, 255, 0, 0);
        public static Color CancelButtonForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Font CancelButtonFont = GlobalFont;
        public static Cursor CancelButtonCursor = Cursors.Hand;
        public static FlatStyle CancelButtonFlatStyle = FlatStyle.Flat;
        public static Color CancelButtonFlatAppearanceBorderColor = Color.FromArgb(255, 255, 255, 255);
        public static uint CancelButtonFlatAppearanceBorderSize = 0;
        public static Color CancelButtonFlatAppearanceCheckedBackColor = Color.FromArgb(255, 255, 109, 109);
        public static Color CancelButtonFlatAppearanceMouseDownBackColor = Color.FromArgb(255, 255, 109, 109);
        public static Color CancelButtonFlatAppearanceMouseOverBackColor = Color.FromArgb(255, 255, 66, 66);

        public static Cursor GripButtonCursor = Cursors.SizeNWSE;
        public static Color GripButtonBackColor = GlobalBackColor;

        public static Color GlobalTextBoxBackColor = Color.FromArgb(255, 80, 80, 80);
        public static Color GlobalTextBoxForeColor = Color.FromArgb(255, 255, 255, 255);
        public static BorderStyle GlobalTextBoxBorderStyle = BorderStyle.None;
        public static int GlobalTextBoxMinHeight = 35;
        public static Font GlobalTextBoxFont = new Font("Segoe UI", GlobalTextBoxMinHeight * 0.45F , FontStyle.Regular, GraphicsUnit.Point);

        public static Color WindowBackColor = Color.FromArgb(255, 255, 255, 255);
        public static Color WindowButtonBackColor = Color.Gray;

        public static FlatStyle ComboBoxFlatStyle = FlatStyle.Flat;
        public static Color ComboBoxBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color ComboBoxForeColor = Color.FromArgb(255, 255, 255, 255);

        public static FlatStyle CheckBoxFlatStyle = FlatStyle.Flat;
        public static Cursor CheckBoxCursor = Cursors.Hand;
        public static Color CheckBoxBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color CheckBoxForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Color CheckBoxFlatAppearanceBorderColor = Color.FromArgb(255, 255, 255, 255);
        public static uint CheckBoxFlatAppearanceBorderSize = 1;
        public static Color CheckBoxFlatAppearanceCheckedBackColor = Color.FromArgb(255, 255, 128, 0);
        public static Color CheckBoxFlatAppearanceMouseDownBackColor = Color.FromArgb(255, 255, 128, 0);
        public static Color CheckBoxFlatAppearanceMouseOverBackColor = Color.FromArgb(255, 255, 192, 128);

        public static Color TabIconColor = Color.FromArgb(255, 255, 255, 255);
        public static int TabIconSize = 24;
        public static Color TabBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color TabBackColorSelected = Color.FromArgb(255, 255, 128, 0);
        public static Color TabBackColorOver = Color.FromArgb(255, 255, 192, 128);

        public static Font GridViewFont = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point);
        public static BorderStyle GridViewBorderStyle = BorderStyle.FixedSingle;
        public static DataGridViewCellBorderStyle GridViewCellBorderStyle = DataGridViewCellBorderStyle.Single;
        public static DataGridViewHeaderBorderStyle GridViewHeaderBorderStyle = DataGridViewHeaderBorderStyle.Raised;
        public static int GridViewColumnHeaderHeight = 25;
        public static Color GridViewBackColor = Color.FromArgb(255, 160, 160, 160);

        public static Color GridViewRowBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color GridViewRowForeColor = Color.FromArgb(255, 255, 255, 255);

        public static Color GridViewRowBackColorAlt = Color.FromArgb(255, 70, 50, 70);
        public static Color GridViewRowForeColorAlt = Color.FromArgb(255, 255, 255, 255);

        public static Color GridViewRowBackColorSelection = Color.FromKnownColor(KnownColor.Highlight);
        public static Color GridViewRowForeColorSelection = Color.FromArgb(255, 255, 255, 255);

        public static Color GridViewColumnHeaderBackColor = Color.Teal;
        public static Color GridViewColumnHeaderForeColor = Color.FromArgb(255, 255, 255, 255);

        public static Color PlaybackProgressBarBackColor = Color.FromArgb(255, 30, 30, 30);
        public static Color PlaybackProgressBarForeColor = Color.FromArgb(255, 255, 255, 255);

        public static Color LibraryFolderButtonBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color LibraryFolderButtonForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Color LibraryFolderButtonBorderColor = Color.FromArgb(255, 255, 255, 255);
        public static Color LibraryFolderButtonMouseOverBackColor = Color.FromArgb(255, 90, 90, 90);
        public static Color LibraryFolderButtonMouseDownBackColor = Color.FromArgb(255, 120, 120, 120);
        public static int LibraryFolderButtonIconSize = 35;

        public static Color RattingStarEmptyColor = Color.FromArgb(255, 178, 178, 178);
        public static Color RattingStarFullColor = Color.FromArgb(255, 246, 171, 39);

        public static Color LyricsTextBoxBackColor = Color.FromArgb(125, 255, 255, 255);

        public Color GetColor(string reference, Color? defaultColor = null)
        {
            if (defaultColor == null) { defaultColor = Color.White; }
            return GetValue<Color>(reference, (Color)defaultColor);
        }

        public T GetValue<T>(string reference, T defaultValue)
        {
            Type valueType = typeof(T);
            try
            {
                object rt = styleType.InvokeMember(reference, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField, Type.DefaultBinder, null, null);
                if (rt == null || rt.GetType() != valueType) { return defaultValue; }
                return (T)rt;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in style GetValue: " + ex.Message + "\r\n" + ex.StackTrace);
                return defaultValue;
            }
        }
    }
}
