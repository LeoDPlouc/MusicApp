using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp;
using LibVLCSharp.Shared;

namespace MusicApp.Control
{
    public partial class Player : UserControl
    {
        Next_Button next;
        Play_Button play;
        Playlist_Button playlist;
        LibVLC vlc;
        MediaPlayer mediaPlayer;

        public int buttonMarging { get; set; }

        public Player()
        {
            InitializeComponent();

            next = new Next_Button();
            play = new Play_Button();
            playlist = new Playlist_Button();

            Controls.Add(next);
            Controls.Add(play);
            Controls.Add(playlist);

            BackColor = Color.Transparent;
            Dock = DockStyle.Bottom;

            play.Click += Play_Click;

            InitAudioPlayer();
        }

        private async void Play_Click(object sender, EventArgs e)
        {
            var b = (Play_Button)sender;

            while(!b.sem)
            {
                await Task.Delay(100);
            }

            if (b.play) mediaPlayer.Play();
            else mediaPlayer.Pause();

            b.sem = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            play.Size = new Size(Size.Height - buttonMarging * 2, Size.Height - buttonMarging * 2);
            play.Location = new Point(Size.Width / 2 - Size.Height / 2 + buttonMarging, buttonMarging);

            next.Size = new Size(Size.Height - buttonMarging * 2, Size.Height - buttonMarging * 2);
            next.Location = new Point(Size.Width / 2 + Size.Height / 2 + buttonMarging, buttonMarging);

            playlist.Size = new Size(Size.Height - buttonMarging * 2, Size.Height - buttonMarging * 2);
            playlist.Location = new Point(Size.Width - Size.Height + buttonMarging, buttonMarging);
        }

        public void InitAudioPlayer()
        {
            Core.Initialize();

            vlc = new LibVLC();
            mediaPlayer = new MediaPlayer(vlc);
        }

        public void AddMedia(string uri)
        {
            Media media = new Media(vlc, uri);
            mediaPlayer.Media = media;

            if (play.play)
            {
                mediaPlayer.Play();
            }
        }
    }
}
