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
            double radX = System.Math.Atan(System.Math.Sin(this.Azimuth) / tanAlt);
            double radY = System.Math.Atan(System.Math.Cos(this.Azimuth) / tanAlt);
            return new TiltXY(radX, radY);
        }

        public TiltXY ToXY_Deg()
        {
            var radXY = this.ToRadians().ToXY_Rad();
            var radXY_2 = new TiltXY(radXY.X, -radXY.Y); // Need to revisit why this is done
            return radXY_2.ToDegrees();
        }
    }
}
