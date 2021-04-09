using MusicLib.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    public partial class Artist
    {
        public Artist() => Albums = new List<Album>();
        public string Name { get; set; }
        public List<Album> Albums;
    }
}
