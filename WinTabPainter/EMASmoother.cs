namespace WinTabPainter
{
    public class EMASmoother
    {
        public double Alpha;
        private double? SmoothingOld;

        public EMASmoother(double alpha)
        {
            this.Alpha = alpha;
            this.SmoothingOld = null;
        }

        public void SetOldSmoothed(double p)
        {
            this.SmoothingOld = p;
        }

        public double Smooth(double value)
        {
            double smoothed_new;
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

        private static double lerp(double oldval, double newval, double alpha)
        {
            double v = (alpha * oldval) + ((1 - alpha) * newval);
            return v;
        }
    }
}
