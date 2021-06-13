using MusicLib.Objects;
using System.Windows.Forms;

namespace MusicApp.Control
{
    public partial class ArtistPresentation : UserControl
    {
        #region Constants
        const int headerHeight = 200;
        #endregion

        #region UI Parts
        AlbumGrid albumGrid;
        FlowLayoutPanel panel;
        ArtistHeader header;
        #endregion
        public ArtistPresentation()
        {
            InitializeComponent();
            Init();
        }

        public void LoadArtist(Artist artist)
        {
            header.LoadArtist(artist);
            albumGrid.LoadAlbum(artist.Albums);
        }

        private void Init()
        {
            panel = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };

            header = new ArtistHeader() { Height = headerHeight };
            albumGrid = new AlbumGrid();

            panel.Controls.Add(header);
            panel.Controls.Add(albumGrid);

            Controls.Add(panel);

            Invalidate(true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            header.Width = Width;
            albumGrid.Width = Width;
        }
    }
}
