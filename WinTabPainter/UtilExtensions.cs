using System.Drawing;


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
        public static Point Divide( this Point p, double scale)
        {
            var np = new Point((int)(p.X / scale), (int)(p.Y / scale));
            return np;
        }

        public static Point Subtract(this Point p1, Point p2)
        {
            var np = new Point(p1.X-p2.X, p1.Y - p2.Y);
            return np;
        }

        public static Point Subtract(this Point p1, int x, int y)
        {
            var np = new Point(p1.X - x, p1.Y - y);
            return np;
        }

    }
}
