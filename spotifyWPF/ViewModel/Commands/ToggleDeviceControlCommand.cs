using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using spotifyWPF.View.Services;

namespace spotifyWPF.ViewModel.Commands;

public class ToggleDeviceControlCommand : ICommand
{
    private Visibility _visibility = Visibility.Collapsed;

    private PlayerVM _vm;
    public ToggleDeviceControlCommand(PlayerVM vm)
    {
        _vm = vm;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _vm.ToggleDeviceWindowVisibility();
        //WindowService win = new WindowService();
        //if (_controlShowing)
        //{
        //    win.CloseWindowByType(typeof(DevicesVM));
        //    _controlShowing = false;
        //}
        //else
        //{
        //    win.ShowWindow(new DevicesVM());
        //    _controlShowing = true;
        //}
    }


    public event EventHandler? CanExecuteChanged;
}