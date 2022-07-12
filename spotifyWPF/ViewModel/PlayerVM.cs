using System;
using System.Collections.ObjectModel;
using System.Net;
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
    private string _startPlaybackUrl = "https://api.spotify.com/v1/me/player/play?device_id=";
    public VolumeSliderCommand VolumeSliderCommand { get; set; }
    private bool _isPlaying;

    public bool IsPlaying
    {
        get { return _isPlaying; }
        set { _isPlaying = value; NotifyPropertyChanged(); }
    } 
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

    public StartPlaybackCommand StartPlaybackCommand { get; set; }
    public PlayerVM()
    {
        StartPlaybackCommand = new StartPlaybackCommand(this);
        ToggleDeviceControlCommand = new ToggleDeviceControlCommand(this);
        MuteVolumeCommand = new MuteVolumeCommand(this);
        VolumeSliderCommand = new VolumeSliderCommand(this);
    //    AppState.OnAuthorized += PlayerVMAuthorized;
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

    public void ToggleDeviceWindowVisibility(Visibility state)
    {
        AppState.DeviceControlVisibility = state == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
    }

    public async Task<bool> StartPlayback()
    {
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, _startPlaybackUrl + AppState.SelectedDevice.ID);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
        HttpResponseMessage resp = await HttpClient.SendAsync(req);
        IsPlaying = resp.StatusCode == HttpStatusCode.NoContent;
        return resp.StatusCode == HttpStatusCode.NoContent;
    }
}