using System;
using SD = System.Drawing;

namespace WinTabPainter
{
    public static class HelperExtensions
    {
        public static Geometry.PointD Divide( this SD.Point p, double scale)
        {
            var p0 = p.ToPointD();
            var p1 = p0.Divide(scale);
            return p1;
        }

        public static Geometry.SizeD Divide(this SD.Size s, double scale)
        {
            var s0 = new Geometry.SizeD(s.Width, s.Height);
            var s1 = s0.Divide(scale);
            return s1;
        }

        public static SD.Point Subtract(this SD.Point p1, SD.Point p2)
        {
            var np = new SD.Point(p1.X-p2.X, p1.Y - p2.Y);
            return np;
        }

        public static SD.Point Subtract(this SD.Point p1, int x, int y)
        {
            var np = new SD.Point(p1.X - x, p1.Y - y);
            return np;
        }

        public static SD.Point Subtract(this SD.Point p, SD.Size s)
        {
            var np = new SD.Point(p.X - s.Width, p.Y - s.Height);
            return np;
        }

        public static Geometry.PointD ToPointD(this SD.Point p)
        {
            var np = new Geometry.PointD(p.X, p.Y);
            return np;
        }

        public static Geometry.Point ToPoint(this SD.Point p)
        {
            var np = new Geometry.Point(p.X, p.Y);
            return np;
        }
        public static string ToStringXY(this SD.Point p)
        {
            return string.Format("({0}x{1})", p.X, p.Y);
        }
    }
}
