using System;
using System.Diagnostics.Eventing.Reader;
using SD = System.Drawing;

namespace WinTabPainter
{
    public struct PaintData
    {
        // POSITION
        public SD.Point PenPos;
        public SD.Point PenPosSmoothed;

        // HOVER
        public int PenHover;

        //TILT
        public double TiltAltitude;
        public double TiltAzimuth;


        // PRESSURE
        public uint PressureRaw;
        public double PressureNormalized;
        public double PressureSmoothed;
        public double PressureCurved;
        public double PressureEffective;

        // BRUSH
        public int BrushWidthAdjusted;

        public PaintData(WintabDN.WintabPacket wintab_pkt, TabletInfo tablet_info, PaintSettings paintsettings)
        {
            // POSITION
            this.PenPos = new SD.Point(wintab_pkt.pkX, wintab_pkt.pkY);
            this.PenPosSmoothed = paintsettings.PositionSmoother.Smooth(this.PenPos.ToPointD()).ToPointWithRounding().ToSDPoint();

            // HOVER
            this.PenHover = wintab_pkt.pkZ;

            // TILT
            this.TiltAltitude = wintab_pkt.pkOrientation.orAltitude / 10.0;
            this.TiltAzimuth = wintab_pkt.pkOrientation.orAzimuth / 10.0;

            // PRESSURE
            // STEP 1 - normalized pressure so that it is in range [0,1]
            // STEP 2 - Smooth it
            // STEP 3 - Apply the pressure curve
            this.PressureRaw = wintab_pkt.pkNormalPressure;
            this.PressureNormalized = this.PressureRaw / (double)tablet_info.MaxPressure;
            this.PressureSmoothed = paintsettings.PressureSmoother.Smooth(this.PressureNormalized);
            this.PressureCurved = paintsettings.pressure_curve.ApplyCurve(this.PressureSmoothed);
            this.PressureEffective = this.PressureCurved;

            // BRUSH SIZE
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
    }
}
