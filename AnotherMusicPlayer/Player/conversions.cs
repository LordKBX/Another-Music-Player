using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AnotherMusicPlayer
{
    public partial class Player
    {
        /// <summary> List of availlable conversion quality for output MP3 file </summary>
        public readonly Int32[] ConvQualityList = new Int32[] { 96, 128, 192, 256, 320 };
        /// <summary> Conversion quality for output MP3 file </summary>
        private Int32 ConvQualityBitrates = 128;

        /// <summary> Define conversion quality for output MP3 file </summary>
        public Int32 ConvQuality(Int32 newQuality = -1) { List<Int32> lc = new List<Int32>(ConvQualityList); if (lc.Contains(newQuality)) { return ConvQualityBitrates = newQuality; } else { return ConvQualityBitrates; } }

        private int ConvCount = 0;
        /// <summary> Public interface for file convertion </summary>
        public async Task<bool> Conv(string FileInput, string FileOutput = null, bool deleteOrigin = false)
        {
            ConvCount += 1;
            bool replace = false;
            if (FileOutput == null) { FileOutput = Path.ChangeExtension(FileInput, ".mp3"); deleteOrigin = true; }
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

            Debug.WriteLine("--> ConvExe");
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
            catch (Exception e)
            {
                Debug.WriteLine("--> ConvExe ERROR : " + JsonConvert.SerializeObject(e));
            }
            return false;
        }
    }
}
