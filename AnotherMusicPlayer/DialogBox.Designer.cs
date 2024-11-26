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
            MessageBlock = new System.Windows.Forms.Label();
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
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(TitleLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            tableLayoutPanel1.Size = new System.Drawing.Size(586, 271);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // TitleLabel
            // 
            TitleLabel.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TitleLabel.AutoSize = true;
            TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            TitleLabel.ForeColor = System.Drawing.Color.White;
            TitleLabel.Location = new System.Drawing.Point(3, 11);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new System.Drawing.Size(580, 28);
            TitleLabel.TabIndex = 0;
            TitleLabel.Text = "label1";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(MessageIcon, 0, 0);
            tableLayoutPanel2.Controls.Add(MessageBlock, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 53);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new System.Drawing.Size(580, 135);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // MessageIcon
            // 
            MessageIcon.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MessageIcon.BackgroundImage = Properties.Resources.dialog_warning;
            MessageIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            MessageIcon.FlatAppearance.BorderSize = 0;
            MessageIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            MessageIcon.Location = new System.Drawing.Point(3, 20);
            MessageIcon.Name = "MessageIcon";
            MessageIcon.Size = new System.Drawing.Size(94, 94);
            MessageIcon.TabIndex = 0;
            MessageIcon.UseVisualStyleBackColor = true;
            // 
            // MessageBlock
            // 
            MessageBlock.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            MessageBlock.AutoSize = true;
            MessageBlock.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            MessageBlock.ForeColor = System.Drawing.Color.White;
            MessageBlock.Location = new System.Drawing.Point(110, 42);
            MessageBlock.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            MessageBlock.Name = "MessageBlock";
            MessageBlock.Size = new System.Drawing.Size(460, 50);
            MessageBlock.TabIndex = 1;
            MessageBlock.Text = "text hyper long de test pour tester le multiligne\r\net voire si ça passe";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(BtnNo);
            flowLayoutPanel1.Controls.Add(BtnYes);
            flowLayoutPanel1.Controls.Add(BtnCancel);
            flowLayoutPanel1.Controls.Add(BtnOK);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 194);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(580, 74);
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
            BtnNo.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BtnNo.ForeColor = System.Drawing.Color.White;
            BtnNo.Location = new System.Drawing.Point(493, 3);
            BtnNo.MinimumSize = new System.Drawing.Size(0, 68);
            BtnNo.Name = "BtnNo";
            BtnNo.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            BtnNo.Size = new System.Drawing.Size(84, 68);
            BtnNo.TabIndex = 0;
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
            BtnYes.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BtnYes.ForeColor = System.Drawing.Color.White;
            BtnYes.Location = new System.Drawing.Point(400, 3);
            BtnYes.MinimumSize = new System.Drawing.Size(0, 68);
            BtnYes.Name = "BtnYes";
            BtnYes.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            BtnYes.Size = new System.Drawing.Size(87, 68);
            BtnYes.TabIndex = 0;
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
            BtnCancel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BtnCancel.ForeColor = System.Drawing.Color.White;
            BtnCancel.Location = new System.Drawing.Point(272, 3);
            BtnCancel.MinimumSize = new System.Drawing.Size(0, 68);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            BtnCancel.Size = new System.Drawing.Size(122, 68);
            BtnCancel.TabIndex = 0;
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
            BtnOK.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            BtnOK.ForeColor = System.Drawing.Color.White;
            BtnOK.Location = new System.Drawing.Point(182, 3);
            BtnOK.MinimumSize = new System.Drawing.Size(0, 68);
            BtnOK.Name = "BtnOK";
            BtnOK.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            BtnOK.Size = new System.Drawing.Size(84, 68);
            BtnOK.TabIndex = 0;
            BtnOK.Text = "OK";
            BtnOK.UseVisualStyleBackColor = false;
            // 
            // DialogBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ClientSize = new System.Drawing.Size(586, 271);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "DialogBox";
            Text = "DialogBox";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button MessageIcon;
        private System.Windows.Forms.Label MessageBlock;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button BtnNo;
        private System.Windows.Forms.Button BtnYes;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOK;
    }
}