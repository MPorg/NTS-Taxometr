using System;
using System.Diagnostics;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            StartSumEntry.Focus();
        }

        private void OnCancelBtn_Clicked(object sender, EventArgs e)
        {
            _onClose?.Invoke(false, new CheckInfoEventArgs());
            Navigation.PopModalAsync();
        }

        private void OnOkBtn_Clicked(object sender, EventArgs e)
        {
            string[] values = new string[4];
            values[0] = StartSumEntry.Text;
            values[1] = PayCashEntry.Text;
            values[2] = PayCardEntry.Text;
            values[3] = PayNoMoneyEntry.Text;

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Replace('.', ',');

                Debug.WriteLine(values[i] /*+ $"{float.Parse(values[i])}"*/);
            }

            int[] ints = new int[4];
            for (int i = 0; i < values.Length; ++i)
            {
                ints[i] = (int)(float.Parse(values[i]) * 100);
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
    }
}