using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinInkHelloWorld
{
    public class BitmapCanvas
    {
        private WriteableBitmap _bitmap;
        public WriteableBitmap Bitmap => _bitmap;
        public int Width { get; }
        public int Height { get; }

        public BitmapCanvas(int width, int height)
        {
            Width = width;
            Height = height;
            _bitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Bgra32, null);
            Clear();
        }

        public void Clear()
        {
            // Fill with white
            int stride = _bitmap.BackBufferStride;
            int bytes = stride * _bitmap.PixelHeight;
            byte[] pixels = new byte[bytes];
            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = 255;     // Blue
                pixels[i + 1] = 255; // Green
                pixels[i + 2] = 255; // Red
                pixels[i + 3] = 255; // Alpha
            }
            _bitmap.WritePixels(new Int32Rect(0, 0, Width, Height), pixels, stride, 0);
        }

        public unsafe void DrawLine(Point start, Point end, float pressure)
        {
            _bitmap.Lock();

            int x0 = (int)start.X;
            int y0 = (int)start.Y;
            int x1 = (int)end.X;
            int y1 = (int)end.Y;

            // Clamp coordinates
            x0 = Math.Clamp(x0, 0, Width - 1);
            y0 = Math.Clamp(y0, 0, Height - 1);
            x1 = Math.Clamp(x1, 0, Width - 1);
            y1 = Math.Clamp(y1, 0, Height - 1);

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            int thickness = (int)(pressure * 5) + 1; // 1 to 6 px thickness

            byte* pBackBuffer = (byte*)_bitmap.BackBuffer;
            int stride = _bitmap.BackBufferStride;

            while (true)
            {
                DrawBrush(pBackBuffer, stride, x0, y0, thickness);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, Width, Height));
            _bitmap.Unlock();
        }

        private unsafe void DrawBrush(byte* buffer, int stride, int x, int y, int radius) 
        {
             for (int i = -radius/2; i <= radius/2; i++)
             {
                 for (int j = -radius/2; j <= radius/2; j++)
                 {
                     int px = x + i;
                     int py = y + j;

                     if (px >= 0 && px < Width && py >= 0 && py < Height)
                     {
                        byte* pPixel = buffer + (py * stride) + (px * 4);
                        pPixel[0] = 0;   // Blue
                        pPixel[1] = 0;   // Green
                        pPixel[2] = 0;   // Red
                        pPixel[3] = 255; // Alpha
                     }
                 }
             }
        }
    }
}
