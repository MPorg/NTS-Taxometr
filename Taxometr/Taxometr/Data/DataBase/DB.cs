using System;
using System.IO;

namespace Taxometr.Data.DataBase
{
    public static class DB
    {
        public const string DBFileName = "NTS_Taxometr_DB.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DBFullPath
        {
            get
            {
                var fullPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(fullPath, DBFileName);
            }
        }

        public static string DebugFullPath
        {
            get
            {
                string fullPath = $"/storage/emulated/0/Android/data/com.nts.taxometr";
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                return Path.Combine(fullPath, "app.log");
            }
        }
        public static string SpecialDebugFullPath
        {
            get
            {
                string fullPath = $"/storage/emulated/0/Android/data/com.nts.taxometr";
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                return Path.Combine(fullPath, "special.log");
            }
        }
    }
}
