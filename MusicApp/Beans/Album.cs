using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SQLite.Net.Attributes;

namespace MusicApp.Beans
{
    class Album
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; }
        public string Title { get; }
        public TimeSpan Duration { get; }
        public Artist Artist { get; }
        public string[] Tags { get; }
    }
}
