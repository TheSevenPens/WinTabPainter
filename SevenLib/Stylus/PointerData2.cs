namespace SevenLib.Stylus;

public struct PointerData2
{
    // Meta
    public DateTime Time { get; set; }

    // Position

    public SevenLib.Geometry.PointD DisplayPoint { get; set; }
    public SevenLib.Geometry.PointD CanvasPoint { get; set; }
    public double Height { get; set; }
    public double PressureNormalized { get; set; }

    // Orientation (Tilt & Twist)
    public SevenLib.Trigonometry.TiltXY TiltXYDeg { get; set; }
    public SevenLib.Trigonometry.TiltAA TiltAADeg { get; set; }
    public double Twist { get; set; } // Barrel Rotation

    // Buttons
    public SevenLib.Stylus.StylusButtonState ButtonState { get; set; }
}