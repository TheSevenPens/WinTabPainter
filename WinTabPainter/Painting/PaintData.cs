using System;
using System.Diagnostics.Eventing.Reader;
using SD = System.Drawing;

namespace WinTabPainter.Painting
{
    public struct PaintData
    {
        // TIME
        public uint Time;

        // POSITION
        public Geometry.Point PenPos;
        public Geometry.Point PenPosSmoothed;

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

            // TIME
            this.Time = wintab_pkt.pkTime;

            // POSITION
            this.PenPos = new Geometry.Point(wintab_pkt.pkX, wintab_pkt.pkY);
            this.PenPosSmoothed = paintsettings.PositionSmoother.Smooth(this.PenPos).Round().ToPoint();

            // HOVER
            this.PenHover = wintab_pkt.pkZ;

            // TILT
            this.TiltAltitude = wintab_pkt.pkOrientation.orAltitude / 10.0;
            this.TiltAzimuth = wintab_pkt.pkOrientation.orAzimuth / 10.0;

            // PRESSURE
            this.PressureRaw = wintab_pkt.pkNormalPressure;
            this.PressureNormalized = this.PressureRaw / (double) tablet_info.MaxPressure;

            bool smooth_before_curve = false;

            if (smooth_before_curve)
            {
                this.PressureSmoothed = paintsettings.PressureSmoother.Smooth(this.PressureNormalized);
                this.PressureCurved = paintsettings.pressure_curve.ApplyCurve(this.PressureSmoothed);
                this.PressureEffective = this.PressureCurved;
            }
            else
            {
                this.PressureCurved = paintsettings.pressure_curve.ApplyCurve(this.PressureNormalized);
                this.PressureSmoothed = paintsettings.PressureSmoother.Smooth(this.PressureCurved);
                this.PressureEffective = this.PressureSmoothed;
            }

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
