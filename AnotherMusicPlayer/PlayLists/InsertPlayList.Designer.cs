namespace AnotherMusicPlayer
{
    partial class InsertPlayList
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
            CustomListLabel = new System.Windows.Forms.Label();
            CustomListComboBox = new System.Windows.Forms.ComboBox();
            CustomListButton = new System.Windows.Forms.Button();
            GlobalTableLayoutPanel.SuspendLayout();
            MainWIndowHead.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
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
            GlobalTableLayoutPanel.Size = new System.Drawing.Size(648, 168);
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
            ValidateButton.Location = new System.Drawing.Point(444, 110);
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
            TitleLabel.Text = "Playlist Selection";
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
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.Controls.Add(CustomListLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(CustomListComboBox, 1, 0);
            tableLayoutPanel1.Controls.Add(CustomListButton, 2, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(4, 55);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(640, 49);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // CustomListLabel
            // 
            CustomListLabel.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CustomListLabel.AutoSize = true;
            CustomListLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            CustomListLabel.ForeColor = System.Drawing.Color.White;
            CustomListLabel.Location = new System.Drawing.Point(3, 9);
            CustomListLabel.Name = "CustomListLabel";
            CustomListLabel.Size = new System.Drawing.Size(194, 32);
            CustomListLabel.TabIndex = 0;
            CustomListLabel.Tag = "Title";
            CustomListLabel.Text = "label1";
            // 
            // CustomListComboBox
            // 
            CustomListComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CustomListComboBox.BackColor = System.Drawing.Color.DarkGray;
            CustomListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CustomListComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CustomListComboBox.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            CustomListComboBox.FormattingEnabled = true;
            CustomListComboBox.Location = new System.Drawing.Point(200, 5);
            CustomListComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            CustomListComboBox.Name = "CustomListComboBox";
            CustomListComboBox.Size = new System.Drawing.Size(387, 39);
            CustomListComboBox.TabIndex = 1;
            // 
            // CustomListButton
            // 
            CustomListButton.BackColor = System.Drawing.Color.Gray;
            CustomListButton.Dock = System.Windows.Forms.DockStyle.Fill;
            CustomListButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            CustomListButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            CustomListButton.ForeColor = System.Drawing.Color.White;
            CustomListButton.Location = new System.Drawing.Point(590, 4);
            CustomListButton.Margin = new System.Windows.Forms.Padding(0, 4, 0, 5);
            CustomListButton.Name = "CustomListButton";
            CustomListButton.Size = new System.Drawing.Size(50, 41);
            CustomListButton.TabIndex = 2;
            CustomListButton.Text = ". . . ";
            CustomListButton.UseVisualStyleBackColor = false;
            CustomListButton.Click += CustomListButton_Click;
            // 
            // InsertPlayList
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(650, 170);
            Controls.Add(GlobalTableLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximumSize = new System.Drawing.Size(650, 170);
            MinimumSize = new System.Drawing.Size(650, 170);
            Name = "InsertPlayList";
            Padding = new System.Windows.Forms.Padding(1);
            ShowInTaskbar = false;
            Text = "AddPlayList";
            GlobalTableLayoutPanel.ResumeLayout(false);
            MainWIndowHead.ResumeLayout(false);
            MainWIndowHead.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        internal System.Windows.Forms.TableLayoutPanel GlobalTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel MainWIndowHead;
        internal System.Windows.Forms.Label TitleLabel;
        internal System.Windows.Forms.Button CloseButton;
        internal System.Windows.Forms.Button ValidateButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label CustomListLabel;
        private System.Windows.Forms.ComboBox CustomListComboBox;
        private System.Windows.Forms.Button CustomListButton;
    }
}