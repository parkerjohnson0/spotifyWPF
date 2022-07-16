using System;
using System.Windows.Input;
using System.Xml.Schema;

namespace spotifyWPF.ViewModel.Commands;

public class LazyListViewLoadCommand : ICommand
{
    private DisplayVM _vm;
    public LazyListViewLoadCommand(object  scrollViewerDataContext)
    {
        _vm = scrollViewerDataContext as DisplayVM;
    }

    public bool CanExecute(object? parameter)
    {
        throw new NotImplementedException();
    }

    public void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }

    public event EventHandler? CanExecuteChanged;
}