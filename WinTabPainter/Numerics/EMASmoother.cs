namespace WinTabPainter.Numerics
{
    public class EMASmoother
    {

        private double _alpha;

        public double Alpha 
        { 
            get => this._alpha; 
            set => this._alpha = this.ClampAlpha(value); 
        }

        private double? _old_smoothed_value;


        public EMASmoother(double alpha)
        {
            this.Alpha = alpha;
            this._old_smoothed_value = null;
        }

        public void SetOldSmoothedValue(double value)
        {
            _old_smoothed_value = value;
        }

        public double Smooth(double value)
        {
            double new_smoothed_value;
            if (_old_smoothed_value.HasValue)
            {

                new_smoothed_value = Interpolation.lerp(_old_smoothed_value.Value, value, this.Alpha);
            }
            else
            {
                new_smoothed_value = value;
            }

            _old_smoothed_value = new_smoothed_value;
            return new_smoothed_value;
        }
        public double ClampAlpha(double value)
        {
            double min = 0.0;
            double max = 1.0;
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }
    }

    public static class Interpolation
    {

        public static double lerp(double a, double b, double t)
        {
            double v = ((1.0 - t) * a) + (t*b);
            return v;
        }

        public static double inverse_lerp(double a, double b, double v)
        {
            double t = (v-a)/(b-a);
            return t;
        }

    }


}
