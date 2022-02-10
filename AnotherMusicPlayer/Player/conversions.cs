using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
            //bool replace = false;
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

        public string GetFfmpegPath()
        {
            string endPath = null;
            foreach (string path in FfmpegPaths) { if (System.IO.File.Exists(path)) { endPath = path; break; } }
            return endPath;
        }

        /// <summary> Private interface for file convertion usign ffmpeg birary </summary>
        private async Task<bool> ConvExe(string FileInput, string FileOutput, Int32 quality = 0)
        {
            if (quality <= 0) { quality = ConvQualityBitrates; }
            string AppName = parent.AppName;
            char sep = System.IO.Path.DirectorySeparatorChar;
            string exePath = GetFfmpegPath();
            if (exePath == null) { return false; }

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = exePath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-i \"" + FileInput + "\" -acodec mp3 -b:a " + quality + "k -map_metadata 0:s:0 \"" + FileOutput + "\"";

            StringBuilder standardOutput = new StringBuilder();
            startInfo.RedirectStandardError = true;

            Debug.WriteLine("--> ConvExe");
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

                    Debug.WriteLine("Output => " + standardOutput.ToString());
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
