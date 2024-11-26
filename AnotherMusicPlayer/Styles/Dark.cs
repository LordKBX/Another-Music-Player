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
        public static Font GlobalFont = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point);
        public static Color GlobalBackColor = Color.FromArgb(255, 30, 30, 30);
        public static Color GlobalForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Color GlobalIconColor = Color.FromArgb(255, 255, 255, 255);
        public static int GlobalIconSize = 24;

        public static Color ContextMenuBackColor = Color.FromArgb(255, 30, 30, 30);
        public static Color ContextMenuForeColor = Color.FromArgb(255, 255, 255, 255);

        public static Color GlobalTrackIconBackColor = Color.FromArgb(255, 0, 0, 0);

        public static Color WindowBackColor = Color.FromArgb(255, 255, 255, 255);
        public static Color WindowButtonBackColor = Color.Gray;

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
        public static Color GridViewRowBackColor = Color.FromArgb(255, 50, 50, 50);
        public static Color GridViewRowBackColorAlt = Color.FromArgb(255, 70, 50, 70);
        public static Color GridViewRowBackColorSelection = Color.FromKnownColor(KnownColor.Highlight);
        public static Color GridViewRowForeColor = Color.FromArgb(255, 255, 255, 255);
        public static Color GridViewRowForeColorAlt = Color.FromArgb(255, 255, 255, 255);
        public static Color GridViewRowForeColorSelection = Color.FromArgb(255, 255, 255, 255);
        public static Color GridViewColumnHeaderBackColor = Color.Teal;
        public static Color GridViewColumnHeaderForeColor = Color.FromArgb(255, 255, 255, 255);

        public static Color PlaybackProgressBarBackColor = Color.FromArgb(255, 30, 30, 30);
        public static Color PlaybackProgressBarForeColor = Color.FromArgb(255, 255, 255, 255);

        private Type styleType = typeof(Dark);

        public Color GetColor(string reference, Color? defaultColor = null)
        {
            if (defaultColor == null) { defaultColor = Color.White; }
            return GetValue<Color>(reference, (Color)defaultColor);
        }

        public T GetValue<T>(string reference, T defaultValue)
        {
            Type valueType = typeof(T);

            object rt = styleType.InvokeMember(reference, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField, Type.DefaultBinder, null, null);
            if (rt == null || rt.GetType() != valueType) { return defaultValue; }
            return (T)rt;
        }
    }
}
