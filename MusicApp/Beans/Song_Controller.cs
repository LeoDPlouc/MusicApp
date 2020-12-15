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
    public partial class Song
    {
        public async static Task<List<Song>> ListAllSongs()
        {
            return await MusicDataBase.ListSongs();
        }
        public async static Task<List<Song>> SearchSongByName(string query)
        {
            var songsTask = ListAllSongs();

            Regex re = new Regex(query);

            var songs = await songsTask;
            return songs.FindAll((Song s) =>
            {
                return re.IsMatch(s.Title);
            });
        }
        public async static Task<bool> ExistSongByPath(string path)
        {
            var songs = await ListAllSongs();
            return songs.Exists((Song s) =>
            {
                return s.Path == path;
            });
        }

        public void Create()
        {
            MusicDataBase.CreateSong(this);
        }
        public void Save()
        {
            MusicDataBase.UpdateSong(this);
            FileHandler.SaveSong(this);
        }

        public void ComputeHash()
        {
            Hash = FileHandler.HashFromFile(Path);
        }
    }
}
