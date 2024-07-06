using System;
using SD = System.Drawing;

namespace WinTabPainter.Geometry;

public readonly struct PointD
{
    public readonly double X;
    public readonly double Y;

    public static PointD Empty => new PointD(0, 0);

    public PointD(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }
    public PointD Add(double dx, double dy) => new PointD(this.X + dx, this.Y + dy);

    public PointD Subtract(Point p) => new PointD(this.X + p.X, this.Y + p.Y);
    public PointD Subtract(Geometry.PointD p) => new PointD(this.X - p.X, this.Y - p.Y);

    public PointD Subtract(Geometry.SizeD s) => new PointD(this.X - s.Width, this.Y - s.Height);

    public PointD Divide(double scale) => new PointD(this.X / scale, this.Y / scale);

    public PointD Multiply (double scale) => new PointD(this.X * scale, this.Y * scale);

    public Geometry.PointD Round()
    {
        double rx = System.Math.Round(this.X);
        double ry = System.Math.Round(this.Y);
        var p = new Geometry.PointD(rx, ry);
        return p;
    }

    public Geometry.Point ToPoint()
    {
        var p = new Geometry.Point((int)this.X, (int)this.Y);
        return p;
    }

    public SD.PointF ToSDPointF()
    {
        var p = new SD.PointF((float)this.X, (float)this.Y);
        return p;
    }

    public string ToStringXY() => string.Format("({0}x{1})", this.X, this.Y);
    public static implicit operator SD.PointF(PointD p) => p.ToSDPointF();
    public static bool operator ==(PointD lhs, PointD rhs) => lhs.Equals(rhs);

    public static bool operator !=(PointD lhs, PointD rhs) => !(lhs == rhs);

    public double DistanceTo(PointD p)
    {
        var dx = p.X - this.X;
        var dy = p.Y - this.Y;
        return Math.Sqrt((dx * dx) + (dy * dy));
    }
}