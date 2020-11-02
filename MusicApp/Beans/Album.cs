using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Drawing;
using MusicApp.DB;
using MusicApp.Processing;

namespace MusicApp.Beans
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public string[] Tags { get; set; }
        public Picture Cover { get; set; }
        public int Year { get; set; }

        public void Save()
        {
            MusicDataBase.UpdateAlbum(this);

            foreach (Song s in MusicDataBase.SelectSongAlbum(this))
            {
                FileHandler.SaveSong(s);
            }
        }
    }
}
