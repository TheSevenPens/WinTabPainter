﻿using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct PointD
    {
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

        public PointD Divide(double scale)
        {
            return new PointD(this.X/ scale, this.Y/ scale);
        }

        public SD.Point ToSDPointWithRounding()
        {
            double rx = System.Math.Round(this.X);
            double ry = System.Math.Round(this.Y);
            var p = new SD.Point((int)rx, (int)ry);
            return p;
        }

        public string ToSmallString()
        {
            return string.Format("({0}x{1})", this.X, this.Y);
        }
    }
}