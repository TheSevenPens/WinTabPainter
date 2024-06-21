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
        public int PenX;
        public int PenY;
        public int PenZ;
        public uint PressureRaw;
        public int Altitude;
        public int Azimuth;

        // calculated properties
        public double PressureNormalized;
        public double PressureAdjusted;
        public int BrushWidthAdjusted;

        public SD.Point PenPosition
        {
            get { return new SD.Point(this.PenX, this.PenY); }
        }

        public PaintData(WintabDN.WintabPacket wintab_pkt, TabletInfo tablet_info, PaintSettings paintsettings)
        {
            this.PenX = wintab_pkt.pkX;
            this.PenY = wintab_pkt.pkY;
            this.PenZ = wintab_pkt.pkZ;
            this.PressureRaw = wintab_pkt.pkNormalPressure;
            this.Altitude = wintab_pkt.pkOrientation.orAltitude;
            this.Azimuth = wintab_pkt.pkOrientation.orAzimuth;

            // Calculate normalized pressure so that it is in range [0,1]
            this.PressureNormalized = this.PressureRaw / (double)tablet_info.MaxPressure;

            // Calcualte the normalize pressure with pressurce curve applied

            this.PressureAdjusted = ApplyCurve(this.PressureNormalized, paintsettings.PressureCurveControl);

            if (wintab_pkt.pkNormalPressure > 0)
            {
                this.BrushWidthAdjusted = (int) System.Math.Max(paintsettings.BrushWidthMin, this.PressureAdjusted * paintsettings.BrushWidth);
            }
            else
            {
                this.BrushWidthAdjusted = 0;
            }
        }

        public static double ApplyCurve(double value, double q)
        {
            q = Helpers.ClampRange(q, -1, 1);

            double new_value;

            if (q > 0)
            {
                new_value = Math.Pow(value, 1.0 - q);
            }
            else if (q < 0) 
            { 
                    new_value = Math.Pow(value, 1.0 / (1.0 + q));
            }
            else
            {
                new_value = value;
            }

            return new_value;
        }
    }
}
