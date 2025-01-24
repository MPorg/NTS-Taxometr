using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private enum ShiftStateInfo
        {
            None,
            Opened,
            Closed
        }

        private ShiftStateInfo _shiftStateInfo = ShiftStateInfo.None;

        bool _checkTaxState = true;
        bool _changeCheck = true;
        public DrivePage()
        {
            InitializeComponent();
            LoadingLayout.IsVisible = false;
            AppData.AutoconnectionCompleated += OnAutoconnectionCompleated;
            AppData.ConnectionLost += OnConnectionLost;

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

        private bool _timerIsWork = false;

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

                CheckStats();

                if(_changeCheck) _checkTaxState = true;

                _changeCheck = true;

                if (!_timerIsWork)
                {
                    _timerIsWork = true;
                    Device.StartTimer(TimeSpan.FromSeconds(10d), new Func<bool>(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            CheckStats();
                        });
                        return true;
                    }));
                }
            }
        }

        private void CheckStats()
        {
            if (_checkTaxState && AppData.MainMenu.CurrentPage is DrivePage)
            {
                AppData.Provider.AnswerCompleate += CheckShiftLocalAnswer;
                AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                AppData.Provider.SentTaxState(true);
            }
            //_checkTaxState = true;
        }

        private void CheckShiftLocalAnswer(byte cmd, Dictionary<string, string> keyValues)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (cmd == ProviderExtentions.ShiftState)
                {
                    if (keyValues.TryGetValue("shiftState", out string shiftState))
                    {
                        if (int.TryParse(shiftState, out int shiftStateCode))
                        {
                            AppData.Provider.AnswerCompleate -= CheckShiftLocalAnswer;
                            ProviderBLE.ShiftInfo shiftInfo = (ProviderBLE.ShiftInfo)shiftStateCode;

                            if (shiftInfo == ProviderBLE.ShiftInfo.Closed)
                            {
                                SetShift(false);
                            }
                            else
                            {
                                SetShift(true);
                            }
                        }
                    }
                }
                if (cmd == ProviderExtentions.TaxState)
                {
                    if (keyValues.TryGetValue("checkState", out string checkState))
                    {
                        AppData.Provider.AnswerCompleate -= CheckShiftLocalAnswer;

                        if (_checkTaxState && AppData.MainMenu.CurrentPage is DrivePage)
                        {
                            if (_shiftStateInfo == ShiftStateInfo.None)
                            {
                                await Task.Delay(1000);
                                OpenShiftBtn.IsEnabled = false;
                                WithdrawCash.IsEnabled = false;
                                DeposCash.IsEnabled = false;
                                OpenCash.IsEnabled = false;
                                AppData.Provider.AnswerCompleate += CheckShiftLocalAnswer;
                                AppData.Provider.SentShiftInfo(true);
                            }
                            else
                            {
                                await Task.Delay(1000);
                                AppData.Provider.AnswerCompleate += CheckShiftLocalAnswer;
                                AppData.Provider.SentShiftInfo(true);
                            }
                        }

                        SetCheck(checkState == "1");
                    }
                }
            });
        }

        private void CheckShift()
        {
            try
            {
                LoadingLayout.IsVisible = true;
                _checkTaxState = false;
                AppData.MainMenu.SetBusy(true, typeof(DrivePage));
                AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                AppData.Provider.SentShiftInfo(true);
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
                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "08")
                        {
                            if (await AppData.MainMenu.DisplayAlert("Ошибка", "Таксометр заблокирован", "<ОК>", "Понятно"))
                            {
                                AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);
                                await Task.Delay(1000);
                                AppData.Provider.SentTaxInfo(true);
                            }
                        }
                    }
                    if (keyValues.TryGetValue("menuState", out string menuStateStr))
                    {
                        AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                        if (int.TryParse(menuStateStr, out int menuState))
                        {
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
                    if (keyValues.TryGetValue("checkState", out string checkState))
                    {
                        //AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                        SetCheck(checkState == "1");
                    }
                }

                if (cmd == ProviderExtentions.ShiftState)
                {
                    string result = "";
                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "08")
                        {
                            if (await AppData.MainMenu.DisplayAlert("Ошибка", "Таксометр заблокирован", "<ОК>", "Понятно"))
                            {
                                AppData.Provider.EmitButton(ProviderBLE.ButtonKey.OK);
                                await Task.Delay(1000);
                                AppData.Provider.SentShiftInfo(true);
                            }
                        }
                    }
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
                                    _checkTaxState = true;
                                    result += "Смена не открыта";
                                    SetShift(false);
                                    //AppData.ShowToast(result);
                                }
                                else
                                {
                                    LoadingLayout.IsVisible = true;
                                    AppData.MainMenu.SetBusy(true, typeof(DrivePage));
                                    await Task.Delay(2000);
                                    AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword(), true, 2);
                                    await Task.Delay(3000);

                                    if (AppData.MainMenu != null)
                                    {
                                        AppData.MainMenu.Mode = MainMenu.MenuMode.Drive;
                                    }


                                    Action OnCPressed = new Action(() =>
                                    {
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {

                                            result += "Смена не открыта";
                                            await Task.Delay(500);
                                            SetShift(false);

                                            AppData.MainMenu.SetBusy(false, typeof(DrivePage));
                                            //AppData.ShowToast(result);
                                            AppData.MainMenu.CleareRemote();
                                            await AppData.MainMenu.GoToAsync("//Drive", true);
                                            LoadingLayout.IsVisible = false;
                                            _checkTaxState = true;
                                        });
                                    });

                                    Action OnOkPressed = new Action(() =>
                                    {
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            Action OnOkPressedInner = new Action(() =>
                                            {
                                                Device.BeginInvokeOnMainThread(async () =>
                                                {
                                                    await Task.Delay(1000);
                                                    result += "Смена была не открыта\r\nВы открыли смену";
                                                    SetShift(true);
                                                    AppData.MainMenu.SetBusy(false, typeof(DrivePage));
                                                    AppData.MainMenu.CleareRemote();
                                                    await AppData.MainMenu.GoToAsync("//Drive", true);
                                                    LoadingLayout.IsVisible = false;
                                                    _checkTaxState = true;
                                                    //AppData.ShowToast(result);
                                                });
                                            });

                                            var actionsDictInner = new Dictionary<ProviderBLE.ButtonKey, Action>()
                                            {
                                                {ProviderBLE.ButtonKey.OK, OnOkPressedInner},
                                                {ProviderBLE.ButtonKey.C, OnCPressed}
                                            };

                                            await AppData.MainMenu.GoToRemote(true, "Подтвердить Дату/Время\r\n[OK] - Да, [C] - Не открывать смену", actionsDictInner, ProviderBLE.ButtonKey.OK | ProviderBLE.ButtonKey.C | ProviderBLE.ButtonKey.Num_1 | ProviderBLE.ButtonKey.Num_2);
                                        });

                                    });

                                    var actionsDict = new Dictionary<ProviderBLE.ButtonKey, Action>()
                                    {
                                        {ProviderBLE.ButtonKey.OK, OnOkPressed},
                                        {ProviderBLE.ButtonKey.C, OnCPressed}
                                    };

                                    AppData.MainMenu.SetBusy(true, typeof(RemotePage));
                                    await AppData.MainMenu.GoToRemote(true, "Смена не открыта\r\nОткрыть смену?\r\n[OK] - Да, [C] - Нет", actionsDict, ProviderBLE.ButtonKey.OK | ProviderBLE.ButtonKey.C);
                                }
                            }
                            else
                            {
                                if (AppData.MainMenu != null && AppData.MainMenu.Mode == MainMenu.MenuMode.Drive)
                                {
                                    result += "Смена открыта";
                                    _checkTaxState = true;
                                    //AppData.ShowToast(result);
                                }
                                else
                                {
                                    result += "Смена была открыта";
                                    LoadingLayout.IsVisible = true;
                                    AppData.MainMenu.SetBusy(true, typeof(DrivePage));
                                    await Task.Delay(2000);
                                    AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword(), true, 2);
                                    await Task.Delay(1000);
                                    AppData.MainMenu.SetBusy(false, typeof(DrivePage));
                                    LoadingLayout.IsVisible = false;
                                    _checkTaxState = true;
                                    //AppData.ShowToast(result);
                                }
                                SetShift(true);
                            }
                        }
                    }
                }

                if (cmd == ProviderExtentions.CheckOpen)
                {
                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "00")
                        {
                            AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;

                            await Task.Delay(500);

                            _checkTaxState = true;
                        }
                    }
                }

                if (cmd == ProviderExtentions.CheckClose)
                {
                    string result = "";

                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "00")
                        {
                            AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                            _checkTaxState = false;
                            _changeCheck = false;
                        }
                    }

                    if (keyValues.TryGetValue("surrender", out string surrender))
                    {
                        if (int.TryParse(surrender, out int sur))
                        {
                            if (sur != 0)
                            {
                                float s = sur;
                                s /= 100;
                                result = $"Сдача: {s}р.";
                            }
                            else
                            {
                                result = "Готово";
                            }
                        }
                        else
                        {
                            result = "Готово";
                        }
                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        Action OnOkPressed = new Action(() =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await Task.Delay(500);
                                AppData.MainMenu.CleareRemote();
                                AppData.MainMenu.SetBusy(false, typeof(DrivePage));
                                await AppData.MainMenu.GoToAsync("//Drive", true);
                                _checkTaxState = true;
                            });
                        });

                        var actionsDict = new Dictionary<ProviderBLE.ButtonKey, Action>()
                        {
                            {ProviderBLE.ButtonKey.OK, OnOkPressed}
                        };

                        AppData.MainMenu.SetBusy(true, typeof(RemotePage));
                        await AppData.MainMenu.GoToRemote(true, result, actionsDict, ProviderBLE.ButtonKey.OK);
                    }
                }

                if (cmd == ProviderExtentions.CheckBreak)
                {
                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "00")
                        {
                            AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;

                            await Task.Delay(1000);

                            _checkTaxState = true;
                        }
                    }
                }

                if (cmd == ProviderExtentions.CheckDuplicate)
                {
                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "00")
                        {
                            AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;

                            await Task.Delay(1000);

                            _checkTaxState = true;
                        }
                    }
                }

                if (cmd == ProviderExtentions.CashDeposWithdraw)
                {
                    if (keyValues.TryGetValue("errCode", out string errCode))
                    {
                        if (errCode == "00")
                        {
                            AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;

                            await Task.Delay(1000);

                            _checkTaxState = true;
                        }
                    }
                }
            });
        }

        private void SetShift(bool shiftIsOpen)
        {
            OpenShiftBtn.IsEnabled = !shiftIsOpen;
            WithdrawCash.IsEnabled = shiftIsOpen;
            DeposCash.IsEnabled = shiftIsOpen;
            OpenCash.IsEnabled = shiftIsOpen;

            if (shiftIsOpen)
                _shiftStateInfo = ShiftStateInfo.Opened;
            else
                _shiftStateInfo = ShiftStateInfo.Closed;
        }

        private void SetCheck(bool checkIsOpened)
        {
            if (checkIsOpened) SetShift(true);

            OpenCash.IsEnabled = !checkIsOpened;
            OpenCash.IsVisible = !checkIsOpened;

            CloseCashLayout.IsEnabled = checkIsOpened;
            CloseCashLayout.IsVisible = checkIsOpened;
        }

        private void OnAutoconnectionCompleated()
        {
            if (IsFocused) OnAppearing();
        }

        private void OnConnectionLost()
        {
            if (IsFocused) OnAppearing();
        }

        private void SwitchBan(bool enable = false)
        {
            BanLayout.IsVisible = enable;
            //InfoBtn.IsEnabled = !enable;
        }

        private void OnDevicesBtnClicked(object sender, EventArgs e)
        {
            AppData.MainMenu.OpenDevicesPage();
        }

        private async void OnOpenShiftBtnClicked(object sender, EventArgs e)
        {
            AppData.Provider.OpenShift();
            await Task.Delay(1000);
            AppData.Provider.AnswerCompleate += CheckShiftLocalAnswer; ;
            AppData.Provider.SentShiftInfo(true);
        }

        private async void OnDeposCashClicked(object sender, EventArgs e)
        {
            _checkTaxState = false;
            _changeCheck = false;

            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            await AppData.GetDeposWithdrawBanner(ProviderBLE.CashMethod.Deposit);
        }

        private async void OnWithdrawCashClicked(object sender, EventArgs e)
        {
            _checkTaxState = false;
            _changeCheck = false;

            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            await AppData.GetDeposWithdrawBanner(ProviderBLE.CashMethod.Withdrawal);
        }

        private async void OnOpenCashClicked(object sender, EventArgs e)
        {
            _checkTaxState = false;
            _changeCheck = false;

            OpenCheckBanner banner = new OpenCheckBanner(async (bool flag, OpenCheckBanner.CheckInfoEventArgs ea) =>
            {
                if (flag)
                {
                    await AppData.Properties.SaveLastCashSum(ea.initialValue);
                    await Task.Delay(100);
                    AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                    AppData.Provider.OpenCheck(ea.initialValue, ea.prePayValue);
                }
            });
            await Navigation.PushModalAsync(banner);
        }

        private async void OnCloseCashClicked(object sender, EventArgs e)
        {
            _checkTaxState = false;
            _changeCheck = false;

            CloseCheckBanner banner = new CloseCheckBanner(async (bool flag, CloseCheckBanner.CheckInfoEventArgs ea) =>
            {
                await Task.Delay(100);
                if (ea.cardValue > 0)
                {
                    ea.initialValue -= ea.cardValue;
                }

                AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                AppData.Provider.CloseCheck(ea.initialValue, ea.cardValue, ea.noMoneyValue, ea.cashValue, 50);

            }, await AppData.Properties.GetLastCashSum());

            await Navigation.PushModalAsync(banner);
        }
        private void OnBreakCashClicked(object sender, EventArgs e)
        {
            _checkTaxState = false;
            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            AppData.Provider.BreakCheck();
        }

        private void OnInfoBtnClicked(object sender, EventArgs e)
        {
            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            AppData.Provider.SentShiftInfo(true);
        }

        private void OnCopyCheckClicked(object sender, EventArgs e)
        {
            _checkTaxState = false;
            AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
            AppData.Provider.CopyCheck();
        }
    }
}