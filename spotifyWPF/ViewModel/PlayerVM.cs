using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.App;
using spotifyWPF.Model.Player;
using spotifyWPF.ViewModel.Commands;

namespace spotifyWPF.ViewModel;

public class PlayerVM : ViewModelBase
{
    private string _getDevicesUrl = "https://api.spotify.com/v1/me/player/devices";
    private string _startPlaybackUrl = "https://api.spotify.com/v1/me/player/play?device_id=";
    private string _pausePlaybackUrl = "https://api.spotify.com/v1/me/player/pause?device_id=";
    public VolumeSliderCommand VolumeSliderCommand { get; set; }
    private bool _isPlaying;

    public bool IsPlaying
    {
        get { return _isPlaying; }
        set
        {
            _isPlaying = value;
            NotifyPropertyChanged();
        }
    }

    private bool _isMuted;

    public bool IsMuted
    {
        get { return _isMuted; }
        set
        {
            _isMuted = value;
            NotifyPropertyChanged();
        }
    }

    private int _lastVolume;
    private int _volume = 50;

    public ToggleDeviceControlCommand ToggleDeviceControlCommand { get; set; }
    public ObservableCollection<Device> Devices { get; set; } = new ObservableCollection<Device>();

    private PlaybackState _playbackState;

    public PlaybackState PlaybackState
    {
        get { return _playbackState; }
        set
        {
            _playbackState = value;
            NotifyPropertyChanged();
        }
    }

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

    public TogglePlaybackCommand TogglePlaybackCommand { get; set; }

    public PlayerVM()
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            _playbackState = new PlaybackState()
            {
                Device = new Device()
                {
                    Volume = 50
                },
                Track = new Track()
                {
                    Album = "Test",
                    Title = "Song name",
                    Artist = "Artist Name",
                    AlbumArt = "https://i.scdn.co/image/ab67616d0000b273ca320286836abf81dd87c9bf",
                    DateAdded = DateTime.Now,
                    DurationMS = 227075
                },
                ProgressMS = 20000,
                
            };
        }

        AppState.OnPlaybackStateChanged += UpdatePlaybackState;
            TogglePlaybackCommand = new TogglePlaybackCommand(this);
        ToggleDeviceControlCommand = new ToggleDeviceControlCommand(this);
        MuteVolumeCommand = new MuteVolumeCommand(this);
        VolumeSliderCommand = new VolumeSliderCommand(this);
        //    AppState.OnAuthorized += PlayerVMAuthorized;
    }

    private void UpdatePlaybackState(object? sender, EventArgs e)
    {
        PlaybackState = AppState.PlaybackState;
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
        string url = PlaybackState.IsPlaying ? _pausePlaybackUrl : _startPlaybackUrl;
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, url + AppState.SelectedDevice.ID);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
        HttpResponseMessage resp = await HttpClient.SendAsync(req);
        //if successful, flip isplaying
        if (resp.StatusCode == HttpStatusCode.NoContent)
        {
            PlaybackState.IsPlaying = !PlaybackState.IsPlaying;
        }
        return resp.StatusCode == HttpStatusCode.NoContent;
    }

}