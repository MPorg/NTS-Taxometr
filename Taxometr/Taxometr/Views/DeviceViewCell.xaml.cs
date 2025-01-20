using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Taxometr.Data;
using Taxometr.Data.DataBase.Objects;
using Taxometr.Fonts.IconFont;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceViewCell : ViewCell
	{
		public class DeviceViewCellBinding : BindableObject
		{
			public IDevice Device { get; private set; } = null;
			public Guid DeviceId { get; private set; }
			public string Name { get => Device.Name; }
			private string _customName = "";
			public string CustomName 
			{ 
				get
				{
					if (String.IsNullOrEmpty(_customName))
					{
						if (Device != null) return Device.Name;
						else return "<???>";
					}
					else return _customName;
				}
            }
            public string SerNum { get; }
            public string BLEPass { get; }
            public string AdminPass { get; }

			private bool _autoConnect;
			public bool AutoConnect { get => _autoConnect; set => _autoConnect = value; }

			public string AutoConnectStr 
			{
				get
				{
					switch (_autoConnect)
					{
						case true:
							return "Да";
						case false:
							return "Нет";
						default:
                            return "Нет";
                    }
				}
			}

			public event Action<DeviceViewCellBinding> AutoconnectionChange;

            public event Action<DeviceViewCellBinding> Deleted;

            public event Action<DeviceViewCellBinding> Updated;

            public string ConnectButtonText
			{
				get
				{
					switch (ConnectionState)
					{
						case "Недоступен":
							return "Недоступно";
                        case "Подключение":
                            return "Подключиться";
                        case "Подключение.":
                            return "Подключиться";
                        case "Подключение..":
                            return "Подключиться";
                        case "Подключение...":
                            return "Подключиться";
                        case "Подключено":
                            return "Отключиться";
						case "Не подключено":
                            return "Подключиться";
                        case "Ожидание":
                            return "Ожидание";
                        default: return "Недоступно";

					}
				}
			}

			public bool ConnectButtonActive
            {
                get
                {
                    switch (ConnectionState)
                    {
                        case "Недоступен":
                            return false;
                        case "Подключение":
                            return false;
                        case "Подключение.":
                            return false;
                        case "Подключение..":
                            return false;
                        case "Подключение...":
                            return false;
                        case "Подключено":
                            return true;
                        case "Не подключено":
                            return true;
                        case "Ожидание":
                            return false;
                        default: return false;

                    }
                }
            }

            public string ConnectionState
			{ 
				get
				{
					if (Device == null) return "Недоступен";

                    switch (Device.State)
					{
						case Plugin.BLE.Abstractions.DeviceState.Connected:
							if (ConnectionCompleate) return "Подключено";
							else return "Подключение" + AppData.Dots;
						case Plugin.BLE.Abstractions.DeviceState.Connecting:
                            return "Подключение" + AppData.Dots;
						case Plugin.BLE.Abstractions.DeviceState.Disconnected:
							return "Не подключено";
						case Plugin.BLE.Abstractions.DeviceState.Limited:
							return "Ожидание";
						default:
							return "<?>";
					}
				} 
			}

			public ICommand OpenContextMenu {  get; set; }
            public ICommand ShowButtons { get; set; }
            public ICommand HideButtons { get; set; }

            internal bool IsShowed { get; private set; } = false;
            internal bool IsOpened { get; set; } = false;
			internal bool IsFavorite { get; private set; }
			internal bool ConnectionCompleate { get; set; } = false;

            internal void SwitchButtonsState()
            {
                IsShowed = !IsShowed;
            }
            public DeviceViewCellBinding(IDevice device)
            {
                Device = device;
				DeviceId = device.Id;
                IsFavorite = false;
            }
            public DeviceViewCellBinding(Guid deviceID, string serNum, string blePass, string adminPass, string customName, bool autoConnect)
            {
                IsFavorite = true;
				DeviceId = deviceID;
				_customName = customName;
				SerNum = serNum;
				BLEPass = blePass;
				AdminPass = adminPass;
				_autoConnect = autoConnect;
				ConnectionCompleate = true;
				FindDevice();
            }


            IDevice _findedDevice = null;   
            internal async void FindDevice()
            {
                Device = null;
                AppData.Debug.WriteLine($"Find device by id {DeviceId}");

                AppData.BLEAdapter.DeviceDiscovered += OnBLEAdapter_DeviceDiscovered;
                await AppData.BLEAdapter.StartScanningForDevicesAsync();

                while(Device == null)
                {
                    await Task.Delay(5000);
                }
            }

            private void OnBLEAdapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (AppData.BLEAdapter.ConnectedDevices.Count > 0 && AppData.BLEAdapter.ConnectedDevices[0].Id == DeviceId)
                    {
                        _findedDevice = AppData.BLEAdapter.ConnectedDevices[0];
                    }

                    if (e.Device != null)
                    {
                        if (e.Device.Id == DeviceId)
                        {
                            _findedDevice = e.Device;
                        }
                    }

                    if (_findedDevice != null)
                    {
                        Device = _findedDevice;
                        AppData.BLEAdapter.DeviceDiscovered -= OnBLEAdapter_DeviceDiscovered;
                    }

                    AppData.BLEAdapter.StartScanningForDevicesAsync();
                });
            }

            public void UpdateDevice()
			{
				if (Device != null && AppData.BLEAdapter.ConnectedDevices.Count > 0)
				{
					IDevice connectedDevice = AppData.BLEAdapter.ConnectedDevices[0];
					if (Device.Id == connectedDevice.Id) Device = connectedDevice;
                }
			}

            internal void Delete()
            {
                Deleted?.Invoke(this);
            }
            internal void Update()
            {
                Updated?.Invoke(this);
            }
        }

		DeviceViewCellBinding _binding;

		private double _baseHeight;
        private Color _baseEditButtonColor;

		public DeviceViewCell ()
		{
			InitializeComponent();
			_baseHeight = Height;
            _baseEditButtonColor = ((FontImageSource)EditSaveBtn.Source).Color;

            FindBinding();
            StartTimer();

            try
            {
                SerNum.Children.Remove(SerNumEntry);
                BLEPass.Children.Remove(BLEPassEntry);
                AdminPass.Children.Remove(AdminPassEntry);
                AutoConnect.Children.Remove(AutoConnectSwitch);

                SerNum.Children.Remove(SerNumValue);
                BLEPass.Children.Remove(BLEPassValue);
                AdminPass.Children.Remove(AdminPassValue);
                AutoConnect.Children.Remove(AutoConnectValue);
            }
            catch { }
			SwitchEditMode();
        }

        private void StartTimer()
        {
			Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
			{
				if (_binding != null) 
				{
                    if (_editMode)
                    {
                        ConnectBtn.Text = "Отмена";
                        ConnectBtn.IsEnabled = true;
                    }
                    else
                    {
					    ConnectionStateLabel.Text = _binding.ConnectionState;
                        ConnectBtn.Text = _binding.ConnectButtonText;
                        ConnectBtn.IsEnabled = _binding.ConnectButtonActive;
                    }
                    _binding.UpdateDevice();
				}
				return true;
			});
        }

        private async void FindBinding()
        {
			int i = 0;
			while (i < 10)
			{
				if (BindingContext is DeviceViewCellBinding binding)
				{
					binding.OpenContextMenu = new Command(OpenContextMenu);
					binding.ShowButtons = new Command(ShowButtons);
					binding.HideButtons = new Command(HideButtons);
					_binding = binding;

					if (binding.IsFavorite)
					{
						binding.IsOpened = true;
						UpdateUI(true);
						binding.ShowButtons.Execute(binding);
						if (binding.AutoConnect) binding.OpenContextMenu.Execute(binding);
					}
					break;
				}
                await Task.Delay(500);
				i++;
            }
        }

		private void OpenContextMenu()
        {

            if (!_editMode)
            {
                AppData.Debug.WriteLine("Click");
                _binding.IsOpened = !_binding.IsOpened;
                UpdateUI(_binding.IsOpened);
            }
        }

        private void UpdateUI(bool opened = false)
        {
			Info.IsVisible = !opened;
            FontImageSource image = OpenCloseBtn.Source as FontImageSource;
            if (opened)
            {
                image.Glyph = Icons.IconArrowDown;
                Height = _baseHeight;
            }
            else
            {
                image.Glyph = Icons.IconArrowUp;
                Height = _baseHeight + 220;
            }
            ViewFrame.HeightRequest = Height - 10;
        }

        public void ShowButtons()
		{
			if (!Buttons.IsVisible)
			{
				Buttons.IsVisible = true;
				_binding.SwitchButtonsState();
			}
        }

		public void HideButtons()
        {
			if (Buttons.IsVisible)
			{
				Buttons.IsVisible = false;
				_binding.SwitchButtonsState();
			}
        }

        private void OnOpenCloseBtnClicked(object sender, EventArgs e)
        {
			_binding?.OpenContextMenu.Execute(_binding);
        }

        private void OnAutoConnectSwitch_Toggled(object sender, ToggledEventArgs e)
        {
			if (_binding != null)
			{
				bool val = e.Value;
				_binding.AutoConnect = val;
			}
        }

        private async void OnConnectBtnClicked(object sender, EventArgs e)
        {
            if (_editMode)
            {
                SwitchEditMode();
            }

			if (_binding != null)
			{
				switch(ConnectBtn.Text)
				{
					case "Подключиться":
						try
                        {
                            if (_binding.Device.Id != Guid.Empty)
                            {

                                if (await AppData.ConnectToDevice(_binding.DeviceId))
                                {
                                    if (AppData.BLEAdapter.ConnectedDevices.Count > 0)
                                    {
                                        AppData.ShowToast($"Подключено: {_binding.CustomName ?? "N/A"}");
                                        _binding.FindDevice();
                                    }
                                }
                            }
                            else
                            {
                                _binding.FindDevice();
                            }
                        }
						catch
						{
                            AppData.ShowToast
                                (
                                    $"Не удалось подключиться к устройству: {_binding.CustomName ?? "N/A"} \r\n"
                                );
                            _binding.FindDevice();
                        }
						break;
					case "Отключиться":
                        await AppData.SpecialDisconnect();
                        _binding.FindDevice();
                        break;
				}
			}
        }

        private async void OnDeleteBtnClicked(object sender, EventArgs e)
        {
			if (_binding == null) return;
			if (await AppData.MainMenu.DisplayAlert($"Удалить {_binding.CustomName}?", $"Вы действительно хотите удалить сохранённое устройство {_binding.CustomName}", "ОК", "Отмена"))
			{
				_binding.Delete();
			}
        }

        private void OnEditSaveBtnClicked(object sender, EventArgs e)
        {
            if (_editMode) SaveChanges();
			SwitchEditMode();
        }

        private async void SaveChanges()
        {
            //if (AppData.BLEAdapter.ConnectedDevices.Count > 0) await AppData.SpecialDisconnect();

            SerNumEntry.Unfocus();
            BLEPassEntry.Unfocus();
            AdminPassEntry.Unfocus();

            string serNum = SerNumEntry.Text;
            string blePass = BLEPassEntry.Text;
            string adminPass = AdminPassEntry.Text;
            bool autoconnect = AutoConnectSwitch.IsToggled;

            if (_binding != null && _binding.Device != null)
            {
                DevicePrefab dp = await AppData.TaxometrDB.DevicePrefabs.GetByIdAsync(_binding.Device.Id);
                dp.SerialNumber = serNum;
                dp.BLEPassword = blePass;
                dp.UserPassword = adminPass;
                dp.AutoConnect = autoconnect;

                List<DevicePrefab> prefs = await AppData.TaxometrDB.DevicePrefabs.GetPrefabsAsync();
                if (autoconnect && prefs.Count > 1)
                {
                    foreach (DevicePrefab prefab in prefs)
                    {
                        prefab.AutoConnect = false;
                        await AppData.TaxometrDB.DevicePrefabs.UpdateAsync(prefab);
                    }
                }

                await AppData.TaxometrDB.DevicePrefabs.UpdateAsync(dp);
                _binding.Update();
                AppData.SetConnectedDevice(dp);
            }
        }

        private bool _editMode = true;
		private void SwitchEditMode()
		{
			FontImageSource source = EditSaveBtn.Source as FontImageSource;
			if (_editMode)
			{
				_editMode = false;

                _binding?.ShowButtons.Execute(_binding);

				try
				{
					SerNum.Children.Remove(SerNumEntry);
					BLEPass.Children.Remove(BLEPassEntry);
					AdminPass.Children.Remove(AdminPassEntry);
					AutoConnect.Children.Remove(AutoConnectSwitch);
				}
				catch { }

                SerNum.Children.Add(SerNumValue);
                BLEPass.Children.Add(BLEPassValue);
                AdminPass.Children.Add(AdminPassValue);
                AutoConnect.Children.Add(AutoConnectValue);

                SerNumEntry.IsVisible = false;
                BLEPassEntry.IsVisible = false;
                AdminPassEntry.IsVisible = false;
                AutoConnectSwitch.IsVisible = false;

                SerNumValue.IsVisible = true;
				BLEPassValue.IsVisible = true;
				AdminPassValue.IsVisible = true;
				AutoConnectValue.IsVisible = true;

				source.Glyph = Icons.IconPen;
                source.Color = _baseEditButtonColor;
            }
			else
			{
				_editMode = true;

                _binding?.HideButtons.Execute(_binding);

                try
                {
                    SerNum.Children.Remove(SerNumValue);
                    BLEPass.Children.Remove(BLEPassValue);
                    AdminPass.Children.Remove(AdminPassValue);
                    AutoConnect.Children.Remove(AutoConnectValue);
                }
                catch { }

                SerNum.Children.Add(SerNumEntry);
                BLEPass.Children.Add(BLEPassEntry);
                AdminPass.Children.Add(AdminPassEntry);
                AutoConnect.Children.Add(AutoConnectSwitch);

                SerNumValue.IsVisible = false;
                BLEPassValue.IsVisible = false;
                AdminPassValue.IsVisible = false;
                AutoConnectValue.IsVisible = false;

                SerNumEntry.IsVisible = true;
                BLEPassEntry.IsVisible = true;
                AdminPassEntry.IsVisible = true;
                AutoConnectSwitch.IsVisible = true;

                SerNumEntry.Text = _binding.SerNum;
                BLEPassEntry.Text = _binding.BLEPass;
                AdminPassEntry.Text = _binding.AdminPass;
				AutoConnectSwitch.IsToggled = _binding.AutoConnect;

                SerNumEntry.Focus();

                source.Glyph = Icons.IconCheck;
                source.Color = Color.Green;
            }
		}

        private void SerNumEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool retry = false;
            string newValue = SerNumEntry.Text;
            if (newValue.Length == 8)
            {
                SerNumEntry.Text = newValue;
                BLEPassEntry.Focus();
            }
            else if (newValue.Length > 8)
            {
                newValue = newValue.Remove(8);
                retry = true;
            }
            if (!int.TryParse(newValue, out int _))
            {
                for (int i = 0; i < newValue.Length; i++)
                {
                    if (!int.TryParse(newValue.ToCharArray()[i].ToString(), out _))
                    {
                        retry = true;
                        newValue = newValue.Remove(i, 1);
                        break;
                    }
                }
            }
            if (retry)
            {
                SerNumEntry.Text = newValue;
            }
        }

        private void SerNumEntry_Completed(object sender, EventArgs e)
        {
            BLEPassEntry.Unfocus();
        }
        private void SerNumEntry_Unfocused(object sender, FocusEventArgs e)
        {
            string result = SerNumEntry.Text;
            if (!String.IsNullOrEmpty(result))
            {
                if (result.Length < 8)
                {
                    string start = "";
                    for (int i = 0; i < 8 - result.Length; i++)
                    {
                        start += "0";
                    }
                    result = start + result;
                    SerNumEntry.Text = result;
                }
            }
        }

        private void BLEPassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool retry = false;
            string newValue = BLEPassEntry.Text;
            if (newValue.Length == 6)
            {
                BLEPassEntry.Text = newValue;
                AdminPassEntry.Focus();
                AdminPassEntry.Text += "\r";
            }
            else if (newValue.Length > 6)
            {
                newValue = newValue.Remove(6);
                retry = true;
            }
            if (!int.TryParse(newValue, out int _))
            {
                for (int i = 0; i < newValue.Length; i++)
                {
                    if (!int.TryParse(newValue.ToCharArray()[i].ToString(), out _))
                    {
                        retry = true;
                        newValue = newValue.Remove(i, 1);
                        break;
                    }
                }
            }
            if (retry)
            {
                BLEPassEntry.Text = newValue;
            }
        }

        private void BLEPassEntry_Completed(object sender, EventArgs e)
        {
            AdminPassEntry.Focus();
        }

        private void BLEPassEntry_Unfocused(object sender, FocusEventArgs e)
        {
            string result = BLEPassEntry.Text;
            if (!String.IsNullOrEmpty(result))
            {
                if (result.Length < 6)
                {
                    string end = "";
                    for (int i = 0; i < 6 - result.Length; i++)
                    {
                        end += "0";
                    }
                    result = result + end;
                    BLEPassEntry.Text = result;
                }
            }
        }

        private void AdminPassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool retry = false;
            string newValue = AdminPassEntry.Text;
            if (newValue.Length == 6)
            {
                AdminPassEntry.Text = newValue;
                AdminPassEntry.Unfocus();
            }
            else if (newValue.Length > 6)
            {
                newValue = newValue.Remove(6);
                retry = true;
            }
            if (!int.TryParse(newValue, out int _))
            {
                for (int i = 0; i < newValue.Length; i++)
                {
                    if (!int.TryParse(newValue.ToCharArray()[i].ToString(), out _))
                    {
                        retry = true;
                        newValue = newValue.Remove(i, 1);
                        break;
                    }
                }
            }
            if (retry)
            {
                AdminPassEntry.Text = newValue;
            }
        }

        private void AdminPassEntry_Completed(object sender, EventArgs e)
        {
            AdminPassEntry.Unfocus();
        }

        private void AdminPassEntry_Unfocused(object sender, FocusEventArgs e)
        {
            string result = AdminPassEntry.Text;
            if (!String.IsNullOrEmpty(result))
            {
                if (result.Length < 6)
                {
                    string start = "";
                    for (int i = 0; i < 6 - result.Length; i++)
                    {
                        start += "0";
                    }
                    result = start + result;
                    AdminPassEntry.Text = result;
                }
            }
        }

        private async void SerNumEntry_Focused(object sender, FocusEventArgs e)
        {
            await Task.Delay(10);
            SerNumEntry.CursorPosition = SerNumEntry.Text.Length;
        }

        private async void BLEPassEntry_Focused(object sender, FocusEventArgs e)
        {
            await Task.Delay(10);
            BLEPassEntry.CursorPosition = BLEPassEntry.Text.Length;
        }

        private async void AdminPassEntry_Focused(object sender, FocusEventArgs e)
        {
            await Task.Delay(10);
            AdminPassEntry.CursorPosition = AdminPassEntry.Text.Length;
        }
    }
}