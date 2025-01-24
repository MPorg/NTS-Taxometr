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
        public enum AppState
        {
            NormalDisconnected,
            NormalConnected,
            BackgroundDisconnected,
            BackgroundConnected
        }

        public static AppState State = AppState.NormalDisconnected;

        public static event Action AutoconnectionCompleated;
        public static event Action ConnectionLost;

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

        private static bool cont = false;

        private static bool _isFirstConnection = true;

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
                    _logger = new Logger(DB.DebugFullPath, DB.SpecialDebugFullPath);
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

        public static DevicePrefab LastDevicePrefab;
        public static string Dots { get => _dots; }

        private static string _dots;

        private static bool _firstInit = true;
        public static async void Initialize()
        {
            if (_taxometrDB == null) _taxometrDB = new TaxometrDB(DB.DBFullPath);

            await Task.Delay(100);

            Debug.WriteLine("Initialization");
            await CheckLocation();
            await CheckBLE();

            if (_firstInit)
            {
                _firstInit = false;
                BLEAdapter.DeviceConnected -= OnDeviceConnected;
                BLEAdapter.DeviceDisconnected -= OnDeviceDisconnected;

                BLEAdapter.DeviceConnected += OnDeviceConnected;
                BLEAdapter.DeviceDisconnected += OnDeviceDisconnected;

                LoadSettings();
                DotsTimer();
                LoadAutoconnectDevice();
                MainMenu.Start();
            }
            else
            {
                LoadAutoconnectDevice();
            }
        }

        public static async Task<bool> ConnectToDevice(Guid id)
        {
            DevicePrefab device = await _taxometrDB.DevicePrefabs.GetByIdAsync(id);
            if (device == null)
            {
                try
                {
                    await _adapter.ConnectToKnownDeviceAsync(id);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return await ConnectToDevice(device);
        }

        public static async Task<bool> ConnectToDevice(DevicePrefab prefab)
        {
            try
            {
                ConnectParameters parameters = new ConnectParameters(false, true);
                var d = await _adapter.ConnectToKnownDeviceAsync(prefab.DeviceId, parameters);
                SetConnectedDevice(prefab);
                ShowToast($"Подключено {prefab.CustomName}");
                return true;
            }
            catch
            {
                ShowToast($"Не удалось подключиться");
                return false;
            }
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
                    ConnectedDP = devicePref;
                    LastDevicePrefab = devicePref;
                    Debug.WriteLine($"________________Connected {devicePref.CustomName}________________");

                    switch (State)
                    {
                        case AppState.NormalDisconnected:
                            State = AppState.NormalConnected;
                            break;
                        case AppState.BackgroundDisconnected:
                            DependencyService.Resolve<INotificationService>().CloseNotifications();
                            DependencyService.Resolve<IBLEConnectionController>().Start();
                            State = AppState.BackgroundConnected;
                            break;
                    }
                    _specialDisconnect = false;
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
                await Task.Delay(100);
                Debug.WriteLine($"________________Connection lost {_connectedDP?.CustomName ?? "N/A"}________________");



                switch (State)
                {
                    case AppState.NormalConnected:
                        State = AppState.NormalDisconnected;
                        break;
                    case AppState.BackgroundConnected:
                        State = AppState.BackgroundDisconnected;
                        Debug.WriteLine("____________________BackgraundDisconnected__________________");
                        DependencyService.Resolve<IBLEConnectionController>().Stop();
                        if (!_specialDisconnect) DependencyService.Resolve<INotificationService>().ShowNotification("Подключение утеряно", $"Подключение с {LastDevicePrefab.CustomName} утеряно");
                        break;
                }

                await Device.InvokeOnMainThreadAsync(() =>
                {
                    ConnectedDP = null;
                    ConnectionLost?.Invoke();
                    if (!_specialDisconnect) LoadAutoconnectDevice();
                });
            });
        }

        private static void ReloadProvider()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(100);
                if (_provider != null) _provider.Dispose();
                _provider = new ProviderBLE();
            });
        }

        public static async void LoadAutoconnectDevice()
        {
            Debug.WriteLine("_____________________Autoconnection_____________________");
            //bool hasAutoConnect = await Properties.GetAutoconnect();
            var prefs = await TaxometrDB.DevicePrefabs.GetPrefabsAsync();
            DevicePrefab deviceToConnect = null;

            if (prefs != null)
            {
                cont = true;
                foreach (var pref in prefs)
                {
                    if (pref.AutoConnect)
                    {
                        deviceToConnect = pref;
                        //hasAutoConnect = true;
                    }
                }
                if (_isFirstConnection)
                {
                    _isFirstConnection = false;
                    BLEAdapter.DeviceDiscovered += async (_, e) =>
                    {
                        if (deviceToConnect != null && e.Device.Id == deviceToConnect.DeviceId && e.Device.Id != Guid.Empty)
                        {
                            _device = e.Device;
                            if (cont)
                            {
                                if (await Properties.GetAutoconnect() && !_specialDisconnect)
                                {
                                    if (deviceToConnect != null)
                                    {
                                        await AutoConnect(deviceToConnect);
                                        cont = false;
                                        return;
                                    }
                                }
                            }
                        }
                    };
                    BLEAdapter.ScanTimeoutElapsed += async (_, e) => { await BLEAdapter.StartScanningForDevicesAsync(); };
                }
            }
            else
            {
                cont = false;
            }
            await BLEAdapter.StartScanningForDevicesAsync();
        }

        private static async Task AutoConnect(DevicePrefab prefab)
        {
            Debug.WriteLine("_____________________Auto connect_____________________");
            AutoConnectBanner banner = new AutoConnectBanner();

            await MainMenu.Navigation.PushModalAsync(banner);

            if (await ConnectToDevice(prefab))
            {
                await banner.Navigation.PopModalAsync();

                ShowToast($"Подключено: {prefab.CustomName ?? "N/A"}");
                AutoconnectionCompleated.Invoke();
            }
            else
            {
                await banner.Navigation.PopModalAsync();
                ShowToast($"Не удалось подключиться к устройству: {prefab.CustomName ?? "N/A"}");
            }
                
        }

        public static async void SetConnectedDevice(DevicePrefab pref)
        {
            if (pref.AutoConnect) AutoconnectDeviceID = pref.DeviceId;

            ConnectedDP = pref;
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

            ReloadProvider();
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

        public static void ShowToast(string message)
        {
            DependencyService.Get<IToastMaker>().Show(message);
        }

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
                    await Task.Run(() =>
                    {
                        adapter.Enable();
                    });
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

        public static void TryOpenCash()
        {
            Debug.WriteLine("______________Try open cash___________");
            MainMenu.GoToAsync("//Drive", true);
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

        public static void ClearAutoconnectDevice()
        {
            AutoConnectDevice = null;
            AutoconnectDeviceID = Guid.Empty;
            ConnectedDP = null;
        }

        public static async Task SpecialDisconnect()
        {
            _specialDisconnect = true;
            if (State == AppState.NormalConnected || State == AppState.BackgroundConnected)
            {
                await BLEAdapter.DisconnectDeviceAsync(BLEAdapter.ConnectedDevices[0]);
            }
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
            private static string LastCashSum = "Last cash sum";

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

            public static async Task<bool> SaveLastCashSum(int value)
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(LastCashSum);
                if (p == null)
                {
                    if (await AppData.TaxometrDB.Property.CreateAsync(new DataBase.Objects.PropertyModel(LastCashSum, value.ToString())) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value.ToString();
                    if (await AppData.TaxometrDB.Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<int> GetLastCashSum()
            {
                var p = await AppData.TaxometrDB.Property.GetByNameAsync(LastCashSum);
                if (p == null) return 0;
                if (int.TryParse(p.Value, out var value))
                {
                    return value;
                }
                return 0;
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
            public static async void WriteLine(string v, bool specialDbg = false)
            {
                if (await Properties.GetDebugMode())
                {
                    System.Diagnostics.Debug.WriteLine(v);
                    if (specialDbg) Logger.LogSpecial($"[{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {v}");
                    else Logger.Log($"[{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {v}");
                }

            }

            public static void SaveLog()
            {
                Logger.SaveLog();
            }
        }
    }
}
