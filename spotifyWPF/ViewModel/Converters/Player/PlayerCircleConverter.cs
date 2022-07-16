using System;
using System.Globalization;
using System.Windows.Data;

namespace spotifyWPF.ViewModel.Converters.Player;

public class PlayerCircleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return null;
        double playerProgress = (double)value - int.Parse( parameter.ToString());
        return $"{(playerProgress < 0 ? 0 : playerProgress)},0,0,2";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}