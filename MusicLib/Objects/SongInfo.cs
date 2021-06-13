using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MusicLib.Objects
{
    [Serializable]
    public class SongInfo
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
