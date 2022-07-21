using System;
using System.Windows.Input;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel.Commands;

public class PlayTrackCommand : ICommand
{
    private DisplayVM _vm;

    public PlayTrackCommand(DisplayVM vm)
    {
        _vm = vm;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (parameter is null || _vm.AppState.PlaybackState is null) return;
        Track track = parameter as Track;
        if (_vm.AppState.PlaybackState.IsPlaying &&
            _vm.AppState.PlaybackState.Track.PlaylistID == track.PlaylistID &&
            _vm.AppState.PlaybackState.Track.SpotifyID == track.SpotifyID)
        {
            _vm.PauseSelectedTrack(track);
        }
        else
        {
            _vm.PlaySelectedTrack(track);
        }
    }

    public event EventHandler? CanExecuteChanged;
}