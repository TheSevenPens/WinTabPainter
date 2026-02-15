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
        private PointerMessageHandler _pointerHandler;
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
            source?.AddHook(WndProc);

            // Enable mouse to act as a pointer device for testing
            NativeMethods.EnableMouseInPointer(true);
            
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
            _pointerHandler = new PointerMessageHandler(_drawingState, _renderer, WritingCanvas);
        }



        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (_pointerHandler.HandlePointerMessage(msg, wParam))
            {
                UpdatePointerStats();
                handled = true;
                return IntPtr.Zero;
            }

            return IntPtr.Zero;
        }

        private void UpdatePointerStats()
        {
            string deviceType = "UNK";
            var pos = this._drawingState.PointerData.CanvasPoint;
            var pressure = this._drawingState.PointerData.PressureNormalized;
            var tiltX = this._drawingState.PointerData.TiltXYDeg.X;
            var tiltY = this._drawingState.PointerData.TiltXYDeg.Y;
            var az = this._drawingState.PointerData.TiltAADeg.Azimuth;
            var alt = this._drawingState.PointerData.TiltAADeg.Altitude;
            var btns = this._drawingState.PointerData.ButtonState.ToString();
            StatusText.Text = $"Device: {deviceType} | Pos: {pos.X:F0},{pos.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY} | Az/Alt: {az:F0},{alt:F0} | Btn: {btns}";
        }

    }

}
