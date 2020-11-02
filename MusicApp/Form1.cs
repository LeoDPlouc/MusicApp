﻿using MusicApp.Beans;
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
using MusicApp.Configuration;

namespace MusicApp
{
    public partial class Form1 : Form
    {
        int playerH = 60;
        int playlistW = 300;
        int tabH = 30;
        int headerH = 200;

        Player player;
        SongList songlist;
        Playlist playlist;
        AlbumGrid albumgrid;
        ArtistGrid artistgrid;
        Label songtab;
        Label albumtab;
        Label artisttab;
        TextBox search;
        AlbumHeader albumHeader;
        ArtistHeader artistHeader;

        public Form1()
        {
            InitializeComponent();

            InitConfig();
            InitDB();
            InitForm();
            InitPlayer();
            InitTabs();
            InitSearch();
            InitSongList();
            InitPlaylist();

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

            player.ForcePlay();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            player.Location = new Point(0, DisplayRectangle.Height - playerH);
            player.Size = new Size(DisplayRectangle.Width, playerH);

            Size middlePanelSize = new Size(DisplayRectangle.Width - (playlist.Visible ? playlistW : 0), DisplayRectangle.Height - playerH - tabH - (albumHeader != null ? headerH : 0));
            Point middlePanelLocation = new Point(0, tabH + (albumHeader != null || artistHeader != null ? headerH : 0));

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
                albumHeader.Location = new Point(0, tabH);
                albumHeader.Size = new Size(middlePanelSize.Width, headerH);
            }

            if (artistHeader != null)
            {
                artistHeader.Location = new Point(0, tabH);
                artistHeader.Size = new Size(middlePanelSize.Width, headerH);
            }

            songtab.Location = new Point(0, 0);
            albumtab.Location = new Point(60, 0);
            artisttab.Location = new Point(120, 0);

            search.Location = new Point(Width - 250 - playlistW, 0);

            playlist.Location = new Point(DisplayRectangle.Width - playlistW, 0);
            playlist.Size = new Size(playlistW, DisplayRectangle.Height - playerH);

        }

        protected void InitForm()
        {
            BackColor = Color.Black;

            Resize += Form1_Resize;
        }
        protected void InitTabs()
        {
            songtab = new Label() { Text = "Song", TextAlign = ContentAlignment.MiddleCenter, Height = tabH, Width = 60, ForeColor = Color.Purple };
            artisttab = new Label() { Text = "Artist", TextAlign = ContentAlignment.MiddleCenter, Height = tabH, Width = 60, ForeColor = Color.Purple };
            albumtab = new Label() { Text = "Album", TextAlign = ContentAlignment.MiddleCenter, Height = tabH, Width = 60, ForeColor = Color.Purple };

            Controls.Add(songtab);
            Controls.Add(artisttab);
            Controls.Add(albumtab);

            songtab.Click += Songtab_Click;
            artisttab.Click += Artisttab_Click;
            albumtab.Click += Albumtab_Click;
        }
        protected void InitSearch()
        {
            search = new TextBox() { Height = tabH, Width = 200, ForeColor = Color.Purple };
            Controls.Add(search);

            search.TextChanged += Search_TextChanged;
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;

            songlist?.Load(Listing.SearchSong(text));
            albumgrid?.LoadAlbum(Listing.SearchAlbum(text));
            artistgrid?.LoadArtist(Listing.SearchArtist(text));

            Invalidate();
        }

        private void Albumtab_Click(object sender, EventArgs e)
        {
            InitAlbumGrid(Listing.SearchAlbum(""));
            search.Text = "";
        }

        private void Artisttab_Click(object sender, EventArgs e)
        {
            InitArtistGrid(Listing.SearchArtist(""));
            search.Text = "";
        }

        private void Songtab_Click(object sender, EventArgs e)
        {
            InitSongList();
            search.Text = "";
        }

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

        protected void InitConfig()
        {
            Configuration.Configuration.Init();
        }
        protected void InitDB()
        {
            MusicDataBase.Start();
        }
        protected void InitSongList()
        {
            ClearMiddlePannel();

            songlist = new SongList();
            songlist.Load(Listing.SearchSong(""));

            Controls.Add(songlist);

            songlist.SongDoubleClicked += SongList_SongDoubleClicked;

            Invalidate();
        }
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
        protected void InitPlaylist()
        {
            playlist = new Playlist() { Visible = false };

            Controls.Add(playlist);

            playlist.PlaylistChanged += Playlist_PlaylistChanged;
        }
        protected void InitAlbumGrid(List<Album> albums)
        {
            ClearMiddlePannel();

            albumgrid = new AlbumGrid();
            albumgrid.LoadAlbum(albums);

            Controls.Add(albumgrid);
            Invalidate();

            albumgrid.AlbumControlClicked += Albumgrid_AlbumControlClicked;
        }

        private void Albumgrid_AlbumControlClicked(object sender, AlbumControlEventArgs e)
        {
            InitAlbumSongList();

            songlist.Load(MusicDataBase.SelectSongAlbum(e.album));
            albumHeader.LoadAlbum(e.album);
        }

        protected void InitArtistGrid(List<Artist> artists)
        {
            ClearMiddlePannel();

            artistgrid = new ArtistGrid();
            artistgrid.LoadArtist(artists);

            Controls.Add(artistgrid);

            Invalidate();

            artistgrid.ArtistControlClicked += Artistgrid_ArtistControlClicked;
        }

        private void Artistgrid_ArtistControlClicked(object sender, ArtistControlEventArgs e)
        {
            InitArtistAlbumList();

            albumgrid.LoadAlbum(MusicDataBase.SelectAlbumArtist(e.artist));
            artistHeader.LoadArtist(e.artist);
        }

        private void Playlist_PlaylistChanged(object sender, SongEventArgs e)
        {
            player.AddMedia(e.song.Path);
        }
        private void Player_SongFinished(object sender, EventArgs e)
        {
            player.AddMedia(playlist.Next().Path);
        }

        private void ClearMiddlePannel()
        {
            albumgrid?.Dispose();
            artistgrid?.Dispose();
            songlist?.Dispose();
            albumHeader?.Dispose();
            artistHeader?.Dispose();

            Controls.Remove(albumgrid);
            Controls.Remove(artistgrid);
            Controls.Remove(songlist);
            Controls.Remove(albumHeader);
            Controls.Remove(artistHeader);

            albumgrid = null;
            artistgrid = null;
            songlist = null;
            albumHeader = null;
            artistHeader = null;
        }
    }
}
