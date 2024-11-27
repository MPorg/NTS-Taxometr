using System;
using System.IO;

namespace Taxometr.Data.DataBase
{
    public static class DB
    {
        public const string DBFileName = "NTS_Taxometr_DB.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DBFullPath
        {
            get
            {
                var fullPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(fullPath, DBFileName);
            }
        }
    }
}
