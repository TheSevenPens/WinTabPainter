namespace SevenLib.Numerics;

public readonly struct RangeD
{

    public readonly double A;
    public readonly double B;

    public RangeD(double a, double b)
    {
        this.A = a;
        this.B = b;
    }

    public double Width => this.B - this.A;
}
