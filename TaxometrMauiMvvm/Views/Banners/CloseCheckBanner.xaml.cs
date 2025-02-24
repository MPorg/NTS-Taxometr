using TaxometrMauiMvvm.Models.Banners;

namespace TaxometrMauiMvvm.Views.Banners;

public partial class CloseCheckBanner : ContentPage
{
    private CloseCheckViewModel _viewModel;

    public event Action<bool> Canceled;

    public CloseCheckBanner(string startVal, string preVal)
	{
		InitializeComponent();
        _viewModel = new CloseCheckViewModel(startVal, preVal);
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
        CashEntry.Focus();
    }
}