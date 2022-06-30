using spotifyWPF.Model.Nav;
using spotifyWPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spotifyWPF.ViewModel
{
    public class NavVM : ViewModelBase
    {
        //todo. navbutton needs propchanged I think? so can update button icon on click
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
    }
}
