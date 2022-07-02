using System;
using System.Configuration;
using System.Globalization;
using System.Net.Mime;
using System.Windows.Data;
using System.Windows.Input;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel.Converters.Root;

public class RootTemplateConverter : IValueConverter 
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return value;
        string template = (value as RootTemplate?).ToString();
        return App.Current.Resources[template];

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}