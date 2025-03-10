using SQLite;
using System.Diagnostics;

namespace TaxometrMauiMvvm.Data.DataBase.Objects
{
    [Table("User")]
    public class UserModel
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Surname")]
        public string? Surname { get; set; }
        [Column("Patronymic")]
        public string? Patronymic { get; set; }

        public UserModel()
        {
            Name = CreateName();
            Debug.WriteLine(Name);
        }

        private string CreateName()
        {
            List<char> chars = Id.ToString().ToCharArray().Reverse().ToList();
            string userId = "";
            foreach (char c in chars)
            {
                userId += c;
            }

            if (userId == null) throw new NullReferenceException("Id is null");

            while (userId.Length < 10)
            {
                userId += "0";
            }
            chars.Clear();
            chars = userId.ToCharArray().Reverse().ToList();
            string result = "User";
            foreach (char c in chars)
            {
                result += c;
            }
            return result;
        }
    }
}
