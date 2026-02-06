namespace WinTabPressureTester
{
    public class ScaleSession
    {
        public SevenUtils.Numerics.MovingAverage logical_pressure_moving_average;

        public ScaleSession()
        {
            this.logical_pressure_moving_average = new SevenUtils.Numerics.MovingAverage(200);

        }

    }
}
