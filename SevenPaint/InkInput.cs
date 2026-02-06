using System;
using System.Windows;
using System.Windows.Input;

namespace SevenPaint
{
    public class InkInput : IInputProvider
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

        public InkInput(FrameworkElement targetElement)
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

        private void OnStylusDown(object sender, StylusEventArgs e)
        {
            ProcessStylusEvent(e, InputDown);
        }

        private void OnStylusMove(object sender, StylusEventArgs e)
        {
            // Windows Ink can fire move events even when pen is up (hover).
            // We usually only want to paint when pressure > 0 or "InAir" handling.
            // For now, let's fire everything and let consumer check pressure if they want, 
            // OR check InAir status.
            
            if (e.InAir) return; // Ignore hover for painting for now
            
            ProcessStylusEvent(e, InputMove);
        }

        private void OnStylusUp(object sender, StylusEventArgs e)
        {
            ProcessStylusEvent(e, InputUp);
        }

        private void ProcessStylusEvent(StylusEventArgs e, Action<DrawInputArgs>? eventHandler)
        {
            if (eventHandler == null) return;

            var points = e.GetStylusPoints(_targetElement);
            foreach (var p in points)
            {
                // Calculate Tilt/Azimuth/Altitude
                double tiltX = 0;
                double tiltY = 0;
                if (p.HasProperty(StylusPointProperties.XTiltOrientation)) tiltX = p.GetPropertyValue(StylusPointProperties.XTiltOrientation);
                if (p.HasProperty(StylusPointProperties.YTiltOrientation)) tiltY = p.GetPropertyValue(StylusPointProperties.YTiltOrientation);

                double azimuth = 0;
                double altitude = 90;

                if (Math.Abs(tiltX) > 0.1 || Math.Abs(tiltY) > 0.1)
                {
                    double txRad = tiltX * Math.PI / 180.0;
                    double tyRad = tiltY * Math.PI / 180.0;
                    double tanX = Math.Tan(txRad);
                    double tanY = Math.Tan(tyRad);

                    double azRad = Math.Atan2(tanY, tanX);
                    azimuth = azRad * 180.0 / Math.PI;
                    if (azimuth < 0) azimuth += 360.0;

                    double denom = Math.Sqrt(tanX * tanX + tanY * tanY);
                    if (denom > 0.001)
                    {
                        double altRad = Math.Atan(1.0 / denom);
                        altitude = altRad * 180.0 / Math.PI;
                    }
                }

                var args = new DrawInputArgs
                {
                    X = p.X,
                    Y = p.Y,
                    Pressure = p.PressureFactor,
                    TiltX = tiltX,
                    TiltY = tiltY,
                    Azimuth = azimuth,
                    Altitude = altitude,
                    // Twist not standard in Ink StylusPoint usually
                    Timestamp = DateTime.Now.Ticks 
                };

                eventHandler(args);
                
                // Consuming event to prevent system badging/selection?
                // Probably yes.
            }
            // Should we mark Handled? 
            // MainWindow logic was: e.Handled = true
            // But we should allow consumer to decide or do it here.
            // Let's do it here if we assume this class "Owns" the input.
            // But MainWindow might have Panning logic that needs it first?
            // In MainWindow, Panning check happened BEFORE drawing.
            // So logic needs to be: MainWindow checks Panning. If not panning, InkInput processes.
            // But InkInput subscribes directly.
            // We can add a property "ConsumeInput" or Handle bubbles.
            // Or simple: MainWindow logic stays for routing? 
            // In "Refactoring", we want to move logic out.
            // Let's assume InkInput is disabled if Panning? 
            // Or we just handle it.
            
            // To mimic MainWindow:
            // if (_isSpaceDown || _isPanning) return;
            // e.Handled = true;
            // Draw(e);
            
            // So if InkInput is "Active", it should handle it?
            if (IsActive)
            {
                 // e.Handled = true; // Maybe let caller handle? 
            }
        }
    }
}
