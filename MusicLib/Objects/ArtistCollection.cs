using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicLib.Objects
{
    public class ArtistCollection
    {
        public static List<Artist> Artists;

        public static void FetchArtists()
        {
            Artists = new List<Artist>();
            foreach (Album alb in AlbumCollection.Albums)
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

        internal static void Init()
        {
            if (Artists == null)
                Artists = new List<Artist>();
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
