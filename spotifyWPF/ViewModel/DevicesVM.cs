using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.Player;

namespace spotifyWPF.ViewModel;

public class DevicesVM : ViewModelBase
{
    private bool _deviceControlFocused;

    private string _getDevicesUrl = "https://api.spotify.com/v1/me/player/devices";
    public ObservableCollection<Device> Devices { get; set; } = new ObservableCollection<Device>();
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