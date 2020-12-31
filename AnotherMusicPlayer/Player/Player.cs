using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace AnotherMusicPlayer
{
    /// <summary> Player class, user for retrieving media file info or playing music </summary>
    public partial class Player
    {
        /// <summary> List of audio threads by file path </summary>
        private Dictionary<string, Thread> ThreadList = null;
        /// <summary> List of audio objects by file path </summary>
        private Dictionary<string, object> AudioList = null;
        /// <summary> List of playing status by file path </summary>
        private Dictionary<string, int> PlayStatus = null;
        /// <summary> List of playing new position by file path </summary>
        private Dictionary<string, long> PlayNewPositions = null;
        /// <summary> Last played file </summary>
        private string CurrentFile = null;
        /// <summary> Status if repeat file playback active </summary>
        private bool PlayRepeat = false;
        /// <summary> Conversion quality for output MP3 file </summary>
        private Int32 ConvQualityBitrates = 128;

        /// <summary> reference object of the application window </summary>
        private MainWindow parent;

        /// <summary> list of equalizer bands </summary>
        private readonly EqualizerBand[] bands;
        /// <summary> Maximum negative gain on an equalizer band </summary>
        public readonly int MinimumGain = -20;
        /// <summary> Maximum gain on an equalizer band </summary>
        public readonly int MaximumGain = 20;

    /// <summary> Constructor </summary>
    public Player(MainWindow parent = null) {
            this.parent = parent;
            ThreadList = new Dictionary<string, Thread>();
            AudioList = new Dictionary<string, object>();
            PlayStatus = new Dictionary<string, int>();
            PlayNewPositions = new Dictionary<string, long>();

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

        /// <summary> update an equalizer band Gain value </summary>
        public void UpdateEqualize(int Band, float Gain) {
            try { bands[Band].Gain = Gain; } catch { }
            //Debug.WriteLine("Equalizer Band("+Band+") = Gain " + Gain);
        }

        /// <summary> Define conversion quality for output MP3 file </summary>
        public Int32 ConvQuality(Int32 newQuality = -1) { if (newQuality != -1)  { return ConvQualityBitrates = newQuality; } else { return ConvQualityBitrates; } }
        /// <summary> Define status if repeat file playback active </summary>
        public void Repeat(bool rep) { PlayRepeat = rep; }
        /// <summary> Get status if repeat file playback active </summary>
        public bool IsRepeat() { return PlayRepeat; }
        /// <summary> Give the List of native accepted file extentions </summary>
        public List<string> AcceptedExtentions() { return new List<string> { ".MP3",".mp3",".WMA",".wma",".FLAC",".flac" }; }

        private int ConvCount = 0;
        /// <summary> Public interface for file convertion </summary>
        public async Task<bool> Conv(string FileInput, string FileOutput = null, bool deleteOrigin = false)
        {
            ConvCount += 1;
            bool replace = false;
            if (FileOutput == null) {FileOutput = Path.ChangeExtension(FileInput, ".mp3"); deleteOrigin = true; }
            //Debug.WriteLine("Task_Start");
            //Debug.WriteLine(FileInput);
            //Debug.WriteLine(FileOutput);

            //Test if output file already exist
            if (System.IO.File.Exists(FileOutput)) { System.IO.File.Delete(FileOutput); }
            
            bool ret = await ConvExe(FileInput, FileOutput);
            if (ret == true && deleteOrigin == true) { System.IO.File.Delete(FileInput); }
            ConvCount -= 1;
            if (ConvCount == 0) { parent.UnsetLockScreen(); }
            //Debug.WriteLine("ret conv : " + ((ret) ? "True" : "False"));
            return true;
        }

        /// <summary> Private interface for file convertion usign ffmpeg birary </summary>
        private async Task<bool> ConvExe(string FileInput, string FileOutput)
        {
            string AppName = Application.Current.MainWindow.GetType().Assembly.GetName().Name;
            char sep = System.IO.Path.DirectorySeparatorChar;
            string convPath1 = "", convPath2 = "", convPath3 = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                convPath1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + sep + AppName + sep + "ffmpeg-win64-static.exe";
                convPath2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + sep + AppName + sep + "ffmpeg-win32-static.exe";
                convPath3 = AppDomain.CurrentDomain.BaseDirectory + sep + "Player" + sep + "ffmpeg.exe";
            }

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            if (System.IO.File.Exists(convPath1)) { startInfo.FileName = convPath1; }
            else if (System.IO.File.Exists(convPath2)) { startInfo.FileName = convPath2; }
            else if (System.IO.File.Exists(convPath3)) { startInfo.FileName = convPath3; }
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

        /// <summary> Recuperate Media MetaData(cover excluded) </summary>
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
                item.Path = FilePath;
                item.OriginPath = OriginPath;
                item.Selected = (Selected) ? MainWindow.PlayListSelectionChar : "";
                item.Duration = (long)0;
                item.Size = new System.IO.FileInfo(OriginPath ?? FilePath).Length;
                item.DurationS = "00:00";

                item.Performers = tags.Tag.JoinedPerformers;
                item.Composers = tags.Tag.JoinedComposers;
                item.Copyright = tags.Tag.Copyright;
                item.Disc = tags.Tag.Disc;
                item.DiscCount = tags.Tag.DiscCount;
                item.AlbumArtists = tags.Tag.JoinedAlbumArtists;
                item.Genres = tags.Tag.FirstGenre;
                item.Lyrics = tags.Tag.Lyrics;
                item.Track = tags.Tag.Track;
                item.TrackCount = tags.Tag.TrackCount;
                item.Year = tags.Tag.Year;

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

        Dictionary<string, Dictionary<string, BitmapImage>> MediaPictureStoredCovers = new Dictionary<string, Dictionary<string, BitmapImage>>();
        public void MediaPictureClearCache() {
            foreach (KeyValuePair<string, Dictionary<string, BitmapImage>> pict in MediaPictureStoredCovers) {
                //foreach (KeyValuePair<string, BitmapImage> bip in pict.Value) { bip.Value = null; }
                pict.Value.Clear();
            }
            MediaPictureStoredCovers.Clear(); 
        }
        /// <summary> Recuperate Media Cover </summary>
        public BitmapImage MediaPicture(string FilePath, bool export = true, int MawWidth = 400, int MawHeight = 400, bool save = true)
        {
            BitmapImage bitmap = null;
            try
            {
                if (System.IO.File.Exists(FilePath))
                {
                    TagLib.File tags = TagLib.File.Create(FilePath);

                    if (tags.Tag.Pictures.Length > 0)
                    {
                        TagLib.IPicture pic = tags.Tag.Pictures[0];
                        string hash = System.Text.Encoding.Default.GetString(MD5.Create().ComputeHash(pic.Data.Data));
                        string key = "" + MawWidth + "x" + MawHeight;
                        if (MediaPictureStoredCovers.ContainsKey(hash)) {
                            if (MediaPictureStoredCovers[hash].ContainsKey(key)) { return MediaPictureStoredCovers[hash][key]; }
                        }

                        //Debug.WriteLine("Picture size = " + pic.Data.Data.Length);
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        ms.Seek(0, SeekOrigin.Begin);
                        // ImageSource for System.Windows.Controls.Image
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        int max = 400;

                        if (bitmap.PixelWidth > max || bitmap.PixelHeight > max)
                        {
                            double width = bitmap.PixelWidth, height = bitmap.PixelHeight;
                            if (width > max) {height = (height / width) * max; width = max; }
                            if (height > max) { width = (width / height) * max; height = max; }

                            ms.Seek(0, SeekOrigin.Begin); System.Drawing.Image im = System.Drawing.Image.FromStream(ms); ms.Close();
                            System.Drawing.Bitmap im2 = ResizeImage(im, Convert.ToInt32(width), Convert.ToInt32(height));
                            bitmap = null; bitmap = ConvertBitmapToBitmapImage(im2);

                            MemoryStream ms2 = new MemoryStream(); im2.Save(ms2, ImageFormat.Jpeg); ms2.Seek(0, SeekOrigin.Begin);

                            if (save)
                            {
                                TagLib.Picture pic2 = new TagLib.Picture();
                                pic2.Type = TagLib.PictureType.FrontCover; pic2.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg; pic2.Description = "Cover";
                                pic2.Data = TagLib.ByteVector.FromStream(ms2); tags.Tag.Pictures = new TagLib.IPicture[1] { pic2 };
                                try { tags.Save(); } catch { } //error if file already opened
                            }
                            ms2.Close();
                        }
                        bitmap.Freeze();
                        if (MediaPictureStoredCovers.ContainsKey(hash)) { MediaPictureStoredCovers[hash].Add(key, bitmap); }
                        else { MediaPictureStoredCovers.Add(hash, new Dictionary<string, BitmapImage>() { { key, bitmap } }); }
                    }
                    tags.Dispose();
                }
            }
            catch { }
            return (export)?bitmap:null;
        }


        private void MediaPictureInvoked(object FilePath = null) { MediaPicture((string)FilePath, false); }
        public void MediaPictureAsync(string FilePath) {
            Thread objThread = new Thread(new ParameterizedThreadStart(MediaPictureInvoked));
            objThread.IsBackground = true;
            objThread.Priority = ThreadPriority.AboveNormal;
            objThread.Start(FilePath);
        }

        /// <summary>
        /// Takes a bitmap and converts it to an image that can be handled by WPF ImageBrush
        /// </summary>
        /// <param name="src">A bitmap image</param>
        /// <returns>The image as a BitmapImage for WPF</returns>
        private BitmapImage ConvertBitmapToBitmapImage(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary> Stop all currently playing threads </summary>
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

        /// <summary> Test if file exist, if input = null remplace it with value in CurrentFile </summary>
        private bool TestFile(string FilePath = null) {
            if (FilePath == null) { if (CurrentFile == null) { return false; }; FilePath = CurrentFile; }
            if (System.IO.File.Exists(FilePath)) { return true; }
            else { return false; }
        }

        /// <summary> Open a new media playing thread </summary>
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

        /// <summary> Start media play for FilePath or CurrentFile if FilePath is null </summary>
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
        /// <summary> Resume media play for FilePath or CurrentFile if FilePath is null </summary>
        public bool Resume(string FilePath = null) { return Play(FilePath); }

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
            if (TestFile(FilePath))
            {
                if (FilePath == null) { FilePath = CurrentFile; }
                if (!PlayStatus.ContainsKey(FilePath)) { return false; }
                if (PlayStatus[FilePath] == 1) { return true; }
            }
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
                else { 
                    if (FilePath.EndsWith(".flac")) { return (long)((FlacReader)AudioList[FilePath]).CurrentTime.TotalMilliseconds; }
                    else { return (long)((AudioFileReader)AudioList[FilePath]).CurrentTime.TotalMilliseconds ; }
                }
            }
            return -1;
        }

        /// <summary> Get media length of FilePath or CurrentFile if FilePath is null </summary>
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

        /// <summary> Playing thread </summary>
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

                /***/ Equalizer equalizer = new Equalizer((ISampleProvider)audioFile, bands);

                using (var outputDevice = new WaveOutEvent())
                {
                    //outputDevice.Init((IWaveProvider)audioFile);
                    outputDevice.Init(equalizer);
                    AudioList.Add(FilePath, audioFile);
                    int ret = -1; long ret2 = -1;

                    //while (outputDevice.PlaybackState == PlaybackState.Playing)
                    while (true)
                    {
                        equalizer.Update();
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

                        Thread.Sleep(100);
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
