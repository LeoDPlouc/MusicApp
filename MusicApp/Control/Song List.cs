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
    public partial class Song_List : DataGridView
    {
        public Song_List()
        {
            InitializeComponent();

            DataSource = Music_DataBase.ListSongs();

            SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            CellBorderStyle = DataGridViewCellBorderStyle.None;
            DefaultCellStyle = new DataGridViewCellStyle()
            {

                BackColor = Color.Black,
                ForeColor = Color.Purple,
                SelectionBackColor = Color.Black,
                SelectionForeColor = Color.Purple,
            };
            ColumnHeadersVisible = false;
            RowHeadersVisible = false;
            ScrollBars = ScrollBars.Vertical;
            AllowUserToResizeColumns = false;
            AllowUserToResizeRows = false;
            GridColor = Color.Purple;
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.FromArgb(10,10,10)
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            AutoResizeColumns();

            base.OnPaint(e);
        }
    }
}
