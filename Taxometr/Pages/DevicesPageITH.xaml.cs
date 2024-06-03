namespace Taxometr.Pages;
using Taxometr.Bloetooth;

public partial class DevicesPageITH : ContentPage
{
	public DevicesPageITH()
	{
		InitializeComponent();
	}

    private async Task FillDevices()
    {

    }

    private async void ListOfDevices_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var device = (DeviceModel)e.SelectedItem;
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