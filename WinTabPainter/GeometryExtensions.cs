using System;
using SD = System.Drawing;


namespace WinTabPainter.GeometryExtensions;

public static class GeometryExtensions
{

    extension(WinTabUtils.Geometry.Point this_p)
    {

        public SD.Point ToSDPoint()
        {
            var p = new SD.Point(this_p.X, this_p.Y);
            return p;
        }

    }

    extension(WinTabUtils.Geometry.PointD this_p)
    {

        public SD.PointF ToSDPointF()
        {
            var p = new SD.PointF((float)this_p.X, (float)this_p.Y);
            return p;
        }

    }

    extension(WinTabUtils.Geometry.SizeD this_s)
    {

        public SD.SizeF ToSDSizeF()
        {
            var s = new SD.SizeF((float)this_s.Width, (float)this_s.Height);
            return s;
        }

    }

    extension(WinTabUtils.Geometry.Size this_s)
    {

        public SD.Size ToSDSize()
        {
            var s = new SD.Size(this_s.Width, this_s.Height);
            return s;
        }

    }

}
