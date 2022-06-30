using System.Security.AccessControl;
using System.Threading.Tasks;
using spotifyWPF.ViewModel.Commands;

namespace spotifyWPF.ViewModel
{
    public class AuthorizeVM : ViewModelBase
    {
        public AuthorizeCommand AuthorizeCommand { get; set; }
        private string _authText = "This application requires the user to authorize with the Spotify API";

        //public bool Authorized
        //{
        //    get { return AppState.Authorized; }
        //    set { AppState.Authorized = value; NotifyPropertyChanged(); }

        //} 
        public string AuthText
        {
            get { return _authText; }
            set
            {
                _authText = value;
                NotifyPropertyChanged();
            }
        }
        private bool _authorizing;

        public bool Authorizing
        {
            get { return _authorizing; }
            set
            {
                _authorizing = value;
                NotifyPropertyChanged();
            }
        }

        public AuthorizeVM()
        {
            AuthorizeCommand = new AuthorizeCommand(this);
        }

        public async Task Authorize()
        {
            AuthText = "Authorizing...";
            Authorizing = true;
            AuthServer.AuthServer server = new AuthServer.AuthServer();
            server.OpenBrowser();
            bool res = await server.Start();
            if (!res)
            {
               AuthText =  "This application requires the user to authorize with the Spotify API";
                Authorizing = false;
                return;
            }

            AppState.AccessToken = await server.getAccessToken();
           AppState.Authorized = true;
            
        }
    }
}