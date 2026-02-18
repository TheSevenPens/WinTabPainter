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
            AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport", true);

            // Initialize session before InitializeComponent so it's ready for OnSourceInitialized
            _winink_session = new SevenLib.WinInk.WinInkSession();
            _winink_session._PointerPenInfoCallback += this.HandlePointerPenInfo;
            _winink_session._PointerInfoCallback += this.HandlePointerInfo;

            InitializeComponent();
            InitializeCanvas();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Get native window handle and attach
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            this._winink_session.AttachToWindow(hwnd);

            // Disable WPF Stylus features that might interfere
            System.Windows.Input.Stylus.SetIsPressAndHoldEnabled(this, false);
            System.Windows.Input.Stylus.SetIsFlicksEnabled(this, false);
            System.Windows.Input.Stylus.SetIsTapFeedbackEnabled(this, false);
            System.Windows.Input.Stylus.SetIsTouchFeedbackEnabled(this, false);
        }

        private void InitializeCanvas()
        {
            _renderer = new SevenLib.Media.CanvasRenderer(CanvasWidth, CanvasHeight);
            WritingCanvas.Source = _renderer.ImageSource;
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
