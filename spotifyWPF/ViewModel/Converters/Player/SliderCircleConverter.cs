﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace spotifyWPF.ViewModel.Converters.Player;

public class SliderCircleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return "0,0,0,0";
        return $"{value},0,0,0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}