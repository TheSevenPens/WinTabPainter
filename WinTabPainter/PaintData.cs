// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

using System;
using System.Diagnostics.Eventing.Reader;
using SD = System.Drawing;

namespace WinTabPainter
{
    public struct PaintData
    {
        // properties from the tablet
        public SD.Point PenPosScreen;
        public int PenZ;
        public uint PressureRaw;
        public int Altitude;
        public int Azimuth;

        // calculated properties
        public double PressureNormalized;
        public double PressureAdjusted;
        public int BrushWidthAdjusted;

        public PaintData(WintabDN.WintabPacket wintab_pkt, TabletInfo tablet_info, PaintSettings paintsettings)
        {
            this.PenPosScreen = new SD.Point(wintab_pkt.pkX, wintab_pkt.pkY);
            this.PenZ = wintab_pkt.pkZ;
            this.PressureRaw = wintab_pkt.pkNormalPressure;
            this.Altitude = wintab_pkt.pkOrientation.orAltitude;
            this.Azimuth = wintab_pkt.pkOrientation.orAzimuth;

            // Calculate normalized pressure so that it is in range [0,1]
            this.PressureNormalized = this.PressureRaw / (double)tablet_info.MaxPressure;

            // Calculate the normalize pressure with pressurce curve applied
            this.PressureAdjusted = Helpers.ApplyCurve(this.PressureNormalized, paintsettings.PressureCurveControl);

            // Calculate the brush width taking into account the pen pressure
            if (wintab_pkt.pkNormalPressure > 0)
            {
                this.BrushWidthAdjusted = (int) System.Math.Max(paintsettings.BrushWidthMin, this.PressureAdjusted * paintsettings.BrushWidth);
            }
            else
            {
                this.BrushWidthAdjusted = 0;
            }
        }


    }
}
