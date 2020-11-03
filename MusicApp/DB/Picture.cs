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
        const string EXIST_PICTURE_STAT = "select count(*) from picture where data = @data;";
        const string UPDATE_PICTURE_STAT = "update picture set data = @data where id =  @id;";
        const string SELECT_PICTURE_ID_STAT = "select * from picture where id = @id;";
        const string SELECT_PICTURE_DATA_STAT = "select * from picture where data = @data;";
        const string LIST_PICTURE_ID_STAT = "select id from picture;";
        const string DELETE_PICTURE_STAT = "delete frome picture where id =@id;";

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
        public static bool ExistPicture(Picture picture)
        {
            SqliteCommand command = new SqliteCommand(EXIST_PICTURE_STAT, connection);

            command.Parameters.Add(new SqliteParameter("data", picture.Data));

            var reader = command.ExecuteReader();

            reader.Read();
            var r = reader.GetInt32(0) > 0;
            reader.Close();

            return r;
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
        public async static Task<List<Picture>> SelectPicture(int id)
        {
            SqliteCommand command = new SqliteCommand(SELECT_PICTURE_ID_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@id", id));

            var reader = command.ExecuteReader();
            List<Picture> pictures = new List<Picture>();

            while(reader.Read())
            {
                pictures.Add(ReaderToPicture(reader));
                await Task.Delay(1);
            }

            return pictures;
        }
        public async static Task<List<Picture>> SelectPicture(byte[] data)
        {
            SqliteCommand command = new SqliteCommand(SELECT_PICTURE_DATA_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@data", data));

            List<Picture> pictures = new List<Picture>();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pictures.Add(ReaderToPicture(reader));
                await Task.Delay(1);
            }

            return pictures;
        }
        public static List<int> ListIDPicture()
        {
            List<int> res = new List<int>();

            SqliteCommand command = new SqliteCommand(LIST_PICTURE_ID_STAT, connection);
            var reader = command.ExecuteReader();
            while (reader.Read()) res.Add(reader.GetInt32(0));
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
