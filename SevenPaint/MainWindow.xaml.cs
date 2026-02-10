using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WinTab.Utils;

namespace SevenPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Paint.CanvasDocument _document;
        private const int default_canvas_width = 1920;
        private const int default_canvas_height = 1080;

        private Paint.BrushSettings _brushSettings = new Paint.BrushSettings();

        private Stylus.WinTabStyusProvider _wintabInput;

        // High DPI Support
        private double _dpiScale = 1.0; 

        // View Manager
        private SevenPaint.View.ViewManager _viewManager;

        // Ribbon Tracking
        private SevenUtils.Geometry.PointD _lastPoint = new SevenUtils.Geometry.PointD(0, 0);
        private long _lastTime = 0;
        private double _lastVelocity = 0;
        private double _lastDirection = 0;

        // Debounce tracking
        private long _lastWintabTime = 0;

        // Button State Tracking
        private WinTab.Utils.StylusButtonState _lastButtonState = new StylusButtonState(0);
        private int _lastButtonsRaw = 0;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Get Screen DPI
            var presentationSource = PresentationSource.FromVisual(this);
            if (presentationSource != null)
            {
                _dpiScale = presentationSource.CompositionTarget.TransformToDevice.M11;
            }
            // Fallback or verify
            // DpiScale dpiInfo = VisualTreeHelper.GetDpi(this);
            // _dpiScale = dpiInfo.DpiScaleX;

            // Initialize ViewManager
            _viewManager = new SevenPaint.View.ViewManager(MainScrollViewer, CanvasScale);
            _viewManager.ZoomChanged += (s, level) => TxtZoomLevel.Text = $"Zoom: {level}x";

            // Initialize CanvasDocument - Use actual DPI to match screen pixel density
            double dpi = 96.0 * _dpiScale;
            _document = new Paint.CanvasDocument(default_canvas_width, default_canvas_height, dpi);
            RenderImage.Source = _document.Source;

            // Explicitly size the image control to LOGICAL units so WPF handles layout correctly
            // Width (Logical) = Width (Pixels) / DpiScale
            RenderImage.Width = default_canvas_width / _dpiScale;
            RenderImage.Height = default_canvas_height / _dpiScale;

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
                // StatusLabel.Text = "Active API: Wintab";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to open Wintab: {ex.Message}");
                // StatusLabel.Text = "Active API: Wintab (Failed)";
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

        private void ButtonNewAuto_Click(object sender, RoutedEventArgs e)
        {
            // Calculate available size in Logical Units
            double availW = MainScrollViewer.ViewportWidth;
            double availH = MainScrollViewer.ViewportHeight;

            // Margin
            double margin = 50.0; 
            
            double targetLogicalW = availW - margin;
            double targetLogicalH = availH - margin;

            if (targetLogicalW < 100) targetLogicalW = 100;
            if (targetLogicalH < 100) targetLogicalH = 100;

            // Convert to Physical Pixels
            // We want the image to be this many *physical* pixels so that when displayed at 
            // logical size (Pixels / DpiScale), it matches the targetLogical size.
            int pixelW = (int)(targetLogicalW * _dpiScale);
            int pixelH = (int)(targetLogicalH * _dpiScale);

            // Re-Initialize Document
            // Note: CanvasDocument takes DPI. If we want 1 pixel = 1 screen pixel, we use screen DPI.
            double dpi = 96.0 * _dpiScale;
            _document = new Paint.CanvasDocument(pixelW, pixelH, dpi);
            RenderImage.Source = _document.Source;

            // Explicitly set Logical Size
            RenderImage.Width = targetLogicalW;
            RenderImage.Height = targetLogicalH;

            // Reset Zoom
            _viewManager.ResetZoom();

            // Clear
            Clear(Colors.White);
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete || e.Key == System.Windows.Input.Key.Back)
            {
                _document.Clear(Colors.White);
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
            // Mouse gives Logical Coordinates relative to the Image (thanks to GetPosition(RenderImage))
            var logicalPoint = e.GetPosition(RenderImage);
            
            // Convert to Pixel Coordinates for display
            double pixelX = logicalPoint.X * _dpiScale;
            double pixelY = logicalPoint.Y * _dpiScale;
            var pixelPoint = new SevenUtils.Geometry.PointD(pixelX, pixelY);

            int mouse_buttons = 0;
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) mouse_buttons |= 1;
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) mouse_buttons |= 2;

            // Simple velocity calc
            long now = DateTime.Now.Ticks; // 100ns units
            double dt = (now - _lastTime) / 10000.0; // milliseconds
            
            // Only calc velocity if time passed significantly (e.g. > 5ms)
            if (_lastTime > 0 && dt > 0)
            {
                double dx = pixelPoint.X - _lastPoint.X;
                double dy = pixelPoint.Y - _lastPoint.Y;
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
            _lastPoint = pixelPoint;
            
            // Note: Wintab updates this too, but Mouse is smoother for "hover"
            // However, if we just got a Wintab packet, don't overwrite with mouse zeros
            if (DateTime.Now.Ticks - _lastWintabTime > 1000000) // 100ms
            {
                var temp_args = new Stylus.StylusEventArgs
                {
                    LocalPos = pixelPoint,
                    PressureNormalized = 0,
                    TiltXYDeg = new SevenUtils.Trigonometry.TiltXY(0, 0),
                    TiltAADeg = new SevenUtils.Trigonometry.TiltAA(0, 0),
                    Twist = 0,
                    PenButtonRaw = 0,
                    PenButtonChange = new StylusButtonChange(0),
                    HoverDistance = 0
                };

                UpdateRibbon(temp_args);
            }

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
            _document.Clear(color);
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

        private void UpdateButtonText()
        {
            TxtButtons.Text = $"{_lastButtonState:X}";
        }

        private void UpdateRibbon(SevenPaint.Stylus.StylusEventArgs args)
        {
            long now = DateTime.Now.Ticks; // 100ns units
            double dt = (now - _lastTime) / 10000.0; // milliseconds

            // Only calc velocity if time passed significantly (e.g. > 5ms) to avoid divide by zero or extreme noise
            if (_lastTime > 0 && dt > 0)
            {
                var deltapos = args.LocalPos.Subtract(_lastPoint);
                double dist = Math.Sqrt((deltapos.X * deltapos.X) + (deltapos.Y * deltapos.Y));

                // Velocity in px/s: (dist / dt_ms) * 1000
                double velocity = (dist / dt) * 1000.0;
                double dirRad = Math.Atan2(deltapos.X, deltapos.Y);
                double dirDeg = SevenUtils.Trigonometry.Angles.RadiansToDegrees(dirRad);
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
            _lastButtonsRaw = args.PenButtonRaw;
            UpdateButtonText();
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
            _lastWintabTime = DateTime.Now.Ticks;

            if (_viewManager.IsSpaceDown || _viewManager.IsPanning) return;

            // Convert Logical (LocalPos from StylusProvider) to Pixel
            double pixelX = args.LocalPos.X * _dpiScale;
            double pixelY = args.LocalPos.Y * _dpiScale;
            
            // Create a modified args for internal use (or just use vars)
            // We need to modify args.LocalPos if we pass it around
            // args is a struct, so modifying copy is safe but we need to pass the copy
            var processedArgs = args;
            processedArgs.LocalPos = new SevenUtils.Geometry.PointD(pixelX, pixelY);
            
            double scaleFactor = processedArgs.PressureNormalized;

            switch (_brushSettings.ScaleType)
            {
                case Paint.ScaleType.None:
                    scaleFactor = 1.0;
                    break;
                case Paint.ScaleType.Pressure:
                    scaleFactor = processedArgs.PressureNormalized;
                    break;
                case Paint.ScaleType.Azimuth:
                    scaleFactor = (processedArgs.TiltAADeg.Azimuth % 360.0) / 360.0;
                    break;
                case Paint.ScaleType.Altitude:
                    scaleFactor = processedArgs.TiltAADeg.Altitude / 90.0;
                    break;
                case Paint.ScaleType.Rotation:
                    scaleFactor = (processedArgs.Twist % 360.0) / 360.0;
                    break;
                default:
                    break;
            }

            if (scaleFactor > 1.0) scaleFactor = 1.0;
            if (scaleFactor < 0.0) scaleFactor = 0.0;

            double radius = _brushSettings.MinRadius + (_brushSettings.MaxRadius - _brushSettings.MinRadius) * scaleFactor;
            if (radius < 0.1) radius = 0.1;

            if (processedArgs.PressureNormalized > 0)
            {
                _document.DrawDab(processedArgs.LocalPos.X, processedArgs.LocalPos.Y, radius, _brushSettings.Color);
            }

            UpdateRibbon(processedArgs);
            
            if (_debugLogWindow != null && _debugLogWindow.IsLoaded)
            {
                // Only log if within canvas bounds
                if (_document.Contains(processedArgs.LocalPos.X, processedArgs.LocalPos.Y))
                {
                    if (!_debugLogWindow.OnlyLogDown || processedArgs.PressureNormalized > 0)
                    {
                        string log = $"{DateTime.Now:HH:mm:ss.fff}: X={processedArgs.LocalPos.X:F1} Y={processedArgs.LocalPos.Y:F1} P={processedArgs.PressureNormalized:F4} TX={processedArgs.TiltXYDeg.X:F1} TY={processedArgs.TiltXYDeg.Y:F1} Az={processedArgs.TiltAADeg.Azimuth:F1} Alt={processedArgs.TiltAADeg.Altitude:F1} Tw={processedArgs.Twist:F1} Btn={processedArgs.PenButtonRaw}";
                        _debugLogWindow.Log(log);
                    }
                }
            }
        }
        private void OnButtonChanged(Stylus.StylusButtonEventArgs args)
        {
            _lastButtonState = args.ButtonState;
            UpdateButtonText();

            if (_debugLogWindow != null && _debugLogWindow.IsLoaded)
            {
                string action = args.IsPressed ? "Pressed" : "Released";
                string log = $"{DateTime.Now:HH:mm:ss.fff}: Button {args.ButtonName} ({args.ButtonId}) {action} State={args.ButtonState:X}";
                _debugLogWindow.Log(log);
            }
        }
    }
}
