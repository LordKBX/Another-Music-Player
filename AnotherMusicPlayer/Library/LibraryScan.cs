using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Windows.Input;

namespace AnotherMusicPlayer
{
    public partial class Library
    {
        private List<string> CacheQuerys;

        public async void InvokeScan(bool preClean = false)
        {
            Thread objThread = new Thread(new ParameterizedThreadStart(Scan));
            objThread.IsBackground = true;
            objThread.Priority = ThreadPriority.Normal;
            objThread.Start(false);
        }

        public async void Scan(object param) {
            Scan((bool)param);
        }
        private async void Scan(bool preClean = false) {
            _Scanning = true;
            Parent.setLoadingState(true, "Library Scan");
            if (preClean is true)
            {
                Bdd.DatabaseQuerys(new string[] { "DELETE FROM covers", "DELETE FROM folders", "DELETE FROM files", "DELETE FROM playlists", "DELETE FROM playlistsItems" });
            }
            Dictionary<string, Dictionary<string, object>> DatabaseFiles = Bdd.DatabaseQuery("SELECT * FROM files ORDER BY Path ASC", "Path");
            string[] files = getDirectoryMediaFIles(RootPath, true);
            List<string> listToScan = new List<string>();

            CacheQuerys = new List<string>();

            foreach (string file in files) {
                FileInfo fi = new FileInfo(file);
                if (DatabaseFiles.ContainsKey(file)) 
                {
                    Dictionary<string, object> dfi = DatabaseFiles[file];
                    if (Convert.ToInt64((string)dfi["LastUpdate"]) != Convert.ToInt64(fi.LastWriteTimeUtc.ToFileTime()) || Convert.ToInt32(dfi["Size"]) == 0)
                    {
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

        private void InsertBddFile(FileInfo fi = null)
        {
            if (fi == null) { return; }
            string query = "INSERT INTO files(Path, Name, Album, Performers, Composers, Genres, Copyright, AlbumArtists, Lyrics, Duration, Size, Disc, " +
                "DiscCount, Track, TrackCount, Year, LastUpdate) VALUES('";
            query += Database.EscapeString(fi.FullName) + "',";
            query += "'" + Database.EscapeString(Path.GetFileName(fi.Name)) + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'0','0','0','0','0','0','0',";

            query += "'" + fi.LastWriteTimeUtc.ToFileTime() + "')";
            CacheQuerys.Add(query);

            if (CacheQuerys.Count >= 100)
            {
                Bdd.DatabaseQuerys(CacheQuerys.ToArray());
                CacheQuerys.Clear();
            }
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