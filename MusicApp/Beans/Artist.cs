using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    class Artist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; }
        public string Name;
    }
}
