using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Services;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrivePage : ContentPage
    {
        public DrivePage()
        {
            InitializeComponent();
            LoadingLayout.IsVisible = false;
            AppData.AutoconnectionCompleated += OnAutoconnectionCompleated;
            AppData.ConnectionLost += OnConnectionLost;
        }

        ProviderBLE.ShiftInfo _shiftState = ProviderBLE.ShiftInfo.Opened;
        int _checkState = 0;

        protected async override void OnAppearing()
        {
            if (AppData.BLEAdapter.ConnectedDevices.Count <= 0)
            {
                SwitchBan(true);

                LoadingLayout.IsVisible = false;
            }
            else
            {
                SwitchBan();

                try
                {

                    LoadingLayout.IsVisible = true;

                    AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                    AppData.Provider.SentShiftInfo(true);
                    await Task.Delay(2000);

                    AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword());
                    await Task.Delay(3000);

                    if (_shiftState == ProviderBLE.ShiftInfo.Closed)
                    {
                        AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                        AppData.Provider.SentTaxState(true);

                        await Task.Delay(500);
                        if (_checkState == 0)
                        {
                            LoadingLayout.IsVisible = false;

                            if (await AppData.MainMenu.DisplayAlert("Смена не открыта", "Открыть смену?", "Да", "Нет"))
                            {
                                AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);

                                LoadingLayout.IsVisible = true;
                                await Task.Delay(2000);
                                LoadingLayout.IsVisible = false;
                            }
                            else
                            {
                                AppData.Provider.EmitButton(ProviderBLE.ButtonKey.C);
                            }
                        }
                    }

                    //AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword());
                }
                catch { }
                finally
                {
                    LoadingLayout.IsVisible = false;
                }

            }
        }

        private void OnProvider_AnswerCompleate(byte cmd, Dictionary<string, string> keyValues)
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                if (cmd == ProviderExtentions.TaxState)
                {
                    AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                    if (keyValues.TryGetValue("checkState", out string checkStateStr))
                    {
                        if (int.TryParse(checkStateStr, out int checkState))
                        {
                            _checkState = checkState;
                            if (checkState == 0)
                            {
                                OpenCash.Text = "Открыть чек";
                                Debug.WriteLine("Чек не открыт");
                            }
                            else
                            {
                                OpenCash.Text = "Закрыть чек";
                                Debug.WriteLine("Чек открыт");
                            }
                        }
                    }
                }

                if (cmd == ProviderExtentions.ShiftState)
                {
                    string result = "";
                    if (keyValues.TryGetValue("Ошибка: ", out string errCode))
                    {
                        result += "Ошибка: " + errCode + "\r\n";
                    }
                    else
                    {
                        AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                        if (keyValues.TryGetValue("shiftState", out string shiftState))
                        {
                            if (int.TryParse(shiftState, out int shiftStateCode))
                            {
                                ProviderBLE.ShiftInfo shiftInfo = (ProviderBLE.ShiftInfo)shiftStateCode;

                                _shiftState = shiftInfo;

                                if (shiftInfo == ProviderBLE.ShiftInfo.Closed)
                                {
                                    result += "Смена не открыта";
                                }
                                else
                                {
                                    result += "Смена открыта";
                                }
                            }
                        }
                    }

                    AppData.ShowToast(result);
                }
            });
        }

        private void OnAutoconnectionCompleated()
        {
            if(IsFocused) OnAppearing();
        }

        private void OnConnectionLost()
        {
            if (IsFocused) OnAppearing();
        }

        private void SwitchBan(bool enable = false)
        {
            BanLayout.IsVisible = enable;
            InfoBtn.IsEnabled = !enable;
        }
        private void OnDevicesBtnClicked(object sender, EventArgs e)
        {
            AppData.MainMenu.OpenDevicesPage();
        }

        private void OnOpenShiftBtnClicked(object sender, EventArgs e)
        {
            AppData.Provider.OpenShift();
        }

        private async void OnDeposCashClicked(object sender, EventArgs e)
        {
            await AppData.GetDeposWithdrawBanner(ProviderBLE.CashMethod.Deposit);
        }
        private async void OnWithdrawCashClicked(object sender, EventArgs e)
        {
            await AppData.GetDeposWithdrawBanner(ProviderBLE.CashMethod.Withdrawal);
        }

        private void OnOpenCashClicked(object sender, EventArgs e)
        {
            switch (OpenCash.Text)
            {
                case "Открыть чек":
                    OpenCashClicked();
                    break;
                case "Закрыть чек":
                    CloseCashClicked();
                    break;
            }

            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            AppData.Provider.SentTaxState(true);
        }

        private void OpenCashClicked()
        {
            OpenCheckBanner banner = new OpenCheckBanner((bool flag, int sum) =>
            {
                if (flag)
                {
                    AppData.Provider.OpenCheck(sum, 0);
                }
            });

            Navigation.PushModalAsync(banner);
        }
        private void OnInfoBtnClicked(object sender, EventArgs e)
        {
            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            AppData.Provider.SentShiftInfo(true);
        }

        private void CloseCashClicked()
        {

        }
    }
}