
namespace SevenPaint.GeometryExtensions;

public static class GeometryExtensions
{

    extension(System.Windows.Point this_p)
    {

        public SevenLib.Geometry.PointD ToSPPointD()
        {
            var p = new SevenLib.Geometry.PointD(this_p.X, this_p.Y);
            return p;
        }

    }

    extension(System.Windows.Size this_s)
    {

        public SevenLib.Geometry.SizeD ToSPSizeD()
        {
            var p = new SevenLib.Geometry.SizeD(this_s.Width, this_s.Height);
            return p;
        }

    }

    extension(SevenLib.Geometry.Point this_p)
    {

        public System.Windows.Point ToSW()
        {
            var p = new System.Windows.Point(this_p.X, this_p.Y);
            return p;
        }

    }

    extension(SevenLib.Geometry.Size this_s)
    {

        public System.Windows.Size ToSWSize()
        {
            var s = new System.Windows.Size(this_s.Width, this_s.Height);
            return s;
        }

    }

}
