using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using m3uParser;
using m3uParser.Model;
using ByteDev.M3u;
using System.Net;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    public class RadioPlayer
    {
        private static Queue<WaveOutEvent> wavePlayerQueue = null;
        private static Queue<string> indexesPlayerQueue = null;
        private static WaveOutEvent wavePlayer = null;
        private static bool radioRecurrentReload = false;
        private static bool radioManualStop = false;
        public static bool corrupted = false;
        public static bool filefound = false;
        private static bool _IsPlaying = false;
        public static bool IsPlaying { get { return _IsPlaying; } }
        public static string radioPrefix = "";
        private static int radioChunkSize = 0;

        private static string Name = "";
        public static string PathStream = "";
        private static RadioType PathType = RadioType.Stream;

        private static System.Timers.Timer aTimer = null;

        public enum RadioType
        {
            Stream = 0,
            M3u = 1
        };

        private static void SetTitle(string title)
        { Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { /*App0.win1.SetTitle(title);*/ })); }

        public static void SetParent()
        {
        }

        public static async void Init(string path, RadioType type, string prefix = "", string name = null)
        {
            corrupted = false;
            PathStream = path;
            PathType = type;
            Name = name;
            radioManualStop = false;
            _IsPlaying = false;
            if (wavePlayerQueue != null)
            {
                while (wavePlayerQueue.Count > 0) { WaveOutEvent t = wavePlayerQueue.Dequeue(); t.Dispose(); t = null; }
                while (indexesPlayerQueue.Count > 0) { string t = indexesPlayerQueue.Dequeue(); t = null; }
            }
            else { wavePlayerQueue = new Queue<WaveOutEvent>(); indexesPlayerQueue = new Queue<string>(); }
            if (wavePlayer != null) { wavePlayer.Dispose(); wavePlayer = null; }
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer.Dispose();
                aTimer = null;
            }

            Player.Mode = Player.Modes.Radio;
            Player.PlaylistClear();
            SetTitle("Web Radio" + ((name != null && name.Trim() != "") ? " - " + name : ""));

            if (type == RadioType.Stream)
            {
                radioPrefix = "";
                radioRecurrentReload = false;
            }
            else if (type == RadioType.M3u)
            {
                if (prefix != null) { radioPrefix = prefix.Trim(); } else { radioPrefix = prefix; }
            }
        }

        private static async Task<bool> LoadChunks(int skip = 0)
        {
            Debug.WriteLine("LoadChunks");
            try
            {
                M3uPlaylist pl = null;
                Extm3u CurentRadioM3u = null;

                string data = null;
                filefound = true;
                corrupted = false;
                try
                {
                    if (System.IO.File.Exists(PathStream)) { data = System.IO.File.ReadAllText(PathStream); }
                    else { using (WebClient client = new WebClient()) { data = client.DownloadString(PathStream); } }
                }
                catch { filefound = false; return false; }

                if (data == null) { return false; }
                try
                {
                    pl = new M3uPlaylist(data);
                    if (Name == null)
                    {
                        if (pl.PlaylistTitle != null && pl.PlaylistTitle != "") { /*App0.win1.SetTitle("Web Radio - " + pl.PlaylistTitle);*/ }
                        else if (pl.Resources[0].TrackInfo != null && pl.Resources[0].TrackInfo != "") { /*App0.win1.SetTitle("Web Radio - " + pl.Resources[0].TrackInfo);*/ }
                        else
                        {
                            string[] tab = (radioPrefix + pl.Resources[0].Location).Split(':', StringSplitOptions.RemoveEmptyEntries);
                            SetTitle("Web Radio - " + tab[0] + ":" + tab[1]);
                        }
                    }
                    try
                    {
                        CurentRadioM3u = M3U.Parse(data);
                        radioChunkSize = (CurentRadioM3u.TargetDuration != null) ? (int)CurentRadioM3u.TargetDuration : 0;
                        if (radioChunkSize > 0) { radioRecurrentReload = true; } else { radioRecurrentReload = false; }
                    }
                    catch { radioChunkSize = 0; radioRecurrentReload = false; }

                    foreach (M3uResource m in pl.Resources)
                    {
                        if (radioChunkSize > 0)
                        {
                            if (skip > 0) { skip -= 1; continue; }
                            try
                            {
                                WaveOutEvent item = new WaveOutEvent();
                                MediaFoundationReader reader = new MediaFoundationReader(radioPrefix + m.Location);
                                item.Init(reader);
                                item.PlaybackStopped += WavePlayer_PlaybackStopped;
                                wavePlayerQueue.Enqueue(item);
                            }
                            catch { return false; }
                        }
                        else
                        {
                            try
                            {
                                wavePlayer = new WaveOutEvent();
                                MediaFoundationReader reader = new MediaFoundationReader(radioPrefix + m.Location);
                                wavePlayer.Init(reader);
                                break;
                            }
                            catch { return false; }
                        }
                    }
                    return true;
                }
                catch { corrupted = true; return false; }
            }
            catch (Exception err)
            {
                Debug.WriteLine(JsonConvert.SerializeObject(err));
                corrupted = true;
                return false;
            }
        }

        public static async Task<bool> Start()
        {
            try
            {
                radioManualStop = false;
                if (PathType == RadioType.M3u)
                {
                    if (aTimer != null) { aTimer.Stop(); aTimer.Dispose(); aTimer = null; }
                    bool ret = await LoadChunks();
                    if (ret == true)
                    {
                        if (radioChunkSize > 0)
                        {
                            aTimer = new Timer(radioChunkSize * 1000);
                            aTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                            {
                                wavePlayer = wavePlayerQueue.Dequeue();
                                wavePlayer.Play();
                            };
                            aTimer.AutoReset = true;
                            aTimer.Enabled = true;
                            wavePlayer = wavePlayerQueue.Dequeue();
                        }

                        wavePlayer.Play();
                        _IsPlaying = true;
                    }
                    else { return false; }
                }
                else
                {
                    if (wavePlayer != null)
                    {
                        wavePlayer.Dispose();
                        wavePlayer = null;
                    }
                    wavePlayer = new WaveOutEvent();
                    wavePlayer.DesiredLatency = 5000;
                    wavePlayer.Init(new MediaFoundationReader(PathStream));
                    wavePlayer.PlaybackStopped += WavePlayer_PlaybackStopped1_Stream;
                    wavePlayer.Play(); _IsPlaying = true;
                }
                return true;
            }
            catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); return false; }
        }

        private static void WavePlayer_PlaybackStopped1_Stream(object sender, StoppedEventArgs e)
        {
            Debug.WriteLine(" >>> WavePlayer_PlaybackStopped1_Stream");
            /*App0.win1.Pause();*/
        }

        public static async void Stop()
        {
            _IsPlaying = false;
            try
            {
                if (aTimer != null) { aTimer.Stop(); }
                radioManualStop = true;
                wavePlayer.PlaybackStopped -= WavePlayer_PlaybackStopped1_Stream;
                wavePlayer.Stop();
                wavePlayer.Dispose();
                wavePlayer = null;
                while (wavePlayerQueue.Count > 0) { WaveOutEvent t = wavePlayerQueue.Dequeue(); t.Dispose(); t = null; }
            }
            catch { }
        }

        private static async void WavePlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (radioRecurrentReload == true && radioManualStop == false && PathType == RadioType.M3u)
            {
                Debug.WriteLine("WavePlayer_PlaybackStopped");

                if (wavePlayerQueue.Count <= 2) { LoadChunks(2); }
                ((WaveOutEvent)sender).Dispose();
            }
            else { _IsPlaying = false; }
        }

    }
}
