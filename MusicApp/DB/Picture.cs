using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using TagLib.Riff;

namespace MusicApp.DB
{
    partial class MusicDataBase
    {
        const string CREATE_PICTURE_STAT = "insert into picture(data) values(@data);";
        const string SELECT_LAST_ID_PICTURE_STAT = "select max(id) from picture;";
        const string UPDATE_PICTURE_STAT = "update picture set data = @data where id =  @id;";
        const string LIST_PICTURE_STAT = "select * from picture;";
        const string DELETE_PICTURE_STAT = "delete from picture where id = @id;";

        public static int CreatePicture(Picture picture)
        {
            SqliteCommand command = new SqliteCommand(CREATE_PICTURE_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@data", picture.Data));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_LAST_ID_PICTURE_STAT, connection);

            var reader = command.ExecuteReader();

            reader.Read();

            return reader.GetInt32(0);
        }
        public static void UpdatePicture(Picture picture)
        {
            SqliteCommand command = new SqliteCommand(UPDATE_PICTURE_STAT, connection);

            command.Parameters.Add(new SqliteParameter("data", picture.Data));
            command.Parameters.Add(new SqliteParameter("id", picture.Id));

            command.ExecuteNonQuery();
        }
        public static void DeletePicture(int id)
        {
            SqliteCommand command = new SqliteCommand(DELETE_PICTURE_STAT, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));

            command.ExecuteNonQuery();
        }
        public static async Task<List<Picture>> ListIdPicture()
        {
            List<Picture> res = new List<Picture>();

            SqliteCommand command = new SqliteCommand(LIST_PICTURE_STAT, connection);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) res.Add(ReaderToPicture(reader);
            reader.Close();

            return res;
        }

        private static Picture ReaderToPicture(SqliteDataReader reader)
        {
            return new Picture
            {
                Id = reader.GetInt32(0),
                Data = (byte[])reader.GetValue(1)
            };
        }
    }
}
