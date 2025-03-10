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

    private void OnLoaded(object? sender, EventArgs e)
    {
        CashEntry.Focused += (async (_, __) =>
        {
            await Task.Delay(50);
            _viewModel?.ClickOnEntry(CashEntry, 0);
        });

        CardEntry.Focused += (async (_, __) =>
        {
            await Task.Delay(50);
            _viewModel?.ClickOnEntry(CardEntry, 1);
        });

        NoncashEntry.Focused += (async (_, __) =>
        {
            await Task.Delay(50);
            _viewModel?.ClickOnEntry(NoncashEntry, 2);
        });


        CashEntry.Unfocused += (async (_, __) =>
        {
            await Task.Delay(50);
            if (!string.IsNullOrEmpty(_viewModel?.CashPayText)) _viewModel.CashPayText = _viewModel.CashPayText.TextCompleate();
        });

        CardEntry.Unfocused += (async (_, __) =>
        {
            await Task.Delay(50);
            if (!string.IsNullOrEmpty(_viewModel?.CardPayText)) _viewModel.CardPayText = _viewModel.CardPayText.TextCompleate(); 
        });

        NoncashEntry.Unfocused += (async (_, __) =>
        {
            await Task.Delay(50);
            if (!string.IsNullOrEmpty(_viewModel?.NoncashPayText)) _viewModel.NoncashPayText = _viewModel.NoncashPayText.TextCompleate();
        });
    }

    private void OnCanceled(bool result)
    {
        Canceled?.Invoke(result);
    }
}