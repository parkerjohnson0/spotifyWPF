using System.Windows;
using System.Windows.Input;
using spotifyWPF.View.Controls;
using spotifyWPF.ViewModel.Commands;

namespace spotifyWPF.ViewModel;

public class RootVM : ViewModelBase
{
    public MouseUpCommand MouseUpCommand { get; set; }
    public RootVM()
    {
        MouseUpCommand = new MouseUpCommand(this);
    }

    public void MouseUp(MouseButtonEventArgs? e)
    {
        //Point point = e.GetPosition((IInputElement)e.OriginalSource);
        if (Mouse.DirectlyOver is not DevicesVM)
        {
            AppState.DeviceControlVisibility = Visibility.Collapsed;
        }
    }
}