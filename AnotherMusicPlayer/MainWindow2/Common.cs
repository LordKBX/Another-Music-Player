using AnotherMusicPlayer.Styles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2Space
{
    internal class Common
    {
        private static Type typeTableLayoutPanel = typeof(TableLayoutPanel);
        private static Type typeButton = typeof(Button);
        private static Type typeComboBox = typeof(ComboBox);
        private static Type typeCheckBox = typeof(CheckBox);
        private static Type typeDataGridView = typeof(DataGridView);
        private static Type typeDataGridViewTextBoxColumn = typeof(DataGridViewTextBoxColumn);
        private static Type typeDataGridViewImageColumn = typeof(DataGridViewImageColumn);
        private static Type typeDataGridViewButtonColumn = typeof(DataGridViewButtonColumn);
        private static Type typeDataGridViewCheckBoxColumn = typeof(DataGridViewCheckBoxColumn);
        private static Type typePlaybackProgressBar = typeof(PlaybackProgressBar);
        private static Type typeTextBox = typeof(TextBox);
        private static Type typeRichTextBox = typeof(RichTextBox);
        private static Type typeLabel = typeof(Label);
        private static Type typeString = typeof(string);

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
                string rawtag = ""; 
                if (parent.Tag != null && parent.Tag.GetType() == typeString) 
                { rawtag = ("" + parent.Tag); }
                List<string> Tags = rawtag.Split('|').ToList();

                if (Tags.Contains("Title"))
                { 
                    try { parent.Font = App.style.GetValue<Font>("GlobalFontTitle", Dark.GlobalFontTitle); } 
                    catch (Exception) { } 
                }
                else if (Tags.Contains("TitleBold"))
                { 
                    try { parent.Font = App.style.GetValue<Font>("GlobalFontTitleBold", Dark.GlobalFontTitleBold); } 
                    catch (Exception) { } 
                }
                else { 
                    try { parent.Font = App.style.GetValue<Font>("GlobalFont", Dark.GlobalFont); } 
                    catch (Exception) { } 
                }

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
                else if (type == typeLabel && (Tags.Contains("Bold")))
                {
                    if (Tags.Contains("Bold"))
                    {
                        parent.BackColor = App.style.GetColor("GlobalBackColor");
                        try { parent.ForeColor = App.style.GetColor("GlobalForeColor", Dark.GlobalForeColor); } catch (Exception) { }
                        ((Label)parent).Font = App.style.GetValue<Font>("GlobalFontTitleBold", Dark.GlobalFontTitleBold);
                        ((Label)parent).TextAlign = ContentAlignment.MiddleLeft;
                    }
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
                else if (type == typeButton)
                {
                    if (Tags.Contains("WindowButton")) // Top Window Buttons
                    { 
                        parent.BackColor = App.style.GetColor("WindowButtonBackColor");
                        ((Button)parent).Cursor = App.style.GetValue<Cursor>("GlobalButtonCursor", Dark.GlobalButtonCursor);
                    }
                    else if (Tags.Contains("GripButton")) // Track cover and album cover
                    {
                        parent.BackColor = App.style.GetColor("GripButtonBackColor");
                        ((Button)parent).Cursor = App.style.GetValue<Cursor>("GripButtonCursor", Cursors.SizeNWSE);
                    }
                    else if (parent.Width == parent.Height && !Tags.Contains("PlayBackButton")) // Track cover and album cover
                    { 
                        parent.BackColor = App.style.GetColor("GlobalTrackIconBackColor");
                        ((Button)parent).Cursor = Cursors.Default;
                    }
                    else
                    {
                        ((Button)parent).Cursor = App.style.GetValue<Cursor>("GlobalButtonCursor", Dark.GlobalButtonCursor);
                        parent.BackColor = App.style.GetColor("GlobalButtonBackColor");
                        parent.ForeColor = App.style.GetColor("GlobalButtonForeColor");
                        ((Button)parent).Font = App.style.GetValue<Font>("GlobalButtonFont", Dark.GlobalButtonFont);
                        ((Button)parent).FlatStyle = App.style.GetValue<FlatStyle>("GlobalButtonFlatStyle", FlatStyle.Flat);
                        ((Button)parent).FlatAppearance.BorderColor = App.style.GetColor("GlobalButtonFlatAppearanceBorderColor");
                        ((Button)parent).FlatAppearance.BorderSize = (int)App.style.GetValue<uint>("GlobalButtonFlatAppearanceBorderSize", 1);
                        ((Button)parent).FlatAppearance.CheckedBackColor = App.style.GetColor("GlobalButtonFlatAppearanceCheckedBackColor");
                        ((Button)parent).FlatAppearance.MouseDownBackColor = App.style.GetColor("GlobalButtonFlatAppearanceMouseDownBackColor");
                        ((Button)parent).FlatAppearance.MouseOverBackColor = App.style.GetColor("GlobalButtonFlatAppearanceMouseOverBackColor");
                    }
                }
                else if (type == typePlaybackProgressBar)
                {
                    parent.BackColor = App.style.GetColor("PlaybackProgressBarBackColor");
                    parent.ForeColor = App.style.GetColor("PlaybackProgressBarForeColor");
                }
                else if (type == typeTextBox)
                {
                    parent.BackColor = App.style.GetColor("GlobalTextBoxBackColor");
                    parent.ForeColor = App.style.GetColor("GlobalTextBoxForeColor");
                    ((TextBox)parent).BorderStyle = App.style.GetValue<BorderStyle>("GlobalTextBoxBorderStyle", BorderStyle.None);
                    ((TextBox)parent).AutoSize = false;
                    ((TextBox)parent).TextAlign = HorizontalAlignment.Left;
                    ((TextBox)parent).Padding = new Padding(0);
                    parent.MinimumSize = new Size(0, App.style.GetValue<int>("GlobalTextBoxMinHeight", Dark.GlobalTextBoxMinHeight));
                    parent.Font = App.style.GetValue<Font>("GlobalTextBoxFont", Dark.GlobalTextBoxFont);
                }
                else if (type == typeRichTextBox)
                {
                    parent.BackColor = App.style.GetColor("GlobalTextBoxBackColor");
                    parent.ForeColor = App.style.GetColor("GlobalTextBoxForeColor");
                    ((RichTextBox)parent).BorderStyle = App.style.GetValue<BorderStyle>("GlobalTextBoxBorderStyle", BorderStyle.None);
                }
                else if (type == typeComboBox)
                {
                    parent.BackColor = App.style.GetColor("ComboBoxBackColor");
                    parent.ForeColor = App.style.GetColor("ComboBoxForeColor");
                    ((ComboBox)parent).FlatStyle = App.style.GetValue<FlatStyle>("ComboBoxFlatStyle", FlatStyle.Flat);
                }
                else if (type == typeCheckBox)
                {
                    parent.BackColor = App.style.GetColor("CheckBoxBackColor");
                    parent.ForeColor = App.style.GetColor("CheckBoxForeColor");
                    ((CheckBox)parent).Cursor = App.style.GetValue<Cursor>("CheckBoxCursor", Dark.CheckBoxCursor);
                    ((CheckBox)parent).FlatStyle = App.style.GetValue<FlatStyle>("CheckBoxFlatStyle", FlatStyle.Flat);
                    ((CheckBox)parent).FlatAppearance.BorderColor = App.style.GetColor("CheckBoxFlatAppearanceBorderColor");
                    ((CheckBox)parent).FlatAppearance.BorderSize = (int)App.style.GetValue<uint>("CheckBoxFlatAppearanceBorderSize", 1);
                    ((CheckBox)parent).FlatAppearance.CheckedBackColor = App.style.GetColor("CheckBoxFlatAppearanceCheckedBackColor");
                    ((CheckBox)parent).FlatAppearance.MouseDownBackColor = App.style.GetColor("CheckBoxFlatAppearanceMouseDownBackColor");
                    ((CheckBox)parent).FlatAppearance.MouseOverBackColor = App.style.GetColor("CheckBoxFlatAppearanceMouseOverBackColor");
                }
                else
                {
                    parent.BackColor = App.style.GetColor("GlobalBackColor");
                    try { parent.ForeColor = App.style.GetColor("GlobalForeColor"); } catch (Exception) { }
                }

                if(parent.Controls.Count > 0) { 
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
