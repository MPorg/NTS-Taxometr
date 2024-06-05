using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxometr.DataBase.Tmp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceView : ContentPage
	{
		private BluetoothDeviceModel device;

		public DeviceView (BluetoothDeviceModel device)
		{
			InitializeComponent ();
			this.device = device;
            Start();
		}

        private async void Start()
        {
			Title = device.Name;
			if (await TestConnectionAsync())
			{
				ConnectBtn.Text = "Подключено";
				ConnectBtn.IsEnabled = false;
			}
        }

		private async Task<bool> TestConnectionAsync()
		{
			if (AppData.Connection != null && AppData.ConnectedDevice == device) return true;
			return false;
		}

        private async void ConnectBtn_Clicked(object sender, EventArgs e)
        {
			if (await TryConnectAsync())
			{

			}
        }

		private async Task<bool> TryConnectAsync()
		{
			ConnectBtn.Text = "Подключение";
			ConnectBtn.IsEnabled = false;
            IBluetoothConnection connection = AppData.Adapter.CreateConnection(device);
			if (await connection.RetryConnectAsync(2))
            {
                ConnectBtn.Text = "Подключено";
                ConnectBtn.IsEnabled = false;
				AppData.Connection = connection;
				AppData.ConnectedDevice = device;
				return true;
            }
            ConnectBtn.Text = "Подключиться";
            ConnectBtn.IsEnabled = true;
            return false;
		}

        private async void DisconectBtn_Clicked(object sender, EventArgs e)
        {
			if (await TestConnectionAsync())
			{
				AppData.Connection.Dispose();
				AppData.Connection = null;
				AppData.ConnectedDevice = null;
                ConnectBtn.Text = "Подключиться";
                ConnectBtn.IsEnabled = true;
            }
        }
    }
}