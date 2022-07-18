using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace spotifyWPF.Model.App;

public class Track  : INotifyPropertyChanged
{
    public string SpotifyID { get; set; }
   public string Title { get; set; } 
   public string Artist { get; set; }
   private bool _active;

   public bool Active
   {
      get { return _active;}
      set { _active = value; NotifyPropertyChanged(); }
   }
   public string Album { get; set; }
   public string AlbumArt { get; set; }
   public long DurationMS { get; set; }

   /// <summary>
   /// Formatted mm:ss duration
   /// </summary>
   public string Duration {
      get
      {
         long seconds = DurationMS / 1000;
         long minutesLength = seconds / 60;
         long secondsLength = seconds % 60;
         string pad = secondsLength < 10 ? "0" : "";
         return $"{minutesLength}:{pad}{secondsLength}";
      }
   }
   public DateTime DateAdded { get; set; }
   public int ListIndex { get; set; }
   public string PlaylistID { get; set; }

   public event PropertyChangedEventHandler? PropertyChanged;

   protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}