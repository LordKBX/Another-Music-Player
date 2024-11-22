using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Color = System.Windows.Media.Color;
using Brush = System.Windows.Media.Brush;
using Pen = System.Windows.Media.Pen;
using System.Windows.Forms;

namespace AnotherMusicPlayer.MainWindow2Space
{
    internal static class Icons
    {
        private static Color DefaultColor = Colors.White;
        private static Brush DefaultBrush = new SolidColorBrush(DefaultColor);

        public static Bitmap FromDataString(string source, int TargetSize = 32, Brush brush = null) 
        {
            try
            {
                if (source == null) { return null; }
                if (brush == null) { brush = DefaultBrush; }
                System.Windows.Media.Pen pen = new System.Windows.Media.Pen(brush, 0);

                GeometryConverter tp0 = new GeometryConverter();
                object? ret = tp0.ConvertFromString(null, null, source);

                Geometry geometry = ((StreamGeometry)ret).GetFlattenedPathGeometry();
                var rect = geometry.GetRenderBounds(pen);

                var bigger = rect.Width > rect.Height ? rect.Width : rect.Height;
                var scale = TargetSize / bigger;

                Geometry scaledGeometry = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, new ScaleTransform(scale, scale));
                rect = scaledGeometry.GetRenderBounds(pen);

                Geometry transformedGeometry = Geometry.Combine(scaledGeometry, scaledGeometry, GeometryCombineMode.Intersect, new TranslateTransform(((TargetSize - rect.Width) / 2) - rect.Left, ((TargetSize - rect.Height) / 2) - rect.Top));

                RenderTargetBitmap bmp = new RenderTargetBitmap(TargetSize, TargetSize, 96, 96, PixelFormats.Pbgra32);

                DrawingVisual viz = new DrawingVisual();
                using (DrawingContext dc = viz.RenderOpen()) { dc.DrawGeometry(brush, null, transformedGeometry); }

                bmp.Render(viz);

                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(bmp));

                using (MemoryStream file = new MemoryStream()) { pngEncoder.Save(file); return new Bitmap(file); }
            }
            catch (Exception) {  }

            return null;
        }

        public static Bitmap FromIconKind(IconKind source, int TargetSize = 32, Brush brush = null) 
        {
            return FromDataString(IconsData.Data[source], TargetSize, brush);
        }

    }
}
