using Plugin.BLE.Abstractions.Contracts;
using System;
using Taxometr.Fonts.IconFont;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;

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
            ((FontImageSource)ShowHidePassBtn.Source).Glyph = Icons.IconEyeHide;

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

        private void OnSerNumEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            bool retry = false;
            string newValue = e.NewTextValue;
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

        private void OnBLEPassEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            bool retry = false;
            string newValue = e.NewTextValue;
            if (newValue.Length == 6)
            {
                BLEPassEntry.Text = newValue;
                AdminPassEntry.Focus();
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

        private void OnAdminPassEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            bool retry = false;
            string newValue = e.NewTextValue;
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

        private void NameEntry_Completed(object sender, EventArgs e)
        {
            SerNumEntry.Focus();
        }

        private void SerNumEntry_Completed(object sender, EventArgs e)
        {
            BLEPassEntry.Focus();
        }

        private void BLEPassEntry_Completed(object sender, EventArgs e)
        {
            AdminPassEntry.Focus();
        }

        private void AdminPassEntry_Completed(object sender, EventArgs e)
        {
            AdminPassEntry.Unfocus();
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

        private void ShowHidePassBtn_Clicked(object sender, EventArgs e)
        {
            FontImageSource source = ShowHidePassBtn.Source as FontImageSource;

            if (source.Glyph == Icons.IconEyeHide)
            {
                source.Glyph = Icons.IconEyeShow;

                BLEPassEntry.IsPassword = false;
                AdminPassEntry.IsPassword = false;
            }
            else
            {
                source.Glyph = Icons.IconEyeHide;

                BLEPassEntry.IsPassword = true;
                AdminPassEntry.IsPassword = true;
            }
        }
    }
}