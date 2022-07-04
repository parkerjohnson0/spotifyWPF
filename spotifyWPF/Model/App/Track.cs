using System;

namespace spotifyWPF.Model.App;

public class Track 
{
   public string Title { get; set; } 
   public string Artist { get; set; }
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
         return $"{minutesLength}:{secondsLength}";
      }
   }
   public DateTime DateAdded { get; set; }
   public int ListIndex { get; set; }
}