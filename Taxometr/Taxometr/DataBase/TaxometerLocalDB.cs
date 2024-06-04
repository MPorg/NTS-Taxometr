using SQLite;
using Taxometr.DataBase.Objects;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;

namespace Taxometr.DataBase
{
    public class TaxometerLocalDB
    {
        private readonly SQLiteAsyncConnection connection;

        public TaxometerLocalDB(string connectionStr)
        {
            connection = new SQLiteAsyncConnection(connectionStr);
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
