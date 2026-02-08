namespace SevenPaint.Stylus
{
    public struct StylusEventArgs
    {
        public SevenUtils.Geometry.PointD ScreenPos { get; set; }
        public SevenUtils.Geometry.PointD LocalPos { get; set; }
        public double HoverDistance { get; set; } 
        public float PressureLevelRaw { get; set; }
        public float PressureNormalized { get; set; }
        public double TiltXDeg { get; set; }
        public double TiltYDeg { get; set; }
        public double TiltAzimuthDeg { get; set; }
        public double TiltAltitudeDeg { get; set; }
        public double Twist { get; set; } // Barrel Rotation
        public int ButtonsRaw { get; set; }
        public long Timestamp { get; set; } // Ticks
    }
}
