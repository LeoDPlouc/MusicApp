using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MusicApp.DB
{
    partial class Music_DataBase
    {
        const string CREATE_ARTIST_STAT = "insert into artist(name) values(%name);";
        const string SELECT_ID_ARTIST_STAT = "select max(id) from artist;";
        const string EXIST_ARTIST_STAT = "select count(*) from artist where name = %name;";
        const string UPDATE_ARTIST_STAT = "update artist set name = %name where id = %id;";

        public static int CreateArtist(Artist artist)
        {
            SqliteCommand command = new SqliteCommand(CREATE_ARTIST_STAT, connection);

            command.Parameters.Add(new SqliteParameter("name", artist.Name));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_ID_ARTIST_STAT, connection);

            var reader = command.ExecuteReader();

            reader.Read();

            return reader.GetInt32(0);
        }
        public static bool ExistArtist(Artist artist)
        {
            SqliteCommand command = new SqliteCommand(EXIST_ARTIST_STAT, connection);

            command.Parameters.Add(new SqliteParameter("name", artist.Name));

            var reader = command.ExecuteReader();

            reader.Read();

            return reader.GetInt32(0) > 0;
        }
        public static void UpdateArtist(Artist artist)
        {
            SqliteCommand command = new SqliteCommand(UPDATE_ARTIST_STAT, connection);

            command.Parameters.Add(new SqliteParameter("title", artist.Name));
            command.Parameters.Add(new SqliteParameter("id", artist.Id));

            command.ExecuteNonQuery();
        }
    }
}
