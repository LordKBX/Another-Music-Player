namespace AnotherMusicPlayer.Components
{
    partial class AlbumBlock
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            diskBlock1 = new DiskBlock();
            MainTableLayoutPanel.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.Black;
            button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button1.Location = new System.Drawing.Point(0, 0);
            button1.Margin = new System.Windows.Forms.Padding(0);
            button1.Name = "button1";
            MainTableLayoutPanel.SetRowSpan(button1, 2);
            button1.Size = new System.Drawing.Size(150, 148);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoEllipsis = true;
            label1.AutoSize = true;
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(153, 2);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(142, 20);
            label1.TabIndex = 0;
            label1.Text = "<AA>";
            // 
            // MainTableLayoutPanel
            // 
            MainTableLayoutPanel.AutoSize = true;
            MainTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            MainTableLayoutPanel.ColumnCount = 2;
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainTableLayoutPanel.Controls.Add(button1);
            MainTableLayoutPanel.Controls.Add(label1, 1, 0);
            MainTableLayoutPanel.Controls.Add(tableLayoutPanel1, 1, 1);
            MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            MainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            MainTableLayoutPanel.RowCount = 2;
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            MainTableLayoutPanel.Size = new System.Drawing.Size(298, 148);
            MainTableLayoutPanel.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(diskBlock1, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel1.Location = new System.Drawing.Point(153, 28);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(142, 87);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // diskBlock1
            // 
            diskBlock1.AutoSize = true;
            diskBlock1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            diskBlock1.BackColor = System.Drawing.Color.Transparent;
            diskBlock1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            diskBlock1.Location = new System.Drawing.Point(0, 0);
            diskBlock1.Margin = new System.Windows.Forms.Padding(0);
            diskBlock1.MinimumSize = new System.Drawing.Size(200, 0);
            diskBlock1.Name = "diskBlock1";
            diskBlock1.Size = new System.Drawing.Size(200, 87);
            diskBlock1.TabIndex = 0;
            // 
            // AlbumBlock
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.Color.Transparent;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(MainTableLayoutPanel);
            MinimumSize = new System.Drawing.Size(300, 150);
            Name = "AlbumBlock";
            Size = new System.Drawing.Size(298, 148);
            MainTableLayoutPanel.ResumeLayout(false);
            MainTableLayoutPanel.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DiskBlock diskBlock1;
    }
}
