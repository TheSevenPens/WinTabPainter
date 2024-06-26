using WinTabPainter.Geometry;

namespace WinTabPainter.Painting
{
    public class PaintSettings
    {
        // brush settings
        public int BrushWidth = 5;
        public int BrushWidthMin = 1;

        // pressure settings
        public Numerics.SimpleCurve pressure_curve = new Numerics.SimpleCurve();

        // smoothing settings
        public double PositionSmoothingAmount = 0.0;
        public double PressureSmoothingAmount = 0.0;
        public readonly Numerics.EMAPositionSmoother PositionSmoother;
        public readonly Numerics.EMASmoother PressureSmoother;

        public static readonly Numerics.ValueRangeDouble SMOOTHING_RANGE = new Numerics.ValueRangeDouble(0.0, 0.99);

        public PaintSettings()
        {
            this.PositionSmoother = new Numerics.EMAPositionSmoother(0);
            this.PressureSmoother = new Numerics.EMASmoother(0);
        }


    }
}
