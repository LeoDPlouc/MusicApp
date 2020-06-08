using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.DB
{
    class Music_DataBase
    {
        static string _dbPath = "db.db3";
        SqliteConnection _connection = new SqliteConnection("Data Source=" + _dbPath);
    }
}
