using MusicLib.Objects;
using System.Windows.Forms;

namespace MusicApp.Control
{
    public partial class AlbumPresentation : UserControl
    {
        #region Constants
        const int headerHeight = 200;
        #endregion

        #region UI Parts
        SongList songList;
        FlowLayoutPanel panel;
        AlbumHeader header;
        #endregion
        public AlbumPresentation()
        {
            InitializeComponent();
            Init();
        }

        public void LoadAlbum(Album album)
        {
            header.LoadAlbum(album);
            songList.Load(album.Songs);
        }

        private void Init()
        {
            panel = new FlowLayoutPanel() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };

            header = new AlbumHeader() { Height = headerHeight };
            songList = new SongList();

            panel.Controls.Add(header);
            panel.Controls.Add(songList);
            
            Controls.Add(panel);

            Invalidate(true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            header.Width = Width;
            songList.Width = Width;
        }
    }
}
