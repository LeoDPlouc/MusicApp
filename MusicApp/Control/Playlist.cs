using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicApp.Beans;

namespace MusicApp.Control
{
    public partial class Playlist : DataGridView
    {
        public int Position { get; set; }
        public Song CurrentSong { get
            {
                return playlist[Position];
            } }
        public BindingList<Song> playlist { get; }

        public event EventHandler<SongEventArgs> PlaylistChanged;

        public Playlist()
        {
            InitializeComponent();
            Init();

            playlist = new BindingList<Song>();
            DataSource = playlist;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            AutoResizeColumns();

            base.OnPaint(e);
        }

        public void Load(IEnumerable<Song> songs)
        {
            playlist.Clear();
            foreach (Song s in songs) playlist.Add(s);

            foreach (DataGridViewColumn c in Columns) c.Visible = false;
            Columns["title"].Visible = true;
            Columns["artist"].Visible = true;

            OnPlaylistChange();
        }
        public void Load(Song song)
        {
            playlist.Clear();
            playlist.Add(song);

            foreach (DataGridViewColumn c in Columns) c.Visible = false;
            Columns["title"].Visible = true;
            Columns["artist"].Visible = true;

            OnPlaylistChange();
        }
        public Song Next()
        {
            Position++;
            return CurrentSong;
        }
        protected void Init()
        {
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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
        }
        protected void OnPlaylistChange()
        {
            PlaylistChanged?.Invoke(this, new SongEventArgs()
            {
                song = CurrentSong,
                pos = Position
            });
        }
    }
}
