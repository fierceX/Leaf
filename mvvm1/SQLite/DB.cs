using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm1.Model
{
    class DB
    {
        private static string DbFileName = "Sqlite.db";
        public static string DbFilePath;
        public static void Init()
        {
            DbFilePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path, DbFileName);
            using (var db = GetDbConnection())
            {
                db.CreateTable<User>();
            }
        }
        public static SQLiteConnection GetDbConnection()
        {
            SQLiteConnection con = new SQLiteConnection(DbFilePath);
            return con;
        }
    }
}
