namespace WinTabPainter.Numerics
{
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

        public static double remap( RangeD from_range, RangeD to_range , double value)
        {
            double t = inverse_lerp(from_range.Lower, from_range.Upper, value);
            double to_value = lerp(to_range.Lower, to_range.Upper, t);
            return to_value;
        }
    }
}
