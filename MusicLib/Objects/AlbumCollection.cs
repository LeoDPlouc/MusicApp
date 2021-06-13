using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicLib.Objects
{
    public class AlbumCollection
    {
        public static List<Album> Albums { get; set; }

        public static void FetchAlbums()
        {
            Albums = new List<Album>();
            foreach (Song s in SongCollection.Songs)
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

            ArtistCollection.FetchArtists();
        }

        internal static void Init()
        {
            if (Albums == null)
                Albums = new List<Album>();
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
