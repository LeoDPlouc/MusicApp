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

        public SongList()
        {
            InitializeComponent();
            Init();

            songlist = new BindingList<Song>();
            DataSource = songlist;
        }

        private void Song_List_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = Rows[e.RowIndex];
            Song s = (Song)row.DataBoundItem;

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
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.FromArgb(10, 10, 10)
            };
            EditMode = DataGridViewEditMode.EditProgrammatically;

            CellDoubleClick += Song_List_CellDoubleClick;
        }
    }

    public class SongEventArgs : EventArgs
    {
        public Song song { get; set; }
        public int pos { get; set; }
    }
}
