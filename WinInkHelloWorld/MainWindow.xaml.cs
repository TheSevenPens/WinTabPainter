using SevenLib.Stylus;
using SevenLib.WinInk;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

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
            
            if (this._winink_session == null)
                return;

            this._winink_session.AttachToWindow(this);

        }

        private void InitializeCanvas()
        {
            _renderer = new SevenLib.Media.CanvasRenderer(CanvasWidth, CanvasHeight);
            WritingCanvas.Source = _renderer.ImageSource;
            _winink_session = new SevenLib.WinInk.WinInkSession();
            _winink_session._onPointerStatsUpdated += HandlePointerStatsUpdate;
            _winink_session._PointerUpdateCallback += HandlePointerUpdate;
            _winink_session._PointerUpCallback += HandlePointerUp;
            _winink_session._PointerDownCallback += HandlePointerDown;
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

        private void HandlePointerUpdate(SevenLib.Stylus.PointerData pointerData)
        {
            bool pointer_in_contact = pointerData.PressureNormalized > 0;

            Dispatcher.Invoke(() =>
            {
                if (_drawingState.IsDrawing && pointer_in_contact)
                {
                    var cp = ScreenToCanvas(pointerData.DisplayPoint);
                    _renderer.DrawLineX(_drawingState.LastCanvasPoint, cp, (float)(pointerData.PressureNormalized * 5));
                    _drawingState.LastCanvasPoint = cp;
                }
            });
        }

        private SevenLib.Geometry.PointD ScreenToCanvas(SevenLib.Geometry.PointD screenpoint)
        {
            var canvasPos = WritingCanvas.PointFromScreen(new Point(screenpoint.X, screenpoint.Y));
            var cp = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);
            return cp;
        }

        private void HandlePointerUp(SevenLib.Stylus.PointerData pointerdata)
        {
            Dispatcher.Invoke(() =>
            {
                _drawingState.IsDrawing = false;
            });
        }

        private void HandlePointerDown(SevenLib.Stylus.PointerData pointerdata)
        {
            Dispatcher.Invoke(() =>
            {
                var cp = this.ScreenToCanvas(pointerdata.DisplayPoint);
                _drawingState.IsDrawing = true;
                _drawingState.LastCanvasPoint = cp;
            });
        }
    }

}
