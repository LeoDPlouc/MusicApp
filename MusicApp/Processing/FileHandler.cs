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
                Cover = cover,
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

            var coverTask = MusicDataBase.SelectPicture(data);

            Beans.Picture cover;
            List<Beans.Picture> pictures = await coverTask;
            if (pictures.Count == 0)
            {
                cover = new Beans.Picture { Data = data };
                cover.Id = MusicDataBase.CreatePicture(cover);
            }
            else cover = pictures.First();

            return cover;
        }
        private async static Task<Artist> LoadArtist(TagLib.File file)
        {
            string name = file.Tag.FirstAlbumArtist;
            var artistTask = MusicDataBase.SelectArtist(name);

            Artist artist;
            List<Artist> artists = await artistTask;
            if (artists.Count == 0)
            {
                artist = new Artist
                {
                    Name = name
                };
                artist.Id = MusicDataBase.CreateArtist(artist);
            }
            else artist = artists.First();

            return artist;
        }
        private async static Task<Album> LoadAlbum(TagLib.File file, Artist artist, Beans.Picture cover)
        {
            string title = file.Tag.Album;
            var albumTask = MusicDataBase.SelectAlbum(title);

            Album album;
            var albumResult = await albumTask;
            List<Album> albums =albumResult.FindAll(x => x.Artist.Name == artist.Name);
            if (albums.Count == 0)
            {
                album = new Album
                {
                    Artist = artist,
                    Cover = cover,
                    Tags = file.Tag.Genres,
                    Title = title,
                    Year = (int)file.Tag.Year
                };
                album.Id = MusicDataBase.CreateAlbum(album);
            }
            else album = albums.First();

            return album;
        }

        public static string HashFromFile(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            using(SHA256 h = SHA256.Create())
            {
                return Encoding.UTF8.GetString(h.ComputeHash(data));
            }
        }
        public static void SaveSong(Song song)
        {
            TagLib.File f = TagLib.File.Create(song.Path);

            f.Tag.Album = song.Album.Title;
            f.Tag.AlbumArtists = new string[] { song.Artist.Name };
            f.Tag.Pictures = new TagLib.Picture[] { new TagLib.Picture(new TagLib.ByteVector(song.Cover.Data)) };
            f.Tag.Track = (uint)song.N;
            f.Tag.Title = song.Title;

            f.Save();
        }
    }
}
