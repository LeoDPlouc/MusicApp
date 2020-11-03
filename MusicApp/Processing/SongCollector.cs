using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MusicApp.Beans;
using MusicApp.DB;
using TagLib;

namespace MusicApp.Processing
{
    class SongCollector
    {
        public async static void Collect()
        {
            string path = @"C:\Users\Leo\Desktop\musictest";

            foreach (string s in Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories))
            {
                if(!MusicDataBase.ExistSong(s))
                {
                    Song song = await FileHandler.LoadSong(s);
                    MusicDataBase.CreateSong(song);
                }
                await Task.Delay(1);
            }
        }
    }
}
