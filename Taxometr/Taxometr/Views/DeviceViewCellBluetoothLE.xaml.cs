using Plugin.BluetoothLE;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Taxometr.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceViewCellBluetoothLE : ViewCell
	{
		public class DeviceViewCellBindingBluetoothLE : BindableObject
		{
			public IDevice Device { get; private set; }
			public string Name { get => Device.Name; }

			public bool Connecting { get; set; } = false;
			public string ConnectionState
			{ 
				get
				{
					if (Connecting) return "Подключение" + AppData.Dots;
                    switch (Device.Status)
					{
						case ConnectionStatus.Connected:
							return "Подключено";
						case ConnectionStatus.Connecting:
                            return "Подключение" + AppData.Dots;
						case ConnectionStatus.Disconnected:
							return "Не подключено";
						case ConnectionStatus.Disconnecting:
							return "Отключение" + AppData.Dots;
						default:
							return "<?>";
					}
				} 
			}

            public ICommand ShowButtons { get; set; }
            public ICommand HideButtons { get; set; }

			private bool _isShowed = false;
			internal void SwitchButtonsState()
			{
				_isShowed = !_isShowed;
			}
			public DeviceViewCellBindingBluetoothLE(IDevice device)
			{
				Device = device;
			}
        }

        DeviceViewCellBindingBluetoothLE _binding;

		public DeviceViewCellBluetoothLE()
		{
			InitializeComponent();
			Debug.WriteLine("DeviceViewCell Initializing!");
            FindBinding();
		}

        private async void FindBinding()
        {
			while (true)
			{
				Debug.WriteLine("Find BindingContext");
                bool result = false;
				if (BindingContext is DeviceViewCellBindingBluetoothLE binding)
				{
					result = true;
					binding.ShowButtons = new Command(ShowButtons);
					binding.HideButtons = new Command(HideButtons);
					_binding = binding;
					Debug.WriteLine($"Binding is DeviceViewCellBinding ? {result}");
					break;
				}
                Debug.WriteLine($"Binding is DeviceViewCellBinding ? {result}");
                await Task.Delay(500);
            }
        }

        public void ShowButtons()
		{
			Buttons.IsVisible = true;
            _binding.SwitchButtonsState();
        }

		public void HideButtons()
        {
            Buttons.IsVisible = false;
            _binding.SwitchButtonsState();
        }
	}
}