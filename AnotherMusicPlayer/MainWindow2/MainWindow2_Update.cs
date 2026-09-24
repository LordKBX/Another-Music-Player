using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2Space
{
    public partial class MainWindow2 : Form
    {
        public static bool IsVersionString(string input)
        { return Regex.IsMatch(input, "^([0-9\\.]{1,})$"); }

        public static string NumEvenner(string input, int length = 3)
        {
            string ret = "" + input;
            while (ret.Length < length) { ret = "0" + ret; }
            return ret;
        }

        public static long AppNumberVersion(string version)
        {
            if (!IsVersionString(version)) { return 0; }
            string endVersion = "";
            string[] tab = version.Split('.');
            foreach (string block in tab) { endVersion += NumEvenner(block, 4); }

            return long.Parse(endVersion);
        }

        private static HttpClient getHttpCLient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Host", "api.github.com");
            httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:155.0) Gecko/20100101 Firefox/155.0");
            httpClient.Timeout = new TimeSpan(0, 0, 10);

            return httpClient;
        }

        private static string CheckAppVersionUrl = "https://api.github.com/repos/LordKBX/Another-Music-Player/releases";
        public static FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo("" + Assembly.GetEntryAssembly().Location);
        private bool CheckAppVersionInUse = false;

        private static (long, string) ParseGitHubManifest(string content)
        {
            string downloadUrl = "";
            long contentVersion = 0;
            List<Dictionary<string, object>> list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(content);
            if (list[0].ContainsKey("assets") == false)
            { return (0, ""); }
            if (list[0].ContainsKey("tag_name") == false)
            { return (0, ""); }

            if (list[0]["assets"].GetType() == typeof(Newtonsoft.Json.Linq.JArray))
            {
                foreach (JObject kvp in ((Newtonsoft.Json.Linq.JArray)list[0]["assets"]))
                {
                    downloadUrl = kvp.GetValue("browser_download_url").ToString();
                }
            }
            contentVersion = AppNumberVersion("" + list[0]["tag_name"]);

            return (contentVersion, downloadUrl);
        }

        public void CheckAppVersion(bool IsBackground = false)
        {
            if (versionInfo == null) { return; }
            if (CheckAppVersionInUse) { return; }
            CheckAppVersionInUse = true;
            try
            {
                long currentVersion = AppNumberVersion("0" + versionInfo.FileVersion);
                HttpClient httpClient = getHttpCLient();
                Task<HttpResponseMessage> task = httpClient.GetAsync(CheckAppVersionUrl);
                task.Wait();
                if (task.Result.IsSuccessStatusCode)
                {
                    string downloadUrl = "";
                    string content = task.Result.Content.ReadAsStringAsync().Result;
                    long contentVersion = 0;

                    (contentVersion, downloadUrl) = ParseGitHubManifest(content);
                    if (contentVersion == 0)
                    { if (IsBackground == false) MessageBox.Show("Impossible de tester la disponibilité d'une mise à jour", "Error - " + versionInfo.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); CheckAppVersionInUse = false; return; }

                    Debug.WriteLine("currentVersionInt = " + currentVersion);
                    Debug.WriteLine("contentVersionInt = " + contentVersion);
                    Debug.WriteLine("downloadUrl = " + downloadUrl);

                    if (contentVersion > currentVersion) { ShowUpdateButton(true); CheckAppVersionInUse = false; return; }
                    else { if (IsBackground == false) MessageBox.Show("L'application est à jour", "Info - " + versionInfo.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1); }
                }
                else
                {
                    if (IsBackground == false) MessageBox.Show("Impossible de tester la disponibilité d'une mise à jour", "Error - " + versionInfo.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            CheckAppVersionInUse = false;
            ShowUpdateButton(false);
        }

        private void DownloadLatestVersion()
        {
            if (!DialogBox.ShowDialog("Alert",
                "Do you confirm downloading newest installer and running it ?",
                DialogBoxButtons.YesNo, DialogBoxIcons.Warning, this))
            { return; }
            HttpClient httpClient = getHttpCLient();
            Task<HttpResponseMessage> task = httpClient.GetAsync(CheckAppVersionUrl);
            task.Wait();
            if (task.Result.IsSuccessStatusCode)
            {
                string downloadUrl = "";
                string content = task.Result.Content.ReadAsStringAsync().Result;
                long contentVersion = 0;

                (contentVersion, downloadUrl) = ParseGitHubManifest(content);
                if (contentVersion == 0)
                { MessageBox.Show("Impossible de récupérer la mise à jour", "Error - " + versionInfo.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); return; }

                string fileName = "" + versionInfo.ProductName + "-" + contentVersion + ".exe";
                string endFile = Path.GetTempPath() + fileName;
                FileStreamOptions streamOption = new FileStreamOptions() { Access = FileAccess.Write, Mode = FileMode.OpenOrCreate, Share = FileShare.ReadWrite };
                FileStream endstream = new FileStream(endFile, streamOption);
                endstream.Position = 0;
                endstream.SetLength(0);

                HttpClient httpClient2 = getHttpCLient();
                httpClient2.Timeout = new TimeSpan(0, 1, 0);
                Task<Stream> downStream = httpClient2.GetStreamAsync(downloadUrl);
                downStream.Wait(httpClient2.Timeout);
                if (downStream.IsCompleted)
                {
                    byte[] buff = new byte[1024];
                    int bytes = -1;
                    do
                    {
                        // Read the client's test message.
                        bytes = downStream.Result.Read(buff, 0, buff.Length);
                        if (bytes < buff.Length)
                        {
                            for (int i = bytes; i < buff.Length; i++) { buff[i] = 0; }
                        }
                        endstream.Write(buff, 0, bytes);
                    } while (bytes != 0);
                    endstream.Close();

                    Process proc = new Process();
                    proc.StartInfo.FileName = endFile;
                    proc.StartInfo.Arguments = "/VERYSILENT /SUPPRESSMSGBOXES /RESTARTAPPLICATIONS";
                    proc.Start();

                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("Impossible de télécharger la mise à jour", "Error - " + versionInfo.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e) { DownloadLatestVersion(); }
        private void ShowUpdateButton(bool visible)
        {
            if (this.InvokeRequired) { this.Invoke(() => { ShowUpdateButton(visible); }); return; }
            UpdateButton.Visible = visible;
            UpdateButton.BackgroundImageLayout = ImageLayout.Zoom;
            MainWIndowHead.ColumnStyles[2].Width = (visible) ? 62 : 0;
        }
    }
}
