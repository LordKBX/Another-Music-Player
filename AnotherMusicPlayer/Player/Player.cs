﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using NAudio.MediaFoundation;
using NAudio.Wave;
using TagLib;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Windows.Threading;

namespace AnotherMusicPlayer
{
    /// <summary> Player class, user for retrieving media file info or playing music </summary>
    public partial class Player
    {
        /// <summary> Give the List of native accepted file extentions </summary>
        public static readonly string[] AcceptedExtentions = new string[] { ".aiff", ".mp3", ".wma" };
        public static readonly string[] AcceptedExtentionsFotConversion = new string[] { ".aac", ".flac", ".ogg", ".m4a" };

        /// <summary> List of audio threads by file path </summary>
        private Dictionary<string, Thread> ThreadList = null;
        /// <summary> List of audio objects by file path </summary>
        private Dictionary<string, object> AudioList = null;
        /// <summary> List of playing status by file path </summary>
        private Dictionary<string, int> PlayStatus = null;
        /// <summary> List of playing new position by file path </summary>
        private Dictionary<string, long> PlayNewPositions = null;

        /// <summary> List of file path for the audio playlist </summary>
        public readonly List<string> PlayList = null;
        /// <summary> PlayList current position </summary>
        private int PlayListIndex = 0;
        public int Index
        {
            get { return PlayListIndex; }
            set { PlayListIndex = (value >= PlayList.Count) ? PlayList.Count - 1 : ((value < 0) ? value : value); }
        }

        /// <summary> Last played file </summary>
        private string CurrentFile = null;
        public string GetCurrentFile() { return CurrentFile; }
        /// <summary> Status if repeat file playback active </summary>
        private bool PlayRepeat = false;

        /// <summary> reference object of the application window </summary>
        private MainWindow parent;

        /// <summary> list of equalizer bands </summary>
        private readonly EqualizerBand[] bands;
        /// <summary> Maximum negative gain on an equalizer band </summary>
        public readonly int MinimumGain = -20;
        /// <summary> Maximum gain on an equalizer band </summary>
        public readonly int MaximumGain = 20;

        private List<string> _FfmpegPaths = null;
        public string[] FfmpegPaths
        {
            get { return (_FfmpegPaths != null) ? _FfmpegPaths.ToArray() : null; }
        }

        /// <summary>
        /// Dispose all used resources.
        /// </summary>
        public void Dispose()
        {
            StopAll();
            ThreadList = null;
            AudioList = null;
            PlayStatus = null;
            PlayNewPositions = null;

            PlayList.Clear();

            parent = null;

            GC.SuppressFinalize(this);
        }


        /// <summary> Constructor </summary>
        public Player(MainWindow parent = null)
        {
            this.parent = parent;
            ThreadList = new Dictionary<string, Thread>();
            AudioList = new Dictionary<string, object>();
            PlayStatus = new Dictionary<string, int>();
            PlayNewPositions = new Dictionary<string, long>();

            PlayList = new List<string>();
            //PlayListSemaphore = new Semaphore(0, 1);

            string AppName = parent.AppName;
            char sep = System.IO.Path.DirectorySeparatorChar;
            _FfmpegPaths = new List<string>() {
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + sep + AppName + sep + "ffmpeg-win64-static.exe",
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + sep + AppName + sep + "ffmpeg-win32-static.exe",
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + sep + AppName + sep + "ffmpeg.exe",
                AppDomain.CurrentDomain.BaseDirectory + sep + "ffmpeg-win64-static.exe",
                AppDomain.CurrentDomain.BaseDirectory + sep + "ffmpeg-win32-static.exe",
                AppDomain.CurrentDomain.BaseDirectory + sep + "ffmpeg.exe"
            };


            var name = "PATH";
            var scope = EnvironmentVariableTarget.Process; // or User
            var oldValue = Environment.GetEnvironmentVariable(name, scope);
            var newValue = oldValue + @";" + AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar;
            Environment.SetEnvironmentVariable(name, newValue, scope);

            //--- NEW ---
            bands = new EqualizerBand[]
                {
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 60, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 170, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 310, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 600, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 1000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 3000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 6000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 12000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 14000, Gain = 0},
                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 16000, Gain = 0}
                };
        }


        /// <summary> clear current file value </summary>
        public void ClearCurrentFile() { CurrentFile = null; }

        public long GetCurrentFilePosition()
        {
            if (CurrentFile == null) { return 0; }
            if (!AudioList.ContainsKey(CurrentFile)) { return 0; }
            return Convert.ToInt64(((AudioFileReader)AudioList[CurrentFile]).CurrentTime.TotalMilliseconds);
        }

        /// <summary> update an equalizer band Gain value </summary>
        public void UpdateEqualizer(int Band, float Gain)
        {
            try { bands[Band].Gain = Gain; } catch { }
            //Debug.WriteLine("Equalizer Band("+Band+") = Gain " + Gain);
        }

        /// <summary> update an equalizer with List<(int,float)>, int = band indicator, float = band gain </summary>
        public void UpdateEqualizer(List<(int, float)> tab)
        {
            foreach ((int, float) band in tab)
            {
                try { bands[band.Item1].Gain = band.Item2; } catch { }
            }
        }

        /// <summary> Define status if repeat file playback active </summary>
        public void Repeat(bool rep) { PlayRepeat = rep; }
        /// <summary> Get status if repeat file playback active </summary>
        public bool IsRepeat() { return PlayRepeat; }

        /// <summary> Stop all currently playing threads </summary>
        public void StopAll()
        {
            if (ThreadList.Count > 0)
            {
                foreach (KeyValuePair<string, Thread> entry in ThreadList)
                {
                    PlayStatus[entry.Key] = 2;
                }
                foreach (KeyValuePair<string, object> entry in AudioList)
                {
                    try { ((AudioFileReader)entry.Value).Dispose(); }
                    catch (Exception) { }
                }

                ThreadList.Clear();
                PlayStatus.Clear();
                PlayNewPositions.Clear();
                AudioList.Clear();
                CurrentFile = null;

                try
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception) { }
            }
        }

        /// <summary> Test if file exist, if input = null remplace it with value in CurrentFile </summary>
        public bool TestFile(string FilePath = null)
        {
            if (FilePath == null) { if (CurrentFile == null) { return false; }; FilePath = CurrentFile; }
            if (System.IO.File.Exists(FilePath)) { return true; }
            else { return false; }
        }

        /// <summary> Add media into playlist </summary>
        public bool PlaylistEnqueue(string[] files, bool random = false, int playIndex = 0, long playDuration = 0, bool autoplay = false)
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
            PlaylistChanged(this, evt);

            PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
            evt2.Position = PlayListIndex;
            PlaylistPositionChanged(this, evt2);

            return (goodFiles > 0) ? true : false;
        }

        /// <summary> Randomize playlist </summary>
        public void PlaylistRandomize()
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
            PlaylistChanged(this, evt);

            PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
            evt2.Position = PlayListIndex;
            PlaylistPositionChanged(this, evt2);
        }

        /// <summary> Clear playlist </summary>
        public void PlaylistClear()
        {

            StopAll();
            PlayList.Clear();
            PlayListIndex = 0;
            CurrentFile = null;

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged(this, evt);

            PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
            evt2.Position = PlayListIndex;
            PlaylistPositionChanged(this, evt2);
        }

        /// <summary> Read playlist </summary>
        public void PlaylistReadIndex(int index)
        {
            if (index >= PlayList.Count) { return; }
            Debug.WriteLine("--> PlaylistReadIndex <--");
            Stop(PlayList[PlayListIndex]);
            PlayListIndex = index;
            Play(PlayList[PlayListIndex]);
            CurrentFile = PlayList[PlayListIndex];

            PlayerPlaylistPositionChangeParams evt = new PlayerPlaylistPositionChangeParams();
            evt.Position = PlayListIndex;
            PlaylistPositionChanged(this, evt);
        }

        /// <summary> Remove item from playlist </summary>
        public void PlaylistRemoveIndex(int index)
        {
            if (index >= PlayList.Count) { return; }
            if (PlayList[index] == CurrentFile) { return; }
            bool reindex = false;
            if (PlayList.IndexOf(CurrentFile) > index) { reindex = true; }
            Debug.WriteLine("--> PlaylistRemoveIndex <--");
            PlayList.RemoveAt(index);

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged(this, evt);
            if (reindex)
            {
                PlayListIndex -= 1;
                PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
                evt2.Position = PlayListIndex;
                PlaylistPositionChanged(this, evt2);
            }
        }

        /// <summary> Remove items from playlist </summary>
        public void PlaylistRemoveIndexes(int[] indexes)
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
            PlaylistChanged(this, evt);
            if (reindex)
            {
                PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
                evt2.Position = PlayListIndex;
                PlaylistPositionChanged(this, evt2);
            }
        }

        /// <summary> Read next index in playlist </summary>
        public void PlaylistNext()
        {
            Debug.WriteLine("--> PlaylistNext <--");
            PlayListIndex = ((PlayListIndex + 1) >= PlayList.Count) ? 0 : PlayListIndex + 1;
            Play(PlayList[PlayListIndex]);
            CurrentFile = PlayList[PlayListIndex];

            PlayerPlaylistPositionChangeParams evt = new PlayerPlaylistPositionChangeParams();
            evt.Position = PlayListIndex;
            PlaylistPositionChanged(this, evt);
        }

        /// <summary> Preload next index in playlist </summary>
        public void PlaylistPreloadNext()
        {
            //Debug.WriteLine("--> PlaylistPreloadNext <--");
            int nextIndex = ((PlayListIndex + 1) >= PlayList.Count) ? 0 : PlayListIndex + 1;
            if (ThreadList.ContainsKey(PlayList[nextIndex])) { return; }
            Open(PlayList[nextIndex], false);
        }

        /// <summary> Open a new media playing thread </summary>
        public bool Open(string FilePath, bool AutoPlay = false, long playDuration = 0)
        {
            //if (IsPlaying(FilePath)) { return false; }
            if (TestFile(FilePath) && !ThreadList.ContainsKey(FilePath))
            {
                try
                {
                    if (playDuration > 0) { PlayNewPositions.Add(FilePath, playDuration); }
                    else { PlayNewPositions.Add(FilePath, -1); }
                    Thread objThread = new Thread(new ParameterizedThreadStart(PlaySoundAsync));
                    objThread.IsBackground = true;
                    objThread.Priority = ThreadPriority.AboveNormal;
                    objThread.Start(FilePath);
                    ThreadList.Add(FilePath, objThread);
                    PlayStatus.Add(FilePath, (AutoPlay) ? 1 : 0);
                    PlayNewPositions.Add(FilePath, -1);
                    if (AutoPlay)
                    {
                        CurrentFile = FilePath;
                        return true;
                    }
                }
                catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
            }
            return false;
        }

        /// <summary> Start media play for FilePath or CurrentFile if FilePath is null </summary>
        public bool Play(string FilePath = null, long playDuration = 0)
        {
            if (FilePath == null) { FilePath = CurrentFile; }
            if (FilePath == null) { return false; }
            try
            {
                if (FilePath == CurrentFile) { PlayStatus[FilePath] = 1; return true; }
                if (TestFile(FilePath))
                {
                    if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 1; return true; }
                    else { return Open(FilePath, true, playDuration); }
                }
            }
            catch { }
            return false;
        }

        /// <summary> Resume media play for FilePath or CurrentFile if FilePath is null </summary>
        public bool Resume(string FilePath = null)
        {
            if (IsSuspended) { IsSuspended = false; PlayStatus[CurrentFile] = 1; return true; } else { return Play(FilePath); }
        }

        /// <summary> Pause media play for FilePath or CurrentFile if FilePath is null </summary>
        public bool Pause(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 0; return true; }
            }
            return false;
        }

        private bool IsSuspended = false;
        /// <summary> Suspend media play for FilePath or CurrentFile if FilePath is null </summary>
        public bool Suspend()
        {
            if (CurrentFile != null)
            {
                IsSuspended = true;
                Stop();
            }
            return false;
        }

        /// <summary> Stop media play for FilePath or CurrentFile if FilePath is null </summary>
        public bool Stop(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 2; return true; }
            }
            return false;
        }

        /// <summary> Test if FilePath played for FilePath or CurrentFile if FilePath is null </summary>
        public bool IsPlaying(string FilePath = null)
        {
            try
            {
                if (TestFile(FilePath))
                {
                    if (FilePath == null) { FilePath = CurrentFile; }
                    if (!PlayStatus.ContainsKey(FilePath)) { return false; }
                    if (PlayStatus[FilePath] == 1) { return true; }
                }
            }
            catch { }
            return false;
        }

        /// <summary> Get/Set position for FilePath or CurrentFile if FilePath is null </summary>
        public long Position(string FilePath = null, long position = -1)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (!PlayStatus.ContainsKey(FilePath)) { return -1; }
                if (position != -1) { PlayNewPositions[FilePath] = position; return position; }
                else { return (long)((AudioFileReader)AudioList[FilePath]).CurrentTime.TotalMilliseconds; }
            }
            return -1;
        }

        /// <summary> advance playing time of current file by 5 seconds </summary>
        public void PlayTimeAdvance(long seconds)
        {
            if (CurrentFile == null) { return; }
            if (TestFile(CurrentFile))
            {
                if (!PlayStatus.ContainsKey(CurrentFile)) { return; }
                long pos = ((AudioFileReader)AudioList[CurrentFile]).Position;
                long lg = ((AudioFileReader)AudioList[CurrentFile]).Length;
                long coef = lg / (long)((AudioFileReader)AudioList[CurrentFile]).TotalTime.TotalMilliseconds;
                long newPos = (pos + (seconds * 1000 * coef));
                if (newPos >= lg) { return; }
                ((AudioFileReader)AudioList[CurrentFile]).Position = newPos;
            }
        }

        /// <summary> rewing playing time of current file by 5 seconds </summary>
        public void PlayTimeRewind(long seconds)
        {
            if (CurrentFile == null) { return; }
            if (TestFile(CurrentFile))
            {
                long pos = ((AudioFileReader)AudioList[CurrentFile]).Position;
                long lg = ((AudioFileReader)AudioList[CurrentFile]).Length;
                long coef = lg / (long)((AudioFileReader)AudioList[CurrentFile]).TotalTime.TotalMilliseconds;
                long newPos = (pos - (seconds * 1000 * coef));
                if (newPos <= 0) { return; }
                ((AudioFileReader)AudioList[CurrentFile]).Position = newPos;
            }
        }

        /// <summary> Get media length of FilePath or CurrentFile if FilePath is null </summary>
        public long Length(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (!AudioList.ContainsKey(FilePath)) { return -1; }
                return (long)((AudioFileReader)AudioList[FilePath]).TotalTime.TotalMilliseconds;
            }
            return -1;
        }

        /// <summary> Playing thread </summary>
        private async void PlaySoundAsync(object file)
        {
            try
            {
                string FilePath = (string)file;
                bool started = false;
                AudioFileReader audioFile = null;
                Equalizer equalizer = null;
                WaveOutEvent outputDevice = null;

                try { audioFile = new AudioFileReader(FilePath); }
                catch (InvalidOperationException err)
                {
                    Debug.WriteLine("ERROR ==> VBR File detected, try file convertion");
                    try
                    {
                        string path2 = Path.GetTempFileName();
                        System.IO.File.Delete(path2);
                        path2 += ".mp3";

                        bool retC = await ConvExe(FilePath, path2);
                        if (retC == false)
                        {
                            Debug.WriteLine("ERROR ==> VBR File convertion failure");
                            PlaylistNext(); return;
                        }
                        FilesTags.SaveMediaInfo(path2, FilesTags.MediaInfo(FilePath, false), FilePath);
                        System.IO.File.Move(FilePath, FilePath + ".old");
                        System.IO.File.Move(path2, FilePath);
                        audioFile = new AudioFileReader(FilePath);
                        System.IO.File.Delete(FilePath + ".old");
                    }
                    catch (Exception err2)
                    {
                        Debug.WriteLine("ERROR ==> VBR File convertion failure");
                        PlaylistNext(); return;
                    }
                }
                catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); PlaylistNext(); return; }
                equalizer = new Equalizer((ISampleProvider)audioFile, bands);
                outputDevice = new WaveOutEvent();

                //outputDevice.Init((IWaveProvider)audioFile);
                outputDevice.Init(equalizer);
                if (!AudioList.ContainsKey(FilePath)) { AudioList.Add(FilePath, audioFile); }

                int ret = -1; long ret2 = -1;

                //while (outputDevice.PlaybackState == PlaybackState.Playing)
                while (true)
                {
                    ret2 = ret = -1;
                    PlayStatus.TryGetValue(FilePath, out ret);
                    PlayNewPositions.TryGetValue(FilePath, out ret2);

                    if (outputDevice == null)
                    {
                        if (IsSuspended == true) { Thread.Sleep(100); continue; }
                        else
                        {
                            Debug.WriteLine("REPLAY !!!!");
                            audioFile = new AudioFileReader(FilePath);
                            equalizer = new Equalizer((ISampleProvider)audioFile, bands);
                            outputDevice = new WaveOutEvent();
                            outputDevice.Init(equalizer);
                            if (!AudioList.ContainsKey(FilePath)) { AudioList.Add(FilePath, audioFile); }
                            outputDevice.Play();
                        }
                    }
                    equalizer.Update();

                    if (ret == 0 && outputDevice.PlaybackState == PlaybackState.Playing) { outputDevice.Pause(); }
                    if (ret == 1 && outputDevice.PlaybackState != PlaybackState.Playing)
                    {
                        outputDevice.Play(); CurrentFile = FilePath;
                        PlayerLengthChangedEventParams evtp = new PlayerLengthChangedEventParams();
                        evtp.duration = (long)(((AudioFileReader)audioFile).TotalTime.TotalMilliseconds);
                        LengthChanged(this, evtp);
                        if (started == false) { parent.bdd.playCountUpdate(FilePath); }
                        started = true;
                    }
                    if (ret == 2)
                    {
                        outputDevice.Stop();
                        if (IsSuspended == true)
                        {
                            long msval = audioFile.WaveFormat.AverageBytesPerSecond / 1000;
                            PlayNewPositions[FilePath] = audioFile.Position / msval;
                            try { outputDevice.Stop(); } catch { }
                            outputDevice.Dispose();
                            outputDevice = null;
                            equalizer = null;
                            audioFile.Close();
                            audioFile.Dispose();
                            audioFile = null;
                            AudioList.Remove(FilePath);
                        }
                        else { break; }
                    }
                    if (ret2 != -1)
                    {
                        long msval = audioFile.WaveFormat.AverageBytesPerSecond / 1000;
                        audioFile.Position = ret2 * msval;
                        PlayNewPositions[FilePath] = -1;
                    }
                    if (IsSuspended == false)
                    {
                        try
                        {
                            PlayerPositionChangedEventParams evt = new PlayerPositionChangedEventParams();
                            evt.Position = (long)(audioFile.CurrentTime.TotalMilliseconds);
                            evt.duration = (long)(audioFile.TotalTime.TotalMilliseconds);
                            if (FilePath == CurrentFile) PositionChanged(this, evt);
                            if (outputDevice.PlaybackState == PlaybackState.Stopped && started == true && evt.Position > 0)
                            {
                                if (PlayRepeat || (PlayListIndex + 1 == PlayList.Count && PlayList.Count == 1))
                                {
                                    audioFile.Position = 0;
                                    PlayStatus[FilePath] = 1;
                                    outputDevice.Play();
                                }
                                else
                                {
                                    PlaylistNext();
                                    break;
                                }
                            }

                            if (evt.Position >= evt.duration - 5000) { PlaylistPreloadNext(); }
                        }
                        catch { break; }
                    }

                    Thread.Sleep(100);
                }
                try { outputDevice.Stop(); } catch { }

                outputDevice.Dispose();
                ((AudioFileReader)audioFile).Close(); ((AudioFileReader)audioFile).Dispose();
                PlayStatus.Remove(FilePath);
                PlayNewPositions.Remove(FilePath);
                if (AudioList.ContainsKey(FilePath))
                {
                    ((AudioFileReader)AudioList[FilePath]).Dispose();
                    AudioList.Remove(FilePath);
                }
                ThreadList.Remove(FilePath);
            }
            catch (Exception error) { Debug.WriteLine(JsonConvert.SerializeObject(error)); }
            return;
        }
    }
}
