using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MusicApp.Config;
using MusicApp.Parts;
using MusicLib.Objects;

namespace MusicApp.Control
{
    public partial class SongList : DataGridView
    {
        public BindingList<Song> songlist;


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
            if (Columns["HeartControl"] == null) Columns.Add("HeartControl", "HeartControl");

            Columns["N"].Visible = Type == SongListType.Album;

            Columns["Path"].Visible = false;
            Columns["Heart"].Visible = false;
            Columns["Like"].Visible = false;
            Columns["AcousticId"].Visible = false;

            Columns["N"].DisplayIndex = 0;

            foreach(DataGridViewRow r in Rows)
            {
                Song s = (Song)r.DataBoundItem;
                Color sColor = s.Like ? (s.Heart ? Color.DarkRed : Color.MediumPurple) : Color.White;

                r.Cells["HeartControl"].Value = "♥";
                r.Cells["HeartControl"].Style = new DataGridViewCellStyle() { ForeColor = sColor, SelectionForeColor = sColor};
            }
        }

        private async void Song_List_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

                await s.Save(Configuration.ServerEnabled);
                SongList_DataBindingComplete(null, null);

                return;
            }

            Playlist.SetCurrentSong(s, songlist);
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

        private async void SongList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow r = Rows[e.RowIndex];
            Song s = r.DataBoundItem as Song;
            await s.Save(Configuration.ServerEnabled);
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
}
