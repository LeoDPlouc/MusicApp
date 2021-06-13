using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicLib.Objects
{
    public class ArtistCollection : IEnumerable<Artist>, ICollection<Artist>
    {
        private static List<Artist> artists;
        private static ArtistCollection instance;

        public int Count => artists.Count;
        public bool IsReadOnly => false;

        public event EventHandler CollectionChanged;
        public void OnCollectionChanged() => CollectionChanged?.Invoke(this, new EventArgs());

        private ArtistCollection()
        {
            if (artists == null)
                artists = new List<Artist>();
        }
        public static ArtistCollection GetInstance()
        {
            if (instance is null)
                instance = new ArtistCollection();
            return instance;
        }

        public static void FetchArtists()
        {
            artists = new List<Artist>();
            foreach (Album alb in AlbumCollection.GetInstance())
            {
                var artist = artists.Find((Artist art) =>
                {
                    return alb.Artist == art.Name;
                });

                if (artist == null)
                {
                    artist = new Artist() { Name = alb.Artist };
                    artists.Add(artist);
                }

                artist.Albums.Add(alb);
            }
        }

        public List<Artist> SearchByName(string arg)
        {
            Regex pattern = new Regex(arg);

            return artists.FindAll((Artist a) =>
            {
                return pattern.IsMatch(a.Name);
            });
        }

        public IEnumerator<Artist> GetEnumerator() => artists.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => artists.GetEnumerator();

        public void Add(Artist item)
        {
            artists.Add(item);
            OnCollectionChanged();
        }
        public void Clear()
        {
            artists.Clear();
            OnCollectionChanged();
        }
        public bool Contains(Artist item) => artists.Contains(item);
        public void CopyTo(Artist[] array, int arrayIndex) => artists.CopyTo(array, arrayIndex);
        public bool Remove(Artist item)
        {
            bool remove = artists.Remove(item);
            OnCollectionChanged();
            return remove;
        }
    }
}
