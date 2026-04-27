using AnotherMusicPlayer.MainWindow2Space;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        private List<string> CacheQuerys;

        public void InvokeScan(bool preClean = false)
        {
            Thread objThread = new Thread(new ParameterizedThreadStart(Scan));
            objThread.IsBackground = true;
            objThread.Priority = ThreadPriority.Normal;
            objThread.Start(false);
        }

        public void Scan(object param)
        {
            Scan((bool)param);
        }

        private void Scan(bool preClean = false)
        {
            _Scanning = true;
            Parent.setLoadingState(true, "Library Scan");
            if (preClean is true)
            {
                Bdd.DatabaseQuerys(new string[] { "DELETE FROM covers", "DELETE FROM files", "DELETE FROM playlists", "DELETE FROM playlistsItems" });
            }
            Dictionary<string, Dictionary<string, object>> DatabaseFiles = Bdd.DatabaseQuery("SELECT * FROM files ORDER BY Path ASC", "Path");
            string[] files = getDirectoryMediaFIles(RootPath, true);
            List<string> listToScan = new List<string>();

            CacheQuerys = new List<string>();

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (DatabaseFiles.ContainsKey(file))
                {
                    Dictionary<string, object> dfi = DatabaseFiles[file];
                    if (Convert.ToInt64((string)dfi["LastUpdate"]) < Convert.ToInt64(fi.LastWriteTimeUtc.ToFileTime()) || Convert.ToInt32(dfi["Size"]) == 0)
                    {
                        /* too much process imposing */
                        //float averageVolume = GetAverageVolume(fi.FullName);
                        //string query = "UPDATE files SET AverageVolume = '" + averageVolume + "' WHERE Path = '" + file.Replace("'", "''") + "'";
                        //Bdd.DatabaseTansactionEnd();
                        //Bdd.DatabaseQuery(query, null, true);

                        listToScan.Add(file);
                    }
                }
                else
                {
                    listToScan.Add(file);
                    InsertBddFile(fi);
                }
            }

            if (CacheQuerys.Count > 0)
            {
                Bdd.DatabaseQuerys(CacheQuerys.ToArray());
                CacheQuerys.Clear();
            }
            Parent.setLoadingState(false);

            Thread objThread = new Thread(new ParameterizedThreadStart(ScanTags));
            objThread.IsBackground = true;
            objThread.Priority = ThreadPriority.Normal;
            objThread.Start(listToScan.ToArray());
            _Scanning = false;
        }

        public static readonly float FailledAverageVolume = -100.0f;
        public static float GetAverageVolume(string file)
        {
            char sep = System.IO.Path.DirectorySeparatorChar;
            string exePath = Player.GetFfmpegPath();
            if (exePath == null) { return FailledAverageVolume; }

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = exePath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-i \"" + file + "\" -filter:a volumedetect -f null /dev/null";

            StringBuilder standardOutput = new StringBuilder();
            startInfo.RedirectStandardError = true;

            //Debug.WriteLine("--> GetAverageVolume");
            //Debug.WriteLine(startInfo.FileName + " " + startInfo.Arguments);
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    // read chunk-wise while process is running.
                    while (!exeProcess.HasExited)
                    {
                        standardOutput.Append(exeProcess.StandardError.ReadToEnd());
                    }

                    // make sure not to miss out on any remaindings.
                    standardOutput.Append(exeProcess.StandardOutput.ReadToEnd());

                    //Debug.WriteLine("Output => " + standardOutput.ToString());
                    string[] lines = standardOutput.ToString().Replace("\r","").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines) {
                        if (line.StartsWith("[Parsed_volumedetect_") && line.Contains("mean_volume: ")) 
                        {
                            //Debug.WriteLine(line);
                            string[] parts = line.Split("mean_volume: ");
                            if (parts.Length > 1)
                            {
                                string valuePart = parts[1].Trim().Split(' ')[0].Replace(".", ",");
                                if (float.TryParse(valuePart, out float averageVolume))
                                {
                                    return averageVolume;
                                }
                            }
                        }
                    }


                    return FailledAverageVolume;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("--> ConvExe ERROR : " + JsonConvert.SerializeObject(e));
            }
            return FailledAverageVolume;
        }

        private void InsertBddFile(FileInfo fi = null, bool commit = false)
        {
            if (fi == null) { return; }
            //float averageVolume = GetAverageVolume(fi.FullName);
            float averageVolume = -100.0f;

            string query = "INSERT INTO files(Path, Name, Album, Performers, Composers, Genres, Copyright, AlbumArtists, Lyrics, Duration, Size, Disc, " +
                "DiscCount, Track, TrackCount, Year, Rating, AverageVolume, InsertionDate, LastUpdate) VALUES('";
            query += Database.EscapeString(fi.FullName) + "',";
            query += "'" + Database.EscapeString(Path.GetFileName(fi.Name)) + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0','0','0','0','0','0','0','0.0','"+ ("" + averageVolume).Replace(".", ",") + "',";

            query += "'" + fi.CreationTimeUtc.ToFileTime() + "',";
            query += "'" + fi.LastWriteTimeUtc.ToFileTime() + "')";

            if (commit == true) { Bdd.DatabaseQuerys(new string[] { query }); }
            else
            {
                if (CacheQuerys.Count >= 100)
                {
                    Bdd.DatabaseQuerys(CacheQuerys.ToArray());
                    CacheQuerys.Clear();
                }
            }
            CacheQuerys.Add(query);
        }

        /// <summary> Launch a scan of the files for tag retrieval </summary>
        private async void ScanTags(object param = null)
        {
            Debug.WriteLine("--> ScanTags <--");
            string[] files = (string[])param;
            Parent.setMetadataScanningState(true, files.Length);
            _ = Bdd.DatabaseFilesInfo(files, Parent, true);
            Parent.setMetadataScanningState(false);
            Debug.WriteLine("--> ScanTags END <--");
        }
    }
}