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
        public const string DB_VERSION = "1.1";

        const string DB_PATH = "db.db3";
        const string CREATE_ARTIST_TABLE_STAT = "create table artist(id integer primary key autoincrement, name string);";
        const string CREATE_PICTURE_TABLE_STAT = "create table picture(id integer primary key autoincrement, data blob);";
        const string CREATE_ALBUM_TABLE_STAT = "create table album(id integer primary key autoincrement, title text, artist_id integer references artist(id), tags text, pic_id integer references picture(id), year integer);";
        const string CREATE_SONG_TABLE_STAT ="create table song(id integer primary key autoincrement, title text, n integer, like boolean, heart boolean, artist_id integer references artist(id), album_id references album(id), path text, pic_id integer references picture(id), hash string);";

        static SqliteConnection connection;

        public static void Start()
        {
            bool dbExist = File.Exists(DB_PATH);

            connection = new SqliteConnection("Data Source=" + DB_PATH);
            connection.Open();

            if (!dbExist) Initialize();
            Update();
            UpdateContent();
        }
        public async static void UpdateContent()
        {
            foreach(Song s in await ListSongs())
            {
                var loadSongTask = FileHandler.LoadSong(s.Path);
                if (s.Hash != FileHandler.HashFromFile(s.Path))
                {
                    Song song = await loadSongTask;

                    song.Id = s.Id;
                    song.ComputeHash();
                    song.Save();
                }
                await Task.Delay(1);
            }
        }
        
        static void Initialize()
        {
            SqliteCommand command = new SqliteCommand(CREATE_ARTIST_TABLE_STAT, connection);
            command.ExecuteNonQuery();

            command = new SqliteCommand(CREATE_PICTURE_TABLE_STAT, connection);
            command.ExecuteNonQuery();

            command = new SqliteCommand(CREATE_ALBUM_TABLE_STAT, connection);
            command.ExecuteNonQuery();

            command = new SqliteCommand(CREATE_SONG_TABLE_STAT, connection);
            command.ExecuteNonQuery();

            Configuration.WriteDBVersion();
        }
    }
}
