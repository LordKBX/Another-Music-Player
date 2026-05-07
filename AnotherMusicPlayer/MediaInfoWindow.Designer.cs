namespace AnotherMusicPlayer
{
    partial class MediaInfoWindow
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
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            flowLayoutPanelRight = new System.Windows.Forms.FlowLayoutPanel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            Cover = new System.Windows.Forms.Button();
            flowLayoutPanelLeft = new System.Windows.Forms.FlowLayoutPanel();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(flowLayoutPanelRight, 2, 0);
            tableLayoutPanel4.Controls.Add(tableLayoutPanel1, 0, 0);
            tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            tableLayoutPanel4.Size = new System.Drawing.Size(982, 483);
            tableLayoutPanel4.TabIndex = 9;
            // 
            // flowLayoutPanelRight
            // 
            flowLayoutPanelRight.AutoScroll = true;
            flowLayoutPanelRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanelRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanelRight.Location = new System.Drawing.Point(257, 3);
            flowLayoutPanelRight.Name = "flowLayoutPanelRight";
            flowLayoutPanelRight.Size = new System.Drawing.Size(722, 477);
            flowLayoutPanelRight.TabIndex = 5;
            flowLayoutPanelRight.WrapContents = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Cover, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanelLeft, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(250, 483);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // Cover
            // 
            Cover.AllowDrop = true;
            Cover.BackColor = System.Drawing.Color.Black;
            Cover.BackgroundImage = Properties.Resources.CoverImg;
            Cover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            Cover.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            Cover.FlatAppearance.BorderSize = 0;
            Cover.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            Cover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            Cover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Cover.Location = new System.Drawing.Point(0, 0);
            Cover.Margin = new System.Windows.Forms.Padding(0);
            Cover.MaximumSize = new System.Drawing.Size(250, 250);
            Cover.MinimumSize = new System.Drawing.Size(250, 250);
            Cover.Name = "Cover";
            Cover.Size = new System.Drawing.Size(250, 250);
            Cover.TabIndex = 4;
            Cover.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanelLeft
            // 
            flowLayoutPanelLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanelLeft.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowLayoutPanelLeft.Location = new System.Drawing.Point(3, 253);
            flowLayoutPanelLeft.Name = "flowLayoutPanelLeft";
            flowLayoutPanelLeft.Size = new System.Drawing.Size(244, 227);
            flowLayoutPanelLeft.TabIndex = 5;
            flowLayoutPanelLeft.WrapContents = false;
            // 
            // MediaInfoWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.DimGray;
            ClientSize = new System.Drawing.Size(982, 483);
            Controls.Add(tableLayoutPanel4);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            MinimumSize = new System.Drawing.Size(1000, 530);
            Name = "MediaInfoWindow";
            Text = "MediaInfo";
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button Cover;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLeft;
    }
}