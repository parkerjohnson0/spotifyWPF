using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using spotifyWPF.Annotations;

namespace spotifyWPF.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static AppState _appState = new AppState();

        public static AppState AppState
        {
            get { return _appState; }
            set
            {
                _appState = value;
            }
        }

        public ViewModelBase()
        {
        }
    }

    public class AppState  : INotifyPropertyChanged
    {
        private bool _authorized;

        public bool Authorized
        {
            get { return _authorized; }
            set
            {
                _authorized = value;
                 NotifyPropertyChanged();
            }
        }

        private string _accessToken;

        public string AccessToken
        {
            get { return _accessToken; }
            set
            {
                _accessToken = value;
                NotifyPropertyChanged();
            }
        }
         public event PropertyChangedEventHandler? PropertyChanged;

         private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
         {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }
    }
}