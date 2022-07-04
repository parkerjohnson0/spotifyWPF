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
   public string Duration { get; set; }
   public DateTime DateAdded { get; set; }
   public int ListIndex { get; set; }
}