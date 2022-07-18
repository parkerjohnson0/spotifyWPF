using spotifyWPF.Model;

namespace LocalDatabase;

public static class Queries
{
   public static string InsertTrack(Track track)
   {
       return $"INSERT INTO Tracks VALUES ({id},{title},{artist},{albumArt},{duration});";
   }
}