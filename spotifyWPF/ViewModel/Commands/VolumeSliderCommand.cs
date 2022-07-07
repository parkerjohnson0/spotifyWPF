using System;
using System.Windows;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class VolumeSliderCommand : ICommand
{
    private PlayerVM _vm;
    public VolumeSliderCommand(PlayerVM vm)
    {
        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        int clickedX = (int) Mouse.GetPosition((IInputElement)parameter).X;
        _vm.SetVolume(clickedX);
    }

    public event EventHandler? CanExecuteChanged;
}