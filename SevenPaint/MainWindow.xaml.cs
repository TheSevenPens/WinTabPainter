using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SevenPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PixelCanvas _canvas;
        private const int ImageWidth = 1920;
        private const int ImageHeight = 1080;

        private BrushSettings _brushSettings = new BrushSettings();

        private Stylus.WinTabStyusProvider _wintabInput;
        private Stylus.WinInkStylusProvider _inkInput;
        private bool _useWintab = false;

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
            // Initialize PixelCanvas - 96 DPI
            _canvas = new PixelCanvas(ImageWidth, ImageHeight, 96.0);
            RenderImage.Source = _canvas.Source;

            // Clear to white
            Clear(Colors.White);

            // Initialize Inputs
            _wintabInput = new Stylus.WinTabStyusProvider(RenderImage);
            _wintabInput.InputMove += OnInputMove;

            _inkInput = new Stylus.WinInkStylusProvider(RenderImage);
            _inkInput.InputMove += OnInputMove;
            _inkInput.InputDown += OnInputMove; // Treat Down as Move for painting

            // Default to Ink
            _inkInput.Open();

            UpdateStatus();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _wintabInput?.Close();
            _inkInput?.Close();
        }

        private void CheckUseWintab_Click(object sender, RoutedEventArgs e)
        {
            _useWintab = CheckUseWintab.IsChecked ?? false;

            if (_useWintab)
            {
                try
                {
                    _inkInput.Close();
                    _wintabInput.Open();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Failed to open Wintab: {ex.Message}");
                    _useWintab = false;
                    CheckUseWintab.IsChecked = false;
                    _wintabInput.Close();
                    _inkInput.Open();
                }
            }
            else
            {
                _wintabInput.Close();
                _inkInput.Open();
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
                    case "Red": _brushSettings.Color = Colors.Red; break;
                    case "Green": _brushSettings.Color = Colors.Green; break;
                    case "Blue": _brushSettings.Color = Colors.Blue; break;
                    default: _brushSettings.Color = Colors.Black; break;
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
                    _brushSettings.MaxRadius = size / 2.0;
                    if (_brushSettings.MaxRadius < 0.1) _brushSettings.MaxRadius = 0.1;
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
                    _brushSettings.MinRadius = size / 2.0;
                    if (_brushSettings.MinRadius < 0) _brushSettings.MinRadius = 0;
                }
            }
        }

        private void ComboScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboScale.SelectedItem is ComboBoxItem item && item.Content is string scaleText)
            {
                switch (scaleText)
                {
                    case "Pressure": _brushSettings.ScaleType = ScaleType.Pressure; break;
                    case "Don't scale": _brushSettings.ScaleType = ScaleType.None; break;
                    case "Tilt Azimuth": _brushSettings.ScaleType = ScaleType.Azimuth; break;
                    case "Tilt Altitude": _brushSettings.ScaleType = ScaleType.Altitude; break;
                    case "Barrel Rotation": _brushSettings.ScaleType = ScaleType.Rotation; break;
                    default: _brushSettings.ScaleType = ScaleType.Pressure; break;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Clear(Colors.White);
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete || e.Key == System.Windows.Input.Key.Back)
            {
                _canvas.Clear(Colors.White);
            }

            if (e.Key == System.Windows.Input.Key.Space && !_isSpaceDown && !_isPanning)
            {
                _isSpaceDown = true;
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
            }
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
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
            if (_isSpaceDown && e.ChangedButton == System.Windows.Input.MouseButton.Left)
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
            if (_isPanning && e.ChangedButton == System.Windows.Input.MouseButton.Left)
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

        // Clear method removed (moved to PixelCanvas)
        private void Clear(System.Windows.Media.Color color)
        {
            _canvas.Clear(color);
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

        private void OnInputMove(Stylus.DrawInputArgs args)
        {
            if (_isSpaceDown || _isPanning) return;

            double scaleFactor = args.Pressure;

            switch (_brushSettings.ScaleType)
            {
                case ScaleType.None:
                    scaleFactor = 1.0;
                    break;
                case ScaleType.Pressure:
                    scaleFactor = args.Pressure;
                    break;
                case ScaleType.Azimuth:
                    scaleFactor = (args.Azimuth % 360.0) / 360.0;
                    break;
                case ScaleType.Altitude:
                    scaleFactor = args.Altitude / 90.0;
                    break;
                case ScaleType.Rotation:
                    scaleFactor = (args.Twist % 360.0) / 360.0;
                    break;
                default:
                    break;
            }

            if (scaleFactor > 1.0) scaleFactor = 1.0;
            if (scaleFactor < 0.0) scaleFactor = 0.0;

            double radius = _brushSettings.MinRadius + (_brushSettings.MaxRadius - _brushSettings.MinRadius) * scaleFactor;
            if (radius < 0.1) radius = 0.1;

            _canvas.DrawDab(args.X, args.Y, radius, _brushSettings.Color);

            UpdateRibbon(new System.Windows.Point(args.X, args.Y), args.Pressure, args.TiltX, args.TiltY, args.Azimuth, args.Altitude, args.Twist, args.Buttons);
        }
    }
}
