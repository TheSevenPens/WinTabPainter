using System;
using System.Windows;
using System.Windows.Input;
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
            InitializeComponent();
            InitializeCanvas();
            HooksEvents();
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

        private void HooksEvents()
        {
            WritingCanvas.MouseDown += OnMouseDown;
            WritingCanvas.MouseMove += OnMouseMove;
            WritingCanvas.MouseUp += OnMouseUp;
            
            // For tablet support, we might want to ensure we're getting raw input messages if possible,
            // or rely on WPF's built-in Stylus events.
            WritingCanvas.StylusDown += OnStylusDown;
            WritingCanvas.StylusMove += OnStylusMove;
            WritingCanvas.StylusUp += OnStylusUp;
        }

        private void OnStylusDown(object sender, StylusDownEventArgs e)
        {
            _isDrawing = true;
            _lastPoint = e.GetPosition(WritingCanvas);
            WritingCanvas.CaptureStylus();
        }

        private void OnStylusMove(object sender, StylusEventArgs e)
        {
            if (!_isDrawing) return;

            var currentPoint = e.GetPosition(WritingCanvas);
            var pressure = e.GetStylusPoints(WritingCanvas)[0].PressureFactor;

            DrawLine(_lastPoint, currentPoint, pressure);
            _lastPoint = currentPoint;
        }

        private void OnStylusUp(object sender, StylusEventArgs e)
        {
            _isDrawing = false;
            WritingCanvas.ReleaseStylusCapture();
        }

        // Fallback for mouse
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null) return; // Handled by Stylus events
            _isDrawing = true;
            _lastPoint = e.GetPosition(WritingCanvas);
            WritingCanvas.CaptureMouse();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice != null) return; // Handled by Stylus events
            if (!_isDrawing) return;

            var currentPoint = e.GetPosition(WritingCanvas);
            DrawLine(_lastPoint, currentPoint, 1.0f); // Default pressure for mouse
            _lastPoint = currentPoint;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null) return; // Handled by Stylus events
            _isDrawing = false;
            WritingCanvas.ReleaseMouseCapture();
        }

        private unsafe void DrawLine(Point start, Point end, float pressure)
        {
            _bitmap.Lock();

            // Simple line drawing (Bresenham's or similar)
            // For simplicity, let's just draw pixels along the line.
            // Pressure affects thickness or color intensity.
            
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

            // Pointer to back buffer
            byte* pBackBuffer = (byte*)_bitmap.BackBuffer;
            int stride = _bitmap.BackBufferStride;

            while (true)
            {
                // Draw a simple brush at (x0, y0)
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
             // Simple square brush for now
             for (int i = -radius/2; i <= radius/2; i++)
             {
                 for (int j = -radius/2; j <= radius/2; j++)
                 {
                     int px = x + i;
                     int py = y + j;

                     if (px >= 0 && px < CanvasWidth && py >= 0 && py < CanvasHeight)
                     {
                        // Calculate pointer
                        byte* pPixel = buffer + (py * stride) + (px * 4);
                        
                        // Set color (Black)
                        pPixel[0] = 0;   // Blue
                        pPixel[1] = 0;   // Green
                        pPixel[2] = 0;   // Red
                        pPixel[3] = 255; // Alpha
                     }
                 }
             }
        }
    }
}
