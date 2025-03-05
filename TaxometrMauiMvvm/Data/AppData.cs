using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;
using TaxometrMauiMvvm.Data.DataBase;
using TaxometrMauiMvvm.Data.DataBase.Objects;
using TaxometrMauiMvvm.Services;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Views.Banners;
using static TaxometrMauiMvvm.Services.ProviderBLE;
using TaxometrMauiMvvm.Models.Cells;

namespace TaxometrMauiMvvm.Data
{
    public static class AppData
    {
        const string BASE_CHECKER = "▚▚▚▚▚";
        const string BASE_CHECKER_INVERCE= "▞▞▞▞▞";

        public enum AppState
        {
            NormalDisconnected,
            NormalConnected,
            BackgroundDisconnected,
            BackgroundConnected
        }

        public static AppState State = AppState.NormalDisconnected;

        public static event Action? AutoconnectionCompleated;
        public static event Action? ConnectionLost;

        private static bool _specialDisconnect = false;

        public static bool RequestedQuit = true;

        private static ProviderBLE _provider;
        public static async Task<ProviderBLE> Provider()
        {
            if (_provider == null)
            {
                _provider = new ProviderBLE();
                await _provider.Initialize();
            }
            return _provider;
        }

        public static TabBarViewModel TabBarViewModel { get; set; }

        private static bool cont = false;

        private static bool _isFirstConnection = true;

        public static MainMenu? MainMenu { get; set; } = null;

        private static TaxometrDB _taxometrDB;

        public static async Task<TaxometrDB> TaxometrDB()
        {
            if (!await PermissionChecker.StoragePermissionRequest())
            {
                //ShowToast("Необходимо разрешение на обработку данных");
                return null;
            }
            else
            {
                if (_taxometrDB == null)
                {
                    _taxometrDB = new TaxometrDB(DB.DBFullPath);
                    await Task.Delay(3000);
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

        private static IDevice _autoconnectDevice;
        public static IDevice AutoConnectDevice
        {
            get => _autoconnectDevice;
            set
            {
                _autoconnectDevice = value;
            }
        }
        private static IDevice _connectedDevice;
        public static IDevice ConnectedDevice
        {
            get => _connectedDevice;
            set
            {
                _connectedDevice = value;
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

        private static async void DevicePrefabs_DeviceChanged(DevicePrefab prefab)
        {
            Debug.WriteLine($"{prefab.CustomName}, {AutoconnectDeviceID}");
            if (prefab.DeviceId == AutoconnectDeviceID)
            {
                await Properties.SaveSerialNumber(prefab.SerialNumber);
                await Properties.SaveBLEPassword(prefab.BLEPassword);
                await Properties.SaveAdminPassword(prefab.UserPassword);
                await ReloadProvider();
            }

            if (prefab.DeviceId == ConnectedDP?.DeviceId)
            {
                ConnectedDP = await (await TaxometrDB()).DevicePrefabs.GetByIdAsync(ConnectedDP.DeviceId);
                await SetConnectedDevicePrefab(ConnectedDP);
            }

            /*}
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }*/
        }

        public static DevicePrefab LastDevicePrefab;
        public static string Dots { get => _dots; }

        private static string _dots;
        public static string Checker { get => _checker; }
        public static string CheckerInverce { get => _checker == BASE_CHECKER ? BASE_CHECKER_INVERCE : BASE_CHECKER; }

        private static string _checker = BASE_CHECKER;

        private static bool _firstInit = true;

        private static IToastMaker _toastMaker;
        private static ISettingsManager _settingsManager;
        private static IKeyboard _keyboard;
        private static IBackgroundConnectionController _backgroundConnectionController;
        private static INotificationService _notificationService;

        private static bool _initializationCompleate = false;
        public static bool InitializationCompleate => _initializationCompleate;

        public static void SetDependencyServices(IToastMaker toastMaker, ISettingsManager settingsManager, IKeyboard keyboard, IBackgroundConnectionController backgroundConnectionController, INotificationService notificationService)
        {
            _toastMaker = toastMaker;
            _settingsManager = settingsManager;
            _keyboard = keyboard;
            _backgroundConnectionController = backgroundConnectionController;
            _notificationService = notificationService;
        }

        public static async Task Initialize()
        {
            await Task.Delay(1000);
            LoadSettings();

            if (_firstInit)
            {
                _firstInit = false;
                BLEAdapter.DeviceConnected -= OnDeviceConnected;
                BLEAdapter.DeviceDisconnected -= OnDeviceDisconnected;
                (await TaxometrDB()).DevicePrefabs.DeviceChanged -= DevicePrefabs_DeviceChanged;

                BLEAdapter.DeviceConnected += OnDeviceConnected;
                BLEAdapter.DeviceDisconnected += OnDeviceDisconnected;
                (await TaxometrDB()).DevicePrefabs.DeviceChanged += DevicePrefabs_DeviceChanged;

                DotsTimer();
                LoadAutoconnectDevice();
            }
            else
            {
                LoadAutoconnectDevice();
            }

            _initializationCompleate = true;
            Debug.WriteLine("_____________________________AppData Initialize_____________________________");
        }

        private static void Quit()
        {
            if (MainMenu == null) return;
            MainMenu.Quit();
        }

        public static async Task<bool> ConnectToDevice(IDevice d)
        {
            Guid id = d.Id;
            DevicePrefab device = await (await TaxometrDB()).DevicePrefabs.GetByIdAsync(id);
            if (device == null)
            {
                try
                {
                    await SetConnectedDevice(d);
                    ConnectParameters parameters = new ConnectParameters(false, true);
                    await _adapter.ConnectToKnownDeviceAsync(id, parameters);
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
                if (d != null) await SetConnectedDevice(d); 
                await SetConnectedDevicePrefab(prefab);
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
            //_specialDisconnect = true;
            Debug.WriteLine("_________________Special Disk________________");
        }

        private static async void OnDeviceConnected(object sender, DeviceEventArgs e)
        {
            while (true)
            {
                var devicePref = await (await TaxometrDB()).DevicePrefabs.GetByIdAsync(e.Device.Id);

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
                            //DependencyService.Resolve<INotificationService>().CloseNotifications();
                            //DependencyService.Resolve<IBLEConnectionController>().Start();
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
                        _backgroundConnectionController.Stop();
                        if (!_specialDisconnect) _notificationService.ShowNotification("Подключение утеряно", $"Подключение с {LastDevicePrefab.CustomName} утеряно");
                        break;
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    ConnectedDP = null;
                    ConnectionLost?.Invoke();
                    if (!_specialDisconnect) LoadAutoconnectDevice();
                });
            });
        }

        private static async Task ReloadProvider()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                Debug.WriteLine($"{await Properties.GetSerialNumber()}, {await Properties.GetBLEPassword()}, {await Properties.GetAdminPassword()}");
                await Task.Delay(100);
                if (_provider != null)
                {
                    _provider.AnswerCompleate -= OnAnswerCompleate;
                    _provider.Dispose();
                }

                _provider = new ProviderBLE();
                await _provider.Initialize();
                _provider.AnswerCompleate += OnAnswerCompleate;
            });
        }

        private static void OnAnswerCompleate(byte cmd, Dictionary<string, string> answer)
        {
            if (answer.TryGetValue("answ", out string? answ))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (MainMenu == null) return;
                    await MainMenu.DisplayAlert("Ошибка", answ, "OK");
                });
            }
        }

        public static async void LoadAutoconnectDevice()
        {
            //Debug.WriteLine("_____________________Autoconnection_____________________");
            //bool hasAutoConnect = await Properties.GetAutoconnect();
            var prefs = await (await TaxometrDB()).DevicePrefabs.GetPrefabsAsync();
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
                            _autoconnectDevice = e.Device;
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
        private static AutoconnectBanner banner = null;
        private static async Task AutoConnect(DevicePrefab prefab)
        {
            Debug.WriteLine("_____________________Auto connect_____________________");
            if (banner == null)
            {
                banner = new AutoconnectBanner();
                if (MainMenu == null) return;
                await MainMenu.Navigation.PushModalAsync(banner);
            }
            else
            {
                Debug.WriteLine("pop");
            }
            if (await ConnectToDevice(await (await TaxometrDB()).DevicePrefabs.GetByIdAsync(prefab.DeviceId)))
            {
                await banner.Navigation.PopModalAsync();
                banner = null;
                //ShowToast($"Подключено: {prefab.CustomName ?? "N/A"}");
                AutoconnectionCompleated?.Invoke();
            }
            else
            {
                await banner.Navigation.PopModalAsync();
                banner = null;
                //ShowToast($"Не удалось подключиться к устройству: {prefab.CustomName ?? "N/A"}");
            }

        }
        public static async Task SetConnectedDevice(IDevice device)
        {
            ConnectedDevice = device;
        }

        public static async Task SetConnectedDevicePrefab(DevicePrefab pref)
        {
            if (pref.AutoConnect) AutoconnectDeviceID = pref.DeviceId;

            ConnectedDP = pref;
            await Properties.SaveSerialNumber(pref.SerialNumber);
            await Properties.SaveBLEPassword(pref.BLEPassword);
            await Properties.SaveAdminPassword(pref.UserPassword);

            if (_adapter.ConnectedDevices.Count > 0)
            {
                var d = _adapter.ConnectedDevices[0];
                if (d.Id == AutoconnectDeviceID) _autoconnectDevice = d;
            }

            await ReloadProvider();
        }

        public static async Task GetDeposWithdrawBanner(ProviderBLE.CashMethod method, string placeholder = "0")
        {
            DeposWithdrawCashBanner banner = new DeposWithdrawCashBanner(method, placeholder);

            await MainMenu.Navigation.PushModalAsync(banner, true);
        }

        public static async Task GetOpenCheckBanner()
        {
            OpenCheckBanner banner = new OpenCheckBanner();
            banner.Canceled += ((result) =>
            {
                MainMenu.Navigation.PopModalAsync(true);
            });
            await MainMenu.Navigation.PushModalAsync(banner, true);
        }

        public static event Action<bool> CloseCheckBannerAnswer;

        public static async Task GetCloseCheckBanner(string startVal, string preVal)
        {
            CloseCheckBanner banner = new CloseCheckBanner(startVal, preVal);
            banner.Canceled += ((result) =>
            {
                CloseCheckBannerAnswer?.Invoke(result);
                MainMenu.Navigation.PopModalAsync(true);
            });
            await MainMenu.Navigation.PushModalAsync(banner, true);
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

        public static async Task<bool> CreateNewPrefab(IDevice device)
        {
            bool result = await MainMenu.CreateDevicePrefabMenu(device);
            return result;
        }

        public static void ShowToast(string message)
        {
            _toastMaker.Show(message);
        }

        public static void HideKeyboard()
        {
            _keyboard.Hide();
        }

        public static void DotsTimer()
        {
            Application.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(0.5d), (Func<bool>)(() =>
            {
                if (_dots == "...") _dots = ".";
                else _dots += ".";

                if (_checker == BASE_CHECKER) _checker = BASE_CHECKER_INVERCE;
                else _checker = BASE_CHECKER;



                return true;
            }));
        }

        public static async Task CheckBLE(Page page)
        {
            if (!_settingsManager.BluetoothIsEnable())
            {
                //if (MainMenu == null) return;
                if (await page.DisplayAlert("Параметры", "Необходимо включить блютуз", "Открыть параметры", "Нет"))
                {
                    _settingsManager.ShowBluetoothSettings();
                }
            }
        }

        public static async Task CheckBLEPermission()
        {
            if (!await PermissionChecker.BluetoothPermissionRequest())
            {
                ShowToast("Необходимо разрешение на работу с блютузом");
                _settingsManager.ShowAppSettings();
            }
        }

        public static async Task CheckNotificationPermission()
        {
            if (!await PermissionChecker.NotificationPermissionRequest())
            {
                ShowToast("Необходимо разрешение на работу с уведомлениями");
                _settingsManager.ShowAppSettings();
            }
        }

        public static async Task CheckLockation(Page page)
        {
            if (!_settingsManager.LockationIsEnable())
            {
                if (await page.DisplayAlert("Параметры", "Необходимо включить геолокацию", "Открыть параметры", "Нет"))
                {
                    _settingsManager.ShowLocationSettings();
                }
            }
        }
            
        public static async Task CheckLockationPermission(bool retry = false)
        {
            if (!await PermissionChecker.LockationPermissionRequest())
            {
                ShowToast("Необходимо разрешение на работу с блютузом");
                _settingsManager.ShowAppSettings();
            }
        }


        public static async void TryOpenCash()
        {
            if (MainMenu == null) return;
            
            await MainMenu.GoToAsync("//Drive", true);
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

        public static async Task PrintReceiptOrSwitchMode(string key)
        {
            var mode = key switch
            {
                "Z" => MenuMode.Z,
                "X" => MenuMode.X,
                "M" => MenuMode.Main,
                "D" => MenuMode.Drive,
                _ => throw new NotImplementedException(),
            };
            (await Provider()).OpenMenuOrPrintReceipt(mode, await Properties.GetAdminPassword(), true, mode == MenuMode.Z || mode == MenuMode.X ? 20 : 10);
        }

        public static void Dispose()
        {
            Debug.SaveLog();
        }

        internal static void StopBackground()
        {
            _backgroundConnectionController.Stop();
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
                var p = await (await TaxometrDB()).Property.GetByNameAsync(AutoconnectName);
                if (p == null)
                {
                    if (await (await TaxometrDB()).Property.CreateAsync(new DataBase.Objects.PropertyModel(AutoconnectName, value.ToString())) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value.ToString();
                    if (await (await TaxometrDB()).Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<bool> GetAutoconnect()
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(AutoconnectName);
                if (p == null) return true;
                return bool.Parse(p.Value);
            }

            public static async Task<bool> SaveSerialNumber(string value)
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(SerialNumberName);
                if (p == null)
                {
                    if (await (await TaxometrDB()).Property.CreateAsync(new DataBase.Objects.PropertyModel(SerialNumberName, value)) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value;
                    if (await (await TaxometrDB()).Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<string> GetSerialNumber()
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(SerialNumberName);
                if (p == null) return "00000000";
                return p.Value;
            }

            public static async Task<bool> SaveBLEPassword(string value)
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(BLEPasswordName);
                if (p == null)
                {
                    if (await (await TaxometrDB()).Property.CreateAsync(new DataBase.Objects.PropertyModel(BLEPasswordName, value)) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value;
                    if (await (await TaxometrDB()).Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<string> GetBLEPassword()
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(BLEPasswordName);
                if (p == null) return "000000";
                return p.Value;
            }

            public static async Task<bool> SaveAdminPassword(string value)
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(AdminPasswordName);
                if (p == null)
                {
                    if (await (await TaxometrDB()).Property.CreateAsync(new DataBase.Objects.PropertyModel(AdminPasswordName, value)) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value;
                    if (await (await TaxometrDB()).Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<string> GetAdminPassword()
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(AdminPasswordName);
                if (p == null) return "000000";
                return p.Value;
            }

            public static async Task<bool> SaveLastCashSum(int value)
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(LastCashSum);
                if (p == null)
                {
                    if (await (await TaxometrDB()).Property.CreateAsync(new DataBase.Objects.PropertyModel(LastCashSum, value.ToString())) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value.ToString();
                    if (await (await TaxometrDB()).Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<int> GetLastCashSum()
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(LastCashSum);
                if (p == null) return 0;
                if (int.TryParse(p.Value, out var value))
                {
                    return value;
                }
                return 0;
            }

            public static async Task<bool> SaveDebugMode(bool value)
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(DebugModeName);
                if (p == null)
                {
                    if (await (await TaxometrDB()).Property.CreateAsync(new DataBase.Objects.PropertyModel(DebugModeName, value.ToString())) > 0) return true;
                    else return false;
                }
                else
                {
                    p.Value = value.ToString();
                    if (await (await TaxometrDB()).Property.UpdateAsync(p) > 0) return true;
                    else return false;
                }
            }
            public static async Task<bool> GetDebugMode()
            {
                var p = await (await TaxometrDB()).Property.GetByNameAsync(DebugModeName);
                if (p == null) return false;
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
                    //if (specialDbg) Logger.LogSpecial($"[{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {v}");
                    //else Logger.Log($"[{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] {v}");
                }

            }

            public static async void SaveLog()
            {
                if (await Properties.GetDebugMode())
                {
                    Logger.SaveLog();
                }
            }
        }
    }
}
