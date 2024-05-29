using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Taxometr.Bloetooth;
namespace Taxometr.Pages;

public partial class TransiverPage : ContentPage
{
    const int BufferSize = 1;
    const int DefaultOffset = 0;
    private DeviceModel Device;

	public TransiverPage()
	{
		InitializeComponent();
    }

    public TransiverPage(DeviceModel device)
    {
        InitializeComponent();
        if (device == null) { }
        else
        {
            this.Title = device.Name;
            this.Device = device;
        }
    }

    private async void Connect_TransmitButton_Clicked(object sender, EventArgs e)
    {
        var adapter = CrossBluetoothLE.Current.Adapter;
        Guid[] guids = new Guid[1];
        guids[0] = Device.Id;
        IDevice device = adapter.GetKnownDevicesByIds(guids).FirstOrDefault();
        if (device == null)
        { }
        else
        {
            try
            {
                await adapter.ConnectToDeviceAsync(device);

                var services = await device.GetServicesAsync();
                var service = services.FirstOrDefault();
                ICharacteristic characteristic = (await service?.GetCharacteristicsAsync()).FirstOrDefault();

                if (characteristic != null)
                {
                    byte[] bytes = new byte[BufferSize] { (byte)StepperDigit.Value };
                    try
                    {
                        await characteristic.WriteAsync(bytes);
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("ERROR", "Can not send data.", "Close");
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("ERROR", "Can not to connect.", "Close");
            }
        }
    }

    private async void ReciveButton_Clicked(object sender, EventArgs e)
    {
        /*BluetoothDeviceModel device = (BluetoothDeviceModel)BindingContext;
        if (device != null)
        {
            IBluetoothAdapter adapter = DependencyService.Resolve<IBluetoothAdapter>();
            using (IBluetoothConnection connection = adapter.CreateConnection(device))
            {
                if (await connection.RetryConnectAsync(retriesCount: 2))
                {
                    byte[] buffer = new byte[BufferSize];
                    if (!(await connection.RetryReciveAsync(buffer, DefaultOffset, buffer.Length)).Succeeded)
                    {
                        await DisplayAlert("ERROR", "Can not recive data.", "Close");
                    }
                    else
                    {
                        StepperDigit.Value = buffer.FirstOrDefault();
                    }
                }
                else
                {
                    await DisplayAlert("ERROR", "Can not to connect.", "Close");
                }
            }
        }*/
    }
}