namespace AnotherMusicPlayer
{
    partial class EditRadio
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
            GlobalTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            MainWIndowHead = new System.Windows.Forms.TableLayoutPanel();
            TitleLabel = new System.Windows.Forms.Label();
            CloseButton = new System.Windows.Forms.Button();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            ValidateButton = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            CategoryLabel = new System.Windows.Forms.Label();
            CategoryComboBox = new System.Windows.Forms.ComboBox();
            NameLabel = new System.Windows.Forms.Label();
            NameTextBox = new System.Windows.Forms.TextBox();
            UrlLabel = new System.Windows.Forms.Label();
            UrlTextBox = new System.Windows.Forms.TextBox();
            UrlPrefixLabel = new System.Windows.Forms.Label();
            UrlPrefixTextBox = new System.Windows.Forms.TextBox();
            DescriptionLabel = new System.Windows.Forms.Label();
            DescriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            LogoButton = new System.Windows.Forms.Button();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            BtnClear = new System.Windows.Forms.Button();
            LogoButtonAction = new System.Windows.Forms.Button();
            ModeLabel = new System.Windows.Forms.Label();
            ModeBoolPresenter = new CustomControl.BoolPresenter();
            GlobalTableLayoutPanel.SuspendLayout();
            MainWIndowHead.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // GlobalTableLayoutPanel
            // 
            GlobalTableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            GlobalTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            GlobalTableLayoutPanel.ColumnCount = 1;
            GlobalTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            GlobalTableLayoutPanel.Controls.Add(MainWIndowHead, 0, 0);
            GlobalTableLayoutPanel.Controls.Add(tableLayoutPanel4, 0, 1);
            GlobalTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            GlobalTableLayoutPanel.Location = new System.Drawing.Point(1, 1);
            GlobalTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            GlobalTableLayoutPanel.Name = "GlobalTableLayoutPanel";
            GlobalTableLayoutPanel.RowCount = 2;
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            GlobalTableLayoutPanel.Size = new System.Drawing.Size(648, 548);
            GlobalTableLayoutPanel.TabIndex = 1;
            // 
            // MainWIndowHead
            // 
            MainWIndowHead.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            MainWIndowHead.ColumnCount = 2;
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            MainWIndowHead.Controls.Add(TitleLabel, 0, 0);
            MainWIndowHead.Controls.Add(CloseButton, 1, 0);
            MainWIndowHead.Dock = System.Windows.Forms.DockStyle.Fill;
            MainWIndowHead.Location = new System.Drawing.Point(1, 1);
            MainWIndowHead.Margin = new System.Windows.Forms.Padding(0);
            MainWIndowHead.Name = "MainWIndowHead";
            MainWIndowHead.RowCount = 1;
            MainWIndowHead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainWIndowHead.Size = new System.Drawing.Size(646, 51);
            MainWIndowHead.TabIndex = 3;
            MainWIndowHead.Tag = "WindowHead";
            // 
            // TitleLabel
            // 
            TitleLabel.AutoEllipsis = true;
            TitleLabel.AutoSize = true;
            TitleLabel.BackColor = System.Drawing.Color.Transparent;
            TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TitleLabel.ForeColor = System.Drawing.Color.White;
            TitleLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            TitleLabel.Location = new System.Drawing.Point(5, 0);
            TitleLabel.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(591, 51);
            TitleLabel.TabIndex = 0;
            TitleLabel.Tag = "Title";
            TitleLabel.Text = "Edit WebRadio Category";
            TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CloseButton
            // 
            CloseButton.BackColor = System.Drawing.Color.Gray;
            CloseButton.BackgroundImage = Properties.Resources.window_close_icon;
            CloseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            CloseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            CloseButton.Dock = System.Windows.Forms.DockStyle.Fill;
            CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            CloseButton.Location = new System.Drawing.Point(599, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(44, 45);
            CloseButton.TabIndex = 3;
            CloseButton.Tag = "WindowButton";
            CloseButton.UseVisualStyleBackColor = false;
            CloseButton.Click += CloseButton_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(ValidateButton, 0, 2);
            tableLayoutPanel4.Controls.Add(tableLayoutPanel1, 2, 0);
            tableLayoutPanel4.Controls.Add(LogoButton, 0, 0);
            tableLayoutPanel4.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel4.Location = new System.Drawing.Point(1, 53);
            tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            tableLayoutPanel4.Size = new System.Drawing.Size(646, 494);
            tableLayoutPanel4.TabIndex = 8;
            // 
            // ValidateButton
            // 
            ValidateButton.BackColor = System.Drawing.Color.ForestGreen;
            ValidateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            ValidateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            ValidateButton.Dock = System.Windows.Forms.DockStyle.Right;
            ValidateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            ValidateButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ValidateButton.ForeColor = System.Drawing.Color.White;
            ValidateButton.Location = new System.Drawing.Point(3, 427);
            ValidateButton.MinimumSize = new System.Drawing.Size(150, 0);
            ValidateButton.Name = "ValidateButton";
            ValidateButton.Size = new System.Drawing.Size(150, 64);
            ValidateButton.TabIndex = 10;
            ValidateButton.Tag = "WindowButton";
            ValidateButton.Text = "&Validate";
            ValidateButton.UseVisualStyleBackColor = false;
            ValidateButton.Click += ValidateButton_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(CategoryLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(CategoryComboBox, 0, 1);
            tableLayoutPanel1.Controls.Add(NameLabel, 0, 2);
            tableLayoutPanel1.Controls.Add(NameTextBox, 0, 3);
            tableLayoutPanel1.Controls.Add(UrlLabel, 0, 4);
            tableLayoutPanel1.Controls.Add(UrlTextBox, 0, 5);
            tableLayoutPanel1.Controls.Add(UrlPrefixLabel, 0, 6);
            tableLayoutPanel1.Controls.Add(UrlPrefixTextBox, 0, 7);
            tableLayoutPanel1.Controls.Add(DescriptionLabel, 0, 8);
            tableLayoutPanel1.Controls.Add(DescriptionRichTextBox, 0, 9);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(162, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 10;
            tableLayoutPanel4.SetRowSpan(tableLayoutPanel1, 3);
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(481, 488);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // CategoryLabel
            // 
            CategoryLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            CategoryLabel.AutoSize = true;
            CategoryLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            CategoryLabel.ForeColor = System.Drawing.Color.White;
            CategoryLabel.Location = new System.Drawing.Point(3, 8);
            CategoryLabel.Name = "CategoryLabel";
            CategoryLabel.Size = new System.Drawing.Size(110, 32);
            CategoryLabel.TabIndex = 0;
            CategoryLabel.Tag = "Title";
            CategoryLabel.Text = "Category";
            // 
            // CategoryComboBox
            // 
            CategoryComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CategoryComboBox.BackColor = System.Drawing.Color.DarkGray;
            CategoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CategoryComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CategoryComboBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            CategoryComboBox.FormattingEnabled = true;
            CategoryComboBox.Location = new System.Drawing.Point(3, 43);
            CategoryComboBox.Name = "CategoryComboBox";
            CategoryComboBox.Size = new System.Drawing.Size(475, 36);
            CategoryComboBox.TabIndex = 3;
            // 
            // NameLabel
            // 
            NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            NameLabel.AutoSize = true;
            NameLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NameLabel.ForeColor = System.Drawing.Color.White;
            NameLabel.Location = new System.Drawing.Point(3, 88);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(78, 32);
            NameLabel.TabIndex = 0;
            NameLabel.Tag = "Title";
            NameLabel.Text = "Name";
            // 
            // NameTextBox
            // 
            NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            NameTextBox.BackColor = System.Drawing.Color.DarkGray;
            NameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            NameTextBox.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NameTextBox.Location = new System.Drawing.Point(3, 124);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new System.Drawing.Size(475, 32);
            NameTextBox.TabIndex = 1;
            // 
            // UrlLabel
            // 
            UrlLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            UrlLabel.AutoSize = true;
            UrlLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            UrlLabel.ForeColor = System.Drawing.Color.White;
            UrlLabel.Location = new System.Drawing.Point(3, 168);
            UrlLabel.Name = "UrlLabel";
            UrlLabel.Size = new System.Drawing.Size(44, 32);
            UrlLabel.TabIndex = 0;
            UrlLabel.Tag = "Title";
            UrlLabel.Text = "Url";
            // 
            // UrlTextBox
            // 
            UrlTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UrlTextBox.BackColor = System.Drawing.Color.DarkGray;
            UrlTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            UrlTextBox.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            UrlTextBox.Location = new System.Drawing.Point(3, 204);
            UrlTextBox.Name = "UrlTextBox";
            UrlTextBox.Size = new System.Drawing.Size(475, 32);
            UrlTextBox.TabIndex = 4;
            // 
            // UrlPrefixLabel
            // 
            UrlPrefixLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            UrlPrefixLabel.AutoSize = true;
            UrlPrefixLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            UrlPrefixLabel.ForeColor = System.Drawing.Color.White;
            UrlPrefixLabel.Location = new System.Drawing.Point(3, 248);
            UrlPrefixLabel.Name = "UrlPrefixLabel";
            UrlPrefixLabel.Size = new System.Drawing.Size(110, 32);
            UrlPrefixLabel.TabIndex = 5;
            UrlPrefixLabel.Tag = "Title";
            UrlPrefixLabel.Text = "Url Prefix";
            // 
            // UrlPrefixTextBox
            // 
            UrlPrefixTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UrlPrefixTextBox.BackColor = System.Drawing.Color.DarkGray;
            UrlPrefixTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            UrlPrefixTextBox.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            UrlPrefixTextBox.Location = new System.Drawing.Point(3, 284);
            UrlPrefixTextBox.Name = "UrlPrefixTextBox";
            UrlPrefixTextBox.Size = new System.Drawing.Size(475, 32);
            UrlPrefixTextBox.TabIndex = 4;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            DescriptionLabel.ForeColor = System.Drawing.Color.White;
            DescriptionLabel.Location = new System.Drawing.Point(3, 328);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new System.Drawing.Size(135, 32);
            DescriptionLabel.TabIndex = 0;
            DescriptionLabel.Tag = "Title";
            DescriptionLabel.Text = "Description";
            // 
            // DescriptionRichTextBox
            // 
            DescriptionRichTextBox.BackColor = System.Drawing.Color.DarkGray;
            DescriptionRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            DescriptionRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            DescriptionRichTextBox.Location = new System.Drawing.Point(3, 363);
            DescriptionRichTextBox.Name = "DescriptionRichTextBox";
            DescriptionRichTextBox.Size = new System.Drawing.Size(475, 122);
            DescriptionRichTextBox.TabIndex = 2;
            DescriptionRichTextBox.Text = "";
            // 
            // LogoButton
            // 
            LogoButton.BackColor = System.Drawing.Color.Black;
            LogoButton.BackgroundImage = Properties.Resources.radio_icon;
            LogoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            LogoButton.Enabled = false;
            LogoButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            LogoButton.FlatAppearance.BorderSize = 0;
            LogoButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            LogoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            LogoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            LogoButton.Location = new System.Drawing.Point(3, 3);
            LogoButton.MaximumSize = new System.Drawing.Size(150, 150);
            LogoButton.MinimumSize = new System.Drawing.Size(150, 150);
            LogoButton.Name = "LogoButton";
            LogoButton.Size = new System.Drawing.Size(150, 150);
            LogoButton.TabIndex = 4;
            LogoButton.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(tableLayoutPanel3);
            flowLayoutPanel1.Controls.Add(ModeLabel);
            flowLayoutPanel1.Controls.Add(ModeBoolPresenter);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 159);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(150, 129);
            flowLayoutPanel1.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(BtnClear, 2, 0);
            tableLayoutPanel3.Controls.Add(LogoButtonAction, 1, 0);
            tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new System.Drawing.Size(150, 63);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // BtnClear
            // 
            BtnClear.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BtnClear.AutoSize = true;
            BtnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            BtnClear.FlatAppearance.BorderColor = System.Drawing.Color.White;
            BtnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(90, 90, 90);
            BtnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnClear.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BtnClear.ForeColor = System.Drawing.Color.White;
            BtnClear.Location = new System.Drawing.Point(78, 11);
            BtnClear.MaximumSize = new System.Drawing.Size(50, 40);
            BtnClear.MinimumSize = new System.Drawing.Size(50, 40);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new System.Drawing.Size(50, 40);
            BtnClear.TabIndex = 7;
            BtnClear.Text = "☒";
            BtnClear.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            BtnClear.UseVisualStyleBackColor = true;
            BtnClear.Click += BtnClear_Click;
            // 
            // LogoButtonAction
            // 
            LogoButtonAction.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LogoButtonAction.AutoSize = true;
            LogoButtonAction.Cursor = System.Windows.Forms.Cursors.Hand;
            LogoButtonAction.FlatAppearance.BorderColor = System.Drawing.Color.White;
            LogoButtonAction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(90, 90, 90);
            LogoButtonAction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            LogoButtonAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            LogoButtonAction.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LogoButtonAction.ForeColor = System.Drawing.Color.White;
            LogoButtonAction.Location = new System.Drawing.Point(22, 11);
            LogoButtonAction.MaximumSize = new System.Drawing.Size(50, 40);
            LogoButtonAction.MinimumSize = new System.Drawing.Size(50, 40);
            LogoButtonAction.Name = "LogoButtonAction";
            LogoButtonAction.Size = new System.Drawing.Size(50, 40);
            LogoButtonAction.TabIndex = 6;
            LogoButtonAction.Text = "···";
            LogoButtonAction.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            LogoButtonAction.UseVisualStyleBackColor = true;
            LogoButtonAction.Click += LogoButtonAction_Click;
            // 
            // ModeLabel
            // 
            ModeLabel.AutoSize = true;
            ModeLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ModeLabel.ForeColor = System.Drawing.Color.White;
            ModeLabel.Location = new System.Drawing.Point(3, 63);
            ModeLabel.Name = "ModeLabel";
            ModeLabel.Size = new System.Drawing.Size(61, 25);
            ModeLabel.TabIndex = 0;
            ModeLabel.Tag = "Title";
            ModeLabel.Text = "Mode";
            // 
            // ModeBoolPresenter
            // 
            ModeBoolPresenter.AutoSize = true;
            ModeBoolPresenter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ModeBoolPresenter.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ModeBoolPresenter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ModeBoolPresenter.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            ModeBoolPresenter.Location = new System.Drawing.Point(0, 88);
            ModeBoolPresenter.Margin = new System.Windows.Forms.Padding(0);
            ModeBoolPresenter.MaximumSize = new System.Drawing.Size(11428571, 59);
            ModeBoolPresenter.MinimumSize = new System.Drawing.Size(140, 41);
            ModeBoolPresenter.Name = "ModeBoolPresenter";
            ModeBoolPresenter.Size = new System.Drawing.Size(140, 41);
            ModeBoolPresenter.StringFalse = "M3u";
            ModeBoolPresenter.StringTrue = "Stream";
            ModeBoolPresenter.TabIndex = 6;
            ModeBoolPresenter.Value = true;
            // 
            // EditRadio
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(650, 550);
            Controls.Add(GlobalTableLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximumSize = new System.Drawing.Size(650, 550);
            MinimumSize = new System.Drawing.Size(650, 550);
            Name = "EditRadio";
            Padding = new System.Windows.Forms.Padding(1);
            ShowInTaskbar = false;
            Text = "AddPlayList";
            GlobalTableLayoutPanel.ResumeLayout(false);
            MainWIndowHead.ResumeLayout(false);
            MainWIndowHead.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        internal System.Windows.Forms.TableLayoutPanel GlobalTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel MainWIndowHead;
        internal System.Windows.Forms.Label TitleLabel;
        internal System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button LogoButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button LogoButtonAction;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label ModeLabel;
        private System.Windows.Forms.Label CategoryLabel;
        private System.Windows.Forms.ComboBox CategoryComboBox;
        private System.Windows.Forms.Label UrlLabel;
        private System.Windows.Forms.TextBox UrlTextBox;
        private System.Windows.Forms.Label UrlPrefixLabel;
        private System.Windows.Forms.TextBox UrlPrefixTextBox;
        private System.Windows.Forms.RichTextBox DescriptionRichTextBox;
        internal System.Windows.Forms.Button ValidateButton;
        private CustomControl.BoolPresenter ModeBoolPresenter;
    }
}