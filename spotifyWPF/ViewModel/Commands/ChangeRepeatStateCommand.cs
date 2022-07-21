using System;
using System.Runtime.InteropServices.ObjectiveC;
using System.Windows.Input;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel.Commands;

public class ChangeRepeatStateCommand : ICommand
{
    private  PlayerVM _vm;

    public ChangeRepeatStateCommand(PlayerVM vm)
    {
        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return _vm.AppState.PlaybackState != null;
    }

    public async void Execute(object? parameter)
    {
        if (await _vm.AppState.SpotifyRequest.ChangeRepeatState( (RepeatState) parameter ))
        {
            _vm.AppState.GetPlaybackState();
        }
    }

    public event EventHandler? CanExecuteChanged;
}