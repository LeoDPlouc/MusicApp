using MusicApp.Beans;
using MusicApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace MusicApp.Processing
{
    class FileHandler
    {
        public async static Task<Song> LoadSong(string path)
        {
            TagLib.File f = TagLib.File.Create(path);

            var coverTask = LoadCover(f);
            var artistTask =  LoadArtist(f);

            Beans.Picture cover = await coverTask;
            Artist artist = await artistTask;

            var albumTask = LoadAlbum(f, artist, cover);

            return new Song
            {
                Album = await albumTask,
                Artist = artist,
                CoverId = cover.Id,
                Duration = f.Length,
                Heart = false,
                Like = false,
                N = (int)f.Tag.Track,
                Path = path,
                Title = f.Tag.Title
            };
        }
        private async static Task<Beans.Picture> LoadCover(TagLib.File file)
        {
            byte[] data = file.Tag.Pictures.First().Data.Data;

            var coverTask = Picture.SelectPictureByData(data);

            Beans.Picture cover = await coverTask;
            if (cover == null)
            {
                cover = new Beans.Picture { Data = data };
                cover.Id = MusicDataBase.CreatePicture(cover);
            }

            return cover;
        }
        private async static Task<Artist> LoadArtist(TagLib.File file)
        {
            string name = file.Tag.FirstAlbumArtist;
            var artistTask = Artist.SelectArtistByName(name);

            Artist artist = await artistTask;
            if (artist == null)
            {
                artist = new Artist
                {
                    Name = name
                };
                artist.Id = MusicDataBase.CreateArtist(artist);
            }

            return artist;
        }
        private async static Task<Album> LoadAlbum(TagLib.File file, Artist artist, Beans.Picture cover)
        {
            string title = file.Tag.Album;
            Album album = await Album.SelectAlbumByTitleAndByArtistName(title, artist.Name);
            if (album == null)
            {
                album = new Album
                {
                    Artist = artist,
                    CoverId = cover.Id,
                    Tags = file.Tag.Genres,
                    Title = title,
                    Year = (int)file.Tag.Year
                };
                album.Create();
            }
            return album;
        }

        public static string HashFromFile(string path)
        {
            var f = TagLib.File.Create(path);
            byte[] data = Encoding.Default.GetBytes(f.ToString());
            using(SHA256 h = SHA256.Create())
            {
                return Encoding.UTF8.GetString(h.ComputeHash(data));
            }
        }
        public static async void SaveSong(Song song)
        {
            TagLib.File f = TagLib.File.Create(song.Path);

            f.Tag.Album = song.Album.Title;
            f.Tag.AlbumArtists = new string[] { song.Artist.Name };
            f.Tag.Pictures = new TagLib.Picture[] { new TagLib.Picture(new TagLib.ByteVector((await Picture.SelectPictureById(song.CoverId)).Data)) };
            f.Tag.Track = (uint)song.N;
            f.Tag.Title = song.Title;

            f.Save();
        }
    }
}
