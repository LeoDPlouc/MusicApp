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
    partial class Album
    {
        public async static Task<List<Album>> SelectAllAlbum()
        {
            return await MusicDataBase.ListAlbum();
        }
        public async static Task<Album> SelectAlbumById(int id)
        {
            var albums = await SelectAllAlbum();
            return albums.Find((Album a) =>
            {
                return a.Id == id;
            });
        }
        public async static Task<Album> SelectAlbumByTitleAndByArtistName(string title, string artistName)
        {
            var albums = await SelectAllAlbum();
            return albums.Find((Album a) =>
            {
                return a.Title == title && a.Artist.Name == artistName;
            });
        }
        public async static Task<List<Album>> SelectAlbumByArtist(string artistName)
        {
            var albums = await SelectAllAlbum();
            return albums.FindAll((Album a) =>
            {
                return a.Artist.Name == artistName;
            });
        }
        public async static Task<List<Album>> SearchAlbumByName(string query)
        {
            var albumsTask = SelectAllAlbum();

            Regex re = new Regex(query);

            var albums = await albumsTask;
            return albums.FindAll((Album a) =>
            {
                return re.IsMatch(a.Title);
            });
        }

        public async Task<List<Song>> SelectSongsFromAlbum()
        {
            var songs = await Song.ListAllSongs();
            return songs.FindAll((Song s) =>
            {
                return s.Album.Id == Id;
            });
        }
        public async void Create()
        {
            Id = await MusicDataBase.CreateAlbum(this);
        }
        public async void Save()
        {
            MusicDataBase.UpdateAlbum(this);

            foreach (Song s in await SelectSongsFromAlbum())
            {
                FileHandler.SaveSong(s);
            }
        }
    }
}
