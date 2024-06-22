using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct PointD
    {
        // simple point struct with x,y of datatype double

        public double X;
        public double Y;

        public static PointD Empty
        {
            get
            {
                return new PointD(0, 0);
            }
        }

        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public PointD Add(int dx, int dy)
        {
            return new PointD(this.X + dx, this.Y + dy);
        }

        public SD.Point ToSDPointWithRounding()
        {
            double rx = System.Math.Round(this.X);
            double ry = System.Math.Round(this.Y);
            var p = new SD.Point((int)rx, (int)ry);
            return p;
        }
    }
}
