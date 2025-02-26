using System.Globalization;
using TaxometrMauiMvvm.Models.Cells;

namespace TaxometrMauiMvvm.Services
{
    public class EnumTranslateConverter : IValueConverter
    {
        public List<string> AdditionTypeRu = new List<string>
        {
            "-Выбрать-",
            "Скидка",
            "Надбавка"
        };

        public List<string> PaymentTypeRu = new List<string>
        {
            "-Выбрать-",
            "Процентная",
            "Суммовая"
        };
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                if (enumValue is AdditionType additionType)
                {
                    return AdditionTypeRu[(int)additionType];
                }
                if (enumValue is PaymentType paymentType)
                {
                    return PaymentTypeRu[(int)paymentType];
                }
            }
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int index = -1;

            if (targetType == typeof(AdditionType))
            {
                index = AdditionTypeRu.IndexOf(value.ToString());
            }
            if (targetType == typeof(PaymentType))
            {
                index = PaymentTypeRu.IndexOf(value.ToString());
            }

            return index;
        }
    }
}
