using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using SevenPaint.GeometryExtensions;

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
        private SevenUtils.Geometry.PointD _lastPoint = new SevenUtils.Geometry.PointD(0, 0);
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
            _wintabInput.ButtonChanged += OnButtonChanged;

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
            var currentPoint = e.GetPosition(RenderImage);
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
            _lastPoint = currentPoint.ToSPPointD();
            
            // Note: Wintab updates this too, but Mouse is smoother for "hover"
            var temp_args = new Stylus.StylusEventArgs
            {
                LocalPos = currentPoint.ToSPPointD(),
                PressureNormalized = 0,
                TiltXYDeg = new SevenUtils.Trigonometry.TiltXY(0, 0),
                TiltAADeg = new SevenUtils.Trigonometry.TiltAA(0, 0),
                Twist = 0,
                ButtonsRaw = buttons,
                HoverDistance = 0
            };

            UpdateRibbon(temp_args);

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




        private void UpdateRibbon(SevenPaint.Stylus.StylusEventArgs args)
        {
            long now = DateTime.Now.Ticks; // 100ns units
            double dt = (now - _lastTime) / 10000.0; // milliseconds

            // Only calc velocity if time passed significantly (e.g. > 5ms) to avoid divide by zero or extreme noise
            if (_lastTime > 0 && dt > 0)
            {
                double dx = args.LocalPos.X - _lastPoint.X;
                double dy = args.LocalPos.Y - _lastPoint.Y;
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
            _lastPoint = args.LocalPos;

            // Update UI (Throttle if needed, but doing it every moved event for "live" feel)
            // Note: Dispatcher.Invoke is implicit if called from UI thread, but Wintab comes from bg thread.
            // We assume this is called inside Dispatcher if from Wintab.
            TxtButtons.Text = $"{args.ButtonsRaw}";
            TxtX.Text = $"{args.LocalPos.X:F1}";
            TxtY.Text = $"{args.LocalPos.Y:F1}";
            TxtVelocity.Text = $"{_lastVelocity:F1}";
            TxtDirection.Text = $"{_lastDirection:F1}";
            TxtHoverDist.Text = $"{args.HoverDistance:F1}";
            TxtPressure.Text = $"{args.PressureNormalized:F4}";
            TxtTiltX.Text = $"{args.TiltXYDeg.X,6:F1}";
            TxtTiltY.Text = $"{args.TiltXYDeg.Y,6:F1}";
            TxtTiltAzimuth.Text = $"{args.TiltAADeg.Azimuth,6:F1}";
            TxtTiltAltitude.Text = $"{args.TiltAADeg.Altitude,6:F1}";
            TxtBarrelRotation.Text = $"{args.Twist,6:F0}";
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
                    scaleFactor = (args.TiltAADeg.Azimuth % 360.0) / 360.0;
                    break;
                case Paint.ScaleType.Altitude:
                    scaleFactor = args.TiltAADeg.Altitude / 90.0;
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

            UpdateRibbon(args);
            
            if (_debugLogWindow != null && _debugLogWindow.IsLoaded)
            {
                if (!_debugLogWindow.OnlyLogDown || args.PressureNormalized > 0)
                {
                    string log = $"{DateTime.Now:HH:mm:ss.fff}: X={args.LocalPos.X:F1} Y={args.LocalPos.Y:F1} P={args.PressureNormalized:F4} TX={args.TiltXYDeg.X:F1} TY={args.TiltXYDeg.Y:F1} Az={args.TiltAADeg.Azimuth:F1} Alt={args.TiltAADeg.Altitude:F1} Tw={args.Twist:F1} Btn={args.ButtonsRaw}";
                    _debugLogWindow.Log(log);
                }
            }
        }
        private void OnButtonChanged(Stylus.StylusButtonEventArgs args)
        {/*
            if (change.ButtonId == WinTabDN.Utils.PenButtonIdentifier.Tip)
            {
                if (change.Change == WinTabDN.Utils.PenButtonPressChangeType.Pressed)
                {
                    // we need to reset the smoothing 
                    // whenever the pen tip touches the tablet
                    // if we don't do this the previous stroke will
                    // have influence on the new stroke
                    paint_settings.Dynamics.PositionSmoother.Reset();
                    paint_settings.Dynamics.PressureSmoother.Reset();
                }
            }*/
            if (_debugLogWindow != null && _debugLogWindow.IsLoaded)
            {
                string action = args.IsPressed ? "Pressed" : "Released";
                string log = $"{DateTime.Now:HH:mm:ss.fff}: Button {args.ButtonName} ({args.ButtonId}) {action}";
                _debugLogWindow.Log(log);
            }
        }
    }
}
