using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MusicApp.Beans;
using MusicApp.Parts;

namespace MusicApp.Control
{
    public partial class PlaylistControl : DataGridView
    {
        #region Constants
        const int playlistWidth = 300;
        #endregion

        #region Private Members
        private Playlist playlist;
        private Player player;
        #endregion

        /// <summary>
        /// Initiate a new instance of PlaylistControl class
        /// </summary>
        public PlaylistControl()
        {
            InitializeComponent();
            Init();

            Playlist.PlaylistChanged += Playlist_PlaylistChanged;
            Playlist.SongChanged += Playlist_SongChanged;

            Player.SongAdded += PlaylistControl_SongAdded;
        }

        private void Playlist_SongChanged(object sender, EventArgs e)
        {
            HighlightCurrentSong();
        }

        private void PlaylistControl_SongAdded(object sender, EventArgs e)
        {
            HighlightCurrentSong();
        }

        private void Playlist_PlaylistChanged(object sender, EventArgs e)
        {
            DataSource = Playlist.SongList;
            foreach (DataGridViewColumn c in Columns) c.Visible = false;
            Columns["title"].Visible = true;
            Columns["artist"].Visible = true;

            HighlightCurrentSong();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            BringToFront();

            AutoResizeColumns();
        }
        protected void Init()
        {
            Width = playlistWidth;

            SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            AllowUserToAddRows = false;
            CellBorderStyle = DataGridViewCellBorderStyle.None;
            DefaultCellStyle = new DataGridViewCellStyle()
            {

                BackColor = Color.Black,
                ForeColor = Color.Purple,
                SelectionBackColor = Color.Black,
                SelectionForeColor = Color.Purple
            };
            ColumnHeadersVisible = false;
            RowHeadersVisible = false;
            ScrollBars = ScrollBars.Vertical;
            AllowUserToResizeColumns = false;
            AllowUserToResizeRows = false;
            GridColor = Color.Purple;
            EditMode = DataGridViewEditMode.EditProgrammatically;
            AutoGenerateColumns = true;
            BackgroundColor = Color.Black;
        }

        protected void HighlightCurrentSong()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle() { BackColor = Color.FromArgb(10, 10, 10) };

            int Position = Playlist.GetPosition();
            if (Position < 0)
                return;

            foreach(DataGridViewRow row in Rows)
            {
                row.Cells["title"].Style = DefaultCellStyle;
                row.Cells["artist"].Style = DefaultCellStyle;
            }

            Rows[Position].Cells["title"].Style = style;
            Rows[Position].Cells["artist"].Style = style;
        }
    }
}
