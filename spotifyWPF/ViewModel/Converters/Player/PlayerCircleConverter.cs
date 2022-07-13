using System;
using System.Globalization;
using System.Windows.Data;

namespace spotifyWPF.ViewModel.Converters.Player;

public class PlayerCircleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}