using System.ComponentModel;
using System.Runtime.CompilerServices;
using spotifyWPF.Model.Player;

namespace spotifyWPF.Model.App;

public class PlaybackState : INotifyPropertyChanged
{
   public Device Device { get; set; }

   private bool _isPlaying;
   public bool IsPlaying
   {
      get { return _isPlaying;  }
      set { _isPlaying = value; NotifyPropertyChanged(); }
   }
   private bool _shuffleState;

   public bool ShuffleState
   {
      get { return _shuffleState; }
      set { _shuffleState = value; NotifyPropertyChanged(); }
   }
   private bool _repeatState;

   public bool RepeatState 
   {
      get { return _repeatState; }
      set { _repeatState = value; NotifyPropertyChanged(); }
   }
   public Track Track { get; set; }
   public event PropertyChangedEventHandler? PropertyChanged;

   protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}