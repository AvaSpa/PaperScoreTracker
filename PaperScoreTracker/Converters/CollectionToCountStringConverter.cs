using System.Globalization;

namespace PaperScoreTracker.Converters;

public class CollectionToCountStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return 0;

        var collection = (System.Collections.ICollection)value;
        return collection.Count;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
