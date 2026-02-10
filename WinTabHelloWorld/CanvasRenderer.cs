using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinTabHelloWorld
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
    }
}
