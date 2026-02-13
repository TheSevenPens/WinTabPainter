using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace SevenLib.Media
{
    public class CanvasRenderer
    {
        private WriteableBitmap _bitmap;
        public ImageSource ImageSource => _bitmap;

        public int Width { get; }
        public int Height { get; }

        public CanvasRenderer(int width, int height)
        {
            Width = width;
            Height = height;

            _bitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgra32, null);
            Clear();
        }

        private void DrawOnCanvas(Action<SKCanvas> drawAction)
        {
            _bitmap.Lock();
            try
            {
                var info = new SKImageInfo(Width, Height, SKColorType.Bgra8888, SKAlphaType.Premul);
                using (var surface = SKSurface.Create(info, _bitmap.BackBuffer, _bitmap.BackBufferStride))
                {
                    drawAction(surface.Canvas);
                }
                _bitmap.AddDirtyRect(new Int32Rect(0, 0, Width, Height));
            }
            finally
            {
                _bitmap.Unlock();
            }
        }

        public void Clear()
        {
            DrawOnCanvas(canvas => canvas.Clear(SKColors.White));
        }

        public void DrawPoint(int x, int y, uint pressure)
        {
            DrawOnCanvas(canvas =>
            {
                using (var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    Style = SKPaintStyle.Fill
                })
                {
                    canvas.DrawRect(x - 1, y - 1, 3, 3, paint);
                }
            });
        }

        public void DrawRectangle(int x, int y, int width, int height, int color)
        {
             DrawOnCanvas(canvas =>
            {
                var skColor = new SKColor((uint)color); // Assuming ARGB int
                // Fix alpha if needed, or assume caller provides logic. 
                // WinTabHelloWorld passed unchecked((int)0xFF000000) for black.
                
                using (var paint = new SKPaint
                {
                    Color = skColor,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 3
                })
                {
                    canvas.DrawRect(x, y, width, height, paint);
                }
            });
        }

        public void DrawLine(int x0, int y0, int x1, int y1, int color)
        {
             DrawOnCanvas(canvas =>
            {
                var skColor = new SKColor((uint)color);
                using (var paint = new SKPaint
                {
                    Color = skColor,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 1,
                    IsAntialias = false // Match original pixelated look? Or true for better quality.
                })
                {
                    canvas.DrawLine(x0, y0, x1, y1, paint);
                }
            });
        }

        public void DrawLineX(Point start, Point end, float thickness)
        {
             DrawOnCanvas(canvas =>
            {
                using (var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = thickness,
                    StrokeCap = SKStrokeCap.Round,
                    IsAntialias = true
                })
                {
                    canvas.DrawLine((float)start.X, (float)start.Y, (float)end.X, (float)end.Y, paint);
                }
            });
        }
    }
}
