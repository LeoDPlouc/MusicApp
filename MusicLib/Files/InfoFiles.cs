using MusicLib.Processing;
using MusicLib.Beans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MusicLib.Files
{
    class InfoFiles
    {
        public static string PATH = "Info/";

        public static async Task Save(Song song)
        {
            FileHandler.CheckDirectory(PATH);

            string json = song.Serialize();
            using (var fs = File.Create(PATH + song.AcousticId))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public static Song Load(string AcousticID)
        {
            string json = File.ReadAllText(PATH + AcousticID);
            return Song.Deserialize(json);
        }
    }
}
