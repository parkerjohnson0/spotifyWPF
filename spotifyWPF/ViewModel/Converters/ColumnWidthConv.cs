using System;
using System.Globalization;
using System.Windows.Data;

namespace spotifyWPF.ViewModel.Converters;

public class ColumnWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return value;
        int numColumns = Int32.Parse(parameter as string);
        double totalWidth = (double)value;
        return totalWidth / numColumns;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}