using SQLite;
using TaxometrMauiMvvm.Data.DataBase.Objects;

namespace TaxometrMauiMvvm.Data.DataBase
{
    public class TaxometrDB
    {
        private Properties _properties;
        private Prefabs _prefabs;
        private Users _users;

        public Properties Property
        { 
            get
            {
                return _properties;
            }
        }
        public Prefabs Device
        {
            get
            {
                return _prefabs;
            }
        }
        public Users User
        {
            get
            {
                return _users;
            }
        }

        public TaxometrDB(string connectionString)
        {
            _properties = new Properties(connectionString);
            _prefabs = new Prefabs(connectionString);
            _users = new Users(connectionString);
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

                int c = await _connection.UpdateAsync(device);
                if (c > 0) DeviceChanged?.Invoke(device);
                return c;
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

        public class Users
        {
            public Users(string connectionStr)
            {
                _connection = new SQLiteAsyncConnection(connectionStr);
                Initialize();
            }

            private async void Initialize()
            {
                await _connection.CreateTableAsync<UserModel>();
            }

            private readonly SQLiteAsyncConnection _connection;

            public async Task<List<UserModel>> GetUsersAsync()
            {
                return await _connection.Table<UserModel>().ToListAsync();
            }

            public async Task<UserModel> GetByNameAsync(string name)
            {
                return await _connection.Table<UserModel>().Where(x => x.Name == name).FirstOrDefaultAsync();
            }
            public async Task<int> CreateAsync(UserModel user)
            {
                return await _connection.InsertAsync(user);
            }

            public async Task<int> UpdateAsync(UserModel user)
            {
                if (await GetByNameAsync(user.Name) == null) await CreateAsync(user);
                return await _connection.UpdateAsync(user);
            }

            public async Task<int> DeleteAsync(UserModel user)
            {
                return await _connection.DeleteAsync(user);
            }
        }
    }
}
