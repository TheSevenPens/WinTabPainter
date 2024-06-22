
using SD = System.Drawing;

namespace WinTabPainter.Geometry
{
    public struct SizeI
    {
        public int Width;
        public int Height;

        public SizeI(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }

        public SizeD Divide(double scale)
        {
            return new SizeD(this.Width / scale, this.Height / scale);
        }

        public SD.Size ToSDSize()
        {
            var s = new SD.Size(this.Width, this.Height);
            return s;
        }
    }

}
