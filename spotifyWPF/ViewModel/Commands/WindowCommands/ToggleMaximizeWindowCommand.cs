using System;
using System.Buffers;
using System.Windows;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands.WindowCommands;

public class ToggleMaximizeWindowCommand : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (App.Current.MainWindow.WindowState == WindowState.Maximized)
        {
            App.Current.MainWindow.WindowState = WindowState.Normal;
        }
        else
        {

            App.Current.MainWindow.WindowState = WindowState.Maximized;
        }
    }

    public event EventHandler? CanExecuteChanged;
}