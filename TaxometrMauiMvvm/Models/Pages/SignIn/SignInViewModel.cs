using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxometrMauiMvvm.Models.Pages.SignIn
{
    public partial class SignInViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _loginBtnIsEnuble;

        public SignInViewModel()
        {
            LoginBtnIsEnuble = false;
        }
    }
}
