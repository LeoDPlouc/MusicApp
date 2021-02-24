using MusicApp.Beans;
using MusicApp.MusicServerController;
using MusicApp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    public partial class Song
    {
        public static List<Song> Songs { get; set; }
        public static async Task CollectSongs()
        {
            if (Songs == null) Songs = new List<Song>();

            Songs.Clear();
            foreach (string path in FileHandler.ListAllSongPath()) Songs.Add(await FileHandler.LoadSong(path));

            Beans.Album.FetchAlbums();
        }

        public static List<Song> SearchByTitle(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Title);
            });
        }
        public static List<Song> SearchByAlbum(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Album);
            });
        }
        public static List<Song> SearchByArtist(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Artist);
            });
        }
        public void Save()
        {
            MusicServer.SaveSong(this);
            FileHandler.SaveSong(this);
        }
    }
}
