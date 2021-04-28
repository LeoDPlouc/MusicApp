using MusicLib.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AcoustID;
using System.Text;
using TagLib;
using TagLib.Id3v2;
using File = System.IO.File;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using static MusicLib.Beans.Song;
using MusicLib.Server;
using MusicLib.Files;

namespace MusicLib.Processing
{
    public class FileHandler
    {
        private const string ACOUSTICID_TAG = "AcousticId";

        public static List<string> ListAllSongPath(string path)
        {
            return Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories).ToList();
        }
        public static async Task<Song> LoadSong(string path, bool serverEnabled)
        {
            TagLib.File f = TagLib.File.Create(path);

            string acousticId = GetCustomTag(f, ACOUSTICID_TAG);
            if (acousticId is null)
                acousticId = ComputeAcousticId(path);

            SongInfo songInfo;
            if (serverEnabled)
                songInfo = await Client.GetSongInfo(acousticId, "127.0.0.1");
            else
                songInfo = InfoFiles.Load(acousticId);


            return new Song
            {
                Album = f.Tag.Album,
                Artist = f.Tag.FirstAlbumArtist,
                Duration = f.Properties.Duration.TotalMinutes,
                N = (int)f.Tag.Track,
                Path = path,
                Title = f.Tag.Title,
                AcousticId = acousticId,
                Heart = songInfo.Heart,
                Like = songInfo.Like
            };
        }
        public static async Task LoadSong(string path, bool serverEnabled, Song song)
        {
            Song s = await LoadSong(path, serverEnabled);

            song.Album = s.Album;
            song.Artist = s.Artist;
            song.Duration = s.Duration;
            song.N = s.N;
            song.Path = s.Path;
            song.Title = s.Title;
            song.AcousticId = s.AcousticId;
            song.Heart = s.Heart;
            song.Like = s.Like;
        }
        public static void SaveSong(Song song)
        {
            TagLib.File f = TagLib.File.Create(song.Path);

            f.Tag.Album = song.Title;
            f.Tag.AlbumArtists = new string[] { song.Artist };
            f.Tag.Pictures = new Picture[] { new Picture(new ByteVector(song.GetCover())) };
            f.Tag.Track = (uint)song.N;
            f.Tag.Title = song.Title;

            SetCustomTag(f, ACOUSTICID_TAG, song.AcousticId);

            while(true)
            {
                try
                {
                    f.Save();
                    break;
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
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

        private static string GetCustomTag(TagLib.File file, string tagName)
        {
            var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
            PrivateFrame frame = PrivateFrame.Get(tag, tagName, false);
            if (frame is null)
                return null;
            return Encoding.Unicode.GetString(frame.PrivateData.Data);
        }
        private static void SetCustomTag(TagLib.File file, string tagName, string value)
        {
            var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
            PrivateFrame frame = PrivateFrame.Get(tag, tagName, true);
            frame.PrivateData = Encoding.Unicode.GetBytes(value);
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
