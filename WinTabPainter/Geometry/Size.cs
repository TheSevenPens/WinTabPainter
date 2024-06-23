﻿
using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct Size
    {
        public int Width;
        public int Height;

        public Size(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }

        public SizeD Divide(double scale)
        {
            return new SizeD(this.Width / scale, this.Height / scale);
        }


        public static implicit operator SD.Size(Size s) => s.ToSDSize();

        public SD.Size ToSDSize()
        {
            var s = new SD.Size(this.Width, this.Height);
            return s;
        }
    }

}
