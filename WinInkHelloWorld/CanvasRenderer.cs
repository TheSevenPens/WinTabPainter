using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinInkHelloWorld
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

        public void Clear()
        {
            _bitmap.Lock();
            try
            {
                IntPtr backBuffer = _bitmap.BackBuffer;
                unsafe
                {
                    int* pBackBuffer = (int*)backBuffer;
                    for (int i = 0; i < Width * Height; i++)
                    {
                        *pBackBuffer++ = unchecked((int)0xFFFFFFFF); // White
                    }
                }
                _bitmap.AddDirtyRect(new Int32Rect(0, 0, Width, Height));
            }
            finally
            {
                _bitmap.Unlock();
            }
        }

        public void DrawLine(Point start, Point end, float pressure)
        {
            _bitmap.Lock();
            try
            {
                unsafe
                {
                    int w = Width;
                    int h = Height;
                    byte* pBackBuffer = (byte*)_bitmap.BackBuffer;
                    int stride = _bitmap.BackBufferStride;

                    int x0 = (int)start.X;
                    int y0 = (int)start.Y;
                    int x1 = (int)end.X;
                    int y1 = (int)end.Y;

                    // Clamp coordinates
                    x0 = Math.Clamp(x0, 0, w - 1);
                    y0 = Math.Clamp(y0, 0, h - 1);
                    x1 = Math.Clamp(x1, 0, w - 1);
                    y1 = Math.Clamp(y1, 0, h - 1);

                    int dx = Math.Abs(x1 - x0);
                    int dy = Math.Abs(y1 - y0);
                    int sx = x0 < x1 ? 1 : -1;
                    int sy = y0 < y1 ? 1 : -1;
                    int err = dx - dy;

                    int thickness = (int)(pressure * 5) + 1; // 1 to 6 px thickness

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
                }
                
                // Add dirty rect for the whole area? Or calculate bounding box.
                // For simplicity, dirty rect the whole thing or bounding box.
                // Calculating bounding box:
                int minX = (int)Math.Min(start.X, end.X) - 5;
                int minY = (int)Math.Min(start.Y, end.Y) - 5;
                int maxX = (int)Math.Max(start.X, end.X) + 5;
                int maxY = (int)Math.Max(start.Y, end.Y) + 5;
                
                minX = Math.Clamp(minX, 0, Width);
                minY = Math.Clamp(minY, 0, Height);
                maxX = Math.Clamp(maxX, 0, Width);
                maxY = Math.Clamp(maxY, 0, Height);
                
                if (maxX > minX && maxY > minY)
                {
                    _bitmap.AddDirtyRect(new Int32Rect(minX, minY, maxX - minX, maxY - minY));
                }
            }
            finally
            {
                _bitmap.Unlock();
            }
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
