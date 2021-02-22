using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Drawing;
using MusicApp.Processing;

namespace MusicApp.Beans
{
    public partial class Album
    {
        public Album() => Songs = new List<Song>();
        public string Title { get; set; }
        public string Artist { get; set; }
        public List<Song> Songs { get; set; }

        public Picture Cover { get => Songs.First().Cover; }
    }
}
