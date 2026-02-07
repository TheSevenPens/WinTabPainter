namespace SevenUtils.Trigonometry
{
    public readonly struct TiltXY
    {
        public readonly double X;
        public readonly double Y;

        public TiltXY(double x, double y)
        {
            X = x;
            Y = y;
        }

        public TiltXY ToDegrees()
        {
            return new TiltXY(Angles.RadiansToDegrees(X), Angles.RadiansToDegrees(Y));
        }

        public TiltXY ToRadians()
        {
            return new TiltXY(Angles.DegreesToRadians(X), Angles.DegreesToRadians(Y));
        }

        public TiltAA ToAA_deg()
        {
            double azimuth_deg = 0;
            double altitude_deg = 90;

            if (this.X != 0.0 || this.Y != 0.0)
            {
                var tiltxy_rad = this.ToRadians();

                double tanX = Math.Tan(tiltxy_rad.X);
                double tanY = Math.Tan(tiltxy_rad.Y);

                double azRad = Math.Atan2(tanY, tanX);
                double denom = Math.Sqrt(tanX * tanX + tanY * tanY);
                double altRad = 0;
                if (denom > 0.001)
                {
                    altRad = Math.Atan(1.0 / denom);
                }
 ;

                azimuth_deg = SevenUtils.Trigonometry.Angles.RadiansToDegrees(azRad);
                if (azimuth_deg < 0)
                {
                    azimuth_deg += 360.0;
                }

                altitude_deg = SevenUtils.Trigonometry.Angles.RadiansToDegrees(altRad);
            }

            return new TiltAA(azimuth_deg, altitude_deg);
        }
    }
}
