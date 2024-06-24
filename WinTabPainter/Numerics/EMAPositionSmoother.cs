using System;

namespace WinTabPainter.Numerics
{
    public class EMAPositionSmoother
    {
        public double Alpha;
        private Geometry.PointD? SmoothingOld;

        public EMAPositionSmoother(double alpha)
        {
            Alpha = alpha;
            SmoothingOld = null;
        }

        public void SetOldSmoothed(Geometry.PointD p)
        {
            SmoothingOld = p;
        }

        public Geometry.PointD Smooth(Geometry.Point value)
        {
            return Smooth(new Geometry.PointD(value.X, value.Y));
        }

        public Geometry.PointD Smooth(Geometry.PointD value)
        {
            Geometry.PointD smoothed_new;
            if (SmoothingOld.HasValue)
            {
                smoothed_new = lerp(SmoothingOld.Value, value, Alpha);
            }
            else
            {
                smoothed_new = value;
            }

            SmoothingOld = smoothed_new;
            return smoothed_new;
        }

        private static Geometry.PointD lerp(Geometry.PointD oldval, Geometry.PointD newval, double alpha)
        {
            double newx = alpha * oldval.X + (1 - alpha) * newval.X;
            double newy = alpha * oldval.Y + (1 - alpha) * newval.Y;
            var p = new Geometry.PointD(newx, newy);
            return p;
        }
    }
}
