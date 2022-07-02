using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace spotifyWPF.ViewModel
{
    public class RootVM : ViewModelBase
    {
        //public AppState AppStateRef
        //{
        //    get { return AppState; }
        //    set { AppState = value; NotifyPropertyChanged(); }
        //}
        private BlurEffect _winBlur;

        public bool Authorized
        {
            get { return AppState.Authorized; }
            set { AppState.Authorized = value; NotifyPropertyChanged(); }

        } 
        public BlurEffect WinBlur
        {
            get { return _winBlur; }
            set { _winBlur = value; }
        }

        public RootVM()
        {
            AppState.OnAuthorized += RootVMAuthorized;

        }

        private void RootVMAuthorized(object? sender, EventArgs e)
        {
            
        }
    }
}
