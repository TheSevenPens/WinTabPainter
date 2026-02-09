namespace SevenUtils.Trigonometry
{
    public readonly struct TiltAA 
    {
        public readonly double Azimuth; 
        public readonly double Altitude; 

        public TiltAA(double azimuth, double altitude)
        {
            Azimuth = azimuth;
            Altitude = altitude;
        }

        public TiltAA ToDegrees()
        {
            return new TiltAA(Angles.RadiansToDegrees(Azimuth), Angles.RadiansToDegrees(Altitude));
        }
        public TiltAA ToRadians()
        {
                return new TiltAA(Angles.DegreesToRadians(Azimuth), Angles.DegreesToRadians(Altitude));
        }

        public TiltXY ToXY_Rad()
        {
            double tanAlt = System.Math.Tan(System.Math.Abs(this.Altitude));
            double x_rad = System.Math.Atan(System.Math.Sin(this.Azimuth) / tanAlt);
            double y_rad = System.Math.Atan(System.Math.Cos(this.Azimuth) / tanAlt);
            var xy_rad = new TiltXY(x_rad, y_rad);
            return xy_rad;
        }

        public TiltXY ToXY_Deg()
        {
            var XY_rad = this.ToRadians().ToXY_Rad();
            var XY_deg = new TiltXY(XY_rad.X, XY_rad.Y); // Need to revisit why this is done
            return XY_deg.ToDegrees();
        }
    }
}
