// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public struct PaintData
    {
        // properties from the tablet
        public int X;
        public int Y;
        public int Z;
        public uint PressureRaw;
        public int Altitude;
        public int Azimuth;

        // calculated properties
        public double PressureNormalized;
        public double adjusted_pressure;

    }
}
