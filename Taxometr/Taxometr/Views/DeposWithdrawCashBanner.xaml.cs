using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxometr.Data;
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

        private async void OnEnterBtnClicked(object sender, EventArgs e)
        {
            await Task.Delay(10);
            try
            {
                ulong sum = (ulong)(double.Parse(CashEntry.Text) * 100);
                AppData.Provider.DeposWithdrawCash(_cashMethod, sum);
                await Navigation.PopModalAsync();
            }
            catch
            {
                AppData.Debug.WriteLine("Не корректное значение");
            }
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
            //CashEntry.Text = CashEntry.Text.Replace(',', '.');
        }

        private void CashEntry_Completed(object sender, EventArgs e)
        {
            CashEntry.Unfocus();
        }
    }
}