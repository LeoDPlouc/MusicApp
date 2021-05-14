using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicApp.Beans
{
    public class Album
    {
        public Album() => Songs = new List<Song>();
        public string Title { get; set; }
        public string Artist { get; set; }
        public List<Song> Songs { get; set; }
        public byte[] Cover { get => Songs.First().GetCover(); }

        public static List<Album> Albums { get; set; }

        public static void FetchAlbums()
        {
            Albums = new List<Album>();
            foreach (Song s in Song.Songs)
            {
                var album = Albums.Find((Album a) =>
                {
                    return a.Title == s.Album;
                });

                if (album == null)
                {
                    album = new Album() { Title = s.Album, Artist = s.Artist };
                    Albums.Add(album);
                }

                album.Songs.Add(s);
            }

            Beans.Artist.FetchArtists();
        }

        public static List<Album> SearchByTitle(string arg)
        {
            Regex pattern = new Regex(arg);

            return Albums.FindAll((Album a) =>
            {
                return pattern.IsMatch(a.Title);
            });
        }
    }
}
