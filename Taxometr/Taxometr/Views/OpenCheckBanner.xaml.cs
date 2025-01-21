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
    public partial class OpenCheckBanner : ContentPage
    {
        private Action<bool, CheckInfoEventArgs> _onClose;

        public class CheckInfoEventArgs : EventArgs
        {
            public int initialValue;
            public int prePayValue;

            /// <summary>
            /// Create empty args whith 0 values
            /// </summary>
            public CheckInfoEventArgs() : this(0, 0) { }

            /// <summary>
            /// Create the CheckInfoEventArgs
            /// </summary>
            /// <param name="initialValue"></param>
            public CheckInfoEventArgs(int initialValue, int prePayValue)
            {
                this.initialValue = initialValue;
                this.prePayValue = prePayValue;
            }
        }

        public OpenCheckBanner(Action<bool, CheckInfoEventArgs> onClose)
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
            OnEntry_Completed(StartSumEntry, e);
            OnEntry_Completed(PreSumEntry, e);
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

            Debug.WriteLine($"___________________________ {entry.Text} ________________________________");

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

            Debug.WriteLine(entry.Text);

            if (entry == StartSumEntry)
            {
                PreSumEntry.Focus();
            }
            else
            {
                Compleate();
            }
        }

        private void Compleate()
        {
            string initValueStr = StartSumEntry.Text;
            string prePayValueStr = PreSumEntry.Text;

            while (initValueStr.Contains(","))
            {
                initValueStr = initValueStr.Remove(initValueStr.IndexOf(','), 1);
            }
            while (prePayValueStr.Contains(","))
            {
                prePayValueStr = prePayValueStr.Remove(prePayValueStr.IndexOf(','), 1);
            }
            int initValue = int.Parse(initValueStr);
            int prePayValue = int.Parse(prePayValueStr);

            try
            {
                _onClose?.Invoke(true, new CheckInfoEventArgs(initValue, prePayValue));
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