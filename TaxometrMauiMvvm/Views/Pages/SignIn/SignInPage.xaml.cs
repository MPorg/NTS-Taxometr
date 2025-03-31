using TaxometrMauiMvvm.Models.Pages.SignIn;

namespace TaxometrMauiMvvm.Views.Pages.SignIn;

public partial class SignInPage : ContentPage
{
    App _app;
    Page _page;
    SignInViewModel _viewModel;

    public SignInPage(SignInViewModel viewModel, App currentApp, Page page)
	{
		InitializeComponent();
        _app = currentApp;
        _page = page;
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        _app.Windows[0].Page = _page;
    }
}