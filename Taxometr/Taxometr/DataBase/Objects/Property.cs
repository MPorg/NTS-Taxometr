using SQLite;

namespace Taxometr.DataBase.Objects
{
    [Table("Settings")]
    public class Property
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string PropertyName { get; set; }
        [Column("value")]
        public string PropertyValue { get; set; }

        public Property() { }

        public Property(string name, string value)
        {
            PropertyName = name;
            PropertyValue = value;
        }
    }
}
