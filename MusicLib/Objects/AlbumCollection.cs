using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicLib.Objects
{
    public class AlbumCollection : IEnumerable<Album>, ICollection<Album>
    {
        private static List<Album> albums;
        private static AlbumCollection instance;

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public event EventHandler CollectionChanged;
        public void OnCollectionChanged() => CollectionChanged?.Invoke(this, new EventArgs());

        private AlbumCollection()
        {
            if (albums == null)
                albums = new List<Album>();
        }
        public static AlbumCollection GetInstance()
        {
            if (instance is null)
                instance = new AlbumCollection();
            return instance;
        }

        public static void FetchAlbums()
        {
            albums = new List<Album>();
            foreach (Song s in SongCollection.GetInstance())
            {
                var album = albums.Find((Album a) =>
                {
                    return a.Title == s.Album;
                });

                if (album == null)
                {
                    album = new Album() { Title = s.Album, Artist = s.Artist };
                    albums.Add(album);
                }

                album.Songs.Add(s);
            }

            ArtistCollection.FetchArtists();
        }

        public List<Album> SearchByTitle(string arg)
        {
            Regex pattern = new Regex(arg);

            return albums.FindAll((Album a) =>
            {
                return pattern.IsMatch(a.Title);
            });
        }

        public IEnumerator<Album> GetEnumerator() => albums.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => albums.GetEnumerator();

        public void Add(Album item)
        {
            albums.Add(item);
            OnCollectionChanged();
        }
        public void Clear()
        {
            albums.Clear();
            OnCollectionChanged();
        }
        public bool Contains(Album item) => albums.Contains(item);
        public void CopyTo(Album[] array, int arrayIndex) => albums.CopyTo(array, arrayIndex);
        public bool Remove(Album item)
        {
            bool remove = albums.Remove(item);
            OnCollectionChanged();
            return remove;
        }
    }
}
