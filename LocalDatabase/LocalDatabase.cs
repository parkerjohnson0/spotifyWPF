
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
                string sql = "CREATE TABLE IF NOT EXISTS Track (ID varchar(100), Title varchar(100), Artist varchar(100)," +
                "Album varchar(100), AlbumArt varchar(256), DurationMS bigint)";

                SqliteCommand sqliteCommand = new SqliteCommand(sql, _conn, tran);
                sqliteCommand.ExecuteNonQuery();
                tran.Commit();
            }
            _conn.Close();


        }
    }
}