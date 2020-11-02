using MusicApp.Beans;
using MusicApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Processing
{
    class FileHandler
    {
        public static Song LoadSong(string path)
        {
            TagLib.File f = TagLib.File.Create(path);

            Artist artist = LoadArtist(f);
            Beans.Picture cover = LoadCover(f);
            Album album = LoadAlbum(f, artist, cover);

            return new Song
            {
                Album = album,
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
        private static Beans.Picture LoadCover(TagLib.File file)
        {
            byte[] data = file.Tag.Pictures.First().Data.Data;

            Beans.Picture cover;
            List<Beans.Picture> pictures = MusicDataBase.SelectPicture(data);
            if (pictures.Count == 0)
            {
                cover = new Beans.Picture { Data = data };
                cover.Id = MusicDataBase.CreatePicture(cover);
            }
            else cover = pictures.First();

            return cover;
        }
        private static Artist LoadArtist(TagLib.File file)
        {
            string name = file.Tag.FirstAlbumArtist;

            Artist artist;
            List<Artist> artists = MusicDataBase.SelectArtist(name);
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
        private static Album LoadAlbum(TagLib.File file, Artist artist, Beans.Picture cover)
        {
            string title = file.Tag.Album;

            Album album;
            List<Album> albums = MusicDataBase.SelectAlbum(title).FindAll(x => x.Artist.Name == artist.Name);
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
