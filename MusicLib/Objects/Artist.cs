using System;
using System.Collections.Generic;
using System.Text;

namespace MusicLib.Objects
{
    public class Artist
    {
        public Artist() => Albums = new List<Album>();
        public string Name { get; set; }
        public List<Album> Albums;
    }
}
