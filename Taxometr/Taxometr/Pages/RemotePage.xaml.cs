using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Interfaces;
using Taxometr.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RemotePage : ContentPage
	{
		public RemotePage ()
		{
			InitializeComponent();
            AppData.AutoconnectionCompleated += OnAutoconnectionCompleated;
            AppData.ConnectionLost += OnConnectionLost;
/*
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Run(async () =>
                {
                    while (AppData.MainMenu == null)
                    {
                        await Task.Delay(1);
                    }

                    TryTransit -= AppData.MainMenu.OnCheck_TryTransit;
                    TryTransit += AppData.MainMenu.OnCheck_TryTransit;
                });
            });*/
		}

        //public event Action<Type> TryTransit;

        protected override void OnAppearing()
        {
            if (AppData.BLEAdapter.ConnectedDevices.Count > 0)
            {
                SwitchBan();
                /*try
                {
                    TryTransit?.Invoke(typeof(RemotePage));
                    if (AppData.MainMenu.SwitchingIsBusy) return;

                    AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Main, await AppData.Properties.GetAdminPassword());
                }
                catch { }*/
            }
            else SwitchBan(true);
/*
            if (AppData.MainMenu !=  null)
                AppData.MainMenu.Mode = MainMenu.MenuMode.Remote;*/
        }

        private void OnAutoconnectionCompleated()
        {
            SwitchBan();
        }

        private void OnConnectionLost()
        {
            SwitchBan(true);
        }

        private void SwitchBan(bool enable = false)
        {
            BanLayout.IsVisible = enable;
            if (!enable)
            {
                //DeviceName.Text = AppData.AutoConnectDevice.Name;
            }
        }

        private void OnDevicesBtnClicked(object sender, EventArgs e)
        {
			AppData.MainMenu.OpenDevicesPage();
        }

        private void OnStatusSCNOBtnClicked(object sender, EventArgs e)
        {
            //_provider.SentFlc(ProviderBLE.FlcType.RDY);
            AppData.Provider.SentScnoState(true);
        }

        private void OnEmitBtnClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button != null)
            {
                switch (button.CommandParameter)
                {
                    case "C":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.C);
                        break;
                    case "OK":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);
                        break;
                    case "Up":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Up);
                        break;
                    case "Down":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Down);
                        break;
                    case "Num_1":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Num_1);
                        break;
                    case "Num_2":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Num_2);
                        break;
                }
            }
        }
    }
}