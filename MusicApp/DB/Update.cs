using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MusicApp.Beans;
using MusicApp.Config;
using MusicApp.Processing;

namespace MusicApp.DB
{
    partial class MusicDataBase
    {
        private static void Update()
        {
            string v = Config.Configuration.ReadDBVersion();
            switch(v)
            {
                case "1.0":
                    Update1_2();
                    break;
            }
            Configuration.WriteDBVersion();
        }

        private static void Update1_2()
        {
            return;
        }
    }
}
