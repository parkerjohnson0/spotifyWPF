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
using Newtonsoft.Json.Linq;
using spotifyWPF.LocalDatabase;
using spotifyWPF.Model.Nav;
using spotifyWPF.ViewModel.Commands;
using spotifyWPF.ViewModel.SpotifyApi;
using Track = spotifyWPF.Model.App.Track;

namespace spotifyWPF.ViewModel
{
    //todo rename displayVM to playlistVM
    public class DisplayVM : ViewModelBase
    {
        private int _currOffset;
        private int _amountPerScroll = 10;
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

        private List<Track> _trackCache = new List<Track>();

        public SelectTrackCommand SelectTrackCommand { get; set; }
        public MouseScrollCommand MouseScrollCommand { get; set; }
        public PlayTrackCommand PlayTrackCommand { get; set; }

        public DisplayVM()
        {
            MouseScrollCommand = new MouseScrollCommand(this);
            PlayTrackCommand = new PlayTrackCommand(this);
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
                await GetTracks(SelectedPlaylistItem.Link);
                _currOffset = SelectedPlaylistItem.SongsList.Count - _amountPerScroll;
                //foreach (Track track in _tempTracks.Take(50))
                //{
                //    SelectedPlaylistItem.SongsList.Add(track);
                //}

                NotifyPropertyChanged(nameof(SelectedPlaylistItem));
            }
        }

        //private ObservableCollection<Track> _tempTracks = new ObservableCollection<Track>();

        private async Task GetTracks(string apiUrl)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
            HttpResponseMessage resp = await HttpClient.SendAsync(req);
            JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            foreach (JToken item in obj["items"])
            {
                Track track = new Track()
                {
                    Artist = item.SelectToken("track.album.artists[0].name")?.ToString() ?? "",
                    Album = item.SelectToken("track.album.name")?.ToString() ?? "",
                    Title = item.SelectToken("track.name")?.ToString() ?? "",
                    AlbumArt = item.SelectToken("track.album.images[0].url")?.ToString() ?? "",
                    DateAdded = DateTime.Parse(item.SelectToken("added_at")?.ToString()),
                    ContextPosition = SelectedPlaylistItem.SongsList.Count + 1,
                    DurationMS = (long)item.SelectToken("track.duration_ms"),
                    SpotifyID = item.SelectToken("track.id").ToString(),
                    PlaylistID = SelectedPlaylistItem.SpotifyID,
                };
                SelectedPlaylistItem.SongsList.Add(track);
                _trackCache.Add(track);
            }
           // if (SelectedPlaylistItem.SongsList.Count > limit)
           // {
           //     RemoveFromStart(limit);
           // }

           // _currOffset = offset;
            // foreach (Track track in _tempTracks.Take(new Range(SelectedPlaylistItem.SongsList.Count, SelectedPlaylistItem.SongsList.Count+50)))
            // {
            //     SelectedPlaylistItem.SongsList.Add(track);
            // }
            string next;
            if ((next = obj.SelectToken("next")?.ToString()) != String.Empty)
            {
                 await Task.Delay(500);
                Task.Run(()=>CachePlaylist(next));
                // await GetTracks(next);
            }
        }

        private async void CachePlaylist(string api)
        {
            
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, api);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
            HttpResponseMessage resp = await HttpClient.SendAsync(req);
            JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            foreach (JToken item in obj["items"])
            {
                Track track = new Track()
                {
                    Artist = item.SelectToken("track.album.artists[0].name")?.ToString() ?? "",
                    Album = item.SelectToken("track.album.name")?.ToString() ?? "",
                    Title = item.SelectToken("track.name")?.ToString() ?? "",
                    AlbumArt = item.SelectToken("track.album.images[0].url")?.ToString() ?? "",
                    DateAdded = DateTime.Parse(item.SelectToken("added_at")?.ToString()),
                    ContextPosition = _trackCache.Count + 1,
                    DurationMS = (long)item.SelectToken("track.duration_ms"),
                    SpotifyID = item.SelectToken("track.id").ToString(),
                    PlaylistID = SelectedPlaylistItem.SpotifyID,
                };
                _trackCache.Add(track);
            }

            string next;
            if ((next = obj.SelectToken("next")?.ToString()) != String.Empty)
            {
                await Task.Delay(100);
                Task.Run(()=>CachePlaylist(next));
                // await GetTracks(next);
            }
            //should only run once playlist has been completely cached in memory.
            //save to sqlite
            else
            {
                _cache.Cache(_trackCache);
                _trackCache.Clear();
            }
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
            _currOffset += _amountPerScroll;
            List<Track> cachedTracks = await _cache.Load(_currOffset,SelectedPlaylistItem.SpotifyID,_amountPerScroll);
            await Task.Run(() =>
            {
                foreach (Track track in cachedTracks)
                {
                    App.Current.Dispatcher.BeginInvoke(new Action(()=>
                    {
                        SelectedPlaylistItem.SongsList.RemoveAt(0);
                        SelectedPlaylistItem.SongsList.Add(track);
                    }));
                }
            });
            //await GetTracks(SelectedPlaylistItem.Link);
        }

        public void PlaySelectedTrack(Track track)
        {
            AppState.PlaySong(track);
        }
        public void PauseSelectedTrack(Track track)
        {
            AppState.PauseSong(track);
        }
    }
}