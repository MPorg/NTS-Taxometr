using System;
using System.Collections.Generic;
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
                Action action = null;
                
                switch (button.CommandParameter)
                {
                    case "C":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.C);
                        _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.C, out action);
                        break;
                    case "OK":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);
                        _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.OK, out action);
                        break;
                    case "Up":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Up);
                        _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Up, out action);
                        break;
                    case "Down":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Down);
                        _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Down, out action);
                        break;
                    case "Num_1":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Num_1);
                        _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Num_1, out action);
                        break;
                    case "Num_2":
                        AppData.Provider.EmitButton(ProviderBLE.ButtonKey.Num_2);
                        _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Num_2, out action);
                        break;
                }

                //if (action != null) _lastKeysActions = new Dictionary<ProviderBLE.ButtonKey, Action>();
                action?.Invoke();
            }
        }


        private Dictionary<ProviderBLE.ButtonKey, Action> _lastKeysActions = new Dictionary<ProviderBLE.ButtonKey, Action>();
        public void SetMessage(string message, Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed, ProviderBLE.ButtonKey enableButtons)
        {
            EnableButtons(enableButtons);
            Message.Text = message;
            _lastKeysActions = onKeysPressed;
        }

        public void Clear()
        {
            EnableButtons(ProviderBLE.ButtonKey.Any);
            Message.Text = "";
            _lastKeysActions = new Dictionary<ProviderBLE.ButtonKey, Action>();

        }

        private void EnableButtons(ProviderBLE.ButtonKey enableButtons)
        {
            Debug.WriteLine($"Enabled buttons {enableButtons}");

            ButC.IsEnabled = (enableButtons.HasFlag(ProviderBLE.ButtonKey.C)) ? true : false;
            ButUp.IsEnabled = (enableButtons.HasFlag(ProviderBLE.ButtonKey.Up)) ? true : false;
            ButDown.IsEnabled = (enableButtons.HasFlag(ProviderBLE.ButtonKey.Down)) ? true : false;
            ButOk.IsEnabled = (enableButtons.HasFlag(ProviderBLE.ButtonKey.OK)) ? true : false;
            ButNum_1.IsEnabled = (enableButtons.HasFlag(ProviderBLE.ButtonKey.Num_1)) ? true : false;
            ButNum_2.IsEnabled = (enableButtons.HasFlag(ProviderBLE.ButtonKey.Num_2)) ? true : false;
        }
    }
}