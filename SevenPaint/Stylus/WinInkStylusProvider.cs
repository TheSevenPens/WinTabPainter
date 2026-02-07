using System.Windows;

public static class WinInkExtensions
{
    extension (System.Windows.Input.StylusPoint point)
    {

        public int GetPropertyValueSafe(System.Windows.Input.StylusPointProperty prop, int defval)
        {
            if (point.HasProperty(prop))
            {
                int value = point.GetPropertyValue(prop);
                return value;
            }
            else
            {
                return defval;
            }
        }

    }

}
namespace SevenPaint.Stylus
{
    public class WinInkStylusProvider : IStylusProvider
    {
        private FrameworkElement _targetElement;
        private bool _isActive;

        public event Action<DrawInputArgs>? InputDown;
        public event Action<DrawInputArgs>? InputMove;
        public event Action<DrawInputArgs>? InputUp;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    UpdateSubscriptions();
                }
            }
        }

        public WinInkStylusProvider(FrameworkElement targetElement)
        {
            _targetElement = targetElement;
        }

        public void Open()
        {
            IsActive = true;
        }

        public void Close()
        {
            IsActive = false;
        }

        private void UpdateSubscriptions()
        {
            if (_isActive)
            {
                _targetElement.StylusDown += OnStylusDown;
                _targetElement.StylusMove += OnStylusMove;
                _targetElement.StylusUp += OnStylusUp;
            }
            else
            {
                _targetElement.StylusDown -= OnStylusDown;
                _targetElement.StylusMove -= OnStylusMove;
                _targetElement.StylusUp -= OnStylusUp;
            }
        }

        private void OnStylusDown(object sender, System.Windows.Input.StylusEventArgs e)
        {
            ProcessStylusEvent(e, InputDown);
        }

        private void OnStylusMove(object sender, System.Windows.Input.StylusEventArgs e)
        {
            // Windows Ink can fire move events even when pen is up (hover).
            // We usually only want to paint when pressure > 0 or "InAir" handling.
            // For now, let's fire everything and let consumer check pressure if they want, 
            // OR check InAir status.
            
            if (e.InAir) return; // Ignore hover for painting for now
            
            ProcessStylusEvent(e, InputMove);
        }

        private void OnStylusUp(object sender, System.Windows.Input.StylusEventArgs e)
        {
            ProcessStylusEvent(e, InputUp);
        }

        private void ProcessStylusEvent(System.Windows.Input.StylusEventArgs e, Action<DrawInputArgs>? eventHandler)
        {
            if (eventHandler == null) return;

            var points = e.GetStylusPoints(_targetElement);
            foreach (var p in points)
            {
                // Calculate Tilt/Azimuth/Altitude
                double tiltX_deg = p.GetPropertyValueSafe(System.Windows.Input.StylusPointProperties.XTiltOrientation, 0) / 100.0;
                double tiltY_deg = p.GetPropertyValueSafe(System.Windows.Input.StylusPointProperties.YTiltOrientation, 0) / 100.0;
                
                var tiltXY_deg = new SevenUtils.Trigonometry.TiltXY(tiltX_deg, tiltY_deg);
                var tiltaa_deg = tiltXY_deg.ToAA_deg();

                var args = new DrawInputArgs
                {
                    X = p.X,
                    Y = p.Y,
                    Pressure = p.PressureFactor,
                    TiltX = tiltX_deg,
                    TiltY = tiltY_deg,
                    Azimuth = tiltaa_deg.Azimuth,
                    Altitude = tiltaa_deg.Altitude,

                    // Twist not standard in Ink StylusPoint usually
                    Timestamp = DateTime.Now.Ticks 
                };

                eventHandler(args);
               
            }
        }
    }
}
