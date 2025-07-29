namespace AnotherMusicPlayer.Components
{
    partial class DiskBlock
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            trackButton1 = new TrackButton();
            trackButton2 = new TrackButton();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(248, 85);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(3, 1);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(242, 23);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(trackButton1, 0, 0);
            tableLayoutPanel2.Controls.Add(trackButton2, 0, 1);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel2.Size = new System.Drawing.Size(248, 60);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // trackButton1
            // 
            trackButton1.BackColor = System.Drawing.Color.FromArgb(54, 54, 54);
            trackButton1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            trackButton1.Location = new System.Drawing.Point(0, 0);
            trackButton1.Margin = new System.Windows.Forms.Padding(0);
            trackButton1.MaximumSize = new System.Drawing.Size(0, 30);
            trackButton1.MinimumSize = new System.Drawing.Size(250, 27);
            trackButton1.Name = "trackButton1";
            trackButton1.Size = new System.Drawing.Size(250, 27);
            trackButton1.TabIndex = 0;
            // 
            // trackButton2
            // 
            trackButton2.BackColor = System.Drawing.Color.FromArgb(54, 54, 54);
            trackButton2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            trackButton2.Location = new System.Drawing.Point(0, 30);
            trackButton2.Margin = new System.Windows.Forms.Padding(0);
            trackButton2.MaximumSize = new System.Drawing.Size(0, 30);
            trackButton2.MinimumSize = new System.Drawing.Size(250, 30);
            trackButton2.Name = "trackButton2";
            trackButton2.Size = new System.Drawing.Size(250, 30);
            trackButton2.TabIndex = 1;
            // 
            // DiskBlock
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.Color.Transparent;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(tableLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(0);
            MinimumSize = new System.Drawing.Size(250, 50);
            Name = "DiskBlock";
            Size = new System.Drawing.Size(248, 85);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private TrackButton trackButton1;
        private TrackButton trackButton2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
