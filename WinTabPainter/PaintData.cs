using System;
using System.Diagnostics.Eventing.Reader;
using SD = System.Drawing;

namespace WinTabPainter
{
    public struct PaintData
    {
        // properties from the tablet
        public SD.Point PenPos;
        public SD.Point PenPosSmoothed;
        public int PenHover;
        public uint PressureRaw;
        public double TiltAltitude;
        public double TiltAzimuth;

        // calculated properties
        public double PressureNormalized;
        public double PressureSmoothed;
        public double PressureCurved;
        public double PressureEffective;
        public int BrushWidthAdjusted;

        public PaintData(WintabDN.WintabPacket wintab_pkt, TabletInfo tablet_info, PaintSettings paintsettings)
        {
            this.PenPos = new SD.Point(wintab_pkt.pkX, wintab_pkt.pkY);
            this.PenHover = wintab_pkt.pkZ;
            this.PressureRaw = wintab_pkt.pkNormalPressure;
            this.TiltAltitude = wintab_pkt.pkOrientation.orAltitude / 10.0;
            this.TiltAzimuth = wintab_pkt.pkOrientation.orAzimuth / 10.0;

            // Process te pressure
            // STEP 1 - normalized pressure so that it is in range [0,1]
            // STEP 2 - Smooth it
            // STEP 3 - Apply the pressure curve
            this.PressureNormalized = this.PressureRaw / (double)tablet_info.MaxPressure;
            this.PressureSmoothed = paintsettings.PressureSmoother.Smooth(this.PressureNormalized);
            this.PressureCurved = paintsettings.pressure_curve.ApplyCurve(this.PressureSmoothed);
            this.PressureEffective = this.PressureCurved;

            this.PenPosSmoothed = paintsettings.PositionSmoother.Smooth(this.PenPos.ToPointD()).ToPointWithRounding().ToSDPoint();

            // Calculate the brush width taking into account the pen pressure
            if (this.PressureRaw > 0)
            {
                this.BrushWidthAdjusted = (int) System.Math.Max(paintsettings.BrushWidthMin, this.PressureEffective * paintsettings.BrushWidth);
            }
            else
            {
                this.BrushWidthAdjusted = 0;
            }
        }

        public double DegreesToRadians(double degrees)
        {
            return degrees * (System.Math.PI / 180.0);
        }

    }
}
