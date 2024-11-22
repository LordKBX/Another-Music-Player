namespace AnotherMusicPlayer
{
    partial class PlaybackProgressBar
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
            progressBar1 = new NewProgressBar();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar1.ForeColor = System.Drawing.Color.Silver;
            progressBar1.Location = new System.Drawing.Point(0, 0);
            progressBar1.Maximum = 100000;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(300, 30);
            progressBar1.Step = 1;
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 0;
            progressBar1.Value = 10000;
            // 
            // PlaybackProgressBar
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(progressBar1);
            Name = "PlaybackProgressBar";
            Size = new System.Drawing.Size(300, 30);
            ResumeLayout(false);
        }

        #endregion

        private NewProgressBar progressBar1;
    }
}
