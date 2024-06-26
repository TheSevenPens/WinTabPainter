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

        public int Clamp(int value) { return this.ClampRangeInt(value, this.Lower, this.Upper); }

        private int ClampRangeInt(int value, int min, int max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }
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

        public double Clamp(double value) { return this.ClampRangeDouble(value, this.Lower, this.Upper); }

        private double ClampRangeDouble(double value, double min, double max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            else { /* dnothing */}
            return value;
        }
    }
}
