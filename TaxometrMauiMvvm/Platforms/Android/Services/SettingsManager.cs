using Android.Bluetooth;
using Android.Content;
using Android.Locations;
using TaxometrMauiMvvm.Interfaces;

namespace TaxometrMauiMvvm.Platforms.Android.Services
{
    public class SettingsManager : ISettingsManager
    {
        public bool BluetoothIsEnable()
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            return bluetoothAdapter != null && bluetoothAdapter.IsEnabled;
        }

        public bool LockationIsEnable()
        {
            LocationManager locationManager = (LocationManager)global::Android.App.Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider) ||
                   locationManager.IsProviderEnabled(LocationManager.NetworkProvider);
        }

        public void ShowAppSettings()
        {
            var intent = new Intent(global::Android.Provider.Settings.ActionApplicationDetailsSettings);
            var uri = global::Android.Net.Uri.FromParts("package:", AppInfo.PackageName, null);
            intent.SetData(uri);
            Platform.CurrentActivity.StartActivity(intent);
        }

        public void ShowBluetoothSettings()
        {
            Intent intent = new Intent(global::Android.Provider.Settings.ActionBluetoothSettings);
            Platform.CurrentActivity.StartActivity(intent);
        }

        public void ShowLocationSettings()
        {
            Intent intent = new Intent(global::Android.Provider.Settings.ActionLocationSourceSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            Platform.CurrentActivity.StartActivity(intent);
        }
    }
}
