using System.Globalization;
using TaxometrMauiMvvm.Models.Cells;

namespace TaxometrMauiMvvm.Services;

public class TypeToUnitConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is PaymentType paymentType)
        {
            switch (paymentType)
            {
                case PaymentType.Parcent:
                    return "%";
                case PaymentType.Fixed:
                    return "руб.";
            }
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
