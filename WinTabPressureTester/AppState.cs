using System.Diagnostics;

namespace WinTabPressureTester
{
    public class AppState
    {
        // get rid of this
        public Stopwatch stopwatch;
        // store in a different place? ???
        public double physi_pressure;
        public double log_pressure;
        // Sessions to held with our two datasources
        // the tablet and the scale
        public WinTabUtils.TabletSession wintab_session;
        public ScaleSession scale_session;


        // move to scalesession
        public System.IO.Ports.SerialPort serial_port;


        public CancellationTokenSource scale_cts;
        public bool scale_isReading = false;




        public PressureRecordCollection record_collection;
        public int logical_pressure_queue_size = 400;
        public WinTabUtils.Numerics.IndexedQueue<double> queue_logical;
    }
}
