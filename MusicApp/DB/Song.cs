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
        const string CREATE_SONG_STAT = "insert into song(title, n, like, heart, artist_id, album_id, path, pic_id) values(%title, %n, %like, %heart, %artist_id, %album_id, %path, %pic_id);";
        const string SELECT_ID_SONG_STAT = "select max(id) from song;";
        const string EXIST_SONG_STAT = "select count(*) from song where title = %title, album_id = %albumid, artist_id = %artistid;";
        const string UPDATE_SONG_STAT = "update song set title = %title, n = %n, like = %like, heart = %heart, artist_id = %artistid, album_id = %albumid, path = %path, pic_id = %picid where id =  %id;";

        public static int CreateSong(Song song, int idPic)
        {
            SqliteCommand command = new SqliteCommand(CREATE_SONG_STAT, connection);

            command.Parameters.Add(new SqliteParameter("title", song.Title));
            command.Parameters.Add(new SqliteParameter("n", song.N));
            command.Parameters.Add(new SqliteParameter("like", song.Like));
            command.Parameters.Add(new SqliteParameter("heart", song.Heart));
            command.Parameters.Add(new SqliteParameter("artist_id", song.Artist.Id));
            command.Parameters.Add(new SqliteParameter("album_id", song.Album.Id));
            command.Parameters.Add(new SqliteParameter("path", song.Path));
            command.Parameters.Add(new SqliteParameter("pic_id", idPic));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_ID_SONG_STAT, connection);

            var reader = command.ExecuteReader();

            reader.Read();

            return reader.GetInt32(0);
        }
        public static bool ExistSong(Song song)
        {
            SqliteCommand command = new SqliteCommand(EXIST_SONG_STAT, connection);

            command.Parameters.Add(new SqliteParameter("title", song.Title));
            command.Parameters.Add(new SqliteParameter("albumid", song.Album.Id));
            command.Parameters.Add(new SqliteParameter("artistid", song.Artist.Id));

            var reader = command.ExecuteReader();

            reader.Read();

            return reader.GetInt32(0) > 0;
        }
        public static void UpdateSong(Song song, int picId)
        {
            SqliteCommand command = new SqliteCommand(UPDATE_SONG_STAT, connection);

            command.Parameters.Add(new SqliteParameter("title", song.Title));
            command.Parameters.Add(new SqliteParameter("n", song.N));
            command.Parameters.Add(new SqliteParameter("like", song.Like));
            command.Parameters.Add(new SqliteParameter("heart", song.Heart));
            command.Parameters.Add(new SqliteParameter("artistid", song.Artist.Id));
            command.Parameters.Add(new SqliteParameter("albumid", song.Album.Id));
            command.Parameters.Add(new SqliteParameter("path", song.Path));
            command.Parameters.Add(new SqliteParameter("picid", picId));
            command.Parameters.Add(new SqliteParameter("id", song.Id));

            command.ExecuteNonQuery();
        }
    }
}
