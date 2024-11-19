using System.Drawing;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2
{
    partial class MainWindow2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow2));
            GlobalTableLayoutPanel = new TableLayoutPanel();
            MainWIndowHead = new TableLayoutPanel();
            TitleLabel = new Label();
            MinimizeButton = new Button();
            MaximizeButton = new Button();
            CloseButton = new Button();
            button1 = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            DisplayPlaybackPosition = new Label();
            panel1 = new Panel();
            GripButton = new Button();
            DisplayPlaybackSize = new Label();
            playbackProgressBar = new PlaybackProgressBar();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            BtnOpen = new Button();
            BtnPrevious = new Button();
            BtnPlayPause = new Button();
            BtnNext = new Button();
            BtnRepeat = new Button();
            BtnShuffle = new Button();
            BtnClearList = new Button();
            TabControler = new Manina.Windows.Forms.TabControl();
            PlaybackTab = new Manina.Windows.Forms.Tab();
            PlaybackTabMainTableLayoutPanel = new TableLayoutPanel();
            PlaybackTabLeftTableLayoutPanel = new TableLayoutPanel();
            PlaybackTabLeftBottomPanel = new Panel();
            PlaybackTabLeftBottomFlowLayoutPanel = new FlowLayoutPanel();
            PlaybackTabTitleLabelInfo = new Label();
            PlaybackTabTitleLabelValue = new Label();
            PlaybackTabAlbumLabelInfo = new Label();
            PlaybackTabAlbumLabelValue = new Label();
            PlaybackTabArtistsLabelInfo = new Label();
            PlaybackTabArtistsLabelValue = new Label();
            PlaybackTabDurationLabelInfo = new Label();
            PlaybackTabDurationLabelValue = new Label();
            PlaybackTabLyricsButton = new Button();
            PlaybackTabRatting = new Ratting2();
            FileCover = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            PlaybackTabDataGridView = new DataGridView();
            SelectedColumn = new DataGridViewTextBoxColumn();
            NameColumn = new DataGridViewTextBoxColumn();
            DurationColumn = new DataGridViewTextBoxColumn();
            ArtistsColumn = new DataGridViewTextBoxColumn();
            AlbumColumn = new DataGridViewTextBoxColumn();
            PlaybackPositionLabel = new Label();
            LibraryTab = new Manina.Windows.Forms.Tab();
            LibraryTabTableLayoutPanel = new TableLayoutPanel();
            PlayListsTab = new Manina.Windows.Forms.Tab();
            TableLayoutPanel = new TableLayoutPanel();
            SettingsTab = new Manina.Windows.Forms.Tab();
            panel2 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            SettingsTabLangGroupBox = new GroupBox();
            SettingsTabLangComboBox = new ComboBox();
            SettingsTabStyleGroupBox = new GroupBox();
            SettingsTabStyleComboBox = new ComboBox();
            SettingsTabAutoPlayGroupBox = new GroupBox();
            SettingsTabAutoPlayComboBox = new ComboBox();
            SettingsTabAlwaysOnTopGroupBox = new GroupBox();
            SettingsTabAlwaysOnTopComboBox = new ComboBox();
            SettingsTabAutoCloseLyricsGroupBox = new GroupBox();
            SettingsTabAutoCloseLyricsComboBox = new ComboBox();
            SettingsTabConvGroupBox = new GroupBox();
            SettingsTabConvTableLayoutPanel = new TableLayoutPanel();
            SettingsTabConvModeLabel = new Label();
            SettingsTabConvModeComboBox = new ComboBox();
            SettingsTabConvQualityLabel = new Label();
            SettingsTabConvQualityComboBox = new ComboBox();
            SettingsTabLibraryGroupBox = new GroupBox();
            SettingsTabLibraryTableLayoutPanel = new TableLayoutPanel();
            SettingsTabLibraryFolderTextBox = new TextBox();
            SettingsTabLibraryFolderButton = new Button();
            SettingsTabLibraryUnixHiddenFileLabel = new Label();
            SettingsTabLibraryUnixHiddenFileComboBox = new ComboBox();
            SettingsTabLibraryWindowsHiddenFileLabel = new Label();
            SettingsTabLibraryWindowsHiddenFileComboBox = new ComboBox();
            SettingsTabEqualizerGroupBox = new GroupBox();
            SettingsTabEqualizerTableLayoutPanel = new TableLayoutPanel();
            SettingsTabEqualizerComboBox = new ComboBox();
            SettingsTabEqualizerButton = new Button();
            SettingsTabEqualizerTableLayoutPanel2 = new TableLayoutPanel();
            SettingsTabEqualizerTrackBar01 = new TrackBar();
            SettingsTabEqualizerTrackBar02 = new TrackBar();
            SettingsTabEqualizerTrackBar03 = new TrackBar();
            SettingsTabEqualizerTrackBar04 = new TrackBar();
            SettingsTabEqualizerTrackBar05 = new TrackBar();
            SettingsTabEqualizerTrackBar06 = new TrackBar();
            SettingsTabEqualizerTrackBar07 = new TrackBar();
            SettingsTabEqualizerTrackBar08 = new TrackBar();
            SettingsTabEqualizerTrackBar09 = new TrackBar();
            SettingsTabEqualizerTrackBar10 = new TrackBar();
            SettingsTabEqualizerLabel01 = new Label();
            SettingsTabEqualizerLabel02 = new Label();
            SettingsTabEqualizerLabel03 = new Label();
            SettingsTabEqualizerLabel04 = new Label();
            SettingsTabEqualizerLabel05 = new Label();
            SettingsTabEqualizerLabel06 = new Label();
            SettingsTabEqualizerLabel07 = new Label();
            SettingsTabEqualizerLabel08 = new Label();
            SettingsTabEqualizerLabel09 = new Label();
            SettingsTabEqualizerLabel10 = new Label();
            LibibraryNavigationPathContener = new FlowLayoutPanel();
            LibraryFiltersGrid = new TableLayoutPanel();
            LibraryFiltersModeLabel = new Label();
            LibraryFiltersMode = new ComboBox();
            LibraryFiltersGenreList = new ComboBox();
            LibraryFiltersGenreSearchBox = new TextBox();
            LibraryFiltersSearchBox = new TextBox();
            splitContainer1 = new SplitContainer();
            LibibraryNavigationContent = new FlowLayoutPanel();
            LibibrarySearchContent = new FlowLayoutPanel();
            GlobalTableLayoutPanel.SuspendLayout();
            MainWIndowHead.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            TabControler.SuspendLayout();
            PlaybackTab.SuspendLayout();
            PlaybackTabMainTableLayoutPanel.SuspendLayout();
            PlaybackTabLeftTableLayoutPanel.SuspendLayout();
            PlaybackTabLeftBottomPanel.SuspendLayout();
            PlaybackTabLeftBottomFlowLayoutPanel.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PlaybackTabDataGridView).BeginInit();
            LibraryTab.SuspendLayout();
            LibraryTabTableLayoutPanel.SuspendLayout();
            PlayListsTab.SuspendLayout();
            SettingsTab.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SettingsTabLangGroupBox.SuspendLayout();
            SettingsTabStyleGroupBox.SuspendLayout();
            SettingsTabAutoPlayGroupBox.SuspendLayout();
            SettingsTabAlwaysOnTopGroupBox.SuspendLayout();
            SettingsTabAutoCloseLyricsGroupBox.SuspendLayout();
            SettingsTabConvGroupBox.SuspendLayout();
            SettingsTabConvTableLayoutPanel.SuspendLayout();
            SettingsTabLibraryGroupBox.SuspendLayout();
            SettingsTabLibraryTableLayoutPanel.SuspendLayout();
            SettingsTabEqualizerGroupBox.SuspendLayout();
            SettingsTabEqualizerTableLayoutPanel.SuspendLayout();
            SettingsTabEqualizerTableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar01).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar02).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar03).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar04).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar05).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar06).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar07).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar08).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar09).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar10).BeginInit();
            LibraryFiltersGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // GlobalTableLayoutPanel
            // 
            GlobalTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            GlobalTableLayoutPanel.ColumnCount = 1;
            GlobalTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            GlobalTableLayoutPanel.Controls.Add(MainWIndowHead, 0, 0);
            GlobalTableLayoutPanel.Controls.Add(tableLayoutPanel1, 0, 2);
            GlobalTableLayoutPanel.Controls.Add(tableLayoutPanel2, 0, 1);
            GlobalTableLayoutPanel.Dock = DockStyle.Fill;
            GlobalTableLayoutPanel.Location = new Point(1, 1);
            GlobalTableLayoutPanel.Margin = new Padding(0);
            GlobalTableLayoutPanel.Name = "GlobalTableLayoutPanel";
            GlobalTableLayoutPanel.Padding = new Padding(1);
            GlobalTableLayoutPanel.RowCount = 3;
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            GlobalTableLayoutPanel.Size = new Size(921, 679);
            GlobalTableLayoutPanel.TabIndex = 0;
            // 
            // MainWIndowHead
            // 
            MainWIndowHead.BackColor = Color.FromArgb(30, 30, 30);
            MainWIndowHead.ColumnCount = 5;
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            MainWIndowHead.Controls.Add(TitleLabel, 1, 0);
            MainWIndowHead.Controls.Add(MinimizeButton, 2, 0);
            MainWIndowHead.Controls.Add(MaximizeButton, 3, 0);
            MainWIndowHead.Controls.Add(CloseButton, 4, 0);
            MainWIndowHead.Controls.Add(button1, 0, 0);
            MainWIndowHead.Dock = DockStyle.Fill;
            MainWIndowHead.Location = new Point(1, 1);
            MainWIndowHead.Margin = new Padding(0);
            MainWIndowHead.Name = "MainWIndowHead";
            MainWIndowHead.RowCount = 1;
            MainWIndowHead.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainWIndowHead.Size = new Size(919, 51);
            MainWIndowHead.TabIndex = 3;
            MainWIndowHead.Tag = "WindowHead";
            // 
            // TitleLabel
            // 
            TitleLabel.AutoEllipsis = true;
            TitleLabel.AutoSize = true;
            TitleLabel.BackColor = Color.Transparent;
            TitleLabel.Dock = DockStyle.Fill;
            TitleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TitleLabel.ForeColor = Color.White;
            TitleLabel.ImageAlign = ContentAlignment.MiddleLeft;
            TitleLabel.Location = new Point(55, 0);
            TitleLabel.Margin = new Padding(5, 0, 0, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(714, 51);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "Title";
            TitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MinimizeButton
            // 
            MinimizeButton.BackColor = Color.Gray;
            MinimizeButton.BackgroundImage = Properties.Resources.window_minimize_icon;
            MinimizeButton.BackgroundImageLayout = ImageLayout.Zoom;
            MinimizeButton.Cursor = Cursors.Hand;
            MinimizeButton.Dock = DockStyle.Fill;
            MinimizeButton.FlatStyle = FlatStyle.Popup;
            MinimizeButton.Location = new Point(772, 3);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Size = new Size(44, 45);
            MinimizeButton.TabIndex = 1;
            MinimizeButton.UseVisualStyleBackColor = false;
            MinimizeButton.Click += MinimizeButton_Click;
            // 
            // MaximizeButton
            // 
            MaximizeButton.BackColor = Color.Gray;
            MaximizeButton.BackgroundImage = Properties.Resources.window_maximize_icon;
            MaximizeButton.BackgroundImageLayout = ImageLayout.Zoom;
            MaximizeButton.Cursor = Cursors.Hand;
            MaximizeButton.Dock = DockStyle.Fill;
            MaximizeButton.FlatStyle = FlatStyle.Popup;
            MaximizeButton.Location = new Point(822, 3);
            MaximizeButton.Name = "MaximizeButton";
            MaximizeButton.Size = new Size(44, 45);
            MaximizeButton.TabIndex = 2;
            MaximizeButton.UseVisualStyleBackColor = false;
            MaximizeButton.Click += MaximizeButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.BackColor = Color.Gray;
            CloseButton.BackgroundImage = Properties.Resources.window_close_icon;
            CloseButton.BackgroundImageLayout = ImageLayout.Zoom;
            CloseButton.Cursor = Cursors.Hand;
            CloseButton.Dock = DockStyle.Fill;
            CloseButton.FlatStyle = FlatStyle.Popup;
            CloseButton.Location = new Point(872, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(44, 45);
            CloseButton.TabIndex = 3;
            CloseButton.UseVisualStyleBackColor = false;
            CloseButton.Click += CloseButton_Click;
            // 
            // button1
            // 
            button1.BackgroundImage = Properties.Resources.album_large;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Fill;
            button1.FlatAppearance.BorderColor = Color.FromArgb(30, 30, 30);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(0, 0);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(50, 51);
            button1.TabIndex = 4;
            button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.Controls.Add(DisplayPlaybackPosition, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 2, 0);
            tableLayoutPanel1.Controls.Add(playbackProgressBar, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(1, 618);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(919, 60);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // DisplayPlaybackPosition
            // 
            DisplayPlaybackPosition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            DisplayPlaybackPosition.AutoSize = true;
            DisplayPlaybackPosition.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            DisplayPlaybackPosition.ForeColor = Color.White;
            DisplayPlaybackPosition.Location = new Point(3, 16);
            DisplayPlaybackPosition.Name = "DisplayPlaybackPosition";
            DisplayPlaybackPosition.Size = new Size(114, 28);
            DisplayPlaybackPosition.TabIndex = 0;
            DisplayPlaybackPosition.Text = "00:00";
            DisplayPlaybackPosition.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(GripButton);
            panel1.Controls.Add(DisplayPlaybackSize);
            panel1.Location = new Point(799, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(120, 60);
            panel1.TabIndex = 0;
            // 
            // GripButton
            // 
            GripButton.BackgroundImage = Properties.Resources.Grip_transparent;
            GripButton.BackgroundImageLayout = ImageLayout.None;
            GripButton.Cursor = Cursors.SizeNWSE;
            GripButton.FlatAppearance.BorderColor = Color.FromArgb(30, 30, 30);
            GripButton.FlatAppearance.BorderSize = 0;
            GripButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
            GripButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            GripButton.FlatStyle = FlatStyle.Flat;
            GripButton.Location = new Point(99, 39);
            GripButton.Margin = new Padding(0);
            GripButton.Name = "GripButton";
            GripButton.Size = new Size(21, 24);
            GripButton.TabIndex = 1;
            // 
            // DisplayPlaybackSize
            // 
            DisplayPlaybackSize.BackColor = Color.Transparent;
            DisplayPlaybackSize.Dock = DockStyle.Fill;
            DisplayPlaybackSize.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            DisplayPlaybackSize.ForeColor = Color.White;
            DisplayPlaybackSize.Location = new Point(0, 0);
            DisplayPlaybackSize.Name = "DisplayPlaybackSize";
            DisplayPlaybackSize.Size = new Size(120, 60);
            DisplayPlaybackSize.TabIndex = 0;
            DisplayPlaybackSize.Text = "00:00";
            DisplayPlaybackSize.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // playbackProgressBar
            // 
            playbackProgressBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            playbackProgressBar.BorderStyle = BorderStyle.FixedSingle;
            playbackProgressBar.Cursor = Cursors.Hand;
            playbackProgressBar.ForeColor = Color.Silver;
            playbackProgressBar.Location = new Point(123, 17);
            playbackProgressBar.Name = "playbackProgressBar";
            playbackProgressBar.Size = new Size(673, 25);
            playbackProgressBar.TabIndex = 1;
            playbackProgressBar.Value = 50D;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel2.Controls.Add(TabControler, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(1, 52);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(919, 566);
            tableLayoutPanel2.TabIndex = 6;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(BtnOpen, 0, 0);
            tableLayoutPanel3.Controls.Add(BtnPrevious, 0, 1);
            tableLayoutPanel3.Controls.Add(BtnPlayPause, 0, 2);
            tableLayoutPanel3.Controls.Add(BtnNext, 0, 3);
            tableLayoutPanel3.Controls.Add(BtnRepeat, 0, 4);
            tableLayoutPanel3.Controls.Add(BtnShuffle, 0, 5);
            tableLayoutPanel3.Controls.Add(BtnClearList, 0, 6);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(859, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 8;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(60, 566);
            tableLayoutPanel3.TabIndex = 6;
            // 
            // BtnOpen
            // 
            BtnOpen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnOpen.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnOpen.BackColor = Color.FromArgb(30, 30, 30);
            BtnOpen.BackgroundImage = Properties.Resources.album_large;
            BtnOpen.BackgroundImageLayout = ImageLayout.Center;
            BtnOpen.Cursor = Cursors.Hand;
            BtnOpen.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnOpen.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnOpen.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnOpen.FlatStyle = FlatStyle.Flat;
            BtnOpen.Location = new Point(9, 3);
            BtnOpen.Margin = new Padding(2, 3, 2, 3);
            BtnOpen.Name = "BtnOpen";
            BtnOpen.Size = new Size(41, 41);
            BtnOpen.TabIndex = 0;
            BtnOpen.UseVisualStyleBackColor = false;
            // 
            // BtnPrevious
            // 
            BtnPrevious.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnPrevious.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnPrevious.BackColor = Color.FromArgb(30, 30, 30);
            BtnPrevious.BackgroundImage = Properties.Resources.album_large;
            BtnPrevious.BackgroundImageLayout = ImageLayout.Center;
            BtnPrevious.Cursor = Cursors.Hand;
            BtnPrevious.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnPrevious.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnPrevious.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnPrevious.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnPrevious.FlatStyle = FlatStyle.Flat;
            BtnPrevious.ImageAlign = ContentAlignment.TopLeft;
            BtnPrevious.Location = new Point(9, 50);
            BtnPrevious.Margin = new Padding(2, 3, 2, 3);
            BtnPrevious.Name = "BtnPrevious";
            BtnPrevious.Size = new Size(41, 41);
            BtnPrevious.TabIndex = 0;
            BtnPrevious.UseVisualStyleBackColor = false;
            // 
            // BtnPlayPause
            // 
            BtnPlayPause.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnPlayPause.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnPlayPause.BackColor = Color.FromArgb(30, 30, 30);
            BtnPlayPause.BackgroundImage = Properties.Resources.album_large;
            BtnPlayPause.BackgroundImageLayout = ImageLayout.Center;
            BtnPlayPause.Cursor = Cursors.Hand;
            BtnPlayPause.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnPlayPause.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnPlayPause.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnPlayPause.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnPlayPause.FlatStyle = FlatStyle.Flat;
            BtnPlayPause.Location = new Point(9, 97);
            BtnPlayPause.Margin = new Padding(2, 3, 2, 3);
            BtnPlayPause.Name = "BtnPlayPause";
            BtnPlayPause.Size = new Size(41, 41);
            BtnPlayPause.TabIndex = 0;
            BtnPlayPause.UseVisualStyleBackColor = false;
            // 
            // BtnNext
            // 
            BtnNext.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnNext.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnNext.BackColor = Color.FromArgb(30, 30, 30);
            BtnNext.BackgroundImage = Properties.Resources.album_large;
            BtnNext.BackgroundImageLayout = ImageLayout.Center;
            BtnNext.Cursor = Cursors.Hand;
            BtnNext.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnNext.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnNext.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnNext.FlatStyle = FlatStyle.Flat;
            BtnNext.Location = new Point(9, 144);
            BtnNext.Margin = new Padding(2, 3, 2, 3);
            BtnNext.Name = "BtnNext";
            BtnNext.Size = new Size(41, 41);
            BtnNext.TabIndex = 0;
            BtnNext.UseVisualStyleBackColor = false;
            // 
            // BtnRepeat
            // 
            BtnRepeat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnRepeat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnRepeat.BackColor = Color.FromArgb(30, 30, 30);
            BtnRepeat.BackgroundImage = Properties.Resources.album_large;
            BtnRepeat.BackgroundImageLayout = ImageLayout.Center;
            BtnRepeat.Cursor = Cursors.Hand;
            BtnRepeat.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnRepeat.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnRepeat.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnRepeat.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnRepeat.FlatStyle = FlatStyle.Flat;
            BtnRepeat.Location = new Point(9, 191);
            BtnRepeat.Margin = new Padding(2, 3, 2, 3);
            BtnRepeat.Name = "BtnRepeat";
            BtnRepeat.Size = new Size(41, 41);
            BtnRepeat.TabIndex = 0;
            BtnRepeat.UseVisualStyleBackColor = false;
            // 
            // BtnShuffle
            // 
            BtnShuffle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnShuffle.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnShuffle.BackColor = Color.FromArgb(30, 30, 30);
            BtnShuffle.BackgroundImage = Properties.Resources.album_large;
            BtnShuffle.BackgroundImageLayout = ImageLayout.Center;
            BtnShuffle.Cursor = Cursors.Hand;
            BtnShuffle.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnShuffle.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnShuffle.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnShuffle.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnShuffle.FlatStyle = FlatStyle.Flat;
            BtnShuffle.Location = new Point(9, 238);
            BtnShuffle.Margin = new Padding(2, 3, 2, 3);
            BtnShuffle.Name = "BtnShuffle";
            BtnShuffle.Size = new Size(41, 41);
            BtnShuffle.TabIndex = 0;
            BtnShuffle.UseVisualStyleBackColor = false;
            // 
            // BtnClearList
            // 
            BtnClearList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnClearList.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnClearList.BackColor = Color.FromArgb(30, 30, 30);
            BtnClearList.BackgroundImage = Properties.Resources.album_large;
            BtnClearList.BackgroundImageLayout = ImageLayout.Center;
            BtnClearList.Cursor = Cursors.Hand;
            BtnClearList.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnClearList.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnClearList.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnClearList.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnClearList.FlatStyle = FlatStyle.Flat;
            BtnClearList.ForeColor = Color.White;
            BtnClearList.Location = new Point(9, 285);
            BtnClearList.Margin = new Padding(2, 3, 2, 3);
            BtnClearList.Name = "BtnClearList";
            BtnClearList.Size = new Size(41, 41);
            BtnClearList.TabIndex = 0;
            BtnClearList.UseVisualStyleBackColor = false;
            // 
            // TabControler
            // 
            TabControler.BackColor = Color.FromArgb(30, 30, 30);
            TabControler.ContentAlignment = Manina.Windows.Forms.Alignment.Center;
            TabControler.Controls.Add(PlaybackTab);
            TabControler.Controls.Add(LibraryTab);
            TabControler.Controls.Add(PlayListsTab);
            TabControler.Controls.Add(SettingsTab);
            TabControler.Dock = DockStyle.Fill;
            TabControler.ForeColor = Color.White;
            TabControler.Location = new Point(0, 0);
            TabControler.Margin = new Padding(0);
            TabControler.Name = "TabControler";
            TabControler.Size = new Size(859, 566);
            TabControler.TabIndex = 7;
            TabControler.TabPadding = new Padding(0);
            TabControler.TabSize = new Size(100, 50);
            TabControler.TabSizing = Manina.Windows.Forms.TabSizing.Stretch;
            // 
            // PlaybackTab
            // 
            PlaybackTab.BackColor = Color.Gray;
            PlaybackTab.Controls.Add(PlaybackTabMainTableLayoutPanel);
            PlaybackTab.ForeColor = Color.White;
            PlaybackTab.Location = new Point(1, 50);
            PlaybackTab.Name = "PlaybackTab";
            PlaybackTab.Size = new Size(857, 515);
            PlaybackTab.Text = " Playback";
            // 
            // PlaybackTabMainTableLayoutPanel
            // 
            PlaybackTabMainTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            PlaybackTabMainTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            PlaybackTabMainTableLayoutPanel.ColumnCount = 2;
            PlaybackTabMainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            PlaybackTabMainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlaybackTabMainTableLayoutPanel.Controls.Add(PlaybackTabLeftTableLayoutPanel, 0, 0);
            PlaybackTabMainTableLayoutPanel.Controls.Add(tableLayoutPanel4, 1, 0);
            PlaybackTabMainTableLayoutPanel.Dock = DockStyle.Fill;
            PlaybackTabMainTableLayoutPanel.Location = new Point(0, 0);
            PlaybackTabMainTableLayoutPanel.Margin = new Padding(0);
            PlaybackTabMainTableLayoutPanel.Name = "PlaybackTabMainTableLayoutPanel";
            PlaybackTabMainTableLayoutPanel.RowCount = 1;
            PlaybackTabMainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlaybackTabMainTableLayoutPanel.Size = new Size(857, 515);
            PlaybackTabMainTableLayoutPanel.TabIndex = 0;
            // 
            // PlaybackTabLeftTableLayoutPanel
            // 
            PlaybackTabLeftTableLayoutPanel.ColumnCount = 1;
            PlaybackTabLeftTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlaybackTabLeftTableLayoutPanel.Controls.Add(PlaybackTabLeftBottomPanel, 0, 1);
            PlaybackTabLeftTableLayoutPanel.Controls.Add(FileCover, 0, 0);
            PlaybackTabLeftTableLayoutPanel.Dock = DockStyle.Fill;
            PlaybackTabLeftTableLayoutPanel.Location = new Point(1, 1);
            PlaybackTabLeftTableLayoutPanel.Margin = new Padding(0);
            PlaybackTabLeftTableLayoutPanel.Name = "PlaybackTabLeftTableLayoutPanel";
            PlaybackTabLeftTableLayoutPanel.RowCount = 2;
            PlaybackTabLeftTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            PlaybackTabLeftTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlaybackTabLeftTableLayoutPanel.Size = new Size(150, 513);
            PlaybackTabLeftTableLayoutPanel.TabIndex = 1;
            // 
            // PlaybackTabLeftBottomPanel
            // 
            PlaybackTabLeftBottomPanel.AutoScroll = true;
            PlaybackTabLeftBottomPanel.Controls.Add(PlaybackTabLeftBottomFlowLayoutPanel);
            PlaybackTabLeftBottomPanel.Dock = DockStyle.Fill;
            PlaybackTabLeftBottomPanel.Location = new Point(0, 150);
            PlaybackTabLeftBottomPanel.Margin = new Padding(0);
            PlaybackTabLeftBottomPanel.Name = "PlaybackTabLeftBottomPanel";
            PlaybackTabLeftBottomPanel.Size = new Size(150, 363);
            PlaybackTabLeftBottomPanel.TabIndex = 0;
            // 
            // PlaybackTabLeftBottomFlowLayoutPanel
            // 
            PlaybackTabLeftBottomFlowLayoutPanel.AutoSize = true;
            PlaybackTabLeftBottomFlowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabTitleLabelInfo);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabTitleLabelValue);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabAlbumLabelInfo);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabAlbumLabelValue);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabArtistsLabelInfo);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabArtistsLabelValue);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabDurationLabelInfo);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabDurationLabelValue);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabLyricsButton);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabRatting);
            PlaybackTabLeftBottomFlowLayoutPanel.Dock = DockStyle.Top;
            PlaybackTabLeftBottomFlowLayoutPanel.Location = new Point(0, 0);
            PlaybackTabLeftBottomFlowLayoutPanel.Margin = new Padding(3, 4, 3, 4);
            PlaybackTabLeftBottomFlowLayoutPanel.MinimumSize = new Size(150, 249);
            PlaybackTabLeftBottomFlowLayoutPanel.Name = "PlaybackTabLeftBottomFlowLayoutPanel";
            PlaybackTabLeftBottomFlowLayoutPanel.Size = new Size(150, 249);
            PlaybackTabLeftBottomFlowLayoutPanel.TabIndex = 0;
            // 
            // PlaybackTabTitleLabelInfo
            // 
            PlaybackTabTitleLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            PlaybackTabTitleLabelInfo.Location = new Point(2, 3);
            PlaybackTabTitleLabelInfo.Margin = new Padding(2, 3, 0, 0);
            PlaybackTabTitleLabelInfo.Name = "PlaybackTabTitleLabelInfo";
            PlaybackTabTitleLabelInfo.Size = new Size(166, 24);
            PlaybackTabTitleLabelInfo.TabIndex = 1;
            PlaybackTabTitleLabelInfo.Text = "Title:";
            // 
            // PlaybackTabTitleLabelValue
            // 
            PlaybackTabTitleLabelValue.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            PlaybackTabTitleLabelValue.Location = new Point(11, 27);
            PlaybackTabTitleLabelValue.Margin = new Padding(11, 0, 0, 0);
            PlaybackTabTitleLabelValue.MinimumSize = new Size(140, 20);
            PlaybackTabTitleLabelValue.Name = "PlaybackTabTitleLabelValue";
            PlaybackTabTitleLabelValue.Size = new Size(140, 20);
            PlaybackTabTitleLabelValue.TabIndex = 1;
            PlaybackTabTitleLabelValue.Text = "Track Name";
            // 
            // PlaybackTabAlbumLabelInfo
            // 
            PlaybackTabAlbumLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            PlaybackTabAlbumLabelInfo.Location = new Point(2, 47);
            PlaybackTabAlbumLabelInfo.Margin = new Padding(2, 0, 0, 0);
            PlaybackTabAlbumLabelInfo.Name = "PlaybackTabAlbumLabelInfo";
            PlaybackTabAlbumLabelInfo.Size = new Size(166, 24);
            PlaybackTabAlbumLabelInfo.TabIndex = 1;
            PlaybackTabAlbumLabelInfo.Text = "Album:";
            // 
            // PlaybackTabAlbumLabelValue
            // 
            PlaybackTabAlbumLabelValue.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            PlaybackTabAlbumLabelValue.Location = new Point(11, 71);
            PlaybackTabAlbumLabelValue.Margin = new Padding(11, 0, 0, 0);
            PlaybackTabAlbumLabelValue.MinimumSize = new Size(140, 20);
            PlaybackTabAlbumLabelValue.Name = "PlaybackTabAlbumLabelValue";
            PlaybackTabAlbumLabelValue.Size = new Size(140, 20);
            PlaybackTabAlbumLabelValue.TabIndex = 1;
            // 
            // PlaybackTabArtistsLabelInfo
            // 
            PlaybackTabArtistsLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            PlaybackTabArtistsLabelInfo.Location = new Point(2, 91);
            PlaybackTabArtistsLabelInfo.Margin = new Padding(2, 0, 0, 0);
            PlaybackTabArtistsLabelInfo.Name = "PlaybackTabArtistsLabelInfo";
            PlaybackTabArtistsLabelInfo.Size = new Size(166, 24);
            PlaybackTabArtistsLabelInfo.TabIndex = 1;
            PlaybackTabArtistsLabelInfo.Text = "Artists:";
            // 
            // PlaybackTabArtistsLabelValue
            // 
            PlaybackTabArtistsLabelValue.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            PlaybackTabArtistsLabelValue.Location = new Point(11, 115);
            PlaybackTabArtistsLabelValue.Margin = new Padding(11, 0, 0, 0);
            PlaybackTabArtistsLabelValue.MinimumSize = new Size(140, 20);
            PlaybackTabArtistsLabelValue.Name = "PlaybackTabArtistsLabelValue";
            PlaybackTabArtistsLabelValue.Size = new Size(140, 20);
            PlaybackTabArtistsLabelValue.TabIndex = 1;
            // 
            // PlaybackTabDurationLabelInfo
            // 
            PlaybackTabDurationLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            PlaybackTabDurationLabelInfo.Location = new Point(2, 135);
            PlaybackTabDurationLabelInfo.Margin = new Padding(2, 0, 0, 0);
            PlaybackTabDurationLabelInfo.Name = "PlaybackTabDurationLabelInfo";
            PlaybackTabDurationLabelInfo.Size = new Size(166, 24);
            PlaybackTabDurationLabelInfo.TabIndex = 1;
            PlaybackTabDurationLabelInfo.Text = "Duration:";
            // 
            // PlaybackTabDurationLabelValue
            // 
            PlaybackTabDurationLabelValue.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            PlaybackTabDurationLabelValue.Location = new Point(11, 159);
            PlaybackTabDurationLabelValue.Margin = new Padding(11, 0, 0, 0);
            PlaybackTabDurationLabelValue.MinimumSize = new Size(80, 20);
            PlaybackTabDurationLabelValue.Name = "PlaybackTabDurationLabelValue";
            PlaybackTabDurationLabelValue.Size = new Size(138, 22);
            PlaybackTabDurationLabelValue.TabIndex = 1;
            // 
            // PlaybackTabLyricsButton
            // 
            PlaybackTabLyricsButton.FlatAppearance.BorderColor = Color.White;
            PlaybackTabLyricsButton.FlatAppearance.MouseDownBackColor = Color.Gray;
            PlaybackTabLyricsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            PlaybackTabLyricsButton.FlatStyle = FlatStyle.Flat;
            PlaybackTabLyricsButton.Location = new Point(3, 184);
            PlaybackTabLyricsButton.Name = "PlaybackTabLyricsButton";
            PlaybackTabLyricsButton.Size = new Size(94, 29);
            PlaybackTabLyricsButton.TabIndex = 2;
            PlaybackTabLyricsButton.Text = "Lyrics";
            PlaybackTabLyricsButton.UseVisualStyleBackColor = true;
            // 
            // PlaybackTabRatting
            // 
            PlaybackTabRatting.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PlaybackTabRatting.BackColor = Color.Transparent;
            PlaybackTabRatting.IsReadOnly = false;
            PlaybackTabRatting.Location = new Point(0, 219);
            PlaybackTabRatting.Margin = new Padding(0, 3, 0, 0);
            PlaybackTabRatting.MaximumSize = new Size(100, 20);
            PlaybackTabRatting.MinimumSize = new Size(100, 20);
            PlaybackTabRatting.Name = "PlaybackTabRatting";
            PlaybackTabRatting.Rate = 0D;
            PlaybackTabRatting.Size = new Size(100, 20);
            PlaybackTabRatting.TabIndex = 3;
            PlaybackTabRatting.Zoom = 1D;
            // 
            // FileCover
            // 
            FileCover.BackColor = Color.Black;
            FileCover.BackgroundImage = Properties.Resources.album_large;
            FileCover.BackgroundImageLayout = ImageLayout.Zoom;
            FileCover.Dock = DockStyle.Fill;
            FileCover.FlatAppearance.BorderColor = Color.Black;
            FileCover.FlatAppearance.BorderSize = 0;
            FileCover.FlatAppearance.MouseDownBackColor = Color.Black;
            FileCover.FlatAppearance.MouseOverBackColor = Color.Black;
            FileCover.FlatStyle = FlatStyle.Flat;
            FileCover.Location = new Point(0, 0);
            FileCover.Margin = new Padding(0);
            FileCover.Name = "FileCover";
            FileCover.Size = new Size(150, 150);
            FileCover.TabIndex = 1;
            FileCover.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(PlaybackTabDataGridView, 0, 1);
            tableLayoutPanel4.Controls.Add(PlaybackPositionLabel, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(152, 1);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(704, 513);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // PlaybackTabDataGridView
            // 
            PlaybackTabDataGridView.AllowUserToAddRows = false;
            PlaybackTabDataGridView.AllowUserToDeleteRows = false;
            PlaybackTabDataGridView.AllowUserToResizeColumns = false;
            PlaybackTabDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(128, 64, 64);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            PlaybackTabDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            PlaybackTabDataGridView.BackgroundColor = Color.FromArgb(30, 30, 30);
            PlaybackTabDataGridView.BorderStyle = BorderStyle.None;
            PlaybackTabDataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            PlaybackTabDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.Teal;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.Teal;
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            PlaybackTabDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            PlaybackTabDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PlaybackTabDataGridView.Columns.AddRange(new DataGridViewColumn[] { SelectedColumn, NameColumn, DurationColumn, ArtistsColumn, AlbumColumn });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.DimGray;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            PlaybackTabDataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            PlaybackTabDataGridView.Dock = DockStyle.Fill;
            PlaybackTabDataGridView.EnableHeadersVisualStyles = false;
            PlaybackTabDataGridView.GridColor = Color.Black;
            PlaybackTabDataGridView.Location = new Point(0, 40);
            PlaybackTabDataGridView.Margin = new Padding(0);
            PlaybackTabDataGridView.MultiSelect = false;
            PlaybackTabDataGridView.Name = "PlaybackTabDataGridView";
            PlaybackTabDataGridView.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.ControlDarkDark;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            PlaybackTabDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            PlaybackTabDataGridView.RowHeadersVisible = false;
            PlaybackTabDataGridView.RowHeadersWidth = 51;
            PlaybackTabDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.DimGray;
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            PlaybackTabDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle6;
            PlaybackTabDataGridView.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            PlaybackTabDataGridView.RowTemplate.DefaultCellStyle.BackColor = Color.DimGray;
            PlaybackTabDataGridView.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            PlaybackTabDataGridView.RowTemplate.DefaultCellStyle.ForeColor = Color.White;
            PlaybackTabDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            PlaybackTabDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            PlaybackTabDataGridView.RowTemplate.Height = 25;
            PlaybackTabDataGridView.ScrollBars = ScrollBars.Vertical;
            PlaybackTabDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PlaybackTabDataGridView.ShowCellErrors = false;
            PlaybackTabDataGridView.ShowEditingIcon = false;
            PlaybackTabDataGridView.ShowRowErrors = false;
            PlaybackTabDataGridView.Size = new Size(704, 473);
            PlaybackTabDataGridView.TabIndex = 1;
            // 
            // SelectedColumn
            // 
            SelectedColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            SelectedColumn.DataPropertyName = "Selected";
            dataGridViewCellStyle3.Font = new Font("Wingdings 3", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SelectedColumn.DefaultCellStyle = dataGridViewCellStyle3;
            SelectedColumn.HeaderText = "";
            SelectedColumn.MinimumWidth = 30;
            SelectedColumn.Name = "SelectedColumn";
            SelectedColumn.ReadOnly = true;
            SelectedColumn.Width = 30;
            // 
            // NameColumn
            // 
            NameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            NameColumn.DataPropertyName = "Name";
            NameColumn.HeaderText = "Title";
            NameColumn.MinimumWidth = 6;
            NameColumn.Name = "NameColumn";
            NameColumn.ReadOnly = true;
            // 
            // DurationColumn
            // 
            DurationColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DurationColumn.DataPropertyName = "DurationS";
            DurationColumn.HeaderText = "Duration";
            DurationColumn.MinimumWidth = 80;
            DurationColumn.Name = "DurationColumn";
            DurationColumn.ReadOnly = true;
            DurationColumn.Width = 80;
            // 
            // ArtistsColumn
            // 
            ArtistsColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ArtistsColumn.DataPropertyName = "Artists";
            ArtistsColumn.HeaderText = "Artists";
            ArtistsColumn.MinimumWidth = 6;
            ArtistsColumn.Name = "ArtistsColumn";
            ArtistsColumn.ReadOnly = true;
            // 
            // AlbumColumn
            // 
            AlbumColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AlbumColumn.DataPropertyName = "Album";
            AlbumColumn.HeaderText = "Album";
            AlbumColumn.MinimumWidth = 6;
            AlbumColumn.Name = "AlbumColumn";
            AlbumColumn.ReadOnly = true;
            // 
            // PlaybackPositionLabel
            // 
            PlaybackPositionLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PlaybackPositionLabel.AutoSize = true;
            PlaybackPositionLabel.Location = new Point(3, 10);
            PlaybackPositionLabel.Name = "PlaybackPositionLabel";
            PlaybackPositionLabel.Size = new Size(698, 20);
            PlaybackPositionLabel.TabIndex = 2;
            PlaybackPositionLabel.Text = "label1";
            // 
            // LibraryTab
            // 
            LibraryTab.BackColor = Color.Gray;
            LibraryTab.Controls.Add(LibraryTabTableLayoutPanel);
            LibraryTab.ForeColor = Color.White;
            LibraryTab.Location = new Point(1, 50);
            LibraryTab.Name = "LibraryTab";
            LibraryTab.Size = new Size(857, 515);
            LibraryTab.Text = " Library";
            // 
            // LibraryTabTableLayoutPanel
            // 
            LibraryTabTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            LibraryTabTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            LibraryTabTableLayoutPanel.ColumnCount = 1;
            LibraryTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            LibraryTabTableLayoutPanel.Controls.Add(LibibraryNavigationPathContener, 0, 0);
            LibraryTabTableLayoutPanel.Controls.Add(LibraryFiltersGrid, 0, 1);
            LibraryTabTableLayoutPanel.Controls.Add(splitContainer1, 0, 2);
            LibraryTabTableLayoutPanel.Dock = DockStyle.Fill;
            LibraryTabTableLayoutPanel.Location = new Point(0, 0);
            LibraryTabTableLayoutPanel.Margin = new Padding(0);
            LibraryTabTableLayoutPanel.Name = "LibraryTabTableLayoutPanel";
            LibraryTabTableLayoutPanel.RowCount = 3;
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            LibraryTabTableLayoutPanel.Size = new Size(857, 515);
            LibraryTabTableLayoutPanel.TabIndex = 1;
            // 
            // PlayListsTab
            // 
            PlayListsTab.BackColor = Color.Gray;
            PlayListsTab.Controls.Add(TableLayoutPanel);
            PlayListsTab.ForeColor = Color.White;
            PlayListsTab.Location = new Point(0, 0);
            PlayListsTab.Name = "PlayListsTab";
            PlayListsTab.Size = new Size(0, 0);
            PlayListsTab.Text = " PlayLists";
            // 
            // TableLayoutPanel
            // 
            TableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            TableLayoutPanel.ColumnCount = 2;
            TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 171F));
            TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TableLayoutPanel.Dock = DockStyle.Fill;
            TableLayoutPanel.Location = new Point(0, 0);
            TableLayoutPanel.Margin = new Padding(0);
            TableLayoutPanel.Name = "TableLayoutPanel";
            TableLayoutPanel.RowCount = 1;
            TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TableLayoutPanel.Size = new Size(0, 0);
            TableLayoutPanel.TabIndex = 1;
            // 
            // SettingsTab
            // 
            SettingsTab.BackColor = Color.Gray;
            SettingsTab.Controls.Add(panel2);
            SettingsTab.ForeColor = Color.White;
            SettingsTab.Location = new Point(1, 50);
            SettingsTab.Name = "SettingsTab";
            SettingsTab.Size = new Size(857, 515);
            SettingsTab.Text = " Settings";
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.BackColor = Color.FromArgb(30, 30, 30);
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(857, 515);
            panel2.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(SettingsTabLangGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabStyleGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabAutoPlayGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabAlwaysOnTopGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabAutoCloseLyricsGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabConvGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabLibraryGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabEqualizerGroupBox);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(836, 617);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // SettingsTabLangGroupBox
            // 
            SettingsTabLangGroupBox.AutoSize = true;
            SettingsTabLangGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabLangGroupBox.Controls.Add(SettingsTabLangComboBox);
            SettingsTabLangGroupBox.ForeColor = Color.White;
            SettingsTabLangGroupBox.Location = new Point(3, 4);
            SettingsTabLangGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabLangGroupBox.MinimumSize = new Size(171, 67);
            SettingsTabLangGroupBox.Name = "SettingsTabLangGroupBox";
            SettingsTabLangGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabLangGroupBox.Size = new Size(171, 67);
            SettingsTabLangGroupBox.TabIndex = 1;
            SettingsTabLangGroupBox.TabStop = false;
            SettingsTabLangGroupBox.Text = "Lang";
            // 
            // SettingsTabLangComboBox
            // 
            SettingsTabLangComboBox.BackColor = Color.Gray;
            SettingsTabLangComboBox.DisplayMember = "English";
            SettingsTabLangComboBox.Dock = DockStyle.Fill;
            SettingsTabLangComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabLangComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabLangComboBox.ForeColor = Color.Black;
            SettingsTabLangComboBox.FormattingEnabled = true;
            SettingsTabLangComboBox.Items.AddRange(new object[] { "English", "Français" });
            SettingsTabLangComboBox.Location = new Point(6, 27);
            SettingsTabLangComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabLangComboBox.MaxDropDownItems = 2;
            SettingsTabLangComboBox.Name = "SettingsTabLangComboBox";
            SettingsTabLangComboBox.Size = new Size(159, 28);
            SettingsTabLangComboBox.TabIndex = 0;
            SettingsTabLangComboBox.ValueMember = "English";
            // 
            // SettingsTabStyleGroupBox
            // 
            SettingsTabStyleGroupBox.AutoSize = true;
            SettingsTabStyleGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabStyleGroupBox.Controls.Add(SettingsTabStyleComboBox);
            SettingsTabStyleGroupBox.ForeColor = Color.White;
            SettingsTabStyleGroupBox.Location = new Point(180, 4);
            SettingsTabStyleGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabStyleGroupBox.MinimumSize = new Size(171, 67);
            SettingsTabStyleGroupBox.Name = "SettingsTabStyleGroupBox";
            SettingsTabStyleGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabStyleGroupBox.Size = new Size(171, 67);
            SettingsTabStyleGroupBox.TabIndex = 1;
            SettingsTabStyleGroupBox.TabStop = false;
            SettingsTabStyleGroupBox.Text = "Style";
            // 
            // SettingsTabStyleComboBox
            // 
            SettingsTabStyleComboBox.BackColor = Color.Gray;
            SettingsTabStyleComboBox.DisplayMember = "English";
            SettingsTabStyleComboBox.Dock = DockStyle.Fill;
            SettingsTabStyleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabStyleComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabStyleComboBox.ForeColor = Color.Black;
            SettingsTabStyleComboBox.FormattingEnabled = true;
            SettingsTabStyleComboBox.Items.AddRange(new object[] { "Dark", "Red" });
            SettingsTabStyleComboBox.Location = new Point(6, 27);
            SettingsTabStyleComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabStyleComboBox.MaxDropDownItems = 2;
            SettingsTabStyleComboBox.Name = "SettingsTabStyleComboBox";
            SettingsTabStyleComboBox.Size = new Size(159, 28);
            SettingsTabStyleComboBox.TabIndex = 0;
            SettingsTabStyleComboBox.ValueMember = "English";
            // 
            // SettingsTabAutoPlayGroupBox
            // 
            SettingsTabAutoPlayGroupBox.AutoSize = true;
            SettingsTabAutoPlayGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabAutoPlayGroupBox.Controls.Add(SettingsTabAutoPlayComboBox);
            SettingsTabAutoPlayGroupBox.ForeColor = Color.White;
            SettingsTabAutoPlayGroupBox.Location = new Point(357, 4);
            SettingsTabAutoPlayGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabAutoPlayGroupBox.MinimumSize = new Size(220, 67);
            SettingsTabAutoPlayGroupBox.Name = "SettingsTabAutoPlayGroupBox";
            SettingsTabAutoPlayGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabAutoPlayGroupBox.Size = new Size(220, 67);
            SettingsTabAutoPlayGroupBox.TabIndex = 1;
            SettingsTabAutoPlayGroupBox.TabStop = false;
            SettingsTabAutoPlayGroupBox.Text = "Auto Play";
            // 
            // SettingsTabAutoPlayComboBox
            // 
            SettingsTabAutoPlayComboBox.BackColor = Color.Gray;
            SettingsTabAutoPlayComboBox.DisplayMember = "English";
            SettingsTabAutoPlayComboBox.Dock = DockStyle.Fill;
            SettingsTabAutoPlayComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabAutoPlayComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabAutoPlayComboBox.ForeColor = Color.Black;
            SettingsTabAutoPlayComboBox.FormattingEnabled = true;
            SettingsTabAutoPlayComboBox.Items.AddRange(new object[] { "No", "Yes" });
            SettingsTabAutoPlayComboBox.Location = new Point(6, 27);
            SettingsTabAutoPlayComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabAutoPlayComboBox.MaxDropDownItems = 2;
            SettingsTabAutoPlayComboBox.Name = "SettingsTabAutoPlayComboBox";
            SettingsTabAutoPlayComboBox.Size = new Size(208, 28);
            SettingsTabAutoPlayComboBox.TabIndex = 0;
            SettingsTabAutoPlayComboBox.ValueMember = "English";
            // 
            // SettingsTabAlwaysOnTopGroupBox
            // 
            SettingsTabAlwaysOnTopGroupBox.AutoSize = true;
            SettingsTabAlwaysOnTopGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabAlwaysOnTopGroupBox.Controls.Add(SettingsTabAlwaysOnTopComboBox);
            SettingsTabAlwaysOnTopGroupBox.ForeColor = Color.White;
            SettingsTabAlwaysOnTopGroupBox.Location = new Point(583, 4);
            SettingsTabAlwaysOnTopGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabAlwaysOnTopGroupBox.MinimumSize = new Size(171, 67);
            SettingsTabAlwaysOnTopGroupBox.Name = "SettingsTabAlwaysOnTopGroupBox";
            SettingsTabAlwaysOnTopGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabAlwaysOnTopGroupBox.Size = new Size(171, 67);
            SettingsTabAlwaysOnTopGroupBox.TabIndex = 1;
            SettingsTabAlwaysOnTopGroupBox.TabStop = false;
            SettingsTabAlwaysOnTopGroupBox.Text = "Always On Top";
            // 
            // SettingsTabAlwaysOnTopComboBox
            // 
            SettingsTabAlwaysOnTopComboBox.BackColor = Color.Gray;
            SettingsTabAlwaysOnTopComboBox.DisplayMember = "English";
            SettingsTabAlwaysOnTopComboBox.Dock = DockStyle.Fill;
            SettingsTabAlwaysOnTopComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabAlwaysOnTopComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabAlwaysOnTopComboBox.ForeColor = Color.Black;
            SettingsTabAlwaysOnTopComboBox.FormattingEnabled = true;
            SettingsTabAlwaysOnTopComboBox.Items.AddRange(new object[] { "No", "Yes" });
            SettingsTabAlwaysOnTopComboBox.Location = new Point(6, 27);
            SettingsTabAlwaysOnTopComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabAlwaysOnTopComboBox.MaxDropDownItems = 2;
            SettingsTabAlwaysOnTopComboBox.Name = "SettingsTabAlwaysOnTopComboBox";
            SettingsTabAlwaysOnTopComboBox.Size = new Size(159, 28);
            SettingsTabAlwaysOnTopComboBox.TabIndex = 0;
            SettingsTabAlwaysOnTopComboBox.ValueMember = "English";
            // 
            // SettingsTabAutoCloseLyricsGroupBox
            // 
            SettingsTabAutoCloseLyricsGroupBox.AutoSize = true;
            SettingsTabAutoCloseLyricsGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabAutoCloseLyricsGroupBox.Controls.Add(SettingsTabAutoCloseLyricsComboBox);
            SettingsTabAutoCloseLyricsGroupBox.ForeColor = Color.White;
            SettingsTabAutoCloseLyricsGroupBox.Location = new Point(3, 79);
            SettingsTabAutoCloseLyricsGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabAutoCloseLyricsGroupBox.MinimumSize = new Size(190, 67);
            SettingsTabAutoCloseLyricsGroupBox.Name = "SettingsTabAutoCloseLyricsGroupBox";
            SettingsTabAutoCloseLyricsGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabAutoCloseLyricsGroupBox.Size = new Size(190, 67);
            SettingsTabAutoCloseLyricsGroupBox.TabIndex = 1;
            SettingsTabAutoCloseLyricsGroupBox.TabStop = false;
            SettingsTabAutoCloseLyricsGroupBox.Text = "Auto Close Lyrics";
            // 
            // SettingsTabAutoCloseLyricsComboBox
            // 
            SettingsTabAutoCloseLyricsComboBox.BackColor = Color.Gray;
            SettingsTabAutoCloseLyricsComboBox.DisplayMember = "English";
            SettingsTabAutoCloseLyricsComboBox.Dock = DockStyle.Fill;
            SettingsTabAutoCloseLyricsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabAutoCloseLyricsComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabAutoCloseLyricsComboBox.ForeColor = Color.Black;
            SettingsTabAutoCloseLyricsComboBox.FormattingEnabled = true;
            SettingsTabAutoCloseLyricsComboBox.Items.AddRange(new object[] { "No", "Yes" });
            SettingsTabAutoCloseLyricsComboBox.Location = new Point(6, 27);
            SettingsTabAutoCloseLyricsComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabAutoCloseLyricsComboBox.MaxDropDownItems = 2;
            SettingsTabAutoCloseLyricsComboBox.Name = "SettingsTabAutoCloseLyricsComboBox";
            SettingsTabAutoCloseLyricsComboBox.Size = new Size(178, 28);
            SettingsTabAutoCloseLyricsComboBox.TabIndex = 0;
            SettingsTabAutoCloseLyricsComboBox.ValueMember = "English";
            // 
            // SettingsTabConvGroupBox
            // 
            SettingsTabConvGroupBox.AutoSize = true;
            SettingsTabConvGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabConvGroupBox.Controls.Add(SettingsTabConvTableLayoutPanel);
            SettingsTabConvGroupBox.ForeColor = Color.White;
            SettingsTabConvGroupBox.Location = new Point(199, 79);
            SettingsTabConvGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabConvGroupBox.MinimumSize = new Size(300, 100);
            SettingsTabConvGroupBox.Name = "SettingsTabConvGroupBox";
            SettingsTabConvGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabConvGroupBox.Size = new Size(300, 100);
            SettingsTabConvGroupBox.TabIndex = 1;
            SettingsTabConvGroupBox.TabStop = false;
            SettingsTabConvGroupBox.Text = "Convertion";
            // 
            // SettingsTabConvTableLayoutPanel
            // 
            SettingsTabConvTableLayoutPanel.ColumnCount = 2;
            SettingsTabConvTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            SettingsTabConvTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvModeLabel, 0, 0);
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvModeComboBox, 1, 0);
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvQualityLabel, 0, 1);
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvQualityComboBox, 1, 1);
            SettingsTabConvTableLayoutPanel.Dock = DockStyle.Fill;
            SettingsTabConvTableLayoutPanel.Location = new Point(6, 27);
            SettingsTabConvTableLayoutPanel.Name = "SettingsTabConvTableLayoutPanel";
            SettingsTabConvTableLayoutPanel.RowCount = 2;
            SettingsTabConvTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            SettingsTabConvTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            SettingsTabConvTableLayoutPanel.Size = new Size(288, 66);
            SettingsTabConvTableLayoutPanel.TabIndex = 0;
            // 
            // SettingsTabConvModeLabel
            // 
            SettingsTabConvModeLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabConvModeLabel.AutoSize = true;
            SettingsTabConvModeLabel.Location = new Point(3, 6);
            SettingsTabConvModeLabel.Name = "SettingsTabConvModeLabel";
            SettingsTabConvModeLabel.Size = new Size(94, 20);
            SettingsTabConvModeLabel.TabIndex = 0;
            SettingsTabConvModeLabel.Text = "Convertion";
            // 
            // SettingsTabConvModeComboBox
            // 
            SettingsTabConvModeComboBox.BackColor = Color.Gray;
            SettingsTabConvModeComboBox.DisplayMember = "English";
            SettingsTabConvModeComboBox.Dock = DockStyle.Fill;
            SettingsTabConvModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabConvModeComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabConvModeComboBox.ForeColor = Color.Black;
            SettingsTabConvModeComboBox.FormattingEnabled = true;
            SettingsTabConvModeComboBox.Items.AddRange(new object[] { "On the Fly", "Replace" });
            SettingsTabConvModeComboBox.Location = new Point(103, 4);
            SettingsTabConvModeComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabConvModeComboBox.MaxDropDownItems = 2;
            SettingsTabConvModeComboBox.Name = "SettingsTabConvModeComboBox";
            SettingsTabConvModeComboBox.Size = new Size(182, 28);
            SettingsTabConvModeComboBox.TabIndex = 1;
            SettingsTabConvModeComboBox.ValueMember = "English";
            // 
            // SettingsTabConvQualityLabel
            // 
            SettingsTabConvQualityLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabConvQualityLabel.AutoSize = true;
            SettingsTabConvQualityLabel.Location = new Point(3, 39);
            SettingsTabConvQualityLabel.Name = "SettingsTabConvQualityLabel";
            SettingsTabConvQualityLabel.Size = new Size(94, 20);
            SettingsTabConvQualityLabel.TabIndex = 0;
            SettingsTabConvQualityLabel.Text = "Quality";
            // 
            // SettingsTabConvQualityComboBox
            // 
            SettingsTabConvQualityComboBox.BackColor = Color.Gray;
            SettingsTabConvQualityComboBox.DisplayMember = "English";
            SettingsTabConvQualityComboBox.Dock = DockStyle.Fill;
            SettingsTabConvQualityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabConvQualityComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabConvQualityComboBox.ForeColor = Color.Black;
            SettingsTabConvQualityComboBox.FormattingEnabled = true;
            SettingsTabConvQualityComboBox.Items.AddRange(new object[] { "96 Kbits/s", "128 Kbits/s", "192 Kbits/s", "256 Kbits/s", "320 Kbits/s" });
            SettingsTabConvQualityComboBox.Location = new Point(103, 37);
            SettingsTabConvQualityComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabConvQualityComboBox.MaxDropDownItems = 5;
            SettingsTabConvQualityComboBox.Name = "SettingsTabConvQualityComboBox";
            SettingsTabConvQualityComboBox.Size = new Size(182, 28);
            SettingsTabConvQualityComboBox.TabIndex = 2;
            SettingsTabConvQualityComboBox.ValueMember = "English";
            // 
            // SettingsTabLibraryGroupBox
            // 
            SettingsTabLibraryGroupBox.AutoSize = true;
            SettingsTabLibraryGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabLibraryGroupBox.Controls.Add(SettingsTabLibraryTableLayoutPanel);
            SettingsTabLibraryGroupBox.ForeColor = Color.White;
            SettingsTabLibraryGroupBox.Location = new Point(3, 187);
            SettingsTabLibraryGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabLibraryGroupBox.MinimumSize = new Size(380, 100);
            SettingsTabLibraryGroupBox.Name = "SettingsTabLibraryGroupBox";
            SettingsTabLibraryGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabLibraryGroupBox.Size = new Size(380, 134);
            SettingsTabLibraryGroupBox.TabIndex = 1;
            SettingsTabLibraryGroupBox.TabStop = false;
            SettingsTabLibraryGroupBox.Text = "Library";
            // 
            // SettingsTabLibraryTableLayoutPanel
            // 
            SettingsTabLibraryTableLayoutPanel.ColumnCount = 2;
            SettingsTabLibraryTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SettingsTabLibraryTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryFolderTextBox, 0, 0);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryFolderButton, 1, 0);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryUnixHiddenFileLabel, 0, 1);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryUnixHiddenFileComboBox, 1, 1);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryWindowsHiddenFileLabel, 0, 2);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryWindowsHiddenFileComboBox, 1, 2);
            SettingsTabLibraryTableLayoutPanel.Dock = DockStyle.Top;
            SettingsTabLibraryTableLayoutPanel.Location = new Point(6, 27);
            SettingsTabLibraryTableLayoutPanel.Name = "SettingsTabLibraryTableLayoutPanel";
            SettingsTabLibraryTableLayoutPanel.RowCount = 4;
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle());
            SettingsTabLibraryTableLayoutPanel.Size = new Size(368, 100);
            SettingsTabLibraryTableLayoutPanel.TabIndex = 0;
            // 
            // SettingsTabLibraryFolderTextBox
            // 
            SettingsTabLibraryFolderTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabLibraryFolderTextBox.BackColor = Color.Gray;
            SettingsTabLibraryFolderTextBox.BorderStyle = BorderStyle.FixedSingle;
            SettingsTabLibraryFolderTextBox.ForeColor = Color.White;
            SettingsTabLibraryFolderTextBox.Location = new Point(3, 3);
            SettingsTabLibraryFolderTextBox.Name = "SettingsTabLibraryFolderTextBox";
            SettingsTabLibraryFolderTextBox.ReadOnly = true;
            SettingsTabLibraryFolderTextBox.Size = new Size(262, 27);
            SettingsTabLibraryFolderTextBox.TabIndex = 4;
            // 
            // SettingsTabLibraryFolderButton
            // 
            SettingsTabLibraryFolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            SettingsTabLibraryFolderButton.BackColor = Color.DimGray;
            SettingsTabLibraryFolderButton.FlatAppearance.BorderColor = Color.White;
            SettingsTabLibraryFolderButton.FlatAppearance.MouseDownBackColor = Color.Silver;
            SettingsTabLibraryFolderButton.FlatAppearance.MouseOverBackColor = Color.Gray;
            SettingsTabLibraryFolderButton.FlatStyle = FlatStyle.Flat;
            SettingsTabLibraryFolderButton.Location = new Point(298, 3);
            SettingsTabLibraryFolderButton.Name = "SettingsTabLibraryFolderButton";
            SettingsTabLibraryFolderButton.Size = new Size(40, 26);
            SettingsTabLibraryFolderButton.TabIndex = 3;
            SettingsTabLibraryFolderButton.Text = ". . .";
            SettingsTabLibraryFolderButton.UseVisualStyleBackColor = false;
            // 
            // SettingsTabLibraryUnixHiddenFileLabel
            // 
            SettingsTabLibraryUnixHiddenFileLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabLibraryUnixHiddenFileLabel.AutoSize = true;
            SettingsTabLibraryUnixHiddenFileLabel.Location = new Point(3, 38);
            SettingsTabLibraryUnixHiddenFileLabel.Name = "SettingsTabLibraryUnixHiddenFileLabel";
            SettingsTabLibraryUnixHiddenFileLabel.Size = new Size(262, 20);
            SettingsTabLibraryUnixHiddenFileLabel.TabIndex = 0;
            SettingsTabLibraryUnixHiddenFileLabel.Text = "Show hidden files style Unix";
            // 
            // SettingsTabLibraryUnixHiddenFileComboBox
            // 
            SettingsTabLibraryUnixHiddenFileComboBox.BackColor = Color.Gray;
            SettingsTabLibraryUnixHiddenFileComboBox.DisplayMember = "English";
            SettingsTabLibraryUnixHiddenFileComboBox.Dock = DockStyle.Fill;
            SettingsTabLibraryUnixHiddenFileComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabLibraryUnixHiddenFileComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabLibraryUnixHiddenFileComboBox.ForeColor = Color.Black;
            SettingsTabLibraryUnixHiddenFileComboBox.FormattingEnabled = true;
            SettingsTabLibraryUnixHiddenFileComboBox.Items.AddRange(new object[] { "No", "Yes" });
            SettingsTabLibraryUnixHiddenFileComboBox.Location = new Point(271, 36);
            SettingsTabLibraryUnixHiddenFileComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabLibraryUnixHiddenFileComboBox.MaxDropDownItems = 2;
            SettingsTabLibraryUnixHiddenFileComboBox.Name = "SettingsTabLibraryUnixHiddenFileComboBox";
            SettingsTabLibraryUnixHiddenFileComboBox.Size = new Size(94, 28);
            SettingsTabLibraryUnixHiddenFileComboBox.TabIndex = 1;
            SettingsTabLibraryUnixHiddenFileComboBox.ValueMember = "English";
            // 
            // SettingsTabLibraryWindowsHiddenFileLabel
            // 
            SettingsTabLibraryWindowsHiddenFileLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabLibraryWindowsHiddenFileLabel.AutoSize = true;
            SettingsTabLibraryWindowsHiddenFileLabel.Location = new Point(3, 70);
            SettingsTabLibraryWindowsHiddenFileLabel.Name = "SettingsTabLibraryWindowsHiddenFileLabel";
            SettingsTabLibraryWindowsHiddenFileLabel.Size = new Size(262, 20);
            SettingsTabLibraryWindowsHiddenFileLabel.TabIndex = 0;
            SettingsTabLibraryWindowsHiddenFileLabel.Text = "Show hidden files style Windows";
            // 
            // SettingsTabLibraryWindowsHiddenFileComboBox
            // 
            SettingsTabLibraryWindowsHiddenFileComboBox.BackColor = Color.Gray;
            SettingsTabLibraryWindowsHiddenFileComboBox.DisplayMember = "English";
            SettingsTabLibraryWindowsHiddenFileComboBox.Dock = DockStyle.Fill;
            SettingsTabLibraryWindowsHiddenFileComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabLibraryWindowsHiddenFileComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabLibraryWindowsHiddenFileComboBox.ForeColor = Color.Black;
            SettingsTabLibraryWindowsHiddenFileComboBox.FormattingEnabled = true;
            SettingsTabLibraryWindowsHiddenFileComboBox.Items.AddRange(new object[] { "No", "Yes" });
            SettingsTabLibraryWindowsHiddenFileComboBox.Location = new Point(271, 68);
            SettingsTabLibraryWindowsHiddenFileComboBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabLibraryWindowsHiddenFileComboBox.MaxDropDownItems = 2;
            SettingsTabLibraryWindowsHiddenFileComboBox.Name = "SettingsTabLibraryWindowsHiddenFileComboBox";
            SettingsTabLibraryWindowsHiddenFileComboBox.Size = new Size(94, 28);
            SettingsTabLibraryWindowsHiddenFileComboBox.TabIndex = 2;
            SettingsTabLibraryWindowsHiddenFileComboBox.ValueMember = "English";
            // 
            // SettingsTabEqualizerGroupBox
            // 
            SettingsTabEqualizerGroupBox.AutoSize = true;
            SettingsTabEqualizerGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabEqualizerGroupBox.Controls.Add(SettingsTabEqualizerTableLayoutPanel);
            SettingsTabEqualizerGroupBox.ForeColor = Color.White;
            SettingsTabEqualizerGroupBox.Location = new Point(3, 329);
            SettingsTabEqualizerGroupBox.Margin = new Padding(3, 4, 3, 4);
            SettingsTabEqualizerGroupBox.MinimumSize = new Size(600, 100);
            SettingsTabEqualizerGroupBox.Name = "SettingsTabEqualizerGroupBox";
            SettingsTabEqualizerGroupBox.Padding = new Padding(6, 7, 6, 7);
            SettingsTabEqualizerGroupBox.Size = new Size(600, 284);
            SettingsTabEqualizerGroupBox.TabIndex = 1;
            SettingsTabEqualizerGroupBox.TabStop = false;
            SettingsTabEqualizerGroupBox.Text = "Library";
            // 
            // SettingsTabEqualizerTableLayoutPanel
            // 
            SettingsTabEqualizerTableLayoutPanel.ColumnCount = 2;
            SettingsTabEqualizerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            SettingsTabEqualizerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SettingsTabEqualizerTableLayoutPanel.Controls.Add(SettingsTabEqualizerComboBox, 0, 0);
            SettingsTabEqualizerTableLayoutPanel.Controls.Add(SettingsTabEqualizerButton, 1, 0);
            SettingsTabEqualizerTableLayoutPanel.Controls.Add(SettingsTabEqualizerTableLayoutPanel2, 0, 1);
            SettingsTabEqualizerTableLayoutPanel.Dock = DockStyle.Top;
            SettingsTabEqualizerTableLayoutPanel.Location = new Point(6, 27);
            SettingsTabEqualizerTableLayoutPanel.Name = "SettingsTabEqualizerTableLayoutPanel";
            SettingsTabEqualizerTableLayoutPanel.RowCount = 2;
            SettingsTabEqualizerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            SettingsTabEqualizerTableLayoutPanel.RowStyles.Add(new RowStyle());
            SettingsTabEqualizerTableLayoutPanel.Size = new Size(588, 250);
            SettingsTabEqualizerTableLayoutPanel.TabIndex = 0;
            // 
            // SettingsTabEqualizerComboBox
            // 
            SettingsTabEqualizerComboBox.BackColor = Color.Gray;
            SettingsTabEqualizerComboBox.DisplayMember = "English";
            SettingsTabEqualizerComboBox.Dock = DockStyle.Fill;
            SettingsTabEqualizerComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabEqualizerComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabEqualizerComboBox.ForeColor = Color.Black;
            SettingsTabEqualizerComboBox.FormattingEnabled = true;
            SettingsTabEqualizerComboBox.Location = new Point(3, 2);
            SettingsTabEqualizerComboBox.Margin = new Padding(3, 2, 3, 3);
            SettingsTabEqualizerComboBox.MaxDropDownItems = 10;
            SettingsTabEqualizerComboBox.Name = "SettingsTabEqualizerComboBox";
            SettingsTabEqualizerComboBox.Size = new Size(194, 28);
            SettingsTabEqualizerComboBox.TabIndex = 4;
            SettingsTabEqualizerComboBox.ValueMember = "English";
            // 
            // SettingsTabEqualizerButton
            // 
            SettingsTabEqualizerButton.AutoSize = true;
            SettingsTabEqualizerButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabEqualizerButton.BackColor = Color.DimGray;
            SettingsTabEqualizerButton.FlatAppearance.BorderColor = Color.White;
            SettingsTabEqualizerButton.FlatAppearance.MouseDownBackColor = Color.Silver;
            SettingsTabEqualizerButton.FlatAppearance.MouseOverBackColor = Color.Gray;
            SettingsTabEqualizerButton.FlatStyle = FlatStyle.Flat;
            SettingsTabEqualizerButton.Location = new Point(200, 0);
            SettingsTabEqualizerButton.Margin = new Padding(0);
            SettingsTabEqualizerButton.MinimumSize = new Size(150, 0);
            SettingsTabEqualizerButton.Name = "SettingsTabEqualizerButton";
            SettingsTabEqualizerButton.Size = new Size(150, 32);
            SettingsTabEqualizerButton.TabIndex = 3;
            SettingsTabEqualizerButton.Text = "Renitialise";
            SettingsTabEqualizerButton.UseVisualStyleBackColor = false;
            // 
            // SettingsTabEqualizerTableLayoutPanel2
            // 
            SettingsTabEqualizerTableLayoutPanel2.ColumnCount = 10;
            SettingsTabEqualizerTableLayoutPanel.SetColumnSpan(SettingsTabEqualizerTableLayoutPanel2, 2);
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar01, 0, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar02, 1, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar03, 2, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar04, 3, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar05, 4, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar06, 5, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar07, 6, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar08, 7, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar09, 8, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerTrackBar10, 9, 0);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel01, 0, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel02, 1, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel03, 2, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel04, 3, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel05, 4, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel06, 5, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel07, 6, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel08, 7, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel09, 8, 1);
            SettingsTabEqualizerTableLayoutPanel2.Controls.Add(SettingsTabEqualizerLabel10, 9, 1);
            SettingsTabEqualizerTableLayoutPanel2.Dock = DockStyle.Fill;
            SettingsTabEqualizerTableLayoutPanel2.Location = new Point(3, 35);
            SettingsTabEqualizerTableLayoutPanel2.Name = "SettingsTabEqualizerTableLayoutPanel2";
            SettingsTabEqualizerTableLayoutPanel2.RowCount = 2;
            SettingsTabEqualizerTableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            SettingsTabEqualizerTableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            SettingsTabEqualizerTableLayoutPanel2.Size = new Size(582, 212);
            SettingsTabEqualizerTableLayoutPanel2.TabIndex = 5;
            // 
            // SettingsTabEqualizerTrackBar01
            // 
            SettingsTabEqualizerTrackBar01.Location = new Point(3, 3);
            SettingsTabEqualizerTrackBar01.Maximum = 600;
            SettingsTabEqualizerTrackBar01.Minimum = -600;
            SettingsTabEqualizerTrackBar01.Name = "SettingsTabEqualizerTrackBar01";
            SettingsTabEqualizerTrackBar01.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar01.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar01.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar01.TabIndex = 0;
            SettingsTabEqualizerTrackBar01.TickFrequency = 100;
            SettingsTabEqualizerTrackBar01.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar02
            // 
            SettingsTabEqualizerTrackBar02.Location = new Point(61, 3);
            SettingsTabEqualizerTrackBar02.Maximum = 600;
            SettingsTabEqualizerTrackBar02.Minimum = -600;
            SettingsTabEqualizerTrackBar02.Name = "SettingsTabEqualizerTrackBar02";
            SettingsTabEqualizerTrackBar02.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar02.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar02.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar02.TabIndex = 0;
            SettingsTabEqualizerTrackBar02.TickFrequency = 100;
            SettingsTabEqualizerTrackBar02.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar03
            // 
            SettingsTabEqualizerTrackBar03.Location = new Point(119, 3);
            SettingsTabEqualizerTrackBar03.Maximum = 600;
            SettingsTabEqualizerTrackBar03.Minimum = -600;
            SettingsTabEqualizerTrackBar03.Name = "SettingsTabEqualizerTrackBar03";
            SettingsTabEqualizerTrackBar03.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar03.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar03.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar03.TabIndex = 0;
            SettingsTabEqualizerTrackBar03.TickFrequency = 100;
            SettingsTabEqualizerTrackBar03.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar04
            // 
            SettingsTabEqualizerTrackBar04.Location = new Point(177, 3);
            SettingsTabEqualizerTrackBar04.Maximum = 600;
            SettingsTabEqualizerTrackBar04.Minimum = -600;
            SettingsTabEqualizerTrackBar04.Name = "SettingsTabEqualizerTrackBar04";
            SettingsTabEqualizerTrackBar04.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar04.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar04.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar04.TabIndex = 0;
            SettingsTabEqualizerTrackBar04.TickFrequency = 100;
            SettingsTabEqualizerTrackBar04.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar05
            // 
            SettingsTabEqualizerTrackBar05.Location = new Point(235, 3);
            SettingsTabEqualizerTrackBar05.Maximum = 600;
            SettingsTabEqualizerTrackBar05.Minimum = -600;
            SettingsTabEqualizerTrackBar05.Name = "SettingsTabEqualizerTrackBar05";
            SettingsTabEqualizerTrackBar05.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar05.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar05.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar05.TabIndex = 0;
            SettingsTabEqualizerTrackBar05.TickFrequency = 100;
            SettingsTabEqualizerTrackBar05.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar06
            // 
            SettingsTabEqualizerTrackBar06.Location = new Point(293, 3);
            SettingsTabEqualizerTrackBar06.Maximum = 600;
            SettingsTabEqualizerTrackBar06.Minimum = -600;
            SettingsTabEqualizerTrackBar06.Name = "SettingsTabEqualizerTrackBar06";
            SettingsTabEqualizerTrackBar06.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar06.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar06.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar06.TabIndex = 0;
            SettingsTabEqualizerTrackBar06.TickFrequency = 100;
            SettingsTabEqualizerTrackBar06.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar07
            // 
            SettingsTabEqualizerTrackBar07.Location = new Point(351, 3);
            SettingsTabEqualizerTrackBar07.Maximum = 600;
            SettingsTabEqualizerTrackBar07.Minimum = -600;
            SettingsTabEqualizerTrackBar07.Name = "SettingsTabEqualizerTrackBar07";
            SettingsTabEqualizerTrackBar07.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar07.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar07.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar07.TabIndex = 0;
            SettingsTabEqualizerTrackBar07.TickFrequency = 100;
            SettingsTabEqualizerTrackBar07.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar08
            // 
            SettingsTabEqualizerTrackBar08.Location = new Point(409, 3);
            SettingsTabEqualizerTrackBar08.Maximum = 600;
            SettingsTabEqualizerTrackBar08.Minimum = -600;
            SettingsTabEqualizerTrackBar08.Name = "SettingsTabEqualizerTrackBar08";
            SettingsTabEqualizerTrackBar08.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar08.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar08.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar08.TabIndex = 0;
            SettingsTabEqualizerTrackBar08.TickFrequency = 100;
            SettingsTabEqualizerTrackBar08.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar09
            // 
            SettingsTabEqualizerTrackBar09.Location = new Point(467, 3);
            SettingsTabEqualizerTrackBar09.Maximum = 600;
            SettingsTabEqualizerTrackBar09.Minimum = -600;
            SettingsTabEqualizerTrackBar09.Name = "SettingsTabEqualizerTrackBar09";
            SettingsTabEqualizerTrackBar09.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar09.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar09.Size = new Size(52, 166);
            SettingsTabEqualizerTrackBar09.TabIndex = 0;
            SettingsTabEqualizerTrackBar09.TickFrequency = 100;
            SettingsTabEqualizerTrackBar09.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar10
            // 
            SettingsTabEqualizerTrackBar10.Location = new Point(525, 3);
            SettingsTabEqualizerTrackBar10.Maximum = 600;
            SettingsTabEqualizerTrackBar10.Minimum = -600;
            SettingsTabEqualizerTrackBar10.Name = "SettingsTabEqualizerTrackBar10";
            SettingsTabEqualizerTrackBar10.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar10.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar10.Size = new Size(54, 166);
            SettingsTabEqualizerTrackBar10.TabIndex = 0;
            SettingsTabEqualizerTrackBar10.TickFrequency = 100;
            SettingsTabEqualizerTrackBar10.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerLabel01
            // 
            SettingsTabEqualizerLabel01.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel01.AutoSize = true;
            SettingsTabEqualizerLabel01.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel01.Location = new Point(3, 182);
            SettingsTabEqualizerLabel01.Name = "SettingsTabEqualizerLabel01";
            SettingsTabEqualizerLabel01.Size = new Size(52, 20);
            SettingsTabEqualizerLabel01.TabIndex = 1;
            SettingsTabEqualizerLabel01.Text = "-00.00";
            SettingsTabEqualizerLabel01.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel02
            // 
            SettingsTabEqualizerLabel02.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel02.AutoSize = true;
            SettingsTabEqualizerLabel02.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel02.Location = new Point(61, 182);
            SettingsTabEqualizerLabel02.Name = "SettingsTabEqualizerLabel02";
            SettingsTabEqualizerLabel02.Size = new Size(52, 20);
            SettingsTabEqualizerLabel02.TabIndex = 1;
            SettingsTabEqualizerLabel02.Text = "-00.00";
            SettingsTabEqualizerLabel02.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel03
            // 
            SettingsTabEqualizerLabel03.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel03.AutoSize = true;
            SettingsTabEqualizerLabel03.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel03.Location = new Point(119, 182);
            SettingsTabEqualizerLabel03.Name = "SettingsTabEqualizerLabel03";
            SettingsTabEqualizerLabel03.Size = new Size(52, 20);
            SettingsTabEqualizerLabel03.TabIndex = 1;
            SettingsTabEqualizerLabel03.Text = "-00.00";
            SettingsTabEqualizerLabel03.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel04
            // 
            SettingsTabEqualizerLabel04.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel04.AutoSize = true;
            SettingsTabEqualizerLabel04.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel04.Location = new Point(177, 182);
            SettingsTabEqualizerLabel04.Name = "SettingsTabEqualizerLabel04";
            SettingsTabEqualizerLabel04.Size = new Size(52, 20);
            SettingsTabEqualizerLabel04.TabIndex = 1;
            SettingsTabEqualizerLabel04.Text = "-00.00";
            SettingsTabEqualizerLabel04.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel05
            // 
            SettingsTabEqualizerLabel05.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel05.AutoSize = true;
            SettingsTabEqualizerLabel05.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel05.Location = new Point(235, 182);
            SettingsTabEqualizerLabel05.Name = "SettingsTabEqualizerLabel05";
            SettingsTabEqualizerLabel05.Size = new Size(52, 20);
            SettingsTabEqualizerLabel05.TabIndex = 1;
            SettingsTabEqualizerLabel05.Text = "-00.00";
            SettingsTabEqualizerLabel05.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel06
            // 
            SettingsTabEqualizerLabel06.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel06.AutoSize = true;
            SettingsTabEqualizerLabel06.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel06.Location = new Point(293, 182);
            SettingsTabEqualizerLabel06.Name = "SettingsTabEqualizerLabel06";
            SettingsTabEqualizerLabel06.Size = new Size(52, 20);
            SettingsTabEqualizerLabel06.TabIndex = 1;
            SettingsTabEqualizerLabel06.Text = "-00.00";
            SettingsTabEqualizerLabel06.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel07
            // 
            SettingsTabEqualizerLabel07.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel07.AutoSize = true;
            SettingsTabEqualizerLabel07.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel07.Location = new Point(351, 182);
            SettingsTabEqualizerLabel07.Name = "SettingsTabEqualizerLabel07";
            SettingsTabEqualizerLabel07.Size = new Size(52, 20);
            SettingsTabEqualizerLabel07.TabIndex = 1;
            SettingsTabEqualizerLabel07.Text = "-00.00";
            SettingsTabEqualizerLabel07.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel08
            // 
            SettingsTabEqualizerLabel08.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel08.AutoSize = true;
            SettingsTabEqualizerLabel08.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel08.Location = new Point(409, 182);
            SettingsTabEqualizerLabel08.Name = "SettingsTabEqualizerLabel08";
            SettingsTabEqualizerLabel08.Size = new Size(52, 20);
            SettingsTabEqualizerLabel08.TabIndex = 1;
            SettingsTabEqualizerLabel08.Text = "-00.00";
            SettingsTabEqualizerLabel08.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel09
            // 
            SettingsTabEqualizerLabel09.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel09.AutoSize = true;
            SettingsTabEqualizerLabel09.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel09.Location = new Point(467, 182);
            SettingsTabEqualizerLabel09.Name = "SettingsTabEqualizerLabel09";
            SettingsTabEqualizerLabel09.Size = new Size(52, 20);
            SettingsTabEqualizerLabel09.TabIndex = 1;
            SettingsTabEqualizerLabel09.Text = "-00.00";
            SettingsTabEqualizerLabel09.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel10
            // 
            SettingsTabEqualizerLabel10.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel10.AutoSize = true;
            SettingsTabEqualizerLabel10.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            SettingsTabEqualizerLabel10.Location = new Point(525, 182);
            SettingsTabEqualizerLabel10.Name = "SettingsTabEqualizerLabel10";
            SettingsTabEqualizerLabel10.Size = new Size(54, 20);
            SettingsTabEqualizerLabel10.TabIndex = 1;
            SettingsTabEqualizerLabel10.Text = "-00.00";
            SettingsTabEqualizerLabel10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LibibraryNavigationPathContener
            // 
            LibibraryNavigationPathContener.Dock = DockStyle.Fill;
            LibibraryNavigationPathContener.Location = new Point(4, 4);
            LibibraryNavigationPathContener.Name = "LibibraryNavigationPathContener";
            LibibraryNavigationPathContener.Size = new Size(849, 34);
            LibibraryNavigationPathContener.TabIndex = 0;
            // 
            // LibraryFiltersGrid
            // 
            LibraryFiltersGrid.ColumnCount = 7;
            LibraryFiltersGrid.ColumnStyles.Add(new ColumnStyle());
            LibraryFiltersGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            LibraryFiltersGrid.ColumnStyles.Add(new ColumnStyle());
            LibraryFiltersGrid.ColumnStyles.Add(new ColumnStyle());
            LibraryFiltersGrid.ColumnStyles.Add(new ColumnStyle());
            LibraryFiltersGrid.ColumnStyles.Add(new ColumnStyle());
            LibraryFiltersGrid.ColumnStyles.Add(new ColumnStyle());
            LibraryFiltersGrid.Controls.Add(LibraryFiltersModeLabel, 0, 0);
            LibraryFiltersGrid.Controls.Add(LibraryFiltersMode, 1, 0);
            LibraryFiltersGrid.Controls.Add(LibraryFiltersGenreList, 2, 0);
            LibraryFiltersGrid.Controls.Add(LibraryFiltersGenreSearchBox, 3, 0);
            LibraryFiltersGrid.Controls.Add(LibraryFiltersSearchBox, 4, 0);
            LibraryFiltersGrid.Dock = DockStyle.Fill;
            LibraryFiltersGrid.Location = new Point(4, 45);
            LibraryFiltersGrid.Name = "LibraryFiltersGrid";
            LibraryFiltersGrid.RowCount = 1;
            LibraryFiltersGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            LibraryFiltersGrid.Size = new Size(849, 34);
            LibraryFiltersGrid.TabIndex = 1;
            // 
            // LibraryFiltersModeLabel
            // 
            LibraryFiltersModeLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            LibraryFiltersModeLabel.AutoSize = true;
            LibraryFiltersModeLabel.Location = new Point(3, 7);
            LibraryFiltersModeLabel.Margin = new Padding(3, 0, 10, 0);
            LibraryFiltersModeLabel.Name = "LibraryFiltersModeLabel";
            LibraryFiltersModeLabel.Size = new Size(50, 20);
            LibraryFiltersModeLabel.TabIndex = 0;
            LibraryFiltersModeLabel.Text = "label1";
            // 
            // LibraryFiltersMode
            // 
            LibraryFiltersMode.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            LibraryFiltersMode.BackColor = Color.DimGray;
            LibraryFiltersMode.DropDownStyle = ComboBoxStyle.DropDownList;
            LibraryFiltersMode.FlatStyle = FlatStyle.Flat;
            LibraryFiltersMode.ForeColor = Color.White;
            LibraryFiltersMode.FormattingEnabled = true;
            LibraryFiltersMode.Items.AddRange(new object[] { "Nothing", "Title", "Artist", "Album", "Genre" });
            LibraryFiltersMode.Location = new Point(66, 3);
            LibraryFiltersMode.Name = "LibraryFiltersMode";
            LibraryFiltersMode.Size = new Size(144, 28);
            LibraryFiltersMode.TabIndex = 1;
            // 
            // LibraryFiltersGenreList
            // 
            LibraryFiltersGenreList.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            LibraryFiltersGenreList.BackColor = Color.DimGray;
            LibraryFiltersGenreList.DropDownStyle = ComboBoxStyle.DropDownList;
            LibraryFiltersGenreList.FlatStyle = FlatStyle.Flat;
            LibraryFiltersGenreList.ForeColor = Color.White;
            LibraryFiltersGenreList.FormattingEnabled = true;
            LibraryFiltersGenreList.Items.AddRange(new object[] { "Nothing", "Title", "Artist", "Album", "Genre" });
            LibraryFiltersGenreList.Location = new Point(216, 3);
            LibraryFiltersGenreList.Name = "LibraryFiltersGenreList";
            LibraryFiltersGenreList.Size = new Size(144, 28);
            LibraryFiltersGenreList.TabIndex = 1;
            // 
            // LibraryFiltersGenreSearchBox
            // 
            LibraryFiltersGenreSearchBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            LibraryFiltersGenreSearchBox.BackColor = Color.DimGray;
            LibraryFiltersGenreSearchBox.BorderStyle = BorderStyle.FixedSingle;
            LibraryFiltersGenreSearchBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            LibraryFiltersGenreSearchBox.ForeColor = Color.White;
            LibraryFiltersGenreSearchBox.Location = new Point(363, 2);
            LibraryFiltersGenreSearchBox.Margin = new Padding(0, 0, 0, 7);
            LibraryFiltersGenreSearchBox.Name = "LibraryFiltersGenreSearchBox";
            LibraryFiltersGenreSearchBox.Size = new Size(125, 30);
            LibraryFiltersGenreSearchBox.TabIndex = 3;
            LibraryFiltersGenreSearchBox.Text = "Search Genre";
            // 
            // LibraryFiltersSearchBox
            // 
            LibraryFiltersSearchBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            LibraryFiltersSearchBox.BackColor = Color.DimGray;
            LibraryFiltersSearchBox.BorderStyle = BorderStyle.FixedSingle;
            LibraryFiltersSearchBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            LibraryFiltersSearchBox.ForeColor = Color.White;
            LibraryFiltersSearchBox.Location = new Point(491, 2);
            LibraryFiltersSearchBox.Margin = new Padding(3, 2, 0, 7);
            LibraryFiltersSearchBox.Name = "LibraryFiltersSearchBox";
            LibraryFiltersSearchBox.Size = new Size(125, 30);
            LibraryFiltersSearchBox.TabIndex = 3;
            LibraryFiltersSearchBox.Text = "Search";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(4, 86);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(LibibraryNavigationContent);
            splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(LibibrarySearchContent);
            splitContainer1.Panel2MinSize = 100;
            splitContainer1.Size = new Size(849, 425);
            splitContainer1.SplitterDistance = 221;
            splitContainer1.SplitterWidth = 1;
            splitContainer1.TabIndex = 3;
            // 
            // LibibraryNavigationContent
            // 
            LibibraryNavigationContent.AutoScroll = true;
            LibibraryNavigationContent.Dock = DockStyle.Fill;
            LibibraryNavigationContent.FlowDirection = FlowDirection.TopDown;
            LibibraryNavigationContent.Location = new Point(0, 0);
            LibibraryNavigationContent.Margin = new Padding(0);
            LibibraryNavigationContent.Name = "LibibraryNavigationContent";
            LibibraryNavigationContent.Size = new Size(849, 221);
            LibibraryNavigationContent.TabIndex = 1;
            LibibraryNavigationContent.WrapContents = false;
            // 
            // LibibrarySearchContent
            // 
            LibibrarySearchContent.AutoScroll = true;
            LibibrarySearchContent.Dock = DockStyle.Fill;
            LibibrarySearchContent.FlowDirection = FlowDirection.TopDown;
            LibibrarySearchContent.Location = new Point(0, 0);
            LibibrarySearchContent.Margin = new Padding(0);
            LibibrarySearchContent.Name = "LibibrarySearchContent";
            LibibrarySearchContent.Size = new Size(849, 203);
            LibibrarySearchContent.TabIndex = 1;
            LibibrarySearchContent.WrapContents = false;
            // 
            // MainWindow2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            ClientSize = new Size(923, 681);
            ControlBox = false;
            Controls.Add(GlobalTableLayoutPanel);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(629, 600);
            Name = "MainWindow2";
            Padding = new Padding(1);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainWindow";
            SizeChanged += MainWindow2_SizeChanged;
            GlobalTableLayoutPanel.ResumeLayout(false);
            MainWIndowHead.ResumeLayout(false);
            MainWIndowHead.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            TabControler.ResumeLayout(false);
            PlaybackTab.ResumeLayout(false);
            PlaybackTabMainTableLayoutPanel.ResumeLayout(false);
            PlaybackTabLeftTableLayoutPanel.ResumeLayout(false);
            PlaybackTabLeftBottomPanel.ResumeLayout(false);
            PlaybackTabLeftBottomPanel.PerformLayout();
            PlaybackTabLeftBottomFlowLayoutPanel.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PlaybackTabDataGridView).EndInit();
            LibraryTab.ResumeLayout(false);
            LibraryTabTableLayoutPanel.ResumeLayout(false);
            PlayListsTab.ResumeLayout(false);
            SettingsTab.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            SettingsTabLangGroupBox.ResumeLayout(false);
            SettingsTabStyleGroupBox.ResumeLayout(false);
            SettingsTabAutoPlayGroupBox.ResumeLayout(false);
            SettingsTabAlwaysOnTopGroupBox.ResumeLayout(false);
            SettingsTabAutoCloseLyricsGroupBox.ResumeLayout(false);
            SettingsTabConvGroupBox.ResumeLayout(false);
            SettingsTabConvTableLayoutPanel.ResumeLayout(false);
            SettingsTabConvTableLayoutPanel.PerformLayout();
            SettingsTabLibraryGroupBox.ResumeLayout(false);
            SettingsTabLibraryTableLayoutPanel.ResumeLayout(false);
            SettingsTabLibraryTableLayoutPanel.PerformLayout();
            SettingsTabEqualizerGroupBox.ResumeLayout(false);
            SettingsTabEqualizerTableLayoutPanel.ResumeLayout(false);
            SettingsTabEqualizerTableLayoutPanel.PerformLayout();
            SettingsTabEqualizerTableLayoutPanel2.ResumeLayout(false);
            SettingsTabEqualizerTableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar01).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar02).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar03).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar04).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar05).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar06).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar07).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar08).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar09).EndInit();
            ((System.ComponentModel.ISupportInitialize)SettingsTabEqualizerTrackBar10).EndInit();
            LibraryFiltersGrid.ResumeLayout(false);
            LibraryFiltersGrid.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel MainWIndowHead;
        private Label TitleLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel1;
        private Button GripButton;
        private TableLayoutPanel PlaybackTabMainTableLayoutPanel;
        private TableLayoutPanel LibraryTabTableLayoutPanel;
        private TableLayoutPanel TableLayoutPanel;
        private Panel panel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel PlaybackTabLeftTableLayoutPanel;
        private Panel PlaybackTabLeftBottomPanel;
        private FlowLayoutPanel PlaybackTabLeftBottomFlowLayoutPanel;
        private Label PlaybackTabTitleLabelValue;
        private Label PlaybackTabAlbumLabelValue;
        private Label PlaybackTabArtistsLabelValue;
        private Button FileCover;
        private TableLayoutPanel tableLayoutPanel4;
        private Button button1;
        private Label PlaybackTabDurationLabelValue;
        private Ratting2 PlaybackTabRatting;
        private DataGridViewTextBoxColumn SelectedColumn;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn DurationColumn;
        private DataGridViewTextBoxColumn ArtistsColumn;
        private DataGridViewTextBoxColumn AlbumColumn;
        private TableLayoutPanel SettingsTabConvTableLayoutPanel;
        private TableLayoutPanel SettingsTabLibraryTableLayoutPanel;
        private TableLayoutPanel SettingsTabEqualizerTableLayoutPanel;
        private TableLayoutPanel SettingsTabEqualizerTableLayoutPanel2;
        internal TableLayoutPanel GlobalTableLayoutPanel;
        internal Button MinimizeButton;
        internal Button CloseButton;
        internal Button MaximizeButton;
        internal Button BtnOpen;
        internal Button BtnPrevious;
        internal Button BtnPlayPause;
        internal Button BtnNext;
        internal Button BtnRepeat;
        internal Button BtnShuffle;
        internal Button BtnClearList;
        internal Label DisplayPlaybackPosition;
        internal Label DisplayPlaybackSize;
        internal PlaybackProgressBar playbackProgressBar;
        internal Manina.Windows.Forms.TabControl TabControler;
        internal GroupBox SettingsTabLangGroupBox;
        internal ComboBox SettingsTabLangComboBox;
        internal Label PlaybackTabTitleLabelInfo;
        internal Label PlaybackTabAlbumLabelInfo;
        internal Label PlaybackTabArtistsLabelInfo;
        internal DataGridView PlaybackTabDataGridView;
        internal Label PlaybackTabDurationLabelInfo;
        internal Button PlaybackTabLyricsButton;
        internal GroupBox SettingsTabStyleGroupBox;
        internal ComboBox SettingsTabStyleComboBox;
        internal GroupBox SettingsTabAutoPlayGroupBox;
        internal ComboBox SettingsTabAutoPlayComboBox;
        internal GroupBox SettingsTabAlwaysOnTopGroupBox;
        internal ComboBox SettingsTabAlwaysOnTopComboBox;
        internal GroupBox SettingsTabConvGroupBox;
        internal Label SettingsTabConvModeLabel;
        internal ComboBox SettingsTabConvModeComboBox;
        internal Label SettingsTabConvQualityLabel;
        internal ComboBox SettingsTabConvQualityComboBox;
        internal GroupBox SettingsTabLibraryGroupBox;
        internal Label SettingsTabLibraryUnixHiddenFileLabel;
        internal ComboBox SettingsTabLibraryUnixHiddenFileComboBox;
        internal Label SettingsTabLibraryWindowsHiddenFileLabel;
        internal ComboBox SettingsTabLibraryWindowsHiddenFileComboBox;
        internal Button SettingsTabLibraryFolderButton;
        internal TextBox SettingsTabLibraryFolderTextBox;
        internal GroupBox SettingsTabEqualizerGroupBox;
        internal ComboBox SettingsTabEqualizerComboBox;
        internal Button SettingsTabEqualizerButton;
        internal Manina.Windows.Forms.Tab PlaybackTab;
        internal Manina.Windows.Forms.Tab LibraryTab;
        internal Manina.Windows.Forms.Tab PlayListsTab;
        internal Manina.Windows.Forms.Tab SettingsTab;
        internal TrackBar SettingsTabEqualizerTrackBar01;
        internal TrackBar SettingsTabEqualizerTrackBar02;
        internal TrackBar SettingsTabEqualizerTrackBar03;
        internal TrackBar SettingsTabEqualizerTrackBar04;
        internal TrackBar SettingsTabEqualizerTrackBar05;
        internal TrackBar SettingsTabEqualizerTrackBar06;
        internal TrackBar SettingsTabEqualizerTrackBar07;
        internal TrackBar SettingsTabEqualizerTrackBar08;
        internal TrackBar SettingsTabEqualizerTrackBar09;
        internal TrackBar SettingsTabEqualizerTrackBar10;
        internal Label SettingsTabEqualizerLabel01;
        internal Label SettingsTabEqualizerLabel02;
        internal Label SettingsTabEqualizerLabel03;
        internal Label SettingsTabEqualizerLabel04;
        internal Label SettingsTabEqualizerLabel05;
        internal Label SettingsTabEqualizerLabel06;
        internal Label SettingsTabEqualizerLabel07;
        internal Label SettingsTabEqualizerLabel08;
        internal Label SettingsTabEqualizerLabel09;
        internal Label SettingsTabEqualizerLabel10;
        internal GroupBox SettingsTabAutoCloseLyricsGroupBox;
        internal ComboBox SettingsTabAutoCloseLyricsComboBox;
        internal Label PlaybackPositionLabel;
        private FlowLayoutPanel LibibraryNavigationPathContener;
        private TableLayoutPanel LibraryFiltersGrid;
        private Panel LibibraryNavigationContentScroll;
        private Label LibraryFiltersModeLabel;
        private ComboBox LibraryFiltersMode;
        private ComboBox LibraryFiltersGenreList;
        private TextBox LibraryFiltersGenreSearchBox;
        private TextBox LibraryFiltersSearchBox;
        private FlowLayoutPanel LibibraryNavigationContent;
        private FlowLayoutPanel LibibrarySearchContent;
        private SplitContainer splitContainer1;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel LibibraryNavigationContent;
    }
}