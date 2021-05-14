﻿using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MusicApp.Control;
using MusicApp.Config;
using MusicApp.Parts;

namespace MusicApp
{
    public partial class Form1 : Form
    {
        #region Constants
        const int tabHeight = 30;
        const int tabWidth = 70;
        const int searchWidth = 300;
        readonly Point middlePanelLocation = new Point(0, tabHeight);
        #endregion

        #region UI Parts
        PlayerControl playerControl;
        SongList songlist;
        PlaylistControl playlistControl;
        AlbumGrid albumgrid;
        ArtistGrid artistgrid;
        AlbumPresentation albumPresentation;
        ArtistPresentation artistPresentation;
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
            Song.CollectSongs();

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

            Configuration.ConfigChanged += Configuration_ConfigChanged;
        }

        private void Configuration_ConfigChanged(object sender, ConfigEventArgs e)
        {
            if (e.Config != ConfigEventArgs.Configs.LibraryPath)
                return;

            Song.CollectSongs();
            songlist.Load(Song.Songs);
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
        //TODO mettre cette fonction dans le control
        private void SongList_SongDoubleClicked(object sender, EventArgs e)
        {
            //Change the position in the playlist and play the song
            Playlist.Load(songlist.songlist);

            playerControl.ForceButtonPlay();
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
            Player.InitPlayer();

            playerControl = new PlayerControl();

            Controls.Add(playerControl);

            playerControl.PlaylistButtonClicked += Player_PlaylistButtonClicked;
            playerControl.NextButtonClicked += Player_NextButtonClicked;
        }
        private void Player_NextButtonClicked(object sender, EventArgs e)
        {
            Player.LoadSong(Playlist.Next());
            playerControl.ForceButtonPlay();
        }
        private void Player_PlaylistButtonClicked(object sender, EventArgs e)
        {
            playlistControl.Visible = ((Playlist_Button)sender).On;
            Invalidate();
        }
        private void Playlist_PlaylistChanged(object sender, EventArgs e)
        {
            Player.LoadSong(Playlist.CurrentSong);
        }
        #endregion

        #region AlbumPresentation Logic
        protected void InitAlbumPresenttion()
        {
            ClearMiddlePannel();

            albumPresentation = new AlbumPresentation();

            Controls.Add(albumPresentation);

            Invalidate();
        }
        #endregion

        #region ArtistAlbumList Logic
        protected void InitArtistAlbumList()
        {
            ClearMiddlePannel();

            artistPresentation = new ArtistPresentation();

            Controls.Add(artistPresentation);

            Invalidate();
        }
        #endregion

        #region PlayList Logic
        protected void InitPlaylist()
        {
            Playlist.InitPlaylist();

            playlistControl = new PlaylistControl() { Visible = false };

            Controls.Add(playlistControl);

            Playlist.PlaylistChanged += Playlist_PlaylistChanged;
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
            InitAlbumPresenttion();

            albumPresentation.LoadAlbum(e.Album);
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

            artistPresentation.LoadArtist(e.Artist);
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
            base.OnPaint(e);

            search.Location = new Point(DisplayRectangle.Width - searchWidth, 0);

            tabPanel.Width = Width - searchWidth - playlistControl.Width;

            Size middlePanelSize = new Size(DisplayRectangle.Width, DisplayRectangle.Height - tabHeight - playerControl.DisplayRectangle.Height);

            playlistControl.Location = new Point(DisplayRectangle.Width - playlistControl.DisplayRectangle.Width, middlePanelLocation.Y);
            playlistControl.Height = middlePanelSize.Height;

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

            if (albumPresentation != null)
            {
                albumPresentation.Location = middlePanelLocation;
                albumPresentation.Size = middlePanelSize;
            }

            if (artistPresentation != null)
            {
                artistPresentation.Location = middlePanelLocation;
                artistPresentation.Size = middlePanelSize;
            }

            if(configControl != null)
            {
                configControl.Location = middlePanelLocation;
                configControl.Size = middlePanelSize;
            }
        }
        private void ClearMiddlePannel()
        {
            albumPresentation?.Dispose();
            artistPresentation?.Dispose();
            configControl?.Dispose();

            Controls.Remove(albumgrid);
            Controls.Remove(artistgrid);
            Controls.Remove(songlist);
            Controls.Remove(albumPresentation);
            Controls.Remove(artistPresentation);
            Controls.Remove(configControl);

            albumPresentation = null;
            artistPresentation = null;
            configControl = null;

            search.Visible = true;

            Invalidate();
        }
    }
}
