using Plugin.BLE.Abstractions.Contracts;
using System;
using Taxometr.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateDevicePrefab : ContentPage
	{
		private IDevice _device;
		private Action<bool, string, string, string, string, bool> _result;

		public CreateDevicePrefab (IDevice device, Action<bool, string, string, string, string, bool> result)
		{
			InitializeComponent();
			_device = device;
			_result = result;
			NameEntry.Placeholder = device.Name;
		}

        private void OnBackClicked(object sender, EventArgs e)
        {
			_result(false, "", "", "", "", false);
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            string serNum = "00000000";
            string blePass = "000000";
            string adminPass = "000001";
            string customName = _device.Name;
            bool autoconnect = AutoConnectTogle.IsToggled;

            if (!String.IsNullOrEmpty(SerNumEntry.Text)) serNum = SerNumEntry.Text;
            if (!String.IsNullOrEmpty(BLEPassEntry.Text)) blePass = BLEPassEntry.Text;
            if (!String.IsNullOrEmpty(AdminPassEntry.Text)) adminPass = AdminPassEntry.Text;
            if (!String.IsNullOrEmpty(NameEntry.Text)) customName = NameEntry.Text;

            _result(true, serNum, blePass, adminPass, customName, autoconnect);
        }

        private void OnNameEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            AppData.Debug.WriteLine($"{e.OldTextValue} => {e.NewTextValue}");
        }

        private void OnSerNumEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            AppData.Debug.WriteLine($"{e.OldTextValue} => {e.NewTextValue}");
        }

        private void OnBLEPassEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            AppData.Debug.WriteLine($"{e.OldTextValue} => {e.NewTextValue}");
        }

        private void OnAdminPassEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            AppData.Debug.WriteLine($"{e.OldTextValue} => {e.NewTextValue}");
        }

        private void OnAutoConnectTogle_Toggled(object sender, ToggledEventArgs e)
        {
            AppData.Debug.WriteLine($"Autoconnect {e.Value}");
        }
    }
}