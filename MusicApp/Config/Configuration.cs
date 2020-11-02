using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MusicApp.DB;

namespace MusicApp.Config
{
    class Configuration
    {
        public const string CONFIG_VERSION = "1.0";

        const string CONFIG_FILE = "config";

        const string FLD_CONFIG = "config";
        const string FLD_CONFIG_VERSION = "version";
        const string FLD_DATABASE = "database";
        const string FLD_DATABASE_VERSION = "version";

        public static void Init()
        {
            if(!File.Exists(CONFIG_FILE))
            {

                JObject json =
                    new JObject(
                        new JProperty(FLD_CONFIG,
                            new JObject(
                                new JProperty(FLD_CONFIG_VERSION))),
                        new JProperty(FLD_DATABASE,
                            new JObject(
                                new JProperty(FLD_DATABASE_VERSION))));

                SaveConfig(json);
                WriteDBVersion();
                WriteConfigVersion();
            }

        }
        public static string ReadConfigVersion()
        {
            JObject config = GetConfig();
            return (string)config[FLD_CONFIG][FLD_CONFIG_VERSION];
        }

        public static void WriteConfigVersion()
        {
            JObject config = GetConfig();
            config[FLD_CONFIG][FLD_CONFIG_VERSION] = CONFIG_VERSION;
            SaveConfig(config);
        }
        public static void WriteConfigVersion(string version)
        {
            JObject config = GetConfig();
            config[FLD_CONFIG][FLD_CONFIG_VERSION] = version;
            SaveConfig(config);
        }

        public static string ReadDBVersion()
        {
            JObject config = GetConfig();
            return (string) config[FLD_DATABASE][FLD_DATABASE_VERSION];
        }

        public static void WriteDBVersion()
        {
            JObject config = GetConfig();
            config[FLD_DATABASE][FLD_DATABASE_VERSION] = MusicDataBase.DB_VERSION;
            SaveConfig(config);
        }
        public static void WriteDBVersion(string version)
        {
            JObject config = GetConfig();
            config[FLD_DATABASE][FLD_DATABASE_VERSION] = version;
            SaveConfig(config);
        }

        private static JObject GetConfig()
        {
            string json = File.ReadAllText(CONFIG_FILE);
            return JObject.Parse(json);
        }

        private static void SaveConfig(JObject json)
        {
            StreamWriter writer = File.CreateText(CONFIG_FILE);
            writer.Write(json.ToString());
            writer.Flush();
            writer.Close();
        }
        private static JsonTextReader GetReader()
        {
            return new JsonTextReader(new StreamReader(CONFIG_FILE));
        }
    }
}
