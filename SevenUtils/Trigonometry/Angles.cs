namespace SevenUtils.Trigonometry
{
    public static class Angles
    {
        public static (double Azimuth, double Altitude) TiltXYToTiltAzAl(double tiltX_deg, double tiltY_deg)
        {
            double azimuth = 0;
            double altitude = 90;

            if (tiltX_deg != 0.0 || tiltY_deg != 0.0)
            {
                double txRad = DegreesToRadians( tiltX_deg );
                double tyRad = DegreesToRadians( tiltY_deg );
                double tanX = Math.Tan(txRad);
                double tanY = Math.Tan(tyRad);

                double azRad = Math.Atan2(tanY, tanX);
                azimuth = RadiansToDegrees( azRad );
                if (azimuth < 0)
                {
                    azimuth += 360.0;
                }

                double denom = Math.Sqrt(tanX * tanX + tanY * tanY);
                if (denom > 0.001)
                {
                    double altRad = Math.Atan(1.0 / denom);
                    altitude = RadiansToDegrees(altRad);
                }
            }

            return (azimuth, altitude);
        }
        public static (double TiltX, double TiltY) AzimuthAndAltudeToTilt_Rad(double azimuth_radians, double altitude_radians)
        {
            double tanAlt = System.Math.Tan(System.Math.Abs(altitude_radians));
            double radX = System.Math.Atan(System.Math.Sin(azimuth_radians) / tanAlt);
            double radY = System.Math.Atan(System.Math.Cos(azimuth_radians) / tanAlt);
            return (radX, radY);
        }

        public static (double TiltX, double TiltY) AzimuthAndAltudeToTiltDeg(double azimuth_deg, double altitude_deg)
        {
            double azimuth_radians = DegreesToRadians(azimuth_deg);
            double altitude_radians = DegreesToRadians(altitude_deg);
            (double radX, double radY) = Angles.AzimuthAndAltudeToTilt_Rad(azimuth_radians, altitude_radians);
            return (Angles.RadiansToDegrees(radX), Angles.RadiansToDegrees(-radY));
        }

        public static double DegreesToRadians(double degrees)
        {
            var radians = degrees * Math.PI / 180;
            return radians;
        }

        public static double RadiansToDegrees(double radians)
        {
            var degrees = radians * 180 / Math.PI;
            return degrees;
        }
    }
}
