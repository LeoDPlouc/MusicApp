using LibVLCSharp.Shared;
using MusicApp.DB;
using MusicApp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MusicApp.Beans
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int N { get; set; }
        public long Duration { get; set; }
        public bool Like { get; set; }
        public bool Heart { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
        public Picture Cover { get; set; }
        public string Path { get; set; }

        public string Hash { get; set; }

        public void ComputeHash()
        {
            Hash = FileHandler.HashFromFile(Path);
        }
        public void Save()
        {
            MusicDataBase.UpdateSong(this);
            FileHandler.SaveSong(this);
        }
    }
}
