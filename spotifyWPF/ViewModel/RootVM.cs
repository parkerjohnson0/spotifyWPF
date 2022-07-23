using System.Windows;
using System.Windows.Input;
using spotifyWPF.View.Controls;
using spotifyWPF.ViewModel.Commands;
using spotifyWPF.ViewModel.Commands.WindowCommands;

namespace spotifyWPF.ViewModel;

public class RootVM : ViewModelBase
{
    public DevicesVM DevicesVm { get; set; }
    public MouseUpCommand MouseUpCommand { get; set; }
    public CloseWindowCommand CloseWindowCommand { get; set; }
    public ToggleMaximizeWindowCommand ToggleMaximizeWindowCommand { get; set; }
    private bool _deviceControlFocused;

    public bool DeviceControlFocused
    {
        get { return _deviceControlFocused; }
        set { _deviceControlFocused = value; NotifyPropertyChanged(); }
    }

    public RootVM()
    {
        AppState.OnDeviceControlClicked += FocusDeviceControl;
        MouseUpCommand = new MouseUpCommand(this);
        CloseWindowCommand = new CloseWindowCommand();
        ToggleMaximizeWindowCommand = new ToggleMaximizeWindowCommand();
        DevicesVm = new DevicesVM(); 
    }

    private void FocusDeviceControl(object? sender, System.EventArgs e)
    {
        
          //DeviceControlFocused = true;
    }

    public void MouseUp(MouseButtonEventArgs? e)
    {
        //Point point = e.GetPosition((IInputElement)e.OriginalSource);
        if ((FrameworkElement)e.Source is not DevicesUC &&
           ((FrameworkElement)e.OriginalSource).Name != "DeviceImage" )
        {
            //DeviceControlFocused = false;
            AppState.DeviceControlUnfocused();  
           //AppState.DeviceControlVisibility = Visibility.Collapsed;
        }
    }
}