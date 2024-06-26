namespace WinTabPainter.Numerics
{
    public struct RangeD
    {

        public readonly double Lower;
        public readonly double Upper;

        public RangeD(double lower, double upper)
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
