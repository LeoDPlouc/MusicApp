using MusicLib.Processing;
using MusicLib.Beans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static MusicLib.Beans.Song;

namespace MusicLib.Files
{
    public class InfoFiles
    {
        public static string PATH = "Info/";

        public static async Task Save(SongInfo songInfo)
        {
            FileHandler.CheckDirectory(PATH);

            string json = songInfo.Serialize();
            using (var fs = File.Create(PATH + songInfo.AcousticId))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public static SongInfo Load(string AcousticID)
        {
            string json = File.ReadAllText(PATH + AcousticID);
            return SongInfo.Deserialize(json);
        }
    }
}
