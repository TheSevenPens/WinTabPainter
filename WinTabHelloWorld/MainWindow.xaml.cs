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
        private Point _lastLocalPoint; 
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
                try
                {
                    // SevenPaint Logic: Directly map System Coordinate to Element Coordinate
                    // WinTab (with CXO_SYSTEM) returns screen coordinates.
                    Point screenPoint = new Point(packet.pkX, packet.pkY);
                    Point localPoint = CanvasImage.PointFromScreen(screenPoint);

                    _lastLocalPoint = localPoint;

                    Log($"Sys: {packet.pkX},{packet.pkY} -> Cx/Cy: {localPoint.X:F0},{localPoint.Y:F0} P:{packet.pkNormalPressure}");

                    if (packet.pkNormalPressure > 0)
                    {
                        _renderer.DrawPoint((int)localPoint.X, (int)localPoint.Y, packet.pkNormalPressure);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error mapping coordinates: {ex.Message}");
                }
            });
        }





        private void ProcessLogBuffer(object sender, EventArgs e)
        {
            // Update UI with last packet info
            // Update UI with last packet info
            if ((DateTime.Now - _lastPacketTime).TotalSeconds < 1.0)
            {
                 ValGx.Text = _lastPacket.pkX.ToString();
                 ValGy.Text = _lastPacket.pkY.ToString();
                 ValCx.Text = _lastLocalPoint.X.ToString("F0");
                 ValCy.Text = _lastLocalPoint.Y.ToString("F0");
                 
                 ValZ.Text = _lastPacket.pkZ.ToString();
                 ValP.Text = _lastPacket.pkNormalPressure.ToString();
                 ValBtn.Text = _lastPacket.pkButtons.ToString("X"); // Hex for buttons
                 ValAz.Text = _lastPacket.pkOrientation.orAzimuth.ToString();
                 ValAlt.Text = _lastPacket.pkOrientation.orAltitude.ToString();
                 ValTime.Text = _lastPacket.pkTime.ToString();
            }
            else
            {
                 // clear or show dashes
                 ValGx.Text = "-"; ValGy.Text = "-"; 
                 ValCx.Text = "-"; ValCy.Text = "-";
                 
                 ValZ.Text = "-"; ValP.Text = "-";
                 ValBtn.Text = "-"; ValAz.Text = "-"; ValAlt.Text = "-"; ValTime.Text = "-";
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
