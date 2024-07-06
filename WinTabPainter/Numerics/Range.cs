namespace WinTabPainter.Numerics;

public readonly struct Range
{

    public readonly int Lower;
    public readonly int Upper;

    public Range(int lower, int upper)
    {
        this.Lower = lower;
        this.Upper = upper;
    }

    public int Width => this.Upper - this.Lower;

    public int Clamp(int value) { return this.ClampRangeInt(value, this.Lower, this.Upper); }

    private int ClampRangeInt(int value, int min, int max)
    {
        if (value < min) { value = min; }
        else if (value > max) { value = max; }
        else { /* dnothing */}
        return value;
    }
}
