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
            
            // Check if _winink_session was initialized
            if (_winink_session == null)
                return;
                
            var source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(_winink_session.WndProc);

            // Enable mouse to act as a pointer device for testing
            SevenLib.WinInk.Interop.NativeMethods.EnableMouseInPointer(true);

            // Disable WPF Stylus features that might interfere
            Stylus.SetIsPressAndHoldEnabled(this, false);
            Stylus.SetIsFlicksEnabled(this, false);
            Stylus.SetIsTapFeedbackEnabled(this, false);
            Stylus.SetIsTouchFeedbackEnabled(this, false);
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

        private void HandlePointerStatsUpdate(SevenLib.Stylus.PointerDataNew pointerData)
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
                StatusText.Text = $"Device: {deviceType} | Pos: {pos.X:F0},{pos.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY} | Az/Alt: {az:F0},{alt:F0} | Btn: {btns}";
            });
        }

        private void HandlePointerUpdate(SevenLib.Stylus.PointerDataNew pointerData)
        {
            Dispatcher.Invoke(() =>
            {
                bool pointer_in_contact = pointerData.PressureNormalized > 0;

                if (_drawingState.IsDrawing && pointer_in_contact)
                {
                    var canvasPos = WritingCanvas.PointFromScreen(new Point(pointerData.DisplayPoint.X, pointerData.DisplayPoint.Y));
                    var cp = new SevenLib.Geometry.PointD(canvasPos.X, canvasPos.Y);
                    _renderer.DrawLineX(_drawingState.LastCanvasPoint, cp, (float)(pointerData.PressureNormalized * 5));
                    _drawingState.LastCanvasPoint = cp;
                }
            });
        }

        private void HandlePointerUp(SevenLib.Stylus.PointerDataNew pointerdata)
        {
            Dispatcher.Invoke(() =>
            {
                _drawingState.IsDrawing = false;
            });
        }

        private void HandlePointerDown(SevenLib.Stylus.PointerDataNew pointerdata)
        {
            Dispatcher.Invoke(() =>
            {
                _drawingState.IsDrawing = true;
            });
        }
    }

}
