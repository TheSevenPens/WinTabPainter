namespace SevenPaint
{
    public struct DrawInputArgs
    {
        public double X { get; set; }
        public double Y { get; set; }
        public float Pressure { get; set; }
        public double TiltX { get; set; }
        public double TiltY { get; set; }
        public double Azimuth { get; set; }
        public double Altitude { get; set; }
        public double Twist { get; set; } // Barrel Rotation
        public int Buttons { get; set; }
        public long Timestamp { get; set; } // Ticks
    }
}
