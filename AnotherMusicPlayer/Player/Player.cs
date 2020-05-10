using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio;
using NAudio.Flac;
using NAudio.Wave;
using TagLib;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;

namespace AnotherMusicPlayer
{

    public partial class Player
    {
        private Dictionary<string, Thread> ThreadList = null;
        private Dictionary<string, object> AudioList = null;
        private Dictionary<string, int> PlayStatus = null;
        private Dictionary<string, long> PlayNewPositions = null;
        private string CurrentFile = null;
        private bool PlayRepeat = false;
        private Int32 ConvQualityBitrates = 128;

        public Player() {
            ThreadList = new Dictionary<string, Thread>();
            AudioList = new Dictionary<string, object>();
            PlayStatus = new Dictionary<string, int>();
            PlayNewPositions = new Dictionary<string, long>();

            var name = "PATH";
            var scope = EnvironmentVariableTarget.Process; // or User
            var oldValue = Environment.GetEnvironmentVariable(name, scope);
            var newValue = oldValue + @";" + AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar;
            Environment.SetEnvironmentVariable(name, newValue, scope);
        }

        public Int32 ConvQuality(Int32 newQuality = -1) { if (newQuality != -1)  { return ConvQualityBitrates = newQuality; } else { return ConvQualityBitrates; } }
        public void Repeat(bool rep) { PlayRepeat = rep; }
        public bool IsRepeat() { return PlayRepeat; }
        public List<string> AcceptedExtentions() { return new List<string> { ".MP3",".mp3",".WMA",".wma",".FLAC",".flac" }; }

        public async Task<bool> Conv(string FileInput, string FileOutput = null, bool deleteOrigin = false)
        {
            bool replace = false;
            if (FileOutput == null)
            {
                FileOutput = Path.ChangeExtension(FileInput, ".mp3");
                replace = true;
            }
            Debug.WriteLine("Task_Start");
            Debug.WriteLine(FileInput);
            Debug.WriteLine(FileOutput);

            if (System.IO.File.Exists(FileOutput)) { System.IO.File.Delete(FileOutput); }
            Debug.WriteLine("Test File");

            bool ret = await ConvExe(FileInput, FileOutput);
            if (ret == true && deleteOrigin == true) { System.IO.File.Delete(FileInput); }
            Debug.WriteLine("ret conv : " + ((ret) ? "True" : "False"));

            if (replace) { System.IO.File.Delete(FileInput); }

            return true;
        }

        private async Task<bool> ConvExe(string FileInput, string FileOutput)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "Player" + System.IO.Path.DirectorySeparatorChar + "ffmpeg" 
                + ((RuntimeInformation.IsOSPlatform(OSPlatform.Windows))?".exe":"");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-i \"" + FileInput + "\" -acodec mp3 -b:a " + ConvQualityBitrates + "k -map_metadata 0:s:0 \"" + FileOutput + "\"";

            Debug.WriteLine(startInfo.FileName);
            Debug.WriteLine(startInfo.Arguments);
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(JsonConvert.SerializeObject(e));
            }
            return false;
        }

        public PlayListViewItem MediaInfo(string FilePath, bool Selected, string OriginPath = null) {
            if (System.IO.File.Exists(FilePath) || System.IO.File.Exists(OriginPath))
            {
                //Debug.WriteLine("MetaData Source: " + (OriginPath ?? FilePath));
                TagLib.File tags;
                if (System.IO.File.Exists(FilePath)) { tags = TagLib.File.Create(FilePath); }
                else { tags = TagLib.File.Create(OriginPath); }
                PlayListViewItem item = new PlayListViewItem();
                item.Name = tags.Tag.Title;
                item.Album = tags.Tag.Album;
                item.Artist = string.Join(", ", tags.Tag.Performers);
                item.Path = FilePath;
                item.OriginPath = OriginPath;
                item.Selected = (Selected) ? MainWindow.PlayListSelectionChar : "";
                item.Duration = (long)0;
                item.Size = new System.IO.FileInfo(OriginPath ?? FilePath).Length;
                item.DurationS = "00:00";

                tags.Dispose();

                try
                {
                    if (System.IO.File.Exists(FilePath))
                    {
                        foreach (string ext in AcceptedExtentions())
                        {
                            if (FilePath.EndsWith(ext))
                            {
                                if (FilePath.EndsWith(".flac"))
                                {
                                    FlacReader fir = new FlacReader(FilePath);
                                    item.Duration = (long)fir.TotalTime.TotalMilliseconds;
                                    fir.Dispose();
                                }
                                else
                                {
                                    AudioFileReader fir = new AudioFileReader(FilePath);
                                    item.Duration = (long)fir.TotalTime.TotalMilliseconds;
                                    fir.Dispose();
                                }
                                item.DurationS = MainWindow.displayTime(item.Duration);
                            }
                        }
                    }
                }
                catch { }
                return item;
            }
            return null;
        }

        public BitmapImage MediaPicture(string FilePath) {
            if (System.IO.File.Exists(FilePath))
            {
                TagLib.File tags = TagLib.File.Create(FilePath);

                if (tags.Tag.Pictures.Length > 0)
                {
                    TagLib.IPicture pic = tags.Tag.Pictures[0];
                    MemoryStream ms = new MemoryStream(pic.Data.Data);
                    ms.Seek(0, SeekOrigin.Begin);
                    // ImageSource for System.Windows.Controls.Image
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    return bitmap;
                }

                tags.Dispose();
            }
            return null;
        }

        public void StopAll() {
            if (ThreadList.Count > 0)
            {
                foreach (KeyValuePair<string, Thread> entry in ThreadList) {
                    PlayStatus[entry.Key] = 2;
                }
                foreach (KeyValuePair<string, object> entry in AudioList)
                {
                    try
                    {
                        if (entry.Key.EndsWith(".flac")) { ((FlacReader)entry.Value).Dispose(); } else { ((AudioFileReader)entry.Value).Dispose(); }
                    }
                    catch (Exception) { }
                }

                ThreadList.Clear();
                //PlayStatus.Clear();
                PlayNewPositions.Clear();
                AudioList.Clear();

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

        private bool TestFile(string FilePath = null) {
            if (FilePath == null) { if (CurrentFile == null) { return false; }; FilePath = CurrentFile; }
            if (System.IO.File.Exists(FilePath)) { return true; }
            else { return false; }
        }

        public bool Open(string FilePath, bool AutoPlay = false) {
            if (TestFile(FilePath)) {
                try
                {
                    Thread objThread = new Thread(new ParameterizedThreadStart(PlaySoundAsync));
                    objThread.IsBackground = true;
                    objThread.Priority = ThreadPriority.AboveNormal;
                    objThread.Start(FilePath);
                    ThreadList.Add(FilePath, objThread);
                    PlayStatus.Add(FilePath, (AutoPlay)?1:0);
                    PlayNewPositions.Add(FilePath, -1);
                    CurrentFile = FilePath;
                    return true;
                }
                catch { }
            }
            return false;
        }

        public bool Play(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 1; return true; }
                else { return Open(FilePath, true); }
            }
            return false;
        }
        public bool Resume(string FilePath = null) { return Play(FilePath); }

        public bool Pause(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 0; return true; }
            }
            return false;
        }

        public bool Stop(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (PlayStatus.ContainsKey(FilePath)) { PlayStatus[FilePath] = 2; return true; }
            }
            return false;
        }

        public bool IsPlaying(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (!PlayStatus.ContainsKey(FilePath)) { return false; }
                if (PlayStatus[FilePath] == 1) { return true; }
            }
            return false;
        }

        public long Position(string FilePath = null, long position = -1)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (!PlayStatus.ContainsKey(FilePath)) { return -1; }
                if (position != -1) { PlayNewPositions[FilePath] = position; return position; }
                else { 
                    if (FilePath.EndsWith(".flac")) { return (long)((FlacReader)AudioList[FilePath]).CurrentTime.TotalMilliseconds; }
                    else { return (long)((AudioFileReader)AudioList[FilePath]).CurrentTime.TotalMilliseconds ; }
                }
            }
            return -1;
        }
        public long Length(string FilePath = null)
        {
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (!AudioList.ContainsKey(FilePath)) { return -1; }
                if (FilePath.EndsWith(".flac")) { return (long)((FlacReader)AudioList[FilePath]).TotalTime.TotalMilliseconds; }
                else { return (long)((AudioFileReader)AudioList[FilePath]).TotalTime.TotalMilliseconds; }
            }
            return -1;
        }

        private void PlaySoundAsync(object file)
        {
            try
            {
                string FilePath = (string)file;
                object audioFile = null;
                bool started = false;
                bool IsFlac = false;

                if (FilePath.EndsWith(".flac")) { audioFile = new FlacReader(FilePath); IsFlac = true; }
                else { audioFile = new AudioFileReader(FilePath); }

                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init((IWaveProvider)audioFile);
                    AudioList.Add(FilePath, audioFile);
                    int ret = -1; long ret2 = -1;

                    //while (outputDevice.PlaybackState == PlaybackState.Playing)
                    while (true)
                    {
                        ret2 = ret = -1;
                        PlayStatus.TryGetValue(FilePath, out ret);
                        PlayNewPositions.TryGetValue(FilePath, out ret2);

                        if (ret == 0 && outputDevice.PlaybackState == PlaybackState.Playing) { outputDevice.Pause(); }
                        if (ret == 1 && outputDevice.PlaybackState != PlaybackState.Playing)
                        {
                            outputDevice.Play(); CurrentFile = FilePath;
                            MediaLengthChangedEventParams evtp = new MediaLengthChangedEventParams();
                            if (IsFlac) { evtp.duration = (long)( ((FlacReader)audioFile).TotalTime.TotalMilliseconds ); }
                            else { evtp.duration = (long)(((AudioFileReader)audioFile).TotalTime.TotalMilliseconds); }
                            LengthChanged(this, evtp);
                            started = true;
                        }
                        if (ret == 2) { outputDevice.Stop(); break; }
                        if (ret2 != -1) { 
                            if (IsFlac)
                            {
                                FlacReader af = ((FlacReader)audioFile);
                                long msval = af.WaveFormat.AverageBytesPerSecond / 1000;
                                ((FlacReader)audioFile).Position = ret2 * msval;
                            }
                            else
                            {
                                AudioFileReader af = ((AudioFileReader)audioFile);
                                long msval = af.WaveFormat.AverageBytesPerSecond / 1000;
                                ((AudioFileReader)audioFile).Position = ret2 * msval;
                            }
                            PlayNewPositions[FilePath] = -1;
                        }
                        try
                        {
                            MediaPositionChangedEventParams evt = new MediaPositionChangedEventParams();
                            if (IsFlac) {
                                FlacReader a = (FlacReader)audioFile;
                                evt.Position = (long)( a.CurrentTime.TotalMilliseconds ); 
                                evt.duration = (long)( a.TotalTime.TotalMilliseconds ); 
                            }
                            else
                            {
                                AudioFileReader a = (AudioFileReader)audioFile;
                                evt.Position = (long)( a.CurrentTime.TotalMilliseconds );
                                evt.duration = (long)( a.TotalTime.TotalMilliseconds );
                            }
                            PositionChanged(this, evt);
                            if (outputDevice.PlaybackState == PlaybackState.Stopped && started == true && evt.Position > 0)
                            {
                                if (PlayRepeat)
                                {
                                    if (IsFlac) { ((FlacReader)audioFile).Position = 0; }
                                    else { ((AudioFileReader)audioFile).Position = 0; }
                                    PlayStatus[FilePath] = 1;
                                    outputDevice.Play();
                                }
                                else
                                {
                                    PlayStoped(this, evt);
                                    break;
                                }
                            }
                        }
                        catch { break; }

                        Thread.Sleep(50);
                    }
                    try { outputDevice.Stop(); } catch { }
                    outputDevice.Dispose();
                }

                if (IsFlac) { ((FlacReader)audioFile).Close(); ((FlacReader)audioFile).Dispose(); }
                else { ((AudioFileReader)audioFile).Close(); ((AudioFileReader)audioFile).Dispose(); }

                PlayStatus.Remove(FilePath);
                PlayNewPositions.Remove(FilePath);
                if (IsFlac) { ((FlacReader)AudioList[FilePath]).Dispose(); }
                else { ((AudioFileReader)AudioList[FilePath]).Dispose(); }
                AudioList.Remove(FilePath);
                ThreadList.Remove(FilePath);
            }
            catch { }
            return;
        }
    }
}
