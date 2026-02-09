namespace SevenPaint.Stylus
{
    public struct StylusEventArgs
    {
        public SevenUtils.Geometry.PointD ScreenPos { get; set; }
        public SevenUtils.Geometry.PointD LocalPos { get; set; }
        public double HoverDistance { get; set; } 
        public float PressureLevelRaw { get; set; }
        public float PressureNormalized { get; set; }

        public SevenUtils.Trigonometry.TiltXY TiltXYDeg { get; set; }
        public SevenUtils.Trigonometry.TiltAA TiltAADeg { get; set; }
        public double Twist { get; set; } // Barrel Rotation
        public int PenButtonRaw { get; set; }
        public WinTab.Utils.StylusButtonState ButtonState { get; set; }
        public WinTab.Utils.StylusButtonChange PenButtonChange { get; set; }
        public long Timestamp { get; set; } // Ticks
    }
}
