using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        private void OnEnterBtnClicked(object sender, EventArgs e)
        {
            try
            {
                AppData.Provider.DeposWithdrawCash(_cashMethod, ulong.Parse(CashEntry.Text));
                Navigation.PopModalAsync();
            }
            catch
            {
                AppData.Debug.WriteLine("Не корректное значение");
            }
        }
    }
}