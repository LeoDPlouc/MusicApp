using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    class Song
    {
        public string Title;
        public int N;
        public TimeSpan Duration;
        public bool Like;
        public bool Heart;
        public Image Cover;
        public DateTime Date;
        public Artist Artist;
    }
}
