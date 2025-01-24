using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Essentials;
using System.Collections.Generic;

namespace Taxometr.Droid
{
    [Activity(Label = "НТС-Таксометр", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            await Permissions.RequestAsync<BLEPermission>();
            await Permissions.RequestAsync<LockationPermission>();
            await Permissions.RequestAsync<StoragePermission>();
            await Permissions.RequestAsync<NotifPermission>();
            
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private class BLEPermission : Xamarin.Essentials.Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>{
                (Android.Manifest.Permission.BluetoothScan, true),
                (Android.Manifest.Permission.BluetoothConnect, true),
                (Android.Manifest.Permission.Bluetooth, true),
                (Android.Manifest.Permission.BluetoothPrivileged, true),
                (Android.Manifest.Permission.BluetoothAdmin, true),
                (Android.Manifest.Permission.Internet, true)
            }.ToArray();
        }
        private class LockationPermission : Xamarin.Essentials.Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>{
                (Android.Manifest.Permission.LocationHardware, true),
                (Android.Manifest.Permission.AccessBackgroundLocation, true),
                (Android.Manifest.Permission.AccessCoarseLocation, true),
                (Android.Manifest.Permission.AccessFineLocation, true)
            }.ToArray();
        }
        private class StoragePermission : Xamarin.Essentials.Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>{
                (Android.Manifest.Permission.ManageExternalStorage, true),
                (Android.Manifest.Permission.ReadExternalStorage, true),
                (Android.Manifest.Permission.WriteExternalStorage, true)
            }.ToArray();
        }
        private class NotifPermission : Xamarin.Essentials.Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>{
                (Android.Manifest.Permission.AccessNotificationPolicy, true),
                (Android.Manifest.Permission.PostNotifications, true),
                (Android.Manifest.Permission.ForegroundService, true)
            }.ToArray();
        }
    }
}