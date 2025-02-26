using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Data;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Cells;
using static TaxometrMauiMvvm.Models.Banners.TextExtentions;

namespace TaxometrMauiMvvm.Models.Banners;

public partial class CloseCheckViewModel : ObservableObject
{
    public event Action<bool> Canceled;
    public event Action<DiscountAllowance> DiscountAllownceCreated;
    public event Action DiscountAllownceRemoved;

    public event Action<string, string> ValueNotAvailable;

    [ObservableProperty]
    private string _startSumText;
    [ObservableProperty]
    private string _previousSumText;
    [ObservableProperty]
    private string _totalSumText;
    [ObservableProperty]
    private string _extraSumText;
    [ObservableProperty]
    string _cashPayText;
    [ObservableProperty]
    string _cardPayText;
    [ObservableProperty]
    string _noncashPayText;

    [ObservableProperty]
    private DiscountAllowance? _discallow;

    [ObservableProperty]
    private string _discallowStrValue;

    [ObservableProperty]
    private bool _discallowsIsVisible;
    [ObservableProperty]
    private bool _okBtnIsEnabled;

    [ObservableProperty]
    private Color _cardTextColor;
    [ObservableProperty]
    private Color _noncashTextColor;

    private Color _normalColorDark;
    private Color _normalColorLight;

    private Color _errorColor;

    public CloseCheckViewModel(string startVal, string preVal)
    {
        DiscallowStrValue = "0,00";
        CashPayText = "";
        CardPayText = "";
        NoncashPayText = "";

        CalculateValues(startVal, preVal, DiscallowStrValue);


        PropertyChanged += OnPropertyChanged;
        DiscallowsIsVisible = false;
        ValueNotAvailable += ((parameter, message) =>
        {
            Debug.WriteLine($"__________________ {parameter}: {message} _____________________");

            switch (parameter)
            {
                case "discallow":
                    SetColors(discallowErr: true);
                    break;
                case "card":
                    SetColors(cardErr: true);
                    break;
                case "noncash":
                    SetColors(noncashErr: true);
                    break;
                case "card&noncash":
                    SetColors(cardErr: true, noncashErr: true);
                    break;
            }
        });
        CheckValues();
        GetColors();
        ClearColors();
    }

    private void ClearColors()
    {
        if (Application.Current.PlatformAppTheme == AppTheme.Dark)
        {
            CardTextColor = _normalColorDark;
            NoncashTextColor = _normalColorDark;
            if (Discallow != null) Discallow.ValueColor = _normalColorDark;
        }
        else
        {
            CardTextColor = _normalColorLight;
            NoncashTextColor = _normalColorLight;
            if (Discallow != null) Discallow.ValueColor = _normalColorLight;
        }
    }

    private void SetColors(bool cardErr = false, bool noncashErr = false, bool discallowErr = false)
    {
        if (cardErr)
        {
            CardTextColor = _errorColor;
        }
        if (noncashErr)
        {
            NoncashTextColor = _errorColor;
        }
        if (discallowErr)
        {
            if (Discallow != null) Discallow.ValueColor = _errorColor;
        }
    }

    private void GetColors()
    {
        if (Application.Current.Resources.TryGetValue("TextDark", out object col))
        {
            if (col is Color color)
            {
                _normalColorDark = color;
            }
        }
        if (Application.Current.Resources.TryGetValue("TextDark", out object col1))
        {
            if (col1 is Color color)
            {
                _normalColorLight = color;
            }
        }
        _errorColor = new Color(255, 5, 5);
    }

    [RelayCommand]
    private void Cancel()
    {
        Canceled?.Invoke(false);
    }

    [RelayCommand]
    private async Task Ok()
    {
        GetValues(out int cash, out int card, out int noncash, out int initial, out int extra, out int discallow, out int total, out int previous);
        cash += previous;
        int sum = cash + card + noncash;
        int trueCash = cash - previous;
        if (sum > initial + discallow)
        {
            cash = (initial + discallow) - (card + noncash);
        }
        AppData.Provider.CloseCheck(cash, card, noncash, trueCash, initial + discallow);

        Canceled?.Invoke(true);
    }

    [RelayCommand]
    private void AddDiscountAllowance()
    {
        Discallow = new DiscountAllowance();
        DiscallowStrValue = Discallow.GetTrueValueStr(_startSumText);
        Discallow.PropertyChanged += OnPropertyChanged;
        DiscountAllownceCreated?.Invoke(Discallow);
    }

    [RelayCommand]
    private void RemoveDiscountAllowance()
    {
        Discallow.PropertyChanged -= OnPropertyChanged;
        Discallow = null;
        DiscallowStrValue = "0,00";
        CalculateValues(StartSumText, PreviousSumText, DiscallowStrValue);
        DiscountAllownceRemoved?.Invoke();
    }

    private bool CheckValues()
    {
        ClearColors();
        GetValues(out int cash, out int card, out int noncash, out int initial, out int extra, out int discallow, out int total, out int previous);
        if (discallow < 0 && Math.Abs(discallow) >= initial)
        {
            ValueNotAvailable?.Invoke(nameof(discallow), "Скидка не может быть больше либо равна итогу");
            OkBtnIsEnabled = false;
            return false;
        }
        
        if (card + noncash > initial + discallow)
        {
            if ((card > initial + discallow) && (noncash > initial + discallow)) ValueNotAvailable?.Invoke("card&noncash", "Значение оплаты картой превышает итог");
            else if (card > initial + discallow) ValueNotAvailable?.Invoke(nameof(card), "Значение оплаты картой превышает итог");
            else if (noncash > initial + discallow) ValueNotAvailable?.Invoke(nameof(noncash), "Значение оплаты безналичными превышает итог");
            else
            {
                if (card >= noncash)
                    ValueNotAvailable?.Invoke(nameof(card), "Значение оплаты картой превышает итог");
                else
                    ValueNotAvailable?.Invoke(nameof(noncash), "Значение оплаты безналичными превышает итог");
            }

            OkBtnIsEnabled = false;
            return false;
        }


        OkBtnIsEnabled = true;
        return true;
    }

    private void GetValues(out int cash, out int card, out int noncash, out int initial, out int extra, out int discallow, out int total, out int previous)
    {
        string cashPayText = CashPayText.TextCompleate();
        string cardPayText = CardPayText.TextCompleate();
        string noncashPayText = NoncashPayText.TextCompleate();

        string cashValStr = cashPayText;
        string cardValStr = cardPayText;
        string noncashValStr = noncashPayText;
        string extraValStr = ExtraSumText;
        string initValStr = StartSumText;
        string discallowValStr = Discallow?.GetTrueValueStr(_startSumText);
        if (string.IsNullOrWhiteSpace(discallowValStr)) discallowValStr = "0,00";
        string totalValStr = TotalSumText;
        string previousValStr = PreviousSumText;

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
        while (extraValStr.Contains(Comma))
        {
            extraValStr = extraValStr.Remove(extraValStr.IndexOf(Comma), 1);
        }
        while (discallowValStr.Contains(Comma))
        {
            discallowValStr = discallowValStr.Remove(discallowValStr.IndexOf(Comma), 1);
        }
        while (totalValStr.Contains(Comma))
        {
            totalValStr = totalValStr.Remove(totalValStr.IndexOf(Comma), 1);
        }
        while (previousValStr.Contains(Comma))
        {
            previousValStr = previousValStr.Remove(previousValStr.IndexOf(Comma), 1);
        }
        while (initValStr.Contains(Comma))
        {
            initValStr = initValStr.Remove(initValStr.IndexOf(Comma), 1);
        }

        cash = int.Parse(cashValStr);
        card = int.Parse(cardValStr);
        noncash = int.Parse(noncashValStr);
        initial = int.Parse(initValStr);
        extra = int.Parse(extraValStr);
        discallow = int.Parse(discallowValStr);
        total = int.Parse(totalValStr);
        previous = int.Parse(previousValStr);
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

            if (CheckValues())
            {
                DiscallowStrValue = Discallow.GetTrueValueStr(_startSumText);
                CalculateValues(StartSumText, PreviousSumText, DiscallowStrValue);
                return;
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

            if (CheckValues())
            {
                DiscallowStrValue = Discallow.GetTrueValueStr(_startSumText);
                CalculateValues(StartSumText, PreviousSumText, DiscallowStrValue);
                return;
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

            if (CheckValues())
            {
                DiscallowStrValue = Discallow.GetTrueValueStr(_startSumText);
                CalculateValues(StartSumText, PreviousSumText, DiscallowStrValue);
                return;
            }
        }
        if (e.PropertyName == nameof(Discallow))
        {
            if (Discallow != null) DiscallowsIsVisible = true;
            else DiscallowsIsVisible = false;
        }

        if (e.PropertyName == nameof(Discallow.Type) || e.PropertyName == nameof(Discallow.PaymentType))
        {
            if (CheckValues())
            {
                DiscallowStrValue = Discallow.GetTrueValueStr(_startSumText);
                CalculateValues(StartSumText, PreviousSumText, DiscallowStrValue);
                return;
            }
        }

        if (e.PropertyName == nameof(Discallow.ValueStr))
        {
            string newValue = Discallow.ValueStr.TextChanged();
            if (Discallow.ValueStr != newValue)
            {
                await Task.Delay(50);
                Discallow.ValueStr = newValue;
            }
            await Task.Delay(100);
            if (CheckValues())
            {
                DiscallowStrValue = Discallow.GetTrueValueStr(_startSumText);
                CalculateValues(StartSumText, PreviousSumText, DiscallowStrValue);
                return;
            }
        }
    }

    private void CalculateValues(string startVal, string preVal, string discallowVal)
    {
        if (!startVal.Contains(Comma))
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
        }

        if (!preVal.Contains(Comma))
        {
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
        }

        StartSumText = startVal;
        PreviousSumText = preVal;
        
        decimal start = decimal.Parse(startVal);
        decimal preval = decimal.Parse(preVal);
        decimal extra = Math.Round(start - preval, 2);
        decimal discallow = decimal.Parse(discallowVal);
        decimal total = Math.Round(start + discallow - preval, 2);
        string extraVal = extra.ToString();
        string totalVal = total.ToString();


        if (!extraVal.Contains(Comma))
        {
            if (extraVal == "0")
            {
                extraVal = "0,00";
            }
            else if (extraVal.Length == 1)
            {
                extraVal = "0,0" + extraVal;
            }
            else if (extraVal.Length == 2)
            {
                extraVal = "0," + extraVal;
            }
            else
            {
                List<char> chars = new List<char>();
                for (int i = 0; i < extraVal.Length; i++)
                {
                    chars.Add(extraVal[i]);
                    if (i == extraVal.Length - 3) chars.Add(',');
                }
                extraVal = new string(chars.ToArray());
            }
        }

        if (!totalVal.Contains(Comma))
        {
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
        }

        ExtraSumText = extraVal;
        TotalSumText = totalVal;
    }
}
