using System;
using System.Drawing;
using System.Windows.Forms;
using MusicApp.Parts;

namespace MusicApp.Control
{
    public partial class PlayerControl : UserControl
    {
        #region UI Parts
        Next_Button next;
        Play_Button play;
        Playlist_Button playlist;
        Slider volume;
        Slider progressBar;
        #endregion

        #region Events
        public event EventHandler PlaylistButtonClicked;
        public event EventHandler NextButtonClicked;
        #endregion

        #region Consts
        const int progressBarH = 8;
        const int volumeH = 10;
        const int volumeW = 100;
        const int margin = 10;
        const int playerHeight = 60;
        #endregion

        public PlayerControl()
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
            Height = playerHeight;

            play.StateChanged += Play_StateChanged;
            playlist.Click += Playlist_Click;
            next.Click += Next_Click;
            volume.SliderValueChanged += Volume_SliderValueChanged;
            progressBar.SliderValueChanged += ProgressBar_SliderValueChanged;

            Player.SongAdded += PlayerControl_SongAdded;
            Player.ProgressChanged += PlayerControl_ProgressChanged;
        }

        private void PlayerControl_ProgressChanged(object sender, EventArgs e)
        {
            progressBar.Value = Player.Progress * 100;
            progressBar.Invalidate();
        }

        private void PlayerControl_SongAdded(object sender, EventArgs e)
        {
            play.ForcePlay();
        }

        private void ProgressBar_SliderValueChanged(object sender, EventArgs e)
        {
            Player.Progress = ((Slider)sender).Value / 100;
        }

        private void Volume_SliderValueChanged(object sender, EventArgs e)
        {
            Player.Volume = (int)((Slider)sender).Value;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            NextButtonClicked?.Invoke(this, new EventArgs());
        }

        public void ForcePlayButtonChangeState()
        {
            play.ForceChangeState();
        }
        public void ForceButtonPlay()
        {
            play.ForcePlay();
        }
        public void ForceButtonPause()
        {
            play.ForcePause();
        }

        private void Playlist_Click(object sender, EventArgs e)
        {
            OnPlaylistButtonClick(e);
        }


        private void Play_StateChanged(object sender, PlayButtonEventArgs e)
        {
            if (e.State == PlayButtonEventArgs.States.Play) Player.Play();
            else Player.Pause();
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
        protected void OnPlaylistButtonClick(EventArgs e)
        {
            PlaylistButtonClicked?.Invoke(playlist, e);
        }
    }
}
