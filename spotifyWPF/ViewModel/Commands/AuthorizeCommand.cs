using System;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class AuthorizeCommand : ICommand
{
    private AuthorizeVM _vm;
    public AuthorizeCommand(AuthorizeVM vm)
    {
        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _vm.Authorize();
    }

    public event EventHandler? CanExecuteChanged;
}