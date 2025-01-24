﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Interfaces;
using Taxometr.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeposWithdrawCashBanner : ContentPage
    {
        private ProviderBLE.CashMethod _cashMethod;
        public DeposWithdrawCashBanner(ProviderBLE.CashMethod method, string placeholder)
        {
            InitializeComponent();
            string mt = "";
            switch (method)
            {
                case ProviderBLE.CashMethod.Deposit:
                    mt = "внесения";
                    break;
                case ProviderBLE.CashMethod.Withdrawal:
                    mt = "изъятия";
                    break;
            }
            Header.Text = $"Укажите сумму {mt}";
            CashEntry.Placeholder = placeholder;
            _cashMethod = method;
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(500);
            CashEntry.Focus();
        }

        private void OnEnterBtnClicked(object sender, EventArgs e)
        {
            CashEntry_Completed(CashEntry, null);
        }

        private void OnCancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void CashEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = e.NewTextValue;
            List<char> chars = txt.ToCharArray().ToList();
            int i = 0;
            string result = "";
            for (int j = 0; j < chars.Count; j++)
            {
                if (j > 2 && chars[j - 3] == ',')
                {
                    continue;
                }
                if (chars[j] == ',')
                {
                    if (j == 0) result += "0";

                    i++;
                    if (i > 1)
                    {
                        continue;
                    }
                }
                result += chars[j];
            }
            CashEntry.Text = result;
        }

        private void CashEntry_Unfocused(object sender, FocusEventArgs e)
        {
        }

        private void CashEntry_Completed(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;

            bool focus = true;
            if (e is OnCompleateEventArgs)
            {
                var ea = e as OnCompleateEventArgs;
                focus = ea.focusNext;
            }

            if (string.IsNullOrEmpty(entry.Text)) entry.Text += "0";

            string txt = "";
            char[] chars = entry.Text.ToCharArray();

            if (!entry.Text.Contains(','))
            {
                entry.Text += ",";
                entry.Text += "00";
            }
            else
            {
                for (int i = 0; i < entry.Text.Length; i++)
                {
                    if (i == entry.Text.Length - 1)
                    {
                        if (chars[i] == ',')
                        {
                            txt += "00";
                        }
                        else if (chars[i - 1] == ',')
                        {
                            txt += chars[i];
                            txt += "0";
                            continue;
                        }
                    }
                    txt += chars[i];
                }
                entry.Text = txt;
            }

            CashEntry.Unfocus();
            DependencyService.Get<IKeyboard>().Hide();
            Compleate();
        }


        private async void Compleate()
        {
            string initValueStr = CashEntry.Text;

            while (initValueStr.Contains(","))
            {
                initValueStr = initValueStr.Remove(initValueStr.IndexOf(','), 1);
            }
            int initValue = int.Parse(initValueStr);

            try
            {
                AppData.Provider.DeposWithdrawCash(_cashMethod, (ulong)initValue);
                await Navigation.PopModalAsync();
            }
            catch
            {
                AppData.Debug.WriteLine("Не корректное значение");
            }
        }

    }
}