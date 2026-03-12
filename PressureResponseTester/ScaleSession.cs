namespace WinTabPressureTester
{
    public class ScaleSession
    {
        private const int DefaultMovingAverageWindowSize = 200;

        public SevenLib.Numerics.MovingAverage LogicalPressureMovingAverage { get; }

        public ScaleSession()
        {
            LogicalPressureMovingAverage = new SevenLib.Numerics.MovingAverage(DefaultMovingAverageWindowSize);
        }
    }
}
