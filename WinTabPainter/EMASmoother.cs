namespace WinTabPainter
{
    public class EMASmoother
    {
        public double Alpha;
        private Geometry.PointD? SmoothingOld;

        public EMASmoother(double alpha)
        {
            this.Alpha = alpha;
            this.SmoothingOld = null;
        }

        public void SetOldSmoothed(Geometry.PointD p)
        {
            this.SmoothingOld = p;
        }

        public Geometry.PointD Smooth(Geometry.PointD value)
        {
            Geometry.PointD smoothed_new;
            if (this.SmoothingOld.HasValue)
            {
                smoothed_new = EMASmoother.lerp(this.SmoothingOld.Value, value, this.Alpha);
            }
            else
            {
                smoothed_new = value;
            }

            this.SmoothingOld = smoothed_new;
            return smoothed_new;
        }

        private static Geometry.PointD lerp(Geometry.PointD oldval, Geometry.PointD newval, double alpha)
        {
            double newx = (alpha * oldval.X) + ((1 - alpha) * newval.X);
            double newy = (alpha * oldval.Y) + ((1 - alpha) * newval.Y);
            var p = new Geometry.PointD(newx, newy);
            return p;
        }
    }
}
