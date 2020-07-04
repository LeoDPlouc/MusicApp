using MusicApp.Beans;
using MusicApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Processing
{
    class SongListing
    {
        public static List<Song> SearchSong(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return Music_DataBase.ListSongs();
            else return Music_DataBase.SearchTitle(arg);
        }
    }
}
