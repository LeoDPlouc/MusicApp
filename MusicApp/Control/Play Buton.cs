using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.Asf;

namespace MusicApp.Control
{
    public partial class Play_Button : Button
    {
        Pen pen;
        Brush brush;

        public bool play { get; set; }
        bool hover;

        public bool sem { get; set; }

        public Play_Button()
        {
            InitializeComponent();

            pen = new Pen(Brushes.Purple, 1);

            BackColor = Color.Transparent;
            brush = Brushes.Purple;

            play = false;
            hover = false;
            sem = false;

            MouseClick += Play_Button_MouseClick;
            MouseEnter += Play_Button_MouseEnter;
            MouseLeave += Play_Button_MouseLeave;
        }

        private void Play_Button_MouseLeave(object sender, EventArgs e)
        {
            hover = false;
            Invalidate();
        }

        private void Play_Button_MouseEnter(object sender, EventArgs e)
        {
            hover = true;
            Invalidate();
        }

        private void Play_Button_MouseClick(object sender, MouseEventArgs e)
        {
            play = !play;
            Invalidate();

            sem = true;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {

            base.OnPaint(pevent);
            var g = pevent.Graphics;
            g.Clear(Color.Transparent);

            if (play) drawPause(g);
            else drawPlay(g);
            
        }

        private void drawPlay(Graphics g)
        {
            var points = new Point[]
            {
                new Point(0, 0),
                new Point(DisplayRectangle.Width, DisplayRectangle.Height /2),
                new Point(0, DisplayRectangle.Height)
            };

            if (hover) g.FillPolygon(brush, points);
            else g.DrawPolygon(pen, points);
        }
        private void drawPause(Graphics g)
        {
            var r1 = new Rectangle(0, 0, DisplayRectangle.Width / 3 - 1, DisplayRectangle.Height - 1);
            var r2 = new Rectangle(2 * DisplayRectangle.Width / 3, 0, DisplayRectangle.Width / 3 - 1, DisplayRectangle.Height - 1);

            if(hover)
            {
                g.FillRectangle(brush, r1);
                g.FillRectangle(brush, r2);
            }
            else
            {
                g.DrawRectangle(pen, r1);
                g.DrawRectangle(pen, r2);
            }
        }
    }
}
