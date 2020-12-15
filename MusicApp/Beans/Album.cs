using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Drawing;
using MusicApp.DB;
using MusicApp.Processing;

namespace MusicApp.Beans
{
    public partial class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public string[] Tags { get; set; }
        public int CoverId { get; set; }
        public int Year { get; set; }
    }
}
