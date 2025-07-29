using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace AnotherMusicPlayer
{
    public static class BitmapMagic
    {
        public static string BitmapToBase64String(Bitmap bmp, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            string base64String = string.Empty;

            MemoryStream memoryStream = new MemoryStream();
            bmp.Save(memoryStream, imageFormat);

            memoryStream.Position = 0;
            byte[] byteBuffer = memoryStream.ToArray();

            memoryStream.Close();

            base64String = Convert.ToBase64String(byteBuffer);
            byteBuffer = null;

            return base64String;
        }

        public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static BitmapImage BitmapToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static BitmapImage Base64StringToBitmap(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.DownloadCompleted += BitmapImage_DownloadCompleted;
            bitmapImage.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public static Bitmap Base64StringToTrueBitmap(string base64String)
        {
            try
            {
                byte[] byteBuffer = Convert.FromBase64String(base64String);
                MemoryStream memoryStream = new MemoryStream(byteBuffer);
                memoryStream.Position = 0;

                Bitmap btm = new Bitmap(memoryStream);

                return btm;
            }
            catch (Exception) { return null; }
        }

        private static void BitmapImage_DownloadCompleted(object sender, EventArgs e)
        {
            ((BitmapImage)sender).Freeze();
            ((BitmapImage)sender).StreamSource.Close();
        }
    }
}