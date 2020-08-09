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
        const string CREATE_ALBUM_STAT = "insert into album(title, artist_id, tags, pic_id, year) values (@title, @artist_id, @tags, @pic_id, @year);";
        const string SELECT_LAST_ID_ALBUM_STAT = "select max(id) from album;";
        const string EXIST_ALBUM_STAT = "select count(*) from album where title = @title, artist_id = @artistid;";
        const string UPDATE_ALBUM_STAT = "update abum set title = @title, artist_id = @artistid, tags = @tags, pic_id = @picid, year = @picid where id = @id;";
        const string SELECT_ALBUM_TITLE_STAT = "select * from album where title=@title;";
        const string SELECT_ALBUM_ID_STAT = "select * from album where id=@id;";
        const string SEARCH_ALBUM_TITLE_STAT = "select * from album where title like @arg;";
        const string LIST_ALBUM_STAT = "select * from album;";

        public static int CreateAlbum(Album album)
        {
            SqliteCommand command = new SqliteCommand(CREATE_ALBUM_STAT, connection);

            string t = "";
            foreach (string tag in album.Tags) t += tag + ";";

            command.Parameters.Add(new SqliteParameter("title", album.Title));
            command.Parameters.Add(new SqliteParameter("artist_id", album.Artist.Id));
            command.Parameters.Add(new SqliteParameter("tags", t));
            command.Parameters.Add(new SqliteParameter("pic_id", album.Cover.Id));
            command.Parameters.Add(new SqliteParameter("year", album.Year));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_LAST_ID_ALBUM_STAT, connection);

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

            command.Parameters.Add(new SqliteParameter("@title", album.Title));
            command.Parameters.Add(new SqliteParameter("@artistid", album.Artist.Id));
            command.Parameters.Add(new SqliteParameter("@tags", t));
            command.Parameters.Add(new SqliteParameter("@picid", picId));
            command.Parameters.Add(new SqliteParameter("@id", album.Id));

            command.ExecuteNonQuery();
        }
        public static List<Album> SelectAlbum(string title)
        {
            SqliteCommand command = new SqliteCommand(SELECT_ALBUM_TITLE_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@title", title));

            var reader = command.ExecuteReader();
            List<Album> albums = new List<Album>();

            while (reader.Read())
            {
                albums.Add(new Album
                {
                    Artist = SelectArtist(reader.GetInt32(2)).First(),
                    Cover = SelectPicture(reader.GetInt32(4)).First(),
                    Id = reader.GetInt32(0),
                    Tags = reader.GetString(3).Split(';'),
                    Title = reader.GetString(1),
                    Year = reader.GetInt32(5)
                });
            }

            return albums;
        }
        public static List<Album> SelectAlbum(int id)
        {
            SqliteCommand command = new SqliteCommand(SELECT_ALBUM_ID_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@id", id));

            var reader = command.ExecuteReader();
            List<Album> albums = new List<Album>();

            while (reader.Read())
            {
                albums.Add(ReaderToAlbum(reader));
            }

            return albums;
        }

        public static List<Album> SearchAlbumTitle(string arg)
        {
            SqliteCommand command = new SqliteCommand(SEARCH_ALBUM_TITLE_STAT, connection);

            command.Parameters.Add(new SqliteParameter("arg", arg));

            List<Album> albums = new List<Album>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                albums.Add(ReaderToAlbum(reader));
            }

            reader.Close();
            return albums;
        }
        public static List<Album> ListAlbum()
        {
            SqliteCommand command = new SqliteCommand(LIST_ALBUM_STAT, connection);

            List<Album> albums = new List<Album>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                albums.Add(ReaderToAlbum(reader));
            }

            reader.Close();
            return albums;
        }

        private static Album ReaderToAlbum(SqliteDataReader reader)
        { 
            return new Album
            {
                Artist = SelectArtist(reader.GetInt32(2)).First(),
                Cover = SelectPicture(reader.GetInt32(4)).First(),
                Id = reader.GetInt32(0),
                Tags = reader.GetString(3).Split(';'),
                Title = reader.GetString(1),
                Year = reader.GetInt32(5)
            };
        }
    }
}
