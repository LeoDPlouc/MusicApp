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
        const string CREATE_SONG_STAT = "insert into song(path, like, heart) values(@path, @like, @heart);";
        const string UPDATE_SONG_STAT = "update song set path = @path,like = @like, heart = @heart;";
        const string SELECT_SONG_STAT = "select * from song where path = @path;";
        const string DELETE_SONG_STAT = "delete from song where path = @path;";
        public static void CreateSong(string path, bool like, bool heart)
        {
            SqliteCommand command = new SqliteCommand(CREATE_SONG_STAT, connection);

            command.Parameters.Add(new SqliteParameter("@path", path));
            command.Parameters.Add(new SqliteParameter("@like", like));
            command.Parameters.Add(new SqliteParameter("@heart", heart));

            command.ExecuteNonQueryAsync();
        }
        public static void UpdateSong(Song song)
        {
            SqliteCommand command = new SqliteCommand(UPDATE_SONG_STAT, connection);

            command.Parameters.Add(new SqliteParameter("path", song.Path));
            command.Parameters.Add(new SqliteParameter("like", song.Like));
            command.Parameters.Add(new SqliteParameter("heart", song.Heart));

            command.ExecuteNonQueryAsync();
        }
        public static Tuple<string, bool, bool> SelectSong(string path)
        {
            SqliteCommand command = new SqliteCommand(SELECT_SONG_STAT, connection);
            command.Parameters.Add(new SqliteParameter("@path", path));

            var reader = command.ExecuteReader();
            try
            {
                return new Tuple<string, bool, bool>(
                    reader.GetString(0),
                    reader.GetBoolean(1),
                    reader.GetBoolean(2));

            }
            catch
            {
                return null;
            }
            finally
            {
                reader.Close();
            }
        }
        public static void DeleteSong(string path)
        {
            SqliteCommand command = new SqliteCommand(DELETE_SONG_STAT, connection);
            command.Parameters.Add(new SqliteParameter("@path", path));

            command.ExecuteNonQueryAsync();
        }
    }
}
