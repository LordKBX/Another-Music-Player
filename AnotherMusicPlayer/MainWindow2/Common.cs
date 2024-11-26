using AnotherMusicPlayer.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2Space
{
    internal class Common
    {
        private static Type typeButton = typeof(Button);
        private static Type typeDataGridView = typeof(DataGridView);
        private static Type typeDataGridViewTextBoxColumn = typeof(DataGridViewTextBoxColumn);
        private static Type typeDataGridViewImageColumn = typeof(DataGridViewImageColumn);
        private static Type typeDataGridViewButtonColumn = typeof(DataGridViewButtonColumn);
        private static Type typeDataGridViewCheckBoxColumn = typeof(DataGridViewCheckBoxColumn);
        private static Type typePlaybackProgressBar = typeof(PlaybackProgressBar);

        public static void SetGlobalColor(Control parent, int lv = 0)
        {
            if (parent is Form)
            {
                parent.BackColor = App.style.GetColor("WindowBackColor");
                parent.Font = App.style.GetValue<Font>("GlobalFont", Dark.GlobalFont);
                foreach (Control ctl in parent.Controls) { SetGlobalColor(ctl, lv + 1); }
            }
            else {
                Type type = parent.GetType();
                if (type == typeDataGridView)
                {
                    DataGridView dgv = (DataGridView)parent;
                    dgv.RowHeadersVisible = false;
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.Font = App.style.GetValue<Font>("GridViewFont", Dark.GridViewFont);
                    dgv.BorderStyle = App.style.GetValue<BorderStyle>("GridViewBorderStyle", BorderStyle.FixedSingle);
                    dgv.CellBorderStyle = App.style.GetValue<DataGridViewCellBorderStyle>("GridViewCellBorderStyle", DataGridViewCellBorderStyle.Single);
                    dgv.ColumnHeadersBorderStyle = App.style.GetValue<DataGridViewHeaderBorderStyle>("GridViewHeaderBorderStyle", DataGridViewHeaderBorderStyle.Single);
                    dgv.RowHeadersBorderStyle = App.style.GetValue<DataGridViewHeaderBorderStyle>("GridViewHeaderBorderStyle", DataGridViewHeaderBorderStyle.Single);
                    dgv.ColumnHeadersHeight = App.style.GetValue<int>("GridViewColumnHeaderHeight", 25);

                    dgv.ColumnHeadersDefaultCellStyle.Font = App.style.GetValue<Font>("GridViewFont", Dark.GridViewFont);
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = App.style.GetColor("GridViewColumnHeaderBackColor");
                    dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = App.style.GetColor("GridViewColumnHeaderBackColor");
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = App.style.GetColor("GridViewColumnHeaderForeColor");
                    dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = App.style.GetColor("GridViewColumnHeaderForeColor");

                    dgv.RowsDefaultCellStyle.Font = dgv.AlternatingRowsDefaultCellStyle.Font = App.style.GetValue<Font>("GridViewFont", Dark.GridViewFont);

                    dgv.RowsDefaultCellStyle.BackColor = App.style.GetColor("GridViewRowBackColor");
                    dgv.RowsDefaultCellStyle.SelectionBackColor = App.style.GetColor("GridViewRowBackColorSelection");
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = App.style.GetColor("GridViewRowBackColorAlt");
                    dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = App.style.GetColor("GridViewRowBackColorSelection");
                    dgv.RowsDefaultCellStyle.ForeColor = App.style.GetColor("GridViewRowForeColor");
                    dgv.RowsDefaultCellStyle.SelectionForeColor = App.style.GetColor("GridViewRowForeColorSelection");
                    dgv.AlternatingRowsDefaultCellStyle.ForeColor = App.style.GetColor("GridViewRowForeColorAlt");
                    dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = App.style.GetColor("GridViewRowForeColorSelection");
                }
                else if (type == typeDataGridViewTextBoxColumn)
                {
                }
                else if (type == typeDataGridViewImageColumn)
                {
                }
                else if (type == typeDataGridViewButtonColumn)
                {
                }
                else if (type == typeDataGridViewCheckBoxColumn)
                {
                }
                else if (parent.GetType() == typeButton)
                {
                    if (parent.Tag != null && parent.Tag.GetType() == typeof(string) && (string)parent.Tag == "WindowButton") // Top Window Buttons
                    { parent.BackColor = App.style.GetColor("WindowButtonBackColor"); }
                    else if (parent.Width == parent.Height) // Track cover and album cover
                    { parent.BackColor = App.style.GetColor("GlobalTrackIconBackColor"); }
                    else { parent.BackColor = App.style.GetColor("GlobalBackColor"); parent.ForeColor = App.style.GetColor("GlobalForeColor"); }
                }
                else if (parent.GetType() == typePlaybackProgressBar)
                {
                     parent.BackColor = App.style.GetColor("PlaybackProgressBarBackColor");
                     parent.ForeColor = App.style.GetColor("PlaybackProgressBarForeColor");
                }
                else
                {
                    parent.BackColor = App.style.GetColor("GlobalBackColor");
                    try { parent.ForeColor = App.style.GetColor("GlobalForeColor"); } catch (Exception) { }
                    try { parent.Font = App.style.GetValue<Font>("GlobalFont", Dark.GlobalFont); } catch (Exception) { }
                    foreach (Control ctl in parent.Controls) { SetGlobalColor(ctl, lv + 1); }
                }
            }
        }

        public static void SetTabStyle(Manina.Windows.Forms.Tab tab)
        {
            tab.BackColor = App.style.GetColor("TabBackColor");
            tab.SelectedBackColor = tab.HotAndActiveTabBackColor = App.style.GetColor("TabBackColorSelected");
            tab.HotTabBackColor = App.style.GetColor("TabBackColorOver");
        }
    }
}
