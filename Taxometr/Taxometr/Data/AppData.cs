using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BluetoothClassic.Abstractions;
using Plugin.Geolocator;
using System;
using System.Threading.Tasks;
using Taxometr.Data.DataBase;
using Taxometr.Data.DataBase.Objects;
using Taxometr.Interfaces;
using Taxometr.Services;
using Taxometr.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Taxometr.Data
{
    public static class AppData
    {
        public static event Action AutoconnectionCompleated;
        public static event Action ConnectionLost;
        public static async void Initialize()
        {
            if (_taxometrDB == null) _taxometrDB = new TaxometrDB(DB.DBFullPath);

            await Task.Delay(100);

            Debug.WriteLine("Initialization");
            await CheckLocation();
            await CheckBLE();

            BLEAdapter.DeviceConnected -= OnDeviceConnected;
            BLEAdapter.DeviceDisconnected -= OnDeviceDisconnected;
            BLEAdapter.DeviceDisconnected -= ReloadProvider;

            BLEAdapter.DeviceConnected += OnDeviceConnected;
            BLEAdapter.DeviceDisconnected += OnDeviceDisconnected;
            BLEAdapter.DeviceDisconnected += ReloadProvider;

            LoadSettings();
            LoadAutoconnectDevice();
            DotsTimer();
            MainMenu.Start();
        }

        private static void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            _specialDisconnect = true;
            Debug.WriteLine("_________________Special Disk________________");
        }

        private static async void OnDeviceConnected(object sender, DeviceEventArgs e)
        {
            while (true)
            {
                var devicePref = await TaxometrDB.DevicePrefabs.GetByIdAsync(e.Device.Id);
                if (devicePref != null)
                {
                    AutoConnectDevice = e.Device;
                    AutoconnectDeviceID = AutoConnectDevice.Id;
                    ConnectedDP = devicePref;
                    ReloadProvider(sender, e);
                    Debug.WriteLine($"________________Connected {devicePref.CustomName}________________");
                    break;
                }
                await Task.Delay(100);
            }
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (BLEAdapter.ConnectedDevices.Count == 0)
                    {
                        break;
                    }
                    await Task.Delay(1000);
                }
                await Task.Delay(10);
                Debug.WriteLine($"________________Connection lost {_connectedDP.CustomName ?? "N/A"}________________");
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    ConnectionLost?.Invoke();
                    if (!_specialDisconnect) LoadAutoconnectDevice();
                    _specialDisconnect = false;
                });
            });
        }

        private static bool _specialDisconnect = false;

        private static ProviderBLE _provider;
        public static ProviderBLE Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = new ProviderBLE();
                }
                return _provider;
            }
        }

        private static void ReloadProvider(object sender, DeviceEventArgs e)
        {
            _provider = new ProviderBLE();
        }

        private static bool cont = false;

        private static bool _isFirstConnection = true;

        private static async void LoadAutoconnectDevice()
        {
            Debug.WriteLine("_____________________Autoconnection_____________________");
            bool hasAutoConnect = await Properties.GetAutoconnect();
            var prefs = await TaxometrDB.DevicePrefabs.GetPrefabsAsync();
            foreach (var pref in prefs)
            {
                if (pref.AutoConnect)
                {
                    AutoconnectDeviceID = pref.DeviceId;
                    await Properties.SaveSerialNumber(pref.SerialNumber);
                    await Properties.SaveBLEPassword(pref.BLEPassword);
                    await Properties.SaveAdminPassword(pref.UserPassword);
                    await Task.Delay(100);
                    Debug.WriteLine($"{await Properties.GetSerialNumber()}, {await Properties.GetBLEPassword()}, {await Properties.GetAdminPassword()}");
                    if (_adapter.ConnectedDevices.Count > 0)
                    {
                        var d = _adapter.ConnectedDevices[0];
                        if (d.Id == AutoconnectDeviceID) _device = d;
                    }
                    _provider = new ProviderBLE();
                    hasAutoConnect = true;
                }
            }
            if (await Properties.GetAutoconnect()) await Properties.SaveAutoconnect(hasAutoConnect);

            cont = true;
            if (_isFirstConnection)
            {
                BLEAdapter.DeviceDiscovered += async (_, e) =>
                {
                    if (e.Device.Id == AutoconnectDeviceID)
                    {
                        _device = e.Device;
                        if (cont)
                        {
                            if (await Properties.GetAutoconnect())
                            {
                                await AutoConnect();
                                cont = false;
                                return;
                            }
                        }
                    }
                };
                BLEAdapter.ScanTimeoutElapsed += async (_, e) => { await BLEAdapter.StartScanningForDevicesAsync(); };
            }
            await BLEAdapter.StartScanningForDevicesAsync();
            _isFirstConnection = false;
        }

        private static async Task AutoConnect()
        {
            Debug.WriteLine("_____________________Auto connect_____________________");
            AutoConnectBanner banner = new AutoConnectBanner();
            try
            {
                await MainMenu.Navigation.PushModalAsync(banner);
                
                var connectParameters = new ConnectParameters(false, true);
                await _adapter.ConnectToDeviceAsync(_device, connectParameters);
            }
            catch (Exception ex)
            {
                AppData.ShowToast
                    (
                        $"Не удалось подключиться к устройству: {_device.Name ?? "N/A"} \r\n" +
                        $"{ex.Message}"
                    );
            }
            finally
            {
                try
                {
                    await banner.Navigation.PopModalAsync();
                }
                catch { }

                if (BLEAdapter.ConnectedDevices.Count > 0)
                {
                    ShowToast($"Подключено: {_device.Name ?? "N/A"}");
                    AutoconnectionCompleated.Invoke();
                }
            }
        }

        public static async Task GetDeposWithdrawBanner(ProviderBLE.CashMethod method, string placeholder = "0")
        {
            DeposWithdrawCashBanner banner = new DeposWithdrawCashBanner(method, placeholder);

            await MainMenu.Navigation.PushModalAsync(banner);
        }

        private static async void LoadSettings()
        {
            bool autoconnect = await Properties.GetAutoconnect();
            #if DEBUG
            await Properties.SaveDebugMode(true);
            #else
            await Properties.SaveDebugMode(false);
            #endif
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

        private static Logger _logger;
        public static Logger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Logger(DB.DebugFullPath);
                }
                return _logger;
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
            set
            {
                _device = value;
            }
        }

        private static Guid _deviceGuid;
        public static Guid AutoconnectDeviceID
        {
            get => _deviceGuid;
            set => _deviceGuid = value;
        }

        private static DevicePrefab _connectedDP;
        public static DevicePrefab ConnectedDP
        {
            get => _connectedDP;
            private set => _connectedDP = value;
        }

        public static void ShowToast(string message)
        {
            DependencyService.Get<IToastMaker>().Show(message);
        }

        public static string Dots { get => _dots; }

        private static string _dots;
        public static void DotsTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(0.5d), (Func<bool>)(() =>
            {
                if (_dots == "...") _dots = ".";
                else _dots += ".";
                return true;
            }));
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

        public static async Task CheckLocation(bool retry = false)
        {
            if (await PermissionLockationGrantedAsync())
            {
                var locator = CrossGeolocator.Current;

                if (!locator.IsGeolocationEnabled)
                {
                    if (await MainMenu.DisplayAlert("Геолокация", "Для работы приложения, необходимо включить голокацию", "Открыть параметры", "Отмена"))
                    {
                        DependencyService.Resolve<ILockationSettings>().Show();
                    }
                }
            }
            else
            {
                if (retry) await CheckLocation(false);
                else
                {
                    ShowToast("Необходимо разрешение на использование геолокации!");
                }
            }
        }

        private static async Task<bool> PermissionLockationGrantedAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();


            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status == PermissionStatus.Granted;
        }
        private static async Task<bool> PermissionStorageGrantedAsync()
        {
            var statusW = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            var statusR = await Permissions.CheckStatusAsync<Permissions.StorageRead>();


            if (statusW != PermissionStatus.Granted || statusR != PermissionStatus.Granted)
            {
                statusW = await Permissions.RequestAsync<Permissions.StorageWrite>();
                statusR = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            return (statusW == PermissionStatus.Granted && statusR == PermissionStatus.Granted);
        }

        public static void Dispose()
        {
            Debug.SaveLog();
        }

        public static class Properties
        {
            private static string AutoconnectName = "Auto connect";
            private static string SerialNumberName = "Serial number";
            private static string BLEPasswordName = "BLE Password";
            private static string AdminPasswordName = "Admin Password";
            private static string DebugModeName = "Debug Mode";

            public static async Task<bool> SaveAutoconnect(bool value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(AutoconnectName);
                if (p == null)
                {
                    if (await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(AutoconnectName, value.ToString())) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value.ToString();
                    if (await AppData.TaxometrDB.Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<bool> GetAutoconnect()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(AutoconnectName);
                if (p == null) return true;
                return bool.Parse(p.Value);
            }

            public static async Task<bool> SaveSerialNumber(string value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(SerialNumberName);
                if (p == null)
                {
                    if (await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(SerialNumberName, value)) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value;
                    if (await AppData.TaxometrDB.Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<string> GetSerialNumber()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(SerialNumberName);
                if (p == null) return "00000616";
                return p.Value;
            }

            public static async Task<bool> SaveBLEPassword(string value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(BLEPasswordName);
                if (p == null)
                {
                    if (await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(BLEPasswordName, value)) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value;
                    if (await AppData.TaxometrDB.Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<string> GetBLEPassword()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(BLEPasswordName);
                if (p == null) return "100000";
                return p.Value;
            }
            public static async Task<bool> SaveAdminPassword(string value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(AdminPasswordName);
                if (p == null)
                {
                    if (await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(AdminPasswordName, value)) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value;
                    if (await AppData.TaxometrDB.Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<string> GetAdminPassword()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(AdminPasswordName);
                if (p == null) return "000001";
                return p.Value;
            }
            public static async Task<bool> SaveDebugMode(bool value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(DebugModeName);
                if (p == null)
                {
                    if (await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(DebugModeName, value.ToString())) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value.ToString();
                    if (await AppData.TaxometrDB.Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<bool> GetDebugMode()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(DebugModeName);
                if (p == null) return true;
                return bool.Parse(p.Value);
            }
        }

        public static class Debug
        {
            internal static async void WriteLine(string v)
            {
                if (await Properties.GetDebugMode())
                {
                    System.Diagnostics.Debug.WriteLine(v);
                    Logger.Log($"[{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {v}");
                }
            }

            internal static void SaveLog()
            {
                Logger.SaveLog();
            }
        }
    }
}
