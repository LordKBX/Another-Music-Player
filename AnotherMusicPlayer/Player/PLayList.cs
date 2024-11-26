using AnotherMusicPlayer.MainWindow2Space;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AnotherMusicPlayer
{
    public partial class Player
    {

        /// <summary> Add media into playlist </summary>
        public static bool PlaylistEnqueue(string[] files, bool random = false, int playIndex = 0, long playDuration = 0, bool autoplay = false)
        {
            int initialNbFiles = PlayList.Count;
            int goodFiles = 0;
            string[] Tfiles = files;
            if (random == true)
            {
                List<string> tmp = new List<string>();
                Random rnd = new Random();
                int index = -1;
                while (tmp.Count < files.Length)
                {
                    index = rnd.Next(0, files.Length);
                    if (tmp.Contains(files[index])) { continue; }
                    tmp.Add(files[index]);
                }
                Tfiles = tmp.ToArray();
            }
            foreach (string file in Tfiles)
            {
                if (TestFile(file))
                {
                    PlayList.Add(file);
                    goodFiles += 1;
                }
            }

            if (initialNbFiles == 0)
            {
                if (PlayList.Count > playIndex && playIndex >= 0)
                {
                    Open(PlayList[playIndex], autoplay, playDuration);
                    PlayListIndex = playIndex;
                    CurrentFile = PlayList[playIndex];
                }
            }

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged?.Invoke(evt);

            PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
            evt2.Position = PlayListIndex;
            PlaylistPositionChanged?.Invoke(evt2);

            SavePlaylist();

            return (goodFiles > 0) ? true : false;
        }

        /// <summary> Randomize playlist </summary>
        public static void PlaylistRandomize()
        {
            List<string> tmp = new List<string>();
            Random rnd = new Random();
            int initialIndex = PlayListIndex;
            string cFile = CurrentFile;
            if (PlayList.Count < initialIndex)
            {
                if (PlayList[initialIndex] != cFile) { cFile = PlayList[initialIndex]; }
            }
            int size = PlayList.Count;

            int index = -1;
            while (tmp.Count < size)
            {
                index = rnd.Next(0, size);
                if (tmp.Contains(PlayList[index])) { continue; }
                tmp.Add(PlayList[index]);
                if (PlayList[index] == cFile) { initialIndex = tmp.Count - 1; }
            }

            PlayList.Clear();
            PlayList.AddRange(tmp);
            PlayListIndex = initialIndex;

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged(evt);

            PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
            evt2.Position = PlayListIndex;
            PlaylistPositionChanged(evt2);

            SavePlaylist();
        }

        private static void SavePlaylist()
        {
            int lindex = 1;
            List<string> querys = new List<string>() { "DELETE FROM queue;" };
            foreach (string line in PlayList)
            {
                string query = "INSERT INTO queue(MIndex, Path1, Path2) VALUES('";
                query += App.NormalizeNumber(lindex, 10) + "','";
                query += Database.EscapeString(line) + "',";
                //query += ((line[1] == null) ? "NULL" : "'" + Database.EscapeString(line[1]) + "'");
                query += "NULL";
                query += ")";
                lindex += 1;
                querys.Add(query);
            }
            App.bdd.DatabaseQuerys(querys.ToArray(), true);
        }

        /// <summary> Clear playlist </summary>
        public static void PlaylistClear()
        {
            StopAll();
            PlayList.Clear();
            PlayListIndex = 0;
            CurrentFile = null;

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged(evt);

            PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
            evt2.Position = PlayListIndex;
            PlaylistPositionChanged(evt2);

            List<string> querys = new List<string>() { "DELETE FROM queue;" };
        }

        /// <summary> Read playlist </summary>
        public static void PlaylistReadIndex(int index)
        {
            if (index >= PlayList.Count) { return; }
            Debug.WriteLine("--> PlaylistReadIndex <--");
            Stop(PlayList[PlayListIndex]);
            PlayListIndex = index;
            Play(PlayList[PlayListIndex]);
            CurrentFile = PlayList[PlayListIndex];

            PlayerPlaylistPositionChangeParams evt = new PlayerPlaylistPositionChangeParams();
            evt.Position = PlayListIndex;
            PlaylistPositionChanged(evt);
        }

        /// <summary> Remove item from playlist </summary>
        public static void PlaylistRemoveIndex(int index)
        {
            if (index >= PlayList.Count) { return; }
            if (PlayList[index] == CurrentFile) { return; }
            bool reindex = false;
            if (PlayList.IndexOf(CurrentFile) > index) { reindex = true; }
            Debug.WriteLine("--> PlaylistRemoveIndex <--");
            PlayList.RemoveAt(index);

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged(evt);
            if (reindex)
            {
                PlayListIndex -= 1;
                PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
                evt2.Position = PlayListIndex;
                PlaylistPositionChanged(evt2);
            }
        }

        /// <summary> Remove items from playlist </summary>
        public static void PlaylistRemoveIndexes(int[] indexes)
        {
            bool reindex = false;
            Debug.WriteLine("--> PlaylistRemoveIndexes <--");
            List<int> idxs = new List<int>(indexes);
            idxs.Sort();
            for (int i = idxs.Count - 1; i >= 0; i--)
            {
                int index = idxs[i];
                if (index >= PlayList.Count) { continue; }
                if (PlayList[index] == CurrentFile) { continue; }
                if (PlayList.IndexOf(CurrentFile) > index) { reindex = true; PlayListIndex -= 1; }
                PlayList.RemoveAt(index);
            }

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged(evt);
            if (reindex)
            {
                PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
                evt2.Position = PlayListIndex;
                PlaylistPositionChanged(evt2);
            }
        }

        /// <summary> Read previous index in playlist </summary>
        public static void PlaylistPrevious()
        {
            Debug.WriteLine("--> PlaylistPrevious <--");
            PlayListIndex = ((PlayListIndex - 1) < 0) ? PlayList.Count - 1 : PlayListIndex - 1;
            Play(PlayList[PlayListIndex]);
            CurrentFile = PlayList[PlayListIndex];

            PlayerPlaylistPositionChangeParams evt = new PlayerPlaylistPositionChangeParams();
            evt.Position = PlayListIndex;
            PlaylistPositionChanged(evt);
        }

        /// <summary> Read next index in playlist </summary>
        public static void PlaylistNext()
        {
            Debug.WriteLine("--> PlaylistNext <--");
            PlayListIndex = ((PlayListIndex + 1) >= PlayList.Count) ? 0 : PlayListIndex + 1;
            if (!PlayLoop && PlayListIndex == 0) { return; }
            Play(PlayList[PlayListIndex]);
            CurrentFile = PlayList[PlayListIndex];

            PlayerPlaylistPositionChangeParams evt = new PlayerPlaylistPositionChangeParams();
            evt.Position = PlayListIndex;
            PlaylistPositionChanged(evt);
        }

        /// <summary> Preload next index in playlist </summary>
        public static void PlaylistPreloadNext()
        {
            //Debug.WriteLine("--> PlaylistPreloadNext <--");
            int nextIndex = ((PlayListIndex + 1) >= PlayList.Count) ? 0 : PlayListIndex + 1;
            if (!PlayLoop && nextIndex == 0) { return; }
            if (ThreadList.ContainsKey(PlayList[nextIndex])) { return; }
            Open(PlayList[nextIndex], false);
        }
    }
}
