using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxometrMauiMvvm.Data;
using static TaxometrMauiMvvm.Models.Banners.TextExtentions;

namespace TaxometrMauiMvvm.Models.Banners;

public partial class CloseCheckViewModel : ObservableObject
{
    public event Action<bool> Canceled;

    [ObservableProperty]
    private string _startSumText;
    [ObservableProperty]
    private string _previousSumText;
    [ObservableProperty]
    private string _totalSumText;
    [ObservableProperty]
    string _cashPayText;
    [ObservableProperty]
    string _cardPayText;
    [ObservableProperty]
    string _noncashPayText;


    public CloseCheckViewModel(string startVal, string preVal)
    {
        CalculateValues(startVal, preVal);
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
        GetValues(out int cash, out int card, out int noncash, out int total);
        int sum = cash + card + noncash;
        int trueCash = cash;
        if (sum > total)
        {
            cash = total - (card + noncash);
        }


        AppData.Provider.CloseCheck(cash, card, noncash, trueCash, total);

        Canceled?.Invoke(true);
    }

    private void GetValues(out int cash, out int card, out int noncash, out int total)
    {
        CashPayText = CashPayText.TextCompleate();
        CardPayText = CardPayText.TextCompleate();
        NoncashPayText = NoncashPayText.TextCompleate();

        string cashValStr = CashPayText;
        string cardValStr = CardPayText;
        string noncashValStr = NoncashPayText;
        string totalValStr = TotalSumText;

        while (cashValStr.Contains(Comma))
        {
            cashValStr = cashValStr.Remove(cashValStr.IndexOf(Comma), 1);
        }
        while (cardValStr.Contains(Comma))
        {
            cardValStr = cardValStr.Remove(cardValStr.IndexOf(Comma), 1);
        }
        while (noncashValStr.Contains(Comma))
        {
            noncashValStr = noncashValStr.Remove(noncashValStr.IndexOf(Comma), 1);
        }
        while (totalValStr.Contains(Comma))
        {
            totalValStr = totalValStr.Remove(totalValStr.IndexOf(Comma), 1);
        }

        cash = int.Parse(cashValStr);
        card = int.Parse(cardValStr);
        noncash = int.Parse(noncashValStr);
        total = int.Parse(totalValStr);
    }

    [RelayCommand]
    private async Task CompleteText(string key)
    {
        switch (key)
        {
            case "Cash":
                string newValue = CashPayText.TextCompleate();
                if (CashPayText != newValue)
                {
                    await Task.Delay(50);
                    CashPayText = newValue;
                }
                break;
            case "Card":
                newValue = CardPayText.TextCompleate();
                if (CardPayText != newValue)
                {
                    await Task.Delay(50);
                    CardPayText = newValue;
                }
                break;
            case "Noncash":
                newValue = NoncashPayText.TextCompleate();
                if (NoncashPayText != newValue)
                {
                    await Task.Delay(50);
                    NoncashPayText = newValue;
                }
                break;
        }
    }
    private async void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CashPayText))
        {
            string newValue = CashPayText.TextChanged();
            if (CashPayText != newValue)
            {
                await Task.Delay(50);
                CashPayText = newValue;
            }
        }
        if (e.PropertyName == nameof(CardPayText))
        {
            string newValue = CardPayText.TextChanged();
            if (CardPayText != newValue)
            {
                await Task.Delay(50);
                CardPayText = newValue;
            }
        }
        if (e.PropertyName == nameof(NoncashPayText))
        {
            string newValue = NoncashPayText.TextChanged();
            if (NoncashPayText != newValue)
            {
                await Task.Delay(50);
                NoncashPayText = newValue;
            }
        }
    }

    private void CalculateValues(string startVal, string preVal)
    {
        if (startVal == "0")
        {
            startVal = "0,00";
        }
        else if (startVal.Length == 1)
        {
            startVal = "0,0" + startVal;
        }
        else if (startVal.Length == 2)
        {
            startVal = "0," + startVal;
        }
        else
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < startVal.Length; i++)
            {
                chars.Add(startVal[i]);
                if (i == startVal.Length - 3) chars.Add(',');
            }
            startVal = new string(chars.ToArray());
        }

        if (preVal == "0")
        {
            preVal = "0,00";
        }
        else if (preVal.Length == 1)
        {
            preVal = "0,0" + preVal;
        }
        else if (preVal.Length == 2)
        {
            preVal = "0," + preVal;
        }
        else
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < preVal.Length; i++)
            {
                chars.Add(preVal[i]);
                if (i == preVal.Length - 3) chars.Add(',');
            }
            preVal = new string(chars.ToArray());
        }

        StartSumText = startVal;
        PreviousSumText = preVal;

        float start = float.Parse(startVal);
        float preval = float.Parse(preVal);
        float total = start - preval;
        string totalVal = (total * 100f).ToString();


        if (totalVal == "0")
        {
            totalVal = "0,00";
        }
        else if (totalVal.Length == 1)
        {
            totalVal = "0,0" + totalVal;
        }
        else if (totalVal.Length == 2)
        {
            totalVal = "0," + totalVal;
        }
        else
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < totalVal.Length; i++)
            {
                chars.Add(totalVal[i]);
                if (i == totalVal.Length - 3) chars.Add(',');
            }
            totalVal = new string(chars.ToArray());
        }

        TotalSumText = totalVal;
        CashPayText = "";
        CardPayText = "";
        NoncashPayText = "";
    }
}
