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
                await Task.Delay(1);
            }
        }
        private async static void CollectTask()
        {
            string path = @"C:\Users\Leo\Desktop\musictest";

            foreach (string s in Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories))
            {
                if (!MusicDataBase.ExistSong(s))
                {
                    Song song = await FileHandler.LoadSong(s);
                    MusicDataBase.CreateSong(song);
                }
                await Task.Delay(1);
            }
        }
        private static void CleanPicTask()
        {
            var ids = MusicDataBase.ListIdPicture();
            foreach(int id in ids)
            {
                if (MusicDataBase.CountAlbumWithPic(id) < 1) MusicDataBase.DeletePicture(id);
            }
        }
    }
}
