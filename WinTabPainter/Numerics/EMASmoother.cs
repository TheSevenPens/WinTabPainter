namespace WinTabPainter.Numerics
{
    public class EMASmoother
    {
        public double Alpha;
        private double? SmoothingOld;

        public EMASmoother(double alpha)
        {
            Alpha = alpha;
            SmoothingOld = null;
        }

        public void SetOldSmoothed(double p)
        {
            SmoothingOld = p;
        }

        public double Smooth(double value)
        {
            double smoothed_new;
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

        private static double lerp(double oldval, double newval, double alpha)
        {
            double v = alpha * oldval + (1 - alpha) * newval;
            return v;
        }
    }
}
