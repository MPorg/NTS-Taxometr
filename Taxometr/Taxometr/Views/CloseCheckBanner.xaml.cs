using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseCheckBanner : ContentPage
    {
        private Action<bool, CheckInfoEventArgs> _onClose;

        public class CheckInfoEventArgs : EventArgs
        {
            public int initialValue;
            public int cashValue;
            public int cardValue;
            public int noMoneyValue;

            /// <summary>
            /// Create empty args whith 0 values
            /// </summary>
            public CheckInfoEventArgs() : this(0, 0, 0, 0) { }

            /// <summary>
            /// Create the CheckInfoEventArgs
            /// </summary>
            /// <param name="initialValue"></param>
            /// <param name="cashValue"></param>
            /// <param name="cardValue"></param>
            /// <param name="noMoneyValue"></param>
            public CheckInfoEventArgs(int initialValue, int cashValue, int cardValue, int noMoneyValue)
            {
                this.initialValue = initialValue;
                this.cashValue = cashValue;
                this.cardValue = cardValue;
                this.noMoneyValue = noMoneyValue;
            }
        }

        public CloseCheckBanner(Action<bool, CheckInfoEventArgs> onClose, int placeholder = 0)
        {
            InitializeComponent();
            _onClose = onClose;
            if (placeholder != 0)
            {
                string pls = "";
                char[] chars = placeholder.ToString().ToCharArray();

                for (int i = 0; i < chars.Length; i++)
                {
                    if (i == chars.Length - 2) pls += ",";
                    pls += chars[i];
                }
                StartSumEntry.Text = pls;
            }
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(500);

            StartSumEntry.Focus();
            if (!string.IsNullOrEmpty(StartSumEntry.Text))
            {
                StartSumEntry.CursorPosition = 0;
                StartSumEntry.SelectionLength = StartSumEntry.Text.Length;
            }
        }

        private void OnCancelBtn_Clicked(object sender, EventArgs e)
        {
            _onClose?.Invoke(false, new CheckInfoEventArgs());
            Navigation.PopModalAsync();
        }

        private void OnOkBtn_Clicked(object sender, EventArgs e)
        {
            OnEntry_Completed(StartSumEntry, e);
            OnEntry_Completed(PayCashEntry, e);
            OnEntry_Completed(PayCardEntry, e);
        }

        private void OnEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;

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
            entry.Text = result;
        }

        private void OnEntry_Completed(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;

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

            if (entry == StartSumEntry)
            {
                PayCashEntry.Focus();
            }
            else if (entry == PayCashEntry)
            {
                PayCardEntry.Focus();
            }
            else if (entry == PayCardEntry)
            {
                Compleate();
            }
        }

        private void Compleate()
        {
            string initValueStr = StartSumEntry.Text;
            string payCashValueStr = PayCashEntry.Text;
            string payCardValueStr = PayCardEntry.Text;

            while (initValueStr.Contains(","))
            {
                initValueStr = initValueStr.Remove(initValueStr.IndexOf(','), 1);
            }
            while (payCashValueStr.Contains(","))
            {
                payCashValueStr = payCashValueStr.Remove(payCashValueStr.IndexOf(','), 1);
            }
            while (payCardValueStr.Contains(","))
            {
                payCardValueStr = payCardValueStr.Remove(payCardValueStr.IndexOf(','), 1);
            }

            int initValue = int.Parse(initValueStr);
            int payCashValue = int.Parse(payCashValueStr);
            int payCardValue = int.Parse(payCardValueStr);

            try
            {
                _onClose?.Invoke(true, new CheckInfoEventArgs(initValue, payCashValue, payCardValue, 0));
            }
            catch
            {
                _onClose?.Invoke(false, new CheckInfoEventArgs());
            }
            finally
            {
                Navigation.PopModalAsync();
            }
        }
    }
}