namespace WinTabPressureTester
{
    public class AppState
    {
        // Sessions with devices
        public SevenLib.WinTab.Tablet.TabletSession wintab_session;
        public ScaleSession scale_session;


        // store in a different place? ???
        public double physi_pressure;
        public double log_pressure;
        
        // move to scalesession
        public System.IO.Ports.SerialPort serial_port;

        public CancellationTokenSource scale_cts;
        public bool scale_isReading = false;

        public PressureRecordCollection record_collection;
        public int logical_pressure_queue_size = 400;
        public SevenLib.Numerics.IndexedQueue<double> queue_logical;


    }


}
