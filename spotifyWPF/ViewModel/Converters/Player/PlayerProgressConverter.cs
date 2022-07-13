using System;
using System.Globalization;
using System.Windows.Data;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel.Converters.Player;

/// <summary>
/// Gets ratio of song progess to total duration. Determines width of playback slider
/// </summary>
public class PlayerProgressConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is null || values[1] is null ) return 0;

        PlaybackState state = values[0] as PlaybackState;
        double width = (double)values[1];
        double ratio = (double) state.ProgressMS / state.Track.DurationMS;
        return width * ratio;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}