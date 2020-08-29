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
        Slider volume;
        Slider progressBar;

        LibVLC vlc;
        MediaPlayer mediaPlayer;

        public event EventHandler SongFinished;
        public event EventHandler PlaylistButtonClicked;
        public event EventHandler NextButtonClicked;
        public Media Media { get; set; }

        int progressBarH = 8;
        int volumeH = 10;
        int volumeW = 100;
        int margin = 10;

        public Player()
        {
            InitializeComponent();

            next = new Next_Button();
            play = new Play_Button();
            playlist = new Playlist_Button();
            volume = new Slider() { Value = 75 };
            progressBar = new Slider() { Value = 0 };

            Controls.Add(next);
            Controls.Add(play);
            Controls.Add(playlist);
            Controls.Add(volume);
            Controls.Add(progressBar);

            BackColor = Color.Transparent;
            Dock = DockStyle.Bottom;

            InitAudioPlayer();

            play.StateChanged += Play_StateChanged;
            mediaPlayer.EndReached += MediaPlayer_EndReached;
            playlist.Click += Playlist_Click;
            next.Click += Next_Click;
            volume.SliderValueChanged += Volume_SliderValueChanged;
            mediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            progressBar.SliderValueChanged += ProgressBar_SliderValueChanged;
        }

        private void ProgressBar_SliderValueChanged(object sender, EventArgs e)
        {
            mediaPlayer.Position = ((Slider)sender).Value / 100;
        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            progressBar.Value = e.Position * 100;
            progressBar.Invalidate();
        }

        private void Volume_SliderValueChanged(object sender, EventArgs e)
        {
            mediaPlayer.Volume = (int)((Slider)sender).Value;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            NextButtonClicked?.Invoke(this, new EventArgs());
        }

        public void ForcePlay()
        {
            play.ForceChangeState();
        }

        private void Playlist_Click(object sender, EventArgs e)
        {
            OnPlaylistButtonClick(e);
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
            int wh = Height - progressBarH - margin * 2;
            int y = (Height - progressBarH) / 2 + progressBarH;

            play.Size = new Size(wh, wh);
            play.Location = new Point(Width / 2 - wh / 2, y - wh / 2);

            next.Size = new Size(wh, wh);
            next.Location = new Point(Width / 2 + wh + margin * 2, y - wh / 2);

            playlist.Size = new Size(wh, wh);
            playlist.Location = new Point(Width - wh - margin, y - wh / 2);

            volume.Size = new Size(volumeW, volumeH);
            volume.Location = new Point(margin, y - volumeH / 2);

            progressBar.Size = new Size(Width, progressBarH);
            progressBar.Location = new Point(0, 0);
        }
        protected void OnSongFinish(EventArgs e)
        {
            SongFinished?.Invoke(this, e);
        }
        protected void OnPlaylistButtonClick(EventArgs e)
        {
            PlaylistButtonClicked?.Invoke(playlist, e);
        }

        public void InitAudioPlayer()
        {
            Core.Initialize();

            vlc = new LibVLC();
            mediaPlayer = new MediaPlayer(vlc);

            mediaPlayer.Volume = (int)volume.Value;
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
