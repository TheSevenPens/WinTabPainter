using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SevenPaint
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
            unsafe
            {
                int* pBackBuffer = (int*)_wbmp.BackBuffer;
                int colorData = ConvertColor(color);

                for (int i = 0; i < _width * _height; i++)
                {
                    *pBackBuffer++ = colorData;
                }
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, _width, _height));
            _wbmp.Unlock();
        }

        public void DrawDab(double x, double y, double radius, System.Windows.Media.Color color)
        {
            if (_wbmp == null) return;

            _wbmp.Lock();
            unsafe
            {
                int* pBackBuffer = (int*)_wbmp.BackBuffer;
                int stride = _wbmp.BackBufferStride;

                DrawDabUnsafe(pBackBuffer, stride, x, y, radius, color);
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, _width, _height));
            _wbmp.Unlock();
        }

        private static int ConvertColor(System.Windows.Media.Color color)
        {
            // Pbgra32: A, R, G, B in int memory (Little Endian) for fully opaque
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }

        private unsafe void DrawDabUnsafe(int* buffer, int stride, double cx, double cy, double radius, System.Windows.Media.Color color)
        {
            // Bounding box
            int minX = (int)Math.Floor(cx - radius - 1);
            int maxX = (int)Math.Ceiling(cx + radius + 1);
            int minY = (int)Math.Floor(cy - radius - 1);
            int maxY = (int)Math.Ceiling(cy + radius + 1);

            // Clamp to image bounds
            minX = Math.Max(0, minX);
            maxX = Math.Min(_width - 1, maxX);
            minY = Math.Max(0, minY);
            maxY = Math.Min(_height - 1, maxY);

            // Pre-calculate color components
            double srcA = color.A / 255.0;
            double srcR = color.R * srcA; // Premultiplied source
            double srcG = color.G * srcA;
            double srcB = color.B * srcA;

            double radiusInner = radius - 1.0;
            if (radiusInner < 0) radiusInner = 0;

            for (int y = minY; y <= maxY; y++)
            {
                int* row = buffer + (y * (stride / 4));

                double dy = y - cy;
                double dy2 = dy * dy;

                for (int x = minX; x <= maxX; x++)
                {
                    double dx = x - cx;
                    double distSq = dx * dx + dy2;

                    if (distSq >= (radius + 1) * (radius + 1)) continue;

                    double alphaFactor = 0.0;
                    double dist = Math.Sqrt(distSq);

                    if (dist < radiusInner)
                    {
                        alphaFactor = 1.0;
                    }
                    else if (dist < radius)
                    {
                        alphaFactor = 1.0 - (dist - radiusInner);
                    }
                    else
                    {
                        alphaFactor = 0.0;
                    }

                    if (alphaFactor > 0)
                    {
                        // Current Destination Pixel
                        int destPixel = row[x];
                        
                        // Extract Dest components (Pbgra32: B G R A)
                        byte dA = (byte)((destPixel >> 24) & 0xFF);
                        byte dR = (byte)((destPixel >> 16) & 0xFF);
                        byte dG = (byte)((destPixel >> 8) & 0xFF);
                        byte dB = (byte)(destPixel & 0xFF);

                        // Effective source alpha for this pixel
                        double outSrcA = srcA * alphaFactor;
                        
                        // Premultiplied Source components for this pixel
                        double pSrcR = srcR * alphaFactor;
                        double pSrcG = srcG * alphaFactor;
                        double pSrcB = srcB * alphaFactor;
                        double pSrcA = color.A * alphaFactor; // = outSrcA * 255.0

                        // Destination blend factor
                        double destFactor = 1.0 - outSrcA;

                        double rA = pSrcA + dA * destFactor;
                        double rR = pSrcR + dR * destFactor;
                        double rG = pSrcG + dG * destFactor;
                        double rB = pSrcB + dB * destFactor;

                        byte fA = (byte)Math.Min(255, Math.Max(0, rA));
                        byte fR = (byte)Math.Min(255, Math.Max(0, rR));
                        byte fG = (byte)Math.Min(255, Math.Max(0, rG));
                        byte fB = (byte)Math.Min(255, Math.Max(0, rB));

                        row[x] = (fA << 24) | (fR << 16) | (fG << 8) | fB;
                    }
                }
            }
        }
    }
}
