using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using MusicApp.Config;
using System.Net.Http;

namespace MusicApp.MusicServerController
{
    class MusicServer
    {
        public static async void SaveSong(Song song)
        {
            SongInfo info = new SongInfo()
            {
                Title = song.Title,
                Album = song.Album,
                Artist = song.Artist,
                Like = song.Like,
                Heart = song.Heart,
                Path = song.Path,
                Host = Configuration.CONFIG_HOST,
                AcousticId = song.AcousticId
            };
            string json = SongInfoToJson(info);

            HttpClient client = new HttpClient();
            var content = new StringContent(json);
            await client.PostAsync("http://127.0.0.1:8000/song/", content);
        }

        public static async Task<SongInfo> GetSongInfo(string acousticId)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync("http://127.0.0.1:8000/song/?acousticId=" + acousticId);
            if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new SongInfo()
                {
                    Heart = false,
                    Like = false
                };
            string json = await res.Content.ReadAsStringAsync();
            return JsonToSongInfo(json);
        }

        [Serializable]
        public struct SongInfo
        {
            public string Title { get; set; }
            public string Album { get; set; }
            public string Artist { get; set; }
            public bool Like { get; set; }
            public bool Heart { get; set; }
            public string Path { get; set; }
            public string Host { get; set; }
            public string AcousticId { get; set; }
        }

        private static SongInfo JsonToSongInfo(string json)
        {
            return (SongInfo)JsonSerializer.Deserialize(json, typeof(SongInfo));
        }
        private static string SongInfoToJson(SongInfo SongInfo)
        {
            return JsonSerializer.Serialize(SongInfo, typeof(SongInfo));
        }
    }
}
