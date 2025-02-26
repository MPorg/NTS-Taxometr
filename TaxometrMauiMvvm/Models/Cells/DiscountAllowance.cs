using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static TaxometrMauiMvvm.Models.Banners.TextExtentions;

namespace TaxometrMauiMvvm.Models.Cells;

public enum AdditionType
{
    None,
    Discount,
    Allowance
}

public enum PaymentType
{
    None,
    Parcent,
    Fixed
}

public partial class DiscountAllowance : ObservableObject
{
    [ObservableProperty]
    private AdditionType _type = AdditionType.None;
    [ObservableProperty]
    private PaymentType _paymentType = PaymentType.None;

    [ObservableProperty]
    ObservableCollection<AdditionType> _typeStr;
    [ObservableProperty]
    ObservableCollection<PaymentType> _payTypeStr;

    [ObservableProperty]
    private Color _valueColor;

    [ObservableProperty]
    private string _valueStr;

    public DiscountAllowance()
    {
        TypeStr = new ObservableCollection<AdditionType>(Enum.GetValues<AdditionType>());
        PayTypeStr = new ObservableCollection<PaymentType>(Enum.GetValues<PaymentType>());
        PropertyChanged += OnPropertyChanged;
    }

    private async void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Type))
        {
            Debug.WriteLine($"_______________________ Type = {Type} ____________________");
        }
        if (e.PropertyName == nameof(PaymentType))
        {
            Debug.WriteLine($"_______________________ Pay type = {PaymentType} ____________________");

            if (!string.IsNullOrEmpty(ValueStr))
            {
                string newValue = PaymentType == PaymentType.Parcent ? ValueStr.TextChanged(false) : ValueStr.TextChanged();
                if (ValueStr != newValue)
                {
                    await Task.Delay(50);
                    ValueStr = newValue;
                }
            }
        }
        if (e.PropertyName == nameof(ValueStr))
        {
            if (!string.IsNullOrEmpty(ValueStr))
            {
                string newValue = PaymentType == PaymentType.Parcent ? ValueStr.TextChanged(false) : ValueStr.TextChanged();
                if (ValueStr != newValue)
                {
                    await Task.Delay(50);
                    ValueStr = newValue;
                }
            }
        }
    }

    public string GetTrueValueStr(string totalSum)
    {
        if (string.IsNullOrEmpty(ValueStr)) return "0,00";
        if (string.IsNullOrEmpty(totalSum)) return "0,00";

        int multiplier = 1;
        decimal trueValue = 0;
        decimal total = decimal.Parse(totalSum);
        decimal val = decimal.Parse(ValueStr.TextCompleate());

        if (_type == AdditionType.None) return "0,00";
        if (_type == AdditionType.Discount) multiplier = -1;

        switch (_paymentType)
        {
            case PaymentType.Parcent:
                trueValue = total / 100 * val;
                break;
            case PaymentType.Fixed:
                trueValue = val;
                break;
            default:
                return "0,00";
        }

        trueValue *= multiplier;

        trueValue = Math.Round(trueValue, 2);

        string trueStr = trueValue.ToString().TextCompleate();

        return trueStr;
    }
}
