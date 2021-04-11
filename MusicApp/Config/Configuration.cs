using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;

namespace MusicApp.Config
{
    class Configuration
    {
        private static string CONFIG = ".config";

        public static string PARAM_SERVER_ENABLED = "PARAM_SERVER_ENABLED";
        public static string PARAM_LIBRARY_PATHS = "PARAM_LIBRARY_PATHS";

        private static Dictionary<string, object> config;

        private static void SaveConfig(Dictionary<string, object> config)
        {
            Serializer serializer = new Serializer();
            string yaml = serializer.Serialize(config);
            File.WriteAllText(CONFIG, yaml);
        }

        private static Dictionary<string, object> GetConfig()
        {
            if (File.Exists(CONFIG))
            {
                Deserializer deserializer = new Deserializer();
                string yaml = File.ReadAllText(CONFIG);
                return (Dictionary<string, object>)deserializer.Deserialize(yaml, typeof(Dictionary<string, object>));
            }
            return new Dictionary<string, object>();
        }

        public static bool ServerEnabled
        {
            get
            {
                var config = GetConfig();
                var res = config[PARAM_SERVER_ENABLED];

                if (res is null)
                    return false;
                return (bool)res;
            }
            set
            {
                var config = GetConfig();
                config[PARAM_SERVER_ENABLED] = value;
                SaveConfig(config);
            }
        }

        public static string[] LibraryPaths
        {
            get
            {
                var config = GetConfig();
                var res = config[PARAM_LIBRARY_PATHS];

                if (res is null)
                    return new string[] { System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) };
                return (string[])res;
            }
            set
            {
                var config = GetConfig();
                config[PARAM_LIBRARY_PATHS] = value;
                SaveConfig(config);
            }
        }
    }
}
