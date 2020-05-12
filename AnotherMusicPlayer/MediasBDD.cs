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
        private static SQLiteConnection MediatequeBddConnection = null;
        /// <summary>
        /// Path to file sqlite
        /// On windows => %SystemDrive%\Users\%USERNAME%\AppData\Local\ + AppName
        /// </summary>
        private static string MediatequeBddFolder = null;

        /// <summary> store status if in transaction mode </summary>
        private static bool inTransaction = false;

        /// <summary> Get status if in transaction mode </summary>
        public static bool IsInTransaction() { return inTransaction; }

        /// <summary> Convert NameValueCollection to Dictionary<string, object> </summary>
        static Dictionary<string, object> MediatequeBdd_NameValueCollectionToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
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
        private static bool MediatequeBdd_IsInitilized() { return (MediatequeBddConnection == null) ? false : true; }

        /// <summary> Initialize SQLite database connection </summary>
        private static void MediatequeBddInit()
        {
            if (MediatequeBdd_IsInitilized()) { return; }
            MediatequeBddFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + SeparatorChar + AppName;
            //MediatequeBddFolder = BaseDir + SeparatorChar + appName;
            if (!System.IO.Directory.Exists(MediatequeBddFolder)) { System.IO.Directory.CreateDirectory(MediatequeBddFolder); }

            Debug.WriteLine(AppName);
            Debug.WriteLine(MediatequeBddFolder);
            MediatequeBddConnection = new SQLiteConnection(
                "Data Source="+ MediatequeBddFolder + SeparatorChar + "base.db; Version = 3; New = True; Compress = True; "
                );
            try { 
                MediatequeBddConnection.Open();
                MediatequeBddTansactionStart();
                Dictionary<string, Dictionary<string, object>> ret = MediatequeBddQuery("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'files'");
                if (ret == null) { Debug.WriteLine("ERROR"); }
                else {
                    if (ret.Count == 0) { 
                        MediatequeBddQuery("CREATE TABLE files(Path TEXT, Name TEXT, Artists TEXT, Album TEXT, Duration INTEGER, Size INTEGER, LastUpdate BIGINT)");
                    }
                    else
                    {
                        Debug.WriteLine( JsonConvert.SerializeObject(ret) );
                    }
                }
                Dictionary<string, Dictionary<string, object>> ret2 = MediatequeBddQuery("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'queue'");
                if (ret2 == null) { Debug.WriteLine("ERROR"); }
                else {
                    if (ret2.Count == 0) { 
                        MediatequeBddQuery("CREATE TABLE queue(MIndex TEXT, Path1 TEXT, Path2 TEXT)");
                    }
                    else
                    {
                        Debug.WriteLine( JsonConvert.SerializeObject(ret2) );
                    }
                }
                //SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{table_name}';
            }
            catch  { Debug.WriteLine("Catch ERROR"); }
        }

        /// <summary> execute SQL query </summary>
        private static Dictionary<string, Dictionary<string, object>> MediatequeBddQuery(string query, string index = null, bool AutoCommit = false)
        {
            Dictionary<string, Dictionary<string, object>> ret = null;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = MediatequeBddConnection.CreateCommand();
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
                    ret.Add( id, MediatequeBdd_NameValueCollectionToDictionary(row, false) );
                    //Debug.WriteLine(line.ToString());
                    line += 1;
                }
            }
            else {
                //if (AutoCommit) { MediatequeBddTansactionStart(); }

                sqlite_cmd.ExecuteNonQuery();

                //if (AutoCommit) { MediatequeBddTansactionEnd(); }
            }
            return ret;
        }

        /// <summary> Used for excape string when building SQL string for preventing sql error </summary>
        private static string MediatequeBddEscapeString(string str)
        {
            if (str == null) { return ""; }
            else { return str.Replace("'", "''"); }
        }

        /// <summary> Initialize transaction </summary>
        private static void MediatequeBddTansactionStart()
        {
            if (inTransaction) { return; }
            try
            {
                SQLiteCommand sqlite_cmdTR = MediatequeBddConnection.CreateCommand();
                sqlite_cmdTR.CommandText = "BEGIN TRANSACTION";
                sqlite_cmdTR.ExecuteNonQuery();
                inTransaction = true;
            }
            catch { }
        }

        /// <summary> Commit transaction </summary>
        private static void MediatequeBddTansactionEnd()
        {
            if (!inTransaction) { return; }
            try
            {
                SQLiteCommand sqlite_cmdTR = MediatequeBddConnection.CreateCommand();
                sqlite_cmdTR.CommandText = "COMMIT";
                sqlite_cmdTR.ExecuteNonQuery();
                inTransaction = false;
            }
            catch { }
        }


    }
}