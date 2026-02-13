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
        private const int CanvasWidth = 600;
        private const int CanvasHeight = 600;

        public MainWindow()
        { // turbo
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
        }



        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {


            switch (msg)
            {
                case NativeMethods.WM_POINTERDOWN:
                case NativeMethods.WM_POINTERUPDATE:
                case NativeMethods.WM_POINTERUP:
                case NativeMethods.WM_POINTERLEAVE:
                    uint pointerId = NativeMethods.GetPointerId(wParam);

                    int pointerType = 0;
                    NativeMethods.GetPointerType(pointerId, out pointerType); 

                    if (NativeMethods.GetPointerPenInfo(pointerId, out POINTER_PEN_INFO penInfo))
                    {
                        // Handling when the pointer is a pen
                        HandlePenMessage(msg, penInfo);
                        handled = true;
                    }
                    else if (NativeMethods.GetPointerInfo(pointerId, out POINTER_INFO pointerInfo))
                    {
                        // Generic pointer handling
                         HandlePointerMessage(msg, pointerInfo, pointerType);
                         handled = true;
                    }
                    else
                    {
                        // Debug log for failure
                        UpdatePointerStats(new Point(0, 0), 0, 0, 0, $"Unknown Pointer ID:{pointerId} Type:{pointerType}");
                    }
                    break;
            }

            return IntPtr.Zero;
        }

        private void HandlePenMessage(int msg, POINTER_PEN_INFO penInfo)
        {

            NativePoint screenPos = new NativePoint(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
            Point clientPos = WritingCanvas.PointFromScreen(new Point(screenPos.X, screenPos.Y));

            float pressure = penInfo.pressure / 1024.0f; 
            int tiltX = penInfo.tiltX;
            int tiltY = penInfo.tiltY;

            UpdatePointerStats(clientPos, pressure, tiltX, tiltY, "Native Pen");

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                _drawingState.IsDrawing = true;
                _drawingState.LastPoint = clientPos;
                // Capture? In native Win32 usually implied, but we are just drawing.
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                _drawingState.IsDrawing = false;
            }
            else // UPDATE
            {
                bool inContact = (penInfo.pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_INCONTACT) != 0;
                
                if (_drawingState.IsDrawing && inContact)
                {
                    _renderer.DrawLine(_drawingState.LastPoint, clientPos, pressure);
                    _drawingState.LastPoint = clientPos;
                }
            }
        }
        private void HandlePointerMessage(int msg, POINTER_INFO pointerInfo, int ptrType)
        {

            NativePoint screenPos = new NativePoint(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            Point clientPos = WritingCanvas.PointFromScreen(new Point(screenPos.X, screenPos.Y));

            float pressure = 1.0f; // generic pointer has no pressure so treat it as max pressure
            string deviceName = pointer_type_to_name(ptrType);
            UpdatePointerStats(clientPos, pressure, 0, 0, deviceName);

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                HandlePointerDown(clientPos);
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                HandlePointerUp();
            }
            else
            {
                HandlePointerUpdate(pointerInfo, clientPos, pressure);
            }
        }

        private void HandlePointerUpdate(POINTER_INFO pointerInfo, Point canvasPos, float pressure)
        {
            bool inContact = (pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_INCONTACT) != 0;

            if (_drawingState.IsDrawing && inContact)
            {
                _renderer.DrawLine(_drawingState.LastPoint, canvasPos, pressure);
                _drawingState.LastPoint = canvasPos;
            }
        }

        private void HandlePointerUp()
        {
            _drawingState.IsDrawing = false;
        }

        private void HandlePointerDown(Point canvasPos)
        {
            _drawingState.IsDrawing = true;
            _drawingState.LastPoint = canvasPos;
        }



        private static string pointer_type_to_name(int ptrType)
        {
            if (ptrType == (int)POINTER_INPUT_TYPE.PT_MOUSE)
            {
                return "Mouse";
            }
            else if (ptrType == (int)POINTER_INPUT_TYPE.PT_TOUCH)
            {
                return ("Touch");
            }
            else
            {
                return ($"Generic({ptrType})");
            }
        }

        private void UpdatePointerStats(Point pos, float pressure, int tiltX, int tiltY, string deviceType)
        {
            StatusText.Text = $"Device: {deviceType} | Pos: {pos.X:F0},{pos.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY}";
        }

    }

}
