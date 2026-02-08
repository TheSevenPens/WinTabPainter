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
        private Paint.PixelCanvas _canvas;
        private const int ImageWidth = 1920;
        private const int ImageHeight = 1080;

        private Paint.BrushSettings _brushSettings = new Paint.BrushSettings();

        private Stylus.WinTabStyusProvider _wintabInput;


        // View Manager
        private SevenPaint.View.ViewManager _viewManager;

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
            // Initialize ViewManager
            _viewManager = new SevenPaint.View.ViewManager(MainScrollViewer, CanvasScale);
            _viewManager.ZoomChanged += (s, level) => TxtZoomLevel.Text = $"Zoom: {level}x";

            // Initialize PixelCanvas - 96 DPI
            _canvas = new Paint.PixelCanvas(ImageWidth, ImageHeight, 96.0);
            RenderImage.Source = _canvas.Source;

            // Clear to white
            Clear(Colors.White);

            // Initialize Inputs
            _wintabInput = new Stylus.WinTabStyusProvider(RenderImage);
            _wintabInput.InputMove += OnInputMove;

            // Default to Wintab
            try
            {
                _wintabInput.Open();
                StatusLabel.Text = "Active API: Wintab";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to open Wintab: {ex.Message}");
                StatusLabel.Text = "Active API: Wintab (Failed)";
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _wintabInput?.Close();
            _debugLogWindow?.Close();
        }

        private void ButtonWintabInfo_Click(object sender, RoutedEventArgs e)
        {
            var infoWindow = new WintabInfoWindow();
            infoWindow.Owner = this;
            infoWindow.ShowDialog();
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
                _brushSettings.ScaleType = scaleText switch
                {
                    "Pressure" => Paint.ScaleType.Pressure,
                    "Don't scale" => Paint.ScaleType.None,
                    "Tilt Azimuth" => Paint.ScaleType.Azimuth,
                    "Tilt Altitude" => Paint.ScaleType.Altitude,
                    "Barrel Rotation" => Paint.ScaleType.Rotation,
                    _ => Paint.ScaleType.Pressure,
                };
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

            _viewManager.ProcessKeyDown(e);
        }

        private void Window_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _viewManager.ProcessKeyUp(e);
        }

        // --- ScrollViewer Panning ---
        private void ScrollViewer_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewManager.ProcessPreviewMouseDown(e);
        }

        private void ScrollViewer_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Update Coordinates for UI
            System.Windows.Point currentPoint = e.GetPosition(RenderImage);
            int buttons = 0;
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) buttons |= 1;
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) buttons |= 2;

            // Simple velocity calc
            long now = DateTime.Now.Ticks; // 100ns units
            double dt = (now - _lastTime) / 10000.0; // milliseconds
            
            // Only calc velocity if time passed significantly (e.g. > 5ms)
            if (_lastTime > 0 && dt > 0)
            {
                double dx = currentPoint.X - _lastPoint.X;
                double dy = currentPoint.Y - _lastPoint.Y;
                double dist = Math.Sqrt(dx * dx + dy * dy);
                
                // Velocity in px/s: (dist / dt_ms) * 1000
                double velocity = (dist / dt) * 1000.0;
                
                // Direction (degrees)
                double dirRad = Math.Atan2(dy, dx);
                double dirDeg = dirRad * (180.0 / Math.PI);
                if (dirDeg < 0) dirDeg += 360.0;
                
                _lastVelocity = velocity;
                _lastDirection = dirDeg;
            }

            _lastTime = now;
            _lastPoint = currentPoint;
            
            // Note: Wintab updates this too, but Mouse is smoother for "hover"
            UpdateRibbon(currentPoint, 0, 0, 0, 0, 90, 0, buttons, 0.0);

            _viewManager.ProcessPreviewMouseMove(e);
        }

        private void ScrollViewer_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewManager.ProcessPreviewMouseUp(e);
        }

        private void ButtonResetZoom_Click(object sender, RoutedEventArgs e)
        {
            _viewManager.ResetZoom();
        }

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            _viewManager.ZoomIn();
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            _viewManager.ZoomOut();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
             _viewManager.ProcessPreviewMouseWheel(e);
        }

        // Clear method removed (moved to PixelCanvas)
        private void Clear(System.Windows.Media.Color color)
        {
            _canvas.Clear(color);
        }

        private DebugLogWindow? _debugLogWindow;

        private void ChkLogData_Checked(object sender, RoutedEventArgs e)
        {
            if (_debugLogWindow == null)
            {
                _debugLogWindow = new DebugLogWindow();
                _debugLogWindow.Owner = this;
                _debugLogWindow.Closed += (s, args) =>
                {
                    _debugLogWindow = null;
                    ChkLogData.IsChecked = false;
                };
                _debugLogWindow.Show();
            }
        }

        private void ChkLogData_Unchecked(object sender, RoutedEventArgs e)
        {
            _debugLogWindow?.Close();
            _debugLogWindow = null;
        }

        // Removed SetDebugVisibility as it is no longer needed




        private void UpdateRibbon(System.Windows.Point currentPoint, double pressure, double tiltX, double tiltY, double azimuth, double altitude, double twist, int buttons, double hoverDist)
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
            TxtHoverDist.Text = $"{hoverDist:F1}";
            TxtPressure.Text = $"{pressure:F4}";
            TxtTiltX.Text = $"{tiltX,6:F1}";
            TxtTiltY.Text = $"{tiltY,6:F1}";
            TxtTiltAzimuth.Text = $"{azimuth,6:F1}";
            TxtTiltAltitude.Text = $"{altitude,6:F1}";
            TxtBarrelRotation.Text = $"{twist,6:F0}";
        }
        
        private void OnInputMove(Stylus.StylusEventArgs args)
        {
            if (_viewManager.IsSpaceDown || _viewManager.IsPanning) return;

            double scaleFactor = args.PressureNormalized;

            switch (_brushSettings.ScaleType)
            {
                case Paint.ScaleType.None:
                    scaleFactor = 1.0;
                    break;
                case Paint.ScaleType.Pressure:
                    scaleFactor = args.PressureNormalized;
                    break;
                case Paint.ScaleType.Azimuth:
                    scaleFactor = (args.TiltAzimuthDeg % 360.0) / 360.0;
                    break;
                case Paint.ScaleType.Altitude:
                    scaleFactor = args.TiltAltitudeDeg / 90.0;
                    break;
                case Paint.ScaleType.Rotation:
                    scaleFactor = (args.Twist % 360.0) / 360.0;
                    break;
                default:
                    break;
            }

            if (scaleFactor > 1.0) scaleFactor = 1.0;
            if (scaleFactor < 0.0) scaleFactor = 0.0;

            double radius = _brushSettings.MinRadius + (_brushSettings.MaxRadius - _brushSettings.MinRadius) * scaleFactor;
            if (radius < 0.1) radius = 0.1;

            if (args.PressureNormalized > 0)
            {
                _canvas.DrawDab(args.LocalPos.X, args.LocalPos.Y, radius, _brushSettings.Color);
            }

            UpdateRibbon(new System.Windows.Point(args.LocalPos.X, args.LocalPos.Y), args.PressureNormalized, args.TiltXDeg, args.TiltYDeg, args.TiltAzimuthDeg, args.TiltAltitudeDeg, args.Twist, args.ButtonsRaw, args.HoverDistance);
            
            if (_debugLogWindow != null && _debugLogWindow.IsLoaded)
            {
                if (!_debugLogWindow.OnlyLogDown || args.PressureNormalized > 0)
                {
                    string log = $"{DateTime.Now:HH:mm:ss.fff}: X={args.LocalPos.X:F1} Y={args.LocalPos.Y:F1} P={args.PressureNormalized:F4} TX={args.TiltXDeg:F1} TY={args.TiltYDeg:F1} Az={args.TiltAzimuthDeg:F1} Alt={args.TiltAltitudeDeg:F1} Tw={args.Twist:F1} Btn={args.ButtonsRaw}";
                    _debugLogWindow.Log(log);
                }
            }
        }
    }
}
