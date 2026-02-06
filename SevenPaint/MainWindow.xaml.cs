using System;
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

namespace SevenPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap _wbmp;
        private const int ImageWidth = 1920;
        private const int ImageHeight = 1080;
        private const int MaxRadius = 20;

        private WinTabUtils.TabletSession? _wintabSession;
        private bool _useWintab = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize WriteableBitmap (Pbgra32) - 96 DPI
            _wbmp = new WriteableBitmap(ImageWidth, ImageHeight, 96, 96, PixelFormats.Pbgra32, null);
            RenderImage.Source = _wbmp;
            
            // Clear to white
            Clear(Colors.White);

            UpdateStatus();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _wintabSession?.Close();
        }

        private void MenuUseWintab_Click(object sender, RoutedEventArgs e)
        {
            _useWintab = MenuUseWintab.IsChecked;

            if (_useWintab)
            {
                try
                {
                    if (_wintabSession == null)
                    {
                        _wintabSession = new WinTabUtils.TabletSession();
                        _wintabSession.PacketHandler = WintabPacketHandler;
                    }
                    _wintabSession.Open(WinTabUtils.TabletContextType.System);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Failed to open Wintab: {ex.Message}");
                    _useWintab = false;
                    MenuUseWintab.IsChecked = false;
                }
            }
            else
            {
                _wintabSession?.Close();
            }
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            StatusLabel.Text = _useWintab ? "Active API: Wintab" : "Active API: Windows Ink";
        }

        private void Clear(System.Windows.Media.Color color)
        {
            _wbmp.Lock();
            unsafe
            {
                int* pBackBuffer = (int*)_wbmp.BackBuffer;
                int colorData = ConvertColor(color);

                for (int i = 0; i < ImageWidth * ImageHeight; i++)
                {
                    *pBackBuffer++ = colorData;
                }
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
            _wbmp.Unlock();
        }

        private static int ConvertColor(System.Windows.Media.Color color)
        {
            // Pbgra32: A, R, G, B in int memory (Little Endian) for fully opaque
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }

        // --- Wintab Handler ---
        private void WintabPacketHandler(WintabDN.Structs.WintabPacket packet)
        {
            if (!_useWintab) return;

            int x = packet.pkX;
            int y = packet.pkY;

            // Pressure: 0 to MaxPressure
            uint pressure = packet.pkNormalPressure;
            uint maxPressure = (uint)_wintabSession.TabletInfo.MaxPressure;
            float pressureFactor = (float)pressure / maxPressure;

            Dispatcher.Invoke(() =>
            {
                // Allow drawing outside just for test, or map global -> local
                var visualPoint = RenderImage.PointFromScreen(new System.Windows.Point(x, y));
                
                DrawDabCore((int)visualPoint.X, (int)visualPoint.Y, pressureFactor);
            });
        }

        private void DrawDabCore(int x, int y, float pressureFactor)
        {
             if (_wbmp == null) return;
             
            _wbmp.Lock();
            unsafe
            {
                int* pBackBuffer = (int*)_wbmp.BackBuffer;
                int stride = _wbmp.BackBufferStride;

                int radius = (int)(MaxRadius * pressureFactor);
                if (radius < 1) radius = 1;

                DrawDab(pBackBuffer, stride, x, y, radius, Colors.Black);
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
            _wbmp.Unlock();
        }

        // --- Windows Ink Handlers ---

        private void RenderImage_StylusDown(object sender, StylusEventArgs e)
        {
            if (_useWintab) return;
            e.Handled = true;
            Draw(e);
        }

        private void RenderImage_StylusMove(object sender, StylusEventArgs e)
        {
            if (_useWintab) return;
            Draw(e);
        }

        private void RenderImage_StylusUp(object sender, StylusEventArgs e)
        {
            if (_useWintab) return;
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

                    DrawDab(pBackBuffer, stride, x, y, radius, Colors.Black);
                }
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
            _wbmp.Unlock();
        }

        private unsafe void DrawDab(int* buffer, int stride, int cx, int cy, int radius, System.Windows.Media.Color color)
        {
            int colorData = ConvertColor(color);
            int r2 = radius * radius;

            int minX = Math.Max(0, cx - radius);
            int maxX = Math.Min(ImageWidth - 1, cx + radius);
            int minY = Math.Max(0, cy - radius);
            int maxY = Math.Min(ImageHeight - 1, cy + radius);

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
}