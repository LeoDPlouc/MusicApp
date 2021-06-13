using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicLib.Objects
{
    public class SongCollection
    {
        public static List<Song> Songs { get; set; }

        public static List<Song> SearchByTitle(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Title);
            });
        }

        public static void Init()
        {
            if (Songs == null)
                Songs = new List<Song>();
        }

        public static List<Song> SearchByAlbum(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Album);
            });
        }
        public static List<Song> SearchByArtist(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Artist);
            });
        }
    }
}
