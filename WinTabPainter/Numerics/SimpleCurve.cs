using System;

namespace WinTabPainter.Numerics
{
    public class SimpleCurve
    {
        double BendAmount;
        public SimpleCurve()
        {
            this.SetBendAmount(0.0);
            this.BendAmount = 0.0;
        }

        public void SetBendAmount(double amt)
        {
            this.BendAmount = HelperMethods.ClampRangeDouble(amt, -1, 1);

        }

        public double ApplyCurve(double value)
        {

            double new_value;

            if (this.BendAmount > 0)
            {
                new_value = Math.Pow(value, 1.0 - this.BendAmount);
            }
            else if (this.BendAmount < 0)
            {
                new_value = Math.Pow(value, 1.0 / (1.0 + this.BendAmount));
            }
            else
            {
                new_value = value;
            }

            return new_value;
        }
    }
}
