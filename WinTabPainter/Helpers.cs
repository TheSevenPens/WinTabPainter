﻿// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public static class Helpers
    {
        public static double ClampRange(double q, double min, double max)
        {
            if (q < min) { q = min; }
            else if (q > max) { q = max; }
            else { /* dnothing */}
            return q;
        }
    }
}
