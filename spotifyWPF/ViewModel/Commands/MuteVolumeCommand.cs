using System;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class MuteVolumeCommand : ICommand
{
    private PlayerVM _vm;
    public MuteVolumeCommand(PlayerVM vm)
    {

        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _vm.SetVolume(0);
    }

    public event EventHandler? CanExecuteChanged;
}