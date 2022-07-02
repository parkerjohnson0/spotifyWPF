using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
        private static HttpClient _httpClient = new HttpClient();
        public static HttpClient HttpClient 
        {
            get { return _httpClient; }
            set { _httpClient = value; }

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
    //todo create authorized event that will trigger data fetching in view models?
    public class AppState  : INotifyPropertyChanged
    {
        public event EventHandler OnAuthorized; 
        private bool _authorized;

        public bool Authorized
        {
            get { return _authorized; }
            set
            {
                _authorized = value;
                 NotifyPropertyChanged();
                 if (_authorized)
                 {
                     OnAuthorized?.Invoke(this, EventArgs.Empty);
                 }
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