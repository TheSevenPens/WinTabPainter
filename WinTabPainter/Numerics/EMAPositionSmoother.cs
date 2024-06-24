using System;

namespace WinTabPainter.Numerics
{
    public class EMAPositionSmoother
    {
        private double _alpha;
        public double Alpha
        {
            get => this._alpha;
            set => this._alpha = this._clamp_alpha(value);
        }
        private Geometry.PointD? _old_smoothed_pos;

        public EMAPositionSmoother(double alpha)
        {
            this.Alpha = alpha;
            this._old_smoothed_pos = null;
        }

        public void SetOldSmoothedValue(Geometry.PointD p)
        {
            this._old_smoothed_pos = p;
        }

        public Geometry.PointD Smooth(Geometry.Point value)
        {
            return this.Smooth(new Geometry.PointD(value.X, value.Y));
        }

        public Geometry.PointD Smooth(Geometry.PointD value)
        {
            Geometry.PointD smoothed_new;
            if (_old_smoothed_pos.HasValue)
            {
                smoothed_new = lerp(_old_smoothed_pos.Value, value, Alpha);
            }
            else
            {
                smoothed_new = value;
            }

            _old_smoothed_pos = smoothed_new;
            return smoothed_new;
        }

        private static Geometry.PointD lerp(Geometry.PointD oldval, Geometry.PointD newval, double alpha)
        {
            double newx = alpha * oldval.X + (1 - alpha) * newval.X;
            double newy = alpha * oldval.Y + (1 - alpha) * newval.Y;
            var p = new Geometry.PointD(newx, newy);
            return p;
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
