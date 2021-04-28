using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MusicApp.Control;

namespace MusicApp
{
    public partial class Form1 : Form
    {
        #region Constants
        const int playerHeight = 60;
        const int playlistWidth = 300;
        const int tabHeight = 30;
        const int tabWidth = 70;
        const int headerHeight = 200;
        const int searchWidth = 300;
        readonly Point middlePanelLocation = new Point(0, tabHeight);
        #endregion

        #region UI Parts
        Player player;
        SongList songlist;
        Playlist playlist;
        AlbumGrid albumgrid;
        ArtistGrid artistgrid;
        AlbumHeader albumHeader;
        ArtistHeader artistHeader;
        ConfigControl configControl;

        FlowLayoutPanel tabPanel;
        Label songTab;
        Label albumTab;
        Label artistTab;
        Label configTab;

        TextBox search;
        #endregion

        #region Form Logic
        public Form1()
        {
            InitializeComponent();

            //Fetch all the songs
            Song.CollectSongs().Wait();

            InitForm();
            InitPlayer();
            InitTabs();
            InitSearch();
            InitSongList();
            InitPlaylist();
        }
        protected void InitForm()
        {
            BackColor = Color.Black;

            Resize += Form1_Resize;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate(true);
        }
        #endregion

        #region SongList Logic
        protected void InitSongList()
        {
            ClearMiddlePannel();

            //Init the songlist if it isnt already
            if(songlist == null)
            {
                songlist = new SongList();
                songlist.SongDoubleClicked += SongList_SongDoubleClicked;
                songlist.Load(Song.Songs);
            }

            Controls.Add(songlist);
            Invalidate();
            songlist.Invalidate(true);
        }
        private void SongList_SongDoubleClicked(object sender, SongEventArgs e)
        {
            //Change the position in the playlist and play the song
            playlist.Position = e.pos;
            playlist.Load(songlist.songlist);

            player.ForcePlay();
        }
        #endregion

        #region Tabs Logic
        protected void InitTabs()
        {
            tabPanel = new FlowLayoutPanel()
            {
                Location = new Point(0, 0),
                Height = tabHeight
            };

            //init the tabs
            songTab = new Label() { Text = "Song", TextAlign = ContentAlignment.MiddleCenter, Height = tabHeight, Width = tabWidth, ForeColor = Color.Purple };
            artistTab = new Label() { Text = "Artist", TextAlign = ContentAlignment.MiddleCenter, Height = tabHeight, Width = tabWidth, ForeColor = Color.Purple };
            albumTab = new Label() { Text = "Album", TextAlign = ContentAlignment.MiddleCenter, Height = tabHeight, Width = tabWidth, ForeColor = Color.Purple };
            configTab = new Label() { Text = "Configuration", TextAlign = ContentAlignment.MiddleCenter, Height = tabHeight, Width = tabWidth, ForeColor = Color.Purple };

            songTab.Click += Songtab_Click;
            artistTab.Click += Artisttab_Click;
            albumTab.Click += Albumtab_Click;
            configTab.Click += ConfigTab_Click;

            tabPanel.Controls.Add(songTab);
            tabPanel.Controls.Add(artistTab);
            tabPanel.Controls.Add(albumTab);
            tabPanel.Controls.Add(configTab);

            Controls.Add(tabPanel);
        }

        private void ConfigTab_Click(object sender, EventArgs e)
        {
            InitConfigControl();
            search.Text = "";
        }
        private void Albumtab_Click(object sender, EventArgs e)
        {
            InitAlbumGrid(Album.Albums);
            search.Text = "";
        }
        private void Artisttab_Click(object sender, EventArgs e)
        {
            InitArtistGrid(Artist.Artists);
            search.Text = "";
        }
        private void Songtab_Click(object sender, EventArgs e)
        {
            InitSongList();
            search.Text = "";
        }
        #endregion

        #region Search Logic
        protected void InitSearch()
        {
            search = new TextBox() { Height = tabHeight, Width = searchWidth, BackColor = Color.FromArgb(50, 50, 50) };
            Controls.Add(search);

            search.TextChanged += Search_TextChanged;
        }
        private void Search_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;

            songlist?.Load(Song.SearchByTitle(text));
            albumgrid?.LoadAlbum(Album.SearchByTitle(text));
            artistgrid?.LoadArtist(Artist.SearchByName(text));

            Invalidate();
        }
        #endregion

        #region Player Logic
        protected void InitPlayer()
        {
            player = new Player();

            Controls.Add(player);

            player.SongFinished += Player_SongFinished;
            player.PlaylistButtonClicked += Player_PlaylistButtonClicked;
            player.NextButtonClicked += Player_NextButtonClicked;
        }
        private void Player_NextButtonClicked(object sender, EventArgs e)
        {
            player.AddMedia(playlist.Next().Path);
            player.ForcePlay();
        }
        private void Player_PlaylistButtonClicked(object sender, EventArgs e)
        {
            playlist.Visible = ((Playlist_Button)sender).On;
            Invalidate();
        }
        private void Playlist_PlaylistChanged(object sender, SongEventArgs e)
        {
            player.AddMedia(e.song.Path);
        }
        private void Player_SongFinished(object sender, EventArgs e)
        {
            player.AddMedia(playlist.Next().Path);
        }
        #endregion

        #region AlbumSongList Logic
        protected void InitAlbumSongList()
        {
            ClearMiddlePannel();

            songlist = new SongList();
            albumHeader = new AlbumHeader();

            Controls.Add(songlist);
            Controls.Add(albumHeader);

            songlist.SongDoubleClicked += SongList_SongDoubleClicked;

            Invalidate();
        }
        #endregion

        #region ArtistAlbumList Logic
        protected void InitArtistAlbumList()
        {
            ClearMiddlePannel();

            albumgrid = new AlbumGrid();
            artistHeader = new ArtistHeader();

            Controls.Add(albumgrid);
            Controls.Add(artistHeader);

            albumgrid.AlbumControlClicked += Albumgrid_AlbumControlClicked;

            Invalidate();
        }
        #endregion

        #region PlayList Logic
        protected void InitPlaylist()
        {
            playlist = new Playlist() { Visible = false };

            Controls.Add(playlist);

            playlist.PlaylistChanged += Playlist_PlaylistChanged;
        }
        #endregion

        #region AlbumGrid Logic
        protected void InitAlbumGrid(List<Album> albums)
        {
            ClearMiddlePannel();

            //init the album grid if it isnt
            if (albumgrid == null)
            {
                albumgrid = new AlbumGrid();
                albumgrid.LoadAlbum(albums);

                albumgrid.AlbumControlClicked += Albumgrid_AlbumControlClicked;
            }

            Controls.Add(albumgrid);
            Invalidate();
            albumgrid.Invalidate(true);
        }
        private void Albumgrid_AlbumControlClicked(object sender, AlbumControlEventArgs e)
        {
            InitAlbumSongList();

            songlist.Load(e.Album.Songs);
            albumHeader.LoadAlbum(e.Album);
        }
        #endregion

        #region ArtistGrid Logic
        protected void InitArtistGrid(List<Artist> artists)
        {
            ClearMiddlePannel();

            //init artistgrid if it isnt
            if(artistgrid == null)
            {
                artistgrid = new ArtistGrid();
                artistgrid.LoadArtist(artists);

                artistgrid.ArtistControlClicked += Artistgrid_ArtistControlClicked;
            }

            Controls.Add(artistgrid);

            Invalidate();
            artistgrid.Invalidate(true);
        }
        private void Artistgrid_ArtistControlClicked(object sender, ArtistControlEventArgs e)
        {
            InitArtistAlbumList();

            albumgrid.LoadAlbum(e.artist.Albums);
            artistHeader.LoadArtist(e.artist);
        }
        #endregion

        #region ConfigControl Logic
        protected void InitConfigControl()
        {
            ClearMiddlePannel();
            search.Visible = false;

            configControl = new ConfigControl();
            
            Controls.Add(configControl);

            Invalidate();
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            player.Location = new Point(0, DisplayRectangle.Height - playerHeight);
            player.Size = new Size(DisplayRectangle.Width, playerHeight);

            Size middlePanelSize = new Size(Width, Height - tabHeight - playerHeight);

            if (songlist != null) 
            {
                songlist.Location = middlePanelLocation;
                songlist.Size = middlePanelSize;
            }

            if (albumgrid != null)
            {
                albumgrid.Location = middlePanelLocation;
                albumgrid.Size = middlePanelSize;
            }

            if (artistgrid != null) 
            {
                artistgrid.Location = middlePanelLocation;
                artistgrid.Size = middlePanelSize;
            }

            if (albumHeader != null)
            {
                albumHeader.Location = new Point(0, tabHeight);
                albumHeader.Size = new Size(middlePanelSize.Width, headerHeight);
            }

            if (artistHeader != null)
            {
                artistHeader.Location = new Point(0, tabHeight);
                artistHeader.Size = new Size(middlePanelSize.Width, headerHeight);
            }

            if(configControl != null)
            {
                configControl.Location = middlePanelLocation;
                configControl.Size = middlePanelSize;
            }

            search.Location = new Point(Width - searchWidth - (playlist.Visible ? playlistWidth : 0), 0);

            tabPanel.Width = Width - searchWidth - (playlist.Visible ? playlistWidth : 0);

            playlist.Location = new Point(DisplayRectangle.Width - playlistWidth, 0);
            playlist.Size = new Size(playlistWidth, DisplayRectangle.Height - playerHeight);

        }
        private void ClearMiddlePannel()
        {
            albumHeader?.Dispose();
            artistHeader?.Dispose();
            configControl?.Dispose();

            Controls.Remove(albumgrid);
            Controls.Remove(artistgrid);
            Controls.Remove(songlist);
            Controls.Remove(albumHeader);
            Controls.Remove(artistHeader);
            Controls.Remove(configControl);

            albumHeader = null;
            artistHeader = null;
            configControl = null;

            search.Visible = true;

            Invalidate();
        }
    }
}
