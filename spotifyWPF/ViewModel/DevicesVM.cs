using System.Threading.Channels;
using System.Windows;

namespace spotifyWPF.ViewModel;

public class DevicesVM : ViewModelBase
{
    private bool _deviceControlFocused;

    public bool DeviceControlFocused
    {
        get { return _deviceControlFocused; }
        set { _deviceControlFocused = value; NotifyPropertyChanged(); }
    }
    private Visibility _visibility = Visibility.Collapsed;

    public Visibility Visibility
    {
        get { return _visibility; }
        set
        {
            _visibility = value;
            NotifyPropertyChanged();
        }
    }

    public DevicesVM()
    {
        AppState.OnDeviceControlClicked += (sender, args) => Visibility = AppState.DeviceControlVisibility;
        AppState.OnDeviceControlUnfocused += (sender, args) => Visibility = AppState.DeviceControlVisibility;

    }
}