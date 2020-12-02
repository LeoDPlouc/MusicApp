using MusicApp.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Xamarin.Forms;
using System.Windows.Forms;

namespace MusicApp.DB
{
    partial class MusicDataBase
    {
        const string CREATE_SONG_STAT = "insert into song(title, n, like, heart, artist_id, album_id, path, pic_id, hash) values(@title, @n, @like, @heart, @artist_id, @album_id, @path, @pic_id, @hash);";
        const string SELECT_LAST_ID_SONG_STAT = "select max(id) from song;";
        const string UPDATE_SONG_STAT = "update song set title = @title, n = @n, like = @like, heart = @heart, artist_id = @artistid, album_id = @albumid, path = @path, pic_id = @picid, hash = @hash where id =  @id;";
        const string LIST_SONG_STAT = "select * from song;";
        const string DELETE_SONG_STAT = "delete from song where id = @id;";
        public static int CreateSong(Song song)
        {
            song.ComputeHash();

            SqliteCommand command = new SqliteCommand(CREATE_SONG_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@title", song.Title));
            command.Parameters.Add(new SqliteParameter("@n", song.N));
            command.Parameters.Add(new SqliteParameter("@like", song.Like));
            command.Parameters.Add(new SqliteParameter("@heart", song.Heart));
            command.Parameters.Add(new SqliteParameter("@artist_id", song.Artist.Id));
            command.Parameters.Add(new SqliteParameter("@album_id", song.Album.Id));
            command.Parameters.Add(new SqliteParameter("@path", song.Path));
            command.Parameters.Add(new SqliteParameter("@pic_id", song.Cover.Id));
            command.Parameters.Add(new SqliteParameter("@hash", song.Hash));

            command.ExecuteNonQuery();

            command = new SqliteCommand(SELECT_LAST_ID_SONG_STAT, connection);

            var reader = command.ExecuteReader();

            reader.Read();
            var r = reader.GetInt32(0);
            reader.Close();

            return r;
        }
        public static void UpdateSong(Song song)
        {
            SqliteCommand command = new SqliteCommand(UPDATE_SONG_STAT, connection);

            command.Parameters.Add(new SqliteParameter("title", song.Title));
            command.Parameters.Add(new SqliteParameter("n", song.N));
            command.Parameters.Add(new SqliteParameter("like", song.Like));
            command.Parameters.Add(new SqliteParameter("heart", song.Heart));
            command.Parameters.Add(new SqliteParameter("artistid", song.Artist.Id));
            command.Parameters.Add(new SqliteParameter("albumid", song.Album.Id));
            command.Parameters.Add(new SqliteParameter("path", song.Path));
            command.Parameters.Add(new SqliteParameter("picid", song.Cover.Id));
            command.Parameters.Add(new SqliteParameter("hash", song.Hash));
            command.Parameters.Add(new SqliteParameter("id", song.Id));

            command.ExecuteNonQueryAsync();
        }
        public async static Task<List<Song>> ListSongs()
        {
            SqliteCommand command = new SqliteCommand(LIST_SONG_STAT, connection);

            List<Song> songs = new List<Song>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                songs.Add(await ReaderToSong(reader));
                await Task.Delay(1);
            }

            reader.Close();
            return songs;
        }
        public static void DeleteSong(int id)
        {
            SqliteCommand command = new SqliteCommand(DELETE_SONG_STAT, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));

            command.ExecuteNonQueryAsync();
        }

        private async static Task<Song> ReaderToSong(SqliteDataReader reader)
        {
            var albumTask = SelectAlbum(reader.GetInt32(6));
            var artistTask = SelectArtist(reader.GetInt32(5));
            var coversTask = SelectPicture(reader.GetInt32(8));

            var album = await albumTask;
            var artist = await artistTask;
            var covers = await coversTask;

            Song song = new Song()
            {
                Album = album.First(),
                Artist = artist.First(),
                Cover = covers.First(),
                Heart = reader.GetBoolean(4),
                Id = reader.GetInt32(0),
                Like = reader.GetBoolean(3),
                N = reader.GetInt32(2),
                Path = reader.GetString(7),
                Title = reader.GetString(1),
                Hash = reader.GetString(9)
            };
            return song;
        }
    }
}
