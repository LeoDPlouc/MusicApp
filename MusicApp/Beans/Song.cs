using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    class Song
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public int N { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Like { get; set; }
        public bool Heart { get; set; }
        public DateTime Date { get; set; }
        [Ignore]
        public Artist Artist { get; set; }
        [Ignore]
        public Album Album { get; set; }
    }
}
