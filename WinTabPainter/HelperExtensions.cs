using System;
using WinTabUtils.Geometry;
using SD = System.Drawing;

namespace WinTabPainter;

public static class HelperExtensions
{
    public static void AppendFormatLine(this System.Text.StringBuilder sb, string format, params object[] args)
    {
        var s = string.Format(format, args);
        sb.AppendLine(s);
    }
    public static WinTabUtils.Geometry.Point ToPoint(this SD.Point p)
    {
        var np = new WinTabUtils.Geometry.Point(p.X, p.Y);
        return np;
    }
    public static string ToStringXY(this SD.Point p)
    {
        return string.Format("({0}x{1})", p.X, p.Y);
    }

    public static WinTabUtils.Geometry.PointD Smooth(this WinTabUtils.Geometry.PointD p, WinTabUtils.Numerics.EMAPositionSmoother smoother)
    {
        return smoother.GetNextSmoothed(p);
    }

    public static double Smooth(this double v, WinTabUtils.Numerics.EMASmoother smoother)
    {
        return smoother.GetNextSmoothed(v);
    }
}
