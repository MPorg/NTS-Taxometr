using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxometrMauiMvvm.Data;
using static TaxometrMauiMvvm.Models.Banners.TextExtentions;

namespace TaxometrMauiMvvm.Models.Banners;

public partial class OpenCheckViewModel : ObservableObject
{
    [ObservableProperty]
    private string _startSumText;
    [ObservableProperty]
    private string _previousSumText;

    public event Action<bool> Canceled;

    public OpenCheckViewModel()
    {
        StartSumText = "";
        PreviousSumText = "";

        PropertyChanged += OnPropertyChanged;
    }

    [RelayCommand]
    private void Cancel()
    {
        Canceled?.Invoke(false);
    }

    [RelayCommand]
    private async Task Ok()
    {
        GetValues(out int startVal, out int preVal);
        (await AppData.Provider()).OpenCheck(startVal, preVal);
        Canceled?.Invoke(true);
    }

    private void GetValues(out int startVal, out int preVal)
    {
        StartSumText = StartSumText.TextCompleate();
        PreviousSumText = PreviousSumText.TextCompleate();
        string initValueStr = StartSumText;
        string prePayValueStr = PreviousSumText;
        while (initValueStr.Contains(Comma))
        {
            initValueStr = initValueStr.Remove(initValueStr.IndexOf(Comma), 1);
        }
        while (prePayValueStr.Contains(Comma))
        {
            prePayValueStr = prePayValueStr.Remove(prePayValueStr.IndexOf(Comma), 1);
        }
        startVal = int.Parse(initValueStr);
        preVal = int.Parse(prePayValueStr);
    }

    [RelayCommand]
    private async Task CompleteText(string key)
    {
        switch (key)
        {
            case "S":
                string newValue = StartSumText.TextCompleate();
                if (StartSumText != newValue)
                {
                    await Task.Delay(50);
                    StartSumText = newValue;
                }
                break;
            case "P":
                newValue = PreviousSumText.TextCompleate();
                if (PreviousSumText != newValue)
                {
                    await Task.Delay(50);
                    PreviousSumText = newValue;
                }
                break;
        }
    }

    private async void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(StartSumText))
        {
            string newValue = StartSumText.TextChanged();
            if (StartSumText != newValue)
            {
                await Task.Delay(50);
                StartSumText = newValue;
            }
        }
        if (e.PropertyName == nameof(PreviousSumText))
        {
            string newValue = PreviousSumText.TextChanged();
            if (PreviousSumText != newValue)
            {
                await Task.Delay(50);
                PreviousSumText = newValue;
            }
        }
    }
}
