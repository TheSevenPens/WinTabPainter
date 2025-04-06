namespace WinTabPressureTester
{
    public class ScaleSession
    {
        public WinTabUtils.Numerics.MovingAverage logical_pressure_moving_average;

        public ScaleSession()
        {
            this.logical_pressure_moving_average = new WinTabUtils.Numerics.MovingAverage(200);

        }

    }
}
