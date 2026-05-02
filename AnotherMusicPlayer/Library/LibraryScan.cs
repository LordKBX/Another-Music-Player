using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

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

        public static readonly float FailledAverageGain = 0.01f;
        public static float GetGain(string file)
        {
            Debug.WriteLine("Libray.GetGain(string file)");
            if (!File.Exists(file)) { return FailledAverageGain; }
            float calc = 0;
            if (file.EndsWith(".mp3")) { calc = GetGainMp3(file); }


            Debug.WriteLine("Mp3Gain rez = " + calc);
            
            return calc;
        }

        public static float GetGainMp3(string file)
        {
            if (!file.EndsWith(".mp3")) { return FailledAverageGain; }
            if (!File.Exists(file)) { return FailledAverageGain; }
            bool needTempFile = false;
            bool deletedTempFile = false;

            if (AnotherMusicPlayer.Common.StringContainsUnicode(file)) {
                needTempFile = true;
                string tmp = Path.GetTempPath() + Path.DirectorySeparatorChar + Guid.NewGuid().ToString() + ".mp3";
                File.Copy(file, tmp, true);
                file = tmp;
            }

            char sep = System.IO.Path.DirectorySeparatorChar;
            string exePath = Player.GetMp3GainPath();
            if (exePath == null) { return FailledAverageGain; }

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = exePath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "/q /s r /o \"" + file + "\"";

            StringBuilder standardOutput = new StringBuilder();
            startInfo.RedirectStandardError = true;

            Debug.WriteLine(startInfo.FileName + " " + startInfo.Arguments);
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

                    string[] lines = standardOutput.ToString().Replace("\r","").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (needTempFile) { File.Delete(file); deletedTempFile = true; }

                    Debug.WriteLine("Output => " + string.Join('\n', lines));
                    if (lines.Length > 1) {
                        string[] blocks = lines[1].Split('\t');
                        if (blocks.Length > 1)
                        {
                            return float.Parse(blocks[1]);
                        }
                    }

                    return FailledAverageGain;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("--> ConvExe ERROR : " + JsonConvert.SerializeObject(e));
            }
            if (needTempFile && !deletedTempFile) { File.Delete(file); }
            return FailledAverageGain;
        }

        private void InsertBddFile(FileInfo fi = null, bool commit = false)
        {
            if (fi == null) { return; }

            string query = "INSERT INTO files(Path, Name, Album, Performers, Composers, Genres, Copyright, AlbumArtists, Lyrics, Duration, Size, Disc, " +
                "DiscCount, Track, TrackCount, Year, Rating, Gain, InsertionDate, LastUpdate) VALUES('";
            query += Database.EscapeString(fi.FullName) + "',";
            query += "'" + Database.EscapeString(Path.GetFileName(fi.Name)) + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0','0','0','0','0','0','0','0.0','"+ ("" + 0.0).Replace(".", ",") + "',";

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