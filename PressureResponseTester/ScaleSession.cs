namespace WinTabPressureTester
{
    public class ScaleSession
    {
        public SevenLib.Numerics.MovingAverage logical_pressure_moving_average;

        public ScaleSession()
        {
            this.logical_pressure_moving_average = new SevenLib.Numerics.MovingAverage(200);

        }

    }
}
