using MusicApp.Beans;
using MusicApp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicApp.DB
{
    class Manager
    {
        public static void VerifyContent()
        {
            Thread t = new Thread(new ThreadStart(VerifyContentTask));
            t.Start();
        }
        public static void Collect()
        {
            Thread t = new Thread(new ThreadStart(CollectTask));
            t.Start();
            t.Join();
        }
        public static void Clean()
        {
            Thread t = new Thread(new ThreadStart(CleanPicTask));
            t.Start();
        }

        private async static void VerifyContentTask()
        {

            foreach (Song s in await MusicDataBase.ListSongs())
            {
                var loadSongTask = FileHandler.LoadSong(s.Path);
                if (s.Hash != FileHandler.HashFromFile(s.Path))
                {
                    Song song = await loadSongTask;

                    song.Id = s.Id;
                    song.ComputeHash();
                    song.Save();
                }
            }
        }
        private async static void CollectTask()
        {
            string path = @"C:\Users\Leo\Desktop\musictest";

            foreach (string s in Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories))
            {
                if (!await Song.ExistSongByPath(s))
                {
                    Song song = await FileHandler.LoadSong(s);
                    song.Create();
                }
            }
        }
        private async static void CleanPicTask()
        {
            var picsTask = Picture.SelectAllPictures();
            var albumsTask = Album.SelectAllAlbum();

            var pics = await picsTask;
            var albums = await albumsTask;
            foreach(Picture pic in pics)
            {
                int count = albums.Count((Album a) =>
                {
                    return a.CoverId == pic.Id;
                });
                if (count < 1) pic.Delete();
            }
        }
    }
}
