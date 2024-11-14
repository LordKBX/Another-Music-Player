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
            GlobalTableLayoutPanel = new TableLayoutPanel();
            MainWIndowHead = new TableLayoutPanel();
            TitleLabel = new Label();
            MinimizeButton = new Button();
            MaximizeButton = new Button();
            CloseButton = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            panel1 = new Panel();
            label2 = new Label();
            GripButton = new Button();
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
            PlaybackTabTableLayoutPanel = new TableLayoutPanel();
            LibraryTab = new Manina.Windows.Forms.Tab();
            LibraryTabTableLayoutPanel = new TableLayoutPanel();
            PlayListsTab = new Manina.Windows.Forms.Tab();
            TableLayoutPanel = new TableLayoutPanel();
            SettingsTab = new Manina.Windows.Forms.Tab();
            panel2 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            SettingsTabLangGroupBox = new GroupBox();
            SettingsTabLangComboBox = new ComboBox();
            GlobalTableLayoutPanel.SuspendLayout();
            MainWIndowHead.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            TabControler.SuspendLayout();
            PlaybackTab.SuspendLayout();
            LibraryTab.SuspendLayout();
            PlayListsTab.SuspendLayout();
            SettingsTab.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SettingsTabLangGroupBox.SuspendLayout();
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
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            GlobalTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            GlobalTableLayoutPanel.Size = new Size(548, 448);
            GlobalTableLayoutPanel.TabIndex = 0;
            // 
            // MainWIndowHead
            // 
            MainWIndowHead.BackColor = Color.FromArgb(30, 30, 30);
            MainWIndowHead.ColumnCount = 4;
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            MainWIndowHead.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            MainWIndowHead.Controls.Add(TitleLabel, 0, 0);
            MainWIndowHead.Controls.Add(MinimizeButton, 1, 0);
            MainWIndowHead.Controls.Add(MaximizeButton, 2, 0);
            MainWIndowHead.Controls.Add(CloseButton, 3, 0);
            MainWIndowHead.Dock = DockStyle.Fill;
            MainWIndowHead.Location = new Point(1, 1);
            MainWIndowHead.Margin = new Padding(0);
            MainWIndowHead.Name = "MainWIndowHead";
            MainWIndowHead.RowCount = 1;
            MainWIndowHead.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainWIndowHead.Size = new Size(546, 38);
            MainWIndowHead.TabIndex = 3;
            MainWIndowHead.Tag = "WindowHead";
            // 
            // TitleLabel
            // 
            TitleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            TitleLabel.AutoSize = true;
            TitleLabel.BackColor = Color.Transparent;
            TitleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            TitleLabel.ForeColor = Color.White;
            TitleLabel.ImageAlign = ContentAlignment.MiddleLeft;
            TitleLabel.Location = new Point(4, 0);
            TitleLabel.Margin = new Padding(4, 0, 0, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(126, 38);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "Backup Manager";
            TitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MinimizeButton
            // 
            MinimizeButton.BackColor = Color.Gray;
            MinimizeButton.BackgroundImage = Properties.Resources.window_minimize_icon;
            MinimizeButton.BackgroundImageLayout = ImageLayout.Zoom;
            MinimizeButton.Dock = DockStyle.Fill;
            MinimizeButton.FlatStyle = FlatStyle.Popup;
            MinimizeButton.Location = new Point(417, 2);
            MinimizeButton.Margin = new Padding(3, 2, 3, 2);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Size = new Size(38, 34);
            MinimizeButton.TabIndex = 1;
            MinimizeButton.UseVisualStyleBackColor = false;
            MinimizeButton.Click += MinimizeButton_Click;
            // 
            // MaximizeButton
            // 
            MaximizeButton.BackColor = Color.Gray;
            MaximizeButton.BackgroundImage = Properties.Resources.window_maximize_icon;
            MaximizeButton.BackgroundImageLayout = ImageLayout.Zoom;
            MaximizeButton.Dock = DockStyle.Fill;
            MaximizeButton.FlatStyle = FlatStyle.Popup;
            MaximizeButton.Location = new Point(461, 2);
            MaximizeButton.Margin = new Padding(3, 2, 3, 2);
            MaximizeButton.Name = "MaximizeButton";
            MaximizeButton.Size = new Size(38, 34);
            MaximizeButton.TabIndex = 2;
            MaximizeButton.UseVisualStyleBackColor = false;
            MaximizeButton.Click += MaximizeButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.BackColor = Color.Gray;
            CloseButton.BackgroundImage = Properties.Resources.window_close_icon;
            CloseButton.BackgroundImageLayout = ImageLayout.Zoom;
            CloseButton.Dock = DockStyle.Fill;
            CloseButton.FlatStyle = FlatStyle.Popup;
            CloseButton.Location = new Point(505, 2);
            CloseButton.Margin = new Padding(3, 2, 3, 2);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(38, 34);
            CloseButton.TabIndex = 3;
            CloseButton.UseVisualStyleBackColor = false;
            CloseButton.Click += CloseButton_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 2, 0);
            tableLayoutPanel1.Controls.Add(playbackProgressBar, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(1, 402);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(546, 45);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(3, 12);
            label1.Name = "label1";
            label1.Size = new Size(99, 21);
            label1.TabIndex = 0;
            label1.Text = "00:00";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(GripButton);
            panel1.Location = new Point(441, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(105, 45);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(0, 12);
            label2.Name = "label2";
            label2.Size = new Size(102, 21);
            label2.TabIndex = 0;
            label2.Text = "00:00";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // GripButton
            // 
            GripButton.BackgroundImage = Properties.Resources.Grip_transparent;
            GripButton.BackgroundImageLayout = ImageLayout.Zoom;
            GripButton.Cursor = Cursors.SizeNWSE;
            GripButton.FlatAppearance.BorderSize = 0;
            GripButton.FlatStyle = FlatStyle.Flat;
            GripButton.Location = new Point(75, 20);
            GripButton.Margin = new Padding(3, 2, 3, 2);
            GripButton.Name = "GripButton";
            GripButton.Size = new Size(31, 26);
            GripButton.TabIndex = 1;
            GripButton.UseVisualStyleBackColor = false;
            // 
            // playbackProgressBar
            // 
            playbackProgressBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            playbackProgressBar.BorderStyle = BorderStyle.FixedSingle;
            playbackProgressBar.Cursor = Cursors.Hand;
            playbackProgressBar.ForeColor = Color.Silver;
            playbackProgressBar.Location = new Point(108, 13);
            playbackProgressBar.Margin = new Padding(3, 2, 3, 2);
            playbackProgressBar.Name = "playbackProgressBar";
            playbackProgressBar.Size = new Size(330, 19);
            playbackProgressBar.TabIndex = 1;
            playbackProgressBar.Value = 50D;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 61F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel2.Controls.Add(TabControler, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(1, 39);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(546, 363);
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
            tableLayoutPanel3.Location = new Point(485, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 8;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(61, 363);
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
            BtnOpen.Location = new Point(10, 2);
            BtnOpen.Margin = new Padding(2);
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
            BtnPrevious.Location = new Point(10, 47);
            BtnPrevious.Margin = new Padding(2);
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
            BtnPlayPause.Location = new Point(10, 92);
            BtnPlayPause.Margin = new Padding(2);
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
            BtnNext.Location = new Point(10, 137);
            BtnNext.Margin = new Padding(2);
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
            BtnRepeat.Location = new Point(10, 182);
            BtnRepeat.Margin = new Padding(2);
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
            BtnShuffle.Location = new Point(10, 227);
            BtnShuffle.Margin = new Padding(2);
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
            BtnClearList.Location = new Point(10, 272);
            BtnClearList.Margin = new Padding(2);
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
            TabControler.Location = new Point(0, 0);
            TabControler.Margin = new Padding(0);
            TabControler.Name = "TabControler";
            TabControler.Size = new Size(485, 363);
            TabControler.TabIndex = 7;
            TabControler.TabPadding = new Padding(0);
            TabControler.TabSize = new Size(100, 50);
            TabControler.TabSizing = Manina.Windows.Forms.TabSizing.Stretch;
            // 
            // PlaybackTab
            // 
            PlaybackTab.BackColor = Color.Gray;
            PlaybackTab.Controls.Add(PlaybackTabTableLayoutPanel);
            PlaybackTab.ForeColor = Color.White;
            PlaybackTab.Location = new Point(1, 50);
            PlaybackTab.Name = "PlaybackTab";
            PlaybackTab.Size = new Size(483, 312);
            PlaybackTab.Text = " Playback";
            // 
            // PlaybackTabTableLayoutPanel
            // 
            PlaybackTabTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            PlaybackTabTableLayoutPanel.ColumnCount = 2;
            PlaybackTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            PlaybackTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            PlaybackTabTableLayoutPanel.Dock = DockStyle.Fill;
            PlaybackTabTableLayoutPanel.Location = new Point(0, 0);
            PlaybackTabTableLayoutPanel.Margin = new Padding(0);
            PlaybackTabTableLayoutPanel.Name = "PlaybackTabTableLayoutPanel";
            PlaybackTabTableLayoutPanel.RowCount = 1;
            PlaybackTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            PlaybackTabTableLayoutPanel.Size = new Size(483, 312);
            PlaybackTabTableLayoutPanel.TabIndex = 0;
            // 
            // LibraryTab
            // 
            LibraryTab.BackColor = Color.Gray;
            LibraryTab.Controls.Add(LibraryTabTableLayoutPanel);
            LibraryTab.ForeColor = Color.White;
            LibraryTab.Location = new Point(0, 0);
            LibraryTab.Name = "LibraryTab";
            LibraryTab.Size = new Size(0, 0);
            LibraryTab.Text = " Library";
            // 
            // LibraryTabTableLayoutPanel
            // 
            LibraryTabTableLayoutPanel.BackColor = Color.FromArgb(30, 30, 30);
            LibraryTabTableLayoutPanel.ColumnCount = 2;
            LibraryTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            LibraryTabTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            LibraryTabTableLayoutPanel.Dock = DockStyle.Fill;
            LibraryTabTableLayoutPanel.Location = new Point(0, 0);
            LibraryTabTableLayoutPanel.Margin = new Padding(0);
            LibraryTabTableLayoutPanel.Name = "LibraryTabTableLayoutPanel";
            LibraryTabTableLayoutPanel.RowCount = 1;
            LibraryTabTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            LibraryTabTableLayoutPanel.Size = new Size(0, 0);
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
            TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
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
            SettingsTab.Size = new Size(483, 312);
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
            panel2.Size = new Size(483, 312);
            panel2.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(SettingsTabLangGroupBox);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(483, 56);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // SettingsTabLangGroupBox
            // 
            SettingsTabLangGroupBox.AutoSize = true;
            SettingsTabLangGroupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SettingsTabLangGroupBox.Controls.Add(SettingsTabLangComboBox);
            SettingsTabLangGroupBox.ForeColor = Color.White;
            SettingsTabLangGroupBox.Location = new Point(3, 3);
            SettingsTabLangGroupBox.MinimumSize = new Size(150, 50);
            SettingsTabLangGroupBox.Name = "SettingsTabLangGroupBox";
            SettingsTabLangGroupBox.Padding = new Padding(5);
            SettingsTabLangGroupBox.Size = new Size(150, 50);
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
            SettingsTabLangComboBox.Location = new Point(5, 21);
            SettingsTabLangComboBox.MaxDropDownItems = 2;
            SettingsTabLangComboBox.Name = "SettingsTabLangComboBox";
            SettingsTabLangComboBox.Size = new Size(140, 23);
            SettingsTabLangComboBox.TabIndex = 0;
            SettingsTabLangComboBox.ValueMember = "English";
            // 
            // MainWindow2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            ClientSize = new Size(550, 450);
            ControlBox = false;
            Controls.Add(GlobalTableLayoutPanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(550, 450);
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
            LibraryTab.ResumeLayout(false);
            PlayListsTab.ResumeLayout(false);
            SettingsTab.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            SettingsTabLangGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel GlobalTableLayoutPanel;
        private TableLayoutPanel MainWIndowHead;
        private Label TitleLabel;
        private Button MinimizeButton;
        private Button CloseButton;
        private Button MaximizeButton;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Button BtnOpen;
        private Button BtnPrevious;
        private Button BtnPlayPause;
        private Button BtnNext;
        private Button BtnRepeat;
        private Button BtnShuffle;
        private Button BtnClearList;
        private Label label1;
        private Panel panel1;
        private Label label2;
        private Button GripButton;
        private AnotherMusicPlayer.MainWindow2.PlaybackProgressBar playbackProgressBar;
        private Manina.Windows.Forms.TabControl TabControler;
        private Manina.Windows.Forms.Tab PlaybackTab;
        private Manina.Windows.Forms.Tab LibraryTab;
        private Manina.Windows.Forms.Tab PlayListsTab;
        private Manina.Windows.Forms.Tab SettingsTab;
        private TableLayoutPanel PlaybackTabTableLayoutPanel;
        private TableLayoutPanel LibraryTabTableLayoutPanel;
        private TableLayoutPanel TableLayoutPanel;
        private Panel panel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox SettingsTabLangGroupBox;
        private ComboBox SettingsTabLangComboBox;
    }
}