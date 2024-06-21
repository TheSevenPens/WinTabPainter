// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public class PaintSettings
    {
        public double pressure_curve_q = 0.0;
        public int brush_width = 5;
        public readonly double smoothing_min = 0.0;
        public readonly double smoothing_max = 0.99;
        public double smoothing = 0.0;
        public EMASmoother smoother;
    }
}
