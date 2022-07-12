using System;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class StartPlaybackCommand : ICommand
{
    private PlayerVM _vm;
    public StartPlaybackCommand(PlayerVM vm)
    {
        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return _vm.AppState.SelectedDevice != null;
    }

    public async void Execute(object? parameter)
    {
        await _vm.StartPlayback();
    }

    public event EventHandler? CanExecuteChanged;
}