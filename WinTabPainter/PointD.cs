// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

using SD=System.Drawing;

namespace WinTabPainter
{
    public struct PointD
    {
        // simple point struct with x,y of datatype double

        public double X;
        public double Y;

        public static PointD Empty
        {
            get
            {
                return new PointD(0, 0);
            }
        }

        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public PointD(SD.Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        public SD.Point ToPoint()
        {
            double rx = System.Math.Round(this.X);
            double ry = System.Math.Round(this.Y);
            var p = new SD.Point((int)rx, (int)ry);
            return p;
        }

        public PointD Add(int dx, int dy)
        {
            return new PointD(this.X + dx, this.Y + dy);
        }
    }
}
