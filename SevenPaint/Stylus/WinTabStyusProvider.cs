using System.Windows;
//
namespace SevenPaint.Stylus
{
    public class WinTabStyusProvider : IStylusProvider
    {
        private WinTabDN.Utils.TabletSession _session;
        private FrameworkElement _targetElement;
        
#pragma warning disable 67
        public event Action<StylusEventArgs>? InputDown; // Wintab packets usually don't distinguish Down/Move easily without logic, but we treat non-zero pressure as active
        public event Action<StylusEventArgs>? InputMove;
        public event Action<StylusEventArgs>? InputUp;
#pragma warning restore 67

        public bool IsActive { get; set; } = false;

        public WinTabStyusProvider(FrameworkElement targetElement)
        {
            _targetElement = targetElement;
            _session = new WinTabDN.Utils.TabletSession();
            _session.PacketHandler = OnWintabPacket;
        }

        public void Open()
        {
            try
            {
                _session.Open(WinTabDN.Utils.TabletContextType.System);
                IsActive = true;
            }
            catch (Exception)
            {
                IsActive = false;
                throw;
            }
        }

        public void Close()
        {
            _session.Close();
            IsActive = false;
        }

        private void OnWintabPacket(WinTabDN.Structs.WintabPacket packet)
        {
            if (!IsActive) return;

            // We need to map coordinates on the UI thread
            _targetElement.Dispatcher.Invoke(() =>
            {
                if (!IsActive) return;

                // WinTab gives us screen coordinates
                // For a "drawing app" so we need to convert them to local coordinates relative to the target element (canvas)
                var localpos = _targetElement.PointFromScreen(new System.Windows.Point(packet.pkX, packet.pkY));

                // Pen Orientation is given in Azimuth (0-360), Altitude (0-90), and Twist (0-360)
                // WinTab gives these values in tenths of degrees, so we divide by 10 to get actual degrees
                double angle_normalization_factor = 10.0;
                double azimuth = packet.pkOrientation.orAzimuth / angle_normalization_factor;
                double altitude = packet.pkOrientation.orAltitude / angle_normalization_factor;
                double twist = packet.pkOrientation.orTwist / angle_normalization_factor;

                // WinTab provides the orientation only in Azimuth/Altitude format
                // For convenience, convert that to XY tilt
                var tiltaa_deg = new SevenUtils.Trigonometry.TiltAA(azimuth, altitude);
                var tiltxy_deg = tiltaa_deg.ToXY_Deg();

                // Create Args
                var args = new StylusEventArgs
                {
                    ScreenPos = new SevenUtils.Geometry.PointD(packet.pkX, packet.pkY),
                    LocalPos = new SevenUtils.Geometry.PointD(localpos.X, localpos.Y),
                    HoverDistance = packet.pkZ,
                    PressureLevelRaw = packet.pkNormalPressure,
                    PressureNormalized = packet.pkNormalPressure / (float)_session.TabletInfo.MaxPressure,
                    TiltAADeg = tiltaa_deg,
                    TiltXYDeg = tiltxy_deg,
                    Twist = twist,
                    ButtonsRaw = (int)packet.pkButtons,
                    Timestamp = packet.pkTime
                };

                InputMove?.Invoke(args);
                
            });
        }
    }
}
