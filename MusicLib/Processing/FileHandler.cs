using MusicLib.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using AcoustID;
using static MusicLib.Beans.Song;
using MusicLib.Files;

namespace MusicLib.Processing
{
    public class FileHandler
    {
        public static List<string> ListAllSongPath()
        {
            return Directory.GetFiles(@"C:\Users\Leo\Desktop\musictest", "*.mp3", SearchOption.AllDirectories).ToList();
        }
        public static Song LoadSong(string path, bool serverEnabled)
        {
            TagLib.File f = TagLib.File.Create(path);

            string acousticId = GetAcousticId(path);

            return new Song
            {
                Album = f.Tag.Album,
                Artist = f.Tag.FirstAlbumArtist,
                Duration = f.Length,
                N = (int)f.Tag.Track,
                Path = path,
                Title = f.Tag.Title,
                AcousticId = acousticId,
            };
        }
        public static void LoadSong(string path, bool serverEnabled, Song song)
        {
            TagLib.File f = TagLib.File.Create(path);

            string acousticId = GetAcousticId(path);

            song.Album = f.Tag.Album;
            song.Artist = f.Tag.FirstAlbumArtist;
            song.Duration = f.Length;
            song.N = (int)f.Tag.Track;
            song.Path = path;
            song.Title = f.Tag.Title;
            song.AcousticId = acousticId;
        }
        public static void SaveSong(Song song)
        {
            TagLib.File f = TagLib.File.Create(song.Path);

            f.Tag.Album = song.Title;
            f.Tag.AlbumArtists = new string[] { song.Artist };
            f.Tag.Pictures = new TagLib.Picture[] { new TagLib.Picture(new TagLib.ByteVector(song.GetCover())) };
            f.Tag.Track = (uint)song.N;
            f.Tag.Title = song.Title;

            f.Save();
        }

        public static byte[] LoadCover(string path)
        {
            TagLib.File f = TagLib.File.Create(path);
            return f.Tag.Pictures.First().Data.Data;
        }
        public static bool CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }
            return true;
        }
        public static bool CheckFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                return false;
            }
            return true;
        }
        public static string GetAcousticId(string path)
        {
            string acousticId = AcousticIdFiles.Get(path);

            if (acousticId is null)
            {
                acousticId = ComputeAcousticId(path);
                AcousticIdFiles.Set(path, acousticId);
            }
            return acousticId;
        }

        private static string ComputeAcousticId(string path)
        {
            NAudio.Wave.AudioFileReader reader = new NAudio.Wave.AudioFileReader(path);

            byte[] buffer = new byte[reader.Length];
            reader.Read(buffer, 0, buffer.Length);
            short[] data = buffer.Select<byte, short>((byte b) =>
            {
                return Convert.ToInt16(b);
            }).ToArray();

            ChromaContext context = new ChromaContext();
            context.Start(reader.WaveFormat.SampleRate, reader.WaveFormat.Channels);
            context.Feed(data, data.Length);
            context.Finish();

            return context.GetFingerprint();
        }
    }
}
