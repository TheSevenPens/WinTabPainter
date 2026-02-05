using System;
using SD = System.Drawing;

namespace WinTabUtils.Geometry;

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

}
