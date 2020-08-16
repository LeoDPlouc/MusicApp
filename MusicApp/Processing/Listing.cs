using MusicApp.Beans;
using MusicApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Processing
{
    class Listing
    {
        public static List<Song> SearchSong(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return MusicDataBase.ListSongs();
            else return MusicDataBase.SearchSongTitle(arg);
        }
        public static List<Album> SearchAlbum(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return MusicDataBase.ListAlbum();
            else return MusicDataBase.SearchAlbumTitle(arg);
        }
        public static List<Artist> SearchArtist(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return MusicDataBase.ListArtist();
            else return MusicDataBase.SelectArtist(arg);
        }
    }
}
