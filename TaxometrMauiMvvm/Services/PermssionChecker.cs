using System.Diagnostics;

namespace TaxometrMauiMvvm.Services
{
    public static class PermssionChecker
    {
        public static async Task<bool> StoragePermissionRequest(int tryCount = 0)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                }

                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                if (tryCount > 5) 
                    return false;
                return await StoragePermissionRequest(tryCount + 1);
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
