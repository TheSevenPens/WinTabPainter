using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using SkiaSharp;

namespace WinInk_Avalonia_HelloWorld;

public partial class MainWindow : Window
{
    private WriteableBitmap _bitmap;
    private DrawingState _drawingState = new DrawingState();
    private SevenLib.WinInk.WinInkSession _winink_session;
    private const int CanvasWidth = 600;
    private const int CanvasHeight = 600;

    public MainWindow()
    {
        _winink_session = new SevenLib.WinInk.WinInkSession();
        _winink_session._PointerPenInfoCallback += this.HandlePointerPenInfo;
        _winink_session._PointerInfoCallback += this.HandlePointerInfo;

        InitializeComponent();
        InitializeCanvas();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        // Get the native window handle once the window is opened
        var handle = TryGetPlatformHandle();
        if (handle != null)
        {
            _winink_session.AttachToWindow(handle.Handle);
        }
    }

    private void InitializeCanvas()
    {
        _bitmap = new WriteableBitmap(
            new PixelSize(CanvasWidth, CanvasHeight),
            new Vector(96, 96),
            Avalonia.Platform.PixelFormat.Bgra8888,
            Avalonia.Platform.AlphaFormat.Premul);

        ClearCanvas();
        WritingCanvas.Source = _bitmap;
    }

    private void ClearCanvas()
    {
        using (var fb = _bitmap.Lock())
        {
            var info = new SKImageInfo(CanvasWidth, CanvasHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(info, fb.Address, fb.RowBytes);
            surface.Canvas.Clear(SKColors.White);
        }
    }

    private void DrawLineOnCanvas(SevenLib.Geometry.PointD start, SevenLib.Geometry.PointD end, float thickness)
    {
        using (var fb = _bitmap.Lock())
        {
            var info = new SKImageInfo(CanvasWidth, CanvasHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var surface = SKSurface.Create(info, fb.Address, fb.RowBytes);
            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = thickness,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };
            surface.Canvas.DrawLine(
                (float)start.X, (float)start.Y,
                (float)end.X, (float)end.Y, paint);
        }

        // Force the Image control to refresh
        WritingCanvas.InvalidateVisual();
    }

    private void HandlePointerStatsUpdate(SevenLib.Stylus.PointerData pointerData)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var pos = pointerData.DisplayPoint;
            var pressure = pointerData.PressureNormalized;
            var tiltX = pointerData.TiltXYDeg.X;
            var tiltY = pointerData.TiltXYDeg.Y;
            var az = pointerData.TiltAADeg.Azimuth;
            var alt = pointerData.TiltAADeg.Altitude;
            var btns = pointerData.ButtonState.ToString();

            var cp = this.ScreenToCanvas(pointerData.DisplayPoint);

            StatusText.Text = $"Pos: {cp.X:F0},{cp.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY} | Az/Alt: {az:F0},{alt:F0} | Btn: {btns}";
        });
    }

    private void HandlePointerPenInfo(int msg, int pointerType, SevenLib.WinInk.Interop.POINTER_PEN_INFO penInfo)
    {
        var pointerdata = SevenLib.WinInk.WinInkSession.create_pointer_data_from_pen_info(penInfo);
        this.HandlePointerData(msg, pointerType, pointerdata);
    }

    private void HandlePointerInfo(int msg, int pointerType, SevenLib.WinInk.Interop.POINTER_INFO pointerInfo)
    {
        var pointerdata = SevenLib.WinInk.WinInkSession.create_pointer_data_from_pointer_info(pointerInfo);
        this.HandlePointerData(msg, pointerType, pointerdata);
    }

    private void HandlePointerData(int msg, int pointerType, SevenLib.Stylus.PointerData pointerdata)
    {
        if (msg == SevenLib.WinInk.Interop.NativeMethods.WM_POINTERDOWN)
        {
            Dispatcher.UIThread.Post(() =>
            {
                var cp = this.ScreenToCanvas(pointerdata.DisplayPoint);
                _drawingState.IsDrawing = true;
                _drawingState.LastCanvasPoint = cp;
                HandlePointerStatsUpdate(pointerdata);
            });
        }
        else if (msg == SevenLib.WinInk.Interop.NativeMethods.WM_POINTERUP)
        {
            Dispatcher.UIThread.Post(() =>
            {
                _drawingState.IsDrawing = false;
                HandlePointerStatsUpdate(pointerdata);
            });
        }
        else if (msg == SevenLib.WinInk.Interop.NativeMethods.WM_POINTERUPDATE)
        {
            Dispatcher.UIThread.Post(() =>
            {
                bool pointer_in_contact = pointerdata.PressureNormalized > 0;

                if (_drawingState.IsDrawing && pointer_in_contact)
                {
                    var cp = ScreenToCanvas(pointerdata.DisplayPoint);
                    DrawLineOnCanvas(_drawingState.LastCanvasPoint, cp, (float)(pointerdata.PressureNormalized * 5));
                    _drawingState.LastCanvasPoint = cp;
                }

                HandlePointerStatsUpdate(pointerdata);
            });
        }
    }

    private SevenLib.Geometry.PointD ScreenToCanvas(SevenLib.Geometry.PointD screenpoint)
    {
        // Convert screen coordinates to canvas-relative coordinates
        var screenPixelPoint = new PixelPoint((int)screenpoint.X, (int)screenpoint.Y);
        var clientPoint = WritingCanvas.PointToClient(screenPixelPoint);
        return new SevenLib.Geometry.PointD(clientPoint.X, clientPoint.Y);
    }
}
