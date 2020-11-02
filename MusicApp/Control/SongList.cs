using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicApp.Processing;
using MusicApp.Beans;
using MusicApp.DB;
using System.Globalization;

namespace MusicApp.Control
{
    public partial class SongList : DataGridView
    {
        public BindingList<Song> songlist;

        public event EventHandler<SongEventArgs> SongDoubleClicked;

        public SongListType Type;
        public enum SongListType
        {
            Main = 0,
            Album = 1
        }

        public SongList()
        {
            InitializeComponent();
            Init();

            songlist = new BindingList<Song>();
            DataSource = songlist;

            DataBindingComplete += SongList_DataBindingComplete;
        }

        private void SongList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (Columns["ArtistName"] == null) Columns.Add("ArtistName", "Artist");
            if (Columns["AlbumName"] == null) Columns.Add("AlbumName", "Album");
            if (Columns["HeartControl"] == null) Columns.Add("HeartControl", "Heart");

            Columns["N"].Visible = Type == SongListType.Album;

            Columns["Id"].Visible = false;
            Columns["Artist"].Visible = false;
            Columns["Album"].Visible = false;
            Columns["Path"].Visible = false;
            Columns["Cover"].Visible = false;
            Columns["Heart"].Visible = false;
            Columns["Like"].Visible = false;

            Columns["N"].DisplayIndex = 0;

            foreach(DataGridViewRow r in Rows)
            {
                Song s = (Song)r.DataBoundItem;
                Color sColor = s.Like ? (s.Heart ? Color.DarkRed : Color.MediumPurple) : Color.White;

                r.Cells["ArtistName"].Value = s.Artist.Name;
                r.Cells["AlbumName"].Value = s.Album.Title;

                r.Cells["HeartControl"].Value = "♥";
                r.Cells["HeartControl"].Style = new DataGridViewCellStyle() { ForeColor = sColor, SelectionForeColor = sColor};
            }
        }

        private void Song_List_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = Rows[e.RowIndex];
            Song s = (Song)row.DataBoundItem;

            if(row.Cells[e.ColumnIndex].OwningColumn.Name == "HeartControl")
            {
                if (s.Like && !s.Heart) s.Heart = true;
                else if (s.Like && s.Heart)
                {
                    s.Like = false;
                    s.Like = false;
                }
                else if (!s.Like && !s.Heart) s.Like = true;

                s.Save();
                SongList_DataBindingComplete(null, null);

                return;
            }

            SongDoubleClicked?.Invoke(this, new SongEventArgs()
            {
                song = s,
                pos = e.RowIndex
            });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            AutoResizeColumns();

            base.OnPaint(e);
        }

        public void Load(IEnumerable<Song> songs)
        {
            songlist.Clear();

            foreach(Song s in songs) songlist.Add(s);
        }

        protected void Init()
        {
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            AllowUserToAddRows = false;
            BackgroundColor = Color.Black;
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
            DoubleBuffered = true;

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add("Edit");
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;

            CellDoubleClick += Song_List_CellDoubleClick;
            CellContextMenuStripNeeded += SongList_CellContextMenuStripNeeded;
            CellEndEdit += SongList_CellEndEdit;
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (CurrentCell != null) e.Cancel = false;
        }

        private void SongList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow r = Rows[e.RowIndex];
            Song s = r.DataBoundItem as Song;

            string artistName = r.Cells[Columns["ArtistName"].Index].Value as string;
            Artist artist = MusicDataBase.SearchArtist(artistName).FirstOrDefault();
            if (artist == null)
            {
                artist = new Artist { Name = artistName };
                artist.Id = MusicDataBase.CreateArtist(artist);
            }

            string albumName = r.Cells[Columns["AlbumName"].Index].Value as string;
            Album album = MusicDataBase.SearchAlbumTitle(albumName).FirstOrDefault();
            if (album == null)
            {
                album = new Album() { Artist = artist, Title = albumName, Cover = s.Cover, Tags = s.Album.Tags, Year = s.Album.Year };
                album.Id = MusicDataBase.CreateAlbum(album);
            }

            s.Album = album;
            s.Artist = artist;

            s.Save();

            CurrentCell = null;
        }

        private void SongList_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            CurrentCell = Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewCell c = CurrentCell;

            if (c.ColumnIndex == Columns["HeartControl"].Index || c.ColumnIndex == Columns["Duration"].Index) return;

            if (e.ClickedItem.Text == "Edit")
            {
                BeginEdit(true);
                c.InitializeEditingControl(c.RowIndex, "", c.Style);
            }
        }
    }

    public class SongEventArgs : EventArgs
    {
        public Song song { get; set; }
        public int pos { get; set; }
    }
}
