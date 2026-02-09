using System.Windows.Media;

namespace SevenPaint.Paint
{
    public class CanvasDocument
    {
        public int Width { get; }
        public int Height { get; }
        public double Dpi { get; }
        public PixelCanvas ImageLayer { get; }

        public ImageSource Source => ImageLayer.Source;

        public CanvasDocument(int width, int height, double dpi)
        {
            Width = width;
            Height = height;
            Dpi = dpi;
            ImageLayer = new PixelCanvas(width, height, dpi);
        }

        public void Clear(System.Windows.Media.Color color)
        {
            ImageLayer.Clear(color);
        }

        public void DrawDab(double x, double y, double radius, System.Windows.Media.Color color)
        {
            ImageLayer.DrawDab(x, y, radius, color);
        }

        public bool Contains(double x, double y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
