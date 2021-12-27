using System;
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

        /// <summary> store status if in transaction mode </summary>
        private static bool inTransaction = false;

        /// <summary> Get status if in transaction mode </summary>
        public static bool IsInTransaction() { return inTransaction; }

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
                //DatabaseTansactionStart();

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
                    + "LastUpdate BIGINT"
                    + ")");
                DatabaseDetectOrCreateTable("queue", "CREATE TABLE queue(MIndex TEXT, Path1 TEXT, Path2 TEXT)");
                DatabaseDetectOrCreateTable("playlists", "CREATE TABLE playlists(FIndex TEXT, Name TEXT, Description TEXT)");
                DatabaseDetectOrCreateTable("playlistsItems", "CREATE TABLE playlistsItems(PIndex TEXT, LIndex Text, Name TEXT, Path TEXT)");
            }
            catch  { Debug.WriteLine("Catch ERROR"); }
        }

        private static bool DatabaseDetectOrCreateTable(string TableName, string QueryCreation) {
            Dictionary<string, Dictionary<string, object>> ret2 = DatabaseQuery("SELECT name,sql FROM sqlite_master WHERE type = 'table' AND name = '" + TableName + "'");
            if (ret2 == null) { Debug.WriteLine("ERROR"); }
            else {
                if (ret2.Count == 0) { DatabaseQuery(QueryCreation); }
                else {
                    if ((string)ret2["0"]["sql"] != QueryCreation) {
                        DatabaseQuery("DROP TABLE "+TableName);
                        DatabaseQuery(QueryCreation);
                    }
                    Debug.WriteLine(JsonConvert.SerializeObject(ret2));
                }
            }
            return true;
        }

        /// <summary> execute SQL query </summary>
        private static Dictionary<string, Dictionary<string, object>> DatabaseQuery(string query, string index = null, bool AutoCommit = false)
        {
            Dictionary<string, Dictionary<string, object>> ret = null;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = DatabaseConnection.CreateCommand();
            sqlite_cmd.CommandText = query;
            string tq = query.ToUpper().Trim();

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
                        else { id = "" + line; }
                    }
                    else { id = "" + line; }
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
        private static void DatabaseQuerys(string[] querys)
        {
            Dictionary<string, Dictionary<string, object>> ret = null;
            SQLiteDataReader sqlite_datareader;
            DatabaseTansactionStart();
            foreach (string query in querys)
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = DatabaseConnection.CreateCommand();
                sqlite_cmd.CommandText = query;
                string tq = query.ToUpper().Trim();
                if (tq.StartsWith("SELECT ")) { }
                else { try { sqlite_cmd.ExecuteNonQuery(); } catch { } }
            }
            DatabaseTansactionEnd();
        }

        /// <summary> Used for excape string when building SQL string for preventing sql error </summary>
        private static string DatabaseEscapeString(string str)
        {
            if (str == null) { return ""; }
            else { return str.Replace("'", "''"); }
        }

        /// <summary> Initialize transaction </summary>
        private static void DatabaseTansactionStart()
        {
            if (inTransaction) { return; }
            try
            {
                SQLiteCommand sqlite_cmdTR = DatabaseConnection.CreateCommand();
                sqlite_cmdTR.CommandText = "BEGIN TRANSACTION";
                sqlite_cmdTR.ExecuteNonQuery();
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
                SQLiteCommand sqlite_cmdTR = DatabaseConnection.CreateCommand();
                sqlite_cmdTR.CommandText = "COMMIT";
                sqlite_cmdTR.ExecuteNonQuery();
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
                SQLiteCommand sqlite_cmdTR = DatabaseConnection.CreateCommand();
                sqlite_cmdTR.CommandText = "COMMIT";
                sqlite_cmdTR.ExecuteNonQuery();
                sqlite_cmdTR.CommandText = "BEGIN TRANSACTION";
                sqlite_cmdTR.ExecuteNonQuery();
                inTransaction = false;
            }
            catch { }
        }


    }
}