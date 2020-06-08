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

namespace MusicApp
{
    public partial class Form1 : Form
    {
        int playerH = 60;

        Player player;

        public Form1()
        {
            Core.Initialize();
            InitializeComponent();

            Resize += Form1_Resize;

            BackColor = Color.Black;

            player = new Player() { Height = 50, buttonMarging = 15 };
            player.InitAudioPlayer();

            Controls.Add(player);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            player.Location = new Point(0, DisplayRectangle.Height - playerH);
            player.Size = new Size(DisplayRectangle.Width, playerH);
        }
    }
}
