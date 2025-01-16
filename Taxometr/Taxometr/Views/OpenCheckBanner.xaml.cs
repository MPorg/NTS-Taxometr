using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpenCheckBanner : ContentPage
    {
        private Action<bool, int> _onClose;

        public OpenCheckBanner(Action<bool, int> onClose)
        {
            InitializeComponent();
            _onClose = onClose;
        }

        private void OnCancelBtn_Clicked(object sender, EventArgs e)
        {
            _onClose?.Invoke(false, 0);
            Navigation.PopModalAsync();
        }

        private void OnOkBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                _onClose?.Invoke(true, int.Parse(StartSumEntry.Text));
            }
            catch
            {
                _onClose?.Invoke(false, 0);
            }
            finally
            {
                Navigation.PopModalAsync();
            }
        }
    }
}