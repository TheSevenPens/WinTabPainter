using System;

namespace WinTabPainter.Numerics
{
    public class SimpleCurve
    {
        double ParamBendAmount;
        public SimpleCurve()
        {
            this.SetBendAmount(0.0);
            this.ParamBendAmount = 0.0;
        }

        public void SetBendAmount(double amt)
        {
            this.ParamBendAmount = HelperMethods.ClampRangeDouble(amt, -1, 1);

        }

        public double ApplyCurve(double value)
        {

            double new_value;

            if (this.ParamBendAmount > 0)
            {
                new_value = Math.Pow(value, 1.0 - this.ParamBendAmount);
            }
            else if (this.ParamBendAmount < 0)
            {
                new_value = Math.Pow(value, 1.0 / (1.0 + this.ParamBendAmount));
            }
            else
            {
                new_value = value;
            }

            return new_value;
        }
    }
}
