namespace WinTabPainter
{
    public class PaintSettings
    {
        // brush settings
        public int BrushWidth = 5;
        public int BrushWidthMin = 1;

        // pressure settings

        public double PressureCurveControl = 0.0;

        // smoothing settings
        public double PositionSmoothingAmount = 0.0;
        public readonly double SMOOTHING_MIN = 0.0;
        public readonly double SMOOTHING_MAX = 0.99;
        public EMASmoother Smoother;
    }
}
