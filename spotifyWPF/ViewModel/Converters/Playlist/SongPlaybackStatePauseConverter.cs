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
public class SongPlaybackStatePauseConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {

           
       Track playlistTrack = values[0] as Track;
       Track playingTrack = values[1] as Track;
       if (playingTrack is null || playlistTrack is null) return null;
       return playingTrack.PlaylistID == playlistTrack.PlaylistID as string &&
              playingTrack.SpotifyID == playlistTrack.SpotifyID;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}