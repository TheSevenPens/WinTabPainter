using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WinInkHelloWorld
{
    public partial class MainWindow : Window
    {
        private BitmapCanvas _canvas;
        private bool _isDrawing;
        private Point _lastPoint;
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
            _canvas = new BitmapCanvas(CanvasWidth, CanvasHeight);
            WritingCanvas.Source = _canvas.Bitmap;
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
                        HandlePenMessage(msg, penInfo);
                        handled = true;
                    }
                    else if (NativeMethods.GetPointerInfo(pointerId, out POINTER_INFO pointerInfo))
                    {
                         HandlePointerMessage(msg, pointerInfo, pointerType);
                         handled = true;
                    }
                    else
                    {
                        // Debug log for failure
                        UpdateStatus(new Point(0, 0), 0, 0, 0, $"Unknown Pointer ID:{pointerId} Type:{pointerType}");
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

            UpdateStatus(clientPos, pressure, tiltX, tiltY, "Native Pen");

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                _isDrawing = true;
                _lastPoint = clientPos;
                // Capture? In native Win32 usually implied, but we are just drawing.
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                _isDrawing = false;
            }
            else // UPDATE
            {
                bool inContact = (penInfo.pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_INCONTACT) != 0;
                
                if (_isDrawing && inContact)
                {
                    _canvas.DrawLine(_lastPoint, clientPos, pressure);
                    _lastPoint = clientPos;
                }
            }
        }

        private void HandlePointerMessage(int msg, POINTER_INFO pointerInfo, int ptrType)
        {

            NativePoint screenPos = new NativePoint(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            Point clientPos = WritingCanvas.PointFromScreen(new Point(screenPos.X, screenPos.Y));
            
            float pressure = 1.0f;
            string deviceName = ptrType == 4 ? "Mouse" : (ptrType == 2 ? "Touch" : $"Generic({ptrType})");

            UpdateStatus(clientPos, pressure, 0, 0, deviceName);

            if (msg == NativeMethods.WM_POINTERDOWN)
            {
                _isDrawing = true;
                _lastPoint = clientPos;
            }
            else if (msg == NativeMethods.WM_POINTERUP)
            {
                _isDrawing = false;
            }
            else
            {
                bool inContact = (pointerInfo.pointerFlags & NativeMethods.POINTER_FLAG_INCONTACT) != 0;
                if (_isDrawing && inContact)
                {
                    _canvas.DrawLine(_lastPoint, clientPos, pressure);
                    _lastPoint = clientPos;
                }
            }
        }


        private void UpdateStatus(Point pos, float pressure, int tiltX, int tiltY, string deviceType)
        {
            StatusText.Text = $"Device: {deviceType} | Pos: {pos.X:F0},{pos.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY}";
        }

    }

}
