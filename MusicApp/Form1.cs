using MusicApp.Beans;
using SQLite.Net;
using SQLite.Net.Platform.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicApp
{
    public partial class Form1 : Form
    {
        static string _dbPath = "db.db3";
        static SQLitePlatformWin32 _platform = new SQLitePlatformWin32();
        SQLiteConnection connection = new SQLiteConnection(_platform, _dbPath);

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection.CreateTable<Artist>();
            connection.CreateTable<Album>();
            connection.CreateTable<Song>();
        }
    }
}
