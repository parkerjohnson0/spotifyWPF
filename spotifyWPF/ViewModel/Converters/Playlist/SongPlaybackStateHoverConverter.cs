using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel.Converters.Playlist;
/// <summary>
/// Compare value of track in playlist and track currently playing. If same track id and same playlist id, display as green
/// </summary>
public class SongPlaybackStateHoverConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {

           
       Track playlistTrack = values[0] as Track;
       PlaybackState state = values[1] as PlaybackState;
       bool isActive = (bool)values[2];
       if (state is null || playlistTrack is null) return null;
       return state.Track.PlaylistID == playlistTrack.PlaylistID &&
              state.Track.SpotifyID == playlistTrack.SpotifyID && isActive;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}