using System;
using System.Windows.Input;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel.Commands;

public class SelectTrackCommand : ICommand
{
    private DisplayVM _vm;
    public SelectTrackCommand(DisplayVM vm)
    {
        _vm = vm;

    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        Track track = parameter as Track;
        foreach (Track t in _vm.SelectedPlaylistItem.SongsList)
        {
            t.Active = false;
        }
        track.Active = true;
    }

    public event EventHandler? CanExecuteChanged;
}