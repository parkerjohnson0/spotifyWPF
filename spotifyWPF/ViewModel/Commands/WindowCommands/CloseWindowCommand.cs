using System;
using System.Windows;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands.WindowCommands;

public class CloseWindowCommand : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        Application.Current.Shutdown();
    }

    public event EventHandler? CanExecuteChanged;
}