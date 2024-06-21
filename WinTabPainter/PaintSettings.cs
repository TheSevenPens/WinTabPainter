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

        // brush settings
        public int brush_width = 5;
        public int brush_width_min = 1;

        // pressure settings

        public double pressure_curve_control = 0.0;

        // smoothing settings
        public double smoothing_amount = 0.0;
        public readonly double SMOOTHING_MIN = 0.0;
        public readonly double SMOOTHING_MAX = 0.99;
        public EMASmoother smoother;
    }
}
