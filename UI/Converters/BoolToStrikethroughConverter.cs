using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace UI.Converters;

public class BoolToStrikethroughConverter : IValueConverter
{
    [SuppressMessage("ReSharper", "MergeIntoPattern")]
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompleted && isCompleted)
        {
            return TextDecorations.Strikethrough;
        }

        return TextDecorations.None;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Anladığım kadarıyla tekrar geri dönmeye gerek yok, hep üsttekini yapıyoruz o kadar.
        return null;
    }
}