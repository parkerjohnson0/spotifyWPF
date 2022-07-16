using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.App;
using spotifyWPF.Model.Player;

namespace spotifyWPF.ViewModel;

public class DevicesVM : ViewModelBase
{
    internal class SelectedDeviceArgs : EventArgs
    {
        public Device Device { get; set; }

        public SelectedDeviceArgs(Device device)
        {
            Device = device;
        }
    }

    private EventHandler<SelectedDeviceArgs> OnSelectedDeviceChanged;
    private bool _deviceControlFocused;

    private string _getDevicesUrl = "https://api.spotify.com/v1/me/player/devices";
    private string _playerUrl = "https://api.spotify.com/v1/me/player";
    public ObservableCollection<Device> Devices { get; set; } = new ObservableCollection<Device>();
    private Device _selectedDevice;

    public Device SelectedDevice
    {
        get { return _selectedDevice; }
        set
        {
            //shouldnt select device if transerplayback fails for whatever reason. though xaml IsSelected might still change
            OnSelectedDeviceChanged?.Invoke(this, new SelectedDeviceArgs(value));
            NotifyPropertyChanged();
        }
    }


    public bool DeviceControlFocused
    {
        get { return _deviceControlFocused; }
        set
        {
            _deviceControlFocused = value;
            NotifyPropertyChanged();
        }
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
        OnSelectedDeviceChanged += SelectedDeviceChanged;
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Devices.Add(new Device()
            {
                Name = "Computer",
                Type = "Computer",
                Volume = 50,
                ID = "1234",
                IsActive = true,
                IsRestricted = false,
                IsPrivateSession = false
            });
            Devices.Add(new Device()
            {
                Name = "Computer 2",
                Type = "Computer",
                Volume = 50,
                ID = "1234",
                IsActive = true,
                IsRestricted = false,
                IsPrivateSession = false
            });
        }

        AppState.OnAuthorized += DevicesVMAuthorized;
        AppState.OnDeviceControlClicked += (sender, args) => Visibility = AppState.DeviceControlVisibility;
        AppState.OnDeviceControlUnfocused += (sender, args) => Visibility = AppState.DeviceControlVisibility;
    }

    private async void SelectedDeviceChanged(object? sender, SelectedDeviceArgs e)
    {
        if (!await TransferPlayback(e.Device)) return;
        await Task.Delay(1000);
        _selectedDevice = e.Device;
        AppState.PlaybackState = await GetPlaybackState();
        AppState.SelectedDevice = _selectedDevice;
    }

    private async Task<PlaybackState> GetPlaybackState()
    {
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _playerUrl);

        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
        HttpResponseMessage resp = await HttpClient.SendAsync(req);
        JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
        PlaybackState playbackState = new PlaybackState()
        {
            IsPlaying = Boolean.Parse(obj.SelectToken("is_playing").ToString()),
            Device = new Device()
            {
                ID = obj.SelectToken("device.id").ToString(),
                Volume = Int32.Parse(obj.SelectToken("device.volume_percent").ToString()),
                Name = obj.SelectToken("device.name").ToString()
            },
            Track = new Track()
            {
                Album = obj.SelectToken("item.album.name").ToString(),
                Artist = obj.SelectToken("item.artists[0].name").ToString(),
                Title = obj.SelectToken("item.name").ToString(),
                AlbumArt = obj.SelectToken("item.album.images[0].url").ToString(),
                DurationMS = long.Parse(obj.SelectToken("item.duration_ms").ToString()),
                ID = obj.SelectToken("item.id").ToString()
            },
            RepeatState = (RepeatState) Enum.Parse(typeof(RepeatState), obj.SelectToken("repeat_state").ToString()),
            ShuffleState = Boolean.Parse(obj.SelectToken("shuffle_state").ToString()),
            ProgressMS = (long) obj.SelectToken("progress_ms")
            
        };
        return playbackState;
    }


    private async Task<bool> TransferPlayback(Device device)
    {
        var body = new
        {
            device_ids = new string[] { device.ID }
        };
        var json = JsonSerializer.Serialize(body);
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, _playerUrl);

        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
        req.Content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage resp = await HttpClient.SendAsync(req);
        return resp.StatusCode == HttpStatusCode.NoContent;
    }

    private async void DevicesVMAuthorized(object? sender, EventArgs e)
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
}