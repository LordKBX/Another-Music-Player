using System.Drawing;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2Space
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            TreeNode treeNode1 = new TreeNode("Automatic");
            TreeNode treeNode2 = new TreeNode("Recorded");
            TreeNode treeNode3 = new TreeNode("Web Radio");
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
            panel3 = new Panel();
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
            PlaybackTabGenresLabelInfo = new Label();
            PlaybackTabGenresLabelValue = new Label();
            PlaybackTabDurationLabelInfo = new Label();
            PlaybackTabDurationLabelValue = new Label();
            PlaybackTabRatting = new Rating2();
            PlaybackTabLyricsButton = new Button();
            FileCover = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            PlaybackPositionLabel = new Label();
            PlaybackTabDataGridView = new DataGridView();
            SelectedColumn = new DataGridViewTextBoxColumn();
            NameColumn = new DataGridViewTextBoxColumn();
            DurationColumn = new DataGridViewTextBoxColumn();
            ArtistsColumn = new DataGridViewTextBoxColumn();
            AlbumColumn = new DataGridViewTextBoxColumn();
            LibraryTab = new Manina.Windows.Forms.Tab();
            LibraryTabTableLayoutPanel = new TableLayoutPanel();
            LibraryNavigationPathContener = new FlowLayoutPanel();
            LibraryFiltersGrid = new FlowLayoutPanel();
            LibraryFiltersModeLabel = new Label();
            LibraryFiltersMode = new ComboBox();
            LibraryFiltersGenreList = new ComboBox();
            LibraryFiltersSearchBox = new TextBox();
            LibraryTabSplitContainer = new SplitContainer();
            LibraryNavigationContent = new TableLayoutPanel();
            LibraryFoldersLabel = new Label();
            LibraryNavigationContentFolders = new DataGridView();
            LibraryTabSplitContainer2 = new SplitContainer();
            LibrarySearchContent = new DataGridView();
            LibraryNavigationContentFilesParent = new Panel();
            LibraryNavigationContentFiles = new TableLayoutPanel();
            PlayListsTab = new Manina.Windows.Forms.Tab();
            PlayListsTabTableLayoutPanel = new TableLayoutPanel();
            PlaylistsTree = new TreeView();
            PlayListsTabTreeImageList = new ImageList(components);
            PlayListsTabSplitContainer1 = new SplitContainer();
            tableLayoutPanel5 = new TableLayoutPanel();
            PlayListsTabVoidLabel = new Label();
            PlayListsTabSplitContainer2 = new SplitContainer();
            PlayListsTabDataGridView = new DataGridView();
            ColumnPlayCount = new DataGridViewTextBoxColumn();
            PlayListsColumnName = new DataGridViewTextBoxColumn();
            PlayListsColumnArtists = new DataGridViewTextBoxColumn();
            PlayListsColumnAlbum = new DataGridViewTextBoxColumn();
            PlayListsColumnDuration = new DataGridViewTextBoxColumn();
            PlayListsColumnRating = new DataGridViewImageColumn();
            PlayListsTabRadioPanel = new TableLayoutPanel();
            PlayListsTabRadioPanelLeft = new TableLayoutPanel();
            PlayListsTabRadioPanelIcon = new Button();
            PlayListsTabRadioButton = new Button();
            PlayListsTabRadioPanelFlowLayoutPanel = new FlowLayoutPanel();
            button2 = new Button();
            PlayListsTabRadioPanelRight = new TableLayoutPanel();
            PlayListsTabRadioPanelTitleLabel = new Label();
            PlayListsTabRadioPanelRightPanelDescriptionLabel = new Label();
            SettingsTab = new Manina.Windows.Forms.Tab();
            panel2 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            SettingsTabLangGroupBox = new GroupBox();
            SettingsTabLangComboBox = new ComboBox();
            SettingsTabStyleGroupBox = new GroupBox();
            SettingsTabStyleComboBox = new ComboBox();
            SettingsTabAutoPlayGroupBox = new GroupBox();
            SettingsTabAutoPlayComboBox = new ComboBox();
            SettingsNormalizeVolumeGroupBox = new GroupBox();
            SettingsNormalizeVolumeComboBox = new ComboBox();
            SettingsTabAlwaysOnTopGroupBox = new GroupBox();
            SettingsTabAlwaysOnTopComboBox = new ComboBox();
            SettingsTabAutoCloseLyricsGroupBox = new GroupBox();
            SettingsTabAutoCloseLyricsComboBox = new ComboBox();
            SettingsTabDisplayLiveLyricsGroupBox = new GroupBox();
            SettingsTabDisplayLiveLyricsComboBox = new ComboBox();
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
            tableLayoutPanel3 = new TableLayoutPanel();
            BtnScheduller = new Button();
            BtnOpen = new Button();
            BtnPrevious = new Button();
            BtnPlayPause = new Button();
            BtnNext = new Button();
            BtnRepeat = new Button();
            BtnShuffle = new Button();
            BtnClearList = new Button();
            GridScanMetadata = new TableLayoutPanel();
            textBox1 = new TextBox();
            pictureBox1 = new PictureBox();
            GridScanMetadataNb = new Label();
            LyricsTextBox = new Label();
            OverPanel = new Panel();
            OverPanelLabel = new Label();
            GlobalTableLayoutPanel.SuspendLayout();
            MainWIndowHead.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel3.SuspendLayout();
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
            LibraryFiltersGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LibraryTabSplitContainer).BeginInit();
            LibraryTabSplitContainer.Panel1.SuspendLayout();
            LibraryTabSplitContainer.Panel2.SuspendLayout();
            LibraryTabSplitContainer.SuspendLayout();
            LibraryNavigationContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LibraryNavigationContentFolders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LibraryTabSplitContainer2).BeginInit();
            LibraryTabSplitContainer2.Panel1.SuspendLayout();
            LibraryTabSplitContainer2.Panel2.SuspendLayout();
            LibraryTabSplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LibrarySearchContent).BeginInit();
            LibraryNavigationContentFilesParent.SuspendLayout();
            PlayListsTab.SuspendLayout();
            PlayListsTabTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PlayListsTabSplitContainer1).BeginInit();
            PlayListsTabSplitContainer1.Panel1.SuspendLayout();
            PlayListsTabSplitContainer1.Panel2.SuspendLayout();
            PlayListsTabSplitContainer1.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PlayListsTabSplitContainer2).BeginInit();
            PlayListsTabSplitContainer2.Panel1.SuspendLayout();
            PlayListsTabSplitContainer2.Panel2.SuspendLayout();
            PlayListsTabSplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PlayListsTabDataGridView).BeginInit();
            PlayListsTabRadioPanel.SuspendLayout();
            PlayListsTabRadioPanelLeft.SuspendLayout();
            PlayListsTabRadioPanelFlowLayoutPanel.SuspendLayout();
            PlayListsTabRadioPanelRight.SuspendLayout();
            SettingsTab.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SettingsTabLangGroupBox.SuspendLayout();
            SettingsTabStyleGroupBox.SuspendLayout();
            SettingsTabAutoPlayGroupBox.SuspendLayout();
            SettingsNormalizeVolumeGroupBox.SuspendLayout();
            SettingsTabAlwaysOnTopGroupBox.SuspendLayout();
            SettingsTabAutoCloseLyricsGroupBox.SuspendLayout();
            SettingsTabDisplayLiveLyricsGroupBox.SuspendLayout();
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
            tableLayoutPanel3.SuspendLayout();
            GridScanMetadata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            OverPanel.SuspendLayout();
            SuspendLayout();
            // 
            // GlobalTableLayoutPanel
            // 
            GlobalTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            GlobalTableLayoutPanel.ColumnCount = 1;
            GlobalTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            GlobalTableLayoutPanel.Controls.Add(MainWIndowHead, 0, 0);
            GlobalTableLayoutPanel.Controls.Add(tableLayoutPanel1, 0, 3);
            GlobalTableLayoutPanel.Controls.Add(tableLayoutPanel2, 0, 1);
            GlobalTableLayoutPanel.Controls.Add(LyricsTextBox, 0, 2);
            GlobalTableLayoutPanel.Dock = DockStyle.Fill;
            GlobalTableLayoutPanel.Location = new Point(1, 1);
            GlobalTableLayoutPanel.Margin = new Padding(0);
            GlobalTableLayoutPanel.Name = "GlobalTableLayoutPanel";
            GlobalTableLayoutPanel.Padding = new Padding(1);
            GlobalTableLayoutPanel.RowCount = 4;
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            GlobalTableLayoutPanel.Size = new Size(1236, 860);
            GlobalTableLayoutPanel.TabIndex = 0;
            // 
            // MainWIndowHead
            // 
            MainWIndowHead.BackColor = Color.FromArgb(30, 30, 30);
            MainWIndowHead.ColumnCount = 5;
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 62F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 62F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 62F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 62F));
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
            MainWIndowHead.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            MainWIndowHead.Size = new Size(1234, 64);
            MainWIndowHead.TabIndex = 3;
            MainWIndowHead.Tag = "WindowHead";
            // 
            // TitleLabel
            // 
            TitleLabel.AutoEllipsis = true;
            TitleLabel.AutoSize = true;
            TitleLabel.BackColor = Color.Transparent;
            TitleLabel.Dock = DockStyle.Fill;
            TitleLabel.Font = new Font("Segoe UI", 12F);
            TitleLabel.ForeColor = Color.White;
            TitleLabel.ImageAlign = ContentAlignment.MiddleLeft;
            TitleLabel.Location = new Point(68, 0);
            TitleLabel.Margin = new Padding(6, 0, 0, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(980, 64);
            TitleLabel.TabIndex = 0;
            TitleLabel.Tag = "Title";
            TitleLabel.Text = "Title";
            TitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            TitleLabel.DoubleClick += TitleLabel_DoubleClick;
            // 
            // MinimizeButton
            // 
            MinimizeButton.BackColor = Color.Gray;
            MinimizeButton.BackgroundImage = Properties.Resources.window_minimize_icon;
            MinimizeButton.BackgroundImageLayout = ImageLayout.Zoom;
            MinimizeButton.Cursor = Cursors.Hand;
            MinimizeButton.Dock = DockStyle.Fill;
            MinimizeButton.FlatStyle = FlatStyle.Popup;
            MinimizeButton.Location = new Point(1052, 4);
            MinimizeButton.Margin = new Padding(4);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Size = new Size(54, 56);
            MinimizeButton.TabIndex = 1;
            MinimizeButton.Tag = "WindowButton";
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
            MaximizeButton.Location = new Point(1114, 4);
            MaximizeButton.Margin = new Padding(4);
            MaximizeButton.Name = "MaximizeButton";
            MaximizeButton.Size = new Size(54, 56);
            MaximizeButton.TabIndex = 2;
            MaximizeButton.Tag = "WindowButton";
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
            CloseButton.Location = new Point(1176, 4);
            CloseButton.Margin = new Padding(4);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(54, 56);
            CloseButton.TabIndex = 3;
            CloseButton.Tag = "WindowButton";
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
            button1.Size = new Size(62, 64);
            button1.TabIndex = 4;
            button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.Controls.Add(DisplayPlaybackPosition, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 2, 0);
            tableLayoutPanel1.Controls.Add(playbackProgressBar, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(1, 784);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1234, 75);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // DisplayPlaybackPosition
            // 
            DisplayPlaybackPosition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            DisplayPlaybackPosition.AutoSize = true;
            DisplayPlaybackPosition.Font = new Font("Segoe UI", 12F);
            DisplayPlaybackPosition.ForeColor = Color.White;
            DisplayPlaybackPosition.Location = new Point(4, 23);
            DisplayPlaybackPosition.Margin = new Padding(4, 0, 4, 0);
            DisplayPlaybackPosition.Name = "DisplayPlaybackPosition";
            DisplayPlaybackPosition.Size = new Size(142, 28);
            DisplayPlaybackPosition.TabIndex = 0;
            DisplayPlaybackPosition.Text = "00:00";
            DisplayPlaybackPosition.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(GripButton);
            panel1.Controls.Add(DisplayPlaybackSize);
            panel1.Location = new Point(1084, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(150, 75);
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
            GripButton.Location = new Point(124, 49);
            GripButton.Margin = new Padding(0);
            GripButton.Name = "GripButton";
            GripButton.Size = new Size(26, 30);
            GripButton.TabIndex = 1;
            GripButton.Tag = "GripButton";
            // 
            // DisplayPlaybackSize
            // 
            DisplayPlaybackSize.BackColor = Color.Transparent;
            DisplayPlaybackSize.Dock = DockStyle.Fill;
            DisplayPlaybackSize.Font = new Font("Segoe UI", 12F);
            DisplayPlaybackSize.ForeColor = Color.White;
            DisplayPlaybackSize.Location = new Point(0, 0);
            DisplayPlaybackSize.Margin = new Padding(4, 0, 4, 0);
            DisplayPlaybackSize.Name = "DisplayPlaybackSize";
            DisplayPlaybackSize.Size = new Size(150, 75);
            DisplayPlaybackSize.TabIndex = 0;
            DisplayPlaybackSize.Text = "00:00";
            DisplayPlaybackSize.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // playbackProgressBar
            // 
            playbackProgressBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            playbackProgressBar.ForeColor = Color.White;
            playbackProgressBar.Location = new Point(150, 22);
            playbackProgressBar.Margin = new Padding(0);
            playbackProgressBar.MaxValue = 100000;
            playbackProgressBar.MinimumSize = new Size(0, 31);
            playbackProgressBar.MinValue = 0;
            playbackProgressBar.Name = "playbackProgressBar";
            playbackProgressBar.Size = new Size(934, 31);
            playbackProgressBar.TabIndex = 1;
            playbackProgressBar.Value = 10000;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tableLayoutPanel2.Controls.Add(panel3, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(1, 65);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 656F));
            tableLayoutPanel2.Size = new Size(1234, 657);
            tableLayoutPanel2.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(TabControler);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 4);
            panel3.Margin = new Padding(0, 4, 0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1159, 653);
            panel3.TabIndex = 1;
            // 
            // TabControler
            // 
            TabControler.BackColor = Color.White;
            TabControler.BorderStyle = BorderStyle.None;
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
            TabControler.Size = new Size(1157, 651);
            TabControler.TabIndex = 7;
            TabControler.TabPadding = new Padding(0);
            TabControler.TabSize = new Size(100, 50);
            TabControler.TabSizing = Manina.Windows.Forms.TabSizing.Stretch;
            // 
            // PlaybackTab
            // 
            PlaybackTab.BackColor = Color.FromArgb(50, 50, 50);
            PlaybackTab.Controls.Add(PlaybackTabMainTableLayoutPanel);
            PlaybackTab.ForeColor = Color.White;
            PlaybackTab.HotAndActiveTabBackColor = Color.FromArgb(255, 128, 0);
            PlaybackTab.HotTabBackColor = Color.FromArgb(255, 192, 128);
            PlaybackTab.Location = new Point(0, 50);
            PlaybackTab.Name = "PlaybackTab";
            PlaybackTab.SelectedBackColor = Color.FromArgb(255, 128, 0);
            PlaybackTab.Size = new Size(1157, 602);
            PlaybackTab.Text = " Playback";
            // 
            // PlaybackTabMainTableLayoutPanel
            // 
            PlaybackTabMainTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            PlaybackTabMainTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            PlaybackTabMainTableLayoutPanel.ColumnCount = 2;
            PlaybackTabMainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 188F));
            PlaybackTabMainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlaybackTabMainTableLayoutPanel.Controls.Add(PlaybackTabLeftTableLayoutPanel, 0, 0);
            PlaybackTabMainTableLayoutPanel.Controls.Add(tableLayoutPanel4, 1, 0);
            PlaybackTabMainTableLayoutPanel.Dock = DockStyle.Fill;
            PlaybackTabMainTableLayoutPanel.Location = new Point(0, 0);
            PlaybackTabMainTableLayoutPanel.Margin = new Padding(0);
            PlaybackTabMainTableLayoutPanel.Name = "PlaybackTabMainTableLayoutPanel";
            PlaybackTabMainTableLayoutPanel.RowCount = 1;
            PlaybackTabMainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlaybackTabMainTableLayoutPanel.Size = new Size(1157, 602);
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
            PlaybackTabLeftTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 188F));
            PlaybackTabLeftTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlaybackTabLeftTableLayoutPanel.Size = new Size(188, 600);
            PlaybackTabLeftTableLayoutPanel.TabIndex = 1;
            // 
            // PlaybackTabLeftBottomPanel
            // 
            PlaybackTabLeftBottomPanel.AutoScroll = true;
            PlaybackTabLeftBottomPanel.Controls.Add(PlaybackTabLeftBottomFlowLayoutPanel);
            PlaybackTabLeftBottomPanel.Dock = DockStyle.Fill;
            PlaybackTabLeftBottomPanel.Location = new Point(0, 188);
            PlaybackTabLeftBottomPanel.Margin = new Padding(0);
            PlaybackTabLeftBottomPanel.Name = "PlaybackTabLeftBottomPanel";
            PlaybackTabLeftBottomPanel.Size = new Size(188, 412);
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
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabGenresLabelInfo);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabGenresLabelValue);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabDurationLabelInfo);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabDurationLabelValue);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabRatting);
            PlaybackTabLeftBottomFlowLayoutPanel.Controls.Add(PlaybackTabLyricsButton);
            PlaybackTabLeftBottomFlowLayoutPanel.Dock = DockStyle.Top;
            PlaybackTabLeftBottomFlowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            PlaybackTabLeftBottomFlowLayoutPanel.Location = new Point(0, 0);
            PlaybackTabLeftBottomFlowLayoutPanel.Margin = new Padding(4, 5, 4, 5);
            PlaybackTabLeftBottomFlowLayoutPanel.MinimumSize = new Size(188, 311);
            PlaybackTabLeftBottomFlowLayoutPanel.Name = "PlaybackTabLeftBottomFlowLayoutPanel";
            PlaybackTabLeftBottomFlowLayoutPanel.Size = new Size(188, 364);
            PlaybackTabLeftBottomFlowLayoutPanel.TabIndex = 0;
            PlaybackTabLeftBottomFlowLayoutPanel.WrapContents = false;
            // 
            // PlaybackTabTitleLabelInfo
            // 
            PlaybackTabTitleLabelInfo.Dock = DockStyle.Fill;
            PlaybackTabTitleLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            PlaybackTabTitleLabelInfo.Location = new Point(0, 4);
            PlaybackTabTitleLabelInfo.Margin = new Padding(0, 4, 0, 0);
            PlaybackTabTitleLabelInfo.Name = "PlaybackTabTitleLabelInfo";
            PlaybackTabTitleLabelInfo.Padding = new Padding(2, 0, 0, 0);
            PlaybackTabTitleLabelInfo.Size = new Size(175, 30);
            PlaybackTabTitleLabelInfo.TabIndex = 1;
            PlaybackTabTitleLabelInfo.Tag = "Title";
            PlaybackTabTitleLabelInfo.Text = "Title:";
            // 
            // PlaybackTabTitleLabelValue
            // 
            PlaybackTabTitleLabelValue.AutoEllipsis = true;
            PlaybackTabTitleLabelValue.Dock = DockStyle.Fill;
            PlaybackTabTitleLabelValue.Font = new Font("Segoe UI", 9F);
            PlaybackTabTitleLabelValue.Location = new Point(0, 34);
            PlaybackTabTitleLabelValue.Margin = new Padding(0);
            PlaybackTabTitleLabelValue.MinimumSize = new Size(175, 25);
            PlaybackTabTitleLabelValue.Name = "PlaybackTabTitleLabelValue";
            PlaybackTabTitleLabelValue.Padding = new Padding(14, 0, 0, 0);
            PlaybackTabTitleLabelValue.Size = new Size(175, 25);
            PlaybackTabTitleLabelValue.TabIndex = 1;
            PlaybackTabTitleLabelValue.Text = "Track Name";
            PlaybackTabTitleLabelValue.UseMnemonic = false;
            // 
            // PlaybackTabAlbumLabelInfo
            // 
            PlaybackTabAlbumLabelInfo.Dock = DockStyle.Fill;
            PlaybackTabAlbumLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            PlaybackTabAlbumLabelInfo.Location = new Point(0, 59);
            PlaybackTabAlbumLabelInfo.Margin = new Padding(0);
            PlaybackTabAlbumLabelInfo.Name = "PlaybackTabAlbumLabelInfo";
            PlaybackTabAlbumLabelInfo.Padding = new Padding(2, 0, 0, 0);
            PlaybackTabAlbumLabelInfo.Size = new Size(175, 30);
            PlaybackTabAlbumLabelInfo.TabIndex = 1;
            PlaybackTabAlbumLabelInfo.Tag = "Title";
            PlaybackTabAlbumLabelInfo.Text = "Album:";
            // 
            // PlaybackTabAlbumLabelValue
            // 
            PlaybackTabAlbumLabelValue.AutoEllipsis = true;
            PlaybackTabAlbumLabelValue.Dock = DockStyle.Fill;
            PlaybackTabAlbumLabelValue.Font = new Font("Segoe UI", 9F);
            PlaybackTabAlbumLabelValue.Location = new Point(0, 89);
            PlaybackTabAlbumLabelValue.Margin = new Padding(0);
            PlaybackTabAlbumLabelValue.MinimumSize = new Size(175, 25);
            PlaybackTabAlbumLabelValue.Name = "PlaybackTabAlbumLabelValue";
            PlaybackTabAlbumLabelValue.Padding = new Padding(14, 0, 0, 0);
            PlaybackTabAlbumLabelValue.Size = new Size(175, 25);
            PlaybackTabAlbumLabelValue.TabIndex = 1;
            PlaybackTabAlbumLabelValue.UseMnemonic = false;
            // 
            // PlaybackTabArtistsLabelInfo
            // 
            PlaybackTabArtistsLabelInfo.Dock = DockStyle.Fill;
            PlaybackTabArtistsLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            PlaybackTabArtistsLabelInfo.Location = new Point(0, 114);
            PlaybackTabArtistsLabelInfo.Margin = new Padding(0);
            PlaybackTabArtistsLabelInfo.Name = "PlaybackTabArtistsLabelInfo";
            PlaybackTabArtistsLabelInfo.Padding = new Padding(2, 0, 0, 0);
            PlaybackTabArtistsLabelInfo.Size = new Size(175, 30);
            PlaybackTabArtistsLabelInfo.TabIndex = 1;
            PlaybackTabArtistsLabelInfo.Tag = "Title";
            PlaybackTabArtistsLabelInfo.Text = "Artists:";
            // 
            // PlaybackTabArtistsLabelValue
            // 
            PlaybackTabArtistsLabelValue.AutoEllipsis = true;
            PlaybackTabArtistsLabelValue.Font = new Font("Segoe UI", 9F);
            PlaybackTabArtistsLabelValue.Location = new Point(0, 144);
            PlaybackTabArtistsLabelValue.Margin = new Padding(0);
            PlaybackTabArtistsLabelValue.MinimumSize = new Size(175, 25);
            PlaybackTabArtistsLabelValue.Name = "PlaybackTabArtistsLabelValue";
            PlaybackTabArtistsLabelValue.Padding = new Padding(14, 0, 0, 0);
            PlaybackTabArtistsLabelValue.Size = new Size(175, 25);
            PlaybackTabArtistsLabelValue.TabIndex = 1;
            PlaybackTabArtistsLabelValue.UseMnemonic = false;
            // 
            // PlaybackTabGenresLabelInfo
            // 
            PlaybackTabGenresLabelInfo.Dock = DockStyle.Fill;
            PlaybackTabGenresLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            PlaybackTabGenresLabelInfo.Location = new Point(0, 169);
            PlaybackTabGenresLabelInfo.Margin = new Padding(0);
            PlaybackTabGenresLabelInfo.Name = "PlaybackTabGenresLabelInfo";
            PlaybackTabGenresLabelInfo.Padding = new Padding(2, 0, 0, 0);
            PlaybackTabGenresLabelInfo.Size = new Size(175, 30);
            PlaybackTabGenresLabelInfo.TabIndex = 1;
            PlaybackTabGenresLabelInfo.Tag = "Title";
            PlaybackTabGenresLabelInfo.Text = "Genre:";
            // 
            // PlaybackTabGenresLabelValue
            // 
            PlaybackTabGenresLabelValue.AutoEllipsis = true;
            PlaybackTabGenresLabelValue.Dock = DockStyle.Fill;
            PlaybackTabGenresLabelValue.Font = new Font("Segoe UI", 9F);
            PlaybackTabGenresLabelValue.Location = new Point(0, 199);
            PlaybackTabGenresLabelValue.Margin = new Padding(0);
            PlaybackTabGenresLabelValue.MinimumSize = new Size(175, 25);
            PlaybackTabGenresLabelValue.Name = "PlaybackTabGenresLabelValue";
            PlaybackTabGenresLabelValue.Padding = new Padding(14, 0, 0, 0);
            PlaybackTabGenresLabelValue.Size = new Size(175, 28);
            PlaybackTabGenresLabelValue.TabIndex = 1;
            PlaybackTabGenresLabelValue.UseMnemonic = false;
            // 
            // PlaybackTabDurationLabelInfo
            // 
            PlaybackTabDurationLabelInfo.Dock = DockStyle.Fill;
            PlaybackTabDurationLabelInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            PlaybackTabDurationLabelInfo.Location = new Point(0, 227);
            PlaybackTabDurationLabelInfo.Margin = new Padding(0);
            PlaybackTabDurationLabelInfo.Name = "PlaybackTabDurationLabelInfo";
            PlaybackTabDurationLabelInfo.Padding = new Padding(2, 0, 0, 0);
            PlaybackTabDurationLabelInfo.Size = new Size(175, 30);
            PlaybackTabDurationLabelInfo.TabIndex = 1;
            PlaybackTabDurationLabelInfo.Tag = "Title";
            PlaybackTabDurationLabelInfo.Text = "Duration:";
            // 
            // PlaybackTabDurationLabelValue
            // 
            PlaybackTabDurationLabelValue.Dock = DockStyle.Fill;
            PlaybackTabDurationLabelValue.Font = new Font("Segoe UI", 9F);
            PlaybackTabDurationLabelValue.Location = new Point(0, 257);
            PlaybackTabDurationLabelValue.Margin = new Padding(0);
            PlaybackTabDurationLabelValue.MinimumSize = new Size(175, 25);
            PlaybackTabDurationLabelValue.Name = "PlaybackTabDurationLabelValue";
            PlaybackTabDurationLabelValue.Padding = new Padding(14, 0, 0, 0);
            PlaybackTabDurationLabelValue.Size = new Size(175, 28);
            PlaybackTabDurationLabelValue.TabIndex = 1;
            PlaybackTabDurationLabelValue.UseMnemonic = false;
            // 
            // PlaybackTabRatting
            // 
            PlaybackTabRatting.BackColor = Color.Transparent;
            PlaybackTabRatting.IsReadOnly = false;
            PlaybackTabRatting.Location = new Point(5, 290);
            PlaybackTabRatting.Margin = new Padding(5);
            PlaybackTabRatting.MaximumSize = new Size(125, 25);
            PlaybackTabRatting.MinimumSize = new Size(125, 25);
            PlaybackTabRatting.Name = "PlaybackTabRatting";
            PlaybackTabRatting.Rate = 0D;
            PlaybackTabRatting.Size = new Size(125, 25);
            PlaybackTabRatting.TabIndex = 3;
            PlaybackTabRatting.Zoom = 1D;
            // 
            // PlaybackTabLyricsButton
            // 
            PlaybackTabLyricsButton.FlatAppearance.BorderColor = Color.White;
            PlaybackTabLyricsButton.FlatAppearance.MouseDownBackColor = Color.Gray;
            PlaybackTabLyricsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            PlaybackTabLyricsButton.FlatStyle = FlatStyle.Flat;
            PlaybackTabLyricsButton.Location = new Point(4, 324);
            PlaybackTabLyricsButton.Margin = new Padding(4);
            PlaybackTabLyricsButton.Name = "PlaybackTabLyricsButton";
            PlaybackTabLyricsButton.Size = new Size(118, 36);
            PlaybackTabLyricsButton.TabIndex = 2;
            PlaybackTabLyricsButton.Text = "Lyrics";
            PlaybackTabLyricsButton.UseVisualStyleBackColor = true;
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
            FileCover.Size = new Size(188, 188);
            FileCover.TabIndex = 1;
            FileCover.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(PlaybackPositionLabel, 0, 0);
            tableLayoutPanel4.Controls.Add(PlaybackTabDataGridView, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(190, 1);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(966, 600);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // PlaybackPositionLabel
            // 
            PlaybackPositionLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PlaybackPositionLabel.AutoSize = true;
            PlaybackPositionLabel.Location = new Point(4, 12);
            PlaybackPositionLabel.Margin = new Padding(4, 0, 4, 0);
            PlaybackPositionLabel.Name = "PlaybackPositionLabel";
            PlaybackPositionLabel.Size = new Size(958, 25);
            PlaybackPositionLabel.TabIndex = 2;
            PlaybackPositionLabel.Text = "label1";
            PlaybackPositionLabel.UseMnemonic = false;
            // 
            // PlaybackTabDataGridView
            // 
            PlaybackTabDataGridView.AccessibleRole = AccessibleRole.None;
            PlaybackTabDataGridView.AllowUserToAddRows = false;
            PlaybackTabDataGridView.AllowUserToDeleteRows = false;
            PlaybackTabDataGridView.AllowUserToResizeColumns = false;
            PlaybackTabDataGridView.AllowUserToResizeRows = false;
            PlaybackTabDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PlaybackTabDataGridView.Columns.AddRange(new DataGridViewColumn[] { SelectedColumn, NameColumn, DurationColumn, ArtistsColumn, AlbumColumn });
            PlaybackTabDataGridView.Dock = DockStyle.Fill;
            PlaybackTabDataGridView.EnableHeadersVisualStyles = false;
            PlaybackTabDataGridView.Location = new Point(4, 54);
            PlaybackTabDataGridView.Margin = new Padding(4);
            PlaybackTabDataGridView.MultiSelect = false;
            PlaybackTabDataGridView.Name = "PlaybackTabDataGridView";
            PlaybackTabDataGridView.ReadOnly = true;
            PlaybackTabDataGridView.RowHeadersVisible = false;
            PlaybackTabDataGridView.RowHeadersWidth = 51;
            PlaybackTabDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            PlaybackTabDataGridView.ScrollBars = ScrollBars.Vertical;
            PlaybackTabDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PlaybackTabDataGridView.ShowCellErrors = false;
            PlaybackTabDataGridView.ShowEditingIcon = false;
            PlaybackTabDataGridView.ShowRowErrors = false;
            PlaybackTabDataGridView.Size = new Size(958, 542);
            PlaybackTabDataGridView.TabIndex = 3;
            // 
            // SelectedColumn
            // 
            SelectedColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            SelectedColumn.DataPropertyName = "Selected";
            dataGridViewCellStyle1.Font = new Font("Wingdings", 9F);
            SelectedColumn.DefaultCellStyle = dataGridViewCellStyle1;
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
            // LibraryTab
            // 
            LibraryTab.BackColor = Color.FromArgb(50, 50, 50);
            LibraryTab.Controls.Add(LibraryTabTableLayoutPanel);
            LibraryTab.ForeColor = Color.White;
            LibraryTab.HotAndActiveTabBackColor = Color.FromArgb(255, 128, 0);
            LibraryTab.HotTabBackColor = Color.FromArgb(255, 192, 128);
            LibraryTab.Location = new Point(0, 62);
            LibraryTab.Name = "LibraryTab";
            LibraryTab.SelectedBackColor = Color.FromArgb(255, 128, 0);
            LibraryTab.Size = new Size(1071, 640);
            LibraryTab.Text = " Library";
            // 
            // LibraryTabTableLayoutPanel
            // 
            LibraryTabTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            LibraryTabTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            LibraryTabTableLayoutPanel.ColumnCount = 1;
            LibraryTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            LibraryTabTableLayoutPanel.Controls.Add(LibraryNavigationPathContener, 0, 0);
            LibraryTabTableLayoutPanel.Controls.Add(LibraryFiltersGrid, 0, 1);
            LibraryTabTableLayoutPanel.Controls.Add(LibraryTabSplitContainer, 0, 2);
            LibraryTabTableLayoutPanel.Dock = DockStyle.Fill;
            LibraryTabTableLayoutPanel.Location = new Point(0, 0);
            LibraryTabTableLayoutPanel.Margin = new Padding(0);
            LibraryTabTableLayoutPanel.Name = "LibraryTabTableLayoutPanel";
            LibraryTabTableLayoutPanel.RowCount = 3;
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            LibraryTabTableLayoutPanel.Size = new Size(1071, 640);
            LibraryTabTableLayoutPanel.TabIndex = 1;
            // 
            // LibraryNavigationPathContener
            // 
            LibraryNavigationPathContener.Dock = DockStyle.Fill;
            LibraryNavigationPathContener.Location = new Point(1, 1);
            LibraryNavigationPathContener.Margin = new Padding(0);
            LibraryNavigationPathContener.Name = "LibraryNavigationPathContener";
            LibraryNavigationPathContener.Size = new Size(1069, 50);
            LibraryNavigationPathContener.TabIndex = 0;
            // 
            // LibraryFiltersGrid
            // 
            LibraryFiltersGrid.Controls.Add(LibraryFiltersModeLabel);
            LibraryFiltersGrid.Controls.Add(LibraryFiltersMode);
            LibraryFiltersGrid.Controls.Add(LibraryFiltersGenreList);
            LibraryFiltersGrid.Controls.Add(LibraryFiltersSearchBox);
            LibraryFiltersGrid.Dock = DockStyle.Fill;
            LibraryFiltersGrid.Location = new Point(5, 56);
            LibraryFiltersGrid.Margin = new Padding(4);
            LibraryFiltersGrid.Name = "LibraryFiltersGrid";
            LibraryFiltersGrid.Size = new Size(1061, 42);
            LibraryFiltersGrid.TabIndex = 1;
            LibraryFiltersGrid.WrapContents = false;
            // 
            // LibraryFiltersModeLabel
            // 
            LibraryFiltersModeLabel.Anchor = AnchorStyles.Left;
            LibraryFiltersModeLabel.AutoSize = true;
            LibraryFiltersModeLabel.Location = new Point(4, 8);
            LibraryFiltersModeLabel.Margin = new Padding(4, 0, 12, 0);
            LibraryFiltersModeLabel.Name = "LibraryFiltersModeLabel";
            LibraryFiltersModeLabel.Size = new Size(50, 25);
            LibraryFiltersModeLabel.TabIndex = 0;
            LibraryFiltersModeLabel.Text = "Filter";
            // 
            // LibraryFiltersMode
            // 
            LibraryFiltersMode.Anchor = AnchorStyles.Left;
            LibraryFiltersMode.BackColor = Color.DimGray;
            LibraryFiltersMode.DropDownStyle = ComboBoxStyle.DropDownList;
            LibraryFiltersMode.FlatStyle = FlatStyle.Flat;
            LibraryFiltersMode.ForeColor = Color.White;
            LibraryFiltersMode.FormattingEnabled = true;
            LibraryFiltersMode.Items.AddRange(new object[] { "Nothing", "Title", "Artist", "Album", "Genre" });
            LibraryFiltersMode.Location = new Point(70, 4);
            LibraryFiltersMode.Margin = new Padding(4);
            LibraryFiltersMode.Name = "LibraryFiltersMode";
            LibraryFiltersMode.Size = new Size(179, 33);
            LibraryFiltersMode.TabIndex = 1;
            // 
            // LibraryFiltersGenreList
            // 
            LibraryFiltersGenreList.Anchor = AnchorStyles.Left;
            LibraryFiltersGenreList.BackColor = Color.DimGray;
            LibraryFiltersGenreList.DropDownStyle = ComboBoxStyle.DropDownList;
            LibraryFiltersGenreList.FlatStyle = FlatStyle.Flat;
            LibraryFiltersGenreList.ForeColor = Color.White;
            LibraryFiltersGenreList.FormattingEnabled = true;
            LibraryFiltersGenreList.Items.AddRange(new object[] { "Nothing", "Title", "Artist", "Album", "Genre" });
            LibraryFiltersGenreList.Location = new Point(257, 4);
            LibraryFiltersGenreList.Margin = new Padding(4);
            LibraryFiltersGenreList.MinimumSize = new Size(249, 0);
            LibraryFiltersGenreList.Name = "LibraryFiltersGenreList";
            LibraryFiltersGenreList.Size = new Size(249, 33);
            LibraryFiltersGenreList.TabIndex = 1;
            // 
            // LibraryFiltersSearchBox
            // 
            LibraryFiltersSearchBox.Anchor = AnchorStyles.Left;
            LibraryFiltersSearchBox.BackColor = Color.DimGray;
            LibraryFiltersSearchBox.BorderStyle = BorderStyle.FixedSingle;
            LibraryFiltersSearchBox.CharacterCasing = CharacterCasing.Lower;
            LibraryFiltersSearchBox.Font = new Font("Segoe UI", 10F);
            LibraryFiltersSearchBox.ForeColor = Color.White;
            LibraryFiltersSearchBox.Location = new Point(514, 2);
            LibraryFiltersSearchBox.Margin = new Padding(4, 2, 0, 9);
            LibraryFiltersSearchBox.Name = "LibraryFiltersSearchBox";
            LibraryFiltersSearchBox.PlaceholderText = " Search";
            LibraryFiltersSearchBox.Size = new Size(156, 30);
            LibraryFiltersSearchBox.TabIndex = 3;
            LibraryFiltersSearchBox.Text = " search";
            // 
            // LibraryTabSplitContainer
            // 
            LibraryTabSplitContainer.Dock = DockStyle.Fill;
            LibraryTabSplitContainer.Location = new Point(5, 107);
            LibraryTabSplitContainer.Margin = new Padding(4);
            LibraryTabSplitContainer.Name = "LibraryTabSplitContainer";
            LibraryTabSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // LibraryTabSplitContainer.Panel1
            // 
            LibraryTabSplitContainer.Panel1.AutoScroll = true;
            LibraryTabSplitContainer.Panel1.BackColor = Color.FromArgb(0, 192, 192);
            LibraryTabSplitContainer.Panel1.Controls.Add(LibraryNavigationContent);
            LibraryTabSplitContainer.Panel1MinSize = 100;
            // 
            // LibraryTabSplitContainer.Panel2
            // 
            LibraryTabSplitContainer.Panel2.Controls.Add(LibraryTabSplitContainer2);
            LibraryTabSplitContainer.Panel2MinSize = 100;
            LibraryTabSplitContainer.Size = new Size(1061, 528);
            LibraryTabSplitContainer.SplitterDistance = 274;
            LibraryTabSplitContainer.SplitterWidth = 1;
            LibraryTabSplitContainer.TabIndex = 3;
            // 
            // LibraryNavigationContent
            // 
            LibraryNavigationContent.AutoSize = true;
            LibraryNavigationContent.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            LibraryNavigationContent.ColumnCount = 1;
            LibraryNavigationContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            LibraryNavigationContent.Controls.Add(LibraryFoldersLabel, 0, 0);
            LibraryNavigationContent.Controls.Add(LibraryNavigationContentFolders, 0, 1);
            LibraryNavigationContent.Dock = DockStyle.Fill;
            LibraryNavigationContent.Location = new Point(0, 0);
            LibraryNavigationContent.Margin = new Padding(0);
            LibraryNavigationContent.MinimumSize = new Size(0, 188);
            LibraryNavigationContent.Name = "LibraryNavigationContent";
            LibraryNavigationContent.RowCount = 2;
            LibraryNavigationContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            LibraryNavigationContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            LibraryNavigationContent.Size = new Size(1061, 274);
            LibraryNavigationContent.TabIndex = 0;
            // 
            // LibraryFoldersLabel
            // 
            LibraryFoldersLabel.AutoSize = true;
            LibraryFoldersLabel.Dock = DockStyle.Fill;
            LibraryFoldersLabel.Location = new Point(0, 0);
            LibraryFoldersLabel.Margin = new Padding(0);
            LibraryFoldersLabel.Name = "LibraryFoldersLabel";
            LibraryFoldersLabel.Padding = new Padding(4, 0, 4, 0);
            LibraryFoldersLabel.Size = new Size(1061, 38);
            LibraryFoldersLabel.TabIndex = 0;
            LibraryFoldersLabel.Tag = "Bold";
            LibraryFoldersLabel.Text = "Folders";
            LibraryFoldersLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LibraryNavigationContentFolders
            // 
            LibraryNavigationContentFolders.AllowUserToAddRows = false;
            LibraryNavigationContentFolders.AllowUserToDeleteRows = false;
            LibraryNavigationContentFolders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LibraryNavigationContentFolders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            LibraryNavigationContentFolders.Dock = DockStyle.Fill;
            LibraryNavigationContentFolders.Location = new Point(0, 38);
            LibraryNavigationContentFolders.Margin = new Padding(0);
            LibraryNavigationContentFolders.Name = "LibraryNavigationContentFolders";
            LibraryNavigationContentFolders.ReadOnly = true;
            LibraryNavigationContentFolders.RowHeadersWidth = 51;
            LibraryNavigationContentFolders.Size = new Size(1061, 236);
            LibraryNavigationContentFolders.TabIndex = 1;
            // 
            // LibraryTabSplitContainer2
            // 
            LibraryTabSplitContainer2.Dock = DockStyle.Fill;
            LibraryTabSplitContainer2.Location = new Point(0, 0);
            LibraryTabSplitContainer2.Margin = new Padding(0);
            LibraryTabSplitContainer2.Name = "LibraryTabSplitContainer2";
            // 
            // LibraryTabSplitContainer2.Panel1
            // 
            LibraryTabSplitContainer2.Panel1.Controls.Add(LibrarySearchContent);
            // 
            // LibraryTabSplitContainer2.Panel2
            // 
            LibraryTabSplitContainer2.Panel2.Controls.Add(LibraryNavigationContentFilesParent);
            LibraryTabSplitContainer2.Size = new Size(1061, 253);
            LibraryTabSplitContainer2.SplitterDistance = 353;
            LibraryTabSplitContainer2.SplitterWidth = 1;
            LibraryTabSplitContainer2.TabIndex = 0;
            // 
            // LibrarySearchContent
            // 
            LibrarySearchContent.AllowUserToAddRows = false;
            LibrarySearchContent.AllowUserToDeleteRows = false;
            LibrarySearchContent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            LibrarySearchContent.Dock = DockStyle.Fill;
            LibrarySearchContent.Location = new Point(0, 0);
            LibrarySearchContent.Margin = new Padding(4);
            LibrarySearchContent.Name = "LibrarySearchContent";
            LibrarySearchContent.ReadOnly = true;
            LibrarySearchContent.RowHeadersWidth = 51;
            LibrarySearchContent.Size = new Size(353, 253);
            LibrarySearchContent.TabIndex = 1;
            // 
            // LibraryNavigationContentFilesParent
            // 
            LibraryNavigationContentFilesParent.AutoScroll = true;
            LibraryNavigationContentFilesParent.Controls.Add(LibraryNavigationContentFiles);
            LibraryNavigationContentFilesParent.Dock = DockStyle.Fill;
            LibraryNavigationContentFilesParent.Location = new Point(0, 0);
            LibraryNavigationContentFilesParent.Margin = new Padding(0);
            LibraryNavigationContentFilesParent.Name = "LibraryNavigationContentFilesParent";
            LibraryNavigationContentFilesParent.Size = new Size(707, 253);
            LibraryNavigationContentFilesParent.TabIndex = 0;
            // 
            // LibraryNavigationContentFiles
            // 
            LibraryNavigationContentFiles.BackColor = Color.Transparent;
            LibraryNavigationContentFiles.ColumnCount = 1;
            LibraryNavigationContentFiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            LibraryNavigationContentFiles.Dock = DockStyle.Top;
            LibraryNavigationContentFiles.Location = new Point(0, 0);
            LibraryNavigationContentFiles.Margin = new Padding(0);
            LibraryNavigationContentFiles.Name = "LibraryNavigationContentFiles";
            LibraryNavigationContentFiles.RowCount = 1;
            LibraryNavigationContentFiles.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            LibraryNavigationContentFiles.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
            LibraryNavigationContentFiles.Size = new Size(707, 12);
            LibraryNavigationContentFiles.TabIndex = 1;
            // 
            // PlayListsTab
            // 
            PlayListsTab.BackColor = Color.FromArgb(50, 50, 50);
            PlayListsTab.Controls.Add(PlayListsTabTableLayoutPanel);
            PlayListsTab.ForeColor = Color.White;
            PlayListsTab.HotAndActiveTabBackColor = Color.FromArgb(255, 128, 0);
            PlayListsTab.HotTabBackColor = Color.FromArgb(255, 192, 128);
            PlayListsTab.Location = new Point(0, 50);
            PlayListsTab.Name = "PlayListsTab";
            PlayListsTab.SelectedBackColor = Color.FromArgb(255, 128, 0);
            PlayListsTab.Size = new Size(1157, 602);
            PlayListsTab.Text = " PlayLists";
            // 
            // PlayListsTabTableLayoutPanel
            // 
            PlayListsTabTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            PlayListsTabTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            PlayListsTabTableLayoutPanel.ColumnCount = 2;
            PlayListsTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            PlayListsTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlayListsTabTableLayoutPanel.Controls.Add(PlaylistsTree, 0, 0);
            PlayListsTabTableLayoutPanel.Controls.Add(PlayListsTabSplitContainer1, 1, 0);
            PlayListsTabTableLayoutPanel.Dock = DockStyle.Fill;
            PlayListsTabTableLayoutPanel.Location = new Point(0, 0);
            PlayListsTabTableLayoutPanel.Margin = new Padding(0);
            PlayListsTabTableLayoutPanel.Name = "PlayListsTabTableLayoutPanel";
            PlayListsTabTableLayoutPanel.RowCount = 1;
            PlayListsTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlayListsTabTableLayoutPanel.Size = new Size(1157, 602);
            PlayListsTabTableLayoutPanel.TabIndex = 1;
            // 
            // PlaylistsTree
            // 
            PlaylistsTree.BackColor = Color.FromArgb(30, 30, 30);
            PlaylistsTree.BorderStyle = BorderStyle.None;
            PlaylistsTree.Dock = DockStyle.Fill;
            PlaylistsTree.Font = new Font("Segoe UI", 12F);
            PlaylistsTree.ForeColor = Color.White;
            PlaylistsTree.FullRowSelect = true;
            PlaylistsTree.HideSelection = false;
            PlaylistsTree.ImageIndex = 0;
            PlaylistsTree.ImageList = PlayListsTabTreeImageList;
            PlaylistsTree.Location = new Point(1, 1);
            PlaylistsTree.Margin = new Padding(0);
            PlaylistsTree.Name = "PlaylistsTree";
            treeNode1.Checked = true;
            treeNode1.ImageKey = "filter_icon.png";
            treeNode1.Name = "PlayListsTabTreeNodeAutomatic";
            treeNode1.SelectedImageKey = "filter_icon.png";
            treeNode1.Text = "Automatic";
            treeNode2.Checked = true;
            treeNode2.ImageKey = "floppy_icon.png";
            treeNode2.Name = "PlayListsTabTreeNodeRecorded";
            treeNode2.SelectedImageKey = "floppy_icon.png";
            treeNode2.Text = "Recorded";
            treeNode3.Checked = true;
            treeNode3.ImageKey = "radio_icon.png";
            treeNode3.Name = "PlayListsTabTreeNodeWebRario";
            treeNode3.SelectedImageKey = "radio_icon.png";
            treeNode3.Text = "Web Radio";
            PlaylistsTree.Nodes.AddRange(new TreeNode[] { treeNode1, treeNode2, treeNode3 });
            PlaylistsTree.SelectedImageIndex = 0;
            PlaylistsTree.Size = new Size(250, 600);
            PlaylistsTree.TabIndex = 0;
            // 
            // PlayListsTabTreeImageList
            // 
            PlayListsTabTreeImageList.ColorDepth = ColorDepth.Depth32Bit;
            PlayListsTabTreeImageList.ImageStream = (ImageListStreamer)resources.GetObject("PlayListsTabTreeImageList.ImageStream");
            PlayListsTabTreeImageList.TransparentColor = Color.Transparent;
            PlayListsTabTreeImageList.Images.SetKeyName(0, "dot.png");
            PlayListsTabTreeImageList.Images.SetKeyName(1, "filter_icon.png");
            PlayListsTabTreeImageList.Images.SetKeyName(2, "floppy_icon.png");
            PlayListsTabTreeImageList.Images.SetKeyName(3, "radio_icon.png");
            PlayListsTabTreeImageList.Images.SetKeyName(4, "folder_open_trimed.png");
            PlayListsTabTreeImageList.Images.SetKeyName(5, "new_icon.png");
            PlayListsTabTreeImageList.Images.SetKeyName(6, "chart_line_icon.png");
            PlayListsTabTreeImageList.Images.SetKeyName(7, "star_white.png");
            PlayListsTabTreeImageList.Images.SetKeyName(8, "star_up.png");
            PlayListsTabTreeImageList.Images.SetKeyName(9, "history_recent_icon.png");
            // 
            // PlayListsTabSplitContainer1
            // 
            PlayListsTabSplitContainer1.Dock = DockStyle.Fill;
            PlayListsTabSplitContainer1.Location = new Point(252, 1);
            PlayListsTabSplitContainer1.Margin = new Padding(0);
            PlayListsTabSplitContainer1.Name = "PlayListsTabSplitContainer1";
            PlayListsTabSplitContainer1.Orientation = Orientation.Horizontal;
            // 
            // PlayListsTabSplitContainer1.Panel1
            // 
            PlayListsTabSplitContainer1.Panel1.Controls.Add(tableLayoutPanel5);
            // 
            // PlayListsTabSplitContainer1.Panel2
            // 
            PlayListsTabSplitContainer1.Panel2.Controls.Add(PlayListsTabSplitContainer2);
            PlayListsTabSplitContainer1.Size = new Size(904, 600);
            PlayListsTabSplitContainer1.SplitterDistance = 112;
            PlayListsTabSplitContainer1.SplitterWidth = 1;
            PlayListsTabSplitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(PlayListsTabVoidLabel, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(904, 112);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // PlayListsTabVoidLabel
            // 
            PlayListsTabVoidLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            PlayListsTabVoidLabel.AutoSize = true;
            PlayListsTabVoidLabel.Font = new Font("Segoe UI", 14F);
            PlayListsTabVoidLabel.Location = new Point(4, 40);
            PlayListsTabVoidLabel.Margin = new Padding(4, 0, 4, 0);
            PlayListsTabVoidLabel.Name = "PlayListsTabVoidLabel";
            PlayListsTabVoidLabel.Size = new Size(896, 32);
            PlayListsTabVoidLabel.TabIndex = 0;
            PlayListsTabVoidLabel.Text = "VOID";
            PlayListsTabVoidLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PlayListsTabSplitContainer2
            // 
            PlayListsTabSplitContainer2.Dock = DockStyle.Fill;
            PlayListsTabSplitContainer2.Location = new Point(0, 0);
            PlayListsTabSplitContainer2.Margin = new Padding(0);
            PlayListsTabSplitContainer2.Name = "PlayListsTabSplitContainer2";
            PlayListsTabSplitContainer2.Orientation = Orientation.Horizontal;
            // 
            // PlayListsTabSplitContainer2.Panel1
            // 
            PlayListsTabSplitContainer2.Panel1.Controls.Add(PlayListsTabDataGridView);
            // 
            // PlayListsTabSplitContainer2.Panel2
            // 
            PlayListsTabSplitContainer2.Panel2.AutoScroll = true;
            PlayListsTabSplitContainer2.Panel2.Controls.Add(PlayListsTabRadioPanel);
            PlayListsTabSplitContainer2.Size = new Size(904, 487);
            PlayListsTabSplitContainer2.SplitterDistance = 146;
            PlayListsTabSplitContainer2.SplitterWidth = 1;
            PlayListsTabSplitContainer2.TabIndex = 2;
            // 
            // PlayListsTabDataGridView
            // 
            PlayListsTabDataGridView.AllowUserToAddRows = false;
            PlayListsTabDataGridView.AllowUserToDeleteRows = false;
            PlayListsTabDataGridView.AllowUserToResizeColumns = false;
            PlayListsTabDataGridView.AllowUserToResizeRows = false;
            PlayListsTabDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            PlayListsTabDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PlayListsTabDataGridView.Columns.AddRange(new DataGridViewColumn[] { ColumnPlayCount, PlayListsColumnName, PlayListsColumnArtists, PlayListsColumnAlbum, PlayListsColumnDuration, PlayListsColumnRating });
            PlayListsTabDataGridView.Dock = DockStyle.Fill;
            PlayListsTabDataGridView.Location = new Point(0, 0);
            PlayListsTabDataGridView.Margin = new Padding(4);
            PlayListsTabDataGridView.MultiSelect = false;
            PlayListsTabDataGridView.Name = "PlayListsTabDataGridView";
            PlayListsTabDataGridView.RowHeadersVisible = false;
            PlayListsTabDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            PlayListsTabDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PlayListsTabDataGridView.ShowCellErrors = false;
            PlayListsTabDataGridView.ShowEditingIcon = false;
            PlayListsTabDataGridView.ShowRowErrors = false;
            PlayListsTabDataGridView.Size = new Size(904, 146);
            PlayListsTabDataGridView.TabIndex = 0;
            // 
            // ColumnPlayCount
            // 
            ColumnPlayCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            ColumnPlayCount.DataPropertyName = "PlayCount";
            ColumnPlayCount.HeaderText = "Played";
            ColumnPlayCount.MinimumWidth = 60;
            ColumnPlayCount.Name = "ColumnPlayCount";
            ColumnPlayCount.ReadOnly = true;
            ColumnPlayCount.Width = 60;
            // 
            // PlayListsColumnName
            // 
            PlayListsColumnName.DataPropertyName = "Name";
            PlayListsColumnName.HeaderText = "Name";
            PlayListsColumnName.MinimumWidth = 6;
            PlayListsColumnName.Name = "PlayListsColumnName";
            PlayListsColumnName.ReadOnly = true;
            // 
            // PlayListsColumnArtists
            // 
            PlayListsColumnArtists.DataPropertyName = "Artists";
            PlayListsColumnArtists.HeaderText = "Artists";
            PlayListsColumnArtists.MinimumWidth = 6;
            PlayListsColumnArtists.Name = "PlayListsColumnArtists";
            PlayListsColumnArtists.ReadOnly = true;
            // 
            // PlayListsColumnAlbum
            // 
            PlayListsColumnAlbum.DataPropertyName = "Album";
            PlayListsColumnAlbum.HeaderText = "Album";
            PlayListsColumnAlbum.MinimumWidth = 6;
            PlayListsColumnAlbum.Name = "PlayListsColumnAlbum";
            PlayListsColumnAlbum.ReadOnly = true;
            // 
            // PlayListsColumnDuration
            // 
            PlayListsColumnDuration.DataPropertyName = "Duration";
            PlayListsColumnDuration.HeaderText = "Duration";
            PlayListsColumnDuration.MinimumWidth = 6;
            PlayListsColumnDuration.Name = "PlayListsColumnDuration";
            PlayListsColumnDuration.ReadOnly = true;
            // 
            // PlayListsColumnRating
            // 
            PlayListsColumnRating.DataPropertyName = "Rating";
            PlayListsColumnRating.HeaderText = "Rating";
            PlayListsColumnRating.MinimumWidth = 6;
            PlayListsColumnRating.Name = "PlayListsColumnRating";
            PlayListsColumnRating.ReadOnly = true;
            PlayListsColumnRating.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // PlayListsTabRadioPanel
            // 
            PlayListsTabRadioPanel.AutoSize = true;
            PlayListsTabRadioPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PlayListsTabRadioPanel.ColumnCount = 2;
            PlayListsTabRadioPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 188F));
            PlayListsTabRadioPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlayListsTabRadioPanel.Controls.Add(PlayListsTabRadioPanelLeft, 0, 0);
            PlayListsTabRadioPanel.Controls.Add(PlayListsTabRadioPanelFlowLayoutPanel, 0, 1);
            PlayListsTabRadioPanel.Controls.Add(PlayListsTabRadioPanelRight, 1, 0);
            PlayListsTabRadioPanel.Dock = DockStyle.Top;
            PlayListsTabRadioPanel.Location = new Point(0, 0);
            PlayListsTabRadioPanel.Margin = new Padding(0);
            PlayListsTabRadioPanel.MinimumSize = new Size(0, 250);
            PlayListsTabRadioPanel.Name = "PlayListsTabRadioPanel";
            PlayListsTabRadioPanel.RowCount = 2;
            PlayListsTabRadioPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
            PlayListsTabRadioPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlayListsTabRadioPanel.Size = new Size(904, 302);
            PlayListsTabRadioPanel.TabIndex = 0;
            // 
            // PlayListsTabRadioPanelLeft
            // 
            PlayListsTabRadioPanelLeft.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PlayListsTabRadioPanelLeft.ColumnCount = 1;
            PlayListsTabRadioPanelLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlayListsTabRadioPanelLeft.Controls.Add(PlayListsTabRadioPanelIcon, 0, 0);
            PlayListsTabRadioPanelLeft.Controls.Add(PlayListsTabRadioButton, 0, 1);
            PlayListsTabRadioPanelLeft.Location = new Point(0, 0);
            PlayListsTabRadioPanelLeft.Margin = new Padding(0);
            PlayListsTabRadioPanelLeft.MaximumSize = new Size(188, 250);
            PlayListsTabRadioPanelLeft.MinimumSize = new Size(188, 250);
            PlayListsTabRadioPanelLeft.Name = "PlayListsTabRadioPanelLeft";
            PlayListsTabRadioPanelLeft.RowCount = 2;
            PlayListsTabRadioPanelLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, 188F));
            PlayListsTabRadioPanelLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            PlayListsTabRadioPanelLeft.Size = new Size(188, 250);
            PlayListsTabRadioPanelLeft.TabIndex = 0;
            // 
            // PlayListsTabRadioPanelIcon
            // 
            PlayListsTabRadioPanelIcon.BackColor = Color.Black;
            PlayListsTabRadioPanelIcon.BackgroundImage = Properties.Resources.radio_icon;
            PlayListsTabRadioPanelIcon.BackgroundImageLayout = ImageLayout.Center;
            PlayListsTabRadioPanelIcon.Enabled = false;
            PlayListsTabRadioPanelIcon.FlatAppearance.BorderColor = Color.Black;
            PlayListsTabRadioPanelIcon.FlatAppearance.BorderSize = 0;
            PlayListsTabRadioPanelIcon.FlatAppearance.MouseDownBackColor = Color.Black;
            PlayListsTabRadioPanelIcon.FlatAppearance.MouseOverBackColor = Color.Black;
            PlayListsTabRadioPanelIcon.FlatStyle = FlatStyle.Flat;
            PlayListsTabRadioPanelIcon.Location = new Point(0, 0);
            PlayListsTabRadioPanelIcon.Margin = new Padding(0);
            PlayListsTabRadioPanelIcon.MinimumSize = new Size(188, 188);
            PlayListsTabRadioPanelIcon.Name = "PlayListsTabRadioPanelIcon";
            PlayListsTabRadioPanelIcon.Size = new Size(188, 188);
            PlayListsTabRadioPanelIcon.TabIndex = 0;
            PlayListsTabRadioPanelIcon.UseVisualStyleBackColor = false;
            // 
            // PlayListsTabRadioButton
            // 
            PlayListsTabRadioButton.Dock = DockStyle.Fill;
            PlayListsTabRadioButton.ForeColor = Color.Black;
            PlayListsTabRadioButton.Location = new Point(4, 192);
            PlayListsTabRadioButton.Margin = new Padding(4);
            PlayListsTabRadioButton.Name = "PlayListsTabRadioButton";
            PlayListsTabRadioButton.Size = new Size(180, 54);
            PlayListsTabRadioButton.TabIndex = 1;
            PlayListsTabRadioButton.Text = "Play";
            PlayListsTabRadioButton.UseVisualStyleBackColor = true;
            // 
            // PlayListsTabRadioPanelFlowLayoutPanel
            // 
            PlayListsTabRadioPanelFlowLayoutPanel.AutoSize = true;
            PlayListsTabRadioPanelFlowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PlayListsTabRadioPanel.SetColumnSpan(PlayListsTabRadioPanelFlowLayoutPanel, 2);
            PlayListsTabRadioPanelFlowLayoutPanel.Controls.Add(button2);
            PlayListsTabRadioPanelFlowLayoutPanel.Dock = DockStyle.Top;
            PlayListsTabRadioPanelFlowLayoutPanel.Location = new Point(4, 254);
            PlayListsTabRadioPanelFlowLayoutPanel.Margin = new Padding(4);
            PlayListsTabRadioPanelFlowLayoutPanel.Name = "PlayListsTabRadioPanelFlowLayoutPanel";
            PlayListsTabRadioPanelFlowLayoutPanel.Size = new Size(896, 44);
            PlayListsTabRadioPanelFlowLayoutPanel.TabIndex = 1;
            // 
            // button2
            // 
            button2.ForeColor = Color.Black;
            button2.Location = new Point(4, 4);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(118, 36);
            button2.TabIndex = 0;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // PlayListsTabRadioPanelRight
            // 
            PlayListsTabRadioPanelRight.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            PlayListsTabRadioPanelRight.ColumnCount = 1;
            PlayListsTabRadioPanelRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlayListsTabRadioPanelRight.Controls.Add(PlayListsTabRadioPanelTitleLabel, 0, 0);
            PlayListsTabRadioPanelRight.Controls.Add(PlayListsTabRadioPanelRightPanelDescriptionLabel, 0, 1);
            PlayListsTabRadioPanelRight.Dock = DockStyle.Fill;
            PlayListsTabRadioPanelRight.Location = new Point(188, 0);
            PlayListsTabRadioPanelRight.Margin = new Padding(0);
            PlayListsTabRadioPanelRight.Name = "PlayListsTabRadioPanelRight";
            PlayListsTabRadioPanelRight.Padding = new Padding(4);
            PlayListsTabRadioPanelRight.RowCount = 2;
            PlayListsTabRadioPanelRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            PlayListsTabRadioPanelRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlayListsTabRadioPanelRight.Size = new Size(716, 250);
            PlayListsTabRadioPanelRight.TabIndex = 2;
            // 
            // PlayListsTabRadioPanelTitleLabel
            // 
            PlayListsTabRadioPanelTitleLabel.AutoSize = true;
            PlayListsTabRadioPanelTitleLabel.Location = new Point(8, 4);
            PlayListsTabRadioPanelTitleLabel.Margin = new Padding(4, 0, 4, 0);
            PlayListsTabRadioPanelTitleLabel.Name = "PlayListsTabRadioPanelTitleLabel";
            PlayListsTabRadioPanelTitleLabel.Size = new Size(59, 25);
            PlayListsTabRadioPanelTitleLabel.TabIndex = 0;
            PlayListsTabRadioPanelTitleLabel.Tag = "TitleBold";
            PlayListsTabRadioPanelTitleLabel.Text = "label1";
            // 
            // PlayListsTabRadioPanelRightPanelDescriptionLabel
            // 
            PlayListsTabRadioPanelRightPanelDescriptionLabel.AutoSize = true;
            PlayListsTabRadioPanelRightPanelDescriptionLabel.Dock = DockStyle.Top;
            PlayListsTabRadioPanelRightPanelDescriptionLabel.Location = new Point(8, 42);
            PlayListsTabRadioPanelRightPanelDescriptionLabel.Margin = new Padding(4, 0, 4, 0);
            PlayListsTabRadioPanelRightPanelDescriptionLabel.Name = "PlayListsTabRadioPanelRightPanelDescriptionLabel";
            PlayListsTabRadioPanelRightPanelDescriptionLabel.Size = new Size(700, 25);
            PlayListsTabRadioPanelRightPanelDescriptionLabel.TabIndex = 1;
            PlayListsTabRadioPanelRightPanelDescriptionLabel.Text = "label2";
            // 
            // SettingsTab
            // 
            SettingsTab.BackColor = Color.FromArgb(50, 50, 50);
            SettingsTab.Controls.Add(panel2);
            SettingsTab.ForeColor = Color.White;
            SettingsTab.HotAndActiveTabBackColor = Color.FromArgb(255, 128, 0);
            SettingsTab.HotTabBackColor = Color.FromArgb(255, 192, 128);
            SettingsTab.Location = new Point(0, 50);
            SettingsTab.Name = "SettingsTab";
            SettingsTab.SelectedBackColor = Color.FromArgb(255, 128, 0);
            SettingsTab.Size = new Size(1157, 602);
            SettingsTab.Text = " Settings";
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.BackColor = Color.FromArgb(30, 30, 30);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1157, 602);
            panel2.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(SettingsTabLangGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabStyleGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabAutoPlayGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsNormalizeVolumeGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabAlwaysOnTopGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabAutoCloseLyricsGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabDisplayLiveLyricsGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabConvGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabLibraryGroupBox);
            flowLayoutPanel1.Controls.Add(SettingsTabEqualizerGroupBox);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1134, 770);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // SettingsTabLangGroupBox
            // 
            SettingsTabLangGroupBox.AutoSize = true;
            SettingsTabLangGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabLangGroupBox.Controls.Add(SettingsTabLangComboBox);
            SettingsTabLangGroupBox.ForeColor = Color.White;
            SettingsTabLangGroupBox.Location = new Point(4, 5);
            SettingsTabLangGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabLangGroupBox.MinimumSize = new Size(214, 84);
            SettingsTabLangGroupBox.Name = "SettingsTabLangGroupBox";
            SettingsTabLangGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabLangGroupBox.Size = new Size(214, 84);
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
            SettingsTabLangComboBox.Location = new Point(8, 33);
            SettingsTabLangComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabLangComboBox.MaxDropDownItems = 2;
            SettingsTabLangComboBox.Name = "SettingsTabLangComboBox";
            SettingsTabLangComboBox.Size = new Size(198, 33);
            SettingsTabLangComboBox.TabIndex = 0;
            SettingsTabLangComboBox.ValueMember = "English";
            // 
            // SettingsTabStyleGroupBox
            // 
            SettingsTabStyleGroupBox.AutoSize = true;
            SettingsTabStyleGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabStyleGroupBox.Controls.Add(SettingsTabStyleComboBox);
            SettingsTabStyleGroupBox.ForeColor = Color.White;
            SettingsTabStyleGroupBox.Location = new Point(226, 5);
            SettingsTabStyleGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabStyleGroupBox.MinimumSize = new Size(214, 84);
            SettingsTabStyleGroupBox.Name = "SettingsTabStyleGroupBox";
            SettingsTabStyleGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabStyleGroupBox.Size = new Size(214, 84);
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
            SettingsTabStyleComboBox.Location = new Point(8, 33);
            SettingsTabStyleComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabStyleComboBox.MaxDropDownItems = 2;
            SettingsTabStyleComboBox.Name = "SettingsTabStyleComboBox";
            SettingsTabStyleComboBox.Size = new Size(198, 33);
            SettingsTabStyleComboBox.TabIndex = 0;
            SettingsTabStyleComboBox.ValueMember = "English";
            // 
            // SettingsTabAutoPlayGroupBox
            // 
            SettingsTabAutoPlayGroupBox.AutoSize = true;
            SettingsTabAutoPlayGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabAutoPlayGroupBox.Controls.Add(SettingsTabAutoPlayComboBox);
            SettingsTabAutoPlayGroupBox.ForeColor = Color.White;
            SettingsTabAutoPlayGroupBox.Location = new Point(448, 5);
            SettingsTabAutoPlayGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabAutoPlayGroupBox.MinimumSize = new Size(275, 84);
            SettingsTabAutoPlayGroupBox.Name = "SettingsTabAutoPlayGroupBox";
            SettingsTabAutoPlayGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabAutoPlayGroupBox.Size = new Size(275, 84);
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
            SettingsTabAutoPlayComboBox.Location = new Point(8, 33);
            SettingsTabAutoPlayComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabAutoPlayComboBox.MaxDropDownItems = 2;
            SettingsTabAutoPlayComboBox.Name = "SettingsTabAutoPlayComboBox";
            SettingsTabAutoPlayComboBox.Size = new Size(259, 33);
            SettingsTabAutoPlayComboBox.TabIndex = 0;
            SettingsTabAutoPlayComboBox.ValueMember = "English";
            // 
            // SettingsNormalizeVolumeGroupBox
            // 
            SettingsNormalizeVolumeGroupBox.AutoSize = true;
            SettingsNormalizeVolumeGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsNormalizeVolumeGroupBox.Controls.Add(SettingsNormalizeVolumeComboBox);
            SettingsNormalizeVolumeGroupBox.ForeColor = Color.White;
            SettingsNormalizeVolumeGroupBox.Location = new Point(731, 5);
            SettingsNormalizeVolumeGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsNormalizeVolumeGroupBox.MinimumSize = new Size(275, 84);
            SettingsNormalizeVolumeGroupBox.Name = "SettingsNormalizeVolumeGroupBox";
            SettingsNormalizeVolumeGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsNormalizeVolumeGroupBox.Size = new Size(275, 84);
            SettingsNormalizeVolumeGroupBox.TabIndex = 3;
            SettingsNormalizeVolumeGroupBox.TabStop = false;
            SettingsNormalizeVolumeGroupBox.Text = "Normalize Volume";
            // 
            // SettingsNormalizeVolumeComboBox
            // 
            SettingsNormalizeVolumeComboBox.BackColor = Color.Gray;
            SettingsNormalizeVolumeComboBox.DisplayMember = "English";
            SettingsNormalizeVolumeComboBox.Dock = DockStyle.Fill;
            SettingsNormalizeVolumeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsNormalizeVolumeComboBox.FlatStyle = FlatStyle.Flat;
            SettingsNormalizeVolumeComboBox.ForeColor = Color.Black;
            SettingsNormalizeVolumeComboBox.FormattingEnabled = true;
            SettingsNormalizeVolumeComboBox.Items.AddRange(new object[] { "No", "Yes" });
            SettingsNormalizeVolumeComboBox.Location = new Point(8, 33);
            SettingsNormalizeVolumeComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsNormalizeVolumeComboBox.MaxDropDownItems = 2;
            SettingsNormalizeVolumeComboBox.Name = "SettingsNormalizeVolumeComboBox";
            SettingsNormalizeVolumeComboBox.Size = new Size(259, 33);
            SettingsNormalizeVolumeComboBox.TabIndex = 0;
            SettingsNormalizeVolumeComboBox.ValueMember = "English";
            // 
            // SettingsTabAlwaysOnTopGroupBox
            // 
            SettingsTabAlwaysOnTopGroupBox.AutoSize = true;
            SettingsTabAlwaysOnTopGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabAlwaysOnTopGroupBox.Controls.Add(SettingsTabAlwaysOnTopComboBox);
            SettingsTabAlwaysOnTopGroupBox.ForeColor = Color.White;
            SettingsTabAlwaysOnTopGroupBox.Location = new Point(4, 99);
            SettingsTabAlwaysOnTopGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabAlwaysOnTopGroupBox.MinimumSize = new Size(214, 84);
            SettingsTabAlwaysOnTopGroupBox.Name = "SettingsTabAlwaysOnTopGroupBox";
            SettingsTabAlwaysOnTopGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabAlwaysOnTopGroupBox.Size = new Size(214, 84);
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
            SettingsTabAlwaysOnTopComboBox.Location = new Point(8, 33);
            SettingsTabAlwaysOnTopComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabAlwaysOnTopComboBox.MaxDropDownItems = 2;
            SettingsTabAlwaysOnTopComboBox.Name = "SettingsTabAlwaysOnTopComboBox";
            SettingsTabAlwaysOnTopComboBox.Size = new Size(198, 33);
            SettingsTabAlwaysOnTopComboBox.TabIndex = 0;
            SettingsTabAlwaysOnTopComboBox.ValueMember = "English";
            // 
            // SettingsTabAutoCloseLyricsGroupBox
            // 
            SettingsTabAutoCloseLyricsGroupBox.AutoSize = true;
            SettingsTabAutoCloseLyricsGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabAutoCloseLyricsGroupBox.Controls.Add(SettingsTabAutoCloseLyricsComboBox);
            SettingsTabAutoCloseLyricsGroupBox.ForeColor = Color.White;
            SettingsTabAutoCloseLyricsGroupBox.Location = new Point(226, 99);
            SettingsTabAutoCloseLyricsGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabAutoCloseLyricsGroupBox.MinimumSize = new Size(238, 84);
            SettingsTabAutoCloseLyricsGroupBox.Name = "SettingsTabAutoCloseLyricsGroupBox";
            SettingsTabAutoCloseLyricsGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabAutoCloseLyricsGroupBox.Size = new Size(238, 84);
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
            SettingsTabAutoCloseLyricsComboBox.Location = new Point(8, 33);
            SettingsTabAutoCloseLyricsComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabAutoCloseLyricsComboBox.MaxDropDownItems = 2;
            SettingsTabAutoCloseLyricsComboBox.Name = "SettingsTabAutoCloseLyricsComboBox";
            SettingsTabAutoCloseLyricsComboBox.Size = new Size(222, 33);
            SettingsTabAutoCloseLyricsComboBox.TabIndex = 0;
            SettingsTabAutoCloseLyricsComboBox.ValueMember = "English";
            // 
            // SettingsTabDisplayLiveLyricsGroupBox
            // 
            SettingsTabDisplayLiveLyricsGroupBox.AutoSize = true;
            SettingsTabDisplayLiveLyricsGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabDisplayLiveLyricsGroupBox.Controls.Add(SettingsTabDisplayLiveLyricsComboBox);
            SettingsTabDisplayLiveLyricsGroupBox.ForeColor = Color.White;
            SettingsTabDisplayLiveLyricsGroupBox.Location = new Point(472, 99);
            SettingsTabDisplayLiveLyricsGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabDisplayLiveLyricsGroupBox.MinimumSize = new Size(238, 84);
            SettingsTabDisplayLiveLyricsGroupBox.Name = "SettingsTabDisplayLiveLyricsGroupBox";
            SettingsTabDisplayLiveLyricsGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabDisplayLiveLyricsGroupBox.Size = new Size(238, 84);
            SettingsTabDisplayLiveLyricsGroupBox.TabIndex = 2;
            SettingsTabDisplayLiveLyricsGroupBox.TabStop = false;
            SettingsTabDisplayLiveLyricsGroupBox.Text = "Display Live Lyrics";
            // 
            // SettingsTabDisplayLiveLyricsComboBox
            // 
            SettingsTabDisplayLiveLyricsComboBox.BackColor = Color.Gray;
            SettingsTabDisplayLiveLyricsComboBox.DisplayMember = "English";
            SettingsTabDisplayLiveLyricsComboBox.Dock = DockStyle.Fill;
            SettingsTabDisplayLiveLyricsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SettingsTabDisplayLiveLyricsComboBox.FlatStyle = FlatStyle.Flat;
            SettingsTabDisplayLiveLyricsComboBox.ForeColor = Color.Black;
            SettingsTabDisplayLiveLyricsComboBox.FormattingEnabled = true;
            SettingsTabDisplayLiveLyricsComboBox.Items.AddRange(new object[] { "No", "Yes" });
            SettingsTabDisplayLiveLyricsComboBox.Location = new Point(8, 33);
            SettingsTabDisplayLiveLyricsComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabDisplayLiveLyricsComboBox.MaxDropDownItems = 2;
            SettingsTabDisplayLiveLyricsComboBox.Name = "SettingsTabDisplayLiveLyricsComboBox";
            SettingsTabDisplayLiveLyricsComboBox.Size = new Size(222, 33);
            SettingsTabDisplayLiveLyricsComboBox.TabIndex = 0;
            SettingsTabDisplayLiveLyricsComboBox.ValueMember = "English";
            // 
            // SettingsTabConvGroupBox
            // 
            SettingsTabConvGroupBox.AutoSize = true;
            SettingsTabConvGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabConvGroupBox.Controls.Add(SettingsTabConvTableLayoutPanel);
            SettingsTabConvGroupBox.ForeColor = Color.White;
            SettingsTabConvGroupBox.Location = new Point(718, 99);
            SettingsTabConvGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabConvGroupBox.MinimumSize = new Size(375, 125);
            SettingsTabConvGroupBox.Name = "SettingsTabConvGroupBox";
            SettingsTabConvGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabConvGroupBox.Size = new Size(375, 125);
            SettingsTabConvGroupBox.TabIndex = 1;
            SettingsTabConvGroupBox.TabStop = false;
            SettingsTabConvGroupBox.Text = "Convertion";
            // 
            // SettingsTabConvTableLayoutPanel
            // 
            SettingsTabConvTableLayoutPanel.ColumnCount = 2;
            SettingsTabConvTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            SettingsTabConvTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvModeLabel, 0, 0);
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvModeComboBox, 1, 0);
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvQualityLabel, 0, 1);
            SettingsTabConvTableLayoutPanel.Controls.Add(SettingsTabConvQualityComboBox, 1, 1);
            SettingsTabConvTableLayoutPanel.Dock = DockStyle.Fill;
            SettingsTabConvTableLayoutPanel.Location = new Point(8, 33);
            SettingsTabConvTableLayoutPanel.Margin = new Padding(4);
            SettingsTabConvTableLayoutPanel.Name = "SettingsTabConvTableLayoutPanel";
            SettingsTabConvTableLayoutPanel.RowCount = 2;
            SettingsTabConvTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            SettingsTabConvTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            SettingsTabConvTableLayoutPanel.Size = new Size(359, 83);
            SettingsTabConvTableLayoutPanel.TabIndex = 0;
            // 
            // SettingsTabConvModeLabel
            // 
            SettingsTabConvModeLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabConvModeLabel.AutoSize = true;
            SettingsTabConvModeLabel.Location = new Point(4, 8);
            SettingsTabConvModeLabel.Margin = new Padding(4, 0, 4, 0);
            SettingsTabConvModeLabel.Name = "SettingsTabConvModeLabel";
            SettingsTabConvModeLabel.Size = new Size(117, 25);
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
            SettingsTabConvModeComboBox.Location = new Point(129, 5);
            SettingsTabConvModeComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabConvModeComboBox.MaxDropDownItems = 2;
            SettingsTabConvModeComboBox.Name = "SettingsTabConvModeComboBox";
            SettingsTabConvModeComboBox.Size = new Size(226, 33);
            SettingsTabConvModeComboBox.TabIndex = 1;
            SettingsTabConvModeComboBox.ValueMember = "English";
            // 
            // SettingsTabConvQualityLabel
            // 
            SettingsTabConvQualityLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabConvQualityLabel.AutoSize = true;
            SettingsTabConvQualityLabel.Location = new Point(4, 49);
            SettingsTabConvQualityLabel.Margin = new Padding(4, 0, 4, 0);
            SettingsTabConvQualityLabel.Name = "SettingsTabConvQualityLabel";
            SettingsTabConvQualityLabel.Size = new Size(117, 25);
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
            SettingsTabConvQualityComboBox.Location = new Point(129, 46);
            SettingsTabConvQualityComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabConvQualityComboBox.MaxDropDownItems = 5;
            SettingsTabConvQualityComboBox.Name = "SettingsTabConvQualityComboBox";
            SettingsTabConvQualityComboBox.Size = new Size(226, 33);
            SettingsTabConvQualityComboBox.TabIndex = 2;
            SettingsTabConvQualityComboBox.ValueMember = "English";
            // 
            // SettingsTabLibraryGroupBox
            // 
            SettingsTabLibraryGroupBox.AutoSize = true;
            SettingsTabLibraryGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabLibraryGroupBox.Controls.Add(SettingsTabLibraryTableLayoutPanel);
            SettingsTabLibraryGroupBox.ForeColor = Color.White;
            SettingsTabLibraryGroupBox.Location = new Point(4, 234);
            SettingsTabLibraryGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabLibraryGroupBox.MinimumSize = new Size(475, 125);
            SettingsTabLibraryGroupBox.Name = "SettingsTabLibraryGroupBox";
            SettingsTabLibraryGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabLibraryGroupBox.Size = new Size(475, 167);
            SettingsTabLibraryGroupBox.TabIndex = 1;
            SettingsTabLibraryGroupBox.TabStop = false;
            SettingsTabLibraryGroupBox.Text = "Library";
            // 
            // SettingsTabLibraryTableLayoutPanel
            // 
            SettingsTabLibraryTableLayoutPanel.ColumnCount = 2;
            SettingsTabLibraryTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SettingsTabLibraryTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryFolderTextBox, 0, 0);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryFolderButton, 1, 0);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryUnixHiddenFileLabel, 0, 1);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryUnixHiddenFileComboBox, 1, 1);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryWindowsHiddenFileLabel, 0, 2);
            SettingsTabLibraryTableLayoutPanel.Controls.Add(SettingsTabLibraryWindowsHiddenFileComboBox, 1, 2);
            SettingsTabLibraryTableLayoutPanel.Dock = DockStyle.Top;
            SettingsTabLibraryTableLayoutPanel.Location = new Point(8, 33);
            SettingsTabLibraryTableLayoutPanel.Margin = new Padding(4);
            SettingsTabLibraryTableLayoutPanel.Name = "SettingsTabLibraryTableLayoutPanel";
            SettingsTabLibraryTableLayoutPanel.RowCount = 4;
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            SettingsTabLibraryTableLayoutPanel.RowStyles.Add(new RowStyle());
            SettingsTabLibraryTableLayoutPanel.Size = new Size(459, 125);
            SettingsTabLibraryTableLayoutPanel.TabIndex = 0;
            // 
            // SettingsTabLibraryFolderTextBox
            // 
            SettingsTabLibraryFolderTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabLibraryFolderTextBox.BackColor = Color.Gray;
            SettingsTabLibraryFolderTextBox.BorderStyle = BorderStyle.FixedSingle;
            SettingsTabLibraryFolderTextBox.ForeColor = Color.White;
            SettingsTabLibraryFolderTextBox.Location = new Point(4, 4);
            SettingsTabLibraryFolderTextBox.Margin = new Padding(4);
            SettingsTabLibraryFolderTextBox.Name = "SettingsTabLibraryFolderTextBox";
            SettingsTabLibraryFolderTextBox.ReadOnly = true;
            SettingsTabLibraryFolderTextBox.Size = new Size(326, 31);
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
            SettingsTabLibraryFolderButton.Location = new Point(371, 4);
            SettingsTabLibraryFolderButton.Margin = new Padding(4);
            SettingsTabLibraryFolderButton.Name = "SettingsTabLibraryFolderButton";
            SettingsTabLibraryFolderButton.Size = new Size(50, 32);
            SettingsTabLibraryFolderButton.TabIndex = 3;
            SettingsTabLibraryFolderButton.Text = ". . .";
            SettingsTabLibraryFolderButton.UseVisualStyleBackColor = false;
            // 
            // SettingsTabLibraryUnixHiddenFileLabel
            // 
            SettingsTabLibraryUnixHiddenFileLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabLibraryUnixHiddenFileLabel.AutoSize = true;
            SettingsTabLibraryUnixHiddenFileLabel.Location = new Point(4, 47);
            SettingsTabLibraryUnixHiddenFileLabel.Margin = new Padding(4, 0, 4, 0);
            SettingsTabLibraryUnixHiddenFileLabel.Name = "SettingsTabLibraryUnixHiddenFileLabel";
            SettingsTabLibraryUnixHiddenFileLabel.Size = new Size(326, 25);
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
            SettingsTabLibraryUnixHiddenFileComboBox.Location = new Point(338, 45);
            SettingsTabLibraryUnixHiddenFileComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabLibraryUnixHiddenFileComboBox.MaxDropDownItems = 2;
            SettingsTabLibraryUnixHiddenFileComboBox.Name = "SettingsTabLibraryUnixHiddenFileComboBox";
            SettingsTabLibraryUnixHiddenFileComboBox.Size = new Size(117, 33);
            SettingsTabLibraryUnixHiddenFileComboBox.TabIndex = 1;
            SettingsTabLibraryUnixHiddenFileComboBox.ValueMember = "English";
            // 
            // SettingsTabLibraryWindowsHiddenFileLabel
            // 
            SettingsTabLibraryWindowsHiddenFileLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabLibraryWindowsHiddenFileLabel.AutoSize = true;
            SettingsTabLibraryWindowsHiddenFileLabel.Location = new Point(4, 87);
            SettingsTabLibraryWindowsHiddenFileLabel.Margin = new Padding(4, 0, 4, 0);
            SettingsTabLibraryWindowsHiddenFileLabel.Name = "SettingsTabLibraryWindowsHiddenFileLabel";
            SettingsTabLibraryWindowsHiddenFileLabel.Size = new Size(326, 25);
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
            SettingsTabLibraryWindowsHiddenFileComboBox.Location = new Point(338, 85);
            SettingsTabLibraryWindowsHiddenFileComboBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabLibraryWindowsHiddenFileComboBox.MaxDropDownItems = 2;
            SettingsTabLibraryWindowsHiddenFileComboBox.Name = "SettingsTabLibraryWindowsHiddenFileComboBox";
            SettingsTabLibraryWindowsHiddenFileComboBox.Size = new Size(117, 33);
            SettingsTabLibraryWindowsHiddenFileComboBox.TabIndex = 2;
            SettingsTabLibraryWindowsHiddenFileComboBox.ValueMember = "English";
            // 
            // SettingsTabEqualizerGroupBox
            // 
            SettingsTabEqualizerGroupBox.AutoSize = true;
            SettingsTabEqualizerGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabEqualizerGroupBox.Controls.Add(SettingsTabEqualizerTableLayoutPanel);
            SettingsTabEqualizerGroupBox.ForeColor = Color.White;
            SettingsTabEqualizerGroupBox.Location = new Point(4, 411);
            SettingsTabEqualizerGroupBox.Margin = new Padding(4, 5, 4, 5);
            SettingsTabEqualizerGroupBox.MinimumSize = new Size(750, 125);
            SettingsTabEqualizerGroupBox.Name = "SettingsTabEqualizerGroupBox";
            SettingsTabEqualizerGroupBox.Padding = new Padding(8, 9, 8, 9);
            SettingsTabEqualizerGroupBox.Size = new Size(750, 354);
            SettingsTabEqualizerGroupBox.TabIndex = 1;
            SettingsTabEqualizerGroupBox.TabStop = false;
            SettingsTabEqualizerGroupBox.Text = "Library";
            // 
            // SettingsTabEqualizerTableLayoutPanel
            // 
            SettingsTabEqualizerTableLayoutPanel.ColumnCount = 2;
            SettingsTabEqualizerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            SettingsTabEqualizerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            SettingsTabEqualizerTableLayoutPanel.Controls.Add(SettingsTabEqualizerComboBox, 0, 0);
            SettingsTabEqualizerTableLayoutPanel.Controls.Add(SettingsTabEqualizerButton, 1, 0);
            SettingsTabEqualizerTableLayoutPanel.Controls.Add(SettingsTabEqualizerTableLayoutPanel2, 0, 1);
            SettingsTabEqualizerTableLayoutPanel.Dock = DockStyle.Top;
            SettingsTabEqualizerTableLayoutPanel.Location = new Point(8, 33);
            SettingsTabEqualizerTableLayoutPanel.Margin = new Padding(4);
            SettingsTabEqualizerTableLayoutPanel.Name = "SettingsTabEqualizerTableLayoutPanel";
            SettingsTabEqualizerTableLayoutPanel.RowCount = 2;
            SettingsTabEqualizerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            SettingsTabEqualizerTableLayoutPanel.RowStyles.Add(new RowStyle());
            SettingsTabEqualizerTableLayoutPanel.Size = new Size(734, 312);
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
            SettingsTabEqualizerComboBox.Location = new Point(4, 2);
            SettingsTabEqualizerComboBox.Margin = new Padding(4, 2, 4, 4);
            SettingsTabEqualizerComboBox.MaxDropDownItems = 10;
            SettingsTabEqualizerComboBox.Name = "SettingsTabEqualizerComboBox";
            SettingsTabEqualizerComboBox.Size = new Size(242, 33);
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
            SettingsTabEqualizerButton.Location = new Point(250, 0);
            SettingsTabEqualizerButton.Margin = new Padding(0);
            SettingsTabEqualizerButton.MinimumSize = new Size(188, 0);
            SettingsTabEqualizerButton.Name = "SettingsTabEqualizerButton";
            SettingsTabEqualizerButton.Size = new Size(188, 37);
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
            SettingsTabEqualizerTableLayoutPanel2.Location = new Point(4, 44);
            SettingsTabEqualizerTableLayoutPanel2.Margin = new Padding(4);
            SettingsTabEqualizerTableLayoutPanel2.Name = "SettingsTabEqualizerTableLayoutPanel2";
            SettingsTabEqualizerTableLayoutPanel2.RowCount = 2;
            SettingsTabEqualizerTableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            SettingsTabEqualizerTableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            SettingsTabEqualizerTableLayoutPanel2.Size = new Size(726, 265);
            SettingsTabEqualizerTableLayoutPanel2.TabIndex = 5;
            // 
            // SettingsTabEqualizerTrackBar01
            // 
            SettingsTabEqualizerTrackBar01.Location = new Point(4, 4);
            SettingsTabEqualizerTrackBar01.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar01.Maximum = 600;
            SettingsTabEqualizerTrackBar01.Minimum = -600;
            SettingsTabEqualizerTrackBar01.Name = "SettingsTabEqualizerTrackBar01";
            SettingsTabEqualizerTrackBar01.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar01.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar01.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar01.TabIndex = 0;
            SettingsTabEqualizerTrackBar01.TickFrequency = 100;
            SettingsTabEqualizerTrackBar01.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar02
            // 
            SettingsTabEqualizerTrackBar02.Location = new Point(76, 4);
            SettingsTabEqualizerTrackBar02.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar02.Maximum = 600;
            SettingsTabEqualizerTrackBar02.Minimum = -600;
            SettingsTabEqualizerTrackBar02.Name = "SettingsTabEqualizerTrackBar02";
            SettingsTabEqualizerTrackBar02.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar02.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar02.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar02.TabIndex = 0;
            SettingsTabEqualizerTrackBar02.TickFrequency = 100;
            SettingsTabEqualizerTrackBar02.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar03
            // 
            SettingsTabEqualizerTrackBar03.Location = new Point(148, 4);
            SettingsTabEqualizerTrackBar03.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar03.Maximum = 600;
            SettingsTabEqualizerTrackBar03.Minimum = -600;
            SettingsTabEqualizerTrackBar03.Name = "SettingsTabEqualizerTrackBar03";
            SettingsTabEqualizerTrackBar03.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar03.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar03.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar03.TabIndex = 0;
            SettingsTabEqualizerTrackBar03.TickFrequency = 100;
            SettingsTabEqualizerTrackBar03.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar04
            // 
            SettingsTabEqualizerTrackBar04.Location = new Point(220, 4);
            SettingsTabEqualizerTrackBar04.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar04.Maximum = 600;
            SettingsTabEqualizerTrackBar04.Minimum = -600;
            SettingsTabEqualizerTrackBar04.Name = "SettingsTabEqualizerTrackBar04";
            SettingsTabEqualizerTrackBar04.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar04.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar04.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar04.TabIndex = 0;
            SettingsTabEqualizerTrackBar04.TickFrequency = 100;
            SettingsTabEqualizerTrackBar04.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar05
            // 
            SettingsTabEqualizerTrackBar05.Location = new Point(292, 4);
            SettingsTabEqualizerTrackBar05.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar05.Maximum = 600;
            SettingsTabEqualizerTrackBar05.Minimum = -600;
            SettingsTabEqualizerTrackBar05.Name = "SettingsTabEqualizerTrackBar05";
            SettingsTabEqualizerTrackBar05.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar05.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar05.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar05.TabIndex = 0;
            SettingsTabEqualizerTrackBar05.TickFrequency = 100;
            SettingsTabEqualizerTrackBar05.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar06
            // 
            SettingsTabEqualizerTrackBar06.Location = new Point(364, 4);
            SettingsTabEqualizerTrackBar06.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar06.Maximum = 600;
            SettingsTabEqualizerTrackBar06.Minimum = -600;
            SettingsTabEqualizerTrackBar06.Name = "SettingsTabEqualizerTrackBar06";
            SettingsTabEqualizerTrackBar06.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar06.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar06.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar06.TabIndex = 0;
            SettingsTabEqualizerTrackBar06.TickFrequency = 100;
            SettingsTabEqualizerTrackBar06.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar07
            // 
            SettingsTabEqualizerTrackBar07.Location = new Point(436, 4);
            SettingsTabEqualizerTrackBar07.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar07.Maximum = 600;
            SettingsTabEqualizerTrackBar07.Minimum = -600;
            SettingsTabEqualizerTrackBar07.Name = "SettingsTabEqualizerTrackBar07";
            SettingsTabEqualizerTrackBar07.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar07.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar07.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar07.TabIndex = 0;
            SettingsTabEqualizerTrackBar07.TickFrequency = 100;
            SettingsTabEqualizerTrackBar07.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar08
            // 
            SettingsTabEqualizerTrackBar08.Location = new Point(508, 4);
            SettingsTabEqualizerTrackBar08.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar08.Maximum = 600;
            SettingsTabEqualizerTrackBar08.Minimum = -600;
            SettingsTabEqualizerTrackBar08.Name = "SettingsTabEqualizerTrackBar08";
            SettingsTabEqualizerTrackBar08.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar08.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar08.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar08.TabIndex = 0;
            SettingsTabEqualizerTrackBar08.TickFrequency = 100;
            SettingsTabEqualizerTrackBar08.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar09
            // 
            SettingsTabEqualizerTrackBar09.Location = new Point(580, 4);
            SettingsTabEqualizerTrackBar09.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar09.Maximum = 600;
            SettingsTabEqualizerTrackBar09.Minimum = -600;
            SettingsTabEqualizerTrackBar09.Name = "SettingsTabEqualizerTrackBar09";
            SettingsTabEqualizerTrackBar09.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar09.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar09.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar09.TabIndex = 0;
            SettingsTabEqualizerTrackBar09.TickFrequency = 100;
            SettingsTabEqualizerTrackBar09.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerTrackBar10
            // 
            SettingsTabEqualizerTrackBar10.Location = new Point(652, 4);
            SettingsTabEqualizerTrackBar10.Margin = new Padding(4);
            SettingsTabEqualizerTrackBar10.Maximum = 600;
            SettingsTabEqualizerTrackBar10.Minimum = -600;
            SettingsTabEqualizerTrackBar10.Name = "SettingsTabEqualizerTrackBar10";
            SettingsTabEqualizerTrackBar10.Orientation = Orientation.Vertical;
            SettingsTabEqualizerTrackBar10.RightToLeft = RightToLeft.No;
            SettingsTabEqualizerTrackBar10.Size = new Size(56, 207);
            SettingsTabEqualizerTrackBar10.TabIndex = 0;
            SettingsTabEqualizerTrackBar10.TickFrequency = 100;
            SettingsTabEqualizerTrackBar10.TickStyle = TickStyle.TopLeft;
            // 
            // SettingsTabEqualizerLabel01
            // 
            SettingsTabEqualizerLabel01.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel01.AutoSize = true;
            SettingsTabEqualizerLabel01.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel01.Location = new Point(4, 230);
            SettingsTabEqualizerLabel01.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel01.Name = "SettingsTabEqualizerLabel01";
            SettingsTabEqualizerLabel01.Size = new Size(64, 20);
            SettingsTabEqualizerLabel01.TabIndex = 1;
            SettingsTabEqualizerLabel01.Text = "-00.00";
            SettingsTabEqualizerLabel01.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel02
            // 
            SettingsTabEqualizerLabel02.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel02.AutoSize = true;
            SettingsTabEqualizerLabel02.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel02.Location = new Point(76, 230);
            SettingsTabEqualizerLabel02.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel02.Name = "SettingsTabEqualizerLabel02";
            SettingsTabEqualizerLabel02.Size = new Size(64, 20);
            SettingsTabEqualizerLabel02.TabIndex = 1;
            SettingsTabEqualizerLabel02.Text = "-00.00";
            SettingsTabEqualizerLabel02.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel03
            // 
            SettingsTabEqualizerLabel03.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel03.AutoSize = true;
            SettingsTabEqualizerLabel03.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel03.Location = new Point(148, 230);
            SettingsTabEqualizerLabel03.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel03.Name = "SettingsTabEqualizerLabel03";
            SettingsTabEqualizerLabel03.Size = new Size(64, 20);
            SettingsTabEqualizerLabel03.TabIndex = 1;
            SettingsTabEqualizerLabel03.Text = "-00.00";
            SettingsTabEqualizerLabel03.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel04
            // 
            SettingsTabEqualizerLabel04.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel04.AutoSize = true;
            SettingsTabEqualizerLabel04.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel04.Location = new Point(220, 230);
            SettingsTabEqualizerLabel04.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel04.Name = "SettingsTabEqualizerLabel04";
            SettingsTabEqualizerLabel04.Size = new Size(64, 20);
            SettingsTabEqualizerLabel04.TabIndex = 1;
            SettingsTabEqualizerLabel04.Text = "-00.00";
            SettingsTabEqualizerLabel04.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel05
            // 
            SettingsTabEqualizerLabel05.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel05.AutoSize = true;
            SettingsTabEqualizerLabel05.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel05.Location = new Point(292, 230);
            SettingsTabEqualizerLabel05.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel05.Name = "SettingsTabEqualizerLabel05";
            SettingsTabEqualizerLabel05.Size = new Size(64, 20);
            SettingsTabEqualizerLabel05.TabIndex = 1;
            SettingsTabEqualizerLabel05.Text = "-00.00";
            SettingsTabEqualizerLabel05.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel06
            // 
            SettingsTabEqualizerLabel06.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel06.AutoSize = true;
            SettingsTabEqualizerLabel06.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel06.Location = new Point(364, 230);
            SettingsTabEqualizerLabel06.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel06.Name = "SettingsTabEqualizerLabel06";
            SettingsTabEqualizerLabel06.Size = new Size(64, 20);
            SettingsTabEqualizerLabel06.TabIndex = 1;
            SettingsTabEqualizerLabel06.Text = "-00.00";
            SettingsTabEqualizerLabel06.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel07
            // 
            SettingsTabEqualizerLabel07.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel07.AutoSize = true;
            SettingsTabEqualizerLabel07.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel07.Location = new Point(436, 230);
            SettingsTabEqualizerLabel07.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel07.Name = "SettingsTabEqualizerLabel07";
            SettingsTabEqualizerLabel07.Size = new Size(64, 20);
            SettingsTabEqualizerLabel07.TabIndex = 1;
            SettingsTabEqualizerLabel07.Text = "-00.00";
            SettingsTabEqualizerLabel07.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel08
            // 
            SettingsTabEqualizerLabel08.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel08.AutoSize = true;
            SettingsTabEqualizerLabel08.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel08.Location = new Point(508, 230);
            SettingsTabEqualizerLabel08.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel08.Name = "SettingsTabEqualizerLabel08";
            SettingsTabEqualizerLabel08.Size = new Size(64, 20);
            SettingsTabEqualizerLabel08.TabIndex = 1;
            SettingsTabEqualizerLabel08.Text = "-00.00";
            SettingsTabEqualizerLabel08.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel09
            // 
            SettingsTabEqualizerLabel09.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel09.AutoSize = true;
            SettingsTabEqualizerLabel09.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel09.Location = new Point(580, 230);
            SettingsTabEqualizerLabel09.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel09.Name = "SettingsTabEqualizerLabel09";
            SettingsTabEqualizerLabel09.Size = new Size(64, 20);
            SettingsTabEqualizerLabel09.TabIndex = 1;
            SettingsTabEqualizerLabel09.Text = "-00.00";
            SettingsTabEqualizerLabel09.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsTabEqualizerLabel10
            // 
            SettingsTabEqualizerLabel10.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            SettingsTabEqualizerLabel10.AutoSize = true;
            SettingsTabEqualizerLabel10.Font = new Font("Segoe UI", 9F);
            SettingsTabEqualizerLabel10.Location = new Point(652, 230);
            SettingsTabEqualizerLabel10.Margin = new Padding(4, 0, 4, 0);
            SettingsTabEqualizerLabel10.Name = "SettingsTabEqualizerLabel10";
            SettingsTabEqualizerLabel10.Size = new Size(70, 20);
            SettingsTabEqualizerLabel10.TabIndex = 1;
            SettingsTabEqualizerLabel10.Text = "-00.00";
            SettingsTabEqualizerLabel10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(BtnScheduller, 0, 7);
            tableLayoutPanel3.Controls.Add(BtnOpen, 0, 0);
            tableLayoutPanel3.Controls.Add(BtnPrevious, 0, 1);
            tableLayoutPanel3.Controls.Add(BtnPlayPause, 0, 2);
            tableLayoutPanel3.Controls.Add(BtnNext, 0, 3);
            tableLayoutPanel3.Controls.Add(BtnRepeat, 0, 4);
            tableLayoutPanel3.Controls.Add(BtnShuffle, 0, 5);
            tableLayoutPanel3.Controls.Add(BtnClearList, 0, 6);
            tableLayoutPanel3.Controls.Add(GridScanMetadata, 0, 8);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(1159, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 9;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(75, 657);
            tableLayoutPanel3.TabIndex = 6;
            // 
            // BtnScheduller
            // 
            BtnScheduller.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            BtnScheduller.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnScheduller.BackColor = Color.FromArgb(30, 30, 30);
            BtnScheduller.BackgroundImage = Properties.Resources.album_large;
            BtnScheduller.BackgroundImageLayout = ImageLayout.Center;
            BtnScheduller.Cursor = Cursors.Hand;
            BtnScheduller.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            BtnScheduller.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
            BtnScheduller.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
            BtnScheduller.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            BtnScheduller.FlatStyle = FlatStyle.Flat;
            BtnScheduller.ForeColor = Color.White;
            BtnScheduller.Location = new Point(12, 417);
            BtnScheduller.Margin = new Padding(2, 4, 2, 4);
            BtnScheduller.Name = "BtnScheduller";
            BtnScheduller.Size = new Size(51, 51);
            BtnScheduller.TabIndex = 2;
            BtnScheduller.UseVisualStyleBackColor = false;
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
            BtnOpen.Location = new Point(12, 4);
            BtnOpen.Margin = new Padding(2, 4, 2, 4);
            BtnOpen.Name = "BtnOpen";
            BtnOpen.Size = new Size(51, 51);
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
            BtnPrevious.Location = new Point(12, 63);
            BtnPrevious.Margin = new Padding(2, 4, 2, 4);
            BtnPrevious.Name = "BtnPrevious";
            BtnPrevious.Size = new Size(51, 51);
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
            BtnPlayPause.Location = new Point(12, 122);
            BtnPlayPause.Margin = new Padding(2, 4, 2, 4);
            BtnPlayPause.Name = "BtnPlayPause";
            BtnPlayPause.Size = new Size(51, 51);
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
            BtnNext.Location = new Point(12, 181);
            BtnNext.Margin = new Padding(2, 4, 2, 4);
            BtnNext.Name = "BtnNext";
            BtnNext.Size = new Size(51, 51);
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
            BtnRepeat.Location = new Point(12, 240);
            BtnRepeat.Margin = new Padding(2, 4, 2, 4);
            BtnRepeat.Name = "BtnRepeat";
            BtnRepeat.Size = new Size(51, 51);
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
            BtnShuffle.Location = new Point(12, 299);
            BtnShuffle.Margin = new Padding(2, 4, 2, 4);
            BtnShuffle.Name = "BtnShuffle";
            BtnShuffle.Size = new Size(51, 51);
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
            BtnClearList.Location = new Point(12, 358);
            BtnClearList.Margin = new Padding(2, 4, 2, 4);
            BtnClearList.Name = "BtnClearList";
            BtnClearList.Size = new Size(51, 51);
            BtnClearList.TabIndex = 0;
            BtnClearList.UseVisualStyleBackColor = false;
            // 
            // GridScanMetadata
            // 
            GridScanMetadata.ColumnCount = 1;
            GridScanMetadata.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            GridScanMetadata.Controls.Add(textBox1, 0, 0);
            GridScanMetadata.Controls.Add(pictureBox1, 0, 1);
            GridScanMetadata.Controls.Add(GridScanMetadataNb, 0, 2);
            GridScanMetadata.Dock = DockStyle.Bottom;
            GridScanMetadata.Location = new Point(0, 508);
            GridScanMetadata.Margin = new Padding(0);
            GridScanMetadata.Name = "GridScanMetadata";
            GridScanMetadata.RowCount = 3;
            GridScanMetadata.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            GridScanMetadata.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            GridScanMetadata.RowStyles.Add(new RowStyle());
            GridScanMetadata.Size = new Size(75, 149);
            GridScanMetadata.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(30, 30, 30);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Dock = DockStyle.Fill;
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(4, 4);
            textBox1.Margin = new Padding(4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(67, 54);
            textBox1.TabIndex = 0;
            textBox1.Text = "Scan Tags";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(0, 62);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(75, 75);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // GridScanMetadataNb
            // 
            GridScanMetadataNb.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            GridScanMetadataNb.AutoSize = true;
            GridScanMetadataNb.BackColor = Color.Transparent;
            GridScanMetadataNb.ForeColor = Color.White;
            GridScanMetadataNb.Location = new Point(4, 137);
            GridScanMetadataNb.Margin = new Padding(4, 0, 4, 0);
            GridScanMetadataNb.Name = "GridScanMetadataNb";
            GridScanMetadataNb.Size = new Size(67, 25);
            GridScanMetadataNb.TabIndex = 2;
            GridScanMetadataNb.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LyricsTextBox
            // 
            LyricsTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            LyricsTextBox.AutoSize = true;
            LyricsTextBox.BackColor = Color.DimGray;
            LyricsTextBox.ForeColor = Color.White;
            LyricsTextBox.Location = new Point(5, 740);
            LyricsTextBox.Margin = new Padding(4, 0, 4, 0);
            LyricsTextBox.Name = "LyricsTextBox";
            LyricsTextBox.Size = new Size(1226, 25);
            LyricsTextBox.TabIndex = 7;
            LyricsTextBox.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // OverPanel
            // 
            OverPanel.BackColor = Color.FromArgb(50, 50, 50, 50);
            OverPanel.Controls.Add(OverPanelLabel);
            OverPanel.Dock = DockStyle.Top;
            OverPanel.Location = new Point(1, 1);
            OverPanel.Margin = new Padding(4);
            OverPanel.Name = "OverPanel";
            OverPanel.Size = new Size(1236, 0);
            OverPanel.TabIndex = 0;
            // 
            // OverPanelLabel
            // 
            OverPanelLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            OverPanelLabel.AutoSize = true;
            OverPanelLabel.BackColor = Color.Transparent;
            OverPanelLabel.Font = new Font("Segoe UI", 14F);
            OverPanelLabel.ForeColor = Color.White;
            OverPanelLabel.Location = new Point(528, -54);
            OverPanelLabel.Margin = new Padding(4, 0, 4, 0);
            OverPanelLabel.Name = "OverPanelLabel";
            OverPanelLabel.Size = new Size(43, 32);
            OverPanelLabel.TabIndex = 0;
            OverPanelLabel.Text = ". . .";
            OverPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainWindow2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            ClientSize = new Size(1238, 862);
            ControlBox = false;
            Controls.Add(OverPanel);
            Controls.Add(GlobalTableLayoutPanel);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(786, 862);
            Name = "MainWindow2";
            Padding = new Padding(1);
            StartPosition = FormStartPosition.Manual;
            Text = "MainWindow";
            SizeChanged += MainWindow2_SizeChanged;
            GlobalTableLayoutPanel.ResumeLayout(false);
            GlobalTableLayoutPanel.PerformLayout();
            MainWIndowHead.ResumeLayout(false);
            MainWIndowHead.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
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
            LibraryFiltersGrid.ResumeLayout(false);
            LibraryFiltersGrid.PerformLayout();
            LibraryTabSplitContainer.Panel1.ResumeLayout(false);
            LibraryTabSplitContainer.Panel1.PerformLayout();
            LibraryTabSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)LibraryTabSplitContainer).EndInit();
            LibraryTabSplitContainer.ResumeLayout(false);
            LibraryNavigationContent.ResumeLayout(false);
            LibraryNavigationContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LibraryNavigationContentFolders).EndInit();
            LibraryTabSplitContainer2.Panel1.ResumeLayout(false);
            LibraryTabSplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)LibraryTabSplitContainer2).EndInit();
            LibraryTabSplitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)LibrarySearchContent).EndInit();
            LibraryNavigationContentFilesParent.ResumeLayout(false);
            PlayListsTab.ResumeLayout(false);
            PlayListsTabTableLayoutPanel.ResumeLayout(false);
            PlayListsTabSplitContainer1.Panel1.ResumeLayout(false);
            PlayListsTabSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PlayListsTabSplitContainer1).EndInit();
            PlayListsTabSplitContainer1.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            PlayListsTabSplitContainer2.Panel1.ResumeLayout(false);
            PlayListsTabSplitContainer2.Panel2.ResumeLayout(false);
            PlayListsTabSplitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PlayListsTabSplitContainer2).EndInit();
            PlayListsTabSplitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PlayListsTabDataGridView).EndInit();
            PlayListsTabRadioPanel.ResumeLayout(false);
            PlayListsTabRadioPanel.PerformLayout();
            PlayListsTabRadioPanelLeft.ResumeLayout(false);
            PlayListsTabRadioPanelFlowLayoutPanel.ResumeLayout(false);
            PlayListsTabRadioPanelRight.ResumeLayout(false);
            PlayListsTabRadioPanelRight.PerformLayout();
            SettingsTab.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            SettingsTabLangGroupBox.ResumeLayout(false);
            SettingsTabStyleGroupBox.ResumeLayout(false);
            SettingsTabAutoPlayGroupBox.ResumeLayout(false);
            SettingsNormalizeVolumeGroupBox.ResumeLayout(false);
            SettingsTabAlwaysOnTopGroupBox.ResumeLayout(false);
            SettingsTabAutoCloseLyricsGroupBox.ResumeLayout(false);
            SettingsTabDisplayLiveLyricsGroupBox.ResumeLayout(false);
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
            tableLayoutPanel3.ResumeLayout(false);
            GridScanMetadata.ResumeLayout(false);
            GridScanMetadata.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            OverPanel.ResumeLayout(false);
            OverPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel MainWIndowHead;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel1;
        private Button GripButton;
        private TableLayoutPanel PlaybackTabMainTableLayoutPanel;
        private TableLayoutPanel LibraryTabTableLayoutPanel;
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
        internal Manina.Windows.Forms.TabControl TabControler;
        internal GroupBox SettingsTabLangGroupBox;
        internal ComboBox SettingsTabLangComboBox;
        internal Label PlaybackTabTitleLabelInfo;
        internal Label PlaybackTabAlbumLabelInfo;
        internal Label PlaybackTabArtistsLabelInfo;
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
        private FlowLayoutPanel LibraryFiltersGrid;
        private Panel LibraryNavigationContentScroll;
        internal Label LibraryFiltersModeLabel;
        internal ComboBox LibraryFiltersMode;
        internal ComboBox LibraryFiltersGenreList;
        internal TextBox LibraryFiltersSearchBox;
        internal Label PlaybackTabGenresLabelInfo;
        private Label PlaybackTabGenresLabelValue;
        internal SplitContainer LibraryTabSplitContainer;
        internal FlowLayoutPanel LibraryNavigationPathContener;
        private Panel OverPanel;
        private Label OverPanelLabel;
        private TableLayoutPanel GridScanMetadata;
        private TextBox textBox1;
        private PictureBox pictureBox1;
        private Label GridScanMetadataNb;
        internal PlaybackProgressBar playbackProgressBar;
        private Rating2 PlaybackTabRatting;
        private Panel panel3;
        internal Label TitleLabel;
        internal TableLayoutPanel LibraryNavigationContent;
        private ImageList PlayListsTabTreeImageList;
        internal TreeView PlaylistsTree;
        internal SplitContainer PlayListsTabSplitContainer1;
        internal SplitContainer PlayListsTabSplitContainer2;
        internal TableLayoutPanel tableLayoutPanel5;
        internal DataGridView PlayListsTabDataGridView;
        private DataGridViewTextBoxColumn ColumnPlayCount;
        private DataGridViewTextBoxColumn PlayListsColumnName;
        private DataGridViewTextBoxColumn PlayListsColumnArtists;
        private DataGridViewTextBoxColumn PlayListsColumnAlbum;
        private DataGridViewTextBoxColumn PlayListsColumnDuration;
        private DataGridViewImageColumn PlayListsColumnRating;
        private DataGridViewTextBoxColumn SelectedColumn;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn DurationColumn;
        private DataGridViewTextBoxColumn ArtistsColumn;
        private DataGridViewTextBoxColumn AlbumColumn;
        internal DataGridView PlaybackTabDataGridView;
        internal TableLayoutPanel PlayListsTabTableLayoutPanel;
        internal Label PlayListsTabVoidLabel;
        private TableLayoutPanel PlayListsTabRadioPanelLeft;
        internal FlowLayoutPanel PlayListsTabRadioPanelFlowLayoutPanel;
        internal Button PlayListsTabRadioPanelIcon;
        internal Button PlayListsTabRadioButton;
        internal Button button2;
        internal TableLayoutPanel PlayListsTabRadioPanelRight;
        internal Label PlayListsTabRadioPanelTitleLabel;
        internal Label PlayListsTabRadioPanelRightPanelDescriptionLabel;
        internal TableLayoutPanel PlayListsTabRadioPanel;
        internal DataGridView LibraryNavigationContentFolders;
        internal Label LibraryFoldersLabel;
        internal DataGridView LibrarySearchContent;
        internal SplitContainer LibraryTabSplitContainer2;
        internal Panel LibraryNavigationContentFilesParent;
        internal TableLayoutPanel LibraryNavigationContentFiles;
        public Button BtnScheduller;
        private Label LyricsTextBox;
        internal GroupBox SettingsTabDisplayLiveLyricsGroupBox;
        internal ComboBox SettingsTabDisplayLiveLyricsComboBox;
        internal GroupBox SettingsNormalizeVolumeGroupBox;
        internal ComboBox SettingsNormalizeVolumeComboBox;
    }
}