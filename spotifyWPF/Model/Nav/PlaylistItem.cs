using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using spotifyWPF.Annotations;

namespace spotifyWPF.Model.Nav
{
    public class PlaylistItem : INotifyPropertyChanged
    {
        private bool _active;

        public bool Active
        {
            get { return _active; }
            set { _active = value; NotifyPropertyChanged(); }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(); }
        }
        
        public string Description{ get; set; }
        public string Owner { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
