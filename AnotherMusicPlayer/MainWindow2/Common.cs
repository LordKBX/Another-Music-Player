using AnotherMusicPlayer.Components;
using AnotherMusicPlayer.Styles;
using Svg;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;
using Color = System.Drawing.Color;

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
        private static Type typeMaskedTextBox = typeof(MaskedTextBox);
        private static Type typeRichTextBox = typeof(RichTextBox);
        private static Type typeNumericUpDown = typeof(NumericUpDown);
        private static Type typeLabel = typeof(Label);
        private static Type typeString = typeof(string);
        private static Type typeRating2 = typeof(Rating2);
        private static Type typeTrackButton = typeof(TrackButton);

        public static void SetGlobalColor(Control parent, int lv = 0)
        {
            if (parent == null) { return; }
            bool skipSub = false;
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
                    dgv.AllowUserToResizeRows = false;
                    dgv.BackgroundColor = App.style.GetColor("GridViewBackColor");
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
                else if (type == typeLabel)
                {
                    ((Label)parent).BackColor = App.style.GetColor("GlobalBackColor", Styles.Dark.GlobalBackColor);
                    ((Label)parent).ForeColor = App.style.GetColor("GlobalForeColor", Styles.Dark.GlobalForeColor);
                    ((Label)parent).Font = App.style.GetValue<Font>("GlobalFont", Styles.Dark.GlobalFont);

                    if (Tags.Contains("Bold"))
                    {
                        ((Label)parent).Font = App.style.GetValue<Font>("GlobalFontTitleBold", Dark.GlobalFontTitleBold);
                        ((Label)parent).TextAlign = ContentAlignment.MiddleLeft;
                    }
                    else if (parent.Name == "TitleLabel")
                    {
                        ((Label)parent).Font = App.style.GetValue<Font>("GlobalFontTitle", Styles.Dark.GlobalFontTitle);
                        ((Label)parent).TextAlign = ContentAlignment.MiddleLeft;
                    }
                    else if (parent.Name == "LyricsTextBox")
                    {
                        ((Label)parent).BackColor = App.style.GetColor("LyricsTextBoxBackColor", Styles.Dark.LyricsTextBoxBackColor);
                        ((Label)parent).Font = App.style.GetValue<Font>("GlobalFontTitle", Styles.Dark.GlobalFontTitle);
                        ((Label)parent).TextAlign = ContentAlignment.MiddleCenter;

                        ((Label)parent).Image = Icons.FromIconKind(IconKind.MicrophoneVariant, 32, new SolidColorBrush(Icons.ToMediaColor(App.style.GetColor("GlobalForeColor", Styles.Dark.GlobalForeColor))));
                        ((Label)parent).ImageAlign = ContentAlignment.TopLeft;

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

                        ((Button)parent).FlatStyle = App.style.GetValue<FlatStyle>("GlobalButtonFlatStyle", FlatStyle.Flat);
                        ((Button)parent).FlatAppearance.BorderColor = App.style.GetColor("GlobalButtonFlatAppearanceBorderColor");
                        ((Button)parent).FlatAppearance.BorderSize = (int)App.style.GetValue<uint>("GlobalButtonFlatAppearanceBorderSize", 1);
                        ((Button)parent).FlatAppearance.CheckedBackColor = App.style.GetColor("GlobalButtonFlatAppearanceCheckedBackColor");
                        ((Button)parent).FlatAppearance.MouseDownBackColor = App.style.GetColor("GlobalButtonFlatAppearanceMouseDownBackColor");
                        ((Button)parent).FlatAppearance.MouseOverBackColor = App.style.GetColor("GlobalButtonFlatAppearanceMouseOverBackColor");
                        ((Button)parent).Cursor = Cursors.Hand;

                        string name = ((Button)parent).Name;
                        System.Windows.Media.Color color = Icons.ToMediaColor(App.style.GetColor("GlobalForeColor", System.Drawing.Color.White));

                        if (name == "CloseButton") { ((Button)parent).BackgroundImage = Icons.FromIconKind(IconKind.WindowClose, 32, new SolidColorBrush(color)); }
                        else if (name == "MaximizeButton") { ((Button)parent).BackgroundImage = Icons.FromIconKind(IconKind.WindowMaximize, 32, new SolidColorBrush(color)); }
                        else if (name == "MinimizeButton") { ((Button)parent).BackgroundImage = Icons.FromIconKind(IconKind.WindowMinimize, 32, new SolidColorBrush(color)); }
                        ((Button)parent).BackgroundImageLayout = ImageLayout.Center;
                    }
                    else if (parent.Name == "WindowIconButton")
                    {
                        parent.BackColor = App.style.GetColor("WindowButtonBackColor");

                        ((Button)parent).FlatStyle = App.style.GetValue<FlatStyle>("GlobalButtonFlatStyle", FlatStyle.Flat);
                        ((Button)parent).FlatAppearance.BorderColor = App.style.GetColor("WindowButtonBackColor");
                        ((Button)parent).FlatAppearance.BorderSize = (int)App.style.GetValue<uint>("GlobalButtonFlatAppearanceBorderSize", 1);
                        ((Button)parent).FlatAppearance.CheckedBackColor = App.style.GetColor("WindowButtonBackColor");
                        ((Button)parent).FlatAppearance.MouseDownBackColor = App.style.GetColor("WindowButtonBackColor");
                        ((Button)parent).FlatAppearance.MouseOverBackColor = App.style.GetColor("WindowButtonBackColor");


                        SvgDocument mySvg = SvgDocument.FromSvg<SvgDocument>(Properties.Resources.album_svg);
                        mySvg.Children[0].Fill = new SvgColourServer(App.style.GetColor("GlobalForeColor", System.Drawing.Color.White));
                        mySvg.Children[1].Fill = new SvgColourServer(App.style.GetColor("GlobalForeColor", System.Drawing.Color.White));
                        Bitmap myBmp = mySvg.Draw();
                        Bitmap myBmp2 = new Bitmap(myBmp, new Size(32, 32));

                        ((Button)parent).BackgroundImage = myBmp2;
                    }
                    else if (Tags.Contains("GripButton") || parent.Name == "GripButton") // Track cover and album cover
                    {
                        parent.BackColor = App.style.GetColor("GripButtonBackColor");
                        ((Button)parent).Cursor = App.style.GetValue<Cursor>("GripButtonCursor", Cursors.SizeNWSE);
                        ((Button)parent).FlatStyle = App.style.GetValue<FlatStyle>("GlobalButtonFlatStyle", FlatStyle.Flat);
                        ((Button)parent).FlatAppearance.BorderSize = (int)App.style.GetValue<uint>("GlobalButtonFlatAppearanceBorderSize", 1);
                        ((Button)parent).FlatAppearance.BorderColor = App.style.GetColor("GripButtonBackColor");
                        ((Button)parent).FlatAppearance.CheckedBackColor = App.style.GetColor("GripButtonBackColor");
                        ((Button)parent).FlatAppearance.MouseDownBackColor = App.style.GetColor("GripButtonBackColor");
                        ((Button)parent).FlatAppearance.MouseOverBackColor = App.style.GetColor("GripButtonBackColor");

                        System.Windows.Media.Color color = Icons.ToMediaColor(App.style.GetColor("GlobalForeColor", System.Drawing.Color.White));

                        ((Button)parent).BackgroundImage = Icons.FromIconKind(IconKind.ResizeBottomRight, 16, new SolidColorBrush(color));

                        skipSub = true;
                    }
                    else if (parent.Width == parent.Height && !Tags.Contains("PlayBackButton")) // Track cover and album cover
                    { 
                        //parent.BackColor = App.style.GetColor("GlobalTrackIconBackColor");
                        parent.BackColor = App.style.GetColor("GlobalButtonBackColor");
                        parent.ForeColor = App.style.GetColor("GlobalButtonForeColor");
                        ((Button)parent).FlatStyle = App.style.GetValue<FlatStyle>("GlobalButtonFlatStyle", FlatStyle.Flat);
                        ((Button)parent).FlatAppearance.BorderColor = App.style.GetColor("GlobalButtonFlatAppearanceBorderColor");
                        ((Button)parent).FlatAppearance.BorderSize = (int)App.style.GetValue<uint>("GlobalButtonFlatAppearanceBorderSize", 1);
                        ((Button)parent).FlatAppearance.CheckedBackColor = App.style.GetColor("GlobalButtonFlatAppearanceCheckedBackColor");
                        ((Button)parent).FlatAppearance.MouseDownBackColor = App.style.GetColor("GlobalButtonFlatAppearanceMouseDownBackColor");
                        ((Button)parent).FlatAppearance.MouseOverBackColor = App.style.GetColor("GlobalButtonFlatAppearanceMouseOverBackColor");
                        ((Button)parent).Cursor = Cursors.Default;
                    }
                    else if (parent.Width == parent.Height && (Tags.Contains("ValidateButton") || parent.Name == "ValidateButton"))
                    { 
                        parent.BackColor = App.style.GetColor("ValidateButtonBackColor");
                        parent.ForeColor = App.style.GetColor("ValidateButtonForeColor");
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
                    skipSub = true;
                }
                else if (type == typeTextBox)
                {
                    parent.BackColor = App.style.GetColor("GlobalTextBoxBackColor");
                    parent.ForeColor = App.style.GetColor("GlobalTextBoxForeColor");
                    parent.Font = App.style.GetValue<Font>("GlobalFont", AnotherMusicPlayer.Styles.Dark.GlobalFont);
                    ((TextBox)parent).BorderStyle = App.style.GetValue<BorderStyle>("GlobalTextBoxBorderStyle", BorderStyle.None);
                    ((TextBox)parent).AutoSize = false;
                    parent.MinimumSize = new Size(0, App.style.GetValue<int>("GlobalTextBoxMinHeight", Dark.GlobalTextBoxMinHeight));
                    parent.Font = App.style.GetValue<Font>("GlobalTextBoxFont", Dark.GlobalTextBoxFont);
                }
                else if (type == typeMaskedTextBox)
                {
                    parent.BackColor = App.style.GetColor("GlobalTextBoxBackColor");
                    parent.ForeColor = App.style.GetColor("GlobalTextBoxForeColor");
                    parent.Font = App.style.GetValue<Font>("GlobalFont", AnotherMusicPlayer.Styles.Dark.GlobalFont);
                    ((MaskedTextBox)parent).BorderStyle = App.style.GetValue<BorderStyle>("GlobalTextBoxBorderStyle", BorderStyle.None);
                    ((MaskedTextBox)parent).AutoSize = false;
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
                else if (type == typeTrackButton)
                {
                    int index = parent.Parent.Controls.GetChildIndex(parent);
                    if (index == 0 || index % 2 == 0) { parent.BackColor = App.style.GetColor("GridViewRowBackColor"); parent.ForeColor = App.style.GetColor("GridViewRowForeColor"); }
                    else { parent.BackColor = App.style.GetColor("GridViewRowBackColorAlt"); parent.ForeColor = App.style.GetColor("GridViewRowForeColorAlt"); }
                    skipSub = true;
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
                else if (type == typeRating2)
                {
                    parent.BackColor = Color.Transparent;
                    return;
                }
                else if (type == typeof(PlayBackContextMenu) || type == typeof(PlayListsNodeContextMenu) || type == typeof(LibraryContextMenu))
                {
                    Debug.WriteLine("<<<< PlayBackContextMenu >>>>");
                    parent.BackColor = App.style.GetColor("ContextMenuBackColor");
                    parent.ForeColor = App.style.GetColor("ContextMenuForeColor");
                    if (type == typeof(PlayBackContextMenu) && App.win1 != null) { App.win1.playBackContextMenu = App.win1.MakePlayBackContextMenu(); }
                    if (type == typeof(PlayListsNodeContextMenu)) { ((PlayListsNodeContextMenu)parent).Update(); }
                    if (type == typeof(LibraryContextMenu)) { ((LibraryContextMenu)parent).Update(); }
                    
                    return;
                }
                else
                {
                    parent.BackColor = App.style.GetColor("GlobalBackColor");
                    try { parent.ForeColor = App.style.GetColor("GlobalForeColor"); } catch (Exception) { }
                }

                if(parent.Controls.Count > 0 && !skipSub) { 
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

    public class CMRenderer : ToolStripProfessionalRenderer
    {
        private CMColorTable? MColors = null; 

        public CMRenderer() { MColors = new CMColorTable(); }
        public CMRenderer(CMColorTable table) { MColors = table; }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color c = MColors.ToolStripDropDownBackground;
            if (e.Item.Pressed) { c = MColors.MenuItemPressedGradientBegin; }
            else if (e.Item.Selected) { c = MColors.MenuItemSelected; }
            
            using (SolidBrush brush = new SolidBrush(c))
                e.Graphics.FillRectangle(brush, rc);
        }
    }


    public class CMColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground
        {
            get
            {
                return App.style.GetColor("ContextMenuBackColor");
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return App.style.GetColor("ContextMenuBackColor");
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return App.style.GetColor("ContextMenuBackColor");
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                return App.style.GetColor("ContextMenuBackColor");
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return App.style.GetColor("ContextMenuBackColor");
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return App.style.GetColor("ContextMenuBackColor");
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return App.style.GetColor("ContextMenuOverBackColor");
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return App.style.GetColor("ContextMenuOverBackColor");
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return App.style.GetColor("ContextMenuOverBackColor");
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return App.style.GetColor("ContextMenuOverBackColor");
            }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return App.style.GetColor("ContextMenuOverBackColor");
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return App.style.GetColor("ContextMenuPushBackColor");
            }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return App.style.GetColor("ContextMenuPushBackColor");
            }
        }
    }

}
