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
        const string UPDATE_ARTIST_STAT = "update artist set name = @name where id = @id;";
        const string DELETE_ARTIST_STAT = "delete from artist where id = @id;";
        const string LIST_ARTIST_STAT = "select * from artist;";
        const string SELECT_LAST_ID_ARTIST_STAT = "select max(id) from artist;";

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
        public static void UpdateArtist(Artist artist)
        {
            SqliteCommand command = new SqliteCommand(UPDATE_ARTIST_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@name", artist.Name));
            command.Parameters.Add(new SqliteParameter("@id", artist.Id));

            command.ExecuteNonQuery();
        }
        public async static Task<List<Artist>> ListArtist()
        {
            SqliteCommand command = new SqliteCommand(LIST_ARTIST_STAT, connection);

            var reader = command.ExecuteReader();

            List<Artist> artists = new List<Artist>();
            while (reader.Read())
            {
                artists.Add(SqlReaderToArtist(reader));
                await Task.Delay(1);
            }

            reader.Close();
            return artists;
        }
        public static void DeleteArtist(int id)
        {
            SqliteCommand command = new SqliteCommand(DELETE_ARTIST_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@id", id));

            command.ExecuteNonQueryAsync();
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
