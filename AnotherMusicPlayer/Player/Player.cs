using System;
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
        /// <summary> Give the List of accepted file extentions for conversion </summary>
        public static readonly string[] AcceptedExtentionsFotConversion = new string[] { ".aac", ".flac", ".ogg", ".m4a" };

        /// <summary> Enum the differents mode of the player object </summary>
        public enum Modes { File = 0, Radio = 1 };
        /// <summary> Current Mode of the player object </summary>
        public static Modes Mode = Modes.File;

        /// <summary> List of audio threads by file path </summary>
        private static Dictionary<string, Thread> ThreadList = null;
        /// <summary> List of audio objects by file path </summary>
        private static Dictionary<string, object> AudioList = null;
        /// <summary> List of playing status by file path </summary>
        private static Dictionary<string, int> PlayStatus = null;
        /// <summary> List of playing new position by file path </summary>
        private static Dictionary<string, long> PlayNewPositions = null;

        /// <summary> List of file path for the audio playlist </summary>
        public static readonly List<string> PlayList = new List<string>();

        /// <summary> PlayList current Index </summary>
        private static int PlayListIndex = 0;
        /// <summary> PlayList current Index </summary>
        public static int Index { get { return PlayListIndex; } set { PlayListIndex = (value >= PlayList.Count) ? PlayList.Count - 1 : ((value < 0) ? value : value); } }

        /// <summary> Current playing file </summary>
        private static string CurrentFile = null;
        /// <summary> Current playing file </summary>
        public static string GetCurrentFile() { return CurrentFile; }

        /// <summary> Status if repeat file playback active </summary>
        private static bool PlayRepeat = false;

        /// <summary> List of potential path for the Ffmpeg Executable </summary>
        private static List<string> _FfmpegPaths = null;
        /// <summary> List of potential path for the Ffmpeg Executable </summary>
        public static string[] FfmpegPaths { get { return (_FfmpegPaths != null) ? _FfmpegPaths.ToArray() : null; } }

        private static PlayerStatus _LatestPlayerStatus = PlayerStatus.Stop;
        public static PlayerStatus LatestPlayerStatus { get{ return _LatestPlayerStatus; } }


        /// <summary> Constructor </summary>
        public static void INIT()
        {
            ThreadList = new Dictionary<string, Thread>();
            AudioList = new Dictionary<string, object>();
            PlayStatus = new Dictionary<string, int>();
            PlayNewPositions = new Dictionary<string, long>();
            Mode = Modes.File;

            //PlayListSemaphore = new Semaphore(0, 1);

            string AppName = App.AppName;
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

            RadioPlayer.SetParent();

            InitializeEqualizer();
        }


        /// <summary> clear current file value </summary>
        public static void ClearCurrentFile() { CurrentFile = null; }

        public static long GetCurrentFilePosition()
        {
            if (CurrentFile == null) { return 0; }
            if (!AudioList.ContainsKey(CurrentFile)) { return 0; }
            return Convert.ToInt64(((AudioFileReader)AudioList[CurrentFile]).CurrentTime.TotalMilliseconds);
        }

        /// <summary> Define status if repeat file playback active </summary>
        public static void Repeat(bool rep) { PlayRepeat = rep; }
        /// <summary> Get status if repeat file playback active </summary>
        public static bool IsRepeat() { return PlayRepeat; }

        /// <summary> Stop all currently playing threads </summary>
        public static void StopAll()
        {
            if (Mode == Modes.File)
            {
                if (ThreadList.Count > 0)
                {
                    foreach (KeyValuePair<string, Thread> entry in ThreadList) { PlayStatus[entry.Key] = 2; }
                    foreach (KeyValuePair<string, object> entry in AudioList) { try { ((AudioFileReader)entry.Value).Dispose(); } catch (Exception) { } }

                    ThreadList.Clear(); PlayStatus.Clear(); PlayNewPositions.Clear(); AudioList.Clear();
                    CurrentFile = null;

                    try { GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers(); }
                    catch (Exception) { }
                }
            }
            else { RadioPlayer.Stop(); }
            _LatestPlayerStatus = PlayerStatus.Stop;
        }

        /// <summary> Stop media play for FilePath or CurrentFile if FilePath is null </summary>
        public static bool Stop(string FilePath = null)
        {
            if (Mode == Modes.File)
            {
                if (TestFile(FilePath))
                {
                    if (FilePath == null) { FilePath = CurrentFile; }
                    if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 2; _LatestPlayerStatus = PlayerStatus.Stop; return true; }
                }
                return false;
            }
            else { RadioPlayer.Stop(); _LatestPlayerStatus = PlayerStatus.Stop; return true; }
        }

        /// <summary> Test if file exist, if input = null remplace it with value in CurrentFile </summary>
        public static bool TestFile(string FilePath = null)
        {
            if (Mode == Modes.File)
            {
                if (FilePath == null) { if (CurrentFile == null) { return false; }; FilePath = CurrentFile; }
                if (System.IO.File.Exists(FilePath)) { return true; }
                else { return false; }
            }
            else { return false; }
        }

        /// <summary> Open a new media playing thread </summary>
        public static bool Open(string FilePath, bool AutoPlay = false, long playDuration = 0, Modes mode = Modes.File)
        {
            Mode = mode;
            if (mode == Modes.File)
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
            }
            else
            {
                PlaylistClear();
                PlayList.Add("Radio:" + FilePath);
                RadioPlayer.Init(FilePath, RadioPlayer.RadioType.M3u);
                if (AutoPlay) { RadioPlayer.Start(); }
            }
            return false;
        }

        /// <summary> Open a new media playing thread </summary>
        public static async void OpenStream(string streamUrl, RadioPlayer.RadioType streamType, string streamID = "", string streamName = "", bool streamAutoPlay = false, string streamPrefix = "")
        {
            Mode = Modes.Radio;

            StopAll();
            PlayList.Clear();
            PlayListIndex = 0;

            RadioPlayer.Init(streamUrl, streamType, streamPrefix, streamName);
            CurrentFile = "Radio|" + streamID + "|" + RadioPlayer.radioPrefix + RadioPlayer.PathStream;
            PlayList.Add(CurrentFile);
            if (streamAutoPlay) { _ = await RadioPlayer.Start(); }

            PlayerPlaylistChangeParams evt = new PlayerPlaylistChangeParams();
            evt.playlist = PlayList.ToArray();
            PlaylistChanged(evt);

            PlayerPlaylistPositionChangeParams evt2 = new PlayerPlaylistPositionChangeParams();
            evt2.Position = 0;
            PlaylistPositionChanged(evt2);
        }

        /// <summary> Start media play for FilePath or CurrentFile if FilePath is null </summary>
        public static bool Play(string FilePath = null, long playDuration = 0)
        {
            if (Mode == Modes.File)
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
            }
            else
            {
                RadioPlayer.Start();
            }
            return false;
        }

        /// <summary> Resume media play for FilePath or CurrentFile if FilePath is null </summary>
        public static async Task<bool> Resume(string FilePath = null)
        {
            if (Mode == Modes.File)
            {
                if (IsSuspended) { IsSuspended = false; PlayStatus[CurrentFile] = 1; return true; } else { return Play(FilePath); }
            }
            else
            {
                if (IsSuspended) { IsSuspended = false; }
                return await RadioPlayer.Start();
            }
        }

        /// <summary> Pause media play for FilePath or CurrentFile if FilePath is null </summary>
        public static bool Pause(string FilePath = null)
        {
            if (Mode == Modes.File)
            {
                if (TestFile(FilePath))
                {
                    if (FilePath == null) { FilePath = CurrentFile; }
                    if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 0; return true; }
                }
                return false;
            }
            else
            {
                RadioPlayer.Stop();
                return true;
            }
        }

        private static bool IsSuspended = false;
        /// <summary> Suspend media play for FilePath or CurrentFile if FilePath is null </summary>
        public static bool Suspend()
        {
            if (Mode == Modes.File)
            {
                if (CurrentFile != null)
                {
                    IsSuspended = true;
                    Stop();
                }
                return false;
            }
            else
            {
                RadioPlayer.Stop();
                return true;
            }
        }

        /// <summary> Test if FilePath played for FilePath or CurrentFile if FilePath is null </summary>
        public static bool IsPlaying(string FilePath = null)
        {
            if (Mode == Modes.File)
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
            else { return RadioPlayer.IsPlaying; }
        }

        /// <summary> Get/Set position for FilePath or CurrentFile if FilePath is null </summary>
        public static long Position(string FilePath = null, long position = -1)
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
        public static void PlayTimeAdvance(long seconds)
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
        public static void PlayTimeRewind(long seconds)
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
        public static long Length(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (!AudioList.ContainsKey(FilePath)) { return -1; }
                return (long)((AudioFileReader)AudioList[FilePath]).TotalTime.TotalMilliseconds;
            }
            return -1;
        }

    }

    public enum PlayerStatus { Play, Pause, Stop }
}
