﻿using MusicLib.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using AcoustID;
using static MusicLib.Beans.Song;

namespace MusicLib.Processing
{
    public class FileHandler
    {
        public static List<string> ListAllSongPath()
        {
            return Directory.GetFiles(@"C:\Users\Leo\Desktop\musictest", "*.mp3", SearchOption.AllDirectories).ToList();
        }
        public static async Task<Song> LoadSong(string path)
        {
            TagLib.File f = TagLib.File.Create(path);

            string acousticIdHash = GetAcousticId(path, out string acousticId);
            var songInfo = await Server.Client.GetSongInfo(acousticId, "127.0.0.1");

            return new Song
            {
                Album = f.Tag.Album,
                Artist = f.Tag.FirstAlbumArtist,
                Duration = f.Length,
                Heart = songInfo.Heart,
                Like = songInfo.Like,
                N = (int)f.Tag.Track,
                Path = path,
                Title = f.Tag.Title,
                AcousticId = acousticId,
                AcousticIdHash = acousticIdHash,
            };
        }
        public static void SaveSong(Song song)
        {
            TagLib.File f = TagLib.File.Create(song.Path);

            f.Tag.Album = song.Title;
            f.Tag.AlbumArtists = new string[] { song.Artist };
            f.Tag.Pictures = new TagLib.Picture[] { new TagLib.Picture(new TagLib.ByteVector(song.GetCover)) };
            f.Tag.Track = (uint)song.N;
            f.Tag.Title = song.Title;

            f.Save();
        }

        public static byte[] LoadCover(string path)
        {
            TagLib.File f = TagLib.File.Create(path);
            return f.Tag.Pictures.First().Data.Data;
        }
        public static void CheckDirectory(string path)
        {
            if (!File.Exists(path))
                Directory.CreateDirectory(path);
        }
        public static string GetAcousticId(string path, out string fingerprint)
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

            fingerprint = context.GetFingerprint();
            return context.GetFingerprintHash().ToString();
        }
    }
}