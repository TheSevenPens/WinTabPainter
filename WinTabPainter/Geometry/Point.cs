using SD = System.Drawing;

namespace WinTabPainter.Geometry;

public struct Point
{
    public readonly int X;
    public readonly int Y;

    public static Point Empty => new Point(0, 0);

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    public Point Add(int dx, int dy) => new Point(this.X + dx, this.Y + dy);
    public Point Subtract(int dx, int dy) => new Point(this.X - dx, this.Y - dy);

    public Point Subtract(Geometry.Size s) => new Point(this.X - s.Width, this.Y - s.Height);

    public Point Subtract(Geometry.Point p) => new Point(this.X - p.X, this.Y - p.Y);

    public PointD Divide(double scale) => new PointD(this.X / scale, this.Y / scale);

    public string ToStringXY() => string.Format("{0}x{1}", this.X, this.Y);

    public static implicit operator SD.Point(Point s) => s.ToSDPoint();

    public SD.Point ToSDPoint()
    {
        var p = new SD.Point(this.X, this.Y);
        return p;
    }

}

