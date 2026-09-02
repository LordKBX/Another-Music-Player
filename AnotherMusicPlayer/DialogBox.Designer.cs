namespace AnotherMusicPlayer
{
    partial class DialogBox
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            TitleLabel = new System.Windows.Forms.Label();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            MessageIcon = new System.Windows.Forms.Button();
            MessageBlock = new System.Windows.Forms.RichTextBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            BtnNo = new System.Windows.Forms.Button();
            BtnYes = new System.Windows.Forms.Button();
            BtnCancel = new System.Windows.Forms.Button();
            BtnOK = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(TitleLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(598, 248);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // TitleLabel
            // 
            TitleLabel.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TitleLabel.AutoSize = true;
            TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            TitleLabel.ForeColor = System.Drawing.Color.White;
            TitleLabel.Location = new System.Drawing.Point(4, 17);
            TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(590, 28);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "label1";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(MessageIcon, 0, 0);
            tableLayoutPanel2.Controls.Add(MessageBlock, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(4, 66);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(590, 128);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // MessageIcon
            // 
            MessageIcon.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MessageIcon.BackgroundImage = Properties.Resources.dialog_warning;
            MessageIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            MessageIcon.FlatAppearance.BorderSize = 0;
            MessageIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            MessageIcon.Location = new System.Drawing.Point(4, 5);
            MessageIcon.Margin = new System.Windows.Forms.Padding(4);
            MessageIcon.Name = "MessageIcon";
            MessageIcon.Size = new System.Drawing.Size(117, 118);
            MessageIcon.TabIndex = 0;
            MessageIcon.UseVisualStyleBackColor = true;
            // 
            // MessageBlock
            // 
            MessageBlock.BackColor = System.Drawing.SystemColors.WindowFrame;
            MessageBlock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            MessageBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            MessageBlock.Location = new System.Drawing.Point(135, 3);
            MessageBlock.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            MessageBlock.Name = "MessageBlock";
            MessageBlock.ReadOnly = true;
            MessageBlock.Size = new System.Drawing.Size(452, 122);
            MessageBlock.TabIndex = 0;
            MessageBlock.Text = "text hyper long de test pour tester le multiligne\net voire si ça passe";
            MessageBlock.ZoomFactor = 1.2F;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(BtnNo);
            flowLayoutPanel1.Controls.Add(BtnYes);
            flowLayoutPanel1.Controls.Add(BtnCancel);
            flowLayoutPanel1.Controls.Add(BtnOK);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new System.Drawing.Point(0, 198);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(598, 50);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // BtnNo
            // 
            BtnNo.AutoSize = true;
            BtnNo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BtnNo.BackColor = System.Drawing.Color.DimGray;
            BtnNo.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            BtnNo.FlatAppearance.BorderSize = 0;
            BtnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            BtnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            BtnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnNo.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            BtnNo.ForeColor = System.Drawing.Color.White;
            BtnNo.Location = new System.Drawing.Point(502, 4);
            BtnNo.Margin = new System.Windows.Forms.Padding(4);
            BtnNo.MinimumSize = new System.Drawing.Size(0, 30);
            BtnNo.Name = "BtnNo";
            BtnNo.Padding = new System.Windows.Forms.Padding(19, 0, 19, 0);
            BtnNo.Size = new System.Drawing.Size(92, 41);
            BtnNo.TabIndex = 0;
            BtnNo.Tag = "CancelButton";
            BtnNo.Text = "No";
            BtnNo.UseVisualStyleBackColor = false;
            // 
            // BtnYes
            // 
            BtnYes.AutoSize = true;
            BtnYes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BtnYes.BackColor = System.Drawing.Color.DimGray;
            BtnYes.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            BtnYes.FlatAppearance.BorderSize = 0;
            BtnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            BtnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            BtnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnYes.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            BtnYes.ForeColor = System.Drawing.Color.White;
            BtnYes.Location = new System.Drawing.Point(399, 4);
            BtnYes.Margin = new System.Windows.Forms.Padding(4);
            BtnYes.MinimumSize = new System.Drawing.Size(0, 30);
            BtnYes.Name = "BtnYes";
            BtnYes.Padding = new System.Windows.Forms.Padding(19, 0, 19, 0);
            BtnYes.Size = new System.Drawing.Size(95, 41);
            BtnYes.TabIndex = 0;
            BtnYes.Tag = "ValidateButton";
            BtnYes.Text = "Yes";
            BtnYes.UseVisualStyleBackColor = false;
            // 
            // BtnCancel
            // 
            BtnCancel.AutoSize = true;
            BtnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BtnCancel.BackColor = System.Drawing.Color.DimGray;
            BtnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            BtnCancel.FlatAppearance.BorderSize = 0;
            BtnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            BtnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnCancel.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            BtnCancel.ForeColor = System.Drawing.Color.White;
            BtnCancel.Location = new System.Drawing.Point(261, 4);
            BtnCancel.Margin = new System.Windows.Forms.Padding(4);
            BtnCancel.MinimumSize = new System.Drawing.Size(0, 30);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Padding = new System.Windows.Forms.Padding(19, 0, 19, 0);
            BtnCancel.Size = new System.Drawing.Size(130, 41);
            BtnCancel.TabIndex = 0;
            BtnCancel.Tag = "CancelButton";
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = false;
            // 
            // BtnOK
            // 
            BtnOK.AutoSize = true;
            BtnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BtnOK.BackColor = System.Drawing.Color.DimGray;
            BtnOK.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            BtnOK.FlatAppearance.BorderSize = 0;
            BtnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkGray;
            BtnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            BtnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnOK.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            BtnOK.ForeColor = System.Drawing.Color.White;
            BtnOK.Location = new System.Drawing.Point(161, 4);
            BtnOK.Margin = new System.Windows.Forms.Padding(4);
            BtnOK.MinimumSize = new System.Drawing.Size(0, 30);
            BtnOK.Name = "BtnOK";
            BtnOK.Padding = new System.Windows.Forms.Padding(19, 0, 19, 0);
            BtnOK.Size = new System.Drawing.Size(92, 41);
            BtnOK.TabIndex = 0;
            BtnOK.Tag = "ValidateButton";
            BtnOK.Text = "OK";
            BtnOK.UseVisualStyleBackColor = false;
            // 
            // DialogBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(600, 250);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "DialogBox";
            Padding = new System.Windows.Forms.Padding(1);
            Text = "DialogBox";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button MessageIcon;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button BtnNo;
        private System.Windows.Forms.Button BtnYes;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.RichTextBox MessageBlock;
    }
}