using SQLite;

namespace Taxometr.Data.DataBase.Objects
{
    [Table("Properties")]
    public class PropertyModel
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Value")]
        public string Value { get; set; }

        public PropertyModel() { }

        public PropertyModel(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
