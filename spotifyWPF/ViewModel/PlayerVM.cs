using System;
using spotifyWPF.ViewModel.Commands;

namespace spotifyWPF.ViewModel;

public class PlayerVM : ViewModelBase
{
    public VolumeSliderCommand VolumeSliderCommand { get; set; }
    private int _volume = 50;
    public int Volume
    {
        get { return _volume; }
        set
        {
            _volume = value;
            AppState.Volume = _volume;
            NotifyPropertyChanged();
        }
    }
    public PlayerVM()
    {
        VolumeSliderCommand = new VolumeSliderCommand(this);
        AppState.OnAuthorized += PlayerVMAuthorized;

    }

    private void PlayerVMAuthorized(object? sender, EventArgs e)
    {
    }

    public void SetVolume(int volume)
    {
        Volume = volume;
    }
}