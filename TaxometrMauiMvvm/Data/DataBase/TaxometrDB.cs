using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxometrMauiMvvm.Data.DataBase.Objects;

namespace TaxometrMauiMvvm.Data.DataBase
{
    public class TaxometrDB
    {
        private Properties _properties;
        private BLEDevices _devices;
        private Prefabs _prefabs;

        public Properties Property
        { 
            get
            {
                return _properties;
            }
        }
        public BLEDevices Devices
        {
            get
            {
                return _devices;
            }
        }
        public Prefabs DevicePrefabs
        {
            get
            {
                return _prefabs;
            }
        }

        public TaxometrDB(string connectionString)
        {
            _properties = new Properties(connectionString);
            _devices = new BLEDevices(connectionString);
            _prefabs = new Prefabs(connectionString);
        }

        public class Properties
        {
            public Properties(string connectionStr)
            {
                _connection = new SQLiteAsyncConnection(connectionStr);
                Initialize();
            }

            private async void Initialize()
            {
                await _connection.CreateTableAsync<PropertyModel>();
            }

            private readonly SQLiteAsyncConnection _connection;

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
            public BLEDevices(string connectionStr)
            {
                _connection = new SQLiteAsyncConnection(connectionStr);
                Initialize();
            }

            private async void Initialize()
            {
                await _connection.CreateTableAsync<DeviceModel>();
            }

            private readonly SQLiteAsyncConnection _connection;

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

        public class Prefabs
        {
            public event Action<DevicePrefab> DeviceChanged;
            public event Action<DevicePrefab> DeviceCreated;

            public Prefabs(string connectionStr)
            {
                _connection = new SQLiteAsyncConnection(connectionStr);
                Initialize();
            }

            private async void Initialize()
            {
                await _connection.CreateTableAsync<DevicePrefab>();
            }

            private readonly SQLiteAsyncConnection _connection;

            public async Task<List<DevicePrefab>> GetPrefabsAsync()
            {
                return await _connection.Table<DevicePrefab>().ToListAsync();
            }

            public async Task<DevicePrefab> GetByIdAsync(Guid id)
            {
                return await _connection.Table<DevicePrefab>().Where(x => x.DeviceId == id).FirstOrDefaultAsync();
            }
            public async Task<int> CreateAsync(DevicePrefab device)
            {
                DeviceCreated?.Invoke(device);
                return await _connection.InsertAsync(device);
            }

            public async Task<int> UpdateAsync(DevicePrefab device)
            {
                if (await GetByIdAsync(device.DeviceId) == null)
                    return await _connection.InsertAsync(device);

                DeviceChanged?.Invoke(device);
                return await _connection.UpdateAsync(device);
            }

            public async Task<int> DeleteAsync(DevicePrefab device)
            {
                if (device == null) return 0;

                int totalCount = (await _connection.Table<DevicePrefab>().ToListAsync()).Count;
                int result = await _connection.DeleteAsync(device);
                if (totalCount - result == 0) Initialize();
                return result;
            }
        }
    }
}
