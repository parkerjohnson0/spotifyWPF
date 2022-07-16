using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using LocalDatabase;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.Nav;
using spotifyWPF.ViewModel.Commands;
using Track = spotifyWPF.Model.App.Track;

namespace spotifyWPF.ViewModel
{
    public class DisplayVM : ViewModelBase
    {
        private int _currOffset;
        private BlurEffect _winBlur;
        private LocalCache _cache;
        public BlurEffect WinBlur
        {
            get { return _winBlur; }
            set { _winBlur = value; }
        }

        private Track _selectedTrack;

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value;
                NotifyPropertyChanged();
            }
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

        public SelectTrackCommand SelectTrackCommand { get; set; }
        public MouseScrollCommand MouseScrollCommand { get; set; }

        public DisplayVM()
        {
            MouseScrollCommand = new MouseScrollCommand(this);
            SelectTrackCommand = new SelectTrackCommand(this);
            AppState.OnAuthorized += RootVMAuthorized;
            AppState.OnPlaylistDataUpdated += SelectedPlaylistItemChanged;
            _cache = LocalCache.Instance;
        }

        private void RootVMAuthorized(object? sender, EventArgs e)
        {
        }

        private async void SelectedPlaylistItemChanged(object? sender, EventArgs e)
        {
            SelectedPlaylistItem = AppState.SelectedPlaylistItem;
            if (SelectedPlaylistItem.SongsList.Count == 0)
            {
                //_tempTracks.Clear();
                await GetTracks(SelectedPlaylistItem.Link, 0,100);
                //foreach (Track track in _tempTracks.Take(50))
                //{
                //    SelectedPlaylistItem.SongsList.Add(track);
                //}

                NotifyPropertyChanged(nameof(SelectedPlaylistItem));
            }
        }

        //private ObservableCollection<Track> _tempTracks = new ObservableCollection<Track>();

        private async Task GetTracks(string apiUrl, int offset, int limit)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiUrl + $"?offset={offset}&limit={limit}");
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
                    DateAdded = DateTime.Parse(item.SelectToken("added_at")?.ToString()),
                    ListIndex = ++offset,
                    DurationMS = (long)item.SelectToken("track.duration_ms"),
                    ID = item.SelectToken("track.id").ToString()
                });
            }
            
            if (SelectedPlaylistItem.SongsList.Count > limit)
            {
                RemoveFromStart(limit);
            }

            _currOffset = offset;
            // foreach (Track track in _tempTracks.Take(new Range(SelectedPlaylistItem.SongsList.Count, SelectedPlaylistItem.SongsList.Count+50)))
            // {
            //     SelectedPlaylistItem.SongsList.Add(track);
            // }
            //string next;
            //if ((next = obj.SelectToken("next")?.ToString()) != String.Empty)
            //{
            //    await Task.Delay(500);
            //    await GetTracks(next);
            //}
        }

        private void RemoveFromStart(int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                SelectedPlaylistItem.SongsList.RemoveAt(0);
            }
        }

        public void PlaylistScrolledDown()
        {
            // foreach (Track track in _tempTracks.Take(new Range(SelectedPlaylistItem.SongsList.Count, SelectedPlaylistItem.SongsList.Count + 10)))
            // {
            //     SelectedPlaylistItem.SongsList.Add(track);
            // }
            // foreach (Track track in SelectedPlaylistItem.SongsList.Take(new Range(0, 10)))
            // {
            //     SelectedPlaylistItem.SongsList.Remove(track);
            // }
        }

        public void PlaylistScrolledUp()
        {
            //foreach (Track track in SelectedPlaylistItem.SongsList.Take(new Range(SelectedPlaylistItem.SongsList.Count - 10, SelectedPlaylistItem.SongsList.Count)))
            //{
            //    SelectedPlaylistItem.SongsList.Remove(track);
            //}
            // foreach (Track track in _tempTracks.Take(new Range(_tempTracks.IndexOf(SelectedPlaylistItem.SongsList[0]), _tempTracks.IndexOf(SelectedPlaylistItem.SongsList[0]) - 10)))
            // {
            //     SelectedPlaylistItem.SongsList.Add(track);
            // }
        }

        public async Task LoadSongs()
        {
            await GetTracks(SelectedPlaylistItem.Link, _currOffset, 25);
        }
    }
}