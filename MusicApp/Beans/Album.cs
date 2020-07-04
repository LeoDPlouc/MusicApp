﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Drawing;

namespace MusicApp.Beans
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public string[] Tags { get; set; }
        public Picture Cover { get; set; }
        public int Year { get; set; }
    }
}
