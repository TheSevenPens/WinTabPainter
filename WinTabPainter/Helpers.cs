using System;

namespace WinTabPainter
{
    public static class Helpers
    {
        public static double ClampRange(double value, double min, double max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }

        public static double ApplyCurve(double value, double q)
        {
            q = Helpers.ClampRange(q, -1, 1);

            double new_value;

            if (q > 0)
            {
                new_value = Math.Pow(value, 1.0 - q);
            }
            else if (q < 0)
            {
                new_value = Math.Pow(value, 1.0 / (1.0 + q));
            }
            else
            {
                new_value = value;
            }

            return new_value;
        }
    }
}
