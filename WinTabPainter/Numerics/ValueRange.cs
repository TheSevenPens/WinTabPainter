namespace WinTabPainter.Numerics
{
    public struct ValueRangeInt
    {

        public readonly int Lower;
        public readonly int Upper;

        public ValueRangeInt(int lower, int upper)
        {
            this.Lower = lower;
            this.Upper = upper;
        }

        public int Width => this.Upper - this.Lower;

        public double Clamp(int value) { return HelperMethods.ClampRangeInt(value, this.Lower, this.Upper); }

    }

    public struct ValueRangeDouble
    {

        public readonly double Lower;
        public readonly double Upper;

        public ValueRangeDouble(double lower, double upper)
        {
            this.Lower = lower;
            this.Upper = upper;
        }

        public double Width => this.Upper - this.Lower;

        public double Clamp(double value) { return HelperMethods.ClampRangeDouble(value, this.Lower, this.Upper); }
    }
}
