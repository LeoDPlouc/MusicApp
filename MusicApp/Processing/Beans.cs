using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MusicApp.Processing
{
    class Beans
    {
        public static Song PathToSong(string path)
        {
            File file = File.Create(path);

            Song song = new Song {/*Album = file.Tag.Album, Artist = file.Tag.Artist,*/ N = (int)file.Tag.Track, Title = file.Tag.Title, Path = path };
            return song;
        }
    }
}
