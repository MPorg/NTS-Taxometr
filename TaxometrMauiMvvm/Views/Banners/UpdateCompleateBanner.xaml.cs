using Microsoft.Extensions.Options;
using TaxometrMauiMvvm.Models.Banners;

namespace TaxometrMauiMvvm.Views.Banners;

public partial class UpdateCompleateBanner : ContentPage
{
	UpdateCompleateViewModel _viewModel;
	public event Action Complete;
	public UpdateCompleateBanner(UpdateCompleateViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    private void OkBtn_Clicked(object sender, EventArgs e)
    {
		Complete?.Invoke();
    }
}