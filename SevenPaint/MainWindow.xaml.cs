using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SevenPaint;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private WriteableBitmap _wbmp;
    private const int Width = 1920;
    private const int Height = 1080;
    private const int MaxRadius = 20;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Initialize WriteableBitmap (Pbgra32) - 96 DPI
        _wbmp = new WriteableBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32, null);
        RenderImage.Source = _wbmp;
        
        // Clear to white
        Clear(Colors.White);
    }

    private void Clear(Color color)
    {
        _wbmp.Lock();
        unsafe
        {
            int* pBackBuffer = (int*)_wbmp.BackBuffer;
            int colorData = ConvertColor(color);

            for (int i = 0; i < Width * Height; i++)
            {
                *pBackBuffer++ = colorData;
            }
        }
        _wbmp.AddDirtyRect(new Int32Rect(0, 0, Width, Height));
        _wbmp.Unlock();
    }

    private static int ConvertColor(Color color)
    {
        // Pbgra32: A, R, G, B in int memory (Little Endian) for fully opaque
        return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
    }

    private void RenderImage_StylusDown(object sender, StylusEventArgs e)
    {
        e.Handled = true;
        Draw(e);
    }

    private void RenderImage_StylusMove(object sender, StylusEventArgs e)
    {
        Draw(e);
    }

    private void RenderImage_StylusUp(object sender, StylusEventArgs e)
    {
        e.Handled = true;
        Draw(e);
    }

    private void Draw(StylusEventArgs e)
    {
        if (_wbmp == null) return;

        var points = e.GetStylusPoints(RenderImage);
        
        _wbmp.Lock();
        unsafe
        {
            int* pBackBuffer = (int*)_wbmp.BackBuffer;
            int stride = _wbmp.BackBufferStride;

            foreach (var point in points)
            {
                int x = (int)point.X;
                int y = (int)point.Y;
                float pressure = point.PressureFactor;

                int radius = (int)(MaxRadius * pressure);
                if (radius < 1) radius = 1;

                // Draw Dab
                DrawDab(pBackBuffer, stride, x, y, radius, Colors.Black);
            }
        }
        _wbmp.AddDirtyRect(new Int32Rect(0, 0, Width, Height));
        _wbmp.Unlock();
    }

    private unsafe void DrawDab(int* buffer, int stride, int cx, int cy, int radius, Color color)
    {
        int colorData = ConvertColor(color);
        int r2 = radius * radius;

        int minX = Math.Max(0, cx - radius);
        int maxX = Math.Min(Width - 1, cx + radius);
        int minY = Math.Max(0, cy - radius);
        int maxY = Math.Min(Height - 1, cy + radius);

        for (int y = minY; y <= maxY; y++)
        {
            int dy = y - cy;
            int dy2 = dy * dy;
            
            // Get row pointer
            // Stride is rendering bytes, so we divide by 4 for int*
            int* row = buffer + (y * (stride / 4));

            for (int x = minX; x <= maxX; x++)
            {
                int dx = x - cx;
                if (dx * dx + dy2 <= r2)
                {
                     row[x] = colorData;
                }
            }
        }
    }
}