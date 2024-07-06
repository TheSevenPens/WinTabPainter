namespace WinTabPainter.Numerics;

public readonly struct OrderedRangeD
{

    public readonly double Lower;
    public readonly double Upper;

    public OrderedRangeD(double lower, double upper)
    {
        if (lower > upper)
        {
            throw new System.ArgumentOutOfRangeException();
        }

        this.Lower = lower;
        this.Upper = upper;
    }

    public double Width => this.Upper - this.Lower;

    public double Clamp(double value) { return this.ClampRangeDouble(value, this.Lower, this.Upper); }

    private double ClampRangeDouble(double value, double min, double max)
    {
        if (value < min) { value = min; }
        else if (value > max) { value = max; }
        else { /* dnothing */}
        return value;
    }
}
