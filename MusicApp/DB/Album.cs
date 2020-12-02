using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace MusicApp.DB
{
    partial class MusicDataBase
    {
        const string CREATE_ALBUM_STAT = "insert into album(title, artist_id, tags, pic_id, year) values (@title, @artist_id, @tags, @pic_id, @year);";
        const string UPDATE_ALBUM_STAT = "update album set title = @title, artist_id = @artistid, tags = @tags, pic_id = @picid, year = @picid where id = @id;";
        const string DELETE_ALBUM_STAT = "delete from album where id = @id";
        const string SELECT_ALBUM_LAST_ID_STAT = "select max(id) from album;";

        public async static Task<int> CreateAlbum(Album album)
        {
            SqliteCommand command = new SqliteCommand(CREATE_ALBUM_STAT, connection);

            string t = "";
            foreach (string tag in album.Tags) t += tag + ";";

            command.Parameters.Add(new SqliteParameter("title", album.Title));
            command.Parameters.Add(new SqliteParameter("artist_id", album.Artist.Id));
            command.Parameters.Add(new SqliteParameter("tags", t));
            command.Parameters.Add(new SqliteParameter("pic_id", album.Cover.Id));
            command.Parameters.Add(new SqliteParameter("year", album.Year));

            await command.ExecuteNonQueryAsync();

            command = new SqliteCommand(SELECT_ALBUM_LAST_ID_STAT, connection);

            var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var r = reader.GetInt32(0);
            reader.Close();

            return r;
        }
        public static void UpdateAlbum(Album album)
        {
            string t = "";
            foreach (string tag in album.Tags) t += tag + ";";

            SqliteCommand command = new SqliteCommand(UPDATE_ALBUM_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@title", album.Title));
            command.Parameters.Add(new SqliteParameter("@artistid", album.Artist.Id));
            command.Parameters.Add(new SqliteParameter("@tags", t));
            command.Parameters.Add(new SqliteParameter("@picid", album.Cover.Id));
            command.Parameters.Add(new SqliteParameter("@id", album.Id));

            command.ExecuteNonQueryAsync();
        }
        public async static Task<List<Album>> ListAlbum()
        {
            SqliteCommand command = new SqliteCommand(LIST_ALBUM_STAT, connection);

            List<Album> albums = new List<Album>();
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                albums.Add(await ReaderToAlbum(reader));
            }

            reader.Close();
            return albums;
        }
        public static void DeleteAlbum(int id)
        {
            SqliteCommand command = new SqliteCommand(DELETE_ALBUM_STAT, connection);
            command.Parameters.Add(new SqliteParameter("id", id));

            command.ExecuteNonQueryAsync();
        }

        private async static Task<Album> ReaderToAlbum(SqliteDataReader reader)
        {
            var coverTask = SelectPicture(reader.GetInt32(4));
            var artistTask = SelectArtist(reader.GetInt32(2));

            var artist = await artistTask;
            var cover = await coverTask;

            return new Album
            {
                Artist = artist.First(),
                Cover =  cover.First(),
                Id = reader.GetInt32(0),
                Tags = reader.GetString(3).Split(';'),
                Title = reader.GetString(1),
                Year = reader.GetInt32(5)
            };
        }
    }
}
