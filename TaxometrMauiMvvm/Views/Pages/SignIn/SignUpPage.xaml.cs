using TaxometrMauiMvvm.Models.Pages.SignIn;

namespace TaxometrMauiMvvm.Views.Pages.SignIn;

public partial class SignUpPage : ContentPage
{
    App _app;
    Page _page;
    SignUpViewModel _viewModel;

    public SignUpPage(SignUpViewModel viewModel, App currentApp, Page page)
	{
		InitializeComponent();
        _app = currentApp;
        _page = page;
        _viewModel = viewModel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        _app.Windows[0].Page = _page;
    }
}