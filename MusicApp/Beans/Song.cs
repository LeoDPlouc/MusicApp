using MusicApp.Config;
using MusicLib.Files;
using MusicLib.Processing;
using MusicLib.Server;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    public partial class Song : MusicLib.Beans.Song
    {

        public static List<Song> Songs { get; set; }
        public static void CollectSongs()
        {
            Songs = new List<Song>();

            string libraryPath = Configuration.LibraryPath;
            if (string.IsNullOrEmpty(libraryPath) || !Directory.Exists(libraryPath))
                return;

            Thread t = new Thread(async () =>
            {

                foreach (string path in FileHandler.ListAllSongPath(Configuration.LibraryPath))
                {
                    Song song = new Song();
                    await FileHandler.LoadSong(path, Configuration.ServerEnabled, song);
                    Songs.Add(song);
                }
            });

            t.Start();
            t.Join();

            Songs.ForEach(async (Song song) => await song.Save());

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

            if (Configuration.ServerEnabled)
                await Client.SendSongInfo(GetSongInfo(), "127.0.0.1");
            else
                await InfoFiles.Save(GetSongInfo());
        }
    }
}
