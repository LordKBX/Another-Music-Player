using System.Drawing;
using System.Windows.Forms;

namespace CustomControl
{
    partial class BoolPresenter
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
            MainTablePanel = new TableLayoutPanel();
            checkBoxTrue = new CheckBox();
            checkBoxFalse = new CheckBox();
            MainTablePanel.SuspendLayout();
            SuspendLayout();
            // 
            // MainTablePanel
            // 
            MainTablePanel.ColumnCount = 3;
            MainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            MainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 2F));
            MainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            MainTablePanel.Controls.Add(checkBoxTrue, 0, 0);
            MainTablePanel.Controls.Add(checkBoxFalse, 2, 0);
            MainTablePanel.Dock = DockStyle.Fill;
            MainTablePanel.Location = new Point(0, 0);
            MainTablePanel.Margin = new Padding(0);
            MainTablePanel.Name = "MainTablePanel";
            MainTablePanel.RowCount = 1;
            MainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainTablePanel.Size = new Size(140, 41);
            MainTablePanel.TabIndex = 0;
            // 
            // checkBoxTrue
            // 
            checkBoxTrue.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            checkBoxTrue.Appearance = Appearance.Button;
            checkBoxTrue.AutoCheck = false;
            checkBoxTrue.AutoSize = true;
            checkBoxTrue.BackColor = Color.FromArgb(64, 64, 64);
            checkBoxTrue.Checked = true;
            checkBoxTrue.CheckState = CheckState.Checked;
            checkBoxTrue.Cursor = Cursors.Hand;
            checkBoxTrue.FlatAppearance.BorderColor = Color.White;
            checkBoxTrue.FlatAppearance.CheckedBackColor = Color.FromArgb(110, 110, 110);
            checkBoxTrue.FlatAppearance.MouseDownBackColor = Color.FromArgb(110, 110, 110);
            checkBoxTrue.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 80);
            checkBoxTrue.FlatStyle = FlatStyle.Flat;
            checkBoxTrue.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxTrue.ForeColor = Color.White;
            checkBoxTrue.Location = new Point(0, 5);
            checkBoxTrue.Margin = new Padding(0);
            checkBoxTrue.Name = "checkBoxTrue";
            checkBoxTrue.Size = new Size(69, 30);
            checkBoxTrue.TabIndex = 0;
            checkBoxTrue.Text = "True";
            checkBoxTrue.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxTrue.UseVisualStyleBackColor = false;
            checkBoxTrue.Click += checkBoxTrue_Click;
            // 
            // checkBoxFalse
            // 
            checkBoxFalse.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            checkBoxFalse.Appearance = Appearance.Button;
            checkBoxFalse.AutoCheck = false;
            checkBoxFalse.AutoSize = true;
            checkBoxFalse.BackColor = Color.FromArgb(64, 64, 64);
            checkBoxFalse.Cursor = Cursors.Hand;
            checkBoxFalse.FlatAppearance.BorderColor = Color.White;
            checkBoxFalse.FlatAppearance.CheckedBackColor = Color.FromArgb(110, 110, 110);
            checkBoxFalse.FlatAppearance.MouseDownBackColor = Color.FromArgb(110, 110, 110);
            checkBoxFalse.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 80);
            checkBoxFalse.FlatStyle = FlatStyle.Flat;
            checkBoxFalse.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxFalse.ForeColor = Color.White;
            checkBoxFalse.Location = new Point(71, 5);
            checkBoxFalse.Margin = new Padding(0);
            checkBoxFalse.Name = "checkBoxFalse";
            checkBoxFalse.Size = new Size(69, 30);
            checkBoxFalse.TabIndex = 0;
            checkBoxFalse.Text = "False";
            checkBoxFalse.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxFalse.UseVisualStyleBackColor = false;
            checkBoxFalse.Click += checkBoxFalse_Click;
            // 
            // BoolPresenter
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.Transparent;
            Controls.Add(MainTablePanel);
            Margin = new Padding(0);
            MaximumSize = new Size(11428571, 59);
            MinimumSize = new Size(140, 41);
            Name = "BoolPresenter";
            Size = new Size(140, 41);
            MainTablePanel.ResumeLayout(false);
            MainTablePanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel MainTablePanel;
        private CheckBox checkBoxTrue;
        private CheckBox checkBoxFalse;
    }
}
