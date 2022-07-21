using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using spotifyWPF.Model.App;
using spotifyWPF.Model.Nav;

namespace spotifyWPF.LocalDatabase
{
    public sealed class LocalCache
    {
        private static LocalCache _instance = null;
        private static readonly object _lock = new object();
        private SqliteConnection _conn;

        public LocalCache()
        {
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
            Directory.CreateDirectory(path + "\\spotifyWPF");
            _conn = new SqliteConnection($"Data Source={path}\\spotifyWPF\\local.sqlite");
            InitDB();
        }

        public static LocalCache Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new LocalCache();
                    }

                    return _instance;
                }
            }
        }

        private void InitDB()
        {
            _conn.Open();
            using (SqliteTransaction tran = _conn.BeginTransaction())
            {
                string createTables = @" 
                    DROP TABLE IF EXISTS Playlists;
                    CREATE TABLE Playlists (
                    SpotifyID	varchar(100),
                    SpotifyURI	varchar(100),
                    Name	varchar(100),
                    Description	varchar(100),
                    Link	varchar(256),
                    Image	varchar(256),
                    Length int,
                    Owner varchar(256),
                    ID INTEGER,
                    PRIMARY KEY(ID));
                   DROP TABLE IF EXISTS Tracks; 
                    CREATE TABLE Tracks (
                   SpotifyID	varchar(100),
                   PlaylistID varchar(100),
                   Title	varchar(100),
                   Artist	varchar(100),
                   Album	varchar(100),
                   AlbumArt	varchar(256),
                    DateAdded Date,
                    ContextPosition int,
                   DurationMS	bigint,
                   ID	INTEGER,
                   PRIMARY KEY(ID));
                    
                    CREATE TABLE IF NOT EXISTS PlaylistTrack(
                        PlaylistID INTEGER,
                        TrackID INTEGER
                    );";
//                string trackTable = "CREATE TABLE IF NOT EXISTS Track (ID varchar(100), Title varchar(100), Artist varchar(100)," +
//                "Album varchar(100), AlbumArt varchar(256), DurationMS bigint);" +
//                "CREATE TABLE IF NOT EXISTS Playlist (ID varchar(100), Name varchar(100),Description varchar(100), " +
//                "Artist varchar(100), Link varchar(256), Image varchar(256));"+
//                "CREATE TABLE PlaylistTracks";

                SqliteCommand sqliteCommand = new SqliteCommand(createTables, _conn, tran);
                sqliteCommand.ExecuteNonQuery();
                tran.Commit();
            }

            _conn.Close();
        }

        public void Cache(List<PlaylistItem> playlists)
        {
            
           _conn.Open();
           using (SqliteTransaction tran = _conn.BeginTransaction())
           {
               string insert = "";
               foreach (PlaylistItem playlist in playlists)
               {
                   insert = Queries.GetInsertPlaylist(playlist);
                   SqliteCommand command = new SqliteCommand(insert, _conn, tran);
                   command.Parameters.Add("$SpotifyID", SqliteType.Text).Value=playlist.SpotifyID;
                   command.Parameters.Add("$Name", SqliteType.Text).Value=playlist.Name;
                   command.Parameters.Add("$Description", SqliteType.Text).Value=playlist.Description;
                   command.Parameters.Add("$Link", SqliteType.Text).Value=playlist.Link;
                   command.Parameters.Add("$Image", SqliteType.Text).Value=playlist.Image;
                   command.Parameters.Add("$Length", SqliteType.Text).Value=playlist.Length;
                   command.Parameters.Add("$Owner", SqliteType.Integer).Value=playlist.Owner;
                   command.ExecuteNonQuery();
               }
               tran.Commit();
           }
           _conn.Close();
        }
        public void Cache(List<Track> tracks)
        {
           _conn.Open();
           using (SqliteTransaction tran = _conn.BeginTransaction())
           {
               string insert = "";
               foreach (Track track in tracks)
               {
                   insert = Queries.GetInsertTrack(track);
                   SqliteCommand command = new SqliteCommand(insert, _conn, tran);
                   command.Parameters.Add("$SpotifyID", SqliteType.Text).Value=track.SpotifyID;
                   command.Parameters.Add("$PlaylistID", SqliteType.Text).Value=track.PlaylistID;
                   command.Parameters.Add("$Title", SqliteType.Text).Value=track.Title;
                   command.Parameters.Add("$Artist", SqliteType.Text).Value=track.Artist;
                   command.Parameters.Add("$Album", SqliteType.Text).Value=track.Album;
                   command.Parameters.Add("$AlbumArt", SqliteType.Text).Value=track.AlbumArt;
                   command.Parameters.Add("$DateAdded", SqliteType.Text).Value=track.DateAdded;
                   command.Parameters.Add("$ContextPosition", SqliteType.Integer).Value=track.ContextPosition;
                   command.Parameters.Add("$DurationMS", SqliteType.Integer).Value=track.DurationMS;
                   command.ExecuteNonQuery();
               }

               tran.Commit();
           }
           _conn.Close();
        }

        public async Task<List<Track>> Load(int index, string playlistID, int amount)
        {
            _conn.Open();
            List<Track> tracks = (await _conn.QueryAsync<Track>(Queries.GetSelectNextTracks(index, playlistID,amount))).ToList();
            _conn.Close();
            return tracks;
        }
    }
}