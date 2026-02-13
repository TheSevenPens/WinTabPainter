using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public void DrawPoint(int x, int y, uint pressure)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return;
            }

            int bx = x;
            int by = y;

            // Draw a simple 3x3 block
            _bitmap.Lock();
            try
            {
                unsafe
                {
                    byte* pBackBuffer = (byte*)_bitmap.BackBuffer;
                    int stride = _bitmap.BackBufferStride;
                    int color = unchecked((int)0xFF000000); // Black (ARGB)

                    // Draw 3x3
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            int px = bx + dx;
                            int py = by + dy;
                            if (px >= 0 && px < Width && py >= 0 && py < Height)
                            {
                                // Use stride to calculate row offset
                                byte* pRow = pBackBuffer + (py * stride);
                                int* pPixel = (int*)(pRow + (px * 4));
                                *pPixel = color;
                            }
                        }
                    }
                }
                // Calculate dirty rect
                int drX = bx - 1;
                int drY = by - 1;
                int drW = 3;
                int drH = 3;

                // Clamp to bitmap bounds
                if (drX < 0) { drW += drX; drX = 0; }
                if (drY < 0) { drH += drY; drY = 0; }
                if (drX + drW > Width) drW = Width - drX;
                if (drY + drH > Height) drH = Height - drY;

                if (drW > 0 && drH > 0)
                {
                    _bitmap.AddDirtyRect(new Int32Rect(drX, drY, drW, drH));
                }
            }
            finally
            {
                _bitmap.Unlock();
            }
        }
        public void DrawRectangle(int x, int y, int width, int height, int color)
        {
            if (width <= 0 || height <= 0) return;

            // Clamp coordinates
            if (x < 0) { width += x; x = 0; }
            if (y < 0) { height += y; y = 0; }
            if (x + width > Width) width = Width - x;
            if (y + height > Height) height = Height - y;

            if (width <= 0 || height <= 0) return;

            _bitmap.Lock();
            try
            {
                unsafe
                {
                    byte* pBackBuffer = (byte*)_bitmap.BackBuffer;
                    int stride = _bitmap.BackBufferStride;

                    for (int dy = 0; dy < height; dy++)
                    {
                        byte* pRow = pBackBuffer + ((y + dy) * stride);
                        int* pPixel = (int*)(pRow + (x * 4));
                        for (int dx = 0; dx < width; dx++)
                        {
                            // Draw only edges (3px thick)
                            if (dx < 3 || dx >= width - 3 || dy < 3 || dy >= height - 3)
                            {
                                *pPixel = color;
                            }
                            pPixel++;
                        }
                    }
                }
                _bitmap.AddDirtyRect(new Int32Rect(x, y, width, height));
            }
            finally
            {
                _bitmap.Unlock();
            }
        }

        public void DrawLine(int x0, int y0, int x1, int y1, int color)
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

                    int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
                    int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
                    int err = dx + dy, e2;

                    while (true)
                    {
                        if (x0 >= 0 && x0 < w && y0 >= 0 && y0 < h)
                        {
                            byte* pRow = pBackBuffer + (y0 * stride);
                            int* pPixel = (int*)(pRow + (x0 * 4));
                            *pPixel = color;
                        }

                        if (x0 == x1 && y0 == y1) break;
                        e2 = 2 * err;
                        if (e2 >= dy) { err += dy; x0 += sx; }
                        if (e2 <= dx) { err += dx; y0 += sy; }
                    }
                }
                // Dirty rect for the whole line bounding box
                int minX = Math.Min(x0, x1);
                int minY = Math.Min(y0, y1);
                int maxX = Math.Max(x0, x1);
                int maxY = Math.Max(y0, y1);
                _bitmap.AddDirtyRect(new Int32Rect(minX, minY, (maxX - minX) + 1, (maxY - minY) + 1));
            }
            finally
            {
                _bitmap.Unlock();
            }
        }
    }
}
