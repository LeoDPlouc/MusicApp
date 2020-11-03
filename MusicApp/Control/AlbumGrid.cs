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
using System.IO;

namespace MusicApp.Control
{
    public partial class AlbumGrid : FlowLayoutPanel
    {
        public BindingList<Album> albumlist;
        public AlbumGrid()
        {
            InitializeComponent();

            albumlist = new BindingList<Album>();
            albumlist.ListChanged += Albumlist_ListChanged;

            Init();
        }

        public event EventHandler<AlbumControlEventArgs> AlbumControlClicked;

        private void Albumlist_ListChanged(object sender, ListChangedEventArgs e)
        {
            Controls.Clear();

            foreach (Album a in albumlist)
            {
                var ac = new AlbumControl();
                ac.LoadAlbum(a);
                Controls.Add(ac);

                ac.DoubleClick += Ac_DoubleClick;
            }
        }

        private void Ac_DoubleClick(object sender, EventArgs e)
        {
            AlbumControl ac = (AlbumControl)sender;
            AlbumControlClicked?.Invoke(this, new AlbumControlEventArgs()
            {
                album = ac.Album
            });
        }

        public async void LoadAlbum(IEnumerable<Album> albums)
        {
            SuspendLayout();
            albumlist.Clear();
            foreach (Album a in albums)
            {
                albumlist.Add(a);
                await Task.Delay(1);
            }
            ResumeLayout();
            Invalidate(true);
        }

        private void Init()
        {
            DoubleBuffered = true;
            AutoScroll = true;

            Resize += AlbumGrid_Resize;
        }

        private void AlbumGrid_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
            int colCount = DisplayRectangle.Width / 200;
            if (colCount == 0) colCount = 1;
            int w = DisplayRectangle.Width / colCount - 2 * Margin.All;
            foreach (AlbumControl a in Controls) a.Width = w;
            ResumeLayout();
            Invalidate(true);
        }
    }

    public class AlbumControl : UserControl
    {
        private FlowLayoutPanel panel;
        private PictureBox cover;
        private Label albumName;
        private Label artistName;

        public Album Album { get; set; }

        public AlbumControl()
        {
            panel = new FlowLayoutPanel() { FlowDirection = FlowDirection.TopDown, AutoSize = true, AutoScroll = false };
            cover = new PictureBox() { SizeMode = PictureBoxSizeMode.StretchImage };
            albumName = new Label() { Anchor = AnchorStyles.None, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.Purple };
            artistName = new Label() { Anchor = AnchorStyles.None, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.Purple };

            panel.DoubleClick += (sender, e) => OnDoubleClick(e);
            cover.DoubleClick += (sender, e) => OnDoubleClick(e);
            albumName.DoubleClick += (sender, e) => OnDoubleClick(e);
            artistName.DoubleClick += (sender, e) => OnDoubleClick(e);

            DoubleBuffered = true;

            panel.Controls.Add(cover);
            panel.Controls.Add(albumName);
            panel.Controls.Add(artistName);
            Controls.Add(panel);
        }

        public void LoadAlbum(Album album)
        {
            Album = album; 

            try
            {
                using (MemoryStream s = new MemoryStream(album.Cover.Data))
                {
                    cover.Image = Image.FromStream(s, true, true);
                }
            }
            catch { }
            albumName.Text = album.Title;
            artistName.Text = album.Artist.Name;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            cover.ClientSize = new Size(Width, Width);
            Height = Width + 50;
        }
    }

    public class AlbumControlEventArgs : EventArgs
    {
        public Album album { get; set; }
    }
}
