using MusicApp.DB;
using MusicApp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    public partial class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public async void Save()
        {
            MusicDataBase.UpdateArtist(this);

            foreach(Album a in await SelectAlbumFromArtist())
            {
                a.Save();
            }
        }
    }
}
