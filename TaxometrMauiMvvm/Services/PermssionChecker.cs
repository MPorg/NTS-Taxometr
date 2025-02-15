namespace TaxometrMauiMvvm.Services
{
    public static class PermssionChecker
    {
        public static async Task<bool> StoragePermissionRequest()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                var status1 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status1 != PermissionStatus.Granted)
                {
                    status1 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }
                return status == PermissionStatus.Granted;
            }
            catch
            {
                return true;
            }
        }

        public static async Task<bool> LockationPermissionRequest()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
                return status == PermissionStatus.Granted;
            }
            catch
            {
                return true;
            }
        }

        public static async Task<bool> BluetoothPermissionRequest()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Bluetooth>();
                }
                return status == PermissionStatus.Granted;
            }
            catch
            {
                return true;
            }
        }
    }
}
