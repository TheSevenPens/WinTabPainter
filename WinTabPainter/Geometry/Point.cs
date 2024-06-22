using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct Point
    {
        public int X;
        public int Y;

        public static Point Empty
        {
            get
            {
                return new Point(0, 0);
            }
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Point Add(int dx, int dy)
        {
            return new Point(this.X + dx, this.Y + dy);
        }
        public Point Subtract(int dx, int dy)
        {
            return new Point(this.X - dx, this.Y - dy);
        }

        public Point Subtract(Geometry.Size s)
        {
            return new Point(this.X - s.Width, this.Y - s.Height);
        }

        public Point Subtract(Geometry.Point p)
        {
            return new Point(this.X - p.X, this.Y - p.Y);
        }

        public PointD Divide(double scale)
        {
            return new PointD(this.X / scale, this.Y / scale);
        }

        public SD.Point ToSDPoint()
        {
            var p = new SD.Point(this.X, this.Y);
            return p;
        }

        public string ToStringXY()
        {
            return string.Format("({0}x{1})", this.X, this.Y);
        }
    }

}