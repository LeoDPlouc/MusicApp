using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SQLite.Net.Attributes;
using System.Drawing;

namespace MusicApp.Beans
{
    class Album
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        [Ignore]
        public Artist Artist { get; set; }
        [Ignore]
        public string[] Tags { get; set; }
        [Ignore]
        public Image Cover { get; set; }
        public int Year { get; set; }
    }
}
