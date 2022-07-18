using Microsoft.Data.Sqlite;

namespace LocalDatabase
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
                string createTables = @" CREATE TABLE IF NOT EXISTS Playlist (
                    SpotifyID	varchar(100),
                    Name	varchar(100),
                    Description	varchar(100),
                    Artist	varchar(100),
                    Link	varchar(256),
                    Image	varchar(256),
                    ID INTEGER,
                    PRIMARY KEY(ID));
                    
                    CREATE TABLE IF NOT EXISTS Track (
                   SpotifyID	varchar(100),
                   Title	varchar(100),
                   Artist	varchar(100),
                   Album	varchar(100),
                   AlbumArt	varchar(256),
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
    }
}