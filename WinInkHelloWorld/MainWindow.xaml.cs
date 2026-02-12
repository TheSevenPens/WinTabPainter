using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinInkHelloWorld
{
    public partial class MainWindow : Window
    {
        private WriteableBitmap _bitmap;
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
            EnableMouseInPointer(true);
            
            // Disable WPF Stylus features that might interfere
            Stylus.SetIsPressAndHoldEnabled(this, false);
            Stylus.SetIsFlicksEnabled(this, false);
            Stylus.SetIsTapFeedbackEnabled(this, false);
            Stylus.SetIsTouchFeedbackEnabled(this, false);
        }

        private void InitializeCanvas()
        {
            _bitmap = new WriteableBitmap(CanvasWidth, CanvasHeight, 96, 96, PixelFormats.Bgra32, null);
            WritingCanvas.Source = _bitmap;
            ClearCanvas();
        }

        private void ClearCanvas()
        {
            // Fill with white
            int stride = _bitmap.BackBufferStride;
            int bytes = stride * _bitmap.PixelHeight;
            byte[] pixels = new byte[bytes];
            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = 255;     // Blue
                pixels[i + 1] = 255; // Green
                pixels[i + 2] = 255; // Red
                pixels[i + 3] = 255; // Alpha
            }
            _bitmap.WritePixels(new Int32Rect(0, 0, CanvasWidth, CanvasHeight), pixels, stride, 0);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_POINTERDOWN = 0x0246;
            const int WM_POINTERUPDATE = 0x0245;
            const int WM_POINTERUP = 0x0247;
            const int WM_POINTERLEAVE = 0x024A;

            switch (msg)
            {
                case WM_POINTERDOWN:
                case WM_POINTERUPDATE:
                case WM_POINTERUP:
                case WM_POINTERLEAVE:
                    uint pointerId = GetPointerId(wParam);
                    int pointerType = 0;
                    GetPointerType(pointerId, out pointerType); // 1=Generic, 2=Touch, 3=Pen, 4=Mouse

                    if (GetPointerPenInfo(pointerId, out POINTER_PEN_INFO penInfo))
                    {
                        HandlePenMessage(msg, penInfo);
                        handled = true;
                    }
                    else if (GetPointerInfo(pointerId, out POINTER_INFO pointerInfo))
                    {
                         HandlePointerMessage(msg, pointerInfo, pointerType);
                         handled = true;
                    }
                    else
                    {
                        // Debug log for failure
                        UpdateStatus(new Point(0,0), 0, 0, 0, $"Unknown Pointer ID:{pointerId} Type:{pointerType}");
                    }
                    break;
            }

            return IntPtr.Zero;
        }

        private void HandlePenMessage(int msg, POINTER_PEN_INFO penInfo)
        {
            const int WM_POINTERDOWN = 0x0246;
            const int WM_POINTERUP = 0x0247;

            NativePoint screenPos = new NativePoint(penInfo.pointerInfo.ptPixelLocation.X, penInfo.pointerInfo.ptPixelLocation.Y);
            Point clientPos = WritingCanvas.PointFromScreen(new Point(screenPos.X, screenPos.Y));

            float pressure = penInfo.pressure / 1024.0f; 
            int tiltX = penInfo.tiltX;
            int tiltY = penInfo.tiltY;

            UpdateStatus(clientPos, pressure, tiltX, tiltY, "Native Pen");

            if (msg == WM_POINTERDOWN)
            {
                _isDrawing = true;
                _lastPoint = clientPos;
                // Capture? In native Win32 usually implied, but we are just drawing.
            }
            else if (msg == WM_POINTERUP)
            {
                _isDrawing = false;
            }
            else // UPDATE
            {
                bool inContact = (penInfo.pointerInfo.pointerFlags & POINTER_FLAG_INCONTACT) != 0;
                
                if (_isDrawing && inContact)
                {
                    DrawLine(_lastPoint, clientPos, pressure);
                    _lastPoint = clientPos;
                }
            }
        }

        private void HandlePointerMessage(int msg, POINTER_INFO pointerInfo, int ptrType)
        {
            const int WM_POINTERDOWN = 0x0246;
            const int WM_POINTERUP = 0x0247;

            NativePoint screenPos = new NativePoint(pointerInfo.ptPixelLocation.X, pointerInfo.ptPixelLocation.Y);
            Point clientPos = WritingCanvas.PointFromScreen(new Point(screenPos.X, screenPos.Y));
            
            float pressure = 1.0f;
            string deviceName = ptrType == 4 ? "Mouse" : (ptrType == 2 ? "Touch" : $"Generic({ptrType})");

            UpdateStatus(clientPos, pressure, 0, 0, deviceName);

            if (msg == WM_POINTERDOWN)
            {
                _isDrawing = true;
                _lastPoint = clientPos;
            }
            else if (msg == WM_POINTERUP)
            {
                _isDrawing = false;
            }
            else
            {
                bool inContact = (pointerInfo.pointerFlags & POINTER_FLAG_INCONTACT) != 0;
                if (_isDrawing && inContact)
                {
                    DrawLine(_lastPoint, clientPos, pressure);
                    _lastPoint = clientPos;
                }
            }
        }

        private unsafe void DrawLine(Point start, Point end, float pressure)
        {
            _bitmap.Lock();

            int x0 = (int)start.X;
            int y0 = (int)start.Y;
            int x1 = (int)end.X;
            int y1 = (int)end.Y;

            // Clamp coordinates
            x0 = Math.Clamp(x0, 0, CanvasWidth - 1);
            y0 = Math.Clamp(y0, 0, CanvasHeight - 1);
            x1 = Math.Clamp(x1, 0, CanvasWidth - 1);
            y1 = Math.Clamp(y1, 0, CanvasHeight - 1);

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            int thickness = (int)(pressure * 5) + 1; // 1 to 6 px thickness

            byte* pBackBuffer = (byte*)_bitmap.BackBuffer;
            int stride = _bitmap.BackBufferStride;

            while (true)
            {
                DrawBrush(pBackBuffer, stride, x0, y0, thickness);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, CanvasWidth, CanvasHeight));
            _bitmap.Unlock();
        }

        private unsafe void DrawBrush(byte* buffer, int stride, int x, int y, int radius) 
        {
             for (int i = -radius/2; i <= radius/2; i++)
             {
                 for (int j = -radius/2; j <= radius/2; j++)
                 {
                     int px = x + i;
                     int py = y + j;

                     if (px >= 0 && px < CanvasWidth && py >= 0 && py < CanvasHeight)
                     {
                        byte* pPixel = buffer + (py * stride) + (px * 4);
                        pPixel[0] = 0;   // Blue
                        pPixel[1] = 0;   // Green
                        pPixel[2] = 0;   // Red
                        pPixel[3] = 255; // Alpha
                     }
                 }
             }
        }

        private void UpdateStatus(Point pos, float pressure, int tiltX, int tiltY, string deviceType)
        {
            StatusText.Text = $"Device: {deviceType} | Pos: {pos.X:F0},{pos.Y:F0} | Press: {pressure:F2} | Tilt: {tiltX},{tiltY}";
        }

        // --- NATIVE INTEROP ---
        
        private const int POINTER_FLAG_INCONTACT = 0x0004;

        private static uint GetPointerId(IntPtr wParam)
        {
            return (uint)(wParam.ToInt64() & 0xFFFF); // LOWORD
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnableMouseInPointer([MarshalAs(UnmanagedType.Bool)] bool fEnable);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPointerPenInfo(uint pointerId, out POINTER_PEN_INFO penInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPointerType(uint pointerId, out int pointerType);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPointerInfo(uint pointerId, out POINTER_INFO pointerInfo);
    }

}
