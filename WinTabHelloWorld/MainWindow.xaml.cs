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
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Windows.Threading;

namespace WinTabHelloWorld;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
    public partial class MainWindow : Window
    {
        private WinTab.Utils.TabletSession _session;
        private WriteableBitmap _bitmap;
        private const int BitmapWidth = 800;
        private const int BitmapHeight = 600;

        public ObservableCollection<string> LogEntries { get; set; }
        private ConcurrentQueue<string> _logBuffer;
        private DispatcherTimer _uiTimer;

        public MainWindow()
        {
            InitializeComponent();
            LogEntries = new ObservableCollection<string>();
            _logBuffer = new ConcurrentQueue<string>();
            _uiTimer = new DispatcherTimer();
            _uiTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            _uiTimer.Tick += ProcessLogBuffer;
            _uiTimer.Start();

            DataContext = this;
            InitializeCanvas();
            InitializeTablet();
            Closing += MainWindow_Closing;
        }

        private void InitializeCanvas()
        {
            _bitmap = new WriteableBitmap(BitmapWidth, BitmapHeight, 96, 96, PixelFormats.Bgra32, null);
            CanvasImage.Source = _bitmap;
            
            // Clear bitmap to white
            _bitmap.Lock();
            try
            {
                IntPtr backBuffer = _bitmap.BackBuffer;
                unsafe
                {
                    int* pBackBuffer = (int*)backBuffer;
                    for (int i = 0; i < BitmapWidth * BitmapHeight; i++)
                    {
                        *pBackBuffer++ = unchecked((int)0xFFFFFFFF); // White
                    }
                }
                _bitmap.AddDirtyRect(new Int32Rect(0, 0, BitmapWidth, BitmapHeight));
            }
            finally
            {
                _bitmap.Unlock();
            }
        }

        private void InitializeTablet()
        {
            try
            {
                _session = new WinTab.Utils.TabletSession();
                _session.PacketHandler = OnPacket;
                _session.Open(WinTab.Utils.TabletContextType.Digitizer);
                Log("Tablet session opened.");
            }
            catch (Exception ex)
            {
                Log($"Error opening tablet session: {ex.Message}");
            }
        }

        private void OnPacket(WinTab.Structs.WintabPacket packet)
        {
            // buffer the log message
            _logBuffer.Enqueue($"X: {packet.pkX}, Y: {packet.pkY}, P: {packet.pkNormalPressure}");

            Dispatcher.Invoke(() =>
            { 
               // Log removed from here to avoid UI saturation

                try
                {
                    // Wintab with CXO_SYSTEM usually returns screen coordinates.
                    // Map from Screen to UI Element coordinates.
                    Point screenPoint = new Point(packet.pkX, packet.pkY);
                    Point localPoint = CanvasImage.PointFromScreen(screenPoint);
                    
                    DrawPoint((int)localPoint.X, (int)localPoint.Y, packet.pkNormalPressure);
                }
                catch (Exception ex)
                {
                    // Can happen if window is not loaded or other visual tree issues
                    System.Diagnostics.Debug.WriteLine($"Error mapping coordinates: {ex.Message}");
                }
            });
        }

        private void DrawPoint(int x, int y, uint pressure)
        {
            if (x < 0 || x >= BitmapWidth || y < 0 || y >= BitmapHeight)
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
                    int* pBackBuffer = (int*)_bitmap.BackBuffer;
                    int color = unchecked((int)0xFF000000); // Black (ARGB)
                    // varied alpha by pressure?
                    byte alpha = (byte)((pressure / 1023.0) * 255); // Assuming 1024 levels roughly
                    color = (255 << 24) | (0 << 16) | (0 << 8) | 0; // Black opaque for simplicity

                    // Draw 3x3
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            int px = bx + dx;
                            int py = by + dy;
                            if (px >= 0 && px < BitmapWidth && py >= 0 && py < BitmapHeight)
                            {
                                pBackBuffer[py * BitmapWidth + px] = color;
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
                if (drX + drW > BitmapWidth) drW = BitmapWidth - drX;
                if (drY + drH > BitmapHeight) drH = BitmapHeight - drY;

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

        private void ProcessLogBuffer(object sender, EventArgs e)
        {
            if (_logBuffer.IsEmpty) return;

            int count = 0;
            while (_logBuffer.TryDequeue(out string message) && count < 50) // Limit updates per tick
            {
                LogEntries.Add(message);
                count++;
            }

            if (LogEntries.Count > 2000)
            {
                // Remove excess
                while (LogEntries.Count > 2000)
                {
                    LogEntries.RemoveAt(0);
                }
            }

            if (LogListBox != null && LogListBox.Items.Count > 0)
            {
               LogListBox.ScrollIntoView(LogListBox.Items[LogListBox.Items.Count - 1]);
            }
        }
        
        private void Log(string message)
        {
             _logBuffer.Enqueue(message);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_session != null)
            {
                _session.Close();
                _session = null;
            }
        }
    }
