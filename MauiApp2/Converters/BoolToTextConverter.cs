using System.Globalization;

namespace MauiApp2.Converters;
public class BoolToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool b || parameter is not string s) return value?.ToString() ?? "";
        string?[] parts = s.Split(';');
        return b ? parts[0] : parts.ElementAtOrDefault(1);

    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
