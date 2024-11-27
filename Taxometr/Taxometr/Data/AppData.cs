using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BluetoothClassic.Abstractions;
using Plugin.Geolocator;
using SQLitePCL;
using System;
using System.Linq;
using System.Threading.Tasks;
using Taxometr.Data.DataBase;
using Taxometr.Interfaces;
using Taxometr.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Taxometr.Data
{
    public static class AppData
    {
        public static event Action AutoconnectionCompleated;
        public static void Initialize()
        {
            _taxometrDB = new TaxometrDB(DB.DBFullPath);
            LoadSettings();
            LoadDevices();
            DotsTimer();
        }

        private static bool cont = false;
        private static async void LoadDevices()
        {
            var devices = await TaxometrDB.Devices.GetDevicesAsync();
            if (devices.Count > 0)
            {
                var d = devices.Last();
                cont = true;
                BLEAdapter.DeviceDiscovered += async (_, e) =>
                {
                    if (cont)
                    {
                        if (e.Device.Id == d.DeviceId)
                        {
                            _device = e.Device;
                            if (await Properties.GetAutoconnect())
                            {
                                await AutoConnect();
                                cont = false;
                            }
                        }
                    }
                };
                await BLEAdapter.StartScanningForDevicesAsync();
            }
        }

        private static async Task AutoConnect()
        {
            AutoConnectBanner banner = new AutoConnectBanner();
            try
            {
                await MainMenu.Navigation.PushModalAsync(banner);
                
                var connectParameters = new ConnectParameters(false, true);
                await _adapter.ConnectToDeviceAsync(_device, connectParameters);
            }
            catch
            {
                ShowToast($"Не удалось подключиться к устройству: {_device.Name ?? "N/A"}");
            }
            finally
            {
                await banner.Navigation.PopModalAsync();
                if (BLEAdapter.ConnectedDevices.Count > 0)
                {
                    ShowToast($"Подключено: {_device.Name ?? "N/A"}");
                    AutoconnectionCompleated.Invoke();
                }
            }

        }

        private static async void LoadSettings()
        {
            await CheckLocation();
            await CheckBLE();
            bool autoconnect = await Properties.GetAutoconnect();
        }

        public static MainMenu MainMenu { get; set; }

        private static TaxometrDB _taxometrDB;
        public static TaxometrDB TaxometrDB
        {
            get
            {
                if (_taxometrDB == null)
                {
                    _taxometrDB = new TaxometrDB(DB.DBFullPath);
                }
                return _taxometrDB;
            }
        }

        private static IAdapter _adapter;
        public static IAdapter BLEAdapter
        {
            get
            {
                if (_adapter == null)
                {
                    _adapter = CrossBluetoothLE.Current.Adapter;
                }
                return _adapter;
            }
        }

        private static IDevice _device;
        public static IDevice AutoConnectDevice
        {
            get => _device;
            set => _device = value;
        }

        public static void ShowToast(string message)
        {
            DependencyService.Get<IToastMaker>().Show(message);
        }

        public static string Dots { get => _dots; }

        private static string _dots;
        public static void DotsTimer()
        {
            string dots = ".";
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(0.5d), (Func<bool>)(() =>
            {
                if (dots == "...") dots = ".";
                else dots += ".";
                return true;
            }));
            _dots = dots;
        }

        public static async Task CheckBLE()
        {
            IBluetoothAdapter adapter = DependencyService.Resolve<IBluetoothAdapter>();
            if (!adapter.Enabled)
            {
                if (await MainMenu.DisplayAlert("Блютуз", "Для работы приложения, необходимо включить блютуз", "ОК", "Отмена"))
                {
                    adapter.Enable();
                }
            }
        }

        public static async Task CheckLocation()
        {
            await PermissionsGrantedAsync();

            var locator = CrossGeolocator.Current;

            if (!locator.IsGeolocationEnabled)
            {
                if (await MainMenu.DisplayAlert("Геолокация", "Для работы приложения, необходимо включить голокацию", "Открыть параметры", "Отмена"))
                {
                    DependencyService.Resolve<ILockationSettings>().Show();
                }
            }
        }


        private static async Task<bool> PermissionsGrantedAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();


            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status == PermissionStatus.Granted;
        }

        public static class Properties
        {
            private static string AutoconnectName = "Auto connect";
            private static string SerialNumberName = "Serial number";

            public static async Task SaveAutoconnect(bool value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(AutoconnectName);
                if (p == null) await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(AutoconnectName, value.ToString()));
                else
                {
                    p.Value = value.ToString();
                    await AppData.TaxometrDB.Property.UpdateAsync(p);
                }
            }
            public static async Task<bool> GetAutoconnect()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(AutoconnectName);
                if (p == null) return false;
                return bool.Parse(p.Value);
            }

            public static async Task SaveSerialNumber(string value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(SerialNumberName);
                if (p == null) await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(SerialNumberName, value));
                else
                {
                    p.Value = value;
                    await AppData.TaxometrDB.Property.UpdateAsync(p);
                }
            }

            public static async Task<string> GetSerialNumber()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(SerialNumberName);
                if (p == null) return "00000616";
                return p.Value;
            }
        }
    }
}
