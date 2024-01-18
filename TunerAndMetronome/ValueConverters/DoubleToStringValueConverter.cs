using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TunerAndMetronome.ValueConverters;

public class DoubleToStringValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value?.GetType() == typeof(double))
            return ((double)value).ToString("F3");
        if (value?.GetType() == typeof(float))
            return ((float)value).ToString("F3");
        return value?.ToString() ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}