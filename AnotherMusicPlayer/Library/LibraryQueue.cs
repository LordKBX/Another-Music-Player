using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Linq;

namespace AnotherMusicPlayer
{
    public partial class MainWindow : Window
    {
        /// <summary> Asynchronus call for loading old playlist in a new thread </summary>
        private void LibraryLoadOldPlaylist()
        {
            try
            {
                Thread objThread = new Thread(new ParameterizedThreadStart(LibraryLoadOldPlaylistP2));
                objThread.IsBackground = true;
                objThread.Priority = ThreadPriority.AboveNormal;
                objThread.Start(null);
            }
            catch { }
        }

        private void LibraryLoadOldPlaylistP2(object param = null)
        {
            Dictionary<string, Dictionary<string, object>> LastPlaylist = bdd.DatabaseQuery("SELECT MIndex,Path1,Path2 FROM queue ORDER BY MIndex ASC", "MIndex");
            if (LastPlaylist != null)
            {
                if (LastPlaylist.Count > 0)
                {
                    Debug.WriteLine("Old PlayList detected");
                    Debug.WriteLine(JsonConvert.SerializeObject(LastPlaylist));
                    List<string> gl = new List<string>();
                    int fails = 0;
                    bool radio = false;
                    foreach (KeyValuePair<string, Dictionary<string, object>> fi in LastPlaylist)
                    {
                        string path1 = (string)fi.Value["Path1"];
                        string path2 = (fi.Value["Path2"] == null) ? null : ((string)fi.Value["Path2"]).Trim();
                        if (path2 != null && path2 != "")
                        {
                            if (System.IO.File.Exists(path2)) { gl.Add(path2); }
                            else
                            {
                                if (System.IO.File.Exists(path1)) { gl.Add(path1); } else { fails += 1; }
                            }
                        }
                        else
                        {
                            if (path1.StartsWith("Radio|"))
                            {
                                Debug.WriteLine(" = = = > RADIO 0000");
                                gl.Clear();
                                gl.Add(path1);
                                radio = true;
                                break;
                            }
                            else
                            {
                                if (System.IO.File.Exists(path1)) { gl.Add(path1); }
                                else { fails += 1; }
                            }
                        }
                    }
                    int newIndex = -1;
                    if (fails > 0) { newIndex = 0; }
                    else { newIndex = Settings.LastPlaylistIndex; }

                    if (radio == true)
                    {
                        Debug.WriteLine(" = = = > RADIO");
                        Debug.WriteLine(gl[0]);
                        string[] rtab = gl[0].Split('|');
                        if (rtab[1].Trim() != "")
                        {
                            Dictionary<string, object> CurentRadio = bdd.DatabaseQuery("SELECT * FROM radios WHERE RID = " + rtab[1], "RID")[rtab[1]];
                            Debug.WriteLine(JsonConvert.SerializeObject(CurentRadio));
                            Player.OpenStream(CurentRadio["Url"] as string, (CurentRadio["FType"] as string == "M3u") ? RadioPlayer.RadioType.M3u : RadioPlayer.RadioType.Stream, CurentRadio["RID"] as string, CurentRadio["Name"] as string, Settings.StartUpPlay, CurentRadio["UrlPrefix"] as string);
                        }
                        else { Player.OpenStream(gl[0], RadioPlayer.RadioType.Stream, "", "", Settings.StartUpPlay, ""); }
                    }
                    else { Open(gl.ToArray(), false, false, newIndex, Settings.StartUpPlay); }
                    //player.Stop();
                }
            }
        }

        /// <summary> Record Playing queue in database </summary>
        public void UpdateRecordedQueue()
        {
            int index = 1;
            List<string> querys = new List<string>() { "DELETE FROM queue;" };
            foreach (string[] line in PlayList)
            {
                string query = "INSERT INTO queue(MIndex, Path1, Path2) VALUES('";
                query += MainWindow.NormalizeNumber(index, 10) + "','";
                query += Database.EscapeString(line[0]) + "',";
                query += ((line[1] == null) ? "NULL" : "'" + Database.EscapeString(line[1]) + "'");
                query += ")";
                index += 1;
                querys.Add(query);
            }
            bdd.DatabaseQuerys(querys.ToArray(), true);
        }


    }
}
