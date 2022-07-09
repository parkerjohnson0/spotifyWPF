using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.Player;
using spotifyWPF.ViewModel.Commands;

namespace spotifyWPF.ViewModel;

public class PlayerVM : ViewModelBase
{
    private string _getDevicesUrl = "https://api.spotify.com/v1/me/player/devices";
    public VolumeSliderCommand VolumeSliderCommand { get; set; }
    private bool _isMuted;

    public bool IsMuted
    {
        get { return _isMuted; }
        set { _isMuted = value; NotifyPropertyChanged();}
    }
    private int _lastVolume;
    private int _volume = 50;

    public ToggleDeviceControlCommand  ToggleDeviceControlCommand{ get; set; }
    public ObservableCollection<Device> Devices { get; set; } = new ObservableCollection<Device>();

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

    public MuteVolumeCommand MuteVolumeCommand { get; set; }

    public PlayerVM()
    {
        ToggleDeviceControlCommand = new ToggleDeviceControlCommand(this);
        MuteVolumeCommand = new MuteVolumeCommand(this);
        VolumeSliderCommand = new VolumeSliderCommand(this);
        AppState.OnAuthorized += PlayerVMAuthorized;
    }

    private async void PlayerVMAuthorized(object? sender, EventArgs e)
    {
        await GetDevices();
    }

    private async Task GetDevices()
    {
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _getDevicesUrl);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
        HttpResponseMessage resp = await HttpClient.SendAsync(req);
        JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
        foreach (JToken token in obj["devices"])
        {
            Device device = JsonSerializer.Deserialize<Device>(token.ToString());
            Devices.Add(device);
        }
    }

    public void SetVolume(int volume)
    {
        if (IsMuted)
        {
            IsMuted = false;
            Volume = volume == 0 ? _lastVolume : volume;
        }
        else if (volume == 0)
        {
            _lastVolume = Volume;
            IsMuted = true;
            Volume = volume;
        }
        else
        {
            Volume = volume;
        }
    }

    public void ToggleDeviceWindowVisibility()
    {
        AppState.DeviceControlVisibility = AppState.DeviceControlVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
    }
}