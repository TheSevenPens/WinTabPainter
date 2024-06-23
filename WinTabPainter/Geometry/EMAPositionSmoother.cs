using System;

namespace WinTabPainter.Geometry
{
    public class EMAPositionSmoother
    {
        public double Alpha;
        private PointD? SmoothingOld;

        public EMAPositionSmoother(double alpha)
        {
            Alpha = alpha;
            SmoothingOld = null;
        }

        public void SetOldSmoothed(PointD p)
        {
            SmoothingOld = p;
        }

        public PointD Smooth(Point value)
        {
            return Smooth(new PointD(value.X, value.Y));
        }

        public PointD Smooth(PointD value)
        {
            PointD smoothed_new;
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

        private static PointD lerp(PointD oldval, PointD newval, double alpha)
        {
            double newx = alpha * oldval.X + (1 - alpha) * newval.X;
            double newy = alpha * oldval.Y + (1 - alpha) * newval.Y;
            var p = new PointD(newx, newy);
            return p;
        }
    }
}
