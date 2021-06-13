using System.Drawing;
using System.Windows.Forms;
using MusicLib.Objects;

namespace MusicApp.Control
{
    public partial class ArtistHeader : UserControl
    {
        Label ArtistName;

        Artist Artist { get; set; }

        public ArtistHeader()
        {
            InitializeComponent();
            Init();
        }

        public void LoadArtist(Artist artist)
        {
            Artist = artist;

            ArtistName.Text = Artist.Name;
        }

        private void Init()
        {
            ArtistName = new Label() { ForeColor = Color.Purple };

            Controls.Add(ArtistName);

            ArtistName.ContextMenuStrip = new ContextMenuStrip();
            ArtistName.ContextMenuStrip.Items.Add("Edit");
            ArtistName.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            /*if (e.ClickedItem.Text == "Edit")
            {
                Label newTitleLabel = new Label() { Text = "New Artist Name", AutoSize = true, Margin = new Padding(0, 6, 0, 0) };
                TextBox NewTitle = new TextBox() { Width = 100 };

                FlowLayoutPanel panel = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight };
                panel.Controls.Add(newTitleLabel);
                panel.Controls.Add(NewTitle);

                Form form = new Form() { Size = new Size(210, 65) };
                form.Controls.Add(panel);

                form.ShowDialog();

                ArtistName.Text = NewTitle.Text;
                Artist.Name = ArtistName.Text;

                Artist.Save();
            }*/
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ArtistName.Location = new Point(Height, 0);
        }
    }
}
