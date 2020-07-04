using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LibVLCSharp.Shared;
using LibVLCSharp.Forms.Shared;
using MusicApp.Control;
using MusicApp.DB;
using MusicApp.Processing;

namespace MusicApp
{
    public partial class Form1 : Form
    {
        int playerH = 60;
        int margin = 15;

        Player player;
        Main main;

        public Form1()
        {
            Music_DataBase.Start();

            InitializeComponent();

            Resize += Form1_Resize;

            BackColor = Color.Black;

            player = new Player() { buttonMarging = margin };
            player.InitAudioPlayer();

            Controls.Add(player);

            main = new Main();
            Controls.Add(main);
            main.SongDoubleClick += Main_SongDoubleClick;

            TextBox t = new TextBox() { Height = 500, Width = 500 };
            Controls.Add(t);

            SongCollector.Collect();

            t.Text = "done";
            BackgroundWorker worker = new BackgroundWorker();


        }

        private void Main_SongDoubleClick(object sender, SongDoubleClickEventArgs e)
        {
            player.AddMedia(e.Song.Path);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            player.Location = new Point(0, DisplayRectangle.Height - playerH);
            player.Size = new Size(DisplayRectangle.Width, playerH);

            main.Location = new Point(0, 0);
            main.Size = new Size(DisplayRectangle.Width, Height - playerH - margin);
        }
    }
}
