using Plugin.BLE.Abstractions.Contracts;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Models.Pages;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class SearchPage : ContentPage
{
    private SearchViewModel _viewModel;
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        AppData.BLEAdapter.DeviceDiscovered += OnDeviceDiscovered;
    }

    private void OnDeviceDiscovered(object? sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
    {
        if (e.Device is IDevice device)
        {
            _viewModel.AddCommand.Execute(device);
        }
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is SearchedDeviceViewModel device)
            _viewModel?.TappedCommand?.Execute(device);
    }
    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("//Remote");
        return true;
    }
}