using SevenLib.WinTab.Stylus;
using System;
using System.Windows;
using System.Windows.Threading;

namespace WinTabHelloWorld;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SevenLib.WinTab.Tablet.TabletSession _wintabsession;
    private SevenLib.Media.CanvasRenderer _renderer;
    private SevenLib.WinTab.Tablet.PointerState _pointerState = new SevenLib.WinTab.Tablet.PointerState();
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
        _wintabsession = new SevenLib.WinTab.Tablet.TabletSession();
        _wintabsession.PacketHandler = HandleWinTabPointerPacket;
        _wintabsession.StylusButtonChangedHandler = OnButtonChange;
        _wintabsession.Open(SevenLib.WinTab.Tablet.TabletContextType.System);
    }

    private void HandleWinTabPointerPacket(SevenLib.WinTab.Structs.WintabPacket packet)
    {

        Dispatcher.Invoke(() =>
        {
            try
            {
                _pointerState.WinTabPacket = packet;
                _pointerState.PointerData = new SevenLib.Stylus.PointerData();
                _pointerState.PointerData.Time = DateTime.Now;

                var screenPos = new Point(packet.pkX, packet.pkY);
                var canvasPos = CanvasImage.PointFromScreen(screenPos);

                _pointerState.PointerData.DisplayPoint= new SevenLib.Geometry.PointD(screenPos.X, screenPos.Y);
                _pointerState.PointerData.CanvasPoint = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y) ;
                _pointerState.PointerData.Height = packet.pkZ;
                float normalized_pressure = (float)packet.pkNormalPressure / _wintabsession.TabletInfo.MaxPressure;
                _pointerState.PointerData.PressureNormalized = normalized_pressure;
                _pointerState.PointerData.TiltAADeg = new SevenLib.Trigonometry.TiltAA(packet.pkOrientation.orAzimuth/10, packet.pkOrientation.orAltitude/10);
                _pointerState.PointerData.TiltXYDeg = _pointerState.PointerData.TiltAADeg.ToXY_Deg();
                _pointerState.PointerData.Twist = packet.pkOrientation.orTwist;
                _pointerState.PointerData.ButtonState = this._wintabsession.StylusButtonState;

                if (_pointerState.PointerData.PressureNormalized > 0)
                {
                    const double brush_size = 15;
                    _renderer.DrawPoint((int)canvasPos.X, (int)canvasPos.Y, (float) (_pointerState.PointerData.PressureNormalized * brush_size));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error mapping coordinates: {ex.Message}");
            }
        });
    }

    private void OnButtonChange(SevenLib.WinTab.Structs.WintabPacket packet, StylusButtonChange buttonChange)
    {
        _buttonStatus = _wintabsession.StylusButtonState.ToString();
    }

    private void UpdatePointerStats(object sender, EventArgs e)
    {
        // Update UI with last packet info
        if ((DateTime.Now - _pointerState.PointerData.Time).TotalSeconds < 1.0)
        {
            ValGx.Text = _pointerState.PointerData.DisplayPoint.X.ToString();
            ValGy.Text = _pointerState.PointerData.DisplayPoint.Y.ToString();
            ValCx.Text = _pointerState.PointerData.CanvasPoint.X.ToString("F0");
            ValCy.Text = _pointerState.PointerData.CanvasPoint.Y.ToString("F0");

            ValZ.Text = _pointerState.PointerData.Height.ToString();
            ValP.Text = _pointerState.PointerData.PressureNormalized.ToString();

            ValAz.Text = _pointerState.PointerData.TiltAADeg.Azimuth.ToString();
            ValAlt.Text = _pointerState.PointerData.TiltAADeg.Altitude.ToString();

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
