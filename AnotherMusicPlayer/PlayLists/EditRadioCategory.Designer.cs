namespace AnotherMusicPlayer
{
    partial class EditRadioCategory
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
            ValidateButton = new System.Windows.Forms.Button();
            MainWIndowHead = new System.Windows.Forms.TableLayoutPanel();
            TitleLabel = new System.Windows.Forms.Label();
            CloseButton = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            NameLabel = new System.Windows.Forms.Label();
            NameTextBox = new System.Windows.Forms.TextBox();
            DescriptionLabel = new System.Windows.Forms.Label();
            DescriptionTextBox = new System.Windows.Forms.RichTextBox();
            LogoLabel = new System.Windows.Forms.Label();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            LogoButton = new System.Windows.Forms.Button();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            BtnClear = new System.Windows.Forms.Button();
            LogoButtonAction = new System.Windows.Forms.Button();
            GlobalTableLayoutPanel.SuspendLayout();
            MainWIndowHead.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // GlobalTableLayoutPanel
            // 
            GlobalTableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            GlobalTableLayoutPanel.ColumnCount = 1;
            GlobalTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            GlobalTableLayoutPanel.Controls.Add(ValidateButton, 0, 2);
            GlobalTableLayoutPanel.Controls.Add(MainWIndowHead, 0, 0);
            GlobalTableLayoutPanel.Controls.Add(tableLayoutPanel1, 0, 1);
            GlobalTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            GlobalTableLayoutPanel.Location = new System.Drawing.Point(1, 1);
            GlobalTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            GlobalTableLayoutPanel.Name = "GlobalTableLayoutPanel";
            GlobalTableLayoutPanel.Padding = new System.Windows.Forms.Padding(1);
            GlobalTableLayoutPanel.RowCount = 3;
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            GlobalTableLayoutPanel.Size = new System.Drawing.Size(648, 448);
            GlobalTableLayoutPanel.TabIndex = 1;
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
            ValidateButton.Location = new System.Drawing.Point(444, 390);
            ValidateButton.MinimumSize = new System.Drawing.Size(200, 0);
            ValidateButton.Name = "ValidateButton";
            ValidateButton.Size = new System.Drawing.Size(200, 54);
            ValidateButton.TabIndex = 4;
            ValidateButton.Tag = "WindowButton";
            ValidateButton.Text = "&Validate";
            ValidateButton.UseVisualStyleBackColor = false;
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
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(NameLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(NameTextBox, 1, 0);
            tableLayoutPanel1.Controls.Add(DescriptionLabel, 0, 1);
            tableLayoutPanel1.Controls.Add(DescriptionTextBox, 1, 1);
            tableLayoutPanel1.Controls.Add(LogoLabel, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(1, 52);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            tableLayoutPanel1.Size = new System.Drawing.Size(646, 335);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // NameLabel
            // 
            NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            NameLabel.AutoSize = true;
            NameLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NameLabel.ForeColor = System.Drawing.Color.White;
            NameLabel.Location = new System.Drawing.Point(4, 10);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(194, 32);
            NameLabel.TabIndex = 0;
            NameLabel.Tag = "Title";
            NameLabel.Text = "label1";
            // 
            // NameTextBox
            // 
            NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            NameTextBox.BackColor = System.Drawing.Color.DarkGray;
            NameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            NameTextBox.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NameTextBox.Location = new System.Drawing.Point(205, 10);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new System.Drawing.Size(437, 32);
            NameTextBox.TabIndex = 1;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            DescriptionLabel.ForeColor = System.Drawing.Color.White;
            DescriptionLabel.Location = new System.Drawing.Point(4, 98);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new System.Drawing.Size(194, 32);
            DescriptionLabel.TabIndex = 0;
            DescriptionLabel.Tag = "Title";
            DescriptionLabel.Text = "Description";
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.BackColor = System.Drawing.Color.DarkGray;
            DescriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            DescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            DescriptionTextBox.Location = new System.Drawing.Point(205, 55);
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.Size = new System.Drawing.Size(437, 119);
            DescriptionTextBox.TabIndex = 2;
            DescriptionTextBox.Text = "";
            // 
            // LogoLabel
            // 
            LogoLabel.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LogoLabel.AutoSize = true;
            LogoLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LogoLabel.ForeColor = System.Drawing.Color.White;
            LogoLabel.Location = new System.Drawing.Point(4, 240);
            LogoLabel.Name = "LogoLabel";
            LogoLabel.Size = new System.Drawing.Size(194, 32);
            LogoLabel.TabIndex = 0;
            LogoLabel.Tag = "Title";
            LogoLabel.Text = "Logo";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(LogoButton, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(202, 178);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(443, 156);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // LogoButton
            // 
            LogoButton.BackColor = System.Drawing.Color.Black;
            LogoButton.BackgroundImage = Properties.Resources.dot;
            LogoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
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
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(BtnClear, 2, 0);
            tableLayoutPanel3.Controls.Add(LogoButtonAction, 1, 0);
            tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel3.Location = new System.Drawing.Point(159, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new System.Drawing.Size(281, 150);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // BtnClear
            // 
            BtnClear.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BtnClear.AutoSize = true;
            BtnClear.FlatAppearance.BorderColor = System.Drawing.Color.White;
            BtnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(90, 90, 90);
            BtnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnClear.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BtnClear.ForeColor = System.Drawing.Color.White;
            BtnClear.Location = new System.Drawing.Point(143, 53);
            BtnClear.MaximumSize = new System.Drawing.Size(50, 0);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new System.Drawing.Size(50, 44);
            BtnClear.TabIndex = 7;
            BtnClear.Text = "☒";
            BtnClear.UseVisualStyleBackColor = true;
            BtnClear.Click += BtnClear_Click;
            // 
            // LogoButtonAction
            // 
            LogoButtonAction.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LogoButtonAction.AutoSize = true;
            LogoButtonAction.FlatAppearance.BorderColor = System.Drawing.Color.White;
            LogoButtonAction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(90, 90, 90);
            LogoButtonAction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            LogoButtonAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            LogoButtonAction.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            LogoButtonAction.ForeColor = System.Drawing.Color.White;
            LogoButtonAction.Location = new System.Drawing.Point(87, 53);
            LogoButtonAction.MaximumSize = new System.Drawing.Size(50, 0);
            LogoButtonAction.Name = "LogoButtonAction";
            LogoButtonAction.Size = new System.Drawing.Size(50, 44);
            LogoButtonAction.TabIndex = 6;
            LogoButtonAction.Text = "···";
            LogoButtonAction.UseVisualStyleBackColor = true;
            LogoButtonAction.Click += LogoButtonAction_Click;
            // 
            // EditRadioCategory
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(650, 450);
            Controls.Add(GlobalTableLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximumSize = new System.Drawing.Size(650, 450);
            MinimumSize = new System.Drawing.Size(650, 450);
            Name = "EditRadioCategory";
            Padding = new System.Windows.Forms.Padding(1);
            ShowInTaskbar = false;
            Text = "AddPlayList";
            GlobalTableLayoutPanel.ResumeLayout(false);
            MainWIndowHead.ResumeLayout(false);
            MainWIndowHead.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        internal System.Windows.Forms.TableLayoutPanel GlobalTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel MainWIndowHead;
        internal System.Windows.Forms.Label TitleLabel;
        internal System.Windows.Forms.Button CloseButton;
        internal System.Windows.Forms.Button ValidateButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.RichTextBox DescriptionTextBox;
        private System.Windows.Forms.Label LogoLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button LogoButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button LogoButtonAction;
        private System.Windows.Forms.Button BtnClear;
    }
}