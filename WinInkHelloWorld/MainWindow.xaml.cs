using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using SevenLib.WinInk;

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
            InitializeCanvas();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
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
            _winink_session = new SevenLib.WinInk.WinInkSession( WritingCanvas );
            _winink_session._onPointerStatsUpdated += HandlePointerStatsUpdate;
            _winink_session._PointerUpdateCallback += HandlePointerUpdate;
            _winink_session._PointerUpCallback += HandlePointerUp;
            _winink_session._PointerDownCallback += HandlPointerDown;

        }



        private void HandlePointerStatsUpdate()
        {
            string deviceType = "UNK";
            var pos = this._winink_session.PointerData.CanvasPoint;
            var pressure = this._winink_session.PointerData.PressureNormalized;
            var tiltX = this._winink_session.PointerData.TiltXYDeg.X;
            var tiltY = this._winink_session.PointerData.TiltXYDeg.Y;
            var az = this._winink_session.PointerData.TiltAADeg.Azimuth;
            var alt = this._winink_session.PointerData.TiltAADeg.Altitude;
            var btns = this._winink_session.PointerData.ButtonState.ToString();
            StatusText.Text = $"Device: {deviceType} | Pos: {pos.X:F0},{pos.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY} | Az/Alt: {az:F0},{alt:F0} | Btn: {btns}";
        }

        private void HandlePointerUpdate()
        {
        
            bool pointer_in_contact = _winink_session.PointerData.PressureNormalized > 0;

            if (_drawingState.IsDrawing && pointer_in_contact)
            {
                _renderer.DrawLineX(_winink_session.LastCanvasPoint, _winink_session.PointerData.CanvasPoint, (float)(_winink_session.PointerData.PressureNormalized * 5));
                _winink_session.LastCanvasPoint = _winink_session.PointerData.CanvasPoint;
            }
        }

        private void HandlePointerUp()
        {
            _drawingState.IsDrawing = false;
        }

        private void HandlPointerDown()
        {
            _drawingState.IsDrawing = true;
        }
    }

}
