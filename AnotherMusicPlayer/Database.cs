using System;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Data.SQLite;

namespace AnotherMusicPlayer
{

    public partial class MainWindow : Window
    {
        /// <summary> Object connection to sqlite file </summary>
        private static SQLiteConnection DatabaseConnection = null;
        /// <summary>
        /// Path to file sqlite
        /// On windows => %SystemDrive%\Users\%USERNAME%\AppData\Local\ + AppName
        /// </summary>
        private static string DatabaseFolder = null;

        /// <summary> Used for excape string when building SQL string for preventing sql error </summary>
        private static string DatabaseEscapeString(string str)
        {
            if (str == null) { return ""; }
            else { return str.Replace("'", "''"); }
        }

        /// <summary> store status if in transaction mode </summary>
        private static bool inTransaction = false;
        private static SQLiteTransaction Transaction = null;

        /// <summary> Get status if in transaction mode </summary>
        public static bool IsInTransaction() { return inTransaction; }

        /// <summary> Initialize transaction </summary>
        private static void DatabaseTansactionStart()
        {
            if (inTransaction) { return; }
            try
            {
                Transaction = DatabaseConnection.BeginTransaction();
                inTransaction = true;
            }
            catch { }
        }

        /// <summary> Commit transaction </summary>
        private static void DatabaseTansactionEnd()
        {
            if (!inTransaction) { return; }
            try
            {
                Transaction.Commit();
                Transaction.Dispose();
                inTransaction = false;
            }
            catch { }
        }

        /// <summary> Commit transaction </summary>
        private static void DatabaseTansactionEndAndStart()
        {
            if (!inTransaction) { return; }
            try
            {
                Transaction.Commit();
                Transaction.Dispose();
                inTransaction = false;

                Transaction = DatabaseConnection.BeginTransaction();
                inTransaction = true;
            }
            catch { }
        }

        /// <summary> Convert NameValueCollection to Dictionary<string, object> </summary>
        static Dictionary<string, object> Database_NameValueCollectionToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values.Length == 1) { result.Add(key, values[0]); } else { result.Add(key, values); }
                }
                else { result.Add(key, nvc[key]); }
            }
            return result;
        }

        /// <summary> Test if connection to SQLite database initialized </summary>
        private static bool Database_IsInitilized() { return (DatabaseConnection == null) ? false : true; }

        /// <summary> Initialize SQLite database connection </summary>
        private static void DatabaseInit()
        {
            if (Database_IsInitilized()) { return; }
            DatabaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName;
            //DatabaseFolder = BaseDir + SeparatorChar + appName;
            if (!System.IO.Directory.Exists(DatabaseFolder)) { System.IO.Directory.CreateDirectory(DatabaseFolder); }

            Debug.WriteLine(AppName);
            Debug.WriteLine(DatabaseFolder);
            DatabaseConnection = new SQLiteConnection(
                "Data Source="+ DatabaseFolder + SeparatorChar + "base.db; Version = 3; New = True; Compress = True; "
                );
            try { 
                DatabaseConnection.Open();
                DatabaseTansactionStart();

                DatabaseDetectOrCreateTable("covers", "CREATE TABLE covers("
                    + "Path TEXT, "
                    + "Hash TEXT, "
                    + "Data BLOB, "
                    + "PRIMARY KEY(\"Path\") "
                    + ")");

                DatabaseDetectOrCreateTable("folders", "CREATE TABLE folders("
                    + "Id TEXT, "
                    + "ParentId TEXT, "
                    + "Name TEXT, "
                    + "Path TEXT"
                    + ")");

                DatabaseDetectOrCreateTable("files", "CREATE TABLE files("
                    + "Folder TEXT, "
                    + "Path TEXT, "
                    + "Name TEXT, "
                    + "Performers TEXT, "
                    + "Composers TEXT, "
                    + "Album TEXT, "
                    + "Genres TEXT, "
                    + "Copyright TEXT, "
                    + "Disc INTERGER, "
                    + "DiscCount INTERGER, "
                    + "AlbumArtists TEXT, "
                    + "Lyrics TEXT, "
                    + "Track INTEGER, "
                    + "TrackCount INTEGER, "
                    + "Year INTEGER, "
                    + "Duration INTEGER, "
                    + "Size INTEGER, "
                    + "LastUpdate BIGINT,  PRIMARY KEY(\"Path\")"
                    + ")");
                DatabaseDetectOrCreateTable("queue", "CREATE TABLE queue(MIndex TEXT, Path1 TEXT, Path2 TEXT)");
                DatabaseDetectOrCreateTable("playlists", "CREATE TABLE playlists(FIndex TEXT, Name TEXT, Description TEXT)");
                DatabaseDetectOrCreateTable("playlistsItems", "CREATE TABLE playlistsItems(PIndex TEXT, LIndex Text, Name TEXT, Path TEXT)");
            }
            catch  { Debug.WriteLine("Catch ERROR"); }
        }

        private static bool DatabaseDetectOrCreateTable(string TableName, string QueryCreation) {
            bool ret = true;
            try
            {
                Dictionary<string, Dictionary<string, object>> ret2 = DatabaseQuery("SELECT name,sql FROM sqlite_master WHERE type = 'table' AND name = '" + TableName + "'");
                if (ret2 == null) { Debug.WriteLine("ERROR"); }
                else
                {
                    if (ret2.Count == 0) { DatabaseQuery(QueryCreation); }
                    else
                    {
                        if ((string)ret2["0"]["sql"] != QueryCreation)
                        {
                            DatabaseQuery("DROP TABLE " + TableName);
                            DatabaseQuery(QueryCreation);
                        }
                        Debug.WriteLine(JsonConvert.SerializeObject(ret2));
                    }
                }
            }
            catch { ret = false; }
            return ret;
        }

        /// <summary> execute SQL query </summary>
        private static Dictionary<string, Dictionary<string, object>> DatabaseQuery(string query, string index = null, bool AutoCommit = false)
        {
            Dictionary<string, Dictionary<string, object>> ret = null;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = DatabaseConnection.CreateCommand();
            sqlite_cmd.CommandText = query;
            string tq = query.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Trim();

            if (tq.StartsWith("SELECT "))
            {
                ret = new Dictionary<string, Dictionary<string, object>>();
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                int line = 0;
                string id = "";
                while (sqlite_datareader.Read())
                {
                    NameValueCollection row = sqlite_datareader.GetValues();

                    row.AllKeys.Contains(index);
                    if (index != null) {
                        if (row.AllKeys.Contains(index))
                        {
                            id = row[index];
                        }
                    }
                    if (id == "") { id = "" + line; }
                    if (ret.ContainsKey(id)) { line += 1; continue; }
                    try { ret.Add(id, Database_NameValueCollectionToDictionary(row, false)); } catch { }
                    //Debug.WriteLine(line.ToString());
                    line += 1;
                }
            }
            else {
                DatabaseTansactionStart();
                try { sqlite_cmd.ExecuteNonQuery(); } catch { }
                DatabaseTansactionEnd();
            }
            return ret;
        }

        /// <summary> execute SQL query </summary>
        private static void DatabaseQuerys(string[] querys, bool autocommit = true)
        {
            if(autocommit)DatabaseTansactionStart();
            foreach (string query in querys)
            {
                if(query == null) { continue; }
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = DatabaseConnection.CreateCommand();
                sqlite_cmd.CommandText = query;
                string tq = query.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Trim();
                if (tq.StartsWith("SELECT ")) { }
                else { try { sqlite_cmd.ExecuteNonQuery(); } catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); } }
            }
            if (autocommit) DatabaseTansactionEnd();
        }


        /// <summary> Get cover for specific hash </summary>
        public static string DatabaseGetCover(string Path, string Hash = null)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = DatabaseConnection.CreateCommand();
            string query = "";
            query = "SELECT Data, Hash FROM covers WHERE Path = '" + Path.Replace("\\", "/").Replace("'", "<") + "'";
            sqlite_cmd.CommandText = query;
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            Debug.WriteLine(sqlite_datareader.StepCount);
            if (sqlite_datareader.HasRows) {
                sqlite_datareader.Read();
                NameValueCollection row = sqlite_datareader.GetValues();
                return row.Get(0);
            }
            return null;
        }

        /// <summary> Save cover for specific Hash </summary>
        public static void DatabaseSaveCover(string Path, string Hash, String CoverData)
        {
            try
            {
                Hash = Hash.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Trim();
                CoverData = CoverData.Trim();
                DatabaseTansactionStart();
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = DatabaseConnection.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO covers(Path, Hash, Data) VALUES('" + Path.Replace("\\", "/").Replace("'", "<") + "', '" + Hash + "', '" + CoverData + "')";
                sqlite_cmd.ExecuteNonQuery();
                DatabaseTansactionEnd();
            }
            catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
        }

        /// <summary> Get Basic Metadata of a media file if stored in database </summary>
        private Dictionary<string, object> DatabaseFileInfo(string path)
        {
            try
            {
                if (!MediatequeScanning)
                {
                    Dictionary<string, Dictionary<string, object>> rt = DatabaseQuery("SELECT * FROM files WHERE Path='" + DatabaseEscapeString(path) + "' ORDER BY Path ASC", "Path");
                    if (rt.Count == 0) { return null; }
                    if (Int32.Parse((string)rt[path]["Size"]) <= 0)
                    {
                        FileInfo fi = new FileInfo(path);
                        PlayListViewItem item = player.MediaInfo(path, false);

                        DatabaseQuerys(new string[]{"UPDATE files SET Name='" + DatabaseEscapeString(item.Name??fi.Name)
                            + "', Album='" + DatabaseEscapeString(item.Album)
                            + "', Performers='" + DatabaseEscapeString(item.Performers)
                            + "', Composers='" + DatabaseEscapeString(item.Composers)
                            + "', Genres='" + DatabaseEscapeString(item.Genres)
                            + "', Copyright='" + DatabaseEscapeString(item.Copyright)
                            + "', AlbumArtists='" + DatabaseEscapeString(item.AlbumArtists)
                            + "', Lyrics='" + DatabaseEscapeString(item.Lyrics)
                            + "', Duration='" + item.Duration
                            + "', Size='" + item.Size
                            + "', Disc='" + item.Disc
                            + "', DiscCount='" + item.DiscCount
                            + "', Track='" + item.Track
                            + "', TrackCount='" + item.TrackCount
                            + "', Year='" + item.Year
                            + "', LastUpdate='" + fi.LastWriteTimeUtc.ToFileTime()
                            + "' WHERE Path='" + DatabaseEscapeString(path) + "'" }, true);

                        rt = DatabaseQuery("SELECT * FROM files WHERE Path='" + DatabaseEscapeString(path) + "' ORDER BY Path ASC", "Path");
                        return rt[path];
                    }
                    return rt[path];
                }
            }
            catch { }
            return null;
        }

        /// <summary> Get Basic Metadata from a list of file if stored in database </summary>
        private Dictionary<string, Dictionary<string, object>> DatabaseFilesInfo(string[] paths)
        {
            try
            {
                if (!MediatequeScanning)
                {
                    Dictionary<string, Dictionary<string, object>> ret = new Dictionary<string, Dictionary<string, object>>();
                    string query = "SELECT * FROM files WHERE Path IN(?) ORDER BY Album ASC, Disc ASC, Track ASC, Name ASC, Path ASC";
                    foreach(string file in paths) { query = query.Replace("?", "'"+ DatabaseEscapeString(file) + "',?"); }
                    query = query.Replace(",?", "");
                    Dictionary<string, Dictionary<string, object>> data = DatabaseQuery(query, "Path");
                    if (data.Count == 0) { return null; }
                    foreach(KeyValuePair<string, Dictionary<string, object>> line in data)
                    {
                        if (Int32.Parse((string)line.Value["Size"]) <= 0)
                        {
                            FileInfo fi = new FileInfo((string)line.Value["Path"]);
                            PlayListViewItem item = player.MediaInfo((string)line.Value["Path"], false);

                            DatabaseQuerys(new string[]{"UPDATE files SET Name='" + DatabaseEscapeString(item.Name??fi.Name)
                            + "', Album='" + DatabaseEscapeString(item.Album)
                            + "', Performers='" + DatabaseEscapeString(item.Performers)
                            + "', Composers='" + DatabaseEscapeString(item.Composers)
                            + "', Genres='" + DatabaseEscapeString(item.Genres)
                            + "', Copyright='" + DatabaseEscapeString(item.Copyright)
                            + "', AlbumArtists='" + DatabaseEscapeString(item.AlbumArtists)
                            + "', Lyrics='" + DatabaseEscapeString(item.Lyrics)
                            + "', Duration='" + item.Duration
                            + "', Size='" + item.Size
                            + "', Disc='" + item.Disc
                            + "', DiscCount='" + item.DiscCount
                            + "', Track='" + item.Track
                            + "', TrackCount='" + item.TrackCount
                            + "', Year='" + item.Year
                            + "', LastUpdate='" + fi.LastWriteTimeUtc.ToFileTime()
                            + "' WHERE Path='" + DatabaseEscapeString((string)line.Value["Path"]) + "'" }, true);

                            Dictionary<string, Dictionary<string, object>> dataT = DatabaseQuery("SELECT * FROM files WHERE Path='" + DatabaseEscapeString((string)line.Value["Path"]) + "' ORDER BY Path ASC", "Path");
                            ret.Add((string)line.Value["Path"], dataT[(string)line.Value["Path"]]);
                            dataT.Clear();
                        }
                        else
                        {
                            ret.Add((string)line.Value["Path"], data[(string)line.Value["Path"]]);
                        }
                    }
                    data.Clear();
                    return ret;
                }
            }
            catch { }
            return null;
        }

    }
}