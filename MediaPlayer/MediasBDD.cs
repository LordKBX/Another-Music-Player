using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Documents;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Data;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Data.SQLite;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace AnotherMusicPlayer
{

    public partial class MainWindow : Window
    {
        private static SQLiteConnection MediatequeBddConnection = null;
        private static string MediatequeBddFolder = null;

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

        private static bool MediatequeBdd_IsInitilized() { return (MediatequeBddConnection == null) ? false : true; }
        private static void MediatequeBddInit()
        {
            if (MediatequeBdd_IsInitilized()) { return; }
            string appName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
            MediatequeBddFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                 + System.IO.Path.DirectorySeparatorChar + appName;
            if (!System.IO.Directory.Exists(MediatequeBddFolder)) { System.IO.Directory.CreateDirectory(MediatequeBddFolder); }

            Debug.WriteLine(appName);
            Debug.WriteLine(MediatequeBddFolder);
            MediatequeBddConnection = new SQLiteConnection(
                "Data Source="+ MediatequeBddFolder + System.IO.Path.DirectorySeparatorChar + "base.db; Version = 3; New = True; Compress = True; "
                );
            try { 
                MediatequeBddConnection.Open();
                List<Dictionary<string, object>> ret = MediatequeBddQuery("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'files'");
                if (ret == null) { Debug.WriteLine("ERROR"); }
                else {
                    if (ret.Count == 0) { 
                        Debug.WriteLine("Not Found");
                        MediatequeBddQuery("CREATE TABLE files(Path TEXT, Name TEXT, Artists TEXT, Album TEXT, Duration INTEGER, LastUpdate TEXT)");
                    }
                    else
                    {
                        Debug.WriteLine( JsonConvert.SerializeObject(ret) );
                    }
                }
                //SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{table_name}';
            }
            catch (Exception ex) { Debug.WriteLine("Catch ERROR"); }
        }

        private static List<Dictionary<string, object>> MediatequeBddQuery(string query)
        {
            List<Dictionary<string, object>> ret = null;
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = MediatequeBddConnection.CreateCommand();
            sqlite_cmd.CommandText = query;

            if (query.ToUpper().Trim().StartsWith("SELECT "))
            {
                ret = new List<Dictionary<string, object>>();
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    NameValueCollection line = sqlite_datareader.GetValues();
                    ret.Add( MediatequeBdd_NameValueCollectionToDictionary(line, false) );
                    //Debug.WriteLine(line.ToString());
                }
            }
            else { sqlite_cmd.ExecuteNonQuery(); }
            //MediatequeBddConnection.Close();
            return ret;
        }


    }
}