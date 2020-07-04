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
    public partial class Main : UserControl
    {

        Song_List _song_list;

        public Main()
        {
            InitializeComponent();

            _song_list = new Song_List()
            {
                EditMode = DataGridViewEditMode.EditProgrammatically
            };

            Controls.Add(_song_list);

            _song_list.CellMouseDoubleClick += _song_list_CellMouseDoubleClick;
        }

        private void _song_list_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = _song_list.Rows[e.RowIndex];
            Song s = (Song)row.DataBoundItem;

            OnSongDoubleClicked(new SongDoubleClickEventArgs() { Song = s });
        }

        public event EventHandler<SongDoubleClickEventArgs> SongDoubleClick;
        protected virtual void OnSongDoubleClicked(SongDoubleClickEventArgs e)
        {
            var handler = SongDoubleClick;
            handler?.Invoke(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _song_list.Size = DisplayRectangle.Size;
            _song_list.Location = new Point(0, 0);
        }
    }

    public class SongDoubleClickEventArgs : EventArgs
    {
        public Song Song { get; set; }
    }
}
