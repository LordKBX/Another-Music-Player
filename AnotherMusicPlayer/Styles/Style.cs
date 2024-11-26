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
        public static Color GlobalBackColor;
        public static Color GlobalForeColor;
        public static Color GlobalIconColor;
        public static int GlobalIconSize;

        public static Color GlobalTrackIconBackColor;

        public static Color WindowBackColor;
        public static Color WindowButtonBackColor;

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

        public Color GetColor(string reference, Color? defaultColor = null);

        public T GetValue<T>(string reference, T defaultValue);
    }
}
