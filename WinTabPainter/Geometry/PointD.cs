using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct PointD
    {
        public double X;
        public double Y;

        public static PointD Empty => new PointD(0, 0);

        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public PointD Add(double dx, double dy) => new PointD(this.X + dx, this.Y + dy);

        public PointD Subtract(Point p) => new PointD(this.X + p.X, this.Y + p.Y);

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

        public string ToStringXY() => string.Format("({0}x{1})", this.X, this.Y);
    }

}