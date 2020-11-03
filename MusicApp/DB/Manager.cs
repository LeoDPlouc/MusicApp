using MusicApp.Beans;
using MusicApp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.DB
{
    class Manager
    {
        public static void VerifyContent()
        {
            TaskFactory factory = new TaskFactory();
            factory.StartNew(() => VerifyContentTask());
        }
        public static void Collect()
        {
            TaskFactory factory = new TaskFactory();
            factory.StartNew(() => CollectTask());
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
    }
}
