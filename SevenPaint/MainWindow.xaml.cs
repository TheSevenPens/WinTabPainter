using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SevenPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap _wbmp;
        private const int ImageWidth = 1920;
        private const int ImageHeight = 1080;
        private double _maxRadius = 25.0; // Default 50px diameter / 2
        private double _minRadius = 0.0;
        private System.Windows.Media.Color _currentColor = Colors.Black;

        private WinTabUtils.TabletSession? _wintabSession;
        private bool _useWintab = false;
        
        private enum ScaleType { Pressure, None, Azimuth, Altitude, Rotation }
        private ScaleType _scaleType = ScaleType.Pressure;

        // Zoom
        private int _zoomLevel = 1;
        private const int MinZoom = 1;
        private const int MaxZoom = 20;
        
        // Panning
        private bool _isSpaceDown = false;
        private bool _isPanning = false;
        private System.Windows.Point _lastMousePosition;

        // Ribbon Tracking
        private System.Windows.Point _lastPoint = new System.Windows.Point(0, 0);
        private long _lastTime = 0;
        private double _lastVelocity = 0;
        private double _lastDirection = 0;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize WriteableBitmap (Pbgra32) - 96 DPI
            _wbmp = new WriteableBitmap(ImageWidth, ImageHeight, 96, 96, PixelFormats.Pbgra32, null);
            RenderImage.Source = _wbmp;
            
            // Clear to white
            Clear(Colors.White);

            UpdateStatus();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _wintabSession?.Close();
        }

        private void CheckUseWintab_Click(object sender, RoutedEventArgs e)
        {
            _useWintab = CheckUseWintab.IsChecked ?? false;

            if (_useWintab)
            {
                try
                {
                    if (_wintabSession == null)
                    {
                        _wintabSession = new WinTabUtils.TabletSession();
                        _wintabSession.PacketHandler = WintabPacketHandler;
                    }
                    _wintabSession.Open(WinTabUtils.TabletContextType.System);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Failed to open Wintab: {ex.Message}");
                    _useWintab = false;
                    CheckUseWintab.IsChecked = false;
                }
            }
            else
            {
                _wintabSession?.Close();
            }
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            StatusLabel.Text = _useWintab ? "Active API: Wintab" : "Active API: Windows Ink";
        }

        private void ComboColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboColor.SelectedItem is ComboBoxItem item && item.Content is string colorName)
            {
                switch (colorName)
                {
                    case "Red": _currentColor = Colors.Red; break;
                    case "Green": _currentColor = Colors.Green; break;
                    case "Blue": _currentColor = Colors.Blue; break;
                    default: _currentColor = Colors.Black; break;
                }
            }
        }

        private void ComboSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSize.SelectedItem is ComboBoxItem item && item.Content is string sizeText)
            {
                // Format is "50px". Remove "px" and parse.
                string raw = sizeText.Replace("px", "");
                if (double.TryParse(raw, out double size))
                {
                    _maxRadius = size / 2.0;
                    if (_maxRadius < 0.1) _maxRadius = 0.1;
                }
            }
        }

        private void ComboMinSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboMinSize.SelectedItem is ComboBoxItem item && item.Content is string sizeText)
            {
                string raw = sizeText.Replace("px", "");
                if (double.TryParse(raw, out double size))
                {
                    _minRadius = size / 2.0;
                    if (_minRadius < 0) _minRadius = 0;
                }
            }
        }

        private void ComboScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if (ComboScale.SelectedItem is ComboBoxItem item && item.Content is string scaleText)
            {
                switch (scaleText)
                {
                    case "Pressure": _scaleType = ScaleType.Pressure; break;
                    case "Don't scale": _scaleType = ScaleType.None; break;
                    case "Tilt Azimuth": _scaleType = ScaleType.Azimuth; break;
                    case "Tilt Altitude": _scaleType = ScaleType.Altitude; break;
                    case "Barrel Rotation": _scaleType = ScaleType.Rotation; break;
                    default: _scaleType = ScaleType.Pressure; break;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Clear(Colors.White);
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                Clear(Colors.White);
            }
            
            if (e.Key == Key.Space && !_isSpaceDown && !_isPanning)
            {
                _isSpaceDown = true;
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
            }
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                _isSpaceDown = false;
                
                // If we are NOT currently dragging mouse, we can reset cursor immediately
                if (!_isPanning)
                {
                    System.Windows.Input.Mouse.OverrideCursor = null; 
                }
                // If we ARE panning, we usually want to stop panning mode as well, 
                // or at least stop the "Hand" mode. 
                else
                {
                   _isPanning = false;
                   MainScrollViewer.ReleaseMouseCapture();
                   System.Windows.Input.Mouse.OverrideCursor = null; 
                }
            }
        }

        private void ScrollViewer_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Only start pan if space is held AND it's left click
            if (_isSpaceDown && e.ChangedButton == MouseButton.Left)
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(MainScrollViewer);
                MainScrollViewer.CaptureMouse();
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.ScrollAll; // Grabbing look
                e.Handled = true; 
            }
        }

        private void ScrollViewer_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isPanning)
            {
                System.Windows.Point currentPos = e.GetPosition(MainScrollViewer);
                double dx = currentPos.X - _lastMousePosition.X;
                double dy = currentPos.Y - _lastMousePosition.Y;
                
                MainScrollViewer.ScrollToHorizontalOffset(MainScrollViewer.HorizontalOffset - dx);
                MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset - dy);
                
                _lastMousePosition = currentPos;
            }
        }

        private void ScrollViewer_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isPanning && e.ChangedButton == MouseButton.Left)
            {
                _isPanning = false;
                MainScrollViewer.ReleaseMouseCapture();
                // If space is still down, go back to Hand, otherwise normal
                System.Windows.Input.Mouse.OverrideCursor = _isSpaceDown ? System.Windows.Input.Cursors.Hand : null;
            }
        }



        private void PerformZoom(int newZoom, System.Windows.Point? centerOnViewport = null)
        {
            if (newZoom < MinZoom) newZoom = MinZoom;
            if (newZoom > MaxZoom) newZoom = MaxZoom;
            
            if (newZoom == _zoomLevel) return;

            // 1. Determine "center" of zoom in Viewport coordinates
            double viewportW = MainScrollViewer.ViewportWidth;
            double viewportH = MainScrollViewer.ViewportHeight;
            
            System.Windows.Point center = centerOnViewport ?? new System.Windows.Point(viewportW / 2.0, viewportH / 2.0);

            // 2. Find that point in the UN-SCALED content space
            // Current Content Offset + Viewport Center = Point in SCALED content
            // Divide by old zoom to get UN-SCALED
            double oldZoom = _zoomLevel;
            double contentX = (MainScrollViewer.HorizontalOffset + center.X) / oldZoom;
            double contentY = (MainScrollViewer.VerticalOffset + center.Y) / oldZoom;

            // 3. Apply new Zoom
            _zoomLevel = newZoom;
            CanvasScale.ScaleX = _zoomLevel;
            CanvasScale.ScaleY = _zoomLevel;

            if (TxtZoomLevel != null)
            {
                TxtZoomLevel.Text = $"Zoom: {_zoomLevel}x";
            }

            // 4. Update Layout to ensure ScrollViewer knows the new Extent sizes
            MainScrollViewer.UpdateLayout();

            // 5. Calculate new scroll offsets to keep 'contentX/Y' at 'center'
            double newContentX = contentX * _zoomLevel;
            double newContentY = contentY * _zoomLevel;

            double newScrollX = newContentX - center.X;
            double newScrollY = newContentY - center.Y;

            MainScrollViewer.ScrollToHorizontalOffset(newScrollX);
            MainScrollViewer.ScrollToVerticalOffset(newScrollY);
        }

        private void ButtonResetZoom_Click(object sender, RoutedEventArgs e)
        {
            PerformZoom(1);
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            PerformZoom(_zoomLevel + 1);
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            PerformZoom(_zoomLevel - 1);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                int delta = (e.Delta > 0) ? 1 : -1;
                System.Windows.Point mousePos = e.GetPosition(MainScrollViewer);
                
                PerformZoom(_zoomLevel + delta, mousePos);

                e.Handled = true;
            }
        }

        private void Clear(System.Windows.Media.Color color)
        {
            _wbmp.Lock();
            unsafe
            {
                int* pBackBuffer = (int*)_wbmp.BackBuffer;
                int colorData = ConvertColor(color);

                for (int i = 0; i < ImageWidth * ImageHeight; i++)
                {
                    *pBackBuffer++ = colorData;
                }
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
            _wbmp.Unlock();
        }

        private static int ConvertColor(System.Windows.Media.Color color)
        {
            // Pbgra32: A, R, G, B in int memory (Little Endian) for fully opaque
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }



        private void UpdateRibbon(System.Windows.Point currentPoint, double pressure, double tiltX, double tiltY, double azimuth, double altitude, double twist, int buttons)
        {
            long now = DateTime.Now.Ticks; // 100ns units
            double dt = (now - _lastTime) / 10000.0; // milliseconds
            
            // Only calc velocity if time passed significantly (e.g. > 5ms) to avoid divide by zero or extreme noise
            if (_lastTime > 0 && dt > 0)
            {
                double dx = currentPoint.X - _lastPoint.X;
                double dy = currentPoint.Y - _lastPoint.Y;
                double dist = Math.Sqrt(dx * dx + dy * dy);
                
                // Velocity in px/s: (dist / dt_ms) * 1000
                double velocity = (dist / dt) * 1000.0;
                
                // Direction (degrees)
                // Atan2 returns radians. 0 is right (positive X).
                // Let's normalize like standard tools usually do (0-360).
                double dirRad = Math.Atan2(dy, dx);
                double dirDeg = dirRad * (180.0 / Math.PI);
                if (dirDeg < 0) dirDeg += 360.0;
                
                // Simple smoothing could be added, but user asked for raw-ish values
                 _lastVelocity = velocity;
                 _lastDirection = dirDeg;
            }

            _lastTime = now;
            _lastPoint = currentPoint;

            // Update UI (Throttle if needed, but doing it every moved event for "live" feel)
            // Note: Dispatcher.Invoke is implicit if called from UI thread, but Wintab comes from bg thread.
            // We assume this is called inside Dispatcher if from Wintab.
            
            TxtButtons.Text = $"{buttons}";
            TxtX.Text = $"{currentPoint.X:F1}";
            TxtY.Text = $"{currentPoint.Y:F1}";
            TxtVelocity.Text = $"{_lastVelocity:F1}";
            TxtDirection.Text = $"{_lastDirection:F1}";
            
            TxtPressure.Text = $"{pressure:F4}";
            TxtTiltX.Text = $"{tiltX:F1}";
            TxtTiltY.Text = $"{tiltY:F1}";
            TxtTiltAzimuth.Text = $"{azimuth:F1}";
            TxtTiltAltitude.Text = $"{altitude:F1}";
            TxtBarrelRotation.Text = $"{twist:F0}";
        }

        // --- Wintab Handler ---
        private void WintabPacketHandler(WintabDN.Structs.WintabPacket packet)
        {
            if (!_useWintab) return;

            int x = packet.pkX;
            int y = packet.pkY;

            // Pressure: 0 to MaxPressure
            uint pressure = packet.pkNormalPressure;
            if (pressure == 0) return;

            uint maxPressure = (uint)_wintabSession.TabletInfo.MaxPressure;
            float pressureFactor = (float)pressure / maxPressure;

            // Wintab Orientation
            double azimuth = packet.pkOrientation.orAzimuth / 10.0; // Usually units are 0.1 deg? Actually spec says "tenths of degrees" commonly.
            double altitude = packet.pkOrientation.orAltitude / 10.0;
            double twist = packet.pkOrientation.orTwist / 10.0;
            
            // Calculate TiltX/Y from Azimuth/Altitude if possible, or just display Az/Alt
            // TiltX ~ Altitude * Cos(Azimuth)? 
            // Standard conversions are non-trivial without specific context (HID vs Wintab). 
            // Let's just output Az/Alt for now.
             
            Dispatcher.Invoke(() =>
            {
                // Allow drawing outside just for test, or map global -> local
                var visualPoint = RenderImage.PointFromScreen(new System.Windows.Point(x, y));
                
                // Adjust for zoom (PointFromScreen should handle layout transform, but let's verify if manual adjustment is needed)
                // Actually, PointFromScreen on the Image element which has a LayoutTransform parent...
                // The Image itself is inside the scaled Grid. 
                // PointFromScreen returns coordinates relative to the Image's top-left, acknowledging transforms?
                // Visual.PointFromScreen converts "a point in screen coordinates into a Point that represents the current coordinate system of the Visual."
                // So it should already account for the ScaleTransform on the parent Grid.
                // HOWEVER, if the ScrollViewer scrolls, PointFromScreen handles that too.
                // So theoretically, visualPoint is already correct in "Image pixel space".
                // BUT, let's verify if we need to divide by zoom if existing logic expects "Bitmap coordinates".
                // If ScaleTransform is applied via LayoutTransform, the Image's "local" coordinates are still 0..1920, 0..1080?
                // Yes, LayoutTransform scales the element's layout size, but `PointFromScreen` returns the point relative to the element's coordinate space.
                // So if we draw at (10, 10) on screen, and it's zoomed 2x, PointFromScreen should return (5, 5)?
                // Wait. If I click on the top-left pixel, it's (0,0). If I click on pixel 100 drawn at size 200, it should return 100.
                // So `PointFromScreen` DOES handle the transform.
                // So `visualPoint` should be correct WITHOUT manual division if the transform is in the visual tree.
                // Let's assume PointFromScreen works correctly for now.
                
                double scaleFactor = pressureFactor;

                switch (_scaleType)
                {
                    case ScaleType.None:
                        scaleFactor = 1.0;
                        break;
                    case ScaleType.Azimuth:
                        // Azimuth: 0-360 degrees
                        scaleFactor = (azimuth % 360.0) / 360.0;
                        break;
                    case ScaleType.Altitude:
                        // Altitude: 0-90 degrees
                        scaleFactor = Math.Abs(altitude) / 90.0;
                        break;
                    case ScaleType.Rotation:
                        // Twist: 0-360 degrees
                        scaleFactor = (twist % 360.0) / 360.0;
                        break;
                    case ScaleType.Pressure:
                    default:
                        scaleFactor = pressureFactor;
                        break;
                }
                
                if (scaleFactor > 1.0) scaleFactor = 1.0;
                if (scaleFactor < 0.0) scaleFactor = 0.0;

                // Radius calculation with Min Size
                double radius = _minRadius + (_maxRadius - _minRadius) * scaleFactor;
                if (radius < 0.1) radius = 0.1;

                UpdateRibbon(visualPoint, pressureFactor, 0, 0, azimuth, altitude, twist, (int)packet.pkButtons);

                DrawDabCore((int)visualPoint.X, (int)visualPoint.Y, (float)scaleFactor);
            });
        }

        private void DrawDabCore(int x, int y, float pressureFactor)
        {
             if (_wbmp == null) return;
             
            _wbmp.Lock();
            unsafe
            {
                int* pBackBuffer = (int*)_wbmp.BackBuffer;
                int stride = _wbmp.BackBufferStride;

                // Recalculate radius here if needed, but we need the 'scaleFactor' logic again or pass calculated radius
                // NOTE: The previous call passed 'scaleFactor' as 'pressureFactor' argument mistakenly in implementation above? 
                // Ah, DrawDabCore signature is (x, y, pressureFactor). 
                // Let's fix DrawDabCore to take calculated radius or calculate it consistently.
                // Ideally DrawDabCore should just take the final radius logic.
                
                // Let's just duplicate the radius logic for now to avoid changing signature too much or fix it.
                // Actually, let's fix the call site in WintabPacketHandler to pass the radius or move logic here.
                // But wait, the Wintab handler calls DrawDabCore with 'scaleFactor'. 
                // Let's use that 'pressureFactor' argument as the interpolation factor.
                
                double radius = _minRadius + (_maxRadius - _minRadius) * pressureFactor;
                if (radius < 0.1) radius = 0.1;

                DrawDab(pBackBuffer, stride, x, y, radius, _currentColor);
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
            _wbmp.Unlock();
        }

        // --- Windows Ink Handlers ---

        private void RenderImage_StylusDown(object sender, StylusEventArgs e)
        {
            if (_useWintab) return;
            if (_isSpaceDown || _isPanning) return; // Allow bubbling/promotion for Panning
            e.Handled = true;
            Draw(e);
        }

        private void RenderImage_StylusMove(object sender, StylusEventArgs e)
        {
            if (_useWintab) return;
            if (_isSpaceDown || _isPanning) return;
            Draw(e);
        }

        private void RenderImage_StylusUp(object sender, StylusEventArgs e)
        {
            if (_useWintab) return;
            if (_isSpaceDown || _isPanning) return;
            e.Handled = true;
            Draw(e);
        }

        private void Draw(StylusEventArgs e)
        {
            if (_wbmp == null) return;

            // Update Ribbon with first point data (sufficient for high freq)
            var stylusPoints = e.GetStylusPoints(RenderImage);
            if (stylusPoints.Count > 0)
            {
                var p = stylusPoints[stylusPoints.Count - 1]; // Use latest point
                // Extract properties
                // TiltX/Y are potentially available but assume 0 if not
                // StylusPoint has properties like PressureFactor.
                // Standard properties:
                // Guid StylusPointProperties.TiltX
                
                double tiltX = 0;
                double tiltY = 0;
                // Need to use StylusPointDescription to check index?
                if (p.HasProperty(StylusPointProperties.XTiltOrientation)) tiltX = p.GetPropertyValue(StylusPointProperties.XTiltOrientation);
                if (p.HasProperty(StylusPointProperties.YTiltOrientation)) tiltY = p.GetPropertyValue(StylusPointProperties.YTiltOrientation);
                
                // Calculate Azimuth/Altitude from Tilt X/Y
                double azimuth = 0;
                double altitude = 90;

                // Only calculate if there is some tilt (otherwise vertical 90)
                if (Math.Abs(tiltX) > 0.1 || Math.Abs(tiltY) > 0.1)
                {
                    double txRad = tiltX * Math.PI / 180.0;
                    double tyRad = tiltY * Math.PI / 180.0;
                    
                    double tanX = Math.Tan(txRad);
                    double tanY = Math.Tan(tyRad);
                    
                    // Azimuth
                    double azRad = Math.Atan2(tanY, tanX);
                    azimuth = azRad * 180.0 / Math.PI;
                    if (azimuth < 0) azimuth += 360.0;
                    
                    // Altitude
                    // Angle from surface. 
                    // Vector is (tanX, tanY, 1). Angle with Z (vertical) is theta. Alt is 90 - theta.
                    // Or simple conversion used in HID:
                    // Altitude = Atan( 1 / Sqrt(tanX^2 + tanY^2) )
                    double denom = Math.Sqrt(tanX * tanX + tanY * tanY);
                    if (denom > 0.001)
                    {
                        double altRad = Math.Atan(1.0 / denom);
                        altitude = altRad * 180.0 / Math.PI;
                    }
                    else
                    {
                        altitude = 90.0;
                    }
                }
                
                UpdateRibbon(new System.Windows.Point(p.X, p.Y), p.PressureFactor, tiltX, tiltY, azimuth, altitude, 0, 0);
            }

            _wbmp.Lock();
            unsafe
            {
                int* pBackBuffer = (int*)_wbmp.BackBuffer;
                int stride = _wbmp.BackBufferStride;

                foreach (var point in stylusPoints)
                {
                    int x = (int)point.X;
                    int y = (int)point.Y;
                    float pressure = point.PressureFactor;
                    float scaleFactor = pressure;

                    // If we need per-point azimuth/altitude, we'd need to re-calc here or fetch properties
                    // For now, let's look for standard properties on the point if _scaleType requires it
                    // Or lazily, use the values computed from the last point above for the whole stroke chunk?
                    // "stylusPoints" is a small chunk of moves. Sharing the 'latest' Az/Alt is a reasonable approximation for this chunk.
                    // Let's refine:
                    double ptAzimuth = 0;
                    double ptAltitude = 90;
                    
                    // Optimization: if scaling by Az/Alt, try to compute for this point or use cached
                    if (_scaleType == ScaleType.Azimuth || _scaleType == ScaleType.Altitude)
                    {
                         // Redo extraction just to be safe if point varies?
                         // StylusPoint properties access is fast?
                         double pTx = 0, pTy = 0;
                         if (point.HasProperty(StylusPointProperties.XTiltOrientation)) pTx = point.GetPropertyValue(StylusPointProperties.XTiltOrientation);
                         if (point.HasProperty(StylusPointProperties.YTiltOrientation)) pTy = point.GetPropertyValue(StylusPointProperties.YTiltOrientation);
                         
                         if (Math.Abs(pTx) > 0.1 || Math.Abs(pTy) > 0.1)
                         {
                            double txR = pTx * Math.PI / 180.0;
                            double tyR = pTy * Math.PI / 180.0;
                            double tX = Math.Tan(txR);
                            double tY = Math.Tan(tyR);
                            if (_scaleType == ScaleType.Azimuth) 
                            {
                                 double az = Math.Atan2(tY, tX) * 180.0 / Math.PI;
                                 if (az < 0) az += 360.0;
                                 ptAzimuth = az;
                            }
                            if (_scaleType == ScaleType.Altitude)
                            {
                                double d = Math.Sqrt(tX*tX + tY*tY);
                                if (d > 0.001) ptAltitude = Math.Atan(1.0/d) * 180.0 / Math.PI;
                            }
                         }
                    }

                    switch (_scaleType)
                    {
                         case ScaleType.None:
                            scaleFactor = 1.0f;
                            break;
                         case ScaleType.Pressure:
                            scaleFactor = pressure;
                            break;
                         case ScaleType.Azimuth:
                            scaleFactor = (float)(ptAzimuth / 360.0);
                            break;
                         case ScaleType.Altitude:
                            scaleFactor = (float)(ptAltitude / 90.0);
                            break;
                         case ScaleType.Rotation:
                            scaleFactor = 0.5f; // Rotation rarely on StylusPoint without Wintab
                            break;
                        default:
                             break;
                    }
                    
                    if (scaleFactor > 1.0f) scaleFactor = 1.0f;
                    if (scaleFactor < 0.0f) scaleFactor = 0.0f;

                    double radius = _minRadius + (_maxRadius - _minRadius) * scaleFactor;
                    if (radius < 0.1) radius = 0.1;

                    DrawDab(pBackBuffer, stride, point.X, point.Y, radius, _currentColor);
                }
            }
            _wbmp.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
            _wbmp.Unlock();
        }

        private unsafe void DrawDab(int* buffer, int stride, double cx, double cy, double radius, System.Windows.Media.Color color)
        {
            // Bounding box
            int minX = (int)Math.Floor(cx - radius - 1);
            int maxX = (int)Math.Ceiling(cx + radius + 1);
            int minY = (int)Math.Floor(cy - radius - 1);
            int maxY = (int)Math.Ceiling(cy + radius + 1);

            // Clamp to image bounds
            minX = Math.Max(0, minX);
            maxX = Math.Min(ImageWidth - 1, maxX);
            minY = Math.Max(0, minY);
            maxY = Math.Min(ImageHeight - 1, maxY);

            // Pre-calculate color components (assuming opaque input color for now, or handle alpha)
            // Windows allows semi-transparent colors, so let's handle input alpha too
            double srcA = color.A / 255.0;
            double srcR = color.R * srcA; // Premultiplied source
            double srcG = color.G * srcA;
            double srcB = color.B * srcA;

            double radiusSq = radius * radius;
            double radiusInner = radius - 1.0;
            if (radiusInner < 0) radiusInner = 0;
            double radiusInnerSq = radiusInner * radiusInner;

            for (int y = minY; y <= maxY; y++)
            {
                // Optimization: pointer to row start
                // stride is in bytes. int* arithmetic moves by 4 bytes. 
                // buffer is int*, so we need strict byte offset logic or assume stride is multiple of 4 (usually is for Pbgra32)
                // Existing code used: int* row = buffer + (y * (stride / 4));
                int* row = buffer + (y * (stride / 4));

                double dy = y - cy;
                double dy2 = dy * dy;

                for (int x = minX; x <= maxX; x++)
                {
                    double dx = x - cx;
                    double distSq = dx * dx + dy2;

                    if (distSq >= (radius + 1) * (radius + 1)) continue; // Optimization: far outside

                    double alphaFactor = 0.0;
                    double dist = Math.Sqrt(distSq);

                    if (dist < radiusInner)
                    {
                        // Fully inside inner radius -> use full source alpha
                        alphaFactor = 1.0;
                    }
                    else if (dist < radius)
                    {
                        // On edge -> antialias
                        // Linear falloff: 1.0 at radiusInner, 0.0 at radius
                        alphaFactor = 1.0 - (dist - radiusInner); // Since radius - radiusInner = 1 (usually)
                    }
                    else
                    {
                        // Just outside radius but inside bounding box (sub-pixel coverage?)
                        // We can be a bit softer: range [radius-0.5, radius+0.5]?
                        // Let's stick to [radius-1, radius] for now aka standard 1px feather
                         alphaFactor = 0.0;
                    }

                    if (alphaFactor > 0)
                    {
                        // Calculate effective source color
                        double coverage = alphaFactor;
                        
                        // Current Destination Pixel
                        int destPixel = row[x];
                        
                        // Extract Dest components (Pbgra32: B G R A)
                        byte dA = (byte)((destPixel >> 24) & 0xFF);
                        byte dR = (byte)((destPixel >> 16) & 0xFF);
                        byte dG = (byte)((destPixel >> 8) & 0xFF);
                        byte dB = (byte)(destPixel & 0xFF);

                        // Blend
                        // Out = Src * Cov + Dest * (1 - SrcA * Cov)
                        
                        // Effective source alpha for this pixel
                        double outSrcA = srcA * coverage;
                        
                        // Premultiplied Source components for this pixel
                        double pSrcR = srcR * coverage;
                        double pSrcG = srcG * coverage;
                        double pSrcB = srcB * coverage;
                        double pSrcA = color.A * coverage; // = outSrcA * 255.0

                        // Destination blend factor
                        double destFactor = 1.0 - outSrcA;

                        double rA = pSrcA + dA * destFactor;
                        double rR = pSrcR + dR * destFactor;
                        double rG = pSrcG + dG * destFactor;
                        double rB = pSrcB + dB * destFactor;

                        // Clamp and Pack
                        byte fA = (byte)Math.Min(255, Math.Max(0, rA));
                        byte fR = (byte)Math.Min(255, Math.Max(0, rR));
                        byte fG = (byte)Math.Min(255, Math.Max(0, rG));
                        byte fB = (byte)Math.Min(255, Math.Max(0, rB));

                        row[x] = (fA << 24) | (fR << 16) | (fG << 8) | fB;
                    }
                }
            }
        }
    }
}