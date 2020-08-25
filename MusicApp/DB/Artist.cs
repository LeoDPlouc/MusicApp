using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MusicApp.DB
{
    partial class MusicDataBase
    {
        const string CREATE_ARTIST_STAT = "insert into artist(name) values(@name);";
        const string SELECT_LAST_ID_ARTIST_STAT = "select max(id) from artist;";
        const string EXIST_ARTIST_STAT = "select count(*) from artist where name = @name;";
        const string UPDATE_ARTIST_STAT = "update artist set name = %name where id = @id;";
        const string SELECT_ARTIST_ID_STAT = "select * from artist where id = @id;";
        const string SELECT_ARTIST_NAME_STAT = "select * from artist where name = @name;";
        const string SEARCH_ARTIST_NAME_STAT = "select * from artist where name like '%@arg%';";
        const string LIST_ARTIST_STAT = "select * from artist;";

        public static int CreateArtist(Artist artist)
        {
            SqliteCommand command = new SqliteCommand(CREATE_ARTIST_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@name", artist.Name));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_LAST_ID_ARTIST_STAT, connection);

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
        public static List<Artist> SelectArtist(int id)
        {
            SqliteCommand command = new SqliteCommand(SELECT_ARTIST_ID_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@id", id));

            var reader = command.ExecuteReader();

            List<Artist> artists = new List<Artist>();
            while (reader.Read()) artists.Add(SqlReaderToArtist(reader));
            return artists;
        }
        public static List<Artist> SelectArtist(string name)
        {
            SqliteCommand command = new SqliteCommand(SELECT_ARTIST_NAME_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@name", name));

            var reader = command.ExecuteReader();

            List<Artist> artists = new List<Artist>();
            while (reader.Read()) artists.Add(SqlReaderToArtist(reader));
            return artists;
        }

        public static List<Artist> SearchArtist(string arg)
        {
            string cmd = SEARCH_ARTIST_NAME_STAT.Replace("@arg", arg);

            SqliteCommand command = new SqliteCommand(cmd, connection);

            var reader = command.ExecuteReader();

            List<Artist> artists = new List<Artist>();
            while (reader.Read()) artists.Add(SqlReaderToArtist(reader));
            return artists;
        }
        public static List<Artist> ListArtist()
        {
            SqliteCommand command = new SqliteCommand(LIST_ARTIST_STAT, connection);

            var reader = command.ExecuteReader();

            List<Artist> artists = new List<Artist>();
            while (reader.Read()) artists.Add(SqlReaderToArtist(reader));
            return artists;
        }

        private static Artist SqlReaderToArtist(SqliteDataReader reader)
        {
            return new Artist
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }

    }
}
