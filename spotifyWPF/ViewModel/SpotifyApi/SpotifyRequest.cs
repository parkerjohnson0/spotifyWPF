using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
                SpotifyID = obj.SelectToken("item.id").ToString()
            },
            RepeatState = (RepeatState)Enum.Parse(typeof(RepeatState), obj.SelectToken("repeat_state").ToString()),
            ShuffleState = Boolean.Parse(obj.SelectToken("shuffle_state").ToString()),
            ProgressMS = (long)obj.SelectToken("progress_ms")
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
}