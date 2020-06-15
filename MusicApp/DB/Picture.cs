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
        const string CREATE_PICTURE_STAT = "insert into song(data) values(%data);";
        const string SELECT_ID_PICTURE_STAT = "select max(id) from picture;";
        const string EXIST_PICTURE_STAT = "select count(*) from picture where data = %data;";
        const string UPDATE_PICTURE_STAT = "update picture set data = %data where id =  %id;";

        public static int CreatePicture(Picture picture)
        {
            SqliteCommand command = new SqliteCommand(CREATE_PICTURE_STAT, connection);

            command.Parameters.Add(new SqliteParameter("data", picture.Data));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_ID_PICTURE_STAT, connection);

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

            return reader.GetInt32(0) > 0;
        }
        public static void UpdatePicture(Picture picture)
        {
            SqliteCommand command = new SqliteCommand(UPDATE_PICTURE_STAT, connection);

            command.Parameters.Add(new SqliteParameter("data", picture.Data));
            command.Parameters.Add(new SqliteParameter("id", picture.Id));

            command.ExecuteNonQuery();
        }
    }
}
