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
using System.Threading;

namespace MusicApp.Control
{
    public partial class Player : UserControl
    {
        Next_Button next;
        Play_Button play;
        Playlist_Button playlist;
        LibVLC vlc;
        MediaPlayer mediaPlayer;

        public event EventHandler SongFinished;

        public int buttonMarging { get; set; }
        public Media Media { get; set; }

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

            InitAudioPlayer();

            play.StateChanged += Play_StateChanged;
            mediaPlayer.EndReached += MediaPlayer_EndReached;
        }

        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ => OnSongFinish(e));
        }

        private void Play_StateChanged(object sender, PlayButtonEventArgs e)
        {
            if (e.State == PlayButtonEventArgs.States.Play) mediaPlayer.Play();
            else mediaPlayer.Pause();
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
        protected void OnSongFinish(EventArgs e)
        {
            SongFinished?.Invoke(this, e);
        }

        public void InitAudioPlayer()
        {
            Core.Initialize();

            vlc = new LibVLC();
            mediaPlayer = new MediaPlayer(vlc);
        }

        public void AddMedia(string uri)
        {
            Media = new Media(vlc, uri);
            Media.AddOption(":no-video");
            mediaPlayer.Media = Media;

            if (play.State == PlayButtonEventArgs.States.Play)
            {
                mediaPlayer.Play();
            }
        }
    }
}
