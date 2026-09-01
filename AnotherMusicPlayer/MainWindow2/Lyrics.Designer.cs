namespace AnotherMusicPlayer.MainWindow2Space
{
    partial class Lyrics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lyrics));
            GripButton = new System.Windows.Forms.Button();
            TitleLabel = new System.Windows.Forms.Label();
            MinimizeButton = new System.Windows.Forms.Button();
            MaximizeButton = new System.Windows.Forms.Button();
            CloseButton = new System.Windows.Forms.Button();
            WindowIconButton = new System.Windows.Forms.Button();
            MainWIndowHead = new System.Windows.Forms.TableLayoutPanel();
            GlobalTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            MainWIndowHead.SuspendLayout();
            GlobalTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // GripButton
            // 
            GripButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            GripButton.BackgroundImage = Properties.Resources.Grip_transparent;
            GripButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            GripButton.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            GripButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 30, 30);
            GripButton.FlatAppearance.BorderSize = 0;
            GripButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            GripButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            GripButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            GripButton.Location = new System.Drawing.Point(791, 651);
            GripButton.Margin = new System.Windows.Forms.Padding(0);
            GripButton.Name = "GripButton";
            GripButton.Size = new System.Drawing.Size(26, 30);
            GripButton.TabIndex = 1;
            // 
            // TitleLabel
            // 
            TitleLabel.AutoEllipsis = true;
            TitleLabel.AutoSize = true;
            TitleLabel.BackColor = System.Drawing.Color.Transparent;
            TitleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F);
            TitleLabel.ForeColor = System.Drawing.Color.White;
            TitleLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            TitleLabel.Location = new System.Drawing.Point(68, 0);
            TitleLabel.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(561, 64);
            TitleLabel.TabIndex = 0;
            TitleLabel.Tag = "Title";
            TitleLabel.Text = "Title";
            TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinimizeButton
            // 
            MinimizeButton.BackColor = System.Drawing.Color.Gray;
            MinimizeButton.BackgroundImage = Properties.Resources.window_minimize_icon;
            MinimizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            MinimizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            MinimizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            MinimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            MinimizeButton.Location = new System.Drawing.Point(633, 4);
            MinimizeButton.Margin = new System.Windows.Forms.Padding(4);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Size = new System.Drawing.Size(54, 56);
            MinimizeButton.TabIndex = 1;
            MinimizeButton.Tag = "WindowButton";
            MinimizeButton.UseVisualStyleBackColor = false;
            // 
            // MaximizeButton
            // 
            MaximizeButton.BackColor = System.Drawing.Color.Gray;
            MaximizeButton.BackgroundImage = Properties.Resources.window_maximize_icon;
            MaximizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            MaximizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            MaximizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            MaximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            MaximizeButton.Location = new System.Drawing.Point(695, 4);
            MaximizeButton.Margin = new System.Windows.Forms.Padding(4);
            MaximizeButton.Name = "MaximizeButton";
            MaximizeButton.Size = new System.Drawing.Size(54, 56);
            MaximizeButton.TabIndex = 2;
            MaximizeButton.Tag = "WindowButton";
            MaximizeButton.UseVisualStyleBackColor = false;
            // 
            // CloseButton
            // 
            CloseButton.BackColor = System.Drawing.Color.Gray;
            CloseButton.BackgroundImage = Properties.Resources.window_close_icon;
            CloseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            CloseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            CloseButton.Dock = System.Windows.Forms.DockStyle.Fill;
            CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            CloseButton.Location = new System.Drawing.Point(757, 4);
            CloseButton.Margin = new System.Windows.Forms.Padding(4);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new System.Drawing.Size(54, 56);
            CloseButton.TabIndex = 3;
            CloseButton.Tag = "WindowButton";
            CloseButton.UseVisualStyleBackColor = false;
            // 
            // WindowIconButton
            // 
            WindowIconButton.BackgroundImage = Properties.Resources.album_large;
            WindowIconButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            WindowIconButton.Dock = System.Windows.Forms.DockStyle.Fill;
            WindowIconButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 30, 30);
            WindowIconButton.FlatAppearance.BorderSize = 0;
            WindowIconButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            WindowIconButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            WindowIconButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            WindowIconButton.Location = new System.Drawing.Point(0, 0);
            WindowIconButton.Margin = new System.Windows.Forms.Padding(0);
            WindowIconButton.Name = "WindowIconButton";
            WindowIconButton.Size = new System.Drawing.Size(62, 64);
            WindowIconButton.TabIndex = 4;
            WindowIconButton.UseVisualStyleBackColor = true;
            // 
            // MainWIndowHead
            // 
            MainWIndowHead.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            MainWIndowHead.ColumnCount = 5;
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            MainWIndowHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            MainWIndowHead.Controls.Add(TitleLabel, 1, 0);
            MainWIndowHead.Controls.Add(MinimizeButton, 2, 0);
            MainWIndowHead.Controls.Add(MaximizeButton, 3, 0);
            MainWIndowHead.Controls.Add(CloseButton, 4, 0);
            MainWIndowHead.Controls.Add(WindowIconButton, 0, 0);
            MainWIndowHead.Dock = System.Windows.Forms.DockStyle.Fill;
            MainWIndowHead.Location = new System.Drawing.Point(2, 2);
            MainWIndowHead.Margin = new System.Windows.Forms.Padding(0);
            MainWIndowHead.Name = "MainWIndowHead";
            MainWIndowHead.RowCount = 1;
            MainWIndowHead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainWIndowHead.Size = new System.Drawing.Size(815, 64);
            MainWIndowHead.TabIndex = 3;
            MainWIndowHead.Tag = "WindowHead";
            // 
            // GlobalTableLayoutPanel
            // 
            GlobalTableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            GlobalTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            GlobalTableLayoutPanel.ColumnCount = 1;
            GlobalTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            GlobalTableLayoutPanel.Controls.Add(MainWIndowHead, 0, 0);
            GlobalTableLayoutPanel.Controls.Add(GripButton, 0, 2);
            GlobalTableLayoutPanel.Controls.Add(richTextBox1, 0, 1);
            GlobalTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            GlobalTableLayoutPanel.Location = new System.Drawing.Point(1, 1);
            GlobalTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            GlobalTableLayoutPanel.Name = "GlobalTableLayoutPanel";
            GlobalTableLayoutPanel.Padding = new System.Windows.Forms.Padding(1);
            GlobalTableLayoutPanel.RowCount = 3;
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            GlobalTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            GlobalTableLayoutPanel.Size = new System.Drawing.Size(819, 683);
            GlobalTableLayoutPanel.TabIndex = 1;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            richTextBox1.DetectUrls = false;
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox1.ForeColor = System.Drawing.Color.White;
            richTextBox1.Location = new System.Drawing.Point(6, 71);
            richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ShortcutsEnabled = false;
            richTextBox1.Size = new System.Drawing.Size(807, 567);
            richTextBox1.TabIndex = 4;
            richTextBox1.Text = "";
            // 
            // Lyrics
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(821, 685);
            Controls.Add(GlobalTableLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Lyrics";
            Padding = new System.Windows.Forms.Padding(1);
            Text = "Lyrics";
            MainWIndowHead.ResumeLayout(false);
            MainWIndowHead.PerformLayout();
            GlobalTableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button GripButton;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button MinimizeButton;
        private System.Windows.Forms.Button MaximizeButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button WindowIconButton;
        private System.Windows.Forms.TableLayoutPanel MainWIndowHead;
        private System.Windows.Forms.TableLayoutPanel GlobalTableLayoutPanel;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}