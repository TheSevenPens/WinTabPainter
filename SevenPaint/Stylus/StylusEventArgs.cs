using SevenLib.WinTab.Stylus;

namespace SevenPaint.Stylus
{
    public struct StylusEventArgs
    {
        public SevenLib.Geometry.PointD ScreenPos { get; set; }
        public SevenLib.Geometry.PointD LocalPos { get; set; }
        public double HoverDistance { get; set; } 
        public float PressureLevelRaw { get; set; }
        public float PressureNormalized { get; set; }

        public SevenLib.Trigonometry.TiltXY TiltXYDeg { get; set; }
        public SevenLib.Trigonometry.TiltAA TiltAADeg { get; set; }
        public double Twist { get; set; } // Barrel Rotation
        public int PenButtonRaw { get; set; }
        public SevenLib.Stylus.StylusButtonState ButtonState { get; set; }
        public StylusButtonChange PenButtonChange { get; set; }
        public long Timestamp { get; set; } // Ticks
    }
}
