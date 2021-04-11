using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MusicApp.Beans
{
    public class Artist
    {
        public Artist() => Albums = new List<Album>();
        public string Name { get; set; }
        public List<Album> Albums;

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
