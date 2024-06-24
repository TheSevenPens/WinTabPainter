
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

        public SizeD Divide(double scale) => new SizeD(this.Width / scale, this.Height / scale);

        public Geometry.SizeD Round()
        {
            double w = System.Math.Round(this.Width);
            double h = System.Math.Round(this.Height);
            var s = new Geometry.SizeD(w, h);
            return s;
        }

        public Geometry.Size ToSize()
        {
            var s = new Geometry.Size((int)this.Width, (int)this.Height);
            return s;
        }

    }

}
