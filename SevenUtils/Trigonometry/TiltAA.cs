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
    }
}
