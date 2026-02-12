using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace SevenPaint.Paint
{
    public class PixelCanvas
    {
        private WriteableBitmap _wbmp;
        private int _width;
        private int _height;

        public ImageSource Source => _wbmp;

        public PixelCanvas(int width, int height, double dpi)
        {
            _width = width;
            _height = height;
            _wbmp = new WriteableBitmap(width, height, dpi, dpi, PixelFormats.Pbgra32, null);
        }

        public void Clear(System.Windows.Media.Color color)
        {
            _wbmp.Lock();
            try
            {
                var info = new SKImageInfo(_width, _height, SKColorType.Bgra8888, SKAlphaType.Premul);
                using (var surface = SKSurface.Create(info, _wbmp.BackBuffer, _wbmp.BackBufferStride))
                {
                    surface.Canvas.Clear(ToSKColor(color));
                }
                _wbmp.AddDirtyRect(new Int32Rect(0, 0, _width, _height));
            }
            finally
            {
                _wbmp.Unlock();
            }
        }

        public void DrawDab(double x, double y, double radius, System.Windows.Media.Color color)
        {
            _wbmp.Lock();
            try
            {
                var info = new SKImageInfo(_width, _height, SKColorType.Bgra8888, SKAlphaType.Premul);
                using (var surface = SKSurface.Create(info, _wbmp.BackBuffer, _wbmp.BackBufferStride))
                {
                    using (var paint = new SKPaint())
                    {
                        paint.Color = ToSKColor(color);
                        paint.IsAntialias = true;
                        paint.Style = SKPaintStyle.Fill;
                        surface.Canvas.DrawCircle((float)x, (float)y, (float)radius, paint);
                    }
                }

                // Optimization: Calculate dirty rect instead of full update
                int r = (int)Math.Ceiling(radius + 2);
                int minX = (int)(x - r);
                int minY = (int)(y - r);
                int w = r * 2;
                int h = r * 2;

                // Clamp
                if (minX < 0) minX = 0;
                if (minY < 0) minY = 0;
                if (minX + w > _width) w = _width - minX;
                if (minY + h > _height) h = _height - minY;

                if (w > 0 && h > 0)
                {
                    _wbmp.AddDirtyRect(new Int32Rect(minX, minY, w, h));
                }
            }
            finally
            {
                _wbmp.Unlock();
            }
        }

        private SKColor ToSKColor(System.Windows.Media.Color color)
        {
            return new SKColor(color.R, color.G, color.B, color.A);
        }
    }
}
