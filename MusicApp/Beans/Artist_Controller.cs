using MusicApp.DB;
using MusicApp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    public partial class Artist
    {
        public static List<Artist> Artists;

        public static void FetchArtists()
        {
            if (Artists == null) Artists = new List<Artist>();
            foreach (Album alb in Album.Albums)
            {
                var artist = Artists.Find((Artist art) =>
                {
                    return alb.Artist == art.Name;
                });

                if (artist == null)
                {
                    artist = new Artist() { Name = alb.Artist };
                    Artists.Add(artist);
                }

                artist.Albums.Add(alb);
            }
        }

        public static List<Artist> SearchByName(string arg)
        {
            Regex pattern = new Regex(arg);

            return Artists.FindAll((Artist a) =>
            {
                return pattern.IsMatch(a.Name);
            });
        }
    }
}
