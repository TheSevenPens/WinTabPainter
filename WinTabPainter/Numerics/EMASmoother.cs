﻿namespace WinTabPainter.Numerics
{
    public class EMASmoother
    {

        private double _alpha;

        public double Alpha 
        { 
            get => this._alpha; 
            set => this._alpha = this._clamp_alpha(value); 
        }

        private double? _old_smoothed_pos;


        public EMASmoother(double alpha)
        {
            this.Alpha = alpha;
            this._old_smoothed_pos = null;
        }

        private void _set_alpha(double aalpha)
        {

        }



        public void SetOldSmoothed(double value)
        {
            _old_smoothed_pos = value;
        }

        public double Smooth(double value)
        {
            double new_smoothed_value;
            if (_old_smoothed_pos.HasValue)
            {
                new_smoothed_value = lerp(_old_smoothed_pos.Value, value, Alpha);
            }
            else
            {
                new_smoothed_value = value;
            }

            _old_smoothed_pos = new_smoothed_value;
            return new_smoothed_value;
        }

        private static double lerp(double oldval, double newval, double alpha)
        {
            double v = alpha * oldval + (1 - alpha) * newval;
            return v;
        }

        private double _clamp_alpha(double value)
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
