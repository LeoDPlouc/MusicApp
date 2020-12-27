﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicApp.Beans;
using System.IO;

namespace MusicApp.Control
{
    public partial class AlbumHeader : UserControl
    {
        PictureBox Cover;
        Label AlbumName;
        Label ArtistName;

        Album Album { get; set; }

        public AlbumHeader()
        {
            InitializeComponent();
            Init();
        }

        public void LoadAlbum(Album album)
        {
            /*Album = album;
            var coverTask = Picture.SelectPictureById(album.CoverId);

            try
            {
                using (MemoryStream s = new MemoryStream((await coverTask).Data))
                {
                    Cover.Image = Image.FromStream(s, true, true);
                }
            }
            catch { }

            AlbumName.Text = Album.Title;
            ArtistName.Text = Album.Artist.Name;*/
        }

        private void Init()
        {
            Cover = new PictureBox() { SizeMode = PictureBoxSizeMode.StretchImage };
            AlbumName = new Label() { ForeColor = Color.Purple };
            ArtistName = new Label() { ForeColor = Color.Purple };

            Controls.Add(Cover);
            Controls.Add(AlbumName);
            Controls.Add(ArtistName);

            AlbumName.ContextMenuStrip = new ContextMenuStrip();
            AlbumName.ContextMenuStrip.Items.Add("Edit");
            AlbumName.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            /*if (e.ClickedItem.Text == "Edit")
            {
                Label newTitleLabel = new Label() { Text = "New Album Title", AutoSize = true, Margin = new Padding(0, 6, 0, 0) };
                TextBox NewTitle = new TextBox() { Width = 100 };

                FlowLayoutPanel panel = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight };
                panel.Controls.Add(newTitleLabel);
                panel.Controls.Add(NewTitle);

                Form form = new Form() { Size = new Size(210, 65) };
                form.Controls.Add(panel);

                form.ShowDialog();

                AlbumName.Text = NewTitle.Text;
                Album.Title = AlbumName.Text;

                Album.Save();
            }*/
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Cover.Location = new Point(0, 0);
            Cover.Size = new Size(Height, Height);

            AlbumName.Location = new Point(Height, 0);
            ArtistName.Location = new Point(Height, AlbumName.Height);
        }
    }
}
