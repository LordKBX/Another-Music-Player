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
    public partial class MainWindow : Window {
        /// <summary> Asynchronus call for loading old playlist in a new thread </summary>
        private async void MediatequeLoadOldPlaylist()
        {
            try
            {
                Thread objThread = new Thread(new ParameterizedThreadStart(MediatequeLoadOldPlaylistP2));
                objThread.IsBackground = true;
                objThread.Priority = ThreadPriority.AboveNormal;
                objThread.Start(null);
            }
            catch { }
        }

        private void MediatequeLoadOldPlaylistP2(object param = null)
        {
            Dictionary<string, Dictionary<string, object>> LastPlaylist = DatabaseQuery("SELECT MIndex,Path1,Path2 FROM queue ORDER BY MIndex ASC", "Index");
            if (LastPlaylist != null)
            {
                if (LastPlaylist.Count > 0)
                {
                    Debug.WriteLine("Old PlayList detected");
                    List<string> gl = new List<string>();
                    int fails = 0;
                    foreach (KeyValuePair<string, Dictionary<string, object>> fi in LastPlaylist)
                    {
                        string path1 = (string)fi.Value["Path1"];
                        string path2 = (fi.Value["Path2"] == null) ? null : (string)fi.Value["Path2"];
                        if (path2 != null)
                        {
                            if (System.IO.File.Exists(path2)) { gl.Add(path2); }
                            else
                            {
                                if (System.IO.File.Exists(path1)) { gl.Add(path1); } else { fails += 1; }
                            }
                        }
                        else if (System.IO.File.Exists(path1)) { gl.Add(path1); }
                        else { fails += 1; }
                    }
                    int newIndex = -1;
                    if (fails > 0) { newIndex = 0; }
                    else { newIndex = Settings.LastPlaylistIndex; }

                    Open(gl.ToArray(), false, newIndex);
                    //player.Stop();
                }
            }
        }


    }
}
