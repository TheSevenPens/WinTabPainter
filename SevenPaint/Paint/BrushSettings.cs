namespace SevenPaint.Paint
{
    public enum ColorMode { Fixed, Pressure, Azimuth, Altitude }

    public class BrushSettings
    {
        public System.Windows.Media.Color Color { get; set; } = System.Windows.Media.Colors.Black;
        public ColorMode ColorMode { get; set; } = ColorMode.Fixed;
        public double MaxRadius { get; set; } = 25.0; // Default 50px diameter / 2
        public double MinRadius { get; set; } = 0.0;
        public ScaleType ScaleType { get; set; } = ScaleType.Pressure;
    }
}
