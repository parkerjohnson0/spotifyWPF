using System;
using System.Windows;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class MouseUpCommand : ICommand
{
    private RootVM _vm;
    public MouseUpCommand(RootVM vm)
    {
        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return _vm.AppState.DeviceControlVisibility == Visibility.Visible;
    }

    public void Execute(object? parameter)
    {
        _vm.MouseUp(parameter as MouseButtonEventArgs);
    }

    public event EventHandler? CanExecuteChanged;
}