using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Interfaces;
using Taxometr.Services;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrivePage : ContentPage, ICheckedTransition
    {
        bool _checkTaxState = true;
        public DrivePage()
        {
            InitializeComponent();
            LoadingLayout.IsVisible = false;
            AppData.AutoconnectionCompleated += OnAutoconnectionCompleated;
            AppData.ConnectionLost += OnConnectionLost;
            OpenCash.Text = "Печатать чек";

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
            });
        }


        public event Action<Type> TryTransit;

        protected override void OnAppearing()
        {
            if (AppData.BLEAdapter.ConnectedDevices.Count <= 0)
            {
                SwitchBan(true);

                LoadingLayout.IsVisible = false;
            }
            else
            {
                SwitchBan();

                if (AppData.MainMenu.SwitchingIsBusy)
                {
                    TryTransit?.Invoke(typeof(DrivePage));
                    return; 
                }

                if (_checkTaxState)
                {
                    AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                    AppData.Provider.SentTaxState(true);
                }
                _checkTaxState = true; 
            }
        }

        private void CheckShift()
        {
            //Debug.WriteLine("___________________ Check Shift _____________________");

            try
            {
                LoadingLayout.IsVisible = true;
                AppData.MainMenu.SetBusy(true, typeof(DrivePage));
                AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                AppData.Provider.SentShiftInfo(true);

                //AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword());
            }
            catch { }
            finally
            {
            }
        }

        private void OnProvider_AnswerCompleate(byte cmd, Dictionary<string, string> keyValues)
        {
            Device.InvokeOnMainThreadAsync(async () =>
            {
                if (cmd == ProviderExtentions.TaxState)
                {
                    if (keyValues.TryGetValue("menuState", out string menuStateStr))
                    {
                        AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                        if (int.TryParse(menuStateStr, out int menuState))
                        {
                            Debug.WriteLine($"______________________ Menu state {menuState} _________________________");
                            if (menuState == 1)
                            {
                                return;
                            }
                            else
                            {
                                await AppData.MainMenu.GoToAsync("//Drive", false);
                                await Task.Delay(1000);
                                if (AppData.MainMenu == null || AppData.MainMenu.Mode != MainMenu.MenuMode.Drive) CheckShift();
                            }
                        }
                    }
                }

                if (cmd == ProviderExtentions.ShiftState)
                {
                    string result = "";
                    if (keyValues.TryGetValue("Ошибка: ", out string errCode))
                    {

                    }
                    else
                    {
                        if (keyValues.TryGetValue("shiftState", out string shiftState))
                        {
                            if (int.TryParse(shiftState, out int shiftStateCode))
                            {
                                AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                                ProviderBLE.ShiftInfo shiftInfo = (ProviderBLE.ShiftInfo)shiftStateCode;

                                if (shiftInfo == ProviderBLE.ShiftInfo.Closed)
                                {
                                    if (AppData.MainMenu != null && AppData.MainMenu.Mode == MainMenu.MenuMode.Drive)
                                    {
                                        result += "Смена не открыта";
                                    }
                                    else
                                    {
                                        LoadingLayout.IsVisible = true;
                                        AppData.MainMenu.SetBusy(true, typeof(DrivePage));
                                        await Task.Delay(2000);
                                        AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword(), true, 2);
                                        await Task.Delay(3000);

                                        if (AppData.MainMenu != null)
                                            AppData.MainMenu.Mode = MainMenu.MenuMode.Drive;

                                        LoadingLayout.IsVisible = false;

                                        if (await AppData.MainMenu.DisplayAlert("Смена не открыта", "Открыть смену?", "Да", "Нет"))
                                        {
                                            AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);

                                            LoadingLayout.IsVisible = true;
                                            await Task.Delay(2000);
                                            LoadingLayout.IsVisible = false;
                                            result += "Смена была не открыта\r\nВы открыли смену";
                                        }
                                        else
                                        {
                                            AppData.Provider.EmitButton(ProviderBLE.ButtonKey.C);
                                            result += "Смена не открыта";
                                        }


                                        AppData.MainMenu.SetBusy(false, typeof(DrivePage));
                                    }
                                }
                                else
                                {
                                    if (AppData.MainMenu != null && AppData.MainMenu.Mode == MainMenu.MenuMode.Drive)
                                    {
                                        result += "Смена открыта";
                                    }
                                    else
                                    {
                                        result += "Смена была открыта";

                                        LoadingLayout.IsVisible = true;
                                        AppData.MainMenu.SetBusy(true, typeof(DrivePage));
                                        await Task.Delay(2000);
                                        AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword(), true, 2);
                                        await Task.Delay(3000);
                                        AppData.MainMenu.SetBusy(false, typeof(DrivePage));
                                        LoadingLayout.IsVisible = false;
                                    }
                                }
                            }
                            AppData.ShowToast(result);
                        }
                    }
                }

                if (cmd == ProviderExtentions.CheckOpen)
                {
                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "0")
                        {
                            await Task.Delay(100);
                            Debug.WriteLine($"{_cashedCheckValues != null}");

                            if (_cashedCheckValues != null)
                            {
                                int initVal = _cashedCheckValues.initialValue;
                                int sum = _cashedCheckValues.initialValue;

                                int cashVal = _cashedCheckValues.cashValue;
                                int cardVal = _cashedCheckValues.cardValue;
                                int nomoneyVal = _cashedCheckValues.noMoneyValue;

                                if (cardVal > 0)
                                {
                                    sum = cardVal + cashVal;
                                    if (sum != initVal) return;

                                    initVal = cashVal;
                                }

                                else if (nomoneyVal > 0)
                                {
                                    sum = nomoneyVal + cashVal;
                                    if (sum != initVal) return;

                                    initVal = cashVal;
                                }



                                AppData.Provider.CloseCheck(initVal, cardVal, nomoneyVal, cashVal);
                            }
                        }
                    }
                }

                if (cmd == ProviderExtentions.CheckClose)
                {
                    string result = "";
                    if (keyValues.TryGetValue("surrender", out string surrender))
                    {
                        AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;

                        if (int.TryParse(surrender, out int sur))
                        {
                            if (sur > 0)
                            {
                                Debug.WriteLine($"sur = {sur}");
                                float s = sur;
                                s /= 100;
                                result += $"Сдача: {s}";


                                if (await AppData.MainMenu.DisplayAlert(result, "", "", "Ок"))
                                {
                                    await Task.Delay(10);
                                    AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);
                                }
                                else
                                {
                                    await Task.Delay(10);
                                    AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);
                                }
                            }
                        }
                    }
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
        PrintCheckBanner.CheckInfoEventArgs _cashedCheckValues = null;
        private void OnOpenCashClicked(object sender, EventArgs e)
        {
            _checkTaxState = false;
            PrintCheckBanner banner = new PrintCheckBanner(async (bool flag, PrintCheckBanner.CheckInfoEventArgs ea) =>
            {
                if (flag)
                {
                    _cashedCheckValues = ea;
                    await Task.Delay(100);
                    AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                    AppData.Provider.OpenCheck(ea.initialValue, 0);
                }
            });

            Navigation.PushModalAsync(banner);
        }

        private void OnInfoBtnClicked(object sender, EventArgs e)
        {
            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            AppData.Provider.SentShiftInfo(true);
        }
    }
}