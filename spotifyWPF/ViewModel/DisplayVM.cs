using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.App;
using spotifyWPF.Model.Nav;

namespace spotifyWPF.ViewModel
{
    public class DisplayVM : ViewModelBase
    {
        private BlurEffect _winBlur;

        public BlurEffect WinBlur
        {
            get { return _winBlur; }
            set { _winBlur = value; }
        }

        private PlaylistItem _selectedPlaylistItem = new PlaylistItem()
        {
            Active = true,
            Image = "https://i.scdn.co/image/ab67706c0000bebb18b830788ea8914e1da3292d",
            Link = "https://api.spotify.com/v1/playlists/3ebHKSjHujS4Tyt2KKP97R/tracks",
            Owner = "Cheerzo",
            Name = "Retrowave Synthwave",
            Description =
                "This is a test description of a spotify playlist, the text should wrap to the next line if it is too long",
            SongsList = new ObservableCollection<Track>()
        };

        public PlaylistItem SelectedPlaylistItem
        {
            get { return _selectedPlaylistItem; }
            set
            {
                _selectedPlaylistItem = value;
                if (_selectedPlaylistItem != null) ;
                NotifyPropertyChanged();
            }
        }

        public DisplayVM()
        {
            AppState.OnAuthorized += RootVMAuthorized;
            AppState.OnPlaylistDataUpdated += SelectedPlaylistItemChanged;
        }

        private void RootVMAuthorized(object? sender, EventArgs e)
        {
        }

        private async void SelectedPlaylistItemChanged(object? sender, EventArgs e)
        {
            SelectedPlaylistItem = AppState.SelectedPlaylistItem;
            if (SelectedPlaylistItem.SongsList.Count == 0)
            {
               await GetTracks(SelectedPlaylistItem.Link);
            }
        }

        private async Task GetTracks(string apiUrl)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
            HttpResponseMessage resp = await HttpClient.SendAsync(req);
            JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            foreach (JToken item in obj["items"])
            {
                SelectedPlaylistItem.SongsList.Add(new Track()
                {
                    Artist = item.SelectToken("track.album.artists[0].name")?.ToString(),
                    Album = item.SelectToken("track.album.name")?.ToString(),
                    Title = item.SelectToken("track.name")?.ToString(),
                    AlbumArt = item.SelectToken("track.album.images[0].url")?.ToString(),
                    DateAdded = DateTime.Parse( item.SelectToken("added_at")?.ToString()),
                   ListIndex = SelectedPlaylistItem.SongsList.Count + 1,
                   DurationMS = (long) item.SelectToken("track.duration_ms")
                });
            }
        }
    }
}