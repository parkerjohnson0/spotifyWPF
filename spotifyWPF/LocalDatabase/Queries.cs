using spotifyWPF.Model.App;
using spotifyWPF.Model.Nav;

namespace spotifyWPF.LocalDatabase;

public static class Queries
{
   public static string GetInsertTrack(Track track)
   {
       //return $"INSERT INTO Tracks (SpotifyID, Title, Artist,Album, AlbumArt, DateAdded, ListIndex," +
       //       $"DurationMS) VALUES ('{track.SpotifyID}'," +
       //       $"'{track.Title}','{track.Artist}','{track.Album}','{track.AlbumArt}'," +
       //       $"{track.DateAdded},{track.ListIndex},{track.DurationMS});";
       return "INSERT INTO Tracks (SpotifyID,PlaylistID, Title, Artist, Album,AlbumArt," +
              "DateAdded,ListIndex,DurationMS) VALUES ($SpotifyID,$PlaylistID," +
              "$Title,$Artist,$Album,$AlbumArt," +
              "$DateAdded,$ListIndex,$DurationMS);";
   }

   public static string GetInsertPlaylist(PlaylistItem playlist)
   {
       return "INSERT INTO Playlists (SpotifyID, Name, Description,Link," +
              "Image,Length,Owner) VALUES ($SpotifyID," +
              "$Name,$Description,$Link," +
              "$Image,$Length,$Owner);";
   }
   public static string GetSelectNextTracks(int index, string playlistID,int amount)
   {
       return $"SELECT * FROM TRACKS WHERE ListIndex>{index} AND ListIndex<{index + amount} AND " +
              $"PlaylistID='{playlistID}';";
   }
}