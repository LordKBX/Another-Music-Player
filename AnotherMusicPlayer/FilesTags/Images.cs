using System;
using System.IO;
using NAudio;
using NAudio.Wave;
using TagLib;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;

namespace AnotherMusicPlayer
{
    public partial class FilesTags
    {
        /// <summary> Recuperate Media Cover </summary>
        public static BitmapImage MediaPicture(string FilePath, Database bdd = null, bool export = true, int MaxWidth = 400, int MaxHeight = 400, bool save = true)
        {
            if (!System.IO.File.Exists(FilePath)) { return null; }
            BitmapImage bitmap = null;
            string FilePathExt = FilePath + "|" + MaxWidth + "x" + MaxHeight;
            try
            {
                string data = null;
                if (bdd != null) { data = bdd.DatabaseGetCover(FilePathExt); }
                if (data != null)
                {
                    bitmap = BitmapMagic.Base64StringToBitmap(data);
                }
                else
                {
                    TagLib.File tags = TagLib.File.Create(FilePath);

                    if (tags.Tag.Pictures.Length > 0)
                    {
                        TagLib.IPicture pic = tags.Tag.Pictures[0];

                        //Debug.WriteLine("Picture size = " + pic.Data.Data.Length);
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        ms.Seek(0, SeekOrigin.Begin);
                        // ImageSource for System.Windows.Controls.Image
                        bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();

                        if ((bitmap.PixelWidth > MaxWidth || bitmap.PixelHeight > MaxHeight) && MaxWidth > 0 && MaxHeight > 0)
                        {
                            double width = bitmap.PixelWidth, height = bitmap.PixelHeight;
                            if (width > MaxWidth) { height = (height / width) * MaxWidth; width = MaxWidth; }
                            if (height > MaxHeight) { width = (width / height) * MaxHeight; height = MaxHeight; }

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

                        if (bdd != null)
                        {
                            bdd.DatabaseSaveCover(
                            FilePathExt,
                            BitmapMagic.BitmapToBase64String(
                                BitmapMagic.BitmapImage2Bitmap(bitmap),
                                System.Drawing.Imaging.ImageFormat.Jpeg
                                )
                            );
                        }
                    }
                    else
                    {
                        char SeparatorChar = System.IO.Path.DirectorySeparatorChar;
                        string folder = FilePath.Substring(0, FilePath.LastIndexOf(SeparatorChar));
                        bitmap = (System.IO.File.Exists(folder + SeparatorChar + "Cover.jpg")) ? new BitmapImage(new Uri(folder + SeparatorChar + "Cover.jpg"))
                            : ((System.IO.File.Exists(folder + SeparatorChar + "Cover.png")) ? new BitmapImage(new Uri(folder + SeparatorChar + "Cover.png")) : MainWindow.Bimage("CoverImg"));
                    }
                    tags.Dispose();
                }
            }
            catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); }
            return (export) ? bitmap : null;
        }

        /// <summary>
        /// Takes a bitmap and converts it to an image that can be handled by WPF ImageBrush
        /// </summary>
        /// <param name="src">A bitmap image</param>
        /// <returns>The image as a BitmapImage for WPF</returns>
        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap src)
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

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

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

    }
}
