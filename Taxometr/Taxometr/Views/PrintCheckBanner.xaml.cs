using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintCheckBanner : ContentPage
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

        public PrintCheckBanner(Action<bool, CheckInfoEventArgs> onClose)
        {
            InitializeComponent();
            _onClose = onClose;
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(500);

            StartSumEntry.Focus();

        }

        private void OnCancelBtn_Clicked(object sender, EventArgs e)
        {
            _onClose?.Invoke(false, new CheckInfoEventArgs());
            Navigation.PopModalAsync();
        }

        private void OnOkBtn_Clicked(object sender, EventArgs e)
        {
            string[] values = new string[4]
            {
                StartSumEntry.Text,
                PayCashEntry.Text,
                PayCardEntry.Text,
                PayNoMoneyEntry.Text
            };

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                    values[i] = values[i].Replace('.', ',');

            }

            int[] ints = new int[4];
            for (int i = 0; i < values.Length; ++i)
            {
                if (float.TryParse(values[i], out float f))
                {
                    ints[i] = (int)(f * 100);
                }
                else
                {
                    ints[i] = 0;
                }
            }

            try
            {
                _onClose?.Invoke(true, new CheckInfoEventArgs(ints[0], ints[1], ints[2], ints[3]));
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

            if (entry == StartSumEntry)
            {
                PayCashEntry.Focus();
                if (PayCashEntry.Text == null || String.IsNullOrEmpty(PayCashEntry.Text)) return;
                PayCashEntry.CursorPosition = 0;
                PayCashEntry.SelectionLength = PayCashEntry.Text.Length;
                return;
            }

            if (entry == PayCashEntry)
            {
                PayCardEntry.Focus();
                if (PayCardEntry.Text == null || String.IsNullOrEmpty(PayCardEntry.Text)) return;
                PayCardEntry.CursorPosition = 0;
                PayCardEntry.SelectionLength = PayCashEntry.Text.Length;
                return;
            }

            if (entry == PayCardEntry)
            {
                PayNoMoneyEntry.Focus();
                if (PayNoMoneyEntry.Text == null || String.IsNullOrEmpty(PayNoMoneyEntry.Text)) return;
                PayNoMoneyEntry.CursorPosition = 0;
                PayNoMoneyEntry.SelectionLength = PayCashEntry.Text.Length;
                return;
            }
        }
    }
}