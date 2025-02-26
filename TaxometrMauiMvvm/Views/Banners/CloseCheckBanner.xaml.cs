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
        _viewModel.DiscountAllownceCreated += ((discalow) =>
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                DiscountAllownaceGrid.BindingContext = discalow;
            });
        });

        _viewModel.DiscountAllownceRemoved += (() =>
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                DiscountAllownaceGrid.BindingContext = null;
            });
        });
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