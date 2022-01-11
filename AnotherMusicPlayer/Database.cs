using System;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{

    public class Database
    {
        /// <summary> Store the application assembly name </summary>
        string AppName = System.Windows.Application.ResourceAssembly.GetName().Name;
        /// <summary> Short notation for ystem.IO.Path.DirectorySeparatorChar </summary>
        public char SeparatorChar = System.IO.Path.DirectorySeparatorChar;
        /// <summary>
        /// Path to sqlite file
        /// On windows => %SystemDrive%\Users\%USERNAME%\AppData\Local\ + AppName
        /// </summary>
        private string DatabaseFolder = null;
        /// <summary> Object connection to sqlite file </summary>
        private SQLiteConnection DatabaseConnection = null;

        /// <summary> Used for excape string when building SQL string for preventing sql error </summary>
        public static string EscapeString(string str)
        {
            if (str == null) { return ""; }
            else { return str.Replace("'", "''"); }
        }

        /// <summary> store status if in transaction mode </summary>
        private bool inTransaction = false;
        private SQLiteTransaction Transaction = null;

        /// <summary> Get status if in transaction mode </summary>
        public bool IsInTransaction() { return inTransaction; }

        /// <summary> Initialize transaction </summary>
        private void DatabaseTansactionStart()
        {
            if (inTransaction) { return; }
            try
            {
                Transaction = DatabaseConnection.BeginTransaction();
                inTransaction = true;
            }
            catch { Debug.WriteLine("--> DatabaseTansactionStart() : Catch ERROR <--"); }
        }

        /// <summary> Commit transaction </summary>
        public void DatabaseTansactionEnd()
        {
            if (!inTransaction) { return; }
            try
            {
                Transaction.Commit();
                Transaction.Dispose();
                inTransaction = false;
            }
            catch { Debug.WriteLine("--> DatabaseTansactionEnd() : Catch ERROR <--"); }
        }

        /// <summary> Commit transaction </summary>
        public void DatabaseTansactionEndAndStart()
        {
            Debug.WriteLine("--> DatabaseTansactionEndAndStart() : P1 <--");
            if (!inTransaction) { return; }
            try
            {
                Debug.WriteLine("--> DatabaseTansactionEndAndStart() : P2 <--");
                Transaction.Commit();
                Debug.WriteLine("--> DatabaseTansactionEndAndStart() : P3 <--");
                Transaction.Dispose();
                Debug.WriteLine("--> DatabaseTansactionEndAndStart() : P4 <--");
                inTransaction = false;

                Transaction = DatabaseConnection.BeginTransaction();
                Debug.WriteLine("--> DatabaseTansactionEndAndStart() : P5 <--");
                inTransaction = true;
            }
            catch { Debug.WriteLine("--> DatabaseTansactionEndAndStart() : Catch ERROR <--"); }
        }

        /// <summary> Convert NameValueCollection to Dictionary<string, object> </summary>
        Dictionary<string, object> Database_NameValueCollectionToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
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

        /// <summary> Close SQLite database connection </summary>
        internal void Finalize()
        {
            DatabaseTansactionEnd();
            DatabaseConnection.Close();
        }

        /// <summary> Initialize SQLite database connection </summary>
        public Database()
        {
            try
            {
                DatabaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName;
                if (!System.IO.Directory.Exists(DatabaseFolder)) { System.IO.Directory.CreateDirectory(DatabaseFolder); }
                DatabaseConnection = new SQLiteConnection("Data Source=" + DatabaseFolder + SeparatorChar + "base.db; Version = 3; New = True; Compress = True; ");
                DatabaseConnection.Open();
                DatabaseTansactionStart();

                DatabaseDetectOrCreateTable("params", "CREATE TABLE params("
                    + "ParamName TEXT, "
                    + "ParamType TEXT, "
                    + "ParamValue TEXT, "
                    + "PRIMARY KEY(\"ParamName\") "
                    + ")");

                DatabaseDetectOrCreateTable("covers", "CREATE TABLE covers("
                    + "Path TEXT, "
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
                DatabaseTansactionEnd();
            }
            catch  { Debug.WriteLine("--> Database() : Catch ERROR <--"); }
        }

        private bool DatabaseDetectOrCreateTable(string TableName, string QueryCreation) {
            bool ret = true;
            try
            {
                Dictionary<string, Dictionary<string, object>> ret2 = DatabaseQuery("SELECT name,sql FROM sqlite_master WHERE type = 'table' AND name = '" + TableName + "'");
                if (ret2 == null) { Debug.WriteLine("--> DatabaseDetectOrCreateTable : ERROR"); }
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
                        //Debug.WriteLine(JsonConvert.SerializeObject(ret2));
                    }
                }
            }
            catch { ret = false; }
            return ret;
        }

        /// <summary> execute SQL query </summary>
        public Dictionary<string, Dictionary<string, object>> DatabaseQuery(string query, string index = null, bool AutoCommit = false)
        {
            Dictionary<string, Dictionary<string, object>> ret = null;
            try
            {
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

                    bool previousTransaction = false;
                    if (AutoCommit)
                    {
                        if (!IsInTransaction()) { DatabaseTansactionStart(); }
                        else { previousTransaction = true; }
                    }
                    try { sqlite_cmd.ExecuteNonQuery(); } catch { }
                    if (AutoCommit)
                    {
                        DatabaseTansactionEnd();
                    }
                }
            }
            catch (Exception err){ 
                Debug.WriteLine("DatabaseQuery => " + query); 
                Debug.WriteLine("DatabaseQuery => " + JsonConvert.SerializeObject(err)); 
            }
            return ret;
        }

        /// <summary> execute SQL query </summary>
        public void DatabaseQuerys(string[] querys, bool autocommit = true)
        {
            bool previousTransaction = false;
            if (autocommit) {
                if (!IsInTransaction()) { DatabaseTansactionStart(); }
            }
            foreach (string query in querys)
            {
                if(query == null) { continue; }
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = DatabaseConnection.CreateCommand();
                sqlite_cmd.CommandText = query;
                string tq = query.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Trim();
                if (tq.StartsWith("SELECT ")) { }
                else { try { sqlite_cmd.ExecuteNonQuery(); } catch (Exception err) { Debug.WriteLine("--> DatabaseQuerys error => " + JsonConvert.SerializeObject(err)); } }
            }
            if (autocommit) {
                DatabaseTansactionEnd();
            }
        }


        /// <summary> Get cover for specific path </summary>
        public string DatabaseGetCover(string Path)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = DatabaseConnection.CreateCommand();
            string query = "";
            query = "SELECT Data FROM covers WHERE Path = '" + Path.Replace("\\", "/").Replace("'", "<") + "'";
            sqlite_cmd.CommandText = query;
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            //Debug.WriteLine(sqlite_datareader.StepCount);
            if (sqlite_datareader.HasRows) {
                sqlite_datareader.Read();
                NameValueCollection row = sqlite_datareader.GetValues();
                return row.Get(0);
            }
            return null;
        }

        /// <summary> Save cover for specific path </summary>
        public void DatabaseSaveCover(string Path, String CoverData)
        {
            try
            {
                CoverData = CoverData.Trim();
                if (!IsInTransaction()) { DatabaseTansactionStart(); }
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = DatabaseConnection.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO covers(Path, Data) VALUES('" + Path.Replace("\\", "/").Replace("'", "<") + "', '" + CoverData + "')";
                sqlite_cmd.ExecuteNonQuery();
                DatabaseTansactionEnd();
            }
            catch (Exception err) { Debug.WriteLine("--> DatabaseSaveCover ERROR : " + JsonConvert.SerializeObject(err)); }
        }


        /// <summary> Get value for specific param </summary>
        public string DatabaseGetParam(string Param, string Default)
        {
            SQLiteCommand sqlite_cmd = DatabaseConnection.CreateCommand();
            sqlite_cmd.CommandText = "SELECT ParamValue,ParamType FROM params WHERE ParamName = '" + EscapeString(Param) + "'";
            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
            if (sqlite_datareader.HasRows) {
                sqlite_datareader.Read();
                NameValueCollection row = sqlite_datareader.GetValues();
                return (row.Get(0) == null)?Default:((row.Get(0).Trim() == "")?Default: row.Get(0));
            }
            return Default;
        }

        /// <summary> Save value for specific param </summary>
        public void DatabaseSaveParam(string Param, string Value, string ParamType = "TEXT", bool autoCommit = false)
        {
            try
            {
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = DatabaseConnection.CreateCommand();
                sqlite_cmd.CommandText = "SELECT ParamValue,ParamType FROM params WHERE ParamName = '" + EscapeString(Param) + "'";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                if (!IsInTransaction() && autoCommit) { DatabaseTansactionStart(); }
                SQLiteCommand sqlite_cmd2;
                sqlite_cmd2 = DatabaseConnection.CreateCommand();
                if (sqlite_datareader.HasRows)
                {
                    sqlite_cmd2.CommandText = "UPDATE params SET ParamValue = '" + EscapeString(Value) + "' WHERE ParamName = '" + EscapeString(Param) + "'";
                }
                else
                {
                    sqlite_cmd2.CommandText = "INSERT INTO params(ParamName, ParamType, ParamValue) VALUES('" + EscapeString(Param) + "', '" + EscapeString(ParamType) + "', '" + EscapeString(Value) + "')";
                }
                sqlite_cmd2.ExecuteNonQuery();
                if(autoCommit)DatabaseTansactionEnd();
            }
            catch (Exception err) { Debug.WriteLine("--> DatabaseSaveParam ERROR : " + JsonConvert.SerializeObject(err)); }
        }

        /// <summary> Get Basic Metadata of a media file if stored in database </summary>
        public Dictionary<string, object> DatabaseFileInfo(string path, bool forceUpdate = false)
        {
            try
            {
                Dictionary<string, Dictionary<string, object>> rt = DatabaseQuery("SELECT * FROM files WHERE Path='" + EscapeString(path) + "' ORDER BY Path ASC", "Path");
                if (rt.Count == 0) { return null; }
                if (Int32.Parse((string)rt[path]["Size"]) <= 0 && forceUpdate == true)
                {
                    return UpdateFileAsync(path, true);
                }
                return rt[path];
            }
            catch { }
            return null;
        }

        /// <summary> Get Basic Metadata from a list of file if stored in database </summary>
        public Dictionary<string, Dictionary<string, object>> DatabaseFilesInfo(string[] paths, MainWindow parent = null, bool forceUpdate = false)
        {
            Dictionary<string, Dictionary<string, object>> ret = new Dictionary<string, Dictionary<string, object>>();
            List<string> filesToUpdate = new List<string>();
            try
            {
                string query = "SELECT * FROM files WHERE Path IN(?) ORDER BY Album ASC, Disc ASC, Track ASC, Name ASC, Path ASC";
                foreach(string file in paths) { query = query.Replace("?", "'"+ EscapeString(file) + "',?"); }
                query = query.Replace(",?", "");

                Dictionary<string, Dictionary<string, object>> data = DatabaseQuery(query, "Path");
                if (data.Count == 0) { return null; }
                foreach(KeyValuePair<string, Dictionary<string, object>> line in data)
                {
                    if (Int32.Parse((string)line.Value["Size"]) <= 0 || forceUpdate)
                    {
                        filesToUpdate.Add((string)line.Value["Path"]);
                    }
                    else
                    {
                        ret.Add((string)line.Value["Path"], line.Value);
                    }
                }
            }
            catch (Exception err) { Debug.WriteLine("--> DatabaseFilesInfo ERROR1 <--");  Debug.WriteLine(JsonConvert.SerializeObject(err)); }
            //try { DatabaseTansactionEndAndStart(); } catch { }

            ConcurrentQueue<string> cq = new ConcurrentQueue<string>();
            foreach (string file in filesToUpdate) {
                cq.Enqueue(file);
            }

            if (!IsInTransaction()) { DatabaseTansactionStart(); }
            Action action = () =>
            {
                int cpt = 0;
                string localValue;

                while (cq.TryDequeue(out localValue)) 
                {
                    Dictionary<string, object> rett = UpdateFileAsync(localValue);
                    if (rett != null) { ret.Add(localValue, rett); }
                    cpt += 1;
                    if (parent != null) { parent.setMetadataScanningState(true, cq.Count); }
                }
            };
            // Start 5 concurrent consuming actions.
            Parallel.Invoke(action, action, action, action, action, action);

            if (IsInTransaction()) { DatabaseTansactionEnd(); }
            return ret;
        }

        public Dictionary<string, object> UpdateFileAsync(string file, bool commit=false)
        {
            if (file == null) { return null; }
            if(!System.IO.File.Exists(file)) { return null; }

            FileInfo fi = new FileInfo(file);
            MediaItem item = FilesTags.MediaInfo(file, false);
            string query = "UPDATE files SET Name='" + EscapeString(item.Name ?? fi.Name);
            if (item.Album != null && item.Album.Trim() != "") query += "', Album='" + EscapeString(item.Album);
            if (item.Performers != null && item.Performers.Trim() != "") query += "', Performers='" + EscapeString(item.Performers);
            if (item.Composers != null && item.Composers.Trim() != "") query += "', Composers='" + EscapeString(item.Composers);
            if (item.Genres != null && item.Genres.Trim() != "") query += "', Genres='" + EscapeString(item.Genres);
            if (item.Copyright != null && item.Copyright.Trim() != "") query += "', Copyright='" + EscapeString(item.Copyright);
            if (item.AlbumArtists != null && item.AlbumArtists.Trim() != "") query += "', AlbumArtists='" + EscapeString(item.AlbumArtists);
            if (item.Lyrics != null && item.Lyrics.Trim() != "") query += "', Lyrics='" + EscapeString(item.Lyrics);
            query += "', Duration='" + item.Duration
                + "', Size='" + item.Size
                + "', Disc='" + item.Disc
                + "', DiscCount='" + item.DiscCount
                + "', Track='" + item.Track
                + "', TrackCount='" + item.TrackCount
                + "', Year='" + item.Year
                + "', LastUpdate='" + fi.LastWriteTimeUtc.ToFileTime()
                + "' WHERE Path='" + EscapeString(file) + "'";

            DatabaseQuerys(new string[]{ query }, commit);
            return (item != null) ? MainWindow.MediaItemToDatabaseItem(item) : null;
        }

    }
}