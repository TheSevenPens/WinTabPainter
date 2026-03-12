namespace WinTabPressureTester
{
    public record class ScaleSession
    {
        private const int DefaultMovingAverageWindowSize = 200;

        public SevenLib.Numerics.MovingAverage LogicalPressureMovingAverage { get; init; }

        public ScaleSession()
        {
            LogicalPressureMovingAverage = new SevenLib.Numerics.MovingAverage(DefaultMovingAverageWindowSize);
        }
    }
}
