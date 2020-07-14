using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MusicApp.Beans;
using MusicApp.DB;
using TagLib;

namespace MusicApp.Processing
{
    class SongCollector
    {
        public static void Collect()
        {
            string path = @"";

            foreach (string s in Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories))
            {
                if(!Music_DataBase.ExistSong(s))
                {
                    Song song = LoadSong(s);
                    Music_DataBase.CreateSong(song);
                }
            }
        }
        
        private static Song LoadSong(string path)
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
            List<Beans.Picture> pictures = Music_DataBase.SelectPicture(data);
            if (pictures.Count == 0)
            {
                cover = new Beans.Picture { Data = data };
                cover.Id = Music_DataBase.CreatePicture(cover);
            }
            else cover = pictures.First();

            return cover;
        }
        private static Artist LoadArtist(TagLib.File file)
        {
            string name = file.Tag.FirstAlbumArtist;

            Artist artist;
            List<Artist> artists = Music_DataBase.SelectArtist(name);
            if (artists.Count == 0)
            {
                artist = new Artist
                {
                    Name = name
                };
                artist.Id = Music_DataBase.CreateArtist(artist);
            }
            else artist = artists.First();

            return artist;
        }
        private static Album LoadAlbum(TagLib.File file, Artist artist, Beans.Picture cover)
        {
            string title = file.Tag.Album;

            Album album;
            List<Album> albums = Music_DataBase.SelectAlbum(title).FindAll(x => x.Artist.Name == artist.Name);
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
                album.Id = Music_DataBase.CreateAlbum(album);
            }
            else album = albums.First();

            return album;
        }
    }
}
