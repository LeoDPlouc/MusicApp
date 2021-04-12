using MusicLib.Processing;
using System;
using System.Text.Json;

namespace MusicLib.Beans
{
    [Serializable]
    public partial class Song
    {
        public string Title { get; set; }
        public int N { get; set; }
        public double Duration { get; set; }
        public bool Like { get; set; }
        public bool Heart { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Path { get; set; }
        public string AcousticId { get; set; }


        public Byte[] GetCover() => FileHandler.LoadCover(Path);
        public SongInfo GetSongInfo()
        {
            return new SongInfo
            {
                AcousticId = this.AcousticId,
                Like = this.Like,
                Heart = this.Heart
            };
        }

        public string Serialize()
        {
            return JsonSerializer.Serialize<Song>(this);
        }
        public static Song Deserialize(string json)
        {
            return JsonSerializer.Deserialize<Song>(json);
        }

        [Serializable]
        public struct SongInfo
        {
            public bool Like { get; set; }
            public bool Heart { get; set; }
            public string AcousticId { get; set; }
            public string Host { get; set; }
            public string Serialize()
            {
                return JsonSerializer.Serialize<SongInfo>(this);
            }
            public static SongInfo Deserialize(string json)
            {
                return JsonSerializer.Deserialize<SongInfo>(json);
            }
        }
    }
}
