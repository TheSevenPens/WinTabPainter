using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SevenLib.MediaXXX
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

    }
}
