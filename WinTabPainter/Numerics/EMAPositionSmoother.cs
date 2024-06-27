using System;

namespace WinTabPainter.Numerics
{
    public class EMAPositionSmoother
    {
        private double _amount;
        public double SmoothingAmount
        {
            get => this._amount;
            set => this._amount = EMASmoother.ClampSmoothingAmount(value);
        }
        private Geometry.PointD? _old_smoothed_pos;

        public EMAPositionSmoother(double amount)
        {
            this.SmoothingAmount = amount;
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
                smoothed_new = lerp(_old_smoothed_pos.Value, value, SmoothingAmount);
            }
            else
            {
                smoothed_new = value;
            }

            _old_smoothed_pos = smoothed_new;
            return smoothed_new;
        }

        private static Geometry.PointD lerp(Geometry.PointD oldval, Geometry.PointD newval, double smoothing_amount)
        {
            double newx = smoothing_amount * oldval.X + (1 - smoothing_amount) * newval.X;
            double newy = smoothing_amount * oldval.Y + (1 - smoothing_amount) * newval.Y;
            var p = new Geometry.PointD(newx, newy);
            return p;
        }

    }
}
