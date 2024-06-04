using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    public enum ButtonType
    {
        Connect,
        Send
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceViewPage : ContentPage
    {
        private BluetoothDeviceModel device;
        private IBluetoothAdapter adapter;
        private IBluetoothConnection BluetoothConnection;
        private int bufferSize = 1;
        private const int offset = 0;

        private ButtonType buttonType = ButtonType.Connect;

        public DeviceViewPage()
        {
            InitializeComponent();
        }
        public DeviceViewPage(BluetoothDeviceModel device)
        {
            InitializeComponent();
            this.device = device;
            Start();
        }

        private async void Start()
        {
            Title = device.Name;
            adapter = DependencyService.Resolve<IBluetoothAdapter>();
            BluetoothConnection = await Connect();
        }

        private async void ConnectAndSendButton_Clicked(object sender, EventArgs e)
        {
            switch (buttonType)
            {
                case ButtonType.Connect:
                    BluetoothConnection = await Connect();
                    break;
                case ButtonType.Send:
                    if (String.IsNullOrEmpty(Message.Text))
                    {
                        if (await DisplayAlert("Внимание!", "Невозможно отправить пустую строку. Введите сообщение", "ОК", "Закрыть"))
                        {
                            Message.Focus();
                        }
                    }
                    else
                    {
                        await SendData(Message.Text);
                    }
                    break;
            }    
        }

        private void SwitchButtonState()
        {
            switch (buttonType)
            {
                case ButtonType.Connect:
                    buttonType = ButtonType.Send;
                    ConnectAndSendButton.Text = "Отправить";
                    ConnectAndSendButton.IsEnabled = true;
                    break;
                case ButtonType.Send:
                    buttonType = ButtonType.Connect;
                    ConnectAndSendButton.Text = "Подключиться";
                    ConnectAndSendButton.IsEnabled = true;
                    break;
            }
        }
        
        private async Task<IBluetoothConnection> Connect()
        {
            ConnectAndSendButton.IsEnabled = false;
            ConnectAndSendButton.Text = "Подключение";
            IBluetoothConnection connection = null;
            try
            {
                using (connection = adapter.CreateConnection(device))
                {
                    if(await connection.RetryConnectAsync(retriesCount: 2))
                    {
                        SwitchButtonState();
                    }
                    else
                    {
                        throw new Exception();
                    }
                };
            }
            catch (Exception _)
            {
                await DisplayAlert("Ошибка", $"Не удалось подключиться к {device.Address}", "ОК");
                ConnectAndSendButton.IsEnabled = true;
                ConnectAndSendButton.Text = "Подключиться";
            }
            return connection;
        }

        private async Task SendData(string data)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                if (!await BluetoothConnection.RetryTransmitAsync(buffer, offset, buffer.Length))
                {
                    throw new Exception();
                }
            }
            catch (Exception ex) { await DisplayAlert("Ошибка", "Не удалось отправить данные", "ОК"); }
        }

        private async Task<string> LoadData()
        {
            string data = "";
            try
            {

            }
            catch { await DisplayAlert("Ошибка", "Не удалось получить данные", "ОК"); }
            return data;
        }
    }
}
