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

namespace spotifyWPF.ViewModel
{
    public class NavVM : ViewModelBase
    {
        //add ?limit to string to control how many playlists are retrieved. max is 50
        private string _playlistUrl = "https://api.spotify.com/v1/me/playlists?limit=50";
        public NavButton Home { get; set; }
        public NavButton Search { get; set; }
        public NavButton Library { get; set; }
        public HomeCommand HomeCommand { get; set; }
        public LibraryCommand LibraryCommand { get; set; }
        public SearchCommand SearchCommand { get; set; }
        public ObservableCollection<PlaylistItem> PlaylistItems {get;set;}

        public NavVM()
        {
            CreateButtons();
            AppState.OnAuthorized += NavVMAuthorized;
            PlaylistItems = new ObservableCollection<PlaylistItem>()
            {
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Playlist 1 Playlist 1 Playlist 1",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
                //
                //new PlaylistItem()
                //{
                //    Description = "Test",
                //    Name = "Retrowave/Synthwave Bullshit",
                //    Image = "https://mosaic.scdn.co/640/ab67616d0000b27359a428dc7ef8e0c12b0fe18aab67616d0000b2737aede4855f6d0d738012e2e5ab67616d0000b273c5649add07ed3720be9d5526ab67616d0000b273ebbff7725d3ce0739be01787",
                //    Owner="test"
                //    
                //},
            };
        }

        private async void NavVMAuthorized(object? sender, EventArgs e)
        {
            await getPlaylists();
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
        }
        internal void SearchClicked()
        {
            Home.Active = false;
            Search.Active = true;
            Library.Active = false;
        }

        internal void LibraryClicked()
        {
            Home.Active = false;
            Search.Active = false;
            Library.Active = true;
        }

        private async Task getPlaylists()
        {
            //loop through endpoint calls until next is null in the response
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _playlistUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AccessToken);
            HttpResponseMessage resp = await HttpClient.SendAsync(req);
            JObject obj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            foreach (JToken item in obj["items"])
            {
                PlaylistItems.Add(new PlaylistItem()
                {
                  Description = item["description"].ToString(),
                  Image = item.SelectToken("images[0].url").ToString(),
                  Name = item["name"].ToString(),
                  Owner = item.SelectToken("owner.display_name").ToString(),
                });    
            }
        }
    }
}
