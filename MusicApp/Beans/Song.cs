﻿using MusicApp.Config;
using MusicLib.Files;
using MusicLib.Processing;
using MusicLib.Server;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    public partial class Song : MusicLib.Beans.Song
    {

        public static List<Song> Songs { get; set; }
        public static void CollectSongs()
        {
            if (Songs == null) Songs = new List<Song>();

            Songs.Clear();
            foreach (string path in FileHandler.ListAllSongPath())
            {
                Song song = new Song();
                FileHandler.LoadSong(path, Configuration.ServerEnabled, song);
                Songs.Add(song as Song);
            }

            Beans.Album.FetchAlbums();
        }

        public static List<Song> SearchByTitle(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Title);
            });
        }
        public static List<Song> SearchByAlbum(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Album);
            });
        }
        public static List<Song> SearchByArtist(string arg)
        {
            Regex pattern = new Regex(arg);

            return Songs.FindAll((Song s) =>
            {
                return pattern.IsMatch(s.Artist);
            });
        }
        public async Task Save()
        {
            FileHandler.SaveSong(this);

            if (Configuration.ServerEnabled)
                await Client.SendSongInfo(GetSongInfo(), "127.0.0.1");
            else
                await InfoFiles.Save(GetSongInfo());
        }

        public async Task Load()
        {
            SongInfo songInfo;
            if (Configuration.ServerEnabled)
                songInfo = await Client.GetSongInfo(AcousticId, "127.0.0.1");
            else
                songInfo = InfoFiles.Load(AcousticId);

            Heart = songInfo.Heart;
            Like = songInfo.Like;

        }
    }
}
