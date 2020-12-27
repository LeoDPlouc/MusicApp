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
        public static List<string> ListAllSongPath()
        {
            return Directory.GetFiles(@"C:\Users\Leo\Desktop\musictest", "*.mp3", SearchOption.AllDirectories).ToList();
        }
        public static Song LoadSong(string path)
        {
            TagLib.File f = TagLib.File.Create(path);

            var songInfo = MusicDataBase.SelectSong(path);

            return new Song
            {
                Album = f.Tag.Album,
                Artist = f.Tag.FirstAlbumArtist,
                Duration = f.Length,
                Heart = songInfo == null ? false : songInfo.Item3,
                Like = songInfo == null ? false : songInfo.Item2,
                N = (int)f.Tag.Track,
                Path = path,
                Title = f.Tag.Title
            };
        }
        public static void SaveSong(Song song)
        {
            TagLib.File f = TagLib.File.Create(song.Path);

            f.Tag.Album = song.Title;
            f.Tag.AlbumArtists = new string[] { song.Artist };
            f.Tag.Pictures = new TagLib.Picture[] { new TagLib.Picture(new TagLib.ByteVector(song.Cover.Data)) };
            f.Tag.Track = (uint)song.N;
            f.Tag.Title = song.Title;

            f.Save();
        }

        public static Picture LoadCover(string path)
        {
            TagLib.File f = TagLib.File.Create(path);
            return new Picture() { Data = f.Tag.Pictures.First().Data.Data };
        }
    }
}
