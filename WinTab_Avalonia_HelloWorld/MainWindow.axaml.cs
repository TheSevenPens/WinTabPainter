using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using SevenLib.Stylus;
using SevenLib.WinTab;
using SevenLib.WinTab.Stylus;
using SkiaSharp;

namespace WinTab_Avalonia_HelloWorld;

public partial class MainWindow : Window
{
    private WinTabSession? _wintabsession;
    private WriteableBitmap _bitmap = null!;
    private string _buttonStateText = "None";
    private PointerData _lastPointerData;
    private DateTime? _lastPointerDataTime;
    private DispatcherTimer _uiTimer = null!;
    private const int DefaultCanvasWidth = 800;
    private const int DefaultCanvasHeight = 600;

    public MainWindow()
    {
        InitializeComponent();

        _uiTimer = new DispatcherTimer();
        _uiTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
        _uiTimer.Tick += UpdatePointerStats;
        _uiTimer.Start();

        InitializeCanvas();
        InitializeTablet();
        Closing += MainWindow_Closing;
    }

    private void InitializeCanvas()
    {
        _bitmap = new WriteableBitmap(
            new PixelSize(DefaultCanvasWidth, DefaultCanvasHeight),
            new Vector(96, 96),
            Avalonia.Platform.PixelFormat.Bgra8888,
            Avalonia.Platform.AlphaFormat.Premul);

        ClearCanvas();
        CanvasImage.Source = _bitmap;
    }

    private void ClearCanvas()
    {
        using var fb = _bitmap.Lock();
        var info = new SKImageInfo(DefaultCanvasWidth, DefaultCanvasHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
        using var surface = SKSurface.Create(info, fb.Address, fb.RowBytes);
        surface.Canvas.Clear(SKColors.White);
    }

    private void InitializeTablet()
    {
        _wintabsession = new WinTabSession();
        _wintabsession.OnButtonStateChanged = HandleButtonStateChange;
        _wintabsession.OnWinTabPacketReceived = (packet) =>
        {
            var pointerData = _wintabsession.create_pointerdata_from_wintabpacket(packet);
            HandlePointerEvent(pointerData);
        };
        _wintabsession.Open(SevenLib.WinTab.Enums.TabletContextType.System);
    }

    private void HandlePointerEvent(PointerData pointerData)
    {
        Dispatcher.UIThread.Post(() =>
        {
            _lastPointerData = pointerData;
            _lastPointerDataTime = DateTime.Now;

            if (pointerData.PressureNormalized <= 0)
                return;

            var cp = ScreenToCanvas(pointerData.DisplayPoint);
            const double max_brush_size = 15;
            float brush_size = (float)(pointerData.PressureNormalized * max_brush_size);
            DrawPoint(cp, brush_size);
        });
    }

    private void DrawPoint(SevenLib.Geometry.PointD pos, float size)
    {
        using var fb = _bitmap.Lock();
        var info = new SKImageInfo(DefaultCanvasWidth, DefaultCanvasHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
        using var surface = SKSurface.Create(info, fb.Address, fb.RowBytes);
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill
        };
        surface.Canvas.DrawRect(
            (float)(pos.X - size / 2), (float)(pos.Y - size / 2),
            size, size, paint);

        CanvasImage.InvalidateVisual();
    }

    private SevenLib.Geometry.PointD ScreenToCanvas(SevenLib.Geometry.PointD screenPoint)
    {
        var screenPixelPoint = new PixelPoint((int)screenPoint.X, (int)screenPoint.Y);
        var clientPoint = CanvasImage.PointToClient(screenPixelPoint);
        return new SevenLib.Geometry.PointD(clientPoint.X, clientPoint.Y);
    }

    private void HandleButtonStateChange(SevenLib.WinTab.Structs.WintabPacket packet, StylusButtonChange buttonChange)
    {
        _buttonStateText = _wintabsession!.StylusButtonState.ToString();
    }

    private void UpdatePointerStats(object? sender, EventArgs e)
    {
        if (_lastPointerDataTime.HasValue && (DateTime.Now - _lastPointerDataTime.Value).TotalSeconds < 1.0)
        {
            ValGx.Text = _lastPointerData.DisplayPoint.X.ToString();
            ValGy.Text = _lastPointerData.DisplayPoint.Y.ToString();

            var cp = ScreenToCanvas(_lastPointerData.DisplayPoint);

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
            ValGx.Text = "-"; ValGy.Text = "-";
            ValCx.Text = "-"; ValCy.Text = "-";
            ValZ.Text = "-"; ValP.Text = "-";
            ValAz.Text = "-"; ValAlt.Text = "-";
        }
    }

    private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        _uiTimer.Stop();
        if (_wintabsession != null)
        {
            _wintabsession.Close();
            _wintabsession = null;
        }
    }
}
