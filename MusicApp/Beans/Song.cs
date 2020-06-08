using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MusicApp.Beans
{
    class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int N { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Like { get; set; }
        public bool Heart { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
        public string Path { get; set; }

        public static Song PathToSong(string path)
        {
            File file = File.Create(path);

            Song song = new Song {/*Album = file.Tag.Album, Artist = file.Tag.Artist,*/ N = (int)file.Tag.Track, Title = file.Tag.Title, Path = path };
            return song;
        }
    }
}
