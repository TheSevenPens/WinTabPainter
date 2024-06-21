using System;
using SD = System.Drawing;

// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public static class UtilExtensions
    {
        public static SD.Point Divide( this SD.Point p, double scale)
        {
            var np = new SD.Point((int)(p.X / scale), (int)(p.Y / scale));
            return np;
        }

        public static SD.Size Divide(this SD.Size s, double scale)
        {
            var ns = new SD.Size((int)(s.Width / scale), (int)(s.Height / scale));
            return ns;
        }


        public static SD.Point Subtract(this SD.Point p1, SD.Point p2)
        {
            var np = new SD.Point(p1.X-p2.X, p1.Y - p2.Y);
            return np;
        }

        public static SD.Point Subtract(this SD.Point p1, int x, int y)
        {
            var np = new SD.Point(p1.X - x, p1.Y - y);
            return np;
        }

        public static SD.Point Subtract(this SD.Point p, SD.Size s)
        {
            var np = new SD.Point(p.X - s.Width, p.Y - s.Height);
            return np;
        }

        public static PointD ToPointD(this SD.Point p)
        {
            var np = new PointD(p.X, p.Y);
            return np;
        }


    }
}
