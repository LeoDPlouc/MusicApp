using MusicLib.Processing;
using MusicLib.Server;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    public partial class Song : MusicLib.Beans.Song
    {

        public static List<Song> Songs { get; set; }
        public static async Task CollectSongs()
        {
            if (Songs == null) Songs = new List<Song>();

            Songs.Clear();
            foreach (string path in FileHandler.ListAllSongPath())
            {
                Song song = await FileHandler.LoadSong(path) as Song;
                Songs.Add(song);
            }

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
        public async Task Save()
        {
            FileHandler.SaveSong(this);
            await Client.SendSongInfo(this.GetSongInfo(), "127.0.0.1");
        }
    }
}
