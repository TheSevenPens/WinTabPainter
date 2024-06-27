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
                double new_smoothed_x = Interpolation.Lerp(_old_smoothed_pos.Value.X, value.X, 1.0 - this.SmoothingAmount);
                double new_smoothed_y = Interpolation.Lerp(_old_smoothed_pos.Value.Y, value.Y, 1.0 - this.SmoothingAmount);
                smoothed_new = new Geometry.PointD(new_smoothed_x, new_smoothed_y);
            }
            else
            {
                smoothed_new = value;
            }

            _old_smoothed_pos = smoothed_new;
            return smoothed_new;
        }
    }
}
