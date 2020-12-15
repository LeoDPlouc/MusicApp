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
using MusicApp.DB;

namespace MusicApp.Control
{
    public partial class ArtistGrid : FlowLayoutPanel
    {
        public BindingList<Artist> artistlist;
        public ArtistGrid()
        {
            InitializeComponent();

            artistlist = new BindingList<Artist>();
            artistlist.ListChanged += Artistlist_ListChanged;

            Init();
        }

        public event EventHandler<ArtistControlEventArgs> ArtistControlClicked;

        private void Artistlist_ListChanged(object sender, ListChangedEventArgs e)
        {
            Controls.Clear();

            foreach (Artist a in artistlist)
            {
                var ac = new ArtistControl();
                ac.LoadArtist(a);
                Controls.Add(ac);

                ac.DoubleClick += Ac_DoubleClick;
            }
        }

        private void Ac_DoubleClick(object sender, EventArgs e)
        {
            ArtistControl ac = (ArtistControl)sender;
            ArtistControlClicked?.Invoke(this, new ArtistControlEventArgs()
            {
                artist = ac.Artist
            });
        }

        public async void LoadArtist(IEnumerable<Artist> artists)
        {
            SuspendLayout();
            artistlist.Clear();
            foreach (Artist a in artists)
            {
                artistlist.Add(a);
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
            foreach (ArtistControl a in Controls) a.Width = w;
            ResumeLayout();
        }
    }
    public class ArtistControl : UserControl
    {
        private FlowLayoutPanel panel;
        private PictureBox cover;
        private Label artistName;

        public Artist Artist { get; set; }

        public ArtistControl()
        {
            panel = new FlowLayoutPanel() { FlowDirection = FlowDirection.TopDown, AutoSize = true, AutoScroll = false };
            cover = new PictureBox() { SizeMode = PictureBoxSizeMode.StretchImage };
            artistName = new Label() { Anchor = AnchorStyles.None, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.Purple };

            DoubleBuffered = true;

            panel.DoubleClick += (sender, e) => OnDoubleClick(e);
            cover.DoubleClick += (sender, e) => OnDoubleClick(e);
            artistName.DoubleClick += (sender, e) => OnDoubleClick(e);

            panel.Controls.Add(cover);
            panel.Controls.Add(artistName);
            Controls.Add(panel);
        }

        public async void LoadArtist(Artist artist)
        {
            List<Album> albums = await artist.SelectAlbumFromArtist();
            Artist = artist;
            var coverTask = Picture.SelectPictureById(albums.First().CoverId);

            try
            {
                using (MemoryStream s = new MemoryStream((await coverTask).Data))
                {
                    cover.Image = Image.FromStream(s, true, true);
                }
            }
            catch { }
            artistName.Text = artist.Name;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            cover.ClientSize = new Size(Width, Width);
            Height = Width + 50;
        }
    }
    public class ArtistControlEventArgs : EventArgs
    {
        public Artist artist { get; set; }
    }
}
