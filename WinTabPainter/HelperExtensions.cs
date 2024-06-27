using System;
using WinTabPainter.Geometry;
using SD = System.Drawing;

namespace WinTabPainter
{
    public static class HelperExtensions
    {

        public static Geometry.Point ToPoint(this SD.Point p)
        {
            var np = new Geometry.Point(p.X, p.Y);
            return np;
        }
        public static string ToStringXY(this SD.Point p)
        {
            return string.Format("({0}x{1})", p.X, p.Y);
        }

        public static PointD Smooth(this PointD p, Numerics.EMAPositionSmoother smoother)
        {
            return smoother.GetNextSmoothed(p);
        }

        public static double Smooth(this double v, Numerics.EMASmoother smoother)
        {
            return smoother.GetNextSmoothed(v);
        }
    }
}
