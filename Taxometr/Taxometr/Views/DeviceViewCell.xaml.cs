using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Taxometr.Data;
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

			public event Action<DeviceViewCellBinding> AutoconnectionChange;

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

            private async void FindDevice()
            {
				Guid id = DeviceId;
				AppData.Debug.WriteLine($"Find device by id {id}");
                while (true)
                {
                    IDevice device = AppData.BLEAdapter.BondedDevices.Where(x => x.Id == id).FirstOrDefault();
					if (AppData.BLEAdapter.ConnectedDevices.Count > 0 && AppData.BLEAdapter.ConnectedDevices[0].Id == device.Id) device = AppData.BLEAdapter.ConnectedDevices[0];
                    if (device != null)
                    {
						Device = device;
						break;
                    }
                    await Task.Delay(5000);
                }
            }
			
			public void UpdateDevice()
			{
				if (Device != null && AppData.BLEAdapter.ConnectedDevices.Count > 0)
				{
					IDevice connectedDevice = AppData.BLEAdapter.ConnectedDevices[0];
					if (Device.Id == connectedDevice.Id) Device = connectedDevice;
                }
			}
        }

		DeviceViewCellBinding _binding;

		double _baseHeight;

		public DeviceViewCell ()
		{
			InitializeComponent();
			_baseHeight = Height;
            FindBinding();
            StartTimer();
		}

        private void StartTimer()
        {
			Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
			{
				if (_binding != null) 
				{
					ConnectionStateLabel.Text = _binding.ConnectionState;
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
					}
					break;
				}
                await Task.Delay(500);
				i++;
            }
        }

		private void OpenContextMenu()
        {
			AppData.Debug.WriteLine("Click");
            _binding.IsOpened = !_binding.IsOpened;
            UpdateUI(_binding.IsOpened);
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
                Height = _baseHeight + 150;
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

        private void OnAutoConnectValue_Toggled(object sender, ToggledEventArgs e)
        {
			if (_binding != null)
			{
				bool val = e.Value;
				_binding.AutoConnect = val;
			}
        }
    }
}