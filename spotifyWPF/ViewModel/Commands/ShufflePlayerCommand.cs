using System;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class ShufflePlayerCommand : ICommand
{
    private PlayerVM _vm;
    public ShufflePlayerCommand(PlayerVM vm)
    {
        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return _vm.PlaybackState is not null;  
    }

    public void Execute(object? parameter)
    {
        _vm.ToggleShuffle();
    }

    public event EventHandler? CanExecuteChanged;
}