using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Models.Pages;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class SavedPage : ContentPage
{
    SavedViewModel _viewModel;
    public SavedPage(SavedViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        if (AppData.InitializationCompleate)
        {
            base.OnAppearing();
            await Task.Delay(100);
            _viewModel?.GetDeviceList();
        }
    }

    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Debug.WriteLine("SelectionChanged 0");
        if (sender is CollectionView collectionView)
        {
            Debug.WriteLine("SelectionChanged 1");
            if (collectionView.SelectedItem is SavedDeviceViewModel model)
            {
                Debug.WriteLine("SelectionChanged 2");
                _viewModel.TappedCommand.Execute(model);
                collectionView.SelectedItem = null;
            }
        }
    }

    private void TouchBehavior_LongPressCompleted(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        Debug.WriteLine($"{e.LongPressCommandParameter != null}");
    }
    protected override bool OnBackButtonPressed()
    {
        if (_viewModel.SelectionMode) _viewModel.EnterToBaseModeCommand.Execute(null);
        else Shell.Current.GoToAsync("//Remote");
        return true;
    }
}