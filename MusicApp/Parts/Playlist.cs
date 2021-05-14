using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Parts
{
    class Playlist
    {
        public static void InitPlaylist()
        {
            songList = new List<Song>();

            Player.SongFinished += Playlist_SongFinished;
        }

        private static void Playlist_SongFinished(object sender, EventArgs e)
        {
            CurrentSong = Next();
        }

        #region Private Members
        private static List<Song> songList;
        #endregion

        #region Public Members
        public static List<Song> SongList { get => songList; }
        public static Song CurrentSong { get; private set; }
        #endregion

        #region Events
        public static event EventHandler PlaylistChanged;
        public static event EventHandler SongChanged;
        /// <summary>
        /// Raise a new PlaylistChanged envent
        /// </summary>
        protected static void OnPlaylistChanged()
        {
            PlaylistChanged?.Invoke(null, new EventArgs());
        }
        protected static void OnSongChanged()
        {
            SongChanged?.Invoke(null, new EventArgs());
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Load the collection of songs as the playlist's songs
        /// </summary>
        /// <param name="songs">A collection of songs to load</param>
        public static void Load(IEnumerable<Song> songs)
        {
            songList.Clear();
            foreach (Song s in songs) songList.Add(s);

            OnPlaylistChanged();
        }
        /// <summary>
        /// Load the song as the playlist's song
        /// </summary>
        /// <param name="songs">A song to load</param>
        public void Load(Song song)
        {
            songList.Clear();
            songList.Add(song);

            OnPlaylistChanged();
        }
        /// <summary>
        /// Go to the next song
        /// </summary>
        /// <returns>The next song</returns>
        public static Song Next()
        {
            CurrentSong = songList[GetPosition() + 1];

            OnSongChanged();

            return CurrentSong;
        }
        public static int GetPosition() => songList.FindIndex((Song s) => s == CurrentSong);
        public static void SetCurrentSong(Song song) => CurrentSong = song;
        #endregion
    }
}
