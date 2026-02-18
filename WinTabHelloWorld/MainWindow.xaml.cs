using SevenLib.Stylus;
using SevenLib.WinTab;
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
    private WinTabSession _wintabsession;
    private SevenLib.Media.CanvasRenderer _renderer;
    private string _buttonStateText = "None";
    private SevenLib.Stylus.PointerData _lastPointerData;
    private DateTime? _lastPointerDataTime;
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
        _wintabsession = new SevenLib.WinTab.WinTabSession();
        _wintabsession.OnButtonStateChanged = HandleButtonStateChange;
        _wintabsession.OnStandardPointerEvent = this.HandlePointerEvent;
        _wintabsession.Open(SevenLib.WinTab.Enums.TabletContextType.System);
    }

    public void HandlePointerEvent(SevenLib.Stylus.PointerData pointerData) 
    {
        Dispatcher.Invoke(() =>
        {
            _lastPointerData = pointerData;
            _lastPointerDataTime = DateTime.Now;

            if (pointerData.PressureNormalized <= 0)
                return;

            var cp = this.ScreenToCanvas(pointerData.DisplayPoint);
            const double max_brush_size = 15;
            float brush_size = (float)(pointerData.PressureNormalized * max_brush_size);
            _renderer.DrawPoint(cp.ToPoint(), brush_size);
        });
    }

    public SevenLib.Geometry.PointD ScreenToCanvas(SevenLib.Geometry.PointD screenPoint)
    {
        var canvasPos = CanvasImage.PointFromScreen(new Point(screenPoint.X, screenPoint.Y));
        return new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);
    }

    private void HandleButtonStateChange(SevenLib.WinTab.Structs.WintabPacket packet, StylusButtonChange buttonChange)
    {
        _buttonStateText = _wintabsession.StylusButtonState.ToString();
    }

    private void UpdatePointerStats(object sender, EventArgs e)
    {
        // Update UI with last pointer data if recent
        if (_lastPointerDataTime.HasValue && (DateTime.Now - _lastPointerDataTime.Value).TotalSeconds < 1.0)
        {
            ValGx.Text = _lastPointerData.DisplayPoint.X.ToString();
            ValGy.Text = _lastPointerData.DisplayPoint.Y.ToString();

            var cp = this.ScreenToCanvas(_lastPointerData.DisplayPoint);

            ValCx.Text = cp.X.ToString("F0");
            ValCy.Text = cp.Y.ToString("F0");

            ValZ.Text = _lastPointerData.Height.ToString();
            ValP.Text = _lastPointerData.PressureNormalized.ToString();

            ValAz.Text = _lastPointerData.TiltAADeg.Azimuth.ToString();
            ValAlt.Text = _lastPointerData.TiltAADeg.Altitude.ToString();

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
