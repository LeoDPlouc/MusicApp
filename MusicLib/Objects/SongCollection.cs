using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicLib.Objects
{
    public class SongCollection : IEnumerable<Song>, ICollection<Song>
    {
        private static List<Song> songs;
        private static SongCollection instance;

        public int Count => songs.Count;
        public bool IsReadOnly => false;

        public event EventHandler CollectionChanged;
        public void OnCollectionChanged() => CollectionChanged?.Invoke(this, new EventArgs());

        private SongCollection()
        {
            if (songs == null)
                songs = new List<Song>();
        }
        public static SongCollection GetInstance()
        {
            if (instance is null)
                instance = new SongCollection();
            return instance;
        }

        public List<Song> SearchByTitle(string arg)
        {
            Regex pattern = new Regex(arg);

            return songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Title);
            });
        }
        public List<Song> SearchByAlbum(string arg)
        {
            Regex pattern = new Regex(arg);

            return songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Album);
            });
        }
        public List<Song> SearchByArtist(string arg)
        {
            Regex pattern = new Regex(arg);

            return songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Artist);
            });
        }

        public IEnumerator<Song> GetEnumerator() => songs.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => songs.GetEnumerator();

        public void Add(Song item)
        {
            songs.Add(item);
            OnCollectionChanged();
        }
        public void Clear()
        {
            songs.Clear();
            OnCollectionChanged();
        }
        public bool Contains(Song item) => songs.Contains(item);
        public void CopyTo(Song[] array, int arrayIndex) => songs.CopyTo(array, arrayIndex);
        public bool Remove(Song item)
        {
            bool remove = songs.Remove(item);
            OnCollectionChanged();
            return remove;
        }
    }
}
