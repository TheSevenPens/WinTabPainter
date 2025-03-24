using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTabUtils.Trigonometry
{
    public static class Angles
    {
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
