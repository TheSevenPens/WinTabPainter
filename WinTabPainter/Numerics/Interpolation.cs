namespace WinTabPainter.Numerics;

public static class Interpolation
{

    public static double Lerp(double a, double b, double t)
    {
        double v = ((1.0 - t) * a) + (t*b);
        return v;
    }

    public static double InverseLerp(double a, double b, double v)
    {
        double t = (v-a)/(b-a);
        return t;
    }

    public static double Remap( RangeD from_range, RangeD to_range , double from_value)
    {
        double t = InverseLerp(from_range.A, from_range.B, from_value);
        double to_value = Lerp(to_range.A, to_range.B, t);
        return to_value;
    }
}
