
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

        public SD.Size ToSDSizeWithRounding()
        {
            double rx = System.Math.Round(this.Width);
            double ry = System.Math.Round(this.Height);
            var s = new SD.Size((int)rx, (int)ry);
            return s;
        }
    }
}
