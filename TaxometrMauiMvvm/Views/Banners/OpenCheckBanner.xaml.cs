using TaxometrMauiMvvm.Models.Banners;

namespace TaxometrMauiMvvm.Views.Banners;

public partial class OpenCheckBanner : ContentPage
{
    private OpenCheckViewModel _viewModel;

    public event Action<bool> Canceled;

	public OpenCheckBanner()
	{
		InitializeComponent();
        _viewModel = new OpenCheckViewModel();
        BindingContext = _viewModel;
        _viewModel.Canceled += OnCanceled;
        Loaded += OnLoaded;
	}

    private void OnCanceled(bool result)
    {
        Canceled?.Invoke(result);
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        StartSumEntry.Focus();
    }
}