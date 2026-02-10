using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private CanvasRenderer _renderer;
        private WinTab.Structs.WintabPacket _lastPacket;
        private DateTime _lastPacketTime;
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
            _renderer = new CanvasRenderer(BitmapWidth, BitmapHeight);
            CanvasImage.Source = _renderer.ImageSource;
        }

        private void InitializeTablet()
        {
            try
            {
                _session = new WinTab.Utils.TabletSession();
                _session.PacketHandler = OnPacket;
                _session.Open(WinTab.Utils.TabletContextType.System);
                Log("Tablet session opened (System Context).");
            }
            catch (Exception ex)
            {
                Log($"Error opening tablet session: {ex.Message}");
            }
        }

        private void OnPacket(WinTab.Structs.WintabPacket packet)
        {
            _lastPacket = packet;
            _lastPacketTime = DateTime.Now;

            // buffer the log message
            _logBuffer.Enqueue($"X: {packet.pkX}, Y: {packet.pkY}, P: {packet.pkNormalPressure}");

            Dispatcher.Invoke(() =>
            { 
               // Log removed from here to avoid UI saturation

                try
                {
                    // Wintab with CXO_SYSTEM returns virtual screen coordinates.
                    // We can map these directly to the element.
                    Point screenPoint = new Point(packet.pkX, packet.pkY);
                    Point localPoint = CanvasImage.PointFromScreen(screenPoint);
                    
                    // Log mapped coordinates for debugging
                    Log($"System: {packet.pkX},{packet.pkY} -> Local: {localPoint.X:F0},{localPoint.Y:F0} P:{packet.pkNormalPressure}");

                    if (packet.pkNormalPressure > 0)
                    {
                        _renderer.DrawPoint((int)localPoint.X, (int)localPoint.Y, packet.pkNormalPressure);
                    }
                }
                catch (Exception ex)
                {
                    // Can happen if window is not loaded or other visual tree issues
                    System.Diagnostics.Debug.WriteLine($"Error mapping coordinates: {ex.Message}");
                }
            });
        }



        private void ProcessLogBuffer(object sender, EventArgs e)
        {
            // Update UI with last packet info
            if ((DateTime.Now - _lastPacketTime).TotalSeconds < 1.0)
            {
                 PacketInfoText.Text = $"X: {_lastPacket.pkX}, Y: {_lastPacket.pkY}, Z: {_lastPacket.pkZ}, P: {_lastPacket.pkNormalPressure}, Btn: {_lastPacket.pkButtons}, Az: {_lastPacket.pkOrientation.orAzimuth}, Alt: {_lastPacket.pkOrientation.orAltitude}";
            }
            else
            {
                 PacketInfoText.Text = "No input...";
            }

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
