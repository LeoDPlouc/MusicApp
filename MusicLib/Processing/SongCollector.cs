using MusicLib.Objects;
using MusicLib.Files;
using MusicLib.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MusicLib.Processing
{
    public class SongCollector
    {
        private static Thread collector;

        public static void Start(string path, bool serverEnabled)
        {
            collector = new Thread(DoWork);
            collector.Start(new object[]
            {
                path,
                serverEnabled
            });
        }

        public static void Stop() => collector.Abort();

        private static void DoWork(object arg)
        {
            object[] args = arg as object[];

            while (true)
            {
                Collect(args[0] as string, (bool)args[1]).Wait();
                Thread.Sleep(5000);
            }
        }

        private static async Task Collect(string path, bool serverEnabled)
        {
            SongCollection songs = SongCollection.GetInstance();
            string[] paths = FileHandler.ListAllSongPath(path);

            foreach(string p in paths)
            {
                Song song = null;
                try
                {
                    song = await LoadSong(p, serverEnabled);
                }
                catch
                {
                    FileHandler.ConvertToMP3(p);
                    song = await LoadSong(p, serverEnabled);
                }

                if (song != null)
                    songs.Add(song);
            }
        }

        private static async Task<Song> LoadSong(string path, bool serverEnabled)
        {
            if (SongCollection.GetInstance().Any(s => s.Path == path))
                return null;

            Song song = FileHandler.LoadSong(path);

            if (song.AcousticId is null)
            {
                song.AcousticId = AcousticID.ComputeAcousticId(song.Path);
                FileHandler.SaveSong(song);
            }

            SongInfo songInfo;
            if (serverEnabled)
                songInfo = await Client.GetSongInfo(song.AcousticId, "127.0.0.1");
            else
                songInfo = InfoFiles.Load(song.AcousticId);

            song.Heart = songInfo.Heart;
            song.Like = songInfo.Like;

            return song;
        }

        /*public static void CollectSongs()
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
        }*/
    }
}
