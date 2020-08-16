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
        int playlistW = 300;

        Player player;
        SongList songlist;
        Playlist playlist;
        AlbumGrid albumgrid;
        ArtistGrid artistgrid;

        public Form1()
        {
            InitializeComponent();

            InitForm();
            InitPlayer();
            InitDB();
            InitSongList();
            InitPlaylist();
            InitArtistGrid(Listing.SearchArtist(""));

            SongCollector.Collect();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
        private void SongList_SongDoubleClicked(object sender, SongEventArgs e)
        {
            playlist.Position = e.pos;
            playlist.Load(songlist.songlist);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            player.Location = new Point(0, DisplayRectangle.Height - playerH);
            player.Size = new Size(DisplayRectangle.Width, playerH);

            songlist.Location = new Point(0, 0);
            songlist.Size = new Size(DisplayRectangle.Width - playlistW, DisplayRectangle.Height - playerH - margin);

            playlist.Location = new Point(DisplayRectangle.Width - playlistW, 0);
            playlist.Size = new Size(playlistW, DisplayRectangle.Height - playerH - margin);

            artistgrid.Location = new Point(0, 0);
            artistgrid.Size = new Size(DisplayRectangle.Width - playlistW, DisplayRectangle.Height - playerH - margin);
        }

        protected void InitForm()
        {
            BackColor = Color.Black;

            Resize += Form1_Resize;
        }
        protected void InitPlayer()
        {
            player = new Player() { buttonMarging = margin };

            Controls.Add(player);

            player.SongFinished += Player_SongFinished;
        }
        protected void InitDB()
        {
            MusicDataBase.Start();
        }
        protected void InitSongList()
        {
            songlist = new SongList();
            songlist.Load(Listing.SearchSong(""));

            Controls.Add(songlist);

            songlist.SongDoubleClicked += SongList_SongDoubleClicked;
            songlist.Visible = false;
        }
        protected void InitPlaylist()
        {
            playlist = new Playlist();

            Controls.Add(playlist);

            playlist.PlaylistChanged += Playlist_PlaylistChanged;
        }
        protected void InitAlbumGrid(List<Album> albums)
        {
            albumgrid = new AlbumGrid();
            albumgrid.LoadAlbum(albums);

            Controls.Add(albumgrid);
        }
        protected void InitArtistGrid(List<Artist> artists)
        {
            artistgrid = new ArtistGrid();
            artistgrid.LoadArtist(artists);

            Controls.Add(artistgrid);
        }

        private void Playlist_PlaylistChanged(object sender, SongEventArgs e)
        {
            player.AddMedia(e.song.Path);
        }
        private void Player_SongFinished(object sender, EventArgs e)
        {
            player.AddMedia(playlist.Next().Path);
        }
    }
}
