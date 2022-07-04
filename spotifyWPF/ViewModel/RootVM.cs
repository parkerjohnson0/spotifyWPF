using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;
using spotifyWPF.Model.App;
using spotifyWPF.Model.Nav;

namespace spotifyWPF.ViewModel
{
    public class RootVM : ViewModelBase
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
            Description = "This is a test description of a spotify playlist, the text should wrap to the next line if it is too long",
            SongsList = new ObservableCollection<Track>()
            {
                new Track()
                {
                   AlbumArt = "https://i.scdn.co/image/ab67616d0000b2735c3ade87b509abf9926171a1",
                   Album = "Twin Cinema",
                   Artist = "The New Pornographers",
                   DurationMS = 267146,
                   Title = "THe Bleeding Heart Show",
                   DateAdded = DateTime.Parse( "2019-12-22T07:03:59Z"),
                   ListIndex = 1
                },
                new Track()
                {
                   AlbumArt = "https://i.scdn.co/image/ab67616d0000b2735c3ade87b509abf9926171a1",
                   Artist = "The New Pornographers",
                   Album = "Twin Cinema",
                   DurationMS = 267146,
                   Title = "THe Bleeding Heart Show",
                   DateAdded = DateTime.Parse( "2019-12-22T07:03:59Z"), 
                   ListIndex = 2
                },
                new Track()
                {
                   AlbumArt = "https://i.scdn.co/image/ab67616d0000b2735c3ade87b509abf9926171a1",
                   Artist = "The New Pornographers",
                   Album = "Twin Cinema",
                   DurationMS = 267146,
                   Title = "THe Bleeding Heart Show",
                   DateAdded = DateTime.Parse( "2019-12-22T07:03:59Z"),
                   ListIndex = 3
                },
            }
            
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
        public RootVM()
        {
            AppState.OnAuthorized += RootVMAuthorized;
            AppState.OnPlaylistDataUpdated += SelectedPlaylistItemChanged;
        }
        private void RootVMAuthorized(object? sender, EventArgs e)
        {
        }

        private void SelectedPlaylistItemChanged(object? sender, EventArgs e)
        {
            SelectedPlaylistItem = AppState.SelectedPlaylistItem;
            if (SelectedPlaylistItem.SongsList == null)
            {
                SelectedPlaylistItem.SongsList = new ObservableCollection<Track>();
                SelectedPlaylistItem.SongsList.Add(new Track()
                {
                    
                });
            }
        }
    }
}