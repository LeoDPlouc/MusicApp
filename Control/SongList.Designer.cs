﻿namespace MusicLib.Control
{
    partial class SongList
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
            this.panel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1200, 600);
            this.panel.TabIndex = 0;
            this.panel.WrapContents = false;
            // 
            // SongList
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel);
            this.Name = "SongList";
            this.Size = new System.Drawing.Size(1200, 600);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel panel;
    }
}
