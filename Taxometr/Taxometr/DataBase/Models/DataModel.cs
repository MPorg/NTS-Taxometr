using SQLite;
using System;

namespace ProductDirectory_v4.Models
{
    public class DataModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        static Guid idGuid;

        public static Guid IdGuid
        {
            get
            {
                if(idGuid == null)
                {
                    idGuid = Guid.NewGuid();
                }
                return idGuid;
            }
        }
    }
}
