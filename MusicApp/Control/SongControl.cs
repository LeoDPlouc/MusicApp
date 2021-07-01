using MusicLib.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicApp.Control
{
    public partial class SongControl : UserControl
    {
        public Song Song { get; private set; }

        public event EventHandler<SongControlEventArgs> SongDoubleClicked;
        private void OnSongDoubleClicked(object sender, SongControlEventArgs e) => SongDoubleClicked?.Invoke(sender, e);

        public SongControl() => InitializeComponent();

        private void SongControl_DoubleClick(object sender, EventArgs e) => OnSongDoubleClicked(sender, new SongControlEventArgs { Song = Song });

        public void LoadSong(Song s)
        {
            Song = s;

            l_like.Text = s.Like.ToString();
            l_heart.Text = s.Heart.ToString();
            l_n.Text = s.N.ToString();
            l_title.Text = s.Title;
            l_duration.Text = s.Duration.ToString();
            l_artist.Text = s.Artist;
            l_album.Text = s.Album;
        }
    }
}
