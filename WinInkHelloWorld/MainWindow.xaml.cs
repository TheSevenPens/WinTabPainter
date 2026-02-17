using System;
using System.Windows;

namespace WinInkHelloWorld
{
    public partial class MainWindow : Window
    {
        private SevenLib.Media.CanvasRenderer _renderer;
        private DrawingState _drawingState = new DrawingState();
        private SevenLib.WinInk.WinInkSession _winink_session;
        private const int CanvasWidth = 600;
        private const int CanvasHeight = 600;


        public MainWindow()
        { 
            // Disable WPF's internal stylus support to prevent it from swallowing WM_POINTER messages
            AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport", true);
            
            InitializeComponent();
            InitializeCanvas();  // Must be before OnSourceInitialized runs
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this._winink_session.AttachToWindow(this);
        }

        private void InitializeCanvas()
        {
            _renderer = new SevenLib.Media.CanvasRenderer(CanvasWidth, CanvasHeight);
            WritingCanvas.Source = _renderer.ImageSource;
            _winink_session = new SevenLib.WinInk.WinInkSession();
            _winink_session._PointerCallback += HandlePointerMessage;
        }

        private void HandlePointerStatsUpdate(SevenLib.Stylus.PointerData pointerData)
        {
            Dispatcher.Invoke(() =>
            {
                string deviceType = "UNK";
                var pos = pointerData.DisplayPoint;
                var pressure = pointerData.PressureNormalized;
                var tiltX = pointerData.TiltXYDeg.X;
                var tiltY = pointerData.TiltXYDeg.Y;
                var az = pointerData.TiltAADeg.Azimuth;
                var alt = pointerData.TiltAADeg.Altitude;
                var btns = pointerData.ButtonState.ToString();

                var cp = this.ScreenToCanvas(pointerData.DisplayPoint);

                StatusText.Text = $"Device: {deviceType} | Pos: {cp.X:F0},{cp.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY} | Az/Alt: {az:F0},{alt:F0} | Btn: {btns}";
            });
        }

        private void HandlePointerMessage(int msg, int pointerType, SevenLib.Stylus.PointerData pointerdata)
        {
            if (msg == SevenLib.WinInk.Interop.NativeMethods.WM_POINTERDOWN)
            {
                Dispatcher.Invoke(() =>
                {
                    var cp = this.ScreenToCanvas(pointerdata.DisplayPoint);
                    _drawingState.IsDrawing = true;
                    _drawingState.LastCanvasPoint = cp;
                    HandlePointerStatsUpdate(pointerdata);
                });
            }
            else if (msg == SevenLib.WinInk.Interop.NativeMethods.WM_POINTERUP)
            {
                Dispatcher.Invoke(() =>
                {
                    _drawingState.IsDrawing = false;
                    HandlePointerStatsUpdate(pointerdata);
                });
            }
            else if (msg == SevenLib.WinInk.Interop.NativeMethods.WM_POINTERUPDATE)
            {
                Dispatcher.Invoke(() =>
                {
                    bool pointer_in_contact = pointerdata.PressureNormalized > 0;

                    if (_drawingState.IsDrawing && pointer_in_contact)
                    {
                        var cp = ScreenToCanvas(pointerdata.DisplayPoint);
                        _renderer.DrawLineX(_drawingState.LastCanvasPoint, cp, (float)(pointerdata.PressureNormalized * 5));
                        _drawingState.LastCanvasPoint = cp;
                    }

                    HandlePointerStatsUpdate(pointerdata);
                });
            }
        }

        private SevenLib.Geometry.PointD ScreenToCanvas(SevenLib.Geometry.PointD screenpoint)
        {
            var canvasPos = WritingCanvas.PointFromScreen(new Point(screenpoint.X, screenpoint.Y));
            var cp = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);
            return cp;
        }
    }

}
