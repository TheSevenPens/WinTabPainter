using System;
using System.Windows;
using System.Windows.Threading;

namespace WinTabHelloWorld;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SevenLib.WinTab.Utils.TabletSession _wintabsession;
    private SevenLib.Media.CanvasRenderer _renderer;
    private DrawingState _drawingState = new DrawingState();
    private string _buttonStatus = "None";
    private const int DefaultCanvasWidth = 800;
    private const int DefaultCanvasHeight = 600;

    private DispatcherTimer _uiTimer;

    public MainWindow()
    {
        InitializeComponent();
        _uiTimer = new DispatcherTimer();
        _uiTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
        _uiTimer.Tick += UpdatePointerStats;
        _uiTimer.Start();

        DataContext = this;
        InitializeCanvas();
        InitializeTablet();
        Closing += MainWindow_Closing;
    }

    private void InitializeCanvas()
    {
        _renderer = new SevenLib.Media.CanvasRenderer(DefaultCanvasWidth, DefaultCanvasHeight);
        CanvasImage.Source = _renderer.ImageSource;
    }

    private void InitializeTablet()
    {
        _wintabsession = new SevenLib.WinTab.Utils.TabletSession();
        _wintabsession.PacketHandler = HandleWinTabPointerPacket;
        _wintabsession.StylusButtonChangedHandler = OnButtonChange;
        _wintabsession.Open(SevenLib.WinTab.Utils.TabletContextType.System);
    }

    private void HandleWinTabPointerPacket(SevenLib.WinTab.Structs.WintabPacket packet)
    {
        _drawingState.Packet = packet;
        _drawingState.Time = DateTime.Now;

        Dispatcher.Invoke(() =>
        {
            try
            {
                // SevenPaint Logic: Directly map System Coordinate to Element Coordinate
                // WinTab (with CXO_SYSTEM) returns screen coordinates.
                Point screenPos = new Point(packet.pkX, packet.pkY);
                Point canvasPos = CanvasImage.PointFromScreen(screenPos);

                _drawingState.LocalPoint = canvasPos;

                if (packet.pkNormalPressure > 0)
                {
                    float normalized_pressure = (float)packet.pkNormalPressure / _wintabsession.TabletInfo.MaxPressure;
                    _renderer.DrawPoint((int)canvasPos.X, (int)canvasPos.Y, normalized_pressure * 15.0f);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error mapping coordinates: {ex.Message}");
            }
        });
    }

    private void OnButtonChange(SevenLib.WinTab.Structs.WintabPacket packet, SevenLib.WinTab.Utils.StylusButtonChange buttonChange)
    {
        _buttonStatus = _wintabsession.StylusButtonState.ToString();
    }

    private void UpdatePointerStats(object sender, EventArgs e)
    {
        // Update UI with last packet info
        if ((DateTime.Now - _drawingState.Time).TotalSeconds < 1.0)
        {
            ValGx.Text = _drawingState.Packet.pkX.ToString();
            ValGy.Text = _drawingState.Packet.pkY.ToString();
            ValCx.Text = _drawingState.LocalPoint.X.ToString("F0");
            ValCy.Text = _drawingState.LocalPoint.Y.ToString("F0");

            ValZ.Text = _drawingState.Packet.pkZ.ToString();
            ValP.Text = _drawingState.Packet.pkNormalPressure.ToString();

            ValAz.Text = _drawingState.Packet.pkOrientation.orAzimuth.ToString();
            ValAlt.Text = _drawingState.Packet.pkOrientation.orAltitude.ToString();

            ValBtn.Text = _buttonStatus;
        }
        else
        {
            // clear or show dashes
            ValGx.Text = "-"; ValGy.Text = "-";
            ValCx.Text = "-"; ValCy.Text = "-";
            ValZ.Text = "-"; ValP.Text = "-";
            ValAz.Text = "-"; ValAlt.Text = "-";
        }
    }

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_wintabsession != null)
        {
            _wintabsession.Close();
            _wintabsession = null;
        }
    }


}
