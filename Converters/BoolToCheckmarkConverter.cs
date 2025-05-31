using System.Globalization;

namespace FlowLife.Converters
{
    public class BoolToCheckmarkConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "✔️" : "❌";
            }
            return "❌";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 