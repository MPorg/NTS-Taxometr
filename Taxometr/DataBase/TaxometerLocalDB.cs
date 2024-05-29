using SQLite;
using Taxometr.DataBase.Objects;

namespace Taxometr.DataBase
{
    public class TaxometerLocalDB
    {
        private const string DB_Name = "Taxometr.db3";
        private readonly SQLiteAsyncConnection connection;

        public TaxometerLocalDB()
        {
            connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));
            connection.CreateTableAsync<Property>();
        }

        public async Task<List<Property>> GetPropertiesAsync()
        {
            return await connection.Table<Property>().ToListAsync();
        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await connection.Table<Property>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Property> GetPropertyByNameAsync(string name)
        {
            return await connection.Table<Property>().Where(x => x.PropertyName == name).FirstOrDefaultAsync();
        }

        public async Task<int> CreatePropertyAsync(Property property)
        {
            return await connection.InsertAsync(property);
        }

        public async Task<int> UpdatePropertyAsync(Property property)
        {
            return await connection.UpdateAsync(property);
        }

        public async Task<int> DeletePropertyAsync(Property property)
        {
            return await connection.DeleteAsync(property);
        }
    }
}
