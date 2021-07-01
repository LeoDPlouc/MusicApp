﻿namespace MusicApp.Control
{
    partial class PlaylistControl
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
            this.songList = new MusicApp.Control.SongList();
            this.SuspendLayout();
            // 
            // songList
            // 
            this.songList.AutoScroll = true;
            this.songList.BackColor = System.Drawing.Color.Transparent;
            this.songList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.songList.Location = new System.Drawing.Point(0, 0);
            this.songList.Name = "songList";
            this.songList.Size = new System.Drawing.Size(300, 483);
            this.songList.TabIndex = 0;
            // 
            // PlaylistControl
            // 
            this.Controls.Add(this.songList);
            this.Name = "PlaylistControl";
            this.Size = new System.Drawing.Size(300, 483);
            this.ResumeLayout(false);

        }

        #endregion

        private SongList songList;
    }
}
