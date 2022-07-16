using System;
using System.Windows.Input;

namespace spotifyWPF.ViewModel.Commands;

public class MouseScrollCommand : ICommand
{
    private DisplayVM _vm;
    private int _delta;

    public MouseScrollCommand(DisplayVM vm)
    {
        _vm = vm;
    }

    public bool CanExecute(object? parameter)
    {
        return _vm.SelectedPlaylistItem.SongsList.Count > 0;
    }

    public void Execute(object? parameter)
    {
        MouseWheelEventArgs args = parameter as MouseWheelEventArgs;
        _delta += args.Delta;

        if (_delta < -1500)
        {
            _vm.PlaylistScrolledDown();
            _delta = 0;
        }
        else if (_delta > 1500)
        {
            _vm.PlaylistScrolledUp();
            _delta = 0;
        }
    }

    public event EventHandler? CanExecuteChanged;
}