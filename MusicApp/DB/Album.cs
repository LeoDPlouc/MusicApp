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
        const string CREATE_ALBUM_STAT = "insert into album(title, artist_id, tags, pic_id, year) values (%title, %artist_id, %tags, %pic_id, %year);";
        const string SELECT_ID_ALBUM_STAT = "select max(id) from album;";
        const string EXIST_ALBUM_STAT = "select count(*) from album where title = %title, artist_id = %artistid;";
        const string UPDATE_ALBUM_STAT = "update abum set title = %title, artist_id = %artistid, tags = %tags, pic_id = %picid, year = %picid where id = %id;";

        public static int CreateAlbum(Album album, int idPic)
        {
            SqliteCommand command = new SqliteCommand(CREATE_ALBUM_STAT, connection);

            string t = "";
            foreach (string tag in album.Tags) t += tag + ";";

            command.Parameters.Add(new SqliteParameter("title", album.Title));
            command.Parameters.Add(new SqliteParameter("artist_id", album.Artist.Id));
            command.Parameters.Add(new SqliteParameter("tags", t));
            command.Parameters.Add(new SqliteParameter("pic_id", idPic));
            command.Parameters.Add(new SqliteParameter("year", album.Year));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_ID_ALBUM_STAT, connection);

            var reader = command.ExecuteReader();

            reader.Read();

            return reader.GetInt32(0);
        }
        public static bool ExistAlbum(Album album)
        {
            SqliteCommand command = new SqliteCommand(EXIST_ALBUM_STAT, connection);

            command.Parameters.Add(new SqliteParameter("title", album.Title));
            command.Parameters.Add(new SqliteParameter("artistid", album.Artist.Id));

            var reader = command.ExecuteReader();

            reader.Read();

            return reader.GetInt32(0) > 0;
        }
        public static void UpdateAlbum(Album album, int picId)
        {
            string t = "";
            foreach (string tag in album.Tags) t += tag + ";";

            SqliteCommand command = new SqliteCommand(UPDATE_ALBUM_STAT, connection);

            command.Parameters.Add(new SqliteParameter("title", album.Title));
            command.Parameters.Add(new SqliteParameter("artistid", album.Artist.Id));
            command.Parameters.Add(new SqliteParameter("tags", t));
            command.Parameters.Add(new SqliteParameter("picid", picId));
            command.Parameters.Add(new SqliteParameter("id", album.Id));

            command.ExecuteNonQuery();
        }
    }
}
