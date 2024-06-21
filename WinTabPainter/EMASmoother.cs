// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public class EMASmoother
    {
        public double Alpha;
        private PointD? SmoothingOld;

        public EMASmoother(double alpha)
        {
            this.Alpha = alpha;
            this.SmoothingOld = null;
        }

        public void SetOldSmoothed(PointD p)
        {
            this.SmoothingOld = p;
        }

        public PointD Smooth(PointD value)
        {
            PointD smoothed_new;
            if (this.SmoothingOld.HasValue)
            {
                smoothed_new = EMASmoother.lerp(this.SmoothingOld.Value, value, this.Alpha);
            }
            else
            {
                smoothed_new = value;
            }

            this.SmoothingOld = smoothed_new;
            return smoothed_new;
        }

        private static PointD lerp(PointD oldval, PointD newval, double alpha)
        {
            double newx = (alpha * oldval.X) + ((1 - alpha) * newval.X);
            double newy = (alpha * oldval.Y) + ((1 - alpha) * newval.Y);
            var p = new PointD(newx, newy);
            return p;
        }
    }
}
