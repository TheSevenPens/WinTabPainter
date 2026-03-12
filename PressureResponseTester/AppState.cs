using SevenLib.WinTab;

namespace WinTabPressureTester
{
    public class AppState
    {
        private const int DefaultLogicalPressureQueueSize = 400;

        // Sessions with devices
        public WinTabSession? WinTabSession { get; set; }
        public ScaleSession? ScaleSession { get; set; }

        // Pressure readings
        public double PhysicalPressure { get; set; }
        public double LogicalPressure { get; set; }

        // Serial port and scale session management
        public System.IO.Ports.SerialPort? SerialPort { get; set; }
        public CancellationTokenSource? ScaleCts { get; set; }
        public bool ScaleIsReading { get; set; }

        public PressureRecordCollection? RecordCollection { get; set; }
        public int LogicalPressureQueueSize { get; } = DefaultLogicalPressureQueueSize;
        public SevenLib.Numerics.IndexedQueue<double>? QueueLogical { get; set; }
    }
}
