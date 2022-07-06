using spotifyWPF.Model.Nav;
using spotifyWPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using spotifyWPF.Model.App;

namespace spotifyWPF.ViewModel
{
    public class NavVM : ViewModelBase
    {
        //add ?limit to string to control how many playlists are retrieved. max is 50
        private string _userPlaylistUrl = "https://api.spotify.com/v1/me/playlists?limit=50";
        public NavButton Home { get; set; }
        public NavButton Search { get; set; }
        public NavButton Library { get; set; }
        public HomeCommand HomeCommand { get; set; }
        public LibraryCommand LibraryCommand { get; set; }
        public SearchCommand SearchCommand { get; set; }
        public SelectPlaylistCommand SelectPlaylistCommand { get; set; }
        public ObservableCollection<PlaylistItem> PlaylistItems {get;set;}

        public bool Authorized
        {
            get { return AppState.Authorized; }
        }

        public NavVM()
        {
            CreateButtons();
            SelectPlaylistCommand = new SelectPlaylistCommand(this); 
            AppState.OnAuthorized += NavVMAuthorized;
            PlaylistItems = new ObservableCollection<PlaylistItem>();
        }

        private async void NavVMAuthorized(object? sender, EventArgs e)
        {
            await GetPlaylists();
        }

        private void CreateButtons()
        {
            Home = new NavButton("Home");
            Search = new NavButton("Search");
            Library = new NavButton("Your Library");
            HomeCommand = new HomeCommand(this);
            LibraryCommand = new LibraryCommand(this);
            SearchCommand = new SearchCommand(this);
        }

        internal void HomeClicked()
        {
            Home.Active = true;
            Search.Active = false;
            Library.Active = false;
            AppState.RootTemplate = RootTemplate.Home;
        }
        internal void SearchClicked()
        {
            Home.Active = false;
            Search.Active = true;
            Library.Active = false;
            AppState.RootTemplate = RootTemplate.Search;
        }

        internal void LibraryClicked()
        {
            Home.Active = false;
            Search.Active = false;
            Library.Active = true;
            AppState.RootTemplate = RootTemplate.Library;
        }

        private async Task GetPlaylists()
        {
            //TODO loop through endpoint calls until next is null in the response
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _userPlaylistUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
            HttpResponseMessage resp = await HttpClient.SendAsync(req);
            JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            foreach (JToken item in obj["items"])
            {
                PlaylistItems.Add(new PlaylistItem()
                {
                  Description = item["description"].ToString(),
                  Image = item.SelectToken("images[0].url")?.ToString(),
                  Name = item["name"].ToString(),
                  Owner = item.SelectToken("owner.display_name").ToString(),
                  Link = item.SelectToken("tracks.href").ToString(),
                  SongsList = new ObservableCollection<Track>()
                });    
            }
        }
        public void SelectPlaylist(PlaylistItem item)
        {
            //if already active dont return, else running method will cause bad things to happen
            if (item.Active) return;
            foreach (PlaylistItem playlistItem in PlaylistItems)
            {
                playlistItem.Active = false;
            }
            item.Active = true;
            AppState.RootTemplate = RootTemplate.Playlist;
            AppState.SelectedPlaylistItem = item;
        }
    }
}
