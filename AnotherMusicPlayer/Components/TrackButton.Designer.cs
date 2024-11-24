namespace AnotherMusicPlayer.Components
{
    partial class TrackButton
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
            label1 = new System.Windows.Forms.Label();
            rating21 = new Rating2();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoEllipsis = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(3, 10);
            label1.MaximumSize = new System.Drawing.Size(0, 75);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(242, 23);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // rating21
            // 
            rating21.BackColor = System.Drawing.Color.Transparent;
            rating21.IsReadOnly = false;
            rating21.Location = new System.Drawing.Point(3, 43);
            rating21.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            rating21.MaximumSize = new System.Drawing.Size(100, 20);
            rating21.MinimumSize = new System.Drawing.Size(100, 20);
            rating21.Name = "rating21";
            rating21.Rate = 0D;
            rating21.Size = new System.Drawing.Size(100, 20);
            rating21.TabIndex = 1;
            rating21.Zoom = 1D;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(rating21, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tableLayoutPanel1.Size = new System.Drawing.Size(248, 68);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // TrackButton
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.FromArgb(54, 54, 54);
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(tableLayoutPanel1);
            MaximumSize = new System.Drawing.Size(250, 70);
            MinimumSize = new System.Drawing.Size(250, 70);
            Name = "TrackButton";
            Size = new System.Drawing.Size(248, 68);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private Rating2 rating21;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
