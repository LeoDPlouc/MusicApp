using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Drawing;

namespace MusicApp.Beans
{
    class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public Artist Artist { get; set; }
        public string[] Tags { get; set; }
        public Image Cover { get; set; }
        public int Year { get; set; }
    }
}
