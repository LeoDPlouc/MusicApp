using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MusicApp.Beans;
using MusicApp.Config;
using MusicApp.Processing;

namespace MusicApp.DB
{
    partial class MusicDataBase
    {
        private static void Update()
        {
            string v = Config.Configuration.ReadDBVersion();
            switch(v)
            {
                case "1.0":
                    Update1_2();
                    break;
            }
            Configuration.WriteDBVersion();
        }

        private async static void Update1_2()
        {
            var listSongTask = ListSongs();

            SqliteCommand command = new SqliteCommand("alter table song add column hash string not null default '_';", connection);
            await command.ExecuteNonQueryAsync();

            List<Song> songs = await listSongTask;
            foreach(Song s in songs)
            {
                s.ComputeHash();
                s.Save();
            }
        }
    }
}
