using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;
using spotifyWPF.Model.App;

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
        public RootVM()
        {
            AppState.OnAuthorized += RootVMAuthorized;
        }
        private void RootVMAuthorized(object? sender, EventArgs e)
        {
        }
    }
}