using System;
using System.Globalization;
using System.Windows.Data;

namespace spotifyWPF.ViewModel.Converters.Root;

public class DateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return value;
        DateTime date = (DateTime) value ;
        return date.ToString("M") + ", " + date.Year;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}