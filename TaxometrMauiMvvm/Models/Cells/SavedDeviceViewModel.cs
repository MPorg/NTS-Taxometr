using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE.Abstractions.Contracts;
using TaxometrMauiMvvm.Fonts.IconFont;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Data.DataBase.Objects;
using TaxometrMauiMvvm.Models.Pages;
using System.Diagnostics;

namespace TaxometrMauiMvvm.Models.Cells;

public partial class SavedDeviceViewModel : ObservableObject
{
    public event Action OnOpen;

    private SavedViewModel _savedViewModel;

    private IDevice _device;
    private DevicePrefab _devicePrefab;
    public DevicePrefab Device => _devicePrefab;

    [ObservableProperty]
    private string _customName;

    [ObservableProperty]
    private string _connectionStateRU;

    [ObservableProperty]
    private string _serialNumber;

    [ObservableProperty]
    private string _blePass;

    [ObservableProperty]
    private string _adminPass;

    [ObservableProperty]
    private bool _autoconnect;

    [ObservableProperty]
    private string _connectBtnText;

    private bool _connectBtnEnabled;

    public bool ConnectBtnEnabled
    {
        get { return _connectBtnEnabled && !IsSelectionMode; }
        set
        {
            OnPropertyChanging(nameof(ConnectBtnEnabled));
            _connectBtnEnabled = value;
            OnPropertyChanged(nameof(ConnectBtnEnabled));
        }
    }
    [ObservableProperty]
    private bool _isSelectionMode;


    [ObservableProperty]
    private bool _settingsBntIsVisible;

    [ObservableProperty]
    private float _cellHeight;
    [ObservableProperty]
    private float _firstRowHeight;
    [ObservableProperty]
    private float _seckondRowHeight;

    private float _cellBaseHeight = 120;
    private float _cellAdditingHeight = 240;

    [ObservableProperty]
    private int fgIndex = 1;
    [ObservableProperty]
    private int bgIndex = -10;

    private bool _isChanged = false;

    [RelayCommand]
    private void Select()
    {
        IsSelectionMode = true;
        LongSelectionIsVisible = false;
        BgColor = _selectedColors[0];
        BgShadowColor = _selectedColors[1];
        BgLightColor = _selectedColors[2];
    }

    [RelayCommand]
    private void Deselect()
    {
        BgColor = _unselectedColors[0];
        BgShadowColor = _unselectedColors[1];
        BgLightColor = _unselectedColors[2];
    }

    private Color[] _unselectedColors = new Color[3];
    private Color[] _selectedColors = new Color[3];
    [ObservableProperty]
    private Color _bgColor;
    [ObservableProperty]
    private Color _bgShadowColor;
    [ObservableProperty]
    private Color _bgLightColor;



    [RelayCommand]
    private void LongPressed()
    {
        _savedViewModel.EnterToEditModeCommand.Execute(this);
    }

    [ObservableProperty]
    private bool _isOpen;

    private bool _longSelectionIsVisible;
    public bool LongSelectionIsVisible
    {
        get { return _longSelectionIsVisible && !IsOpen; }
        set
        {
            OnPropertyChanging(nameof(LongSelectionIsVisible));
            _longSelectionIsVisible = value;
            OnPropertyChanged(nameof(LongSelectionIsVisible));
        }
    }

    [ObservableProperty]
    private string _switchButtonGlyph;

    [RelayCommand]
    private async Task TryOpenClose(bool isEdited)
    {
        if (isEdited)
            await Close(_isChanged);
        else 
            Open();
    }

    [RelayCommand]
    private async void BreakEdit()
    {
        if (!_isChanged || AppData.MainMenu != null && await AppData.MainMenu.DisplayAlert("Выйти без сохранения?", "Это действие отменит последние изменения", "Да", "Продолжить редактирование"))
        {
            await Close(false);
        }
    }

    private void Open()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            CellHeight = _cellBaseHeight + _cellAdditingHeight + 80;
            SeckondRowHeight = _cellAdditingHeight - 20;
            SwitchIndex();
            OnOpen?.Invoke();
            SwitchButtonGlyph = IsOpen ? Icons.IconCheck : Icons.IconGear;
        }
    }

    private async Task Close(bool save = true)
    {
        if (IsOpen)
        {
            IsOpen = false;
            CellHeight = _cellBaseHeight + 80;
            SeckondRowHeight = 0;
            SwitchIndex();
            if (save) await SavePrefab();
            await LoadPrefab();
            SwitchButtonGlyph = IsOpen ? Icons.IconCheck : Icons.IconGear;
        }
        _isChanged = false;
    }

    private async Task LoadPrefab()
    {
        if (_devicePrefab == null) return;
        await (await AppData.TaxometrDB()).Device.GetByIdAsync(_devicePrefab.DeviceId);
        CustomName = _devicePrefab.CustomName;
        SerialNumber = _devicePrefab.SerialNumber;
        BlePass = _devicePrefab.BLEPassword;
        AdminPass = _devicePrefab.UserPassword;
        Autoconnect = _devicePrefab.AutoConnect;
    }

    private async Task SavePrefab()
    {
        if (_devicePrefab == null) return;
        _devicePrefab.CustomName = CustomName;
        _devicePrefab.SerialNumber = SerialNumber;
        _devicePrefab.BLEPassword = BlePass;
        _devicePrefab.UserPassword = AdminPass;
        _devicePrefab.AutoConnect = Autoconnect;
        await (await AppData.TaxometrDB()).Device.UpdateAsync(_devicePrefab);
    }

    private void SwitchIndex()
    {
        int tmp = BgIndex;
        BgIndex = FgIndex;
        FgIndex = tmp;
    }
    private SavedDeviceViewModel(SavedViewModel savedViewModel, string name, string connectionState, string serNum = "00000000", string blePass = "000000", string adminPass = "000000", bool autoconnect = false)
    {
        CustomName = name;
        ConnectionStateRU = connectionState;
        SerialNumber = serNum;
        BlePass = blePass;
        AdminPass = adminPass;
        Autoconnect = autoconnect;
        ConnectBtnText = "";
        CellHeight = _cellBaseHeight + 80;
        FirstRowHeight = _cellBaseHeight - 20;
        SeckondRowHeight = 0;
        SwitchButtonGlyph = IsOpen ? Icons.IconCheck : Icons.IconGear;
        LongSelectionIsVisible = true;
        ConnectBtnText = "Подключиться";
        ConnectBtnEnabled = false;
        GetColors();
        BgColor = _unselectedColors[0];
        BgShadowColor = _unselectedColors[1];
        BgLightColor = _unselectedColors[2];
        _savedViewModel = savedViewModel;
        PropertyChanged += SavedDeviceViewModel_PropertyChanged;
        AppData.BLEAdapter.DeviceDiscovered += BLEAdapter_DeviceDiscovered;
        AppData.ConnectionLost += AppData_ConnectionLost;
        Start();
    }

    private void AppData_ConnectionLost()
    {
        _device = null;
        UpdateState();
    }

    private void BLEAdapter_DeviceDiscovered(object? sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
    {
        if (e.Device != null && e.Device.Id != Guid.Empty && Device != null && e.Device.Id == Device.DeviceId)
        {
            _device = e.Device;
        }
    }

    private void GetColors()
    {
        if (Application.Current.PlatformAppTheme == AppTheme.Dark)
        {
            //BG
            if (Application.Current.Resources.TryGetValue("BackgroundDark", out var color))
            {
                if (color is Color c) _unselectedColors[0] = c;
            }

            if (Application.Current.Resources.TryGetValue("TextLink", out var linkColor))
            {
                if (linkColor is Color c) _selectedColors[0] = c;
            }
            //Shadow
            if (Application.Current.Resources.TryGetValue("BackgroundDarked", out var colorSh))
            {
                if (colorSh is Color c) _unselectedColors[1] = c;
            }

            if (Application.Current.Resources.TryGetValue("TextRedLink", out var linkColorSh))
            {
                if (linkColorSh is Color c) _selectedColors[1] = c;
            }
            //Light
            if (Application.Current.Resources.TryGetValue("Background", out var colorLi))
            {
                if (colorLi is Color c) _unselectedColors[2] = c;
            }

            if (Application.Current.Resources.TryGetValue("TextLightLink", out var linkColorLi))
            {
                if (linkColorLi is Color c) _selectedColors[2] = c;
            }
        }
        else if (Application.Current.PlatformAppTheme == AppTheme.Light)
        {
            //BG
            if (Application.Current.Resources.TryGetValue("BackgroundLighted", out var color))
            {
                if (color is Color c) _unselectedColors[0] = c;
            }
            if (Application.Current.Resources.TryGetValue("TextLink", out var linkColor))
            {
                if (linkColor is Color c) _selectedColors[0] = c;
            }
            //Shadow
            if (Application.Current.Resources.TryGetValue("Background", out var colorSh))
            {
                if (colorSh is Color c) _unselectedColors[1] = c;
            }

            if (Application.Current.Resources.TryGetValue("TextRedLink", out var linkColorSh))
            {
                if (linkColorSh is Color c) _selectedColors[1] = c;
            }
            //Light
            if (Application.Current.Resources.TryGetValue("BackgroundLight", out var colorLi))
            {
                if (colorLi is Color c) _unselectedColors[2] = c;
            }

            if (Application.Current.Resources.TryGetValue("TextLightLink", out var linkColorLi))
            {
                if (linkColorLi is Color c) _selectedColors[2] = c;
            }
        }
    }

    private void SavedDeviceViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsOpen)) OnPropertyChanged(nameof(LongSelectionIsVisible));
        if (e.PropertyName == nameof(IsSelectionMode)) OnPropertyChanged(nameof(ConnectBtnEnabled));

        if
        (
            e.PropertyName == nameof(CustomName) ||
            e.PropertyName == nameof(SerialNumber) ||
            e.PropertyName == nameof(BlePass) ||
            e.PropertyName == nameof(AdminPass) ||
            e.PropertyName == nameof(Autoconnect)
        )
        {
            _isChanged = true;
        }
    }

    internal SavedDeviceViewModel(SavedViewModel savedViewModel) : this(savedViewModel, "Default name", DeviceViewModelExtentions.ConnectionStateRU["Disconnected"]) { }

    public SavedDeviceViewModel(DevicePrefab devicePrefab, SavedViewModel savedViewModel) : this(savedViewModel, devicePrefab.CustomName, "Неизвестно", devicePrefab.SerialNumber, devicePrefab.BLEPassword, devicePrefab.UserPassword, devicePrefab.AutoConnect)
    {
        _devicePrefab = devicePrefab;
        FindDevice();
        //else _device = AppData.BLEAdapter.GetKnownDevicesByIds([devicePrefab.DeviceId])[0];
    }

    private void FindDevice()
    {
        if (AppData.BLEAdapter.ConnectedDevices.Count > 0 && AppData.BLEAdapter.ConnectedDevices[0].Id == _devicePrefab?.DeviceId) _device = AppData.BLEAdapter.ConnectedDevices[0];
    }

    bool _timerIsStarted = false;

    private void Start()
    {
        if (!_timerIsStarted)
        {
            _timerIsStarted = true;
            Application.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), new Func<bool>(()=>
            {
                UpdateState();
                return true;
            }));
        }
    }

    private void UpdateState()
    {
        FindDevice();
        if (_device != null)
        {
            ConnectionStateRU = DeviceViewModelExtentions.ConnectionStateRU[_device.State.ToString()];
            if (AppData.BLEAdapter.ConnectedDevices.Count > 0 && AppData.BLEAdapter.ConnectedDevices.Contains(_device))
            {
                ConnectBtnText = "Отключиться";
            }
            else
            {
                ConnectBtnText = "Подключиться";
            }
            ConnectBtnEnabled = true;
        }
        else
        {
            ConnectBtnText = "Подключиться";

            ConnectBtnEnabled = false;
        }
    }

    [RelayCommand]
    private async void ConnectDisconnect()
    {
        if (_device != null)
        {
            if (AppData.ConnectedDP != null)
            {
                bool isThis = AppData.ConnectedDP.DeviceId == _device.Id;
                await AppData.SpecialDisconnect();
                if (isThis) return;
            }

            bool result = await AppData.ConnectToDevice(Device);

            Debug.WriteLine(result);
        }
    }
}
