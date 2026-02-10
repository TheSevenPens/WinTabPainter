using System;
using System.Windows;
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

        private DispatcherTimer _uiTimer;

        public MainWindow()
        {
            InitializeComponent();
            _uiTimer = new DispatcherTimer();
            _uiTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            _uiTimer.Tick += UpdateUI;
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error opening tablet session: {ex.Message}");
            }
        }

        private void OnPacket(WinTab.Structs.WintabPacket packet)
        {
            _lastPacket = packet;
            _lastPacketTime = DateTime.Now;

            Dispatcher.Invoke(() =>
            {
                try
                {
                    // SevenPaint Logic: Directly map System Coordinate to Element Coordinate
                    // WinTab (with CXO_SYSTEM) returns screen coordinates.
                    Point screenPoint = new Point(packet.pkX, packet.pkY);
                    Point localPoint = CanvasImage.PointFromScreen(screenPoint);

                    _lastLocalPoint = localPoint;

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





        private void UpdateUI(object sender, EventArgs e)
        {
            // Update UI with last packet info
            if ((DateTime.Now - _lastPacketTime).TotalSeconds < 1.0)
            {
                 ValGx.Text = _lastPacket.pkX.ToString();
                 ValGy.Text = _lastPacket.pkY.ToString();
                 ValCx.Text = _lastLocalPoint.X.ToString("F0");
                 ValCy.Text = _lastLocalPoint.Y.ToString("F0");
                 
                 ValZ.Text = _lastPacket.pkZ.ToString();
                 ValP.Text = _lastPacket.pkNormalPressure.ToString();

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
                 ValAz.Text = "-"; ValAlt.Text = "-"; ValTime.Text = "-";
            }
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
