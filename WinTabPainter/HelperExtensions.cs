using System;
using SevenUtils.Geometry;
using SD = System.Drawing;

namespace WinTabPainter;

public static class HelperExtensions
{
    public static void AppendFormatLine(this System.Text.StringBuilder sb, string format, params object[] args)
    {
        var s = string.Format(format, args);
        sb.AppendLine(s);
    }
    public static SevenUtils.Geometry.Point ToPoint(this SD.Point p)
    {
        var np = new SevenUtils.Geometry.Point(p.X, p.Y);
        return np;
    }
    public static string ToStringXY(this SD.Point p)
    {
        return string.Format("({0}x{1})", p.X, p.Y);
    }

    public static SevenUtils.Geometry.PointD Smooth(this SevenUtils.Geometry.PointD p, SevenUtils.Numerics.EMAPositionSmoother smoother)
    {
        return smoother.GetNextSmoothed(p);
    }

    public static double Smooth(this double v, SevenUtils.Numerics.EMASmoother smoother)
    {
        return smoother.GetNextSmoothed(v);
    }
}
