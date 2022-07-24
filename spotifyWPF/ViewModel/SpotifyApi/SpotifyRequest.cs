using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.App;
using spotifyWPF.Model.Player;

namespace spotifyWPF.ViewModel.SpotifyApi;

public class SpotifyRequest
{
    private string _accessToken;
    public SpotifyRequest(string accessToken)
    {
        _accessToken = accessToken;
    }
    private  string _playerUrl = "https://api.spotify.com/v1/me/player/";
    private  string _userUrl = "https://api.spotify.com/v1/me/";
    private string _shufflePlaybackUrl = "https://api.spotify.com/v1/me/player/shuffle?state=";
    private string _changeRepeatStateUrl = "https://api.spotify.com/v1/me/player/repeat?state=";
    private  HttpClient _httpClient = new HttpClient();
    public async Task<PlaybackState> GetPlaybackState()
    {
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _playerUrl);

        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        HttpResponseMessage resp = await _httpClient.SendAsync(req);
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
                SpotifyID = obj.SelectToken("item.id").ToString(),
                PlaylistID = obj.SelectToken("context.uri") is null ? "" : obj.SelectToken("context.uri").ToString().Split(":")[2]
            },
            RepeatState = (RepeatState)Enum.Parse(typeof(RepeatState), obj.SelectToken("repeat_state").ToString()),
            ShuffleState = Boolean.Parse(obj.SelectToken("shuffle_state").ToString()),
            ProgressMS = (long)obj.SelectToken("progress_ms"),
            ContextType = obj.SelectToken("context.type") is null ? ContextType.none :  (ContextType) Enum.Parse(typeof(ContextType),obj.SelectToken("context.type")?.ToString()),
        };
        return playbackState;
    }
    public async Task<bool> NextSong()
    {
        string url = _playerUrl + "next";
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        HttpResponseMessage resp = await _httpClient.SendAsync(req);
        //if successful, flip isplaying
        return resp.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<bool> PreviousSong()
    {
        string url = _playerUrl + "previous";
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        HttpResponseMessage resp = await _httpClient.SendAsync(req);
        //if successful, flip isplaying
        return resp.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task PlaySong(Track track, Device selectedDevice)
    {
        await ToggleSong(track, selectedDevice, "play", 0);
    }

    private async Task<bool> ToggleSong(Track track, Device selectedDevice, string endPoint, long position)
    {
        //if (!selectedDevice.IsActive) return false;
        
        string url = _playerUrl + $"{endPoint}?device_id={selectedDevice.ID}";
        var body = new
        {
            //uris = new string[] { $"spotify:track:{track.SpotifyID}" },
            context_uri = $"spotify:playlist:{track.PlaylistID}",
            offset= new
            {
                //todo rename this to ContextPosition 
                //todo will have to do this after figure out a different scheme for listing the numbers in each playlist listviewitem.
               position=track.ContextPosition - 1 
            },
           position_ms = position 
            
        };
        string json = System.Text.Json.JsonSerializer.Serialize(body);
        
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        req.Content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage resp = await _httpClient.SendAsync(req);
        //if successful, flip isplaying
        return resp.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task PauseSong(Track track, Device selectedDevice)
    {
        await ToggleSong(track, selectedDevice, "pause", 0);
    }

    public async Task ResumeSong(Track track, Device selectedDevice, long progress)
    {
        await ToggleSong(track, selectedDevice, "play", progress);
    }

    public async Task<bool> ToggleShuffle(bool shuffleState)
    {
        //pass in flipped bool
        string url = _shufflePlaybackUrl + $"{!shuffleState}";
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        HttpResponseMessage resp = await _httpClient.SendAsync(req);
        //if successful, flip isplaying
        return resp.StatusCode == HttpStatusCode.NoContent;
    }
    
    /// <summary>
    /// Pass in current state, then increments
    /// </summary>
    /// <param name="currRepeatState"></param>
    /// <returns></returns>
    public async Task<bool> ChangeRepeatState(RepeatState currRepeatState)
    {
        RepeatState nextState = (RepeatState) (((int)currRepeatState + 1) % Enum.GetNames(typeof(RepeatState)).Length); 
        string url = _changeRepeatStateUrl + nextState;
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        HttpResponseMessage resp = await _httpClient.SendAsync(req);
        return resp.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<User> GetUser()
    {
        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _userUrl);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        HttpResponseMessage resp = await _httpClient.SendAsync(req);
        if (resp.StatusCode == HttpStatusCode.OK)
        {
            JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            return new User
            {
                UserName = obj.SelectToken("display_name").ToString(),
                UserImage= obj.SelectToken("images[0].url").ToString()
            };
        }
        return new User();
    }
}