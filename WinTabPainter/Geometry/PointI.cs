using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct PointI
    {
        public int X;
        public int Y;

        public static PointI Empty
        {
            get
            {
                return new PointI(0, 0);
            }
        }

        public PointI(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public PointI Add(int dx, int dy)
        {
            return new PointI(this.X + dx, this.Y + dy);
        }
        public PointI Subtract(int dx, int dy)
        {
            return new PointI(this.X - dx, this.Y - dy);
        }

        public PointI Subtract(Geometry.SizeI s)
        {
            return new PointI(this.X - s.Width, this.Y - s.Height);
        }

        public PointI Subtract(Geometry.PointI p)
        {
            return new PointI(this.X - p.X, this.Y - p.Y);
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

        public string ToSmallString()
        {
            return string.Format("({0}x{1})", this.X, this.Y);
        }
    }

}