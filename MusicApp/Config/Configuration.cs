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

        private static string PARAM_SERVER_ENABLED = "PARAM_SERVER_ENABLED";
        private static string PARAM_LIBRARY_PATHS = "PARAM_LIBRARY_PATHS";

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


                if (config.TryGetValue(PARAM_SERVER_ENABLED, out object res))
                    return Boolean.Parse((string)res);
                return false;
            }
            set
            {
                var config = GetConfig();
                config[PARAM_SERVER_ENABLED] = value;
                SaveConfig(config);
            }
        }

        public static string LibraryPath
        {
            get
            {
                var config = GetConfig();

                if (config.TryGetValue(PARAM_LIBRARY_PATHS, out object res))
                    return (string)res;
                return null;
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
