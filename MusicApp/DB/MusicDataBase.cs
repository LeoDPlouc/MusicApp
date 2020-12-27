using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibVLCSharp.Shared;
using MusicApp.Config;
using MusicApp.Beans;
using MusicApp.Processing;

namespace MusicApp.DB
{
    partial class MusicDataBase
    {
        public const string DB_VERSION = "1.0";

        const string DB_PATH = ".db";
        const string CREATE_SONG_TABLE_STAT = "create table song(path text primary key, like boolean, heart boolean);";

        static SqliteConnection connection;

        public static void Start()
        {
            bool dbExist = File.Exists(DB_PATH);

            connection = new SqliteConnection("Data Source=" + DB_PATH);
            connection.Open();

            if (!dbExist) Initialize();
        }

        static void Initialize()
        {
            SqliteCommand command = new SqliteCommand(CREATE_SONG_TABLE_STAT, connection);
            command.ExecuteNonQuery();

            Configuration.WriteDBVersion();
        }
    }
}
