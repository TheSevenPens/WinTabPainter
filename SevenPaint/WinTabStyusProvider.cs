using System.Windows;

namespace SevenPaint
{
    public class WinTabStyusProvider : IStylusProvider
    {
        private WinTabDN.Utils.TabletSession _session;
        private FrameworkElement _targetElement;
        
        public event Action<DrawInputArgs>? InputDown; // Wintab packets usually don't distinguish Down/Move easily without logic, but we treat non-zero pressure as active
        public event Action<DrawInputArgs>? InputMove;
        public event Action<DrawInputArgs>? InputUp;

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

            // Basic filtering
            if (packet.pkNormalPressure == 0) 
            {
                 // Could fire Up if we tracked state
                 return; 
            }

            // We need to map coordinates on the UI thread
            _targetElement.Dispatcher.Invoke(() =>
            {
                if (!IsActive) return;

                // Map Screen -> Local
                System.Windows.Point p = _targetElement.PointFromScreen(new System.Windows.Point(packet.pkX, packet.pkY));

                // Normalize Pressure
                float pressure = packet.pkNormalPressure / (float)_session.TabletInfo.MaxPressure;
                
                // Tilt/Orientation
                double azimuth = packet.pkOrientation.orAzimuth / 10.0;
                double altitude = packet.pkOrientation.orAltitude / 10.0;
                double twist = packet.pkOrientation.orTwist / 10.0;

                // Create Args
                var args = new DrawInputArgs
                {
                    X = p.X,
                    Y = p.Y,
                    Pressure = pressure,
                    Azimuth = azimuth,
                    Altitude = altitude,
                    Twist = twist,
                    Buttons = (int)packet.pkButtons,
                    Timestamp = packet.pkTime
                };

                // Fire Move (treating all pressure > 0 as move/draw)
                InputMove?.Invoke(args);
                
                // TODO: Logic for Down/Up? 
                // Creating a state machine here (wasPressure0 -> >0 = Down) might be better, 
                // but for now keeping it simple as a stream of paint events.
            });
        }
    }
}
