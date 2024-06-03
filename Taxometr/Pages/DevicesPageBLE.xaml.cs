using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Taxometr.Bloetooth;
namespace Taxometr.Pages;

public partial class DevicesPageBLE : ContentPage
{
	public DevicesPageBLE()
	{
		InitializeComponent();
        var ble = CrossBluetoothLE.Current;
        FillDevices();
        ble.StateChanged += (s, a) =>
        {
            FillDevices();
        };
    }

    private async Task FillDevices()
    {
        if (CrossBluetoothLE.Current.State == BluetoothState.Off)
        {
            await DisplayAlert("Оповещение", "Включите Bluetooth", "ОК");
        }
        var adapter = CrossBluetoothLE.Current.Adapter;
        var devices = new List<IDevice>();
        devices = adapter.BondedDevices.ToList();
        List<DeviceModel> deviceModels = new List<DeviceModel>();

        foreach (IDevice d in devices)
        {
            DeviceModel deviceModel = new DeviceModel(d.Name, d.Id, d.IsConnectable);
            deviceModels.Add(deviceModel);
        }
        ListOfDevices.ItemsSource = deviceModels;
    }

    private async void ListOfDevices_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var device  = (DeviceModel)e.SelectedItem;
        if (device != null)
        {
            await Navigation.PushAsync(new TransiverPage(device));
        }

        ListOfDevices.SelectedItem = null;
    }

    private async void RefreshPage(object sender, EventArgs e)
    {
        await FillDevices();
        await Task.Delay(1000);
        Refresh.IsRefreshing = false;
    }
}