namespace SevenUtils.Numerics;

public readonly struct OrderedRange
{

    public readonly int Lower;
    public readonly int Upper;

    public OrderedRange(int lower, int upper)
    {
        if (lower > upper)
        {
            throw new System.ArgumentOutOfRangeException();
        }

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
