namespace WinTabPainter.Numerics
{
    public class EMASmoother
    {

        private double _amount;

        public double SmoothingAmount 
        { 
            get => this._amount; 
            set => this._amount = EMASmoother.ClampSmoothingAmount(value); 
        }

        private double? _old_smoothed_value;


        public EMASmoother(double alpha)
        {
            this.SmoothingAmount = alpha;
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

                new_smoothed_value = Interpolation.lerp(_old_smoothed_value.Value, value, 1.0-this.SmoothingAmount);
            }
            else
            {
                new_smoothed_value = value;
            }

            _old_smoothed_value = new_smoothed_value;
            return new_smoothed_value;
        }
        public static double ClampSmoothingAmount(double value)
        {
            double min = 0.0;
            double max = 1.0;
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }
    }


}
