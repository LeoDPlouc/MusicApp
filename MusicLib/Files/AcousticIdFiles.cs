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
    public class AcousticIdFiles
    {
        public static string PATH = ".acousticid";

        private static Dictionary<string, string> acousticIdHash;

        public static void Set(string path, string acousticId)
        {
            Load();
            acousticIdHash[path] = acousticId;
            Save();
        }
        public static string Get(string path)
        {
            Load();
            if (acousticIdHash.TryGetValue(path, out string acousticId))
                return acousticId;
            return null;
        }

        private static void Save()
        {
            string json = JsonSerializer.Serialize(acousticIdHash);
            using (var fs = File.Create(PATH))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        private static void Load()
        {
            if (acousticIdHash != null)
                return;
            if (FileHandler.CheckFile(PATH))
            {
                string json = File.ReadAllText(PATH);
                acousticIdHash = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            else
                acousticIdHash = new Dictionary<string, string>();
        }


    }
}
