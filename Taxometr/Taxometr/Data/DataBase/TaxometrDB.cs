using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxometr.Data.DataBase.Objects;

namespace Taxometr.Data.DataBase
{
    public class TaxometrDB
    {
        private readonly SQLiteAsyncConnection _connection;

        public Properties Property { get; private set; } 
        public BLEDevices Devices { get; private set; }

        public TaxometrDB(string connectionString)
        {
            _connection = new SQLiteAsyncConnection(connectionString);
            Property = new Properties(_connection);
            Devices = new BLEDevices(_connection);
        }

        public class Properties
        {
            public Properties(SQLiteAsyncConnection connection)
            {
                _connection = connection;
                _connection.CreateTableAsync<PropertyModel>();
            }

            private SQLiteAsyncConnection _connection;

            public async Task<List<PropertyModel>> GetPropertiesAsync()
            {
                return await _connection.Table<PropertyModel>().ToListAsync();
            }

            public async Task<PropertyModel> GetByNameAsync(string name)
            {
                return await _connection.Table<PropertyModel>().Where(x => x.Name == name).FirstOrDefaultAsync();
            }
            public async Task<int> CreateAsync(PropertyModel property)
            {
                return await _connection.InsertAsync(property);
            }

            public async Task<int> UpdateAsync(PropertyModel property)
            {
                if (await GetByNameAsync(property.Name) == null) await CreateAsync(property);
                return await _connection.UpdateAsync(property);
            }

            public async Task<int> DeleteAsync(PropertyModel property)
            {
                return await _connection.DeleteAsync(property);
            }
        }

        public class BLEDevices
        {
            public BLEDevices(SQLiteAsyncConnection connection)
            {
                _connection = connection;
                _connection.CreateTableAsync<DeviceModel>();
            }

            private SQLiteAsyncConnection _connection;

            public async Task<List<DeviceModel>> GetDevicesAsync()
            {
                return await _connection.Table<DeviceModel>().ToListAsync();
            }

            public async Task<DeviceModel> GetByIdAsync(Guid id)
            {
                return await _connection.Table<DeviceModel>().Where(x => x.DeviceId == id).FirstOrDefaultAsync();
            }
            public async Task<int> CreateAsync(DeviceModel device)
            {
                return await _connection.InsertAsync(device);
            }

            public async Task<int> UpdateAsync(DeviceModel device)
            {
                if (await GetByIdAsync(device.DeviceId) == null)
                    return await _connection.InsertAsync(device);
                return await _connection.UpdateAsync(device);
            }

            public async Task<int> DeleteAsync(DeviceModel device)
            {
                return await _connection.DeleteAsync(device);
            }
        }
    }
}
