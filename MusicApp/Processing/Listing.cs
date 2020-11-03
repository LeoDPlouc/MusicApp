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
        public async static Task<List<Song>> SearchSong(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return await MusicDataBase.ListSongs();
            else return await MusicDataBase.SearchSongTitle(arg);
        }
        public async static Task<List<Album>> SearchAlbum(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return await MusicDataBase.ListAlbum();
            else return await MusicDataBase.SearchAlbumTitle(arg);
        }
        public async static Task<List<Artist>> SearchArtist(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return await MusicDataBase.ListArtist();
            else return await MusicDataBase.SearchArtist(arg);
        }
    }
}
