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
    private SevenLib.WinTab.Tablet.WinTabSession2 _wintabsession;
    private SevenLib.Media.CanvasRenderer _renderer;
    private string _buttonStateText = "None";
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
        _wintabsession = new SevenLib.WinTab.Tablet.WinTabSession2();
        _wintabsession.OnRawPacketReceived = HandleRawPacket;
        _wintabsession.OnButtonStateChanged = HandleButtonStateChange;
        _wintabsession.OnPointerEvent = this.HandlePointerEvent;
        _wintabsession.Open(SevenLib.WinTab.Tablet.TabletContextType.System);
    }

    private void HandleRawPacket(SevenLib.WinTab.Structs.WintabPacket packet)
    {

        Dispatcher.Invoke(() =>
        {
            try
            {
                this._wintabsession.WinTabPacket = packet;
                this._wintabsession.PointerData = new SevenLib.Stylus.PointerData2();
                this._wintabsession.PointerData.Time = DateTime.Now;

                var screenPos = new Point(packet.pkX, packet.pkY);
                this._wintabsession.PointerData.DisplayPoint = new SevenLib.Geometry.PointD(screenPos.X, screenPos.Y);

                this._wintabsession.PointerData.Height = packet.pkZ;
                float normalized_pressure = (float)packet.pkNormalPressure / _wintabsession.TabletInfo.MaxPressure;
                this._wintabsession.PointerData.PressureNormalized = normalized_pressure;
                this._wintabsession.PointerData.TiltAADeg = new SevenLib.Trigonometry.TiltAA(packet.pkOrientation.orAzimuth/10, packet.pkOrientation.orAltitude/10);
                this._wintabsession.PointerData.TiltXYDeg = this._wintabsession.PointerData.TiltAADeg.ToXY_Deg();
                this._wintabsession.PointerData.Twist = packet.pkOrientation.orTwist;
                this._wintabsession.PointerData.ButtonState = this._wintabsession.StylusButtonState;

                if (this.HandlePointerEvent!=null)
                {
                    this.HandlePointerEvent();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error mapping coordinates: {ex.Message}");
            }
        });
    }

    public void HandlePointerEvent() 
    {
        if (this._wintabsession.PointerData.PressureNormalized > 0)
        {

            var canvasPos = CanvasImage.PointFromScreen( new Point(
                this._wintabsession.PointerData.DisplayPoint.X,
                this._wintabsession.PointerData.DisplayPoint.Y) );


            var cp = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);

            const double brush_size = 15;
            _renderer.DrawPoint((int)cp.X, (int)cp.Y, (float)(this._wintabsession.PointerData.PressureNormalized * brush_size));
        }
    }

    private void HandleButtonStateChange(SevenLib.WinTab.Structs.WintabPacket packet, StylusButtonChange buttonChange)
    {
        _buttonStateText = _wintabsession.StylusButtonState.ToString();
    }

    private void UpdatePointerStats(object sender, EventArgs e)
    {
        // Update UI with last packet info
        if ((DateTime.Now - this._wintabsession.PointerData.Time).TotalSeconds < 1.0)
        {
            ValGx.Text = this._wintabsession.PointerData.DisplayPoint.X.ToString();
            ValGy.Text = this._wintabsession.PointerData.DisplayPoint.Y.ToString();
            //ValCx.Text = this._wintabsession.PointerData.CanvasPoint.X.ToString("F0");
            //ValCy.Text = this._wintabsession.PointerData.CanvasPoint.Y.ToString("F0");

            ValZ.Text = this._wintabsession.PointerData.Height.ToString();
            ValP.Text = this._wintabsession.PointerData.PressureNormalized.ToString();

            ValAz.Text = this._wintabsession.PointerData.TiltAADeg.Azimuth.ToString();
            ValAlt.Text = this._wintabsession.PointerData.TiltAADeg.Altitude.ToString();

            ValBtn.Text = _buttonStateText;
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
