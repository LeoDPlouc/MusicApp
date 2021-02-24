using LibVLCSharp.Shared;
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
    public partial class Song
    {
        public string Title { get; set; }
        public int N { get; set; }
        public long Duration { get; set; }
        public bool Like { get; set; }
        public bool Heart { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public Picture Cover { get => FileHandler.LoadCover(Path); }
        public string Path { get; set; }
        public string AcousticId { get; set; }
    }
}
