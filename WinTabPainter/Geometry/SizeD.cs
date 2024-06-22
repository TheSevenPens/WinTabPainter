﻿
using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct SizeD
    {
        public double Width;
        public double Height;

        public SizeD(double w, double h)
        {
            this.Width = w;
            this.Height = h;
        }

        public SizeD Divide(double scale)
        {
            return new SizeD( this.Width/ scale, this.Height / scale );
        }

        public SD.Size ToSDSizeWithRounding()
        {
            double w = System.Math.Round(this.Width);
            double h = System.Math.Round(this.Height);
            var s = new SD.Size((int)w, (int)h);
            return s;
        }
    }

}
