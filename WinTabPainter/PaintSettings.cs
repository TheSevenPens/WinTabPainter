using WinTabPainter.Geometry;

namespace WinTabPainter
{
    public class PaintSettings
    {
        // brush settings
        public int BrushWidth = 5;
        public int BrushWidthMin = 1;

        // pressure settings
        public SimpleCurve pressure_curve = new SimpleCurve();

        // smoothing settings
        public double PositionSmoothingAmount = 0.0;
        public double PressureSmoothingAmount = 0.0;
        public readonly double SMOOTHING_MIN = 0.0;
        public readonly double SMOOTHING_MAX = 0.99;
        public readonly Numerics.EMAPositionSmoother PositionSmoother;
        public readonly Numerics.EMASmoother PressureSmoother;

        public PaintSettings()
        {
            this.PositionSmoother = new Numerics.EMAPositionSmoother(0);
            this.PressureSmoother = new Numerics.EMASmoother(0);
        }


    }
}
